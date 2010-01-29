// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_5

using System;
 using System.Collections.Generic;
 using System.Windows.Forms;
 using Mogre;
 using System.Drawing;
 
 namespace Tutorial05
 {
     static class Program
     {
         [STAThread]
         static void Main()
         {
             OgreStartup ogre = new OgreStartup();
             ogre.Go();
         }
     }
 
     class OgreStartup
     {
         Root mRoot = null;
         float ticks = 0;
 
         public void Go()
         {
             CreateRoot();
             DefineResources();
             SetupRenderSystem();
             CreateRenderWindow();
             InitResources();
             CreateScene();
             StartRenderLoop();
         }
 
         void CreateRoot()
         {
             mRoot = new Root();
         }
 
         void DefineResources()
         {
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
         }
 
         void SetupRenderSystem()
         {
             if (!mRoot.ShowConfigDialog())
                 throw new Exception("The user canceled the configuration dialog.");
 
             //// Setting up the RenderSystem manually.
             //RenderSystem rs = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
             //                                    // or use "OpenGL Rendering Subsystem"
             //mRoot.RenderSystem = rs;
             //rs.SetConfigOption("Full Screen", "No");
             //rs.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
         }
 
         void CreateRenderWindow()
         {
             mRoot.Initialise(true, "Main Ogre Window");
 
             //// Embedding ogre in a windows hWnd.  The "handle" variable holds the hWnd.
             //NameValuePairList misc = new NameValuePairList();
             //misc["externalWindowHandle"] = handle.ToString();
             //mWindow = mRoot.CreateRenderWindow("Main RenderWindow", 800, 600, false, misc);
         }
 
         void InitResources()
         {
             TextureManager.Singleton.DefaultNumMipmaps = 5;
             ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
         }
 
         void CreateScene()
         {
             SceneManager mgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
             Camera cam = mgr.CreateCamera("Camera");
             mRoot.AutoCreatedWindow.AddViewport(cam);
 
             Entity ent = mgr.CreateEntity("ninja", "ninja.mesh");
             mgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
 
             cam.Position = new Vector3(0, 200, -400);
             cam.LookAt(ent.BoundingBox.Center);
 
             mRoot.FrameEnded += new FrameListener.FrameEndedHandler(FrameEnded);
             ticks = Environment.TickCount;
         }
 
         void StartRenderLoop()
         {
             mRoot.StartRendering();
 
             //// Alternate Rendering Loop
             //while (mRoot.RenderOneFrame())
             //{
             //    // Do other things here, such as sleep for a short period of time
             //}
         }
 
         bool FrameEnded(FrameEvent evt)
         {
             if (Environment.TickCount - ticks > 5000)
                 return false;
 
             return true;
         }
     }
 }
