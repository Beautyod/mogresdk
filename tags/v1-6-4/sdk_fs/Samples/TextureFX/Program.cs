using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.TextureFX
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TextureFXApp app = new TextureFXApp();
                app.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}
