#nullable enable

using System.Diagnostics.Tracing;

namespace IKVM.CoreLib.Diagnostics.Tracing
{

    partial class DiagnosticEventSource
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Found main method in class "{arg0}".
        /// </remarks>
        [Event(1, Message = "Found main method in class \"{0}\".", Level = EventLevel.Informational)]
        public void MainMethodFound(string arg0) => WriteEvent(1, arg0);

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Output file is "{arg0}".
        /// </remarks>
        [Event(2, Message = "Output file is \"{0}\".", Level = EventLevel.Informational)]
        public void OutputFileIs(string arg0) => WriteEvent(2, arg0);

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Automatically adding reference to "{arg0}".
        /// </remarks>
        [Event(3, Message = "Automatically adding reference to \"{0}\".", Level = EventLevel.Informational)]
        public void AutoAddRef(string arg0) => WriteEvent(3, arg0);

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        [Event(4, Message = "Using main class \"{0}\" based on jar manifest.", Level = EventLevel.Informational)]
        public void MainMethodFromManifest(string arg0) => WriteEvent(4, arg0);

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(5, Message = "{0}", Level = EventLevel.Informational)]
        public void GenericCompilerInfo(string arg0) => WriteEvent(5, arg0);

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(6, Message = "{0}", Level = EventLevel.Informational)]
        public void GenericClassLoadingInfo(string arg0) => WriteEvent(6, arg0);

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(7, Message = "{0}", Level = EventLevel.Informational)]
        public void GenericVerifierInfo(string arg0) => WriteEvent(7, arg0);

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(8, Message = "{0}", Level = EventLevel.Informational)]
        public void GenericRuntimeInfo(string arg0) => WriteEvent(8, arg0);

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(9, Message = "{0}", Level = EventLevel.Informational)]
        public void GenericJniInfo(string arg0) => WriteEvent(9, arg0);

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class "{arg0}" not found.
        /// </remarks>
        [Event(100, Message = "Class \"{0}\" not found.", Level = EventLevel.Warning)]
        public void ClassNotFound(string arg0) => WriteEvent(100, arg0);

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        [Event(101, Message = "Unable to compile class \"{0}\". (class format error \"{1}\")", Level = EventLevel.Warning)]
        public void ClassFormatError(string arg0, string arg1) => WriteEvent(101, arg0, arg1);

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate class name: "{arg0}".
        /// </remarks>
        [Event(102, Message = "Duplicate class name: \"{0}\".", Level = EventLevel.Warning)]
        public void DuplicateClassName(string arg0) => WriteEvent(102, arg0);

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        [Event(103, Message = "Unable to compile class \"{0}\". (illegal access error \"{1}\")", Level = EventLevel.Warning)]
        public void IllegalAccessError(string arg0, string arg1) => WriteEvent(103, arg0, arg1);

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        [Event(104, Message = "Unable to compile class \"{0}\". (verification error \"{1}\")", Level = EventLevel.Warning)]
        public void VerificationError(string arg0, string arg1) => WriteEvent(104, arg0, arg1);

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        [Event(105, Message = "Unable to compile class \"{0}\". (missing class \"{1}\")", Level = EventLevel.Warning)]
        public void NoClassDefFoundError(string arg0, string arg1) => WriteEvent(105, arg0, arg1);

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        [Event(106, Message = "Unable to compile class \"{0}\". (\"{1}\": \"{2}\")", Level = EventLevel.Warning)]
        public void GenericUnableToCompileError(string arg0, string arg1, string arg2) => WriteEvent(106, arg0, arg1, arg2);

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping resource (name clash): "{arg0}"
        /// </remarks>
        [Event(107, Message = "Skipping resource (name clash): \"{0}\"", Level = EventLevel.Warning)]
        public void DuplicateResourceName(string arg0) => WriteEvent(107, arg0);

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        [Event(109, Message = "Skipping class: \"{0}\". (class is already available in referenced assembly \"{1}\")", Level = EventLevel.Warning)]
        public void SkippingReferencedClass(string arg0, string arg1) => WriteEvent(109, arg0, arg1);

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime JNI assembly.
        /// </remarks>
        [Event(110, Message = "Unable to load runtime JNI assembly.", Level = EventLevel.Warning)]
        public void NoJniRuntime() => WriteEvent(110);

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        [Event(111, Message = "Emitted java.lang.NoClassDefFoundError in \"{0}\". (\"{1}\").", Level = EventLevel.Warning)]
        public void EmittedNoClassDefFoundError(string arg0, string arg1) => WriteEvent(111, arg0, arg1);

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(112, Message = "Emitted java.lang.IllegalAccessError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedIllegalAccessError(string arg0, string arg1) => WriteEvent(112, arg0, arg1);

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(113, Message = "Emitted java.lang.InstantiationError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedInstantiationError(string arg0, string arg1) => WriteEvent(113, arg0, arg1);

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(114, Message = "Emitted java.lang.IncompatibleClassChangeError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedIncompatibleClassChangeError(string arg0, string arg1) => WriteEvent(114, arg0, arg1);

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(115, Message = "Emitted java.lang.NoSuchFieldError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedNoSuchFieldError(string arg0, string arg1) => WriteEvent(115, arg0, arg1);

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(116, Message = "Emitted java.lang.AbstractMethodError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedAbstractMethodError(string arg0, string arg1) => WriteEvent(116, arg0, arg1);

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(117, Message = "Emitted java.lang.NoSuchMethodError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedNoSuchMethodError(string arg0, string arg1) => WriteEvent(117, arg0, arg1);

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(118, Message = "Emitted java.lang.LinkageError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedLinkageError(string arg0, string arg1) => WriteEvent(118, arg0, arg1);

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(119, Message = "Emitted java.lang.VerificationError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedVerificationError(string arg0, string arg1) => WriteEvent(119, arg0, arg1);

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(120, Message = "Emitted java.lang.ClassFormatError in \"{0}\". (\"{1}\")", Level = EventLevel.Warning)]
        public void EmittedClassFormatError(string arg0, string arg1) => WriteEvent(120, arg0, arg1);

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        [Event(121, Message = "Error emitting \"{0}\" custom attribute. (\"{1}\")", Level = EventLevel.Warning)]
        public void InvalidCustomAttribute(string arg0, string arg1) => WriteEvent(121, arg0, arg1);

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        [Event(122, Message = "Custom attribute \"{0}\" was ignored. (\"{1}\")", Level = EventLevel.Warning)]
        public void IgnoredCustomAttribute(string arg0, string arg1) => WriteEvent(122, arg0, arg1);

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        [Event(123, Message = "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime p" +
    "olicy", Level = EventLevel.Warning)]
        public void AssumeAssemblyVersionMatch(string arg0, string arg1) => WriteEvent(123, arg0, arg1);

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        [Event(124, Message = "Directory \"{0}\" specified in -lib option is not valid.", Level = EventLevel.Warning)]
        public void InvalidDirectoryInLibOptionPath(string arg0) => WriteEvent(124, arg0);

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        [Event(125, Message = "Directory \"{0}\" specified in LIB environment is not valid.", Level = EventLevel.Warning)]
        public void InvalidDirectoryInLibEnvironmentPath(string arg0) => WriteEvent(125, arg0);

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        [Event(126, Message = "Found assembly \"{0}\" using legacy search rule, please append \'.dll\' to the refere" +
    "nce.", Level = EventLevel.Warning)]
        public void LegacySearchRule(string arg0) => WriteEvent(126, arg0);

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        [Event(127, Message = "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identi" +
    "ty \"{2}\".", Level = EventLevel.Warning)]
        public void AssemblyLocationIgnored(string arg0, string arg1, string arg2) => WriteEvent(127, arg0, arg1, arg2);

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        [Event(128, Message = "Ignoring @ikvm.lang.Internal annotation on interface method. (\"{0}.{1}{2}\")", Level = EventLevel.Warning)]
        public void InterfaceMethodCantBeInternal(string arg0, string arg1, string arg2) => WriteEvent(128, arg0, arg1, arg2);

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate assembly reference "{arg0}"
        /// </remarks>
        [Event(132, Message = "Duplicate assembly reference \"{0}\"", Level = EventLevel.Warning)]
        public void DuplicateAssemblyReference(string arg0) => WriteEvent(132, arg0);

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        [Event(133, Message = "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not " +
    "be found.", Level = EventLevel.Warning)]
        public void UnableToResolveType(string arg0, string arg1, string arg2) => WriteEvent(133, arg0, arg1, arg2);

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        [Event(134, Message = "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.", Level = EventLevel.Warning)]
        public void StubsAreDeprecated(string arg0) => WriteEvent(134, arg0);

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        [Event(135, Message = "Unable to compile \"{0}\" (wrong name: \"{1}\")", Level = EventLevel.Warning)]
        public void WrongClassName(string arg0, string arg1) => WriteEvent(135, arg0, arg1);

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        [Event(136, Message = "Reflection.getCallerClass() called from non-CallerID method. (\"{0}.{1}{2}\")", Level = EventLevel.Warning)]
        public void ReflectionCallerClassRequiresCallerID(string arg0, string arg1, string arg2) => WriteEvent(136, arg0, arg1, arg2);

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        [Event(137, Message = "Legacy assembly attributes container found. Please use the -assemblyattributes:<f" +
    "ile> option.", Level = EventLevel.Warning)]
        public void LegacyAssemblyAttributesFound() => WriteEvent(137);

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create static lambda factory.
        /// </remarks>
        [Event(138, Message = "Unable to create static lambda factory.", Level = EventLevel.Warning)]
        public void UnableToCreateLambdaFactory() => WriteEvent(138);

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(999, Message = "{0}", Level = EventLevel.Warning)]
        public void UnknownWarning(string arg0) => WriteEvent(999, arg0);

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        [Event(139, Message = "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}.", Level = EventLevel.Warning)]
        public void DuplicateIkvmLangProperty(string arg0, string arg1) => WriteEvent(139, arg0, arg1);

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        [Event(140, Message = "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}.", Level = EventLevel.Warning)]
        public void MalformedIkvmLangProperty(string arg0, string arg1) => WriteEvent(140, arg0, arg1);

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(141, Message = "{0}", Level = EventLevel.Warning)]
        public void GenericCompilerWarning(string arg0) => WriteEvent(141, arg0);

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(142, Message = "{0}", Level = EventLevel.Warning)]
        public void GenericClassLoadingWarning(string arg0) => WriteEvent(142, arg0);

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(143, Message = "{0}", Level = EventLevel.Warning)]
        public void GenericVerifierWarning(string arg0) => WriteEvent(143, arg0);

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(144, Message = "{0}", Level = EventLevel.Warning)]
        public void GenericRuntimeWarning(string arg0) => WriteEvent(144, arg0);

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(145, Message = "{0}", Level = EventLevel.Warning)]
        public void GenericJniWarning(string arg0) => WriteEvent(145, arg0);

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        [Event(4001, Message = "Unable to create proxy \"{0}\". (\"{1}\")", Level = EventLevel.Error)]
        public void UnableToCreateProxy(string arg0, string arg1) => WriteEvent(4001, arg0, arg1);

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate proxy "{arg0}".
        /// </remarks>
        [Event(4002, Message = "Duplicate proxy \"{0}\".", Level = EventLevel.Error)]
        public void DuplicateProxy(string arg0) => WriteEvent(4002, arg0);

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        [Event(4003, Message = "Unable to resolve opcode in remap file: {0}.", Level = EventLevel.Error)]
        public void MapXmlUnableToResolveOpCode(string arg0) => WriteEvent(4003, arg0);

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error in remap file: {arg0}.
        /// </remarks>
        [Event(4004, Message = "Error in remap file: {0}.", Level = EventLevel.Error)]
        public void MapXmlError(string arg0) => WriteEvent(4004, arg0);

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Source file '{arg0}' not found.
        /// </remarks>
        [Event(4005, Message = "Source file \'{0}\' not found.", Level = EventLevel.Error)]
        public void InputFileNotFound(string arg0) => WriteEvent(4005, arg0);

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown file type: {arg0}.
        /// </remarks>
        [Event(4006, Message = "Unknown file type: {0}.", Level = EventLevel.Error)]
        public void UnknownFileType(string arg0) => WriteEvent(4006, arg0);

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        [Event(4007, Message = "Unknown element {0} in remap file, line {1}, column {2}.", Level = EventLevel.Error)]
        public void UnknownElementInMapFile(string arg0, string arg1, string arg2) => WriteEvent(4007, arg0, arg1, arg2);

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        [Event(4008, Message = "Unknown attribute {0} in remap file, line {1}, column {2}.", Level = EventLevel.Error)]
        public void UnknownAttributeInMapFile(string arg0, string arg1, string arg2) => WriteEvent(4008, arg0, arg1, arg2);

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        [Event(4009, Message = "Invalid {0} name \'{1}\' in remap file in class {2}.", Level = EventLevel.Error)]
        public void InvalidMemberNameInMapFile(string arg0, string arg1, string arg2) => WriteEvent(4009, arg0, arg1, arg2);

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        [Event(4010, Message = "Invalid {0} signature \'{3}\' in remap file for {0} {1}.{2}.", Level = EventLevel.Error)]
        public void InvalidMemberSignatureInMapFile(string arg0, string arg1, string arg2, string arg3) => WriteEvent(4010, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        [Event(4011, Message = "Invalid property {0} name \'{3}\' in remap file for property {1}.{2}.", Level = EventLevel.Error)]
        public void InvalidPropertyNameInMapFile(string arg0, string arg1, string arg2, string arg3) => WriteEvent(4011, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        [Event(4012, Message = "Invalid property {0} signature \'{3}\' in remap file for property {1}.{2}.", Level = EventLevel.Error)]
        public void InvalidPropertySignatureInMapFile(string arg0, string arg1, string arg2, string arg3) => WriteEvent(4012, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        [Event(4013, Message = "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader gr" +
    "oup, please reference primary assembly \"{1}\" instead.", Level = EventLevel.Error)]
        public void NonPrimaryAssemblyReference(string arg0, string arg1) => WriteEvent(4013, arg0, arg1);

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        [Event(4014, Message = "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found." +
    "", Level = EventLevel.Error)]
        public void MissingType(string arg0, string arg1) => WriteEvent(4014, arg0, arg1);

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        [Event(4015, Message = "The type \'{0}\' is defined in an assembly that is notResponseFileDepthExceeded ref" +
    "erenced. You must add a reference to assembly \'{1}\'.", Level = EventLevel.Error)]
        public void MissingReference(string arg0, string arg1) => WriteEvent(4015, arg0, arg1);

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        [Event(4016, Message = "CallerSensitive annotation on unsupported method. (\"{0}.{1}{2}\")", Level = EventLevel.Error)]
        public void CallerSensitiveOnUnsupportedMethod(string arg0, string arg1, string arg2) => WriteEvent(4016, arg0, arg1, arg2);

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        [Event(4017, Message = "{0} does not implement default interface method {1}.", Level = EventLevel.Error)]
        public void RemappedTypeMissingDefaultInterfaceMethod(string arg0, string arg1) => WriteEvent(4017, arg0, arg1);

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(4018, Message = "{0}", Level = EventLevel.Error)]
        public void GenericCompilerError(string arg0) => WriteEvent(4018, arg0);

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(4019, Message = "{0}", Level = EventLevel.Error)]
        public void GenericClassLoadingError(string arg0) => WriteEvent(4019, arg0);

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(4020, Message = "{0}", Level = EventLevel.Error)]
        public void GenericVerifierError(string arg0) => WriteEvent(4020, arg0);

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(4021, Message = "{0}", Level = EventLevel.Error)]
        public void GenericRuntimeError(string arg0) => WriteEvent(4021, arg0);

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(4022, Message = "{0}", Level = EventLevel.Error)]
        public void GenericJniError(string arg0) => WriteEvent(4022, arg0);

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Exporting previously imported assemblies is not supported.
        /// </remarks>
        [Event(4023, Message = "Exporting previously imported assemblies is not supported.", Level = EventLevel.Error)]
        public void ExportingImportsNotSupported() => WriteEvent(4023);

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Response file nesting depth exceeded.
        /// </remarks>
        [Event(5000, Message = "Response file nesting depth exceeded.", Level = EventLevel.Critical)]
        public void ResponseFileDepthExceeded() => WriteEvent(5000);

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        [Event(5001, Message = "Unable to read file: {0}. ({1})", Level = EventLevel.Critical)]
        public void ErrorReadingFile(string arg0, string arg1) => WriteEvent(5001, arg0, arg1);

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// No targets found
        /// </remarks>
        [Event(5002, Message = "No targets found", Level = EventLevel.Critical)]
        public void NoTargetsFound() => WriteEvent(5002);

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// File format limitation exceeded: {arg0}.
        /// </remarks>
        [Event(5003, Message = "File format limitation exceeded: {0}.", Level = EventLevel.Critical)]
        public void FileFormatLimitationExceeded(string arg0) => WriteEvent(5003, arg0);

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot specify both a key file and container.
        /// </remarks>
        [Event(5004, Message = "You cannot specify both a key file and container.", Level = EventLevel.Critical)]
        public void CannotSpecifyBothKeyFileAndContainer() => WriteEvent(5004);

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot delay sign without a key file or container.
        /// </remarks>
        [Event(5005, Message = "You cannot delay sign without a key file or container.", Level = EventLevel.Critical)]
        public void DelaySignRequiresKey() => WriteEvent(5005);

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        [Event(5006, Message = "Invalid key {0} specified. (\"{1}\")", Level = EventLevel.Critical)]
        public void InvalidStrongNameKeyPair(string arg0, string arg1) => WriteEvent(5006, arg0, arg1);

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference not found: {arg0}
        /// </remarks>
        [Event(5007, Message = "Reference not found: {0}", Level = EventLevel.Critical)]
        public void ReferenceNotFound(string arg0) => WriteEvent(5007, arg0);

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// You can only specify options before any child levels.
        /// </remarks>
        [Event(5008, Message = "You can only specify options before any child levels.", Level = EventLevel.Critical)]
        public void OptionsMustPreceedChildLevels() => WriteEvent(5008);

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -target option.
        /// </remarks>
        [Event(5009, Message = "Invalid value \'{0}\' for -target option.", Level = EventLevel.Critical)]
        public void UnrecognizedTargetType(string arg0) => WriteEvent(5009, arg0);

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -platform option.
        /// </remarks>
        [Event(5010, Message = "Invalid value \'{0}\' for -platform option.", Level = EventLevel.Critical)]
        public void UnrecognizedPlatform(string arg0) => WriteEvent(5010, arg0);

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        [Event(5011, Message = "Invalid value \'{0}\' for -apartment option.", Level = EventLevel.Critical)]
        public void UnrecognizedApartment(string arg0) => WriteEvent(5011, arg0);

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Missing file specification for '{arg0}' option.
        /// </remarks>
        [Event(5012, Message = "Missing file specification for \'{0}\' option.", Level = EventLevel.Critical)]
        public void MissingFileSpecification(string arg0) => WriteEvent(5012, arg0);

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Path too long: {arg0}.
        /// </remarks>
        [Event(5013, Message = "Path too long: {0}.", Level = EventLevel.Critical)]
        public void PathTooLong(string arg0) => WriteEvent(5013, arg0);

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Path not found: {arg0}.
        /// </remarks>
        [Event(5014, Message = "Path not found: {0}.", Level = EventLevel.Critical)]
        public void PathNotFound(string arg0) => WriteEvent(5014, arg0);

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid path: {arg0}.
        /// </remarks>
        [Event(5015, Message = "Invalid path: {0}.", Level = EventLevel.Critical)]
        public void InvalidPath(string arg0) => WriteEvent(5015, arg0);

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid option: {arg0}.
        /// </remarks>
        [Event(5016, Message = "Invalid option: {0}.", Level = EventLevel.Critical)]
        public void InvalidOptionSyntax(string arg0) => WriteEvent(5016, arg0);

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file does not exist: {arg0}.
        /// </remarks>
        [Event(5017, Message = "External resource file does not exist: {0}.", Level = EventLevel.Critical)]
        public void ExternalResourceNotFound(string arg0) => WriteEvent(5017, arg0);

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file may not include path specification: {arg0}.
        /// </remarks>
        [Event(5018, Message = "External resource file may not include path specification: {0}.", Level = EventLevel.Critical)]
        public void ExternalResourceNameInvalid(string arg0) => WriteEvent(5018, arg0);

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid version specified: {arg0}.
        /// </remarks>
        [Event(5019, Message = "Invalid version specified: {0}.", Level = EventLevel.Critical)]
        public void InvalidVersionFormat(string arg0) => WriteEvent(5019, arg0);

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        [Event(5020, Message = "Invalid value \'{0}\' for -filealign option.", Level = EventLevel.Critical)]
        public void InvalidFileAlignment(string arg0) => WriteEvent(5020, arg0);

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        [Event(5021, Message = "Unable to write file: {0}. ({1})", Level = EventLevel.Critical)]
        public void ErrorWritingFile(string arg0, string arg1) => WriteEvent(5021, arg0, arg1);

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Unrecognized option: {arg0}.
        /// </remarks>
        [Event(5022, Message = "Unrecognized option: {0}.", Level = EventLevel.Critical)]
        public void UnrecognizedOption(string arg0) => WriteEvent(5022, arg0);

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// No output file specified.
        /// </remarks>
        [Event(5023, Message = "No output file specified.", Level = EventLevel.Critical)]
        public void NoOutputFileSpecified() => WriteEvent(5023);

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        [Event(5024, Message = "Incompatible options: -target:module and -sharedclassloader cannot be combined.", Level = EventLevel.Critical)]
        public void SharedClassLoaderCannotBeUsedOnModuleTarget() => WriteEvent(5024);

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime assembly.
        /// </remarks>
        [Event(5025, Message = "Unable to load runtime assembly.", Level = EventLevel.Critical)]
        public void RuntimeNotFound() => WriteEvent(5025);

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class cannot be specified for library or module.
        /// </remarks>
        [Event(5026, Message = "Main class cannot be specified for library or module.", Level = EventLevel.Critical)]
        public void MainClassRequiresExe() => WriteEvent(5026);

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// No main method found.
        /// </remarks>
        [Event(5027, Message = "No main method found.", Level = EventLevel.Critical)]
        public void ExeRequiresMainClass() => WriteEvent(5027);

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Properties cannot be specified for library or module.
        /// </remarks>
        [Event(5028, Message = "Properties cannot be specified for library or module.", Level = EventLevel.Critical)]
        public void PropertiesRequireExe() => WriteEvent(5028);

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Cannot specify assembly class loader for modules.
        /// </remarks>
        [Event(5029, Message = "Cannot specify assembly class loader for modules.", Level = EventLevel.Critical)]
        public void ModuleCannotHaveClassLoader() => WriteEvent(5029);

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        [Event(5030, Message = "Unable to parse remap file: {0}. ({1})", Level = EventLevel.Critical)]
        public void ErrorParsingMapFile(string arg0, string arg1) => WriteEvent(5030, arg0, arg1);

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Bootstrap classes missing and core assembly not found.
        /// </remarks>
        [Event(5031, Message = "Bootstrap classes missing and core assembly not found.", Level = EventLevel.Critical)]
        public void BootstrapClassesMissing() => WriteEvent(5031);

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        [Event(5032, Message = "All referenced assemblies must be strong named, to be able to sign the output ass" +
    "embly.", Level = EventLevel.Critical)]
        public void StrongNameRequiresStrongNamedRefs() => WriteEvent(5032);

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class not found.
        /// </remarks>
        [Event(5033, Message = "Main class not found.", Level = EventLevel.Critical)]
        public void MainClassNotFound() => WriteEvent(5033);

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main method not found.
        /// </remarks>
        [Event(5034, Message = "Main method not found.", Level = EventLevel.Critical)]
        public void MainMethodNotFound() => WriteEvent(5034);

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Redirected main method not supported.
        /// </remarks>
        [Event(5035, Message = "Redirected main method not supported.", Level = EventLevel.Critical)]
        public void UnsupportedMainMethod() => WriteEvent(5035);

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// External main method must be public and in a public class.
        /// </remarks>
        [Event(5036, Message = "External main method must be public and in a public class.", Level = EventLevel.Critical)]
        public void ExternalMainNotAccessible() => WriteEvent(5036);

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class not found.
        /// </remarks>
        [Event(5037, Message = "Custom assembly class loader class not found.", Level = EventLevel.Critical)]
        public void ClassLoaderNotFound() => WriteEvent(5037);

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is not accessible.
        /// </remarks>
        [Event(5038, Message = "Custom assembly class loader class is not accessible.", Level = EventLevel.Critical)]
        public void ClassLoaderNotAccessible() => WriteEvent(5038);

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is abstract.
        /// </remarks>
        [Event(5039, Message = "Custom assembly class loader class is abstract.", Level = EventLevel.Critical)]
        public void ClassLoaderIsAbstract() => WriteEvent(5039);

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        [Event(5040, Message = "Custom assembly class loader class does not extend java.lang.ClassLoader.", Level = EventLevel.Critical)]
        public void ClassLoaderNotClassLoader() => WriteEvent(5040);

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader constructor is missing.
        /// </remarks>
        [Event(5041, Message = "Custom assembly class loader constructor is missing.", Level = EventLevel.Critical)]
        public void ClassLoaderConstructorMissing() => WriteEvent(5041);

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        [Event(5042, Message = "Type \'{0}\' referenced in remap file was not found.", Level = EventLevel.Critical)]
        public void MapFileTypeNotFound(string arg0) => WriteEvent(5042, arg0);

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        [Event(5043, Message = "Class \'{0}\' referenced in remap file was not found.", Level = EventLevel.Critical)]
        public void MapFileClassNotFound(string arg0) => WriteEvent(5043, arg0);

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Maximum error count reached.
        /// </remarks>
        [Event(5044, Message = "Maximum error count reached.", Level = EventLevel.Critical)]
        public void MaximumErrorCountReached() => WriteEvent(5044);

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Link error: {arg0}
        /// </remarks>
        [Event(5045, Message = "Link error: {0}", Level = EventLevel.Critical)]
        public void LinkageError(string arg0) => WriteEvent(5045, arg0);

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly {arg0} was compiled with an incompatible IKVM.Runtime version. Current runtime: {arg1}. Referenced assembly runtime: {arg2}
        /// </remarks>
        [Event(5046, Message = "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version. C" +
    "urrent runtime: {1}. Referenced assembly runtime: {2}", Level = EventLevel.Critical)]
        public void RuntimeMismatch(string arg0, string arg1, string arg2) => WriteEvent(5046, arg0, arg1, arg2);

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
///
        /// </remarks>
        [Event(5047, Message = "", Level = EventLevel.Critical)]
        public void RuntimeMismatchStrongName() => WriteEvent(5047);

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Failed to find core classes in core library.
        /// </remarks>
        [Event(5048, Message = "Failed to find core classes in core library.", Level = EventLevel.Critical)]
        public void CoreClassesMissing() => WriteEvent(5048);

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load critical class '{arg0}'.
        /// </remarks>
        [Event(5049, Message = "Unable to load critical class \'{0}\'.", Level = EventLevel.Critical)]
        public void CriticalClassNotFound(string arg0) => WriteEvent(5049, arg0);

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        [Event(5050, Message = "Type \'{0}\' and \'{1}\' both map to the same name \'{2}\'. ({3})", Level = EventLevel.Critical)]
        public void AssemblyContainsDuplicateClassNames(string arg0, string arg1, string arg2, string arg3) => WriteEvent(5050, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        [Event(5051, Message = "CallerID.getCallerID() requires a HasCallerID annotation.", Level = EventLevel.Critical)]
        public void CallerIDRequiresHasCallerIDAnnotation() => WriteEvent(5051);

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        [Event(5052, Message = "Unable to resolve interface \'{0}\' on type \'{1}\'.", Level = EventLevel.Critical)]
        public void UnableToResolveInterface(string arg0, string arg1) => WriteEvent(5052, arg0, arg1);

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        [Event(5053, Message = "The base class or interface \'{0}\' in assembly \'{1}\' referenced by type \'{2}\' in \'" +
    "{3}\' could not be resolved.", Level = EventLevel.Critical)]
        public void MissingBaseType(string arg0, string arg1, string arg2, string arg3) => WriteEvent(5053, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        [Event(5054, Message = "The type \'{0}\' is defined in an assembly that is not referenced. You must add a r" +
    "eference to assembly \'{1}\'.", Level = EventLevel.Critical)]
        public void MissingBaseTypeReference(string arg0, string arg1) => WriteEvent(5054, arg0, arg1);

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// File not found: {arg0}.
        /// </remarks>
        [Event(5055, Message = "File not found: {0}.", Level = EventLevel.Critical)]
        public void FileNotFound(string arg0) => WriteEvent(5055, arg0);

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Runtime method '{arg0}' not found.
        /// </remarks>
        [Event(5056, Message = "Runtime method \'{0}\' not found.", Level = EventLevel.Critical)]
        public void RuntimeMethodMissing(string arg0) => WriteEvent(5056, arg0);

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        [Event(5057, Message = "Field \'{0}\' referenced in remap file was not found in class \'{1}\'.", Level = EventLevel.Critical)]
        public void MapFileFieldNotFound(string arg0, string arg1) => WriteEvent(5057, arg0, arg1);

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        [Event(5058, Message = "Remapped class \'{0}\' does not implement ghost interface method. ({1}.{2}{3})", Level = EventLevel.Critical)]
        public void GhostInterfaceMethodMissing(string arg0, string arg1, string arg2, string arg3) => WriteEvent(5058, arg0, arg1, arg2, arg3);

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        [Event(5059, Message = "Method \'{0}.{1}{2}\' does not meet the requirements of a module initializer.", Level = EventLevel.Critical)]
        public void ModuleInitializerMethodRequirements(string arg1, string arg2, string arg3) => WriteEvent(5059, arg1, arg2, arg3);

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid zip: {name}.
        /// </remarks>
        [Event(5060, Message = "Invalid zip: {0}.", Level = EventLevel.Critical)]
        public void InvalidZip(string name) => WriteEvent(5060, name);

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(6000, Message = "{0}", Level = EventLevel.Verbose)]
        public void GenericRuntimeTrace(string arg0) => WriteEvent(6000, arg0);

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(6001, Message = "{0}", Level = EventLevel.Verbose)]
        public void GenericJniTrace(string arg0) => WriteEvent(6001, arg0);

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        [Event(6002, Message = "{0}", Level = EventLevel.Verbose)]
        public void GenericCompilerTrace(string arg0) => WriteEvent(6002, arg0);

    }

}
