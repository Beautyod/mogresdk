// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_2

using System;
 using System.Collections.Generic;
 using System.Windows.Forms;
 using MogreFramework;
 using Mogre;
 
 namespace Tutorial02
 {
     static class Program
     {
         [STAThread]
         static void Main()
         {
             try
             {
                 MyOgreWindow win = new MyOgreWindow();
                 new SceneCreator(win);
                 win.Go();
             }
             catch (System.Runtime.InteropServices.SEHException)
             {
                 if (OgreException.IsThrown)
                     MessageBox.Show(OgreException.LastException.FullDescription, "An Ogre exception has occurred!");
                 else
                     throw;
             }
         }
     }
 
     class MyOgreWindow : OgreWindow
     {
         protected override void CreateCamera()
         {
             Camera = this.SceneManager.CreateCamera("PlayerCam");
 
 
             Camera.Position = new Vector3(0, 10, 500);
             Camera.LookAt(Vector3.ZERO);
 
 
             Camera.NearClipDistance = 5;
         }
 
 
        protected override void CreateViewport()
         {
             Viewport = this.RenderWindow.AddViewport(Camera);
 
             Viewport.BackgroundColour = ColourValue.Black;
             Camera.AspectRatio = (float)Viewport.ActualWidth / Viewport.ActualHeight;
 
       }
     }
 
     class SceneCreator
     {
         public SceneCreator(OgreWindow win)
         {
             win.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
         }
 
         void SceneCreating(OgreWindow win)
         {
             // Set the ambient light and shadow technique
             SceneManager mgr = win.SceneManager;
             mgr.SetShadowUseInfiniteFarPlane(false);
             mgr.AmbientLight = ColourValue.Black;
             mgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
 
             // Create a ninja
             Entity ent = mgr.CreateEntity("ninja", "ninja.mesh");
             ent.CastShadows = true;
             mgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
 
             // Define a ground plane
             Plane plane = new Plane(Vector3.UNIT_Y, 0);
             MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                 plane, 1500, 1500, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);
 
             // Create a ground plane
             ent = mgr.CreateEntity("GroundEntity", "ground");
             mgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
 
             ent.SetMaterialName("Examples/Rockwall");
             ent.CastShadows = false;
 
 
            // Create the first light
             Light light;
             light = mgr.CreateLight("Light1");
             light.Type = Light.LightTypes.LT_POINT;
             light.Position = new Vector3(0, 150, 250);
             light.DiffuseColour = ColourValue.Red;
             light.SpecularColour = ColourValue.Red;
 
             // Create the second light
             light = mgr.CreateLight("Light2");
             light.Type = Light.LightTypes.LT_DIRECTIONAL;
             light.DiffuseColour = new ColourValue(.25f, .25f, 0);
             light.SpecularColour = new ColourValue(.25f, .25f, 0);
             light.Direction = new Vector3(0, -1, -1);
 
             // Create the third light
             light = mgr.CreateLight("Light3");
             light.Type = Light.LightTypes.LT_SPOTLIGHT;
             light.DiffuseColour = ColourValue.Blue;
             light.SpecularColour = ColourValue.Blue;
             light.Direction = new Vector3(-1, -1, 0);
             light.Position = new Vector3(300, 300, 0);
             light.SetSpotlightRange(new Degree(35), new Degree(50));
         }
     }
 }
