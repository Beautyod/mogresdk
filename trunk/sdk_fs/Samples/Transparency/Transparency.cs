using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.Transparency
{
    class Transparency : Mogre.Demo.ExampleApplication.Example
    {
        public override void CreateScene()
        {
            // Just override the mandatory create scene method
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);

            // Create a prefab plane
            Entity planeEnt = sceneMgr.CreateEntity("Plane", SceneManager.PrefabType.PT_PLANE);
            // Give the plane a texture
            planeEnt.SetMaterialName("Examples/BumpyMetal");

            // Create an entity from a model (will be loaded automatically)
            Entity knotEnt = sceneMgr.CreateEntity("Knot", "knot.mesh");

            knotEnt.SetMaterialName("Examples/TransparentTest");

            // Attach the 2 new entities to the root of the scene
            sceneMgr.RootSceneNode.AttachObject(planeEnt);
            sceneMgr.RootSceneNode.AttachObject(knotEnt);

            // Add a whole bunch of extra transparent entities
            Entity cloneEnt;
            for (int n = 0; n < 10; ++n)
            {
                // Create a new node under the root
                SceneNode node = sceneMgr.CreateSceneNode();
                // Random translate
                Vector3 nodePos;
                nodePos.x = Mogre.Math.SymmetricRandom() * 500.0f;
                nodePos.y = Mogre.Math.SymmetricRandom() * 500.0f;
                nodePos.z = Mogre.Math.SymmetricRandom() * 500.0f;
                node.Position = nodePos;
                sceneMgr.RootSceneNode.AddChild(node);
                // Clone knot
                cloneEnt = knotEnt.Clone("Knot" + n.ToString());
                // Attach to new node
                node.AttachObject(cloneEnt);

            }
        }
    } 
}
