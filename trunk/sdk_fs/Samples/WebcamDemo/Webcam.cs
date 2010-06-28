namespace Mogre.Demo.Webcam
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;

    using DSMogre;

    using Mogre;

    public class Webcam
    {
        #region Fields

        private Camera camera;
        private int captureSource;
        private CaptureToTexture ctt;
        private Root root;
        private SceneManager sceneMgr;
        private Viewport viewport;
        private RenderWindow window;

        #endregion Fields

        #region Constructors

        public Webcam(int captureSource)
        {
            this.captureSource = captureSource - 2;
            this.Setup();

            this.root.StartRendering();
            this.ctt.Dispose();

            this.root.Dispose();
            this.root = null;
        }

        #endregion Constructors

        #region Methods

        #region Private Static Methods

        private static int NextPowerOfTwo(int val)
        {
            double vald = val;
            return (int)System.Math.Pow(2, System.Math.Ceiling(System.Math.Log(vald, 2)));
        }

        #endregion Private Static Methods

        #region Private Methods

        private bool Scene_FrameStarted(FrameEvent evt)
        {
            if (this.window.IsClosed)
            {
                return false;
            }

            // update the video texture
            this.ctt.Update();

            return true;
        }

        private void Setup()
        {
            this.root = new Root();
            this.SetupResources();

            this.root.ShowConfigDialog();
            
            this.window = this.root.Initialise(true);

            // Get the SceneManager, in this case a generic one
            this.sceneMgr = Root.Singleton.CreateSceneManager("DefaultSceneManager");

            // Load resources
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            this.root.FrameStarted += new FrameListener.FrameStartedHandler(this.Scene_FrameStarted);

            // Create the camera
            this.camera = this.sceneMgr.CreateCamera("PlayerCam");

            this.camera.NearClipDistance = 5;
            this.camera.FarClipDistance = 0;
            this.camera.FOVy = 45;

            this.viewport = this.window.AddViewport(this.camera);
            this.viewport.BackgroundColour = ColourValue.White;

            this.camera.Position = new Vector3 (100, 100, 100);

            this.camera.LookAt(new Vector3 (0, 0, 0));
            Light l = sceneMgr.CreateLight();
            l.DiffuseColour = ColourValue.White;
            l.Position = new Vector3(450, 200, 1000);
            l.Type = Light.LightTypes.LT_POINT;

            l.PowerScale = 20;

            this.SetupCapture();
        }

        private void SetupCapture()
        {
            Capture cap;
            if (this.captureSource == -1)
            {
                cap = new VideoFileCapture(@"..\..\Media\test.avi")
                {
                    Looping = true
                };
            }
            else
            {
                cap = new WebcamCapture(this.captureSource);
            }

            this.ctt = new CaptureToTexture(cap, "vidtex");

            // get video size
            this.ctt.Start();
            while (!cap.IsRunning)
            {
            }
            this.ctt.Stop();
            Size s = new Size(NextPowerOfTwo(cap.Size.Width), NextPowerOfTwo(cap.Size.Height));

            using (TexturePtr tex = TextureManager.Singleton.CreateManual(
                "vidtex",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                TextureType.TEX_TYPE_2D,
                (uint)s.Width,
                (uint)s.Height,
                0,
                PixelFormat.PF_A8R8G8B8))
            {
                using (MaterialPtr mat = MaterialManager.Singleton.Create("vidmat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME))
                {
                    mat.GetTechnique(0).CreatePass().CreateTextureUnitState(tex.Name);
                    Entity e = this.sceneMgr.CreateEntity("cube", "cube.mesh");
                    e.SetMaterial(mat);
                    this.sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(e);
                }
            }

            this.ctt.Start();
        }

        private void SetupResources()
        {
            // Load resource paths from config file
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);

            // Go through all sections & settings in the file
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();

            string secName, typeName, archName;

            // Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName);
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}