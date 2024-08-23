using System.Diagnostics.Tracing;

namespace IKVM.CoreLib.Diagnostics.Tracing
{

    public partial class DiagnosticEventSource
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        [Event(1, Message = Found main method in class "{0}", Level = EventLevel.Informational)]
        public void MainMethodFound(object arg1) => WriteEvent(1, arg1);

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        [Event(2, Message = Output file is "{0}", Level = EventLevel.Informational)]
        public void OutputFileIs(object arg1) => WriteEvent(2, arg1);

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        [Event(3, Message = Automatically adding reference to "{0}", Level = EventLevel.Informational)]
        public void AutoAddRef(object arg1) => WriteEvent(3, arg1);

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        [Event(4, Message = Using main class "{0}" based on jar manifest, Level = EventLevel.Informational)]
        public void MainMethodFromManifest(object arg1) => WriteEvent(4, arg1);

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        [Event(5, Message = {0}, Level = EventLevel.Informational)]
        public void GenericCompilerInfo(object arg1) => WriteEvent(5, arg1);

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Informational)]
        public void GenericClassLoadingInfo(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Informational)]
        public void GenericVerifierInfo(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        [Event(7, Message = {0}, Level = EventLevel.Informational)]
        public void GenericRuntimeInfo(object arg1) => WriteEvent(7, arg1);

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Informational)]
        public void GenericJniInfo(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        [Event(100, Message = Class "{0}" not found, Level = EventLevel.Warning)]
        public void ClassNotFound(object arg1) => WriteEvent(100, arg1);

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        [Event(101, Message = Unable to compile class "{0}" 
    (class format error "{1}"), Level = EventLevel.Warning)]
        public void ClassFormatError(object arg1, object arg2) => WriteEvent(101, arg1, arg2);

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        [Event(102, Message = Duplicate class name: "{0}", Level = EventLevel.Warning)]
        public void DuplicateClassName(object arg1) => WriteEvent(102, arg1);

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        [Event(103, Message = Unable to compile class "{0}" 
    (illegal access error "{1}"), Level = EventLevel.Warning)]
        public void IllegalAccessError(object arg1, object arg2) => WriteEvent(103, arg1, arg2);

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        [Event(104, Message = Unable to compile class "{0}" 
    (verification error "{1}"), Level = EventLevel.Warning)]
        public void VerificationError(object arg1, object arg2) => WriteEvent(104, arg1, arg2);

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        [Event(105, Message = Unable to compile class "{0}" 
    (missing class "{1}"), Level = EventLevel.Warning)]
        public void NoClassDefFoundError(object arg1, object arg2) => WriteEvent(105, arg1, arg2);

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        [Event(106, Message = Unable to compile class "{0}" 
    ("{1}": "{2}"), Level = EventLevel.Warning)]
        public void GenericUnableToCompileError(object arg1, object arg2, object arg3) => WriteEvent(106, arg1, arg2, arg3);

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        [Event(107, Message = Skipping resource (name clash): "{0}", Level = EventLevel.Warning)]
        public void DuplicateResourceName(object arg1) => WriteEvent(107, arg1);

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        [Event(109, Message = Skipping class: "{0}"
    (class is already available in referenced assembly "{1}"), Level = EventLevel.Warning)]
        public void SkippingReferencedClass(object arg1, object arg2) => WriteEvent(109, arg1, arg2);

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        [Event(110, Message = Unable to load runtime JNI assembly, Level = EventLevel.Warning)]
        public void NoJniRuntime() => WriteEvent(110);

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        [Event(111, Message = Emitted java.lang.NoClassDefFoundError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedNoClassDefFoundError(object arg1, object arg2) => WriteEvent(111, arg1, arg2);

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        [Event(112, Message = Emitted java.lang.IllegalAccessError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedIllegalAccessError(object arg1, object arg2) => WriteEvent(112, arg1, arg2);

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        [Event(113, Message = Emitted java.lang.InstantiationError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedInstantiationError(object arg1, object arg2) => WriteEvent(113, arg1, arg2);

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        [Event(114, Message = Emitted java.lang.IncompatibleClassChangeError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedIncompatibleClassChangeError(object arg1, object arg2) => WriteEvent(114, arg1, arg2);

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        [Event(115, Message = Emitted java.lang.NoSuchFieldError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedNoSuchFieldError(object arg1, object arg2) => WriteEvent(115, arg1, arg2);

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        [Event(116, Message = Emitted java.lang.AbstractMethodError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedAbstractMethodError(object arg1, object arg2) => WriteEvent(116, arg1, arg2);

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        [Event(117, Message = Emitted java.lang.NoSuchMethodError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedNoSuchMethodError(object arg1, object arg2) => WriteEvent(117, arg1, arg2);

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        [Event(118, Message = Emitted java.lang.LinkageError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedLinkageError(object arg1, object arg2) => WriteEvent(118, arg1, arg2);

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        [Event(119, Message = Emitted java.lang.VerificationError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedVerificationError(object arg1, object arg2) => WriteEvent(119, arg1, arg2);

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        [Event(120, Message = Emitted java.lang.ClassFormatError in "{0}"
    ("{1}"), Level = EventLevel.Warning)]
        public void EmittedClassFormatError(object arg1, object arg2) => WriteEvent(120, arg1, arg2);

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        [Event(121, Message = Error emitting "{0}" custom attribute
    ("{1}"), Level = EventLevel.Warning)]
        public void InvalidCustomAttribute(object arg1, object arg2) => WriteEvent(121, arg1, arg2);

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        [Event(122, Message = Custom attribute "{0}" was ignored
    ("{1}"), Level = EventLevel.Warning)]
        public void IgnoredCustomAttribute(object arg1, object arg2) => WriteEvent(122, arg1, arg2);

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        [Event(123, Message = Assuming assembly reference "{0}" matches "{1}", you may need to supply runtime policy, Level = EventLevel.Warning)]
        public void AssumeAssemblyVersionMatch(object arg1, object arg2) => WriteEvent(123, arg1, arg2);

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        [Event(124, Message = Directory "{0}" specified in -lib option is not valid, Level = EventLevel.Warning)]
        public void InvalidDirectoryInLibOptionPath(object arg1) => WriteEvent(124, arg1);

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        [Event(125, Message = Directory "{0}" specified in LIB environment is not valid, Level = EventLevel.Warning)]
        public void InvalidDirectoryInLibEnvironmentPath(object arg1) => WriteEvent(125, arg1);

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        [Event(126, Message = Found assembly "{0}" using legacy search rule, please append '.dll' to the reference, Level = EventLevel.Warning)]
        public void LegacySearchRule(object arg1) => WriteEvent(126, arg1);

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        [Event(127, Message = Assembly "{0}" is ignored as previously loaded assembly "{1}" has the same identity "{2}", Level = EventLevel.Warning)]
        public void AssemblyLocationIgnored(object arg1, object arg2, object arg3) => WriteEvent(127, arg1, arg2, arg3);

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        [Event(128, Message = Ignoring @ikvm.lang.Internal annotation on interface method
    ("{0}.{1}{2}"), Level = EventLevel.Warning)]
        public void InterfaceMethodCantBeInternal(object arg1, object arg2, object arg3) => WriteEvent(128, arg1, arg2, arg3);

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        [Event(132, Message = Duplicate assembly reference "{0}", Level = EventLevel.Warning)]
        public void DuplicateAssemblyReference(object arg1) => WriteEvent(132, arg1);

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        [Event(133, Message = Reference in "{0}" to type "{1}" claims it is defined in "{2}", but it could not be found, Level = EventLevel.Warning)]
        public void UnableToResolveType(object arg1, object arg2, object arg3) => WriteEvent(133, arg1, arg2, arg3);

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        [Event(134, Message = Compiling stubs is deprecated. Please add a reference to assembly "{0}" instead., Level = EventLevel.Warning)]
        public void StubsAreDeprecated(object arg1) => WriteEvent(134, arg1);

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        [Event(135, Message = Unable to compile "{0}" (wrong name: "{1}"), Level = EventLevel.Warning)]
        public void WrongClassName(object arg1, object arg2) => WriteEvent(135, arg1, arg2);

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        [Event(136, Message = Reflection.getCallerClass() called from non-CallerID method
    ("{0}.{1}{2}"), Level = EventLevel.Warning)]
        public void ReflectionCallerClassRequiresCallerID(object arg1, object arg2, object arg3) => WriteEvent(136, arg1, arg2, arg3);

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        [Event(137, Message = Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option., Level = EventLevel.Warning)]
        public void LegacyAssemblyAttributesFound() => WriteEvent(137);

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        [Event(138, Message = Unable to create static lambda factory., Level = EventLevel.Warning)]
        public void UnableToCreateLambdaFactory() => WriteEvent(138);

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        [Event(999, Message = {0}, Level = EventLevel.Warning)]
        public void UnknownWarning(object arg1) => WriteEvent(999, arg1);

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        [Event(139, Message = Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}, Level = EventLevel.Warning)]
        public void DuplicateIkvmLangProperty(object arg1, object arg2) => WriteEvent(139, arg1, arg2);

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        [Event(140, Message = Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}, Level = EventLevel.Warning)]
        public void MalformedIkvmLangProperty(object arg1, object arg2) => WriteEvent(140, arg1, arg2);

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        [Event(5, Message = {0}, Level = EventLevel.Warning)]
        public void GenericCompilerWarning(object arg1) => WriteEvent(5, arg1);

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Warning)]
        public void GenericClassLoadingWarning(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Warning)]
        public void GenericVerifierWarning(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        [Event(7, Message = {0}, Level = EventLevel.Warning)]
        public void GenericRuntimeWarning(object arg1) => WriteEvent(7, arg1);

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        [Event(6, Message = {0}, Level = EventLevel.Warning)]
        public void GenericJniWarning(object arg1) => WriteEvent(6, arg1);

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        [Event(4001, Message = Unable to create proxy "{0}"
    ("{1}"), Level = EventLevel.Error)]
        public void UnableToCreateProxy(object arg1, object arg2) => WriteEvent(4001, arg1, arg2);

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        [Event(4002, Message = Duplicate proxy "{0}", Level = EventLevel.Error)]
        public void DuplicateProxy(object arg1) => WriteEvent(4002, arg1);

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        [Event(4003, Message = Unable to resolve opcode in remap file: {0}, Level = EventLevel.Error)]
        public void MapXmlUnableToResolveOpCode(object arg1) => WriteEvent(4003, arg1);

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        [Event(4004, Message = Error in remap file: {0}, Level = EventLevel.Error)]
        public void MapXmlError(object arg1) => WriteEvent(4004, arg1);

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        [Event(4005, Message = Source file '{0}' not found, Level = EventLevel.Error)]
        public void InputFileNotFound(object arg1) => WriteEvent(4005, arg1);

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        [Event(4006, Message = Unknown file type: {0}, Level = EventLevel.Error)]
        public void UnknownFileType(object arg1) => WriteEvent(4006, arg1);

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        [Event(4007, Message = Unknown element {0} in remap file, line {1}, column {2}, Level = EventLevel.Error)]
        public void UnknownElementInMapFile(object arg1, object arg2, object arg3) => WriteEvent(4007, arg1, arg2, arg3);

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        [Event(4008, Message = Unknown attribute {0} in remap file, line {1}, column {2}, Level = EventLevel.Error)]
        public void UnknownAttributeInMapFile(object arg1, object arg2, object arg3) => WriteEvent(4008, arg1, arg2, arg3);

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        [Event(4009, Message = Invalid {0} name '{1}' in remap file in class {2}, Level = EventLevel.Error)]
        public void InvalidMemberNameInMapFile(object arg1, object arg2, object arg3) => WriteEvent(4009, arg1, arg2, arg3);

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        [Event(4010, Message = Invalid {0} signature '{3}' in remap file for {0} {1}.{2}, Level = EventLevel.Error)]
        public void InvalidMemberSignatureInMapFile(object arg1, object arg2, object arg3, object arg4) => WriteEvent(4010, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        [Event(4011, Message = Invalid property {0} name '{3}' in remap file for property {1}.{2}, Level = EventLevel.Error)]
        public void InvalidPropertyNameInMapFile(object arg1, object arg2, object arg3, object arg4) => WriteEvent(4011, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        [Event(4012, Message = Invalid property {0} signature '{3}' in remap file for property {1}.{2}, Level = EventLevel.Error)]
        public void InvalidPropertySignatureInMapFile(object arg1, object arg2, object arg3, object arg4) => WriteEvent(4012, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        [Event(4013, Message = Referenced assembly "{0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{1}" instead, Level = EventLevel.Error)]
        public void NonPrimaryAssemblyReference(object arg1, object arg2) => WriteEvent(4013, arg1, arg2);

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        [Event(4014, Message = Reference to type "{0}" claims it is defined in "{1}", but it could not be found, Level = EventLevel.Error)]
        public void MissingType(object arg1, object arg2) => WriteEvent(4014, arg1, arg2);

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        [Event(4015, Message = The type '{0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{1}', Level = EventLevel.Error)]
        public void MissingReference(object arg1, object arg2) => WriteEvent(4015, arg1, arg2);

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        [Event(4016, Message = CallerSensitive annotation on unsupported method
    ("{0}.{1}{2}"), Level = EventLevel.Error)]
        public void CallerSensitiveOnUnsupportedMethod(object arg1, object arg2, object arg3) => WriteEvent(4016, arg1, arg2, arg3);

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        [Event(4017, Message = {0} does not implement default interface method {1}, Level = EventLevel.Error)]
        public void RemappedTypeMissingDefaultInterfaceMethod(object arg1, object arg2) => WriteEvent(4017, arg1, arg2);

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        [Event(4018, Message = {0}, Level = EventLevel.Error)]
        public void GenericCompilerError(object arg1) => WriteEvent(4018, arg1);

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        [Event(4019, Message = {0}, Level = EventLevel.Error)]
        public void GenericClassLoadingError(object arg1) => WriteEvent(4019, arg1);

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        [Event(4020, Message = {0}, Level = EventLevel.Error)]
        public void GenericVerifierError(object arg1) => WriteEvent(4020, arg1);

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        [Event(4021, Message = {0}, Level = EventLevel.Error)]
        public void GenericRuntimeError(object arg1) => WriteEvent(4021, arg1);

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        [Event(4022, Message = {0}, Level = EventLevel.Error)]
        public void GenericJniError(object arg1) => WriteEvent(4022, arg1);

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        [Event(5000, Message = Response file nesting depth exceeded, Level = EventLevel.Fatal)]
        public void ResponseFileDepthExceeded() => WriteEvent(5000);

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        [Event(5001, Message = Unable to read file: {0}
	({1}), Level = EventLevel.Fatal)]
        public void ErrorReadingFile(object arg1, object arg2) => WriteEvent(5001, arg1, arg2);

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        [Event(5002, Message = No targets found, Level = EventLevel.Fatal)]
        public void NoTargetsFound() => WriteEvent(5002);

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        [Event(5003, Message = File format limitation exceeded: {0}, Level = EventLevel.Fatal)]
        public void FileFormatLimitationExceeded(object arg1) => WriteEvent(5003, arg1);

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        [Event(5004, Message = You cannot specify both a key file and container, Level = EventLevel.Fatal)]
        public void CannotSpecifyBothKeyFileAndContainer() => WriteEvent(5004);

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        [Event(5005, Message = You cannot delay sign without a key file or container, Level = EventLevel.Fatal)]
        public void DelaySignRequiresKey() => WriteEvent(5005);

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        [Event(5006, Message = Invalid key {0} specified.
	("{1}"), Level = EventLevel.Fatal)]
        public void InvalidStrongNameKeyPair(object arg1, object arg2) => WriteEvent(5006, arg1, arg2);

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        [Event(5007, Message = Reference not found: {0}, Level = EventLevel.Fatal)]
        public void ReferenceNotFound(object arg1) => WriteEvent(5007, arg1);

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        [Event(5008, Message = You can only specify options before any child levels, Level = EventLevel.Fatal)]
        public void OptionsMustPreceedChildLevels() => WriteEvent(5008);

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        [Event(5009, Message = Invalid value '{0}' for -target option, Level = EventLevel.Fatal)]
        public void UnrecognizedTargetType(object arg1) => WriteEvent(5009, arg1);

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        [Event(5010, Message = Invalid value '{0}' for -platform option, Level = EventLevel.Fatal)]
        public void UnrecognizedPlatform(object arg1) => WriteEvent(5010, arg1);

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        [Event(5011, Message = Invalid value '{0}' for -apartment option, Level = EventLevel.Fatal)]
        public void UnrecognizedApartment(object arg1) => WriteEvent(5011, arg1);

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        [Event(5012, Message = Missing file specification for '{0}' option, Level = EventLevel.Fatal)]
        public void MissingFileSpecification(object arg1) => WriteEvent(5012, arg1);

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        [Event(5013, Message = Path too long: {0}, Level = EventLevel.Fatal)]
        public void PathTooLong(object arg1) => WriteEvent(5013, arg1);

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        [Event(5014, Message = Path not found: {0}, Level = EventLevel.Fatal)]
        public void PathNotFound(object arg1) => WriteEvent(5014, arg1);

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        [Event(5015, Message = Invalid path: {0}, Level = EventLevel.Fatal)]
        public void InvalidPath(object arg1) => WriteEvent(5015, arg1);

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        [Event(5016, Message = Invalid option: {0}, Level = EventLevel.Fatal)]
        public void InvalidOptionSyntax(object arg1) => WriteEvent(5016, arg1);

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        [Event(5017, Message = External resource file does not exist: {0}, Level = EventLevel.Fatal)]
        public void ExternalResourceNotFound(object arg1) => WriteEvent(5017, arg1);

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        [Event(5018, Message = External resource file may not include path specification: {0}, Level = EventLevel.Fatal)]
        public void ExternalResourceNameInvalid(object arg1) => WriteEvent(5018, arg1);

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        [Event(5019, Message = Invalid version specified: {0}, Level = EventLevel.Fatal)]
        public void InvalidVersionFormat(object arg1) => WriteEvent(5019, arg1);

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        [Event(5020, Message = Invalid value '{0}' for -filealign option, Level = EventLevel.Fatal)]
        public void InvalidFileAlignment(object arg1) => WriteEvent(5020, arg1);

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        [Event(5021, Message = Unable to write file: {0}
	({1}), Level = EventLevel.Fatal)]
        public void ErrorWritingFile(object arg1, object arg2) => WriteEvent(5021, arg1, arg2);

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        [Event(5022, Message = Unrecognized option: {0}, Level = EventLevel.Fatal)]
        public void UnrecognizedOption(object arg1) => WriteEvent(5022, arg1);

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        [Event(5023, Message = No output file specified, Level = EventLevel.Fatal)]
        public void NoOutputFileSpecified() => WriteEvent(5023);

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        [Event(5024, Message = Incompatible options: -target:module and -sharedclassloader cannot be combined, Level = EventLevel.Fatal)]
        public void SharedClassLoaderCannotBeUsedOnModuleTarget() => WriteEvent(5024);

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        [Event(5025, Message = Unable to load runtime assembly, Level = EventLevel.Fatal)]
        public void RuntimeNotFound() => WriteEvent(5025);

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        [Event(5026, Message = Main class cannot be specified for library or module, Level = EventLevel.Fatal)]
        public void MainClassRequiresExe() => WriteEvent(5026);

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        [Event(5027, Message = No main method found, Level = EventLevel.Fatal)]
        public void ExeRequiresMainClass() => WriteEvent(5027);

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        [Event(5028, Message = Properties cannot be specified for library or module, Level = EventLevel.Fatal)]
        public void PropertiesRequireExe() => WriteEvent(5028);

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        [Event(5029, Message = Cannot specify assembly class loader for modules, Level = EventLevel.Fatal)]
        public void ModuleCannotHaveClassLoader() => WriteEvent(5029);

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        [Event(5030, Message = Unable to parse remap file: {0}
	({1}), Level = EventLevel.Fatal)]
        public void ErrorParsingMapFile(object arg1, object arg2) => WriteEvent(5030, arg1, arg2);

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        [Event(5031, Message = Bootstrap classes missing and core assembly not found, Level = EventLevel.Fatal)]
        public void BootstrapClassesMissing() => WriteEvent(5031);

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        [Event(5032, Message = All referenced assemblies must be strong named, to be able to sign the output assembly, Level = EventLevel.Fatal)]
        public void StrongNameRequiresStrongNamedRefs() => WriteEvent(5032);

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        [Event(5033, Message = Main class not found, Level = EventLevel.Fatal)]
        public void MainClassNotFound() => WriteEvent(5033);

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        [Event(5034, Message = Main method not found, Level = EventLevel.Fatal)]
        public void MainMethodNotFound() => WriteEvent(5034);

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        [Event(5035, Message = Redirected main method not supported, Level = EventLevel.Fatal)]
        public void UnsupportedMainMethod() => WriteEvent(5035);

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        [Event(5036, Message = External main method must be public and in a public class, Level = EventLevel.Fatal)]
        public void ExternalMainNotAccessible() => WriteEvent(5036);

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        [Event(5037, Message = Custom assembly class loader class not found, Level = EventLevel.Fatal)]
        public void ClassLoaderNotFound() => WriteEvent(5037);

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        [Event(5038, Message = Custom assembly class loader class is not accessible, Level = EventLevel.Fatal)]
        public void ClassLoaderNotAccessible() => WriteEvent(5038);

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        [Event(5039, Message = Custom assembly class loader class is abstract, Level = EventLevel.Fatal)]
        public void ClassLoaderIsAbstract() => WriteEvent(5039);

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        [Event(5040, Message = Custom assembly class loader class does not extend java.lang.ClassLoader, Level = EventLevel.Fatal)]
        public void ClassLoaderNotClassLoader() => WriteEvent(5040);

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        [Event(5041, Message = Custom assembly class loader constructor is missing, Level = EventLevel.Fatal)]
        public void ClassLoaderConstructorMissing() => WriteEvent(5041);

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        [Event(5042, Message = Type '{0}' referenced in remap file was not found, Level = EventLevel.Fatal)]
        public void MapFileTypeNotFound(object arg1) => WriteEvent(5042, arg1);

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        [Event(5043, Message = Class '{0}' referenced in remap file was not found, Level = EventLevel.Fatal)]
        public void MapFileClassNotFound(object arg1) => WriteEvent(5043, arg1);

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        [Event(5044, Message = Maximum error count reached, Level = EventLevel.Fatal)]
        public void MaximumErrorCountReached() => WriteEvent(5044);

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        [Event(5045, Message = Link error: {0}, Level = EventLevel.Fatal)]
        public void LinkageError(object arg1) => WriteEvent(5045, arg1);

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        [Event(5046, Message = Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version
	Current runtime: {1}
	Referenced assembly runtime: {2}, Level = EventLevel.Fatal)]
        public void RuntimeMismatch(object arg1, object arg2, object arg3) => WriteEvent(5046, arg1, arg2, arg3);

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        [Event(5047, Message = , Level = EventLevel.Fatal)]
        public void RuntimeMismatchStrongName() => WriteEvent(5047);

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        [Event(5048, Message = Failed to find core classes in core library, Level = EventLevel.Fatal)]
        public void CoreClassesMissing() => WriteEvent(5048);

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        [Event(5049, Message = Unable to load critical class '{0}', Level = EventLevel.Fatal)]
        public void CriticalClassNotFound(object arg1) => WriteEvent(5049, arg1);

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        [Event(5050, Message = Type '{0}' and '{1}' both map to the same name '{2}'
	({3}), Level = EventLevel.Fatal)]
        public void AssemblyContainsDuplicateClassNames(object arg1, object arg2, object arg3, object arg4) => WriteEvent(5050, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        [Event(5051, Message = CallerID.getCallerID() requires a HasCallerID annotation, Level = EventLevel.Fatal)]
        public void CallerIDRequiresHasCallerIDAnnotation() => WriteEvent(5051);

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        [Event(5052, Message = Unable to resolve interface '{0}' on type '{1}', Level = EventLevel.Fatal)]
        public void UnableToResolveInterface(object arg1, object arg2) => WriteEvent(5052, arg1, arg2);

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        [Event(5053, Message = The base class or interface '{0}' in assembly '{1}' referenced by type '{2}' in '{3}' could not be resolved, Level = EventLevel.Fatal)]
        public void MissingBaseType(object arg1, object arg2, object arg3, object arg4) => WriteEvent(5053, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        [Event(5054, Message = The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}', Level = EventLevel.Fatal)]
        public void MissingBaseTypeReference(object arg1, object arg2) => WriteEvent(5054, arg1, arg2);

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        [Event(5055, Message = File not found: {0}, Level = EventLevel.Fatal)]
        public void FileNotFound(object arg1) => WriteEvent(5055, arg1);

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        [Event(5056, Message = Runtime method '{0}' not found, Level = EventLevel.Fatal)]
        public void RuntimeMethodMissing(object arg1) => WriteEvent(5056, arg1);

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        [Event(5057, Message = Field '{0}' referenced in remap file was not found in class '{1}', Level = EventLevel.Fatal)]
        public void MapFileFieldNotFound(object arg1, object arg2) => WriteEvent(5057, arg1, arg2);

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        [Event(5058, Message = Remapped class '{0}' does not implement ghost interface method
	({1}.{2}{3}), Level = EventLevel.Fatal)]
        public void GhostInterfaceMethodMissing(object arg1, object arg2, object arg3, object arg4) => WriteEvent(5058, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        [Event(5059, Message = Method '{1}.{2}{3}' does not meet the requirements of a module initializer., Level = EventLevel.Fatal)]
        public void ModuleInitializerMethodRequirements(object arg1, object arg2, object arg3, object arg4) => WriteEvent(5059, arg1, arg2, arg3, arg4);

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        [Event(6000, Message = {0}, Level = EventLevel.Trace)]
        public void GenericRuntimeTrace(object arg1) => WriteEvent(6000, arg1);

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        [Event(6001, Message = {0}, Level = EventLevel.Trace)]
        public void GenericJniTrace(object arg1) => WriteEvent(6001, arg1);

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        [Event(6002, Message = {0}, Level = EventLevel.Trace)]
        public void GenericCompilerTrace(object arg1) => WriteEvent(6002, arg1);

    }

}

