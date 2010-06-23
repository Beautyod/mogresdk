namespace DSMogre
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using Mogre;

    public sealed class CaptureToTexture : IDisposable
    {
        #region Fields

        private Bitmap bitmap;
        private Capture capture;
        private object padLock = new object();
        private string textureName;

        #endregion Fields

        #region Constructors

        public CaptureToTexture(Capture capture, string texName)
        {
            this.capture = capture;
            this.capture.NewBitmap += this.OnNewBitmap;
            this.textureName = texName;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        public bool IsRunning
        {
            get
            {
                if (this.capture != null)
                {
                    return this.capture.IsRunning;
                }

                return false;
            }
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        public void Dispose()
        {
            if (this.capture != null)
            {
                this.capture.Dispose();
            }
        }

        public void Pause()
        {
            if (this.capture != null)
            {
                this.capture.Pause();
            }
        }

        public void Start()
        {
            if (this.capture != null)
            {
                this.capture.Stop();
                this.capture.Start();
                while (!this.capture.IsRunning)
                {
                }
            }
        }

        public void Stop()
        {
            if (this.capture != null)
            {
                this.capture.Stop();
            }
        }

        public void Update()
        {
            lock (this.padLock)
            {
                if (this.bitmap != null)
                {
                    ConvertBitmapToTexture(this.bitmap, this.textureName, this.bitmap.Size);
                    this.bitmap = null;
                }
            }
        }

        #endregion Public Methods

        #region Private Static Methods

        private static unsafe void ConvertBitmapToTexture(Bitmap image, string textureName, Size size)
        {
            int width = size.Width;
            int height = size.Height;
            using (ResourcePtr rpt = TextureManager.Singleton.GetByName(textureName))
            {
                using (TexturePtr texture = rpt)
                {
                    HardwarePixelBufferSharedPtr texBuffer = texture.GetBuffer();
                    texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
                    PixelBox pb = texBuffer.CurrentLock;

                    BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    NativeMethods.CopyMemory(pb.data, data.Scan0, width * height * 4);
                    image.UnlockBits(data);

                    texBuffer.Unlock();
                    texBuffer.Dispose();
                }
            }
        }

        #endregion Private Static Methods

        #region Private Methods

        private void OnNewBitmap(object sender, EventArgs e)
        {
            lock (this.padLock)
            {
                this.bitmap = (Bitmap)sender;
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}