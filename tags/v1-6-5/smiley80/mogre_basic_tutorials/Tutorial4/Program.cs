// http://www.ogre3d.org/wiki/index.php/Mogre_Basic_Tutorial_4

using System;
 using System.Collections.Generic;
 using System.Windows.Forms;
 using MogreFramework;
 using Mogre;
 using System.Drawing;
 
 namespace Tutorial04
 {
     #region class Program
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
     #endregion
 
     #region class MyOgreWindow
     class MyOgreWindow : OgreWindow
     {
         #region Fields
         const float TRANSLATE = 200;
         const float ROTATE = 0.2f;
         bool mRotating = false;
         Vector3 mTranslation = Vector3.ZERO;
         Point mLastPosition;
         #endregion
 
         #region Overridden Methods
         protected override void CreateInputHandler()
         {
             this.Root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
             this.KeyDown += new KeyEventHandler(KeyDownHandler);
             this.KeyUp += new KeyEventHandler(KeyUpHandler);
             this.MouseDown += new MouseEventHandler(MouseDownHandler);
             this.MouseUp += new MouseEventHandler(MouseUpHandler);
             this.MouseMove += new MouseEventHandler(MouseMoveHandler);
         }
         #endregion
 
         #region Mouse Handlers
         void MouseMoveHandler(object sender, MouseEventArgs e)
         {
             if (mRotating)
             {
                 float x = mLastPosition.X - Cursor.Position.X;
                 float y = mLastPosition.Y - Cursor.Position.Y;
 
                 Camera.Yaw(new Degree(x * ROTATE));
                 Camera.Pitch(new Degree(y * ROTATE));
 
                 mLastPosition = Cursor.Position;
             }
         }
 
         void MouseUpHandler(object sender, MouseEventArgs e)
         {
             if (e.Button == MouseButtons.Right)
             {
                 Cursor.Show();
                 mRotating = false;
             }
         }
 
         void MouseDownHandler(object sender, MouseEventArgs e)
         {
             if (e.Button == MouseButtons.Right)
             {
                 Cursor.Hide();
                 mRotating = true;
                 mLastPosition = Cursor.Position;
             }
         }
         #endregion
 
         #region Key Handlers
         void KeyUpHandler(object sender, KeyEventArgs e)
         {
             switch (e.KeyCode)
             {
                 case Keys.Up:
                 case Keys.W:
                 case Keys.Down:
                 case Keys.S:
                     mTranslation.z = 0;
                     break;
 
                 case Keys.Left:
                 case Keys.A:
                 case Keys.Right:
                 case Keys.D:
                     mTranslation.x = 0;
                     break;
 
                 case Keys.PageUp:
                 case Keys.Q:
                 case Keys.PageDown:
                 case Keys.E:
                     mTranslation.y = 0;
                     break;
             }
         }
 
         void KeyDownHandler(object sender, KeyEventArgs e)
         {
             switch (e.KeyCode)
             {
                 case Keys.Up:
                 case Keys.W:
                     mTranslation.z = -TRANSLATE;
                     break;
 
                 case Keys.Down:
                 case Keys.S:
                     mTranslation.z = TRANSLATE;
                     break;
 
                 case Keys.Left:
                 case Keys.A:
                     mTranslation.x = -TRANSLATE;
                     break;
 
                 case Keys.Right:
                 case Keys.D:
                     mTranslation.x = TRANSLATE;
                     break;
 
                 case Keys.PageUp:
                 case Keys.Q:
                     mTranslation.y = TRANSLATE;
                     break;
 
                 case Keys.PageDown:
                 case Keys.E:
                     mTranslation.y = -TRANSLATE;
                     break;
             }
         }
         #endregion
 
         #region Frame Handlers
         bool FrameStarted(FrameEvent evt)
         {
             Camera.Position += Camera.Orientation * mTranslation * evt.timeSinceLastFrame;
 
             return true;
         }
         #endregion
     }
     #endregion
 
     #region class SceneCreator
     class SceneCreator
     {
         public SceneCreator(OgreWindow win)
         {
             win.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
         }
 
         void SceneCreating(OgreWindow win)
         {
             win.SceneManager.AmbientLight = new ColourValue(0.25f, 0.25f, 0.25f);
 
             Entity ent = win.SceneManager.CreateEntity("Ninja", "ninja.mesh");
             win.SceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(ent);
 
             Light light = win.SceneManager.CreateLight("Light");
             light.Type = Light.LightTypes.LT_POINT;
             light.Position = new Vector3(250, 150, 250);
             light.DiffuseColour = ColourValue.White;
             light.SpecularColour = ColourValue.White;
 
             win.Camera.Position = new Vector3(0, 200, 400);
             win.Camera.LookAt(ent.BoundingBox.Center);
         }
     }
     #endregion
 }
