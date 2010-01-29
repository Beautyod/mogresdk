namespace Mogre.Demo.SkyBox
{
    using System;

    class SkyBoxApplication : ExampleApplication.Example
    {
        #region Fields

        float fDefDim = 25.0f;
        float fDefVel = 50.0f;
        ParticleSystem pThrusters;

        #endregion Fields

        #region Methods

        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a skybox
            sceneMgr.SetSkyBox(true, "Examples/SpaceSkyBox", 50 );

            // Create a light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20,80,50);

            // Also add a nice starship in
            Entity ent = sceneMgr.CreateEntity( "razor", "razor.mesh" );

            sceneMgr.RootSceneNode.AttachObject( ent );

            pThrusters = sceneMgr.CreateParticleSystem( "ParticleSys1", 200 );

            pThrusters.MaterialName = "Examples/Flare";
            pThrusters.SetDefaultDimensions( 25, 25 );

            ParticleEmitter pEmit1 = pThrusters.AddEmitter( "Point" );
            ParticleEmitter pEmit2 = pThrusters.AddEmitter( "Point" );

            // Thruster 1
            pEmit1.Angle = new Degree(3);
            pEmit1.TimeToLive = 0.2f;
            pEmit1.EmissionRate = 70;

            pEmit1.ParticleVelocity = 50;

            pEmit1.Direction = -Vector3.UNIT_Z;
            pEmit1.SetColour(ColourValue.White, ColourValue.Red);

            // Thruster 2
            pEmit2.Angle = new Degree(3);
            pEmit2.TimeToLive = 0.2f;
            pEmit2.EmissionRate = 70;

            pEmit2.ParticleVelocity = 50;

            pEmit2.Direction = -Vector3.UNIT_Z;
            pEmit2.SetColour(ColourValue.White, ColourValue.Red);

            // Set the position of the thrusters
            pEmit1.Position = new Vector3( 5.7f, 0.0f, 0.0f );
            pEmit2.Position = new Vector3( -18.0f, 0.0f, 0.0f );

            sceneMgr.RootSceneNode.CreateChildSceneNode( new Vector3( 0.0f, 6.5f, -67.0f ) )
                .AttachObject(pThrusters);
        }

        protected override bool ExampleApp_FrameStarted(FrameEvent evt)
        {
            if(base.ExampleApp_FrameStarted(evt) == false )
                return false;

            if( inputKeyboard.IsKeyDown( MOIS.KeyCode.KC_N ) )
            {
                pThrusters.SetDefaultDimensions( fDefDim + 0.25f, fDefDim + 0.25f );
                fDefDim += 0.25f;
            }

            if( inputKeyboard.IsKeyDown( MOIS.KeyCode.KC_M ) )
            {
                pThrusters.SetDefaultDimensions( fDefDim - 0.25f, fDefDim - 0.25f );
                fDefDim -= 0.25f;
            }

            if( inputKeyboard.IsKeyDown( MOIS.KeyCode.KC_H ) )
            {
                pThrusters.GetEmitter( 0 ).ParticleVelocity = fDefVel + 1;
                pThrusters.GetEmitter( 1 ).ParticleVelocity = fDefVel + 1;
                fDefVel += 1;
            }

            if( inputKeyboard.IsKeyDown( MOIS.KeyCode.KC_J ) && !( fDefVel < 0.0f ) )
            {
                pThrusters.GetEmitter( 0 ).ParticleVelocity = fDefVel - 1;
                pThrusters.GetEmitter( 1 ).ParticleVelocity = fDefVel - 1;
                fDefVel -= 1;
            }

            return true;
        }

        #endregion Methods
    }
}