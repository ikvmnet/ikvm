namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Specifies the type of the portable executable (PE) file.
    /// </summary>
    public enum PEFileKinds
    {

        /// <summary>
        /// The portable executable (PE) file is a DLL.
        /// </summary>
        Dll = 1,

        /// <summary>
        /// The application is a console (not a Windows-based) application.
        /// </summary>
        ConsoleApplication = 2,

        /// <summary>
        /// The application is a Windows-based application.
        /// </summary>
        WindowApplication = 3,


    }

}
