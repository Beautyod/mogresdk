using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre.Demo.CelShading
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CelShading app = new CelShading();
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