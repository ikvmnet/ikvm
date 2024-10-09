using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IAssemblySymbolBuilder : ISymbolBuilder<IAssemblySymbol>, IAssemblySymbol
    {

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IModuleSymbolBuilder DefineModule(string name);

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IModuleSymbolBuilder DefineModule(string name, string fileName);

        /// <summary>
        /// Defines a named module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="emitSymbolInfo"></param>
        /// <returns></returns>
        IModuleSymbolBuilder DefineModule(string name, string fileName, bool emitSymbolInfo);

        /// <summary>
        /// Set a custom attribute using a custom attribute builder.
        /// </summary>
        /// <param name="customBuilder"></param>
        void SetCustomAttribute(ICustomAttributeBuilder customBuilder);

        /// <summary>
        /// Sets a custom attribute using a specified custom attribute blob.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="binaryAttribute"></param>
        void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute);

        /// <summary>
        /// Sets a Win32 icon on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        void DefineIconResource(byte[] bytes);

        /// <summary>
        /// Sets a manifest resource on the generated assembly.
        /// </summary>
        /// <param name="bytes"></param>
        void DefineManifestResource(byte[] bytes);

        /// <summary>
        /// Sets a Win32 version info resource on the generated assembly.
        /// </summary>
        void DefineVersionInfoResource();

        /// <summary>
        /// Sets the entry point for this assembly, assuming that a console application is being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        void SetEntryPoint(IMethodSymbol entryMethod);

        /// <summary>
        /// Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built.
        /// </summary>
        /// <param name="entryMethod"></param>
        /// <param name="fileKind"></param>
        void SetEntryPoint(IMethodSymbol entryMethod, PEFileKinds fileKind);

        /// <summary>
        /// Adds a forwarded type to this assembly.
        /// </summary>
        /// <param name="type"></param>
        void AddTypeForwarder(ITypeSymbol type);

        /// <summary>
        /// Adds an external resource file to the assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        void AddResourceFile(string name, string fileName);

        /// <summary>
        /// Saves this assembly to disk.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine);

    }

}
