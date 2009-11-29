using System;
using System.Collections.Generic;
using Mogre;

namespace Mogre.Demo.SkeletalAnimation
{
    class Skeletal : Demo.ExampleApplication.Example
    {
        const int NUM_ROBOTS = 10;
        const int ROW_COUNT = 10;

        AnimationState[] _animState = new AnimationState[NUM_ROBOTS];
        float[] _animationSpeed = new float[NUM_ROBOTS];

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            root.FrameStarted += new FrameListener.FrameStartedHandler(Skeletal_FrameStarted);
        }

        bool Skeletal_FrameStarted(FrameEvent evt)
        {
            for (int i = 0; i < NUM_ROBOTS; ++i)
            {
                _animState[i].AddTime(evt.timeSinceLastFrame * _animationSpeed[i]);
            }

            return true;
        }

        public override void CreateScene()
        {
            // Setup animation default
            Animation.DefaultInterpolationMode = Animation.InterpolationMode.IM_LINEAR;
            Animation.DefaultRotationInterpolationMode = Animation.RotationInterpolationMode.RIM_LINEAR;

            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            Entity ent = null;
            int row = 0;
            int column = 0;
            Random rnd = new Random();
            for (int i = 0; i < NUM_ROBOTS; ++i, ++column)
            {
                if (column > ROW_COUNT)
                {
                    ++row;
                    column = 0;
                }
                ent = sceneMgr.CreateEntity("robot" + i, "robot.mesh");
                sceneMgr.RootSceneNode.CreateChildSceneNode(
                    new Vector3(-(row * 100), 0, (column * 50))).AttachObject(ent);

                _animState[i] = ent.GetAnimationState("Walk");
                _animState[i].Enabled = true;
                _animationSpeed[i] = (float)(rnd.NextDouble() + 0.5);
            }


            // Give it a little ambience with lights
            Light l = null;
            l = sceneMgr.CreateLight("BlueLight");
            l.SetPosition(-200, -80, -100);
            l.SetDiffuseColour(0.5f, 0.5f, 1.0f);

            l = sceneMgr.CreateLight("GreenLight");
            l.SetPosition(0, 0, -100);
            l.SetDiffuseColour(0.5f, 1.0f, 0.5f);

            // Position the camera
            camera.SetPosition(100, 50, 100);
            camera.LookAt(-50, 50, 0);

            // Report whether hardware skinning is enabled or not
            Technique te = ent.GetSubEntity(0).GetMaterial().GetBestTechnique();
            Pass p = te.GetPass(0);
            if (p.HasVertexProgram && p.GetVertexProgram().IsSkeletalAnimationIncluded)
            {
                mDebugText = "Hardware skinning is enabled";
            }
            else
            {
                mDebugText = "Software skinning is enabled";
            }
        }
    }
}
