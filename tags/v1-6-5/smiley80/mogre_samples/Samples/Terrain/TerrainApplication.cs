namespace Mogre.Demo.Terrain
{
    using System;

    using Mogre;

    class TerrainApplication : ExampleApplication.Example
    {
        #region Fields

        RaySceneQuery raySceneQuery = null;

        #endregion Fields

        #region Constructors

        public TerrainApplication()
        {
            this.camSpeed = 50;
        }

        #endregion Constructors

        #region Methods

        public override void ChooseSceneManager()
        {
            // Get the SceneManager, in this case a generic one
            sceneMgr = root.CreateSceneManager("TerrainSceneManager");
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 500 in Z direction
            camera.Position = new Vector3(128,25,128);
            // Look back along -Z
            camera.LookAt(new Vector3(0,0,-300));
            camera.NearClipDistance = 1;
            camera.FarClipDistance = 1000;
        }

        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.SetPosition(20,80,50);

            // Fog
            // NB it's VERY important to set this before calling setWorldGeometry
            // because the vertex program picked will be different
            ColourValue fadeColour = new ColourValue(0.93f, 0.86f, 0.76f);
            sceneMgr.SetFog(FogMode.FOG_LINEAR, fadeColour, .001f, 500, 1000);
            window.GetViewport(0).BackgroundColour = fadeColour;

            string terrain_cfg = "terrain.cfg";
            sceneMgr.SetWorldGeometry( terrain_cfg );
            // Infinite far plane?
            if (root.RenderSystem.Capabilities.HasCapability(Capabilities.RSC_INFINITE_FAR_PLANE))
            {
                camera.FarClipDistance = 0;
            }

            // Define the required skyplane
            Plane plane;
            // 5000 world units from the camera
            plane.d = 5000;
            // Above the camera, facing down
            plane.normal = -Vector3.UNIT_Y;

            // Set a nice viewpoint
            camera.SetPosition(707,2500,528);
            camera.Orientation = new Quaternion(-0.3486f, 0.0122f, 0.9365f, 0.0329f);
            //mRoot -> showDebugOverlay( true );

            raySceneQuery = sceneMgr.CreateRayQuery(
                new Ray(camera.Position, Vector3.NEGATIVE_UNIT_Y));
        }

        protected override bool ExampleApp_FrameStarted(FrameEvent evt)
        {
            if(base.ExampleApp_FrameStarted(evt) == false )
                return false;

            // clamp to terrain
            Ray updateRay = new Ray(this.camera.Position, Vector3.NEGATIVE_UNIT_Y);
            raySceneQuery.Ray = updateRay;
            RaySceneQueryResult qryResult = raySceneQuery.Execute();

            RaySceneQueryResultEntry rs = qryResult[0];

            if (rs != null && rs.worldFragment != null)
            {
                camera.SetPosition(camera.Position.x,
                                   rs.worldFragment.singleIntersection.y + 10,
                                   camera.Position.z);
            }

            return true;
        }

        #endregion Methods
    }
}