
using System;
using Mogre;

namespace Mogre.Demo.SkyPlane
{
    class SkyPlaneApplication : ExampleApplication.Example
    {
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
            
            // Define the required skyplane
            Plane plane;
            // 5000 world units from the camera
            plane.d = 5000;
            // Above the camera, facing down
            plane.normal = -Vector3.UNIT_Y;
            // Create the plane 10000 units wide, tile the texture 3 times
            sceneMgr.SetSkyPlane(true, plane, "Examples/SpaceSkyPlane",10000,3);
            
            // Create a light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.SetPosition(20,80,50);
            
            // Also add a nice dragon in
            Entity ent = sceneMgr.CreateEntity("dragon", "dragon.mesh");
            sceneMgr.RootSceneNode.AttachObject(ent);
        }
    }
}
