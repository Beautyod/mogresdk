namespace DSMogre
{
    using System;
    using System.Runtime.InteropServices;

    using DirectShowLib;

    public sealed class WebcamCapture : Capture
    {
        #region Fields

        private readonly int deviceNum;

        #endregion Fields

        #region Constructors

        public WebcamCapture()
            : this(0)
        {
        }

        public WebcamCapture(int deviceNum)
        {
            this.deviceNum = deviceNum;
        }

        public WebcamCapture(string deviceName)
        {
            if (Capture.CaptureDeviceNames.Contains(deviceName))
            {
                this.deviceNum = Capture.CaptureDeviceNames.IndexOf(deviceName);
            }
            else
            {
                throw new ArgumentException("Not a valid device name", "deviceName");
            }
        }

        #endregion Constructors

        #region Methods

        #region Protected Methods

        protected override void SetupGraph()
        {
            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (this.deviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            DsDevice dev = capDevices[this.deviceNum];

            this.HasBeenSetup = true;
            IBaseFilter capFilter = null;
            ICaptureGraphBuilder2 capGraph = null;

            // Get the graphbuilder object
            this.FilterGraph = (IFilterGraph2)new FilterGraph();
            this.MediaCtrl = this.FilterGraph as IMediaControl;

            try
            {
                // Get the ICaptureGraphBuilder2
                capGraph = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();

                // Get the SampleGrabber interface
                this.SampGrabber = (ISampleGrabber) new SampleGrabber();

                // Start building the graph
                int hr = capGraph.SetFiltergraph(this.FilterGraph);
                DsError.ThrowExceptionForHR(hr);

                // Add the video device
                hr = this.FilterGraph.AddSourceFilterForMoniker(dev.Mon, null, "Video input", out capFilter);
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter baseGrabFlt = (IBaseFilter)this.SampGrabber;
                this.ConfigureSampleGrabber(this.SampGrabber);

                // Add the frame grabber to the graph
                hr = this.FilterGraph.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);

                this.MediaEvent = this.FilterGraph as IMediaEvent;

                this.SampGrabber.SetBufferSamples(true);
                this.SampGrabber.SetOneShot(false);

                // If any of the default config items are set
                if (this.FrameRate + this.Size.Height + this.Size.Width > 0)
                {
                    this.SetConfigParms(capGraph, capFilter);
                }

                hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Video, capFilter, null, baseGrabFlt);
                DsError.ThrowExceptionForHR(hr);

                this.SaveSizeInfo(this.SampGrabber);
            }
            finally
            {
                if (capFilter != null)
                {
                    Marshal.ReleaseComObject(capFilter);
                }

                if (this.SampGrabber != null)
                {
                    Marshal.ReleaseComObject(this.SampGrabber);
                    this.SampGrabber = null;
                }

                if (capGraph != null)
                {
                    Marshal.ReleaseComObject(capGraph);
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetConfigParms(ICaptureGraphBuilder2 capGraph, IBaseFilter capFilter)
        {
            object o;
            AMMediaType media;

            // Find the stream config interface
            capGraph.FindInterface(PinCategory.Capture, MediaType.Video, capFilter, typeof(IAMStreamConfig).GUID, out o);

            IAMStreamConfig videoStreamConfig = o as IAMStreamConfig;
            if (videoStreamConfig == null)
            {
                throw new Exception("Failed to get IAMStreamConfig");
            }

            // Get the existing format block
            int hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);

            // copy out the videoinfoheader
            VideoInfoHeader v = new VideoInfoHeader();
            Marshal.PtrToStructure(media.formatPtr, v);

            // if overriding the framerate, set the frame rate
            if (this.FrameRate > 0)
            {
                v.AvgTimePerFrame = 10000000 / this.FrameRate;
            }

            // if overriding the width, set the width
            if (this.Size.Width > 0)
            {
                v.BmiHeader.Width = this.Size.Width;
            }

            // if overriding the Height, set the Height
            if (this.Size.Height > 0)
            {
                v.BmiHeader.Height = this.Size.Height;
            }

            // Copy the media structure back
            Marshal.StructureToPtr(v, media.formatPtr, false);

            // Set the new format
            hr = videoStreamConfig.SetFormat(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);
        }

        #endregion Private Methods

        #endregion Methods
    }
}