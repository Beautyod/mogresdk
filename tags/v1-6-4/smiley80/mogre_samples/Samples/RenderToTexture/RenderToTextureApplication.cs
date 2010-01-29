namespace Mogre.Demo.RenderToTexture
{
	using System;
	using System.Collections.Generic;

	using Mogre;
	class RenderToTextureApplication : ExampleApplication.Example
	{
	    #region Fields
	
	    MovablePlane mPlane;
	    Entity mPlaneEnt;
	    SceneNode mPlaneNode;
	    Camera mReflectCam;
	
	    #endregion Fields
	
	    #region Methods
	
	    // Just override the mandatory create scene method
	    public override void CreateScene()
	    {
	        // Set ambient light
	        sceneMgr.AmbientLight = new ColourValue(0.2f, 0.2f, 0.2f);
	        // Skybox
	        sceneMgr.SetSkyBox(true, "Examples/MorningSkyBox");
	
	        // Create a light
	        Light l = sceneMgr.CreateLight("MainLight");
	        l.Type = Light.LightTypes.LT_DIRECTIONAL;
	        Vector3 dir = new Vector3(0.5f, -1, 0);
	        dir.Normalise();
	        l.Direction = dir;
	        l.DiffuseColour = new ColourValue(1.0f, 1.0f, 0.8f);
	        l.SpecularColour= new ColourValue(1.0f, 1.0f, 1.0f);
	
	        // Create a prefab plane
	        mPlane = new MovablePlane(Vector3.UNIT_Y, 0);
	
	        MeshManager.Singleton.CreatePlane("ReflectionPlane",
	                                          ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
	                                          mPlane._getDerivedPlane(), 2000, 2000,
	                                          1, 1, true, 1, 1, 1, Vector3.UNIT_Z);
	        mPlaneEnt = sceneMgr.CreateEntity( "Plane", "ReflectionPlane" );
	
	        // Create an entity from a model (will be loaded automatically)
	        Entity knotEnt = sceneMgr.CreateEntity("Knot", "knot.mesh");
	
	        // Create an entity from a model (will be loaded automatically)
	        Entity ogreHead = sceneMgr.CreateEntity("Head", "ogrehead.mesh");
	
	        knotEnt.SetMaterialName("Examples/TextureEffect2");
	
	        // Attach the rtt entity to the root of the scene
	        SceneNode rootNode = sceneMgr.RootSceneNode;
	        mPlaneNode = rootNode.CreateChildSceneNode();
	
	        // Attach both the plane entity, and the plane definition
	        mPlaneNode.AttachObject(mPlaneEnt);
	        mPlaneNode.AttachObject(mPlane);
	        mPlaneNode.Translate(0, -10, 0);
	        // Tilt it a little to make it interesting
	        mPlaneNode.Roll(new Degree(5));
	
	        rootNode.CreateChildSceneNode( "Head" ).AttachObject( ogreHead );
	
	        TexturePtr texture = TextureManager.Singleton.CreateManual( "RttTex",
	                                                                   ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
	                                                                   TextureType.TEX_TYPE_2D,
	                                                                   512, 512, 0,
	                                                                   PixelFormat.PF_R8G8B8,
	                                                                   (int) TextureUsage.TU_RENDERTARGET );
	        RenderTarget rttTex = texture.GetBuffer().GetRenderTarget();
	        
	            mReflectCam = sceneMgr.CreateCamera("ReflectCam");
	            mReflectCam.NearClipDistance = camera.NearClipDistance;
	            mReflectCam.FarClipDistance = camera.FarClipDistance;
	            mReflectCam.AspectRatio =
	                (float)window.GetViewport(0).ActualWidth /
	                (float)window.GetViewport(0).ActualHeight;
	            mReflectCam.FOVy = camera.FOVy;
	
	            Viewport v = rttTex.AddViewport( mReflectCam );
	            v.SetClearEveryFrame(true);
	            v.BackgroundColour = ColourValue.Black;
	
	            MaterialPtr mat = MaterialManager.Singleton.Create("RttMat",
	                                                               ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
	            TextureUnitState t = mat.GetTechnique(0).GetPass(0).CreateTextureUnitState("RustedMetal.jpg");
	            t = mat.GetTechnique(0).GetPass(0).CreateTextureUnitState("RttTex");
	            // Blend with base texture
	            t.SetColourOperationEx(LayerBlendOperationEx.LBX_BLEND_MANUAL,
	                                   LayerBlendSource.LBS_TEXTURE,
	                                   LayerBlendSource.LBS_CURRENT,
	                                   ColourValue.White,
	                                   ColourValue.White, 0.25f);
	            t.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_CLAMP);
	            t.SetProjectiveTexturing(true, mReflectCam);
	            rttTex.PostRenderTargetUpdate += postRenderTargetUpdate;
	            rttTex.PreRenderTargetUpdate += preRenderTargetUpdate;
	
	            // set up linked reflection
	            mReflectCam.EnableReflection(mPlane);
	            // Also clip
	            mReflectCam.EnableCustomNearClipPlane(mPlane);
	        
	
	        // Give the plane a texture
	        mPlaneEnt.SetMaterialName("RttMat");
	
	        // Add a whole bunch of extra transparent entities
	        Entity cloneEnt;
	        for (int n = 0; n < 10; ++n)
	        {
	            // Create a new node under the root
	            SceneNode node = sceneMgr.CreateSceneNode();
	            // Random translate
	            Vector3 nodePos;
	            nodePos.x = Mogre.Math.SymmetricRandom() * 750.0f;
	            nodePos.y = Mogre.Math.SymmetricRandom() * 100.0f + 25;
	            nodePos.z = Mogre.Math.SymmetricRandom() * 750.0f;
	            node.Position = nodePos;
	            rootNode.AddChild(node);
	            // Clone knot
	            string cloneName = "Knot" + n;
	            cloneEnt = knotEnt.Clone(cloneName);
	            // Attach to new node
	            node.AttachObject(cloneEnt);
	
	        }
	
	        camera.Position = new Vector3(-50, 100, 500);
	        camera.LookAt(0,0,0);
	    }
	
	    protected override bool ExampleApp_FrameStarted(FrameEvent evt)
	    {
	        if(base.ExampleApp_FrameStarted(evt) == false )
	            return false;
	
	        // Make sure reflection camera is updated too
	        mReflectCam.Orientation = camera.Orientation;
	        mReflectCam.Position = camera.Position;
	
	        // Rotate plane
	        mPlaneNode.Yaw(new Degree(30 * evt.timeSinceLastFrame), Node.TransformSpace.TS_PARENT);
	        return true;
	    }
	
	    void postRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
	    {
	        // Show plane
	        mPlaneEnt.Visible = true;
	    }
	
	    // render target events
	    void preRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
	    {
	        // Hide plane
	        mPlaneEnt.Visible = false;
	    }
	
	    #endregion Methods
	}
}
