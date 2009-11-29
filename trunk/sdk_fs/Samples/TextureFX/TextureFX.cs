using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace Mogre.Demo.TextureFX
{
    class TextureFXApp : Mogre.Demo.ExampleApplication.Example
    {
        void createScalingPlane()
        {
            // Set up a material for the plane

            // Create a prefab plane
            Entity planeEnt = sceneMgr.CreateEntity("Plane", SceneManager.PrefabType.PT_PLANE);
            // Give the plane a texture
            planeEnt.SetMaterialName("Examples/TextureEffect1");

            SceneNode node =
                sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(-250, -40, -100));

            node.AttachObject(planeEnt);
        }

        void createScrollingKnot()
        {
            Entity ent = sceneMgr.CreateEntity("knot", "knot.mesh");


            ent.SetMaterialName("Examples/TextureEffect2");
            // Add entity to the root scene node
            SceneNode node =
                sceneMgr.RootSceneNode.CreateChildSceneNode(new Vector3(200, 50, 150));

            node.AttachObject(ent);

        }

        void createWateryPlane()
        {
            // Create a prefab plane
            Entity planeEnt = sceneMgr.CreateEntity("WaterPlane", SceneManager.PrefabType.PT_PLANE);
            // Give the plane a texture
            planeEnt.SetMaterialName("Examples/TextureEffect3");

            sceneMgr.RootSceneNode.AttachObject(planeEnt);
        }
        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);

            createScalingPlane();
            createScrollingKnot();
            createWateryPlane();

            // Set up a material for the skydome
            MaterialPtr skyMat = MaterialManager.Singleton.Create("SkyMat",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            // Perform no dynamic lighting on the sky
            skyMat.SetLightingEnabled(false);
            // Use a cloudy sky
            TextureUnitState t = skyMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("clouds.jpg");
            // Scroll the clouds
            t.SetScrollAnimation(0.15f, 0f);

            // System will automatically set no depth write

            // Create a skydome
            sceneMgr.SetSkyDome(true, "SkyMat", -5, 2);
        }
    }

}
