using System;
using System.Windows.Forms;

namespace Mogre.SDK.SampleBrowser
{
    /// <summary>
    /// Demo Windows Forms browser entry point.
    /// </summary>
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();
                Application.Run(new BrowserForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(BuildExceptionString(ex), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string BuildExceptionString( Exception exception )
        {
            var errMessage = string.Empty;

            errMessage += exception.Message + Environment.NewLine + exception.StackTrace;

            while ( exception.InnerException != null )
            {
                errMessage += BuildInnerExceptionString( exception.InnerException );
                exception = exception.InnerException;
            }

            return errMessage;
        }

        private static string BuildInnerExceptionString( Exception innerException )
        {
            var errMessage = string.Empty;

            errMessage += Environment.NewLine + " InnerException ";
            errMessage += Environment.NewLine + innerException.Message + Environment.NewLine + innerException.StackTrace;

            return errMessage;
        }
    }
}