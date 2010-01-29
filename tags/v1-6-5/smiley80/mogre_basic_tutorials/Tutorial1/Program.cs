// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_1

using System;
 using System.Collections.Generic;
 using System.Windows.Forms;
 using MogreFramework;
 using Mogre;
 
 namespace Tutorial01
 {
     static class Program
     {
         [STAThread]
         static void Main()
         {
             try
             {
                 OgreWindow win = new OgreWindow();
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
 
     class SceneCreator
     {
         public SceneCreator(OgreWindow win)
         {
             win.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
         }
 
         void SceneCreating(OgreWindow win)
         {
             // Setting the ambient light
             SceneManager mgr = win.SceneManager;
             mgr.AmbientLight = new ColourValue(1, 1, 1);
 
             // Adding Objects
             Entity ent = mgr.CreateEntity("Robot", "robot.mesh");
             SceneNode node = mgr.RootSceneNode.CreateChildSceneNode("RobotNode");
             node.AttachObject(ent);
 
             Entity ent2 = mgr.CreateEntity("Robot2", "robot.mesh");
             SceneNode node2 = mgr.RootSceneNode.CreateChildSceneNode("RobotNode2", new Vector3(50, 0, 0));
             node2.AttachObject(ent2);
 
             // Translating Objects
             node2.Position += new Vector3(10, 0, 10);
             node.Position += new Vector3(25, 0, 0);
 
             // Scaling Objects
             node.Scale(.5f, 1, 2);
             node2.Scale(1, 2, 1);
 
             // Rotating Objects
             Entity ent3 = mgr.CreateEntity("Robot3", "robot.mesh");
             SceneNode node3 = mgr.RootSceneNode.CreateChildSceneNode("RobotNode3", new Vector3(100, 0, 0));
             node3.AttachObject(ent3);
 
             node.Pitch(new Degree(90));
             node2.Yaw(new Degree(90));
             node3.Roll(new Degree(90));
         }
     }
 }
