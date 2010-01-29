/*
 * CelShading demo
 * Contributor: grizzley90
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Mogre.Demo.CelShading
{
    class CelShading : Mogre.Demo.ExampleApplication.Example
    {
        const uint CUSTOM_SHININESS = 1;
        const uint CUSTOM_DIFFUSE = 2;
        const uint CUSTOM_SPECULAR = 3;

        Mogre.SceneNode rootNode;

        public override void CreateScene()
        {
             // Check capabilities
          RenderSystemCapabilities caps = Root.Singleton.RenderSystem.Capabilities;
            if (!caps.HasCapability(Mogre.Capabilities.RSC_VERTEX_PROGRAM) || !(caps.HasCapability(Mogre.Capabilities.RSC_FRAGMENT_PROGRAM)))
            {
                MessageBox.Show("Your card does not support vertex and fragment programs, so cannot run this demo. Sorry! CelShading::createScene");
            }

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // Add light to the scene node
            rootNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            rootNode.CreateChildSceneNode(new Vector3(20,40,50)).AttachObject(l);

            Entity ent = sceneMgr.CreateEntity("head", "ogrehead.mesh");

            camera.Position = new Vector3(20, 0, 100);
            camera.LookAt( new Vector3(0,0,0));


            // Set common material, but define custom parameters to change colours
            // See Example-Advanced.material for how these are finally bound to GPU parameters
            SubEntity sub;
            // eyes
            sub = ent.GetSubEntity(0);
            sub.MaterialName = ("Examples/CelShading");
            sub.SetCustomParameter(CUSTOM_SHININESS, new Vector4(35.0f, 0.0f, 0.0f, 0.0f));
            sub.SetCustomParameter(CUSTOM_DIFFUSE, new Vector4(1.0f, 0.3f, 0.3f, 1.0f));
            sub.SetCustomParameter(CUSTOM_SPECULAR, new Vector4(1.0f, 0.6f, 0.6f, 1.0f));
            // skin
            sub = ent.GetSubEntity(1);
            sub.MaterialName = ("Examples/CelShading");
            sub.SetCustomParameter(CUSTOM_SHININESS, new Vector4(10.0f, 0.0f, 0.0f, 0.0f));
            sub.SetCustomParameter(CUSTOM_DIFFUSE, new Vector4(0.0f, 0.5f, 0.0f, 1.0f));
            sub.SetCustomParameter(CUSTOM_SPECULAR, new Vector4(0.3f, 0.5f, 0.3f, 1.0f));
            // earring
            sub = ent.GetSubEntity(2);
            sub.MaterialName = ("Examples/CelShading");
            sub.SetCustomParameter(CUSTOM_SHININESS, new Vector4(25.0f, 0.0f, 0.0f, 0.0f));
            sub.SetCustomParameter(CUSTOM_DIFFUSE, new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
            sub.SetCustomParameter(CUSTOM_SPECULAR, new Vector4(1.0f, 1.0f, 0.7f, 1.0f));
            // teeth
            sub = ent.GetSubEntity(3);
            sub.MaterialName = ("Examples/CelShading");
            sub.SetCustomParameter(CUSTOM_SHININESS, new Vector4(20.0f, 0.0f, 0.0f, 0.0f));
            sub.SetCustomParameter(CUSTOM_DIFFUSE, new Vector4(1.0f, 1.0f, 0.7f, 1.0f));
            sub.SetCustomParameter(CUSTOM_SPECULAR, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

            // Add entity to the root scene node
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);

            window.GetViewport(0).BackgroundColour = (Mogre.ColourValue.White);
       
        }

        bool CellShading_FrameStarted(FrameEvent evt)
        {
            rootNode.Yaw(new Degree(evt.timeSinceLastFrame * 30));
            return true;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            root.FrameStarted += new FrameListener.FrameStartedHandler(CellShading_FrameStarted);
        }
    }
} 