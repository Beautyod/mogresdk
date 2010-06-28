namespace DSMogre
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Threading;

    using DirectShowLib;

    public abstract class Capture : IDisposable
    {
        #region Fields

        private ManualResetEvent resetEvent;
        private int stride;
        private Thread thread;
        private int videoHeight;
        private int videoWidth;

        #endregion Fields

        #region Constructors

        static Capture()
        {
            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            CaptureDeviceNames = new Collection<string>();
            foreach (DsDevice dd in capDevices)
            {
                CaptureDeviceNames.Add(dd.Name);
            }
        }

        ~Capture()
        {
            this.Dispose(false);
        }

        #endregion Constructors

        #region Events

        public event EventHandler NewBitmap;

        #endregion Events

        #region Enumerations

        public enum GraphState
        {
            Stopped,
            Paused,
            Running,
            Exiting
        }

        #endregion Enumerations

        #region Properties

        #region Public Static Properties

        public static Collection<string> CaptureDeviceNames
        {
            get;
            private set;
        }

        #endregion Public Static Properties

        #region Public Properties

        public int FrameRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get
            {
                return this.State == GraphState.Running;
            }
        }

        public bool Looping
        {
            get;
            set;
        }

        public Size Size
        {
            get
            {
                return new Size(this.videoWidth, this.videoHeight);
            }
        }

        public int Stride
        {
            get
            {
                return this.stride;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        protected IFilterGraph2 FilterGraph
        {
            get;
            set;
        }

        protected bool HasBeenSetup
        {
            get;
            set;
        }

        protected IMediaControl MediaCtrl
        {
            get;
            set;
        }

        protected IMediaEvent MediaEvent
        {
            get;
            set;
        }

        protected IMediaPosition MediaPosition
        {
            get;
            set;
        }

        protected ISampleGrabber SampGrabber
        {
            get;
            set;
        }

        protected GraphState State
        {
            get;
            set;
        }

        #endregion Protected Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public void Dispose()
        {
            this.Dispose(true);
        }

        public virtual void Pause()
        {
            if (this.State == GraphState.Running)
            {
                int hr = this.MediaCtrl.Pause();
                DsError.ThrowExceptionForHR(hr);

                this.State = GraphState.Paused;
            }
        }

        public virtual void Start()
        {
            if (this.thread == null)
            {
                this.resetEvent = new ManualResetEvent(false);

                // create and start new thread
                this.thread = new Thread(this.CaptureThread)
                {
                    Name = "DirectShow " + this.GetType().Name + " Thread"
                };
                this.thread.Start();
            }

            if (this.State == GraphState.Paused)
            {
                int hr = this.MediaCtrl.Run();
                DsError.ThrowExceptionForHR(hr);

                this.State = GraphState.Running;
            }
        }

        public virtual void Stop()
        {
            if (this.thread != null)
            {
                // signal to reset
                this.resetEvent.Set();
                this.thread.Join();

                this.thread = null;
                this.resetEvent.Close();
                this.resetEvent = null;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void CloseInterfaces()
        {
            this.State = GraphState.Exiting;
            this.MediaCtrl = null;

            if (this.SampGrabber != null)
            {
                Marshal.ReleaseComObject(this.SampGrabber);
                this.SampGrabber = null;
            }

            if (this.FilterGraph != null)
            {
                Marshal.ReleaseComObject(this.FilterGraph);
                this.FilterGraph = null;
            }

            this.HasBeenSetup = false;
        }

        protected void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            // Set the media type to Video/RBG24
            AMMediaType media = new AMMediaType
            {
                majorType = MediaType.Video,
                subType = MediaSubType.RGB24,
                formatType = FormatType.VideoInfo
            };
            int hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);

            // Configure the samplegrabber
            hr = sampGrabber.SetCallback(new GrabberCB(this), 1);
            DsError.ThrowExceptionForHR(hr);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Stop();
        }

        protected void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();
            int hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            this.videoWidth = videoInfoHeader.BmiHeader.Width;
            this.videoHeight = videoInfoHeader.BmiHeader.Height;
            this.stride = this.videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
        }

        protected abstract void SetupGraph();

        #endregion Protected Methods

        #region Private Methods

        private void CaptureThread()
        {
            if (!this.HasBeenSetup)
            {
                this.SetupGraph();
            }

            int hr = this.MediaCtrl.Run();
            DsError.ThrowExceptionForHR(hr);

            this.State = GraphState.Running;

            // run
            while (!this.resetEvent.WaitOne(0, true))
            {
                Thread.Sleep(100);

                if (this.MediaEvent != null && this.State != GraphState.Exiting)
                {
                    IntPtr p1, p2;
                    EventCode eventCode;
                    if (this.MediaEvent.GetEvent(out eventCode, out p1, out p2, 0) >= 0)
                    {
                        this.MediaEvent.FreeEventParams(eventCode, p1, p2);

                        if (eventCode == EventCode.Complete)
                        {
                            if (this.Looping && this.MediaPosition != null)
                            {
                                this.MediaPosition.put_CurrentPosition(0);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            this.CloseInterfaces();
            this.State = GraphState.Stopped;
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Types

        private class GrabberCB : ISampleGrabberCB
        {
            #region Fields

            private readonly Capture owner;

            private IntPtr handle;

            #endregion Fields

            #region Constructors

            public GrabberCB(Capture owner)
            {
                this.owner = owner;
            }

            #endregion Constructors

            #region Methods

            #region Explicit Interface Methods

            int ISampleGrabberCB.BufferCB(double sampleTime, IntPtr buffer, int bufferLen)
            {
                if (this.owner.NewBitmap != null)
                {
                    this.handle = Marshal.AllocCoTaskMem(this.owner.stride * this.owner.videoHeight);

                    // Copy the frame to the buffer
                    NativeMethods.CopyMemory(this.handle, buffer, this.owner.stride * this.owner.videoHeight);

                    // create new image
                    // We know the Bits Per Pixel is 24 (3 bytes) because we forced it
                    // to be with sampGrabber.SetMediaType()
                    int bufSize = this.owner.videoWidth * this.owner.videoHeight * 3;
                    Bitmap result;

                    using (Bitmap temp = new Bitmap(
                        this.owner.videoWidth,
                        this.owner.videoHeight,
                        -this.owner.stride,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                        (IntPtr)(this.handle.ToInt32() + bufSize - this.owner.stride)))
                    {
                        int width = NextPowerOfTwo(this.owner.videoWidth);
                        int height = NextPowerOfTwo(this.owner.videoHeight);

                        if (width == this.owner.videoWidth && height == this.owner.videoHeight)
                        {
                            result = (Bitmap)temp.Clone();
                        }
                        else
                        {
                            result = new Bitmap(width, height);
                            using (Graphics g = Graphics.FromImage(result))
                            {
                                g.InterpolationMode = InterpolationMode.Low;
                                g.DrawImage(temp, 0, 0, width, height);
                            }
                        }

                        Marshal.FreeCoTaskMem(this.handle);
                    }

                    // notify parent
                    this.owner.NewBitmap(result, null);
                }

                return 0;
            }

            int ISampleGrabberCB.SampleCB(double sampleTime, IMediaSample sample)
            {
                return 0;
            }

            #endregion Explicit Interface Methods

            #region Private Static Methods

            private static int NextPowerOfTwo(int val)
            {
                double vald = val;
                return (int)System.Math.Pow(2, System.Math.Ceiling(System.Math.Log(vald, 2)));
            }

            #endregion Private Static Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}