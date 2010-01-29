namespace Mogre.Demo.SkyDome
{
	using System;

	using Mogre;
	
	class SkyDomeApplication : ExampleApplication.Example
	{
	    #region Fields
	
	    float mCurvature = 1;
	    float mTiling = 15;
	
	    #endregion Fields
	
	    #region Methods
	
	    public override void CreateScene()
	    {
	        // Set ambient light
	           sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
	
	        // Create a skydome
	        sceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);
	
	        // Create a light
	        Light l = sceneMgr.CreateLight("MainLight");
	        // Accept default settings: point light, white diffuse, just set position
	        // NB I could attach the light to a SceneNode if I wanted it to move automatically with
	        //  other objects, but I don't
	        l.SetPosition(20,80,50);
	
	        Entity ent;
	
	        // Define a floor plane mesh
	        Plane p;
	        p.normal = Vector3.UNIT_Y;
	        p.d = 200;
	        MeshManager.Singleton.CreatePlane("FloorPlane",
	                                                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
	                                                p,2000,2000,1,1,true,1,5,5,Vector3.UNIT_Z);
	
	        // Create an entity (the floor)
	        ent = sceneMgr.CreateEntity("floor", "FloorPlane");
	        ent.SetMaterialName("Examples/RustySteel");
	
	        sceneMgr.RootSceneNode.AttachObject(ent);
	
	        ent = sceneMgr.CreateEntity("head", "ogrehead.mesh");
	        // Attach to child of root node, better for culling (otherwise bounds are the combination of the 2)
	        sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
	    }
	
	    public override bool Setup()
	    {
	        base.Setup();
	        LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME );
	        return true;
	    }
	
	    protected override bool ExampleApp_FrameStarted(FrameEvent evt)
	    {
	        // Change curvature / tiling
	        // Delay timer to stop too quick updates of curvature
	        float timeDelay = 0;
	
	        bool updateSky;
	        updateSky = false;
	
	        if(!base.ExampleApp_FrameStarted(evt))
	            return false;
	
	        if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_H) && timeDelay <= 0)
	        {
	            mCurvature += 1;
	            timeDelay = 0.1f;
	            updateSky = true;
	        }
	        if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_G) && timeDelay <= 0)
	        {
	            mCurvature -= 1;
	            timeDelay = 0.1f;
	            updateSky = true;
	        }
	
	        if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_U) && timeDelay <= 0)
	        {
	            mTiling += 1;
	            timeDelay = 0.1f;
	            updateSky = true;
	        }
	        if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_Y) && timeDelay <= 0)
	        {
	            mTiling -= 1;
	            timeDelay = 0.1f;
	            updateSky = true;
	        }
	
	        if (timeDelay > 0)
	            timeDelay -= evt.timeSinceLastFrame;
	
	        if (updateSky)
	        {
	            sceneMgr.SetSkyDome(true, "Examples/CloudySky", mCurvature, mTiling);
	        }
	
	        return true;
	    }
	
	    #endregion Methods
	}
}
