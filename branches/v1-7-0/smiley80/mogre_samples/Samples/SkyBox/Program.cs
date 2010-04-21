namespace Mogre.Demo.SkyBox
{
    using System;

    class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            try
            {
                SkyBoxApplication app = new SkyBoxApplication();
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