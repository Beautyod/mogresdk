
using System;
using Mogre;

namespace Mogre.Demo.SkyPlane
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SkyPlaneApplication app = new SkyPlaneApplication();
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