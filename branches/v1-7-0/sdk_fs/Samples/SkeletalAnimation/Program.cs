using System;
using System.Collections;
using System.Collections.Generic;
using Mogre;

namespace Mogre.Demo.SkeletalAnimation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Skeletal app = new Skeletal();
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
