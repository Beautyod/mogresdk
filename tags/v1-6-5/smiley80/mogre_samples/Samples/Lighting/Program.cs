namespace Mogre.Demo.Lighting
{
    using System;
    using System.Collections.Generic;

    using Mogre;

    class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            try
            {
                LightingApplication app = new LightingApplication();
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

        #endregion Methods
    }
}