using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.EnvMapping
{
    class EnvMapping : Mogre.Demo.ExampleApplication.Example
    {
        public override void CreateScene()
        {
            // Just override the mandatory create scene method
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);

            Entity ent = sceneMgr.CreateEntity("head", "ogrehead.mesh");

            // Set material loaded from Example.material
            ent.SetMaterialName("Examples/EnvMappedRustySteel");

            // Add entity to the root scene node
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
        }
    } 
}
