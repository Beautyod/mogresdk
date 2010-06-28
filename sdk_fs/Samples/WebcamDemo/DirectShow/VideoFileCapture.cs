namespace DSMogre
{
    using System.IO;
    using System.Runtime.InteropServices;

    using DirectShowLib;

    public sealed class VideoFileCapture : Capture
    {
        #region Fields

        private readonly string fileName;

        private IBasicAudio audio;
        private int volume;

        #endregion Fields

        #region Constructors

        public VideoFileCapture(string fileName)
        {
            this.volume = 100;

            if (File.Exists(fileName))
            {
                this.fileName = fileName;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        public int Volume
        {
            get
            {
                return this.volume;
            }

            set
            {
                if (this.audio != null)
                {
                    this.audio.put_Volume((value * 100) - 10000);
                }

                this.volume = value;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Protected Methods

        protected override void SetupGraph()
        {
            this.HasBeenSetup = true;

            // Get the graphbuilder object
            this.FilterGraph = new FilterGraph() as IFilterGraph2;

            // Get a ICaptureGraphBuilder2 to help build the graph
            ICaptureGraphBuilder2 icgb2 = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;

            try
            {
                // Link the ICaptureGraphBuilder2 to the IFilterGraph2
                if (icgb2 != null)
                {
                    int hr = icgb2.SetFiltergraph(this.FilterGraph);
                    DsError.ThrowExceptionForHR(hr);

                    // Add the filters necessary to render the file.  This function will
                    // work with a number of different file types.
                    IBaseFilter sourceFilter;
                    hr = this.FilterGraph.AddSourceFilter(this.fileName, this.fileName, out sourceFilter);
                    DsError.ThrowExceptionForHR(hr);

                    // VIDEO
                    // Get the SampleGrabber interface
                    this.SampGrabber = (ISampleGrabber)new SampleGrabber();
                    IBaseFilter baseGrabFlt = (IBaseFilter)this.SampGrabber;

                    // Configure the Sample Grabber
                    this.ConfigureSampleGrabber(this.SampGrabber);

                    // Add it to the filter
                    hr = this.FilterGraph.AddFilter(baseGrabFlt, "Ds.NET GrabberV");
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the pieces together, use the default renderer
                    hr = icgb2.RenderStream(null, null, sourceFilter, baseGrabFlt, null);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect audio
                    hr = icgb2.RenderStream(null, MediaType.Audio, sourceFilter, null, null);
                    if (hr != -2147467259 || hr == 0)
                    {
                        // don't throw for videos without audio track
                        DsError.ThrowExceptionForHR(hr);
                    }
                }

                IVideoWindow window = (IVideoWindow)this.FilterGraph;
                if (window != null)
                {
                    window.put_AutoShow(OABool.False);
                }

                // Now that the graph is built, read the dimensions of the bitmaps we'll be getting
                this.SaveSizeInfo(this.SampGrabber);

                // Grab some other interfaces
                this.MediaCtrl = this.FilterGraph as IMediaControl;

                this.SampGrabber.SetBufferSamples(true);
                this.SampGrabber.SetOneShot(false);

                this.MediaEvent = this.FilterGraph as IMediaEvent;
                this.audio = this.FilterGraph as IBasicAudio;
                this.MediaPosition = this.FilterGraph as IMediaPosition;
                this.Volume = this.volume;
            }
            finally
            {
                if (icgb2 != null)
                {
                    Marshal.ReleaseComObject(icgb2);
                }
            }
        }

        #endregion Protected Methods

        #endregion Methods
    }
}