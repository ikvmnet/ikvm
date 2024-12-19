namespace IKVM.Tools.Importer
{

    enum DebugMode
    {

        /// <summary>
        /// Emit no debugging information.
        /// </summary>
        None,

        /// <summary>
        /// Emit debugging information to to .pdb file using cross-platform Portable PDB format
        /// </summary>
        Portable,

        /// <summary>
        /// Emit debugging information into the .dll/.exe itself (.pdb file is not produced) using Portable PDB format.
        /// </summary>
        Embedded,

    }

}
