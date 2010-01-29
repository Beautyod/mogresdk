namespace Mogre.Demo.Smoke
{
    using System;

    using Mogre;

    class SmokeApplication : ExampleApplication.Example
    {
        #region Fields

        SceneNode mFountainNode;

        #endregion Fields

        #region Methods

        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a skydome
            sceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            // Create shared node for 2 fountains
            mFountainNode = (SceneNode)(sceneMgr.RootSceneNode.CreateChild());

            // smoke
            ParticleSystem pSys2 = sceneMgr.CreateParticleSystem("fountain1",
                                                                 "Examples/Smoke");
            // Point the fountain at an angle
            SceneNode fNode = (SceneNode)(mFountainNode.CreateChild());
            fNode.AttachObject(pSys2);
        }

        protected override bool ExampleApp_FrameStarted(FrameEvent evt)
        {
            // Rotate fountains
            mFountainNode.Yaw(evt.timeSinceLastFrame * 30);

            // Call default
            return base.ExampleApp_FrameStarted(evt);
        }

        #endregion Methods
    }
}