namespace Mogre.Demo.ParticleFX
{
    using System;

    // Event handler to add ability to alter curvature
    class ParticleApplication : ExampleApplication.Example
    {
        #region Fields

        protected SceneNode mFountainNode;

        #endregion Fields

        #region Methods

        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            Entity ent = sceneMgr.CreateEntity("head", "ogrehead.mesh");

            // Add entity to the root scene node
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);

            // Green nimbus around Ogre
            //mSceneMgr->getRootSceneNode()->createChildSceneNode()->attachObject(
            //        mSceneMgr->createParticleSystem("Nimbus", "Examples/GreenyNimbus"));

            // Create some nice fireworks

            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(
                sceneMgr.CreateParticleSystem("Fireworks", "Examples/Fireworks"));

            // Create shared node for 2 fountains
            mFountainNode = sceneMgr.RootSceneNode.CreateChildSceneNode();

            // fountain 1
            ParticleSystem pSys2 = sceneMgr.CreateParticleSystem("fountain1",
                                                                 "Examples/PurpleFountain");
            // Point the fountain at an angle
            SceneNode fNode = mFountainNode.CreateChildSceneNode();
            fNode.Translate(200,-100,0);
            fNode.Rotate(Vector3.UNIT_Z, new Degree(20));
            fNode.AttachObject(pSys2);

            // fountain 2
            ParticleSystem pSys3 = sceneMgr.CreateParticleSystem("fountain2",
                                                                 "Examples/PurpleFountain");
            // Point the fountain at an angle
            fNode = mFountainNode.CreateChildSceneNode();
            fNode.Translate(-200,-100,0);
            fNode.Rotate(Vector3.UNIT_Z, new Degree(-20));
            fNode.AttachObject(pSys3);

            // Create a rainstorm
            ParticleSystem pSys4 = sceneMgr.CreateParticleSystem("rain",
                                                                 "Examples/Rain");
            SceneNode rNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            rNode.Translate(0,1000,0);
            rNode.AttachObject(pSys4);
            // Fast-forward the rain so it looks more natural
            pSys4.FastForward(5);

            // Aureola around Ogre perpendicular to the ground
            ParticleSystem pSys5 = sceneMgr.CreateParticleSystem("Aureola",
                                                                 "Examples/Aureola");
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(pSys5);

            // Set nonvisible timeout
            ParticleSystem.DefaultNonVisibleUpdateTimeout = 5;
        }

        protected override bool ExampleApp_FrameStarted(FrameEvent evt)
        {
            if(base.ExampleApp_FrameStarted(evt) == false )
                return false;

            // Rotate fountains
            mFountainNode.Yaw(new Degree(evt.timeSinceLastFrame * 30));

            // Call default
            return true;
        }

        #endregion Methods
    }
}