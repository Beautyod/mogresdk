
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mogre;

namespace Tutorial06
{
   public partial class OgreForm : Form
   {
      Root mRoot;
      RenderWindow mWindow;
      
      public OgreForm()
      {
     
         InitializeComponent();
         
         this.Size = new Size(800, 600);
         Disposed += new EventHandler(OgreForm_Disposed);
         Resize += new EventHandler(OgreForm_Resize);
      }
      
      void OgreForm_Resize(object sender, EventArgs e)
      {
         mWindow.WindowMovedOrResized();
      }
      
      void OgreForm_Disposed(object sender, EventArgs e)
      {
         mRoot.Dispose();
         mRoot = null;
      }
      
      public void Go()
      {
         Show();
         while (mRoot != null && mRoot.RenderOneFrame())
            Application.DoEvents();
      }
      
      public void Init()
      {
         // Create root object
         mRoot = new Root();
         
         // Define Resources
         ConfigFile cf = new ConfigFile();
         cf.Load("resources.cfg", "\t:=", true);
         ConfigFile.SectionIterator seci = cf.GetSectionIterator();
         String secName, typeName, archName;
         
         while (seci.MoveNext())
         {
            secName = seci.CurrentKey;
            ConfigFile.SettingsMultiMap settings = seci.Current;
            foreach (KeyValuePair<string, string> pair in settings)
            {
               typeName = pair.Key;
               archName = pair.Value;
               ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
            }
         }
         
         // Setup RenderSystem
         RenderSystem rs = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
         // or use "OpenGL Rendering Subsystem"
         mRoot.RenderSystem = rs;
         rs.SetConfigOption("Full Screen", "No");
         rs.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
         
         // Create Render Window
         mRoot.Initialise(false, "Main Ogre Window");
         NameValuePairList misc = new NameValuePairList();
         misc["externalWindowHandle"] = Handle.ToString();
         mWindow = mRoot.CreateRenderWindow("Main RenderWindow", 800, 600, false, misc);
         
         // Init resources
         TextureManager.Singleton.DefaultNumMipmaps = 5;
         ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
         
         // Create a Simple Scene
         SceneManager mgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
         Camera cam = mgr.CreateCamera("Camera");
         cam.AutoAspectRatio = true;
         mWindow.AddViewport(cam);
         
         Entity ent = mgr.CreateEntity("ninja", "ninja.mesh");
         mgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
         
         cam.Position = new Vector3(0, 200, -400);
         cam.LookAt(ent.BoundingBox.Center);
      }
   }
}
