#nullable enable

namespace IKVM.CoreLib.Diagnostics
{

    partial interface IDiagnosticHandler
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Found main method in class "{arg0}".
        /// </remarks>
        void MainMethodFound(string arg0);

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Output file is "{arg0}".
        /// </remarks>
        void OutputFileIs(string arg0);

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Automatically adding reference to "{arg0}".
        /// </remarks>
        void AutoAddRef(string arg0);

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        void MainMethodFromManifest(string arg0);

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        void GenericCompilerInfo(string arg0);

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        void GenericClassLoadingInfo(string arg0);

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        void GenericVerifierInfo(string arg0);

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        void GenericRuntimeInfo(string arg0);

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        void GenericJniInfo(string arg0);

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Class "{arg0}" not found.
        /// </remarks>
        void ClassNotFound(string arg0);

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        void ClassFormatError(string arg0, string arg1);

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Duplicate class name: "{arg0}".
        /// </remarks>
        void DuplicateClassName(string arg0);

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        void IllegalAccessError(string arg0, string arg1);

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        void VerificationError(string arg0, string arg1);

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        void NoClassDefFoundError(string arg0, string arg1);

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        void GenericUnableToCompileError(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Skipping resource (name clash): "{arg0}"
        /// </remarks>
        void DuplicateResourceName(string arg0);

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        void SkippingReferencedClass(string arg0, string arg1);

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to load runtime JNI assembly.
        /// </remarks>
        void NoJniRuntime();

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        void EmittedNoClassDefFoundError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedIllegalAccessError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedInstantiationError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedIncompatibleClassChangeError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedNoSuchFieldError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedAbstractMethodError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedNoSuchMethodError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedLinkageError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedVerificationError(string arg0, string arg1);

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        void EmittedClassFormatError(string arg0, string arg1);

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        void InvalidCustomAttribute(string arg0, string arg1);

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        void IgnoredCustomAttribute(string arg0, string arg1);

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        void AssumeAssemblyVersionMatch(string arg0, string arg1);

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        void InvalidDirectoryInLibOptionPath(string arg0);

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        void InvalidDirectoryInLibEnvironmentPath(string arg0);

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        void LegacySearchRule(string arg0);

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        void AssemblyLocationIgnored(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        void InterfaceMethodCantBeInternal(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Duplicate assembly reference "{arg0}"
        /// </remarks>
        void DuplicateAssemblyReference(string arg0);

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        void UnableToResolveType(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        void StubsAreDeprecated(string arg0);

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        void WrongClassName(string arg0, string arg1);

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        void ReflectionCallerClassRequiresCallerID(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        void LegacyAssemblyAttributesFound();

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to create static lambda factory.
        /// </remarks>
        void UnableToCreateLambdaFactory();

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void UnknownWarning(string arg0);

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        void DuplicateIkvmLangProperty(string arg0, string arg1);

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        void MalformedIkvmLangProperty(string arg0, string arg1);

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void GenericCompilerWarning(string arg0);

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void GenericClassLoadingWarning(string arg0);

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void GenericVerifierWarning(string arg0);

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void GenericRuntimeWarning(string arg0);

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        void GenericJniWarning(string arg0);

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        void UnableToCreateProxy(string arg0, string arg1);

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Duplicate proxy "{arg0}".
        /// </remarks>
        void DuplicateProxy(string arg0);

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        void MapXmlUnableToResolveOpCode(string arg0);

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Error in remap file: {arg0}.
        /// </remarks>
        void MapXmlError(string arg0);

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Source file '{arg0}' not found.
        /// </remarks>
        void InputFileNotFound(string arg0);

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown file type: {arg0}.
        /// </remarks>
        void UnknownFileType(string arg0);

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        void UnknownElementInMapFile(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        void UnknownAttributeInMapFile(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        void InvalidMemberNameInMapFile(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        void InvalidMemberSignatureInMapFile(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        void InvalidPropertyNameInMapFile(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        void InvalidPropertySignatureInMapFile(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        void NonPrimaryAssemblyReference(string arg0, string arg1);

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        void MissingType(string arg0, string arg1);

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        void MissingReference(string arg0, string arg1);

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        void CallerSensitiveOnUnsupportedMethod(string arg0, string arg1, string arg2);

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        void RemappedTypeMissingDefaultInterfaceMethod(string arg0, string arg1);

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        void GenericCompilerError(string arg0);

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        void GenericClassLoadingError(string arg0);

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        void GenericVerifierError(string arg0);

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        void GenericRuntimeError(string arg0);

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        void GenericJniError(string arg0);

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Exporting previously imported assemblies is not supported.
        /// </remarks>
        void ExportingImportsNotSupported();

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Response file nesting depth exceeded.
        /// </remarks>
        void ResponseFileDepthExceeded();

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        void ErrorReadingFile(string arg0, string arg1);

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No targets found
        /// </remarks>
        void NoTargetsFound();

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: File format limitation exceeded: {arg0}.
        /// </remarks>
        void FileFormatLimitationExceeded(string arg0);

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You cannot specify both a key file and container.
        /// </remarks>
        void CannotSpecifyBothKeyFileAndContainer();

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You cannot delay sign without a key file or container.
        /// </remarks>
        void DelaySignRequiresKey();

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        void InvalidStrongNameKeyPair(string arg0, string arg1);

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Reference not found: {arg0}
        /// </remarks>
        void ReferenceNotFound(string arg0);

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You can only specify options before any child levels.
        /// </remarks>
        void OptionsMustPreceedChildLevels();

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -target option.
        /// </remarks>
        void UnrecognizedTargetType(string arg0);

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -platform option.
        /// </remarks>
        void UnrecognizedPlatform(string arg0);

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        void UnrecognizedApartment(string arg0);

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Missing file specification for '{arg0}' option.
        /// </remarks>
        void MissingFileSpecification(string arg0);

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Path too long: {arg0}.
        /// </remarks>
        void PathTooLong(string arg0);

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Path not found: {arg0}.
        /// </remarks>
        void PathNotFound(string arg0);

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid path: {arg0}.
        /// </remarks>
        void InvalidPath(string arg0);

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid option: {arg0}.
        /// </remarks>
        void InvalidOptionSyntax(string arg0);

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External resource file does not exist: {arg0}.
        /// </remarks>
        void ExternalResourceNotFound(string arg0);

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External resource file may not include path specification: {arg0}.
        /// </remarks>
        void ExternalResourceNameInvalid(string arg0);

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid version specified: {arg0}.
        /// </remarks>
        void InvalidVersionFormat(string arg0);

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        void InvalidFileAlignment(string arg0);

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        void ErrorWritingFile(string arg0, string arg1);

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unrecognized option: {arg0}.
        /// </remarks>
        void UnrecognizedOption(string arg0);

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No output file specified.
        /// </remarks>
        void NoOutputFileSpecified();

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        void SharedClassLoaderCannotBeUsedOnModuleTarget();

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load runtime assembly.
        /// </remarks>
        void RuntimeNotFound();

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main class cannot be specified for library or module.
        /// </remarks>
        void MainClassRequiresExe();

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No main method found.
        /// </remarks>
        void ExeRequiresMainClass();

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Properties cannot be specified for library or module.
        /// </remarks>
        void PropertiesRequireExe();

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Cannot specify assembly class loader for modules.
        /// </remarks>
        void ModuleCannotHaveClassLoader();

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        void ErrorParsingMapFile(string arg0, string arg1);

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Bootstrap classes missing and core assembly not found.
        /// </remarks>
        void BootstrapClassesMissing();

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        void StrongNameRequiresStrongNamedRefs();

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main class not found.
        /// </remarks>
        void MainClassNotFound();

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main method not found.
        /// </remarks>
        void MainMethodNotFound();

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Redirected main method not supported.
        /// </remarks>
        void UnsupportedMainMethod();

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External main method must be public and in a public class.
        /// </remarks>
        void ExternalMainNotAccessible();

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class not found.
        /// </remarks>
        void ClassLoaderNotFound();

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class is not accessible.
        /// </remarks>
        void ClassLoaderNotAccessible();

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class is abstract.
        /// </remarks>
        void ClassLoaderIsAbstract();

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        void ClassLoaderNotClassLoader();

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader constructor is missing.
        /// </remarks>
        void ClassLoaderConstructorMissing();

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        void MapFileTypeNotFound(string arg0);

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        void MapFileClassNotFound(string arg0);

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Maximum error count reached.
        /// </remarks>
        void MaximumErrorCountReached();

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Link error: {arg0}
        /// </remarks>
        void LinkageError(string arg0);

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Referenced assembly {referencedAssemblyPath} was compiled with an incompatible IKVM.Runtime version. Current runtime: {runtimeAssemblyName}. Referenced assembly runtime: {referencedAssemblyName}
        /// </remarks>
        void RuntimeMismatch(string referencedAssemblyPath, string runtimeAssemblyName, string referencedAssemblyName);

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal:
        /// </remarks>
        void RuntimeMismatchStrongName();

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Failed to find core classes in core library.
        /// </remarks>
        void CoreClassesMissing();

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load critical class '{arg0}'.
        /// </remarks>
        void CriticalClassNotFound(string arg0);

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        void AssemblyContainsDuplicateClassNames(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        void CallerIDRequiresHasCallerIDAnnotation();

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        void UnableToResolveInterface(string arg0, string arg1);

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        void MissingBaseType(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        void MissingBaseTypeReference(string arg0, string arg1);

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: File not found: {arg0}.
        /// </remarks>
        void FileNotFound(string arg0);

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Runtime method '{arg0}' not found.
        /// </remarks>
        void RuntimeMethodMissing(string arg0);

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        void MapFileFieldNotFound(string arg0, string arg1);

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        void GhostInterfaceMethodMissing(string arg0, string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        void ModuleInitializerMethodRequirements(string arg1, string arg2, string arg3);

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid zip: {name}.
        /// </remarks>
        void InvalidZip(string name);

        /// <summary>
        /// The 'CoreAssemblyVersionMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load assembly '{0}' as it depends on a higher version of {1} than the one currently loaded.
        /// </remarks>
        void CoreAssemblyVersionMismatch(string arg0, string arg1);

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        void GenericRuntimeTrace(string arg0);

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        void GenericJniTrace(string arg0);

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        void GenericCompilerTrace(string arg0);

    }

}
