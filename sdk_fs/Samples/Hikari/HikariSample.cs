using System;
using System.Collections.Generic;
using System.Text;
using HikariWrapper;

namespace Mogre.Demo.HikariSample
{
    class HikariSample : Mogre.Demo.ExampleApplication.Example
    {
        private HikariManager _HikariManager;
        private FlashControl _FpsControl, _ColorPickerControl;
        private CallbackDelegate _AsyncExitCallback, _AsyncOpacityCallback, _AsyncColorCallback;
        private bool ShouldQuit = false;

        public override bool UseBufferedInput
        {
            get
            {
                return true;
            }
        }

        public override bool IsMouseNonExclusive
        {
            get
            {
                return true;
            }
        }


        public override void CreateScene()
        {
            //create our hikarimanager
            _HikariManager = new HikariManager("..\\..\\Media\\gui");
            viewport.BackgroundColour = new ColourValue(200, 200, 200);

            //creation of a flash control, first we create an overlay, then we load a flash control onto it
            _FpsControl = _HikariManager.CreateFlashOverlay("Fps", viewport, 130, 91, RelativePosition.TopLeft, 0, 0);
            _FpsControl.Load("fps.swf");
            _FpsControl.SetTransparent(true);

            _ColorPickerControl = _HikariManager.CreateFlashOverlay("ColorPicker", viewport, 350, 400, RelativePosition.Center, 1, 0);
            _ColorPickerControl.Load("controls.swf");
            _ColorPickerControl.SetTransparent(true, true);

            //define delegate of csharp functions that we can call from flash
            _AsyncExitCallback = this.OnExitClick;
            _AsyncOpacityCallback = this.OnOpacityChange;
            _AsyncColorCallback = this.OnColorChange;
            //bind it to our flash control
            _ColorPickerControl.Bind("opacityChange", _AsyncOpacityCallback);
            _ColorPickerControl.Bind("colorChange", _AsyncColorCallback);
            _ColorPickerControl.Bind("exitClick", _AsyncExitCallback);

        }

        public override void CreateFrameListener()
        {
            this.root.FrameStarted += new FrameListener.FrameStartedHandler(root_FrameStarted);
            base.CreateFrameListener();
        }

        public override void CreateInput()
        {
            base.CreateInput();

            this.inputMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(inputMouse_MouseMoved);
            this.inputMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(inputMouse_MousePressed);
            this.inputMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(inputMouse_MouseReleased);
        }

        public bool inputMouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return _HikariManager.InjectMouseUp(id);
        }

        public bool inputMouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return _HikariManager.InjectMouseDown(id);
        }

        public bool inputMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            return _HikariManager.InjectMouseMove(arg.state.X.abs, arg.state.Y.abs);
        }

        public bool root_FrameStarted(FrameEvent evt)
        {
            HandleInput(evt);
            //mandatory call to update, without it, our overlay aren't refreshed
            _HikariManager.Update();
            RenderTarget.FrameStats stats = this.window.GetStatistics();

            object[] args = new object[1];
            args[0] = (int)stats.LastFPS;

            //call a flash function to update the number of fps
            _FpsControl.CallFunction("setFPS", args);


            return !ShouldQuit;
        }

        //callback function, called from flash using ExternalInterface.call

        public void OnExitClick(object[] args)
        {
            ShouldQuit = true;
        }

        public void OnOpacityChange(object[] args)
        {
            //we can call a function of our flash control within a callback function
            _ColorPickerControl.SetOpacity((float)args[0] / 100);
        }

        public void OnColorChange(object[] args)
        {
            viewport.BackgroundColour = Utilities.GetNumberAsColor((float)args[0]);
        }


    }
}
