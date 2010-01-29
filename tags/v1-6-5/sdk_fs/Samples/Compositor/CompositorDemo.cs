using System;
using System.Collections.Generic;
using Mogre;

namespace Mogre.Demo.Compositor
{
    class HDRListener
    {
        protected int mVpWidth, mVpHeight;
        protected int mBloomSize;
        // Array params - have to pack in groups of 4 since this is how Cg generates them
        // also prevents dependent texture read problems if ops don't require swizzle
        protected float[,] mBloomTexWeights = new float[15, 4];
        protected float[,] mBloomTexOffsetsHorz = new float[15, 4];
        protected float[,] mBloomTexOffsetsVert = new float[15, 4];

        public void NotifyViewportSize(int width, int height)
        {
            mVpWidth = width;
            mVpHeight = height;
        }

        public void NotifyCompositor(CompositorInstance instance)
        {
            instance.NotifyMaterialSetup += new CompositorInstance.Listener.NotifyMaterialSetupHandler(NotifyMaterialSetup);

            // Get some RTT dimensions for later calculations
            CompositionTechnique.TextureDefinitionIterator defIter =
                instance.Technique.GetTextureDefinitionIterator();

            foreach (CompositionTechnique.TextureDefinition_NativePtr def in defIter)
            {
                if (def.name == "rt_bloom0")
                {
                    mBloomSize = (int)def.width; // should be square
                    // Calculate gaussian texture offsets & weights
                    float deviation = 3.0f;
                    float texelSize = 1.0f / (float)mBloomSize;

                    // central sample, no offset
                    mBloomTexOffsetsHorz[0, 0] = 0.0f;
                    mBloomTexOffsetsHorz[0, 1] = 0.0f;
                    mBloomTexOffsetsVert[0, 0] = 0.0f;
                    mBloomTexOffsetsVert[0, 1] = 0.0f;
                    mBloomTexWeights[0, 0] = mBloomTexWeights[0, 1] =
                        mBloomTexWeights[0, 2] = Math.GaussianDistribution(0, 0, deviation);
                    mBloomTexWeights[0, 3] = 1.0f;

                    // 'pre' samples
                    for (int i = 1; i < 8; ++i)
                    {
                        mBloomTexWeights[i, 0] = mBloomTexWeights[i, 1] =
                            mBloomTexWeights[i, 2] = 1.25f * Math.GaussianDistribution(i, 0, deviation);
                        mBloomTexWeights[i, 3] = 1.0f;
                        mBloomTexOffsetsHorz[i, 0] = i * texelSize;
                        mBloomTexOffsetsHorz[i, 1] = 0.0f;
                        mBloomTexOffsetsVert[i, 0] = 0.0f;
                        mBloomTexOffsetsVert[i, 1] = i * texelSize;
                    }
                    // 'post' samples
                    for (int i = 8; i < 15; ++i)
                    {
                        mBloomTexWeights[i, 0] = mBloomTexWeights[i, 1] =
                            mBloomTexWeights[i, 2] = mBloomTexWeights[i - 7, 0];
                        mBloomTexWeights[i, 3] = 1.0f;

                        mBloomTexOffsetsHorz[i, 0] = -mBloomTexOffsetsHorz[i - 7, 0];
                        mBloomTexOffsetsHorz[i, 1] = 0.0f;
                        mBloomTexOffsetsVert[i, 0] = 0.0f;
                        mBloomTexOffsetsVert[i, 1] = -mBloomTexOffsetsVert[i - 7, 1];
                    }

                }
            }
        }

        unsafe private void NotifyMaterialSetup(uint pass_id, MaterialPtr mat)
        {
            // Prepare the fragment params offsets
            switch (pass_id)
            {
                //case 994: // rt_lum4
                case 993: // rt_lum3
                case 992: // rt_lum2
                case 991: // rt_lum1
                case 990: // rt_lum0
                    break;
                case 800: // rt_brightpass
                    break;
                case 701: // rt_bloom1
                    {
                        // horizontal bloom
                        mat.Load();
                        GpuProgramParametersSharedPtr fparams =
                            mat.GetBestTechnique().GetPass(0).GetFragmentProgramParameters();
                        String progName = mat.GetBestTechnique().GetPass(0).FragmentProgramName;
                        fixed (float* p_mBloomTexOffsetsHorz = &mBloomTexOffsetsHorz[0, 0])
                        {
                            fparams.SetNamedConstant("sampleOffsets", p_mBloomTexOffsetsHorz, 15);
                        }
                        fixed (float* p_mBloomTexWeights = &mBloomTexWeights[0, 0])
                        {
                            fparams.SetNamedConstant("sampleWeights", p_mBloomTexWeights, 15);
                        }

                        break;
                    }
                case 700: // rt_bloom0
                    {
                        // vertical bloom
                        mat.Load();
                        GpuProgramParametersSharedPtr fparams =
                            mat.GetTechnique(0).GetPass(0).GetFragmentProgramParameters();
                        String progName = mat.GetBestTechnique().GetPass(0).FragmentProgramName;
                        fixed (float* p_mBloomTexOffsetsVert = &mBloomTexOffsetsVert[0, 0])
                        {
                            fparams.SetNamedConstant("sampleOffsets", p_mBloomTexOffsetsVert, 15);
                        }
                        fixed (float* p_mBloomTexWeights = &mBloomTexWeights[0, 0])
                        {
                            fparams.SetNamedConstant("sampleWeights", p_mBloomTexWeights, 15);
                        }

                        break;
                    }
            }
        }
    }

    class CompositorToggler
    {
        public string debugText = null;

        public string compositorName;
        public MOIS.KeyCode keyCode;
        Viewport vp;

        bool keyPressed = false;
        bool compositorIsOn = false;

        public CompositorToggler(string compositorName, MOIS.KeyCode keyCode, Viewport vp)
        {
            this.compositorName = compositorName;
            this.keyCode = keyCode;
            this.vp = vp;
        }

        public bool Update(MOIS.Keyboard input)
        {
            if (input.IsKeyDown(keyCode))
            {
                if (!keyPressed)
                {
                    keyPressed = true;
                    ToggleCompositor();
                    debugText = compositorName + " " + (compositorIsOn ? "Enabled" : "Disabled");
                    return true;
                }
            }
            else
                keyPressed = false;

            return false;
        }

        private void ToggleCompositor()
        {
            compositorIsOn = !compositorIsOn;
            CompositorManager.Singleton.SetCompositorEnabled(vp, compositorName, compositorIsOn);
        }
    }    

    class CompositorDemo : Demo.ExampleApplication.Example
    {
        CompositorToggler[] togglers;
        SceneNode mSpinny;

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            root.FrameStarted += new FrameListener.FrameStartedHandler(Compositor_FrameStarted);
        }

        bool Compositor_FrameStarted(FrameEvent evt)
        {
			if (mSpinny != null)
				mSpinny.Yaw(new Degree(10 * evt.timeSinceLastFrame));

            foreach (CompositorToggler toggler in togglers)
            {
                if (toggler.Update(inputKeyboard))
                    mDebugText = toggler.debugText;
            }

            return true;
        }

        public override void CreateScene()
        {
            sceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
            sceneMgr.ShadowFarDistance = 1000;

            MovableObject.DefaultVisibilityFlags = 0x00000001;

            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.3f, 0.3f, 0.2f);

            Light l = sceneMgr.CreateLight("Light2");
            Vector3 dir = new Vector3(-1, -1, 0);
            dir.Normalise();
            l.Type = Light.LightTypes.LT_DIRECTIONAL;
            l.Direction = dir;
            l.SetDiffuseColour(1, 1, 0.8f);
            l.SetSpecularColour(1, 1, 1);


            Entity pEnt;

            // House
            pEnt = sceneMgr.CreateEntity("1", "tudorhouse.mesh");
            SceneNode n1 = sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(350, 450, -200));
            n1.AttachObject(pEnt);

            pEnt = sceneMgr.CreateEntity("2", "tudorhouse.mesh");
            SceneNode n2 = sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(-350, 450, -200));
            n2.AttachObject(pEnt);

            pEnt = sceneMgr.CreateEntity("3", "knot.mesh");
            mSpinny = sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(0, 0, 300));
            mSpinny.AttachObject(pEnt);
            pEnt.SetMaterialName("Examples/MorningCubeMap");

            sceneMgr.SetSkyBox(true, "Examples/MorningSkyBox");


            Plane plane;
            plane.normal = Vector3.UNIT_Y;
            plane.d = 100;
            MeshManager.Singleton.CreatePlane("Myplane",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                1500, 1500, 10, 10, true, 1, 5, 5, Vector3.UNIT_Z);
            Entity pPlaneEnt = sceneMgr.CreateEntity("plane", "Myplane");
            pPlaneEnt.SetMaterialName("Examples/Rockwall");
            pPlaneEnt.CastShadows = false;
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(pPlaneEnt);

            camera.SetPosition(-400, 50, 900);
            camera.LookAt(0, 80, 0);

            AddCompositors();
        }

        void AddCompositors()
        {
            // HDR
            CompositorInstance instance = CompositorManager.Singleton.AddCompositor(viewport, "HDR", 0);
            CompositorManager.Singleton.SetCompositorEnabled(viewport, "HDR", false);
            HDRListener hdrListener = new HDRListener();
            hdrListener.NotifyViewportSize(viewport.ActualWidth, viewport.ActualHeight);
            hdrListener.NotifyCompositor(instance);

            // Glass
            instance = CompositorManager.Singleton.AddCompositor(viewport, "Glass");
            CompositorManager.Singleton.SetCompositorEnabled(viewport, "Glass", false);

            togglers = new CompositorToggler[]
                {
                    new CompositorToggler("HDR",    MOIS.KeyCode.KC_1, viewport),
                    new CompositorToggler("Glass",  MOIS.KeyCode.KC_2, viewport)
                };

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (CompositorToggler toggler in togglers)
            {
                sb.Append(toggler.keyCode.ToString().Substring(3));
                sb.Append('-');
                sb.Append(toggler.compositorName);
                sb.Append(", ");
            }
            sb.Length -= 2;

            mDebugText = sb.ToString();
        }
    }
}
