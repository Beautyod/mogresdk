// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_3

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MogreFramework;
using Mogre;

namespace Tutorial03
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
      protected override void CreateSceneManager()
      {
         SceneManager = Root.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
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
         win.SceneManager.SetWorldGeometry("terrain.cfg");
         
         //win.SceneManager.SetSkyBox(true, "Examples/SpaceSkyBox");
         //win.SceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 10);
         //win.SceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 5000, false);
         //win.SceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 100, true);
         
         //win.SceneManager.SetSkyDome(true, "Examples/CloudySky", 5, 8);
         //win.SceneManager.SetSkyDome(false, "");
         
         Plane plane;
         plane.d = 1000;
         plane.normal = Vector3.NEGATIVE_UNIT_Y;
         
         //win.SceneManager.SetSkyPlane(true, plane, "Examples/SpaceSkyPlane", 1500, 75);
         //win.SceneManager.SetSkyPlane(true, plane, "Examples/SpaceSkyPlane", 1500, 50, true, 1.5f, 150, 150);
         //win.SceneManager.SetSkyPlane(true, plane, "Examples/CloudySky", 1500, 40, true, 1.5f, 150, 150);
         //win.SceneManager.SetSkyPlane(false, new Plane(), "");
         
         //ColourValue fadeColour = new ColourValue(0.9f, 0.9f, 0.9f);
         //win.Viewport.BackgroundColour = fadeColour;
         
         //win.SceneManager.SetFog(FogMode.FOG_LINEAR, fadeColour, 0, 50, 500);
         //win.SceneManager.SetFog(FogMode.FOG_EXP, fadeColour, 0.005f);
         //win.SceneManager.SetFog(FogMode.FOG_EXP, fadeColour, 0.003f);
      }
   }
}
