using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace Mogre.Demo.Fresnel
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Fresnel app = new Fresnel();
                app.Go();
            }
            catch(System.Runtime.InteropServices.SEHException) 
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
