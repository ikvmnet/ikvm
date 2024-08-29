#nullable enable

namespace IKVM.CoreLib.Diagnostics
{

    internal abstract partial class DiagnosticEventHandler
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Found main method in class "{arg0}".
        /// </remarks>
        public void MainMethodFound(string arg0) => Report(Diagnostic.MainMethodFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Output file is "{arg0}".
        /// </remarks>
        public void OutputFileIs(string arg0) => Report(Diagnostic.OutputFileIs.Event((object[])[arg0]));

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Automatically adding reference to "{arg0}".
        /// </remarks>
        public void AutoAddRef(string arg0) => Report(Diagnostic.AutoAddRef.Event((object[])[arg0]));

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        public void MainMethodFromManifest(string arg0) => Report(Diagnostic.MainMethodFromManifest.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerInfo(string arg0) => Report(Diagnostic.GenericCompilerInfo.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingInfo(string arg0) => Report(Diagnostic.GenericClassLoadingInfo.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierInfo(string arg0) => Report(Diagnostic.GenericVerifierInfo.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeInfo(string arg0) => Report(Diagnostic.GenericRuntimeInfo.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniInfo(string arg0) => Report(Diagnostic.GenericJniInfo.Event((object[])[arg0]));

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class "{arg0}" not found.
        /// </remarks>
        public void ClassNotFound(string arg0) => Report(Diagnostic.ClassNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        public void ClassFormatError(string arg0, string arg1) => Report(Diagnostic.ClassFormatError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate class name: "{arg0}".
        /// </remarks>
        public void DuplicateClassName(string arg0) => Report(Diagnostic.DuplicateClassName.Event((object[])[arg0]));

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        public void IllegalAccessError(string arg0, string arg1) => Report(Diagnostic.IllegalAccessError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        public void VerificationError(string arg0, string arg1) => Report(Diagnostic.VerificationError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        public void NoClassDefFoundError(string arg0, string arg1) => Report(Diagnostic.NoClassDefFoundError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        public void GenericUnableToCompileError(string arg0, string arg1, string arg2) => Report(Diagnostic.GenericUnableToCompileError.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping resource (name clash): "{arg0}"
        /// </remarks>
        public void DuplicateResourceName(string arg0) => Report(Diagnostic.DuplicateResourceName.Event((object[])[arg0]));

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        public void SkippingReferencedClass(string arg0, string arg1) => Report(Diagnostic.SkippingReferencedClass.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime JNI assembly.
        /// </remarks>
        public void NoJniRuntime() => Report(Diagnostic.NoJniRuntime.Event((object[])[]));

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        public void EmittedNoClassDefFoundError(string arg0, string arg1) => Report(Diagnostic.EmittedNoClassDefFoundError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedIllegalAccessError(string arg0, string arg1) => Report(Diagnostic.EmittedIllegalAccessError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedInstantiationError(string arg0, string arg1) => Report(Diagnostic.EmittedInstantiationError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedIncompatibleClassChangeError(string arg0, string arg1) => Report(Diagnostic.EmittedIncompatibleClassChangeError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedNoSuchFieldError(string arg0, string arg1) => Report(Diagnostic.EmittedNoSuchFieldError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedAbstractMethodError(string arg0, string arg1) => Report(Diagnostic.EmittedAbstractMethodError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedNoSuchMethodError(string arg0, string arg1) => Report(Diagnostic.EmittedNoSuchMethodError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedLinkageError(string arg0, string arg1) => Report(Diagnostic.EmittedLinkageError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedVerificationError(string arg0, string arg1) => Report(Diagnostic.EmittedVerificationError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedClassFormatError(string arg0, string arg1) => Report(Diagnostic.EmittedClassFormatError.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        public void InvalidCustomAttribute(string arg0, string arg1) => Report(Diagnostic.InvalidCustomAttribute.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        public void IgnoredCustomAttribute(string arg0, string arg1) => Report(Diagnostic.IgnoredCustomAttribute.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        public void AssumeAssemblyVersionMatch(string arg0, string arg1) => Report(Diagnostic.AssumeAssemblyVersionMatch.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        public void InvalidDirectoryInLibOptionPath(string arg0) => Report(Diagnostic.InvalidDirectoryInLibOptionPath.Event((object[])[arg0]));

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        public void InvalidDirectoryInLibEnvironmentPath(string arg0) => Report(Diagnostic.InvalidDirectoryInLibEnvironmentPath.Event((object[])[arg0]));

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        public void LegacySearchRule(string arg0) => Report(Diagnostic.LegacySearchRule.Event((object[])[arg0]));

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        public void AssemblyLocationIgnored(string arg0, string arg1, string arg2) => Report(Diagnostic.AssemblyLocationIgnored.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void InterfaceMethodCantBeInternal(string arg0, string arg1, string arg2) => Report(Diagnostic.InterfaceMethodCantBeInternal.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate assembly reference "{arg0}"
        /// </remarks>
        public void DuplicateAssemblyReference(string arg0) => Report(Diagnostic.DuplicateAssemblyReference.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        public void UnableToResolveType(string arg0, string arg1, string arg2) => Report(Diagnostic.UnableToResolveType.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        public void StubsAreDeprecated(string arg0) => Report(Diagnostic.StubsAreDeprecated.Event((object[])[arg0]));

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        public void WrongClassName(string arg0, string arg1) => Report(Diagnostic.WrongClassName.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void ReflectionCallerClassRequiresCallerID(string arg0, string arg1, string arg2) => Report(Diagnostic.ReflectionCallerClassRequiresCallerID.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        public void LegacyAssemblyAttributesFound() => Report(Diagnostic.LegacyAssemblyAttributesFound.Event((object[])[]));

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create static lambda factory.
        /// </remarks>
        public void UnableToCreateLambdaFactory() => Report(Diagnostic.UnableToCreateLambdaFactory.Event((object[])[]));

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void UnknownWarning(string arg0) => Report(Diagnostic.UnknownWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public void DuplicateIkvmLangProperty(string arg0, string arg1) => Report(Diagnostic.DuplicateIkvmLangProperty.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public void MalformedIkvmLangProperty(string arg0, string arg1) => Report(Diagnostic.MalformedIkvmLangProperty.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerWarning(string arg0) => Report(Diagnostic.GenericCompilerWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingWarning(string arg0) => Report(Diagnostic.GenericClassLoadingWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierWarning(string arg0) => Report(Diagnostic.GenericVerifierWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeWarning(string arg0) => Report(Diagnostic.GenericRuntimeWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniWarning(string arg0) => Report(Diagnostic.GenericJniWarning.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        public void UnableToCreateProxy(string arg0, string arg1) => Report(Diagnostic.UnableToCreateProxy.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate proxy "{arg0}".
        /// </remarks>
        public void DuplicateProxy(string arg0) => Report(Diagnostic.DuplicateProxy.Event((object[])[arg0]));

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        public void MapXmlUnableToResolveOpCode(string arg0) => Report(Diagnostic.MapXmlUnableToResolveOpCode.Event((object[])[arg0]));

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error in remap file: {arg0}.
        /// </remarks>
        public void MapXmlError(string arg0) => Report(Diagnostic.MapXmlError.Event((object[])[arg0]));

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Source file '{arg0}' not found.
        /// </remarks>
        public void InputFileNotFound(string arg0) => Report(Diagnostic.InputFileNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown file type: {arg0}.
        /// </remarks>
        public void UnknownFileType(string arg0) => Report(Diagnostic.UnknownFileType.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public void UnknownElementInMapFile(string arg0, string arg1, string arg2) => Report(Diagnostic.UnknownElementInMapFile.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public void UnknownAttributeInMapFile(string arg0, string arg1, string arg2) => Report(Diagnostic.UnknownAttributeInMapFile.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        public void InvalidMemberNameInMapFile(string arg0, string arg1, string arg2) => Report(Diagnostic.InvalidMemberNameInMapFile.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        public void InvalidMemberSignatureInMapFile(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.InvalidMemberSignatureInMapFile.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public void InvalidPropertyNameInMapFile(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.InvalidPropertyNameInMapFile.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public void InvalidPropertySignatureInMapFile(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.InvalidPropertySignatureInMapFile.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        public void NonPrimaryAssemblyReference(string arg0, string arg1) => Report(Diagnostic.NonPrimaryAssemblyReference.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        public void MissingType(string arg0, string arg1) => Report(Diagnostic.MissingType.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public void MissingReference(string arg0, string arg1) => Report(Diagnostic.MissingReference.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void CallerSensitiveOnUnsupportedMethod(string arg0, string arg1, string arg2) => Report(Diagnostic.CallerSensitiveOnUnsupportedMethod.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        public void RemappedTypeMissingDefaultInterfaceMethod(string arg0, string arg1) => Report(Diagnostic.RemappedTypeMissingDefaultInterfaceMethod.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerError(string arg0) => Report(Diagnostic.GenericCompilerError.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingError(string arg0) => Report(Diagnostic.GenericClassLoadingError.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierError(string arg0) => Report(Diagnostic.GenericVerifierError.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeError(string arg0) => Report(Diagnostic.GenericRuntimeError.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniError(string arg0) => Report(Diagnostic.GenericJniError.Event((object[])[arg0]));

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Exporting previously imported assemblies is not supported.
        /// </remarks>
        public void ExportingImportsNotSupported() => Report(Diagnostic.ExportingImportsNotSupported.Event((object[])[]));

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Response file nesting depth exceeded.
        /// </remarks>
        public void ResponseFileDepthExceeded() => Report(Diagnostic.ResponseFileDepthExceeded.Event((object[])[]));

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorReadingFile(string arg0, string arg1) => Report(Diagnostic.ErrorReadingFile.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// No targets found
        /// </remarks>
        public void NoTargetsFound() => Report(Diagnostic.NoTargetsFound.Event((object[])[]));

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// File format limitation exceeded: {arg0}.
        /// </remarks>
        public void FileFormatLimitationExceeded(string arg0) => Report(Diagnostic.FileFormatLimitationExceeded.Event((object[])[arg0]));

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot specify both a key file and container.
        /// </remarks>
        public void CannotSpecifyBothKeyFileAndContainer() => Report(Diagnostic.CannotSpecifyBothKeyFileAndContainer.Event((object[])[]));

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot delay sign without a key file or container.
        /// </remarks>
        public void DelaySignRequiresKey() => Report(Diagnostic.DelaySignRequiresKey.Event((object[])[]));

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        public void InvalidStrongNameKeyPair(string arg0, string arg1) => Report(Diagnostic.InvalidStrongNameKeyPair.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference not found: {arg0}
        /// </remarks>
        public void ReferenceNotFound(string arg0) => Report(Diagnostic.ReferenceNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// You can only specify options before any child levels.
        /// </remarks>
        public void OptionsMustPreceedChildLevels() => Report(Diagnostic.OptionsMustPreceedChildLevels.Event((object[])[]));

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -target option.
        /// </remarks>
        public void UnrecognizedTargetType(string arg0) => Report(Diagnostic.UnrecognizedTargetType.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -platform option.
        /// </remarks>
        public void UnrecognizedPlatform(string arg0) => Report(Diagnostic.UnrecognizedPlatform.Event((object[])[arg0]));

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        public void UnrecognizedApartment(string arg0) => Report(Diagnostic.UnrecognizedApartment.Event((object[])[arg0]));

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Missing file specification for '{arg0}' option.
        /// </remarks>
        public void MissingFileSpecification(string arg0) => Report(Diagnostic.MissingFileSpecification.Event((object[])[arg0]));

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Path too long: {arg0}.
        /// </remarks>
        public void PathTooLong(string arg0) => Report(Diagnostic.PathTooLong.Event((object[])[arg0]));

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Path not found: {arg0}.
        /// </remarks>
        public void PathNotFound(string arg0) => Report(Diagnostic.PathNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid path: {arg0}.
        /// </remarks>
        public void InvalidPath(string arg0) => Report(Diagnostic.InvalidPath.Event((object[])[arg0]));

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid option: {arg0}.
        /// </remarks>
        public void InvalidOptionSyntax(string arg0) => Report(Diagnostic.InvalidOptionSyntax.Event((object[])[arg0]));

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file does not exist: {arg0}.
        /// </remarks>
        public void ExternalResourceNotFound(string arg0) => Report(Diagnostic.ExternalResourceNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file may not include path specification: {arg0}.
        /// </remarks>
        public void ExternalResourceNameInvalid(string arg0) => Report(Diagnostic.ExternalResourceNameInvalid.Event((object[])[arg0]));

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid version specified: {arg0}.
        /// </remarks>
        public void InvalidVersionFormat(string arg0) => Report(Diagnostic.InvalidVersionFormat.Event((object[])[arg0]));

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        public void InvalidFileAlignment(string arg0) => Report(Diagnostic.InvalidFileAlignment.Event((object[])[arg0]));

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorWritingFile(string arg0, string arg1) => Report(Diagnostic.ErrorWritingFile.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Unrecognized option: {arg0}.
        /// </remarks>
        public void UnrecognizedOption(string arg0) => Report(Diagnostic.UnrecognizedOption.Event((object[])[arg0]));

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// No output file specified.
        /// </remarks>
        public void NoOutputFileSpecified() => Report(Diagnostic.NoOutputFileSpecified.Event((object[])[]));

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        public void SharedClassLoaderCannotBeUsedOnModuleTarget() => Report(Diagnostic.SharedClassLoaderCannotBeUsedOnModuleTarget.Event((object[])[]));

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime assembly.
        /// </remarks>
        public void RuntimeNotFound() => Report(Diagnostic.RuntimeNotFound.Event((object[])[]));

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class cannot be specified for library or module.
        /// </remarks>
        public void MainClassRequiresExe() => Report(Diagnostic.MainClassRequiresExe.Event((object[])[]));

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// No main method found.
        /// </remarks>
        public void ExeRequiresMainClass() => Report(Diagnostic.ExeRequiresMainClass.Event((object[])[]));

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Properties cannot be specified for library or module.
        /// </remarks>
        public void PropertiesRequireExe() => Report(Diagnostic.PropertiesRequireExe.Event((object[])[]));

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Cannot specify assembly class loader for modules.
        /// </remarks>
        public void ModuleCannotHaveClassLoader() => Report(Diagnostic.ModuleCannotHaveClassLoader.Event((object[])[]));

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorParsingMapFile(string arg0, string arg1) => Report(Diagnostic.ErrorParsingMapFile.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Bootstrap classes missing and core assembly not found.
        /// </remarks>
        public void BootstrapClassesMissing() => Report(Diagnostic.BootstrapClassesMissing.Event((object[])[]));

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        public void StrongNameRequiresStrongNamedRefs() => Report(Diagnostic.StrongNameRequiresStrongNamedRefs.Event((object[])[]));

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class not found.
        /// </remarks>
        public void MainClassNotFound() => Report(Diagnostic.MainClassNotFound.Event((object[])[]));

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main method not found.
        /// </remarks>
        public void MainMethodNotFound() => Report(Diagnostic.MainMethodNotFound.Event((object[])[]));

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Redirected main method not supported.
        /// </remarks>
        public void UnsupportedMainMethod() => Report(Diagnostic.UnsupportedMainMethod.Event((object[])[]));

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// External main method must be public and in a public class.
        /// </remarks>
        public void ExternalMainNotAccessible() => Report(Diagnostic.ExternalMainNotAccessible.Event((object[])[]));

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class not found.
        /// </remarks>
        public void ClassLoaderNotFound() => Report(Diagnostic.ClassLoaderNotFound.Event((object[])[]));

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is not accessible.
        /// </remarks>
        public void ClassLoaderNotAccessible() => Report(Diagnostic.ClassLoaderNotAccessible.Event((object[])[]));

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is abstract.
        /// </remarks>
        public void ClassLoaderIsAbstract() => Report(Diagnostic.ClassLoaderIsAbstract.Event((object[])[]));

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        public void ClassLoaderNotClassLoader() => Report(Diagnostic.ClassLoaderNotClassLoader.Event((object[])[]));

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader constructor is missing.
        /// </remarks>
        public void ClassLoaderConstructorMissing() => Report(Diagnostic.ClassLoaderConstructorMissing.Event((object[])[]));

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public void MapFileTypeNotFound(string arg0) => Report(Diagnostic.MapFileTypeNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public void MapFileClassNotFound(string arg0) => Report(Diagnostic.MapFileClassNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Maximum error count reached.
        /// </remarks>
        public void MaximumErrorCountReached() => Report(Diagnostic.MaximumErrorCountReached.Event((object[])[]));

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Link error: {arg0}
        /// </remarks>
        public void LinkageError(string arg0) => Report(Diagnostic.LinkageError.Event((object[])[arg0]));

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly {arg0} was compiled with an incompatible IKVM.Runtime version. Current runtime: {arg1}. Referenced assembly runtime: {arg2}
        /// </remarks>
        public void RuntimeMismatch(string arg0, string arg1, string arg2) => Report(Diagnostic.RuntimeMismatch.Event((object[])[arg0, arg1, arg2]));

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
///
        /// </remarks>
        public void RuntimeMismatchStrongName() => Report(Diagnostic.RuntimeMismatchStrongName.Event((object[])[]));

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Failed to find core classes in core library.
        /// </remarks>
        public void CoreClassesMissing() => Report(Diagnostic.CoreClassesMissing.Event((object[])[]));

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load critical class '{arg0}'.
        /// </remarks>
        public void CriticalClassNotFound(string arg0) => Report(Diagnostic.CriticalClassNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        public void AssemblyContainsDuplicateClassNames(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.AssemblyContainsDuplicateClassNames.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        public void CallerIDRequiresHasCallerIDAnnotation() => Report(Diagnostic.CallerIDRequiresHasCallerIDAnnotation.Event((object[])[]));

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        public void UnableToResolveInterface(string arg0, string arg1) => Report(Diagnostic.UnableToResolveInterface.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        public void MissingBaseType(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.MissingBaseType.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public void MissingBaseTypeReference(string arg0, string arg1) => Report(Diagnostic.MissingBaseTypeReference.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// File not found: {arg0}.
        /// </remarks>
        public void FileNotFound(string arg0) => Report(Diagnostic.FileNotFound.Event((object[])[arg0]));

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Runtime method '{arg0}' not found.
        /// </remarks>
        public void RuntimeMethodMissing(string arg0) => Report(Diagnostic.RuntimeMethodMissing.Event((object[])[arg0]));

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        public void MapFileFieldNotFound(string arg0, string arg1) => Report(Diagnostic.MapFileFieldNotFound.Event((object[])[arg0, arg1]));

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        public void GhostInterfaceMethodMissing(string arg0, string arg1, string arg2, string arg3) => Report(Diagnostic.GhostInterfaceMethodMissing.Event((object[])[arg0, arg1, arg2, arg3]));

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        public void ModuleInitializerMethodRequirements(string arg1, string arg2, string arg3) => Report(Diagnostic.ModuleInitializerMethodRequirements.Event((object[])[arg1, arg2, arg3]));

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid zip: {name}.
        /// </remarks>
        public void InvalidZip(string name) => Report(Diagnostic.InvalidZip.Event((object[])[name]));

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeTrace(string arg0) => Report(Diagnostic.GenericRuntimeTrace.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniTrace(string arg0) => Report(Diagnostic.GenericJniTrace.Event((object[])[arg0]));

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerTrace(string arg0) => Report(Diagnostic.GenericCompilerTrace.Event((object[])[arg0]));

    }

}
