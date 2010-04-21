namespace Mogre.Demo.RenderToTexture
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
                RenderToTextureApplication app = new RenderToTextureApplication();
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