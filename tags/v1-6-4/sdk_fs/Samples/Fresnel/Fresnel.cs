using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace Mogre.Demo.Fresnel
{
    class Fresnel : Demo.ExampleApplication.Example
    {
        Entity pPlaneEnt;
        List<Entity> aboveWaterEnts = new List<Entity>();
        List<Entity> belowWaterEnts = new List<Entity>();

        // Fish!
        const int NUM_FISH = 30;
        const int NUM_FISH_WAYPOINTS = 10;
        const int FISH_PATH_LENGTH = 200;
        AnimationState[] fishAnimations = new AnimationState[NUM_FISH];
        SimpleSpline[] fishSplines = new SimpleSpline[NUM_FISH];
        Vector3[] fishLastPosition = new Vector3[NUM_FISH];
        SceneNode[] fishNodes = new SceneNode[NUM_FISH];
        float animTime = 0.0f;

        Plane reflectionPlane;

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();

            root.FrameStarted += new FrameListener.FrameStartedHandler(root_FrameStarted);
        }

        bool root_FrameStarted(FrameEvent evt)
        {
            animTime += evt.timeSinceLastFrame;
            while (animTime > FISH_PATH_LENGTH)
                animTime -= FISH_PATH_LENGTH;

            for (int fish = 0; fish < NUM_FISH; ++fish)
            {
                // Animate the fish
                fishAnimations[fish].AddTime(evt.timeSinceLastFrame * 2);
                // Move the fish
                Vector3 newPos = fishSplines[fish].Interpolate(animTime / FISH_PATH_LENGTH);
                fishNodes[fish].Position = newPos;
                // Work out the direction
                Vector3 direction = fishLastPosition[fish] - newPos;
                direction.Normalise();
                // Test for opposite vectors
                float d = 1.0f + Vector3.UNIT_X.DotProduct(direction);
                if (System.Math.Abs(d) < 0.00001)
                {
                    // Diametrically opposed vectors
                    Quaternion orientation = new Quaternion();
                    orientation.FromAxes(Vector3.NEGATIVE_UNIT_X,
                        Vector3.UNIT_Y, Vector3.NEGATIVE_UNIT_Z);
                    fishNodes[fish].Orientation = orientation;
                }
                else
                {
                    fishNodes[fish].Orientation =
                        Vector3.UNIT_X.GetRotationTo(direction);
                }
                fishLastPosition[fish] = newPos;

            }

            return true;
        }

        public override void CreateScene()
        {
            //init spline array
            for (int i = 0; i < NUM_FISH; i++)
            {
                fishSplines[i] = new SimpleSpline();
            }


            // Check prerequisites first
            RenderSystemCapabilities caps = Root.Singleton.RenderSystem.Capabilities;
            if (!caps.HasCapability(Capabilities.RSC_VERTEX_PROGRAM) || !(caps.HasCapability(Capabilities.RSC_FRAGMENT_PROGRAM)))
            {
                throw new System.Exception("Your card does not support vertex and fragment programs, so cannot run this demo. Sorry!");
            }
            else
            {
                if (!GpuProgramManager.Singleton.IsSyntaxSupported("arbfp1") &&
                    !GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_0") &&
                    !GpuProgramManager.Singleton.IsSyntaxSupported("ps_1_4")
                    )
                {
                    throw new System.Exception("Your card does not support advanced fragment programs, so cannot run this demo. Sorry!");
                }
            }

            camera.SetPosition(-50, 125, 760);
            camera.SetDirection(0, 0, -1);
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            l.Type = Light.LightTypes.LT_DIRECTIONAL;
            l.Direction = -Vector3.UNIT_Y;

            Entity pEnt;

            TexturePtr mTexture = TextureManager.Singleton.CreateManual("Refraction",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                512, 512, 0, PixelFormat.PF_R8G8B8, (int)TextureUsage.TU_RENDERTARGET);
            //RenderTexture* rttTex = mRoot.getRenderSystem().createRenderTexture( "Refraction", 512, 512 );
            RenderTarget rttTex = mTexture.GetBuffer().GetRenderTarget();

            {
                Viewport v = rttTex.AddViewport(camera);
                MaterialPtr mat = MaterialManager.Singleton.GetByName("Examples/FresnelReflectionRefraction");
                mat.GetTechnique(0).GetPass(0).GetTextureUnitState(2).SetTextureName("Refraction");
                v.OverlaysEnabled = false;
                rttTex.PreRenderTargetUpdate += new RenderTargetListener.PreRenderTargetUpdateHandler(Refraction_PreRenderTargetUpdate);
                rttTex.PostRenderTargetUpdate += new RenderTargetListener.PostRenderTargetUpdateHandler(Refraction_PostRenderTargetUpdate);
            }

            mTexture = TextureManager.Singleton.CreateManual("Reflection",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                512, 512, 0, PixelFormat.PF_R8G8B8, (int)TextureUsage.TU_RENDERTARGET);
            //rttTex = mRoot.getRenderSystem().createRenderTexture( "Reflection", 512, 512 );
            rttTex = mTexture.GetBuffer().GetRenderTarget();
            {
                Viewport v = rttTex.AddViewport(camera);
                MaterialPtr mat = MaterialManager.Singleton.GetByName("Examples/FresnelReflectionRefraction");
                mat.GetTechnique(0).GetPass(0).GetTextureUnitState(1).SetTextureName("Reflection");
                v.OverlaysEnabled = false;
                rttTex.PreRenderTargetUpdate += new RenderTargetListener.PreRenderTargetUpdateHandler(Reflection_PreRenderTargetUpdate);
                rttTex.PostRenderTargetUpdate += new RenderTargetListener.PostRenderTargetUpdateHandler(Reflection_PostRenderTargetUpdate);
            }


            // Define a floor plane mesh
            reflectionPlane.normal = Vector3.UNIT_Y;
            reflectionPlane.d = 0;
            MeshManager.Singleton.CreatePlane("ReflectPlane",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                reflectionPlane,
                1500, 1500, 10, 10, true, 1, 5, 5, Vector3.UNIT_Z);
            pPlaneEnt = sceneMgr.CreateEntity("plane", "ReflectPlane");
            pPlaneEnt.SetMaterialName("Examples/FresnelReflectionRefraction");
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(pPlaneEnt);


            sceneMgr.SetSkyBox(true, "Examples/CloudyNoonSkyBox");

            // My node to which all objects will be attached
            SceneNode myRootNode = sceneMgr.RootSceneNode.CreateChildSceneNode();

		// above water entities
        pEnt = sceneMgr.CreateEntity( "RomanBathUpper", "RomanBathUpper.mesh" );
		myRootNode.AttachObject(pEnt);        
        aboveWaterEnts.Add(pEnt);

        pEnt = sceneMgr.CreateEntity( "Columns", "Columns.mesh" );
		myRootNode.AttachObject(pEnt);        
        aboveWaterEnts.Add(pEnt);

		SceneNode headNode = myRootNode.CreateChildSceneNode ();
		pEnt = sceneMgr.CreateEntity( "OgreHead", "ogrehead.mesh" );
		pEnt.SetMaterialName ("RomanBath/OgreStone");
        headNode.AttachObject(pEnt);
		headNode.SetPosition(-350,55,130);
		headNode.Rotate(Vector3.UNIT_Y, new Degree (90));
        aboveWaterEnts.Add(pEnt);

		// below water entities
		pEnt = sceneMgr.CreateEntity( "RomanBathLower", "RomanBathLower.mesh" );
        myRootNode.AttachObject(pEnt);
        belowWaterEnts.Add(pEnt);

            for (int fishNo = 0; fishNo < NUM_FISH; ++fishNo)
            {
                pEnt = sceneMgr.CreateEntity("fish" + fishNo, "fish.mesh");
                fishNodes[fishNo] = myRootNode.CreateChildSceneNode();
                fishAnimations[fishNo] = pEnt.GetAnimationState("swim");
                fishAnimations[fishNo].Enabled = true;
                fishNodes[fishNo].AttachObject(pEnt);
                belowWaterEnts.Add(pEnt);


                // Generate a random selection of points for the fish to swim to
                fishSplines[fishNo].SetAutoCalculate(false);
                Vector3 lastPos = new Vector3();
                for (int waypoint = 0; waypoint < NUM_FISH_WAYPOINTS; ++waypoint)
                {
                    Vector3 pos = new Vector3(
                        Mogre.Math.SymmetricRandom() * 270, -10, Mogre.Math.SymmetricRandom() * 700);
                    if (waypoint > 0)
                    {
                        // check this waypoint isn't too far, we don't want turbo-fish ;)
                        // since the waypoints are achieved every 5 seconds, half the length
                        // of the pond is ok
                        while ((lastPos - pos).Length > 750)
                        {
                            pos = new Vector3(
                                Mogre.Math.SymmetricRandom() * 270, -10, Mogre.Math.SymmetricRandom() * 700);
                        }
                    }
                    fishSplines[fishNo].AddPoint(pos);
                    lastPos = pos;
                }
                // close the spline
                fishSplines[fishNo].AddPoint(fishSplines[fishNo].GetPoint(0));
                // recalc
                fishSplines[fishNo].RecalcTangents();


            }
        }

        void Refraction_PreRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            // Hide plane and objects above the water
            pPlaneEnt.Visible = false;
            foreach (Entity ent in aboveWaterEnts)
            {
                ent.Visible = false;
            }
        }

        void Refraction_PostRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            // Show plane and objects above the water
            pPlaneEnt.Visible = true;
            foreach (Entity ent in aboveWaterEnts)
            {
                ent.Visible = true;
            }
        }

        void Reflection_PreRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            // Hide plane and objects below the water
            pPlaneEnt.Visible = false;
            foreach (Entity ent in belowWaterEnts)
            {
                ent.Visible = false;
            }
            camera.EnableReflection(reflectionPlane);
        }

        void Reflection_PostRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            // Show plane and objects below the water
            pPlaneEnt.Visible = true;
            foreach (Entity ent in belowWaterEnts)
            {
                ent.Visible = true;
            }
            camera.DisableReflection();
        }

        public override void DestroyScene()
        {
            base.DestroyScene();

            //clean up the splines
            for (int i = 0; i < NUM_FISH; i++)
            {
                fishSplines[i].Dispose();
            }
        }
    }
}
