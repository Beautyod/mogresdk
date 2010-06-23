namespace DSMogre
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        #region Methods

        #region Public Static Methods

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, int length);

        #endregion Public Static Methods

        #endregion Methods
    }
}