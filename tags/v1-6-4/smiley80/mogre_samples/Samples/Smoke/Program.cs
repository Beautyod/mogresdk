namespace Mogre.Demo.Smoke
{
    using System;

    using Mogre;

    class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            try
            {
                SmokeApplication app = new SmokeApplication();
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