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
        public void MainMethodFound(string arg0)
        {
            if (IsEnabled(Diagnostic.MainMethodFound))
                Report(Diagnostic.MainMethodFound.Event([arg0]));
        }

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Output file is "{arg0}".
        /// </remarks>
        public void OutputFileIs(string arg0)
        {
            if (IsEnabled(Diagnostic.OutputFileIs))
                Report(Diagnostic.OutputFileIs.Event([arg0]));
        }

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Automatically adding reference to "{arg0}".
        /// </remarks>
        public void AutoAddRef(string arg0)
        {
            if (IsEnabled(Diagnostic.AutoAddRef))
                Report(Diagnostic.AutoAddRef.Event([arg0]));
        }

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        public void MainMethodFromManifest(string arg0)
        {
            if (IsEnabled(Diagnostic.MainMethodFromManifest))
                Report(Diagnostic.MainMethodFromManifest.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerInfo(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericCompilerInfo))
                Report(Diagnostic.GenericCompilerInfo.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingInfo(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericClassLoadingInfo))
                Report(Diagnostic.GenericClassLoadingInfo.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierInfo(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericVerifierInfo))
                Report(Diagnostic.GenericVerifierInfo.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeInfo(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericRuntimeInfo))
                Report(Diagnostic.GenericRuntimeInfo.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniInfo(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericJniInfo))
                Report(Diagnostic.GenericJniInfo.Event([arg0]));
        }

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class "{arg0}" not found.
        /// </remarks>
        public void ClassNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.ClassNotFound))
                Report(Diagnostic.ClassNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        public void ClassFormatError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.ClassFormatError))
                Report(Diagnostic.ClassFormatError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate class name: "{arg0}".
        /// </remarks>
        public void DuplicateClassName(string arg0)
        {
            if (IsEnabled(Diagnostic.DuplicateClassName))
                Report(Diagnostic.DuplicateClassName.Event([arg0]));
        }

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        public void IllegalAccessError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.IllegalAccessError))
                Report(Diagnostic.IllegalAccessError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        public void VerificationError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.VerificationError))
                Report(Diagnostic.VerificationError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        public void NoClassDefFoundError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.NoClassDefFoundError))
                Report(Diagnostic.NoClassDefFoundError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        public void GenericUnableToCompileError(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.GenericUnableToCompileError))
                Report(Diagnostic.GenericUnableToCompileError.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping resource (name clash): "{arg0}"
        /// </remarks>
        public void DuplicateResourceName(string arg0)
        {
            if (IsEnabled(Diagnostic.DuplicateResourceName))
                Report(Diagnostic.DuplicateResourceName.Event([arg0]));
        }

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        public void SkippingReferencedClass(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.SkippingReferencedClass))
                Report(Diagnostic.SkippingReferencedClass.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime JNI assembly.
        /// </remarks>
        public void NoJniRuntime()
        {
            if (IsEnabled(Diagnostic.NoJniRuntime))
                Report(Diagnostic.NoJniRuntime.Event([]));
        }

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        public void EmittedNoClassDefFoundError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedNoClassDefFoundError))
                Report(Diagnostic.EmittedNoClassDefFoundError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedIllegalAccessError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedIllegalAccessError))
                Report(Diagnostic.EmittedIllegalAccessError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedInstantiationError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedInstantiationError))
                Report(Diagnostic.EmittedInstantiationError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedIncompatibleClassChangeError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedIncompatibleClassChangeError))
                Report(Diagnostic.EmittedIncompatibleClassChangeError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedNoSuchFieldError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedNoSuchFieldError))
                Report(Diagnostic.EmittedNoSuchFieldError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedAbstractMethodError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedAbstractMethodError))
                Report(Diagnostic.EmittedAbstractMethodError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedNoSuchMethodError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedNoSuchMethodError))
                Report(Diagnostic.EmittedNoSuchMethodError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedLinkageError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedLinkageError))
                Report(Diagnostic.EmittedLinkageError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedVerificationError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedVerificationError))
                Report(Diagnostic.EmittedVerificationError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        public void EmittedClassFormatError(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.EmittedClassFormatError))
                Report(Diagnostic.EmittedClassFormatError.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        public void InvalidCustomAttribute(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.InvalidCustomAttribute))
                Report(Diagnostic.InvalidCustomAttribute.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        public void IgnoredCustomAttribute(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.IgnoredCustomAttribute))
                Report(Diagnostic.IgnoredCustomAttribute.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        public void AssumeAssemblyVersionMatch(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.AssumeAssemblyVersionMatch))
                Report(Diagnostic.AssumeAssemblyVersionMatch.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        public void InvalidDirectoryInLibOptionPath(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidDirectoryInLibOptionPath))
                Report(Diagnostic.InvalidDirectoryInLibOptionPath.Event([arg0]));
        }

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        public void InvalidDirectoryInLibEnvironmentPath(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidDirectoryInLibEnvironmentPath))
                Report(Diagnostic.InvalidDirectoryInLibEnvironmentPath.Event([arg0]));
        }

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        public void LegacySearchRule(string arg0)
        {
            if (IsEnabled(Diagnostic.LegacySearchRule))
                Report(Diagnostic.LegacySearchRule.Event([arg0]));
        }

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        public void AssemblyLocationIgnored(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.AssemblyLocationIgnored))
                Report(Diagnostic.AssemblyLocationIgnored.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void InterfaceMethodCantBeInternal(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.InterfaceMethodCantBeInternal))
                Report(Diagnostic.InterfaceMethodCantBeInternal.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate assembly reference "{arg0}"
        /// </remarks>
        public void DuplicateAssemblyReference(string arg0)
        {
            if (IsEnabled(Diagnostic.DuplicateAssemblyReference))
                Report(Diagnostic.DuplicateAssemblyReference.Event([arg0]));
        }

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        public void UnableToResolveType(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.UnableToResolveType))
                Report(Diagnostic.UnableToResolveType.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        public void StubsAreDeprecated(string arg0)
        {
            if (IsEnabled(Diagnostic.StubsAreDeprecated))
                Report(Diagnostic.StubsAreDeprecated.Event([arg0]));
        }

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        public void WrongClassName(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.WrongClassName))
                Report(Diagnostic.WrongClassName.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void ReflectionCallerClassRequiresCallerID(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.ReflectionCallerClassRequiresCallerID))
                Report(Diagnostic.ReflectionCallerClassRequiresCallerID.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        public void LegacyAssemblyAttributesFound()
        {
            if (IsEnabled(Diagnostic.LegacyAssemblyAttributesFound))
                Report(Diagnostic.LegacyAssemblyAttributesFound.Event([]));
        }

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create static lambda factory.
        /// </remarks>
        public void UnableToCreateLambdaFactory()
        {
            if (IsEnabled(Diagnostic.UnableToCreateLambdaFactory))
                Report(Diagnostic.UnableToCreateLambdaFactory.Event([]));
        }

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void UnknownWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.UnknownWarning))
                Report(Diagnostic.UnknownWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public void DuplicateIkvmLangProperty(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.DuplicateIkvmLangProperty))
                Report(Diagnostic.DuplicateIkvmLangProperty.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public void MalformedIkvmLangProperty(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.MalformedIkvmLangProperty))
                Report(Diagnostic.MalformedIkvmLangProperty.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericCompilerWarning))
                Report(Diagnostic.GenericCompilerWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericClassLoadingWarning))
                Report(Diagnostic.GenericClassLoadingWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericVerifierWarning))
                Report(Diagnostic.GenericVerifierWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericRuntimeWarning))
                Report(Diagnostic.GenericRuntimeWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniWarning(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericJniWarning))
                Report(Diagnostic.GenericJniWarning.Event([arg0]));
        }

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        public void UnableToCreateProxy(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.UnableToCreateProxy))
                Report(Diagnostic.UnableToCreateProxy.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate proxy "{arg0}".
        /// </remarks>
        public void DuplicateProxy(string arg0)
        {
            if (IsEnabled(Diagnostic.DuplicateProxy))
                Report(Diagnostic.DuplicateProxy.Event([arg0]));
        }

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        public void MapXmlUnableToResolveOpCode(string arg0)
        {
            if (IsEnabled(Diagnostic.MapXmlUnableToResolveOpCode))
                Report(Diagnostic.MapXmlUnableToResolveOpCode.Event([arg0]));
        }

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error in remap file: {arg0}.
        /// </remarks>
        public void MapXmlError(string arg0)
        {
            if (IsEnabled(Diagnostic.MapXmlError))
                Report(Diagnostic.MapXmlError.Event([arg0]));
        }

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Source file '{arg0}' not found.
        /// </remarks>
        public void InputFileNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.InputFileNotFound))
                Report(Diagnostic.InputFileNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown file type: {arg0}.
        /// </remarks>
        public void UnknownFileType(string arg0)
        {
            if (IsEnabled(Diagnostic.UnknownFileType))
                Report(Diagnostic.UnknownFileType.Event([arg0]));
        }

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public void UnknownElementInMapFile(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.UnknownElementInMapFile))
                Report(Diagnostic.UnknownElementInMapFile.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public void UnknownAttributeInMapFile(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.UnknownAttributeInMapFile))
                Report(Diagnostic.UnknownAttributeInMapFile.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        public void InvalidMemberNameInMapFile(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.InvalidMemberNameInMapFile))
                Report(Diagnostic.InvalidMemberNameInMapFile.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        public void InvalidMemberSignatureInMapFile(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.InvalidMemberSignatureInMapFile))
                Report(Diagnostic.InvalidMemberSignatureInMapFile.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public void InvalidPropertyNameInMapFile(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.InvalidPropertyNameInMapFile))
                Report(Diagnostic.InvalidPropertyNameInMapFile.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public void InvalidPropertySignatureInMapFile(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.InvalidPropertySignatureInMapFile))
                Report(Diagnostic.InvalidPropertySignatureInMapFile.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        public void NonPrimaryAssemblyReference(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.NonPrimaryAssemblyReference))
                Report(Diagnostic.NonPrimaryAssemblyReference.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        public void MissingType(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.MissingType))
                Report(Diagnostic.MissingType.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public void MissingReference(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.MissingReference))
                Report(Diagnostic.MissingReference.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public void CallerSensitiveOnUnsupportedMethod(string arg0, string arg1, string arg2)
        {
            if (IsEnabled(Diagnostic.CallerSensitiveOnUnsupportedMethod))
                Report(Diagnostic.CallerSensitiveOnUnsupportedMethod.Event([arg0, arg1, arg2]));
        }

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        public void RemappedTypeMissingDefaultInterfaceMethod(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.RemappedTypeMissingDefaultInterfaceMethod))
                Report(Diagnostic.RemappedTypeMissingDefaultInterfaceMethod.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerError(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericCompilerError))
                Report(Diagnostic.GenericCompilerError.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericClassLoadingError(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericClassLoadingError))
                Report(Diagnostic.GenericClassLoadingError.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericVerifierError(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericVerifierError))
                Report(Diagnostic.GenericVerifierError.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeError(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericRuntimeError))
                Report(Diagnostic.GenericRuntimeError.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniError(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericJniError))
                Report(Diagnostic.GenericJniError.Event([arg0]));
        }

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Exporting previously imported assemblies is not supported.
        /// </remarks>
        public void ExportingImportsNotSupported()
        {
            if (IsEnabled(Diagnostic.ExportingImportsNotSupported))
                Report(Diagnostic.ExportingImportsNotSupported.Event([]));
        }

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Response file nesting depth exceeded.
        /// </remarks>
        public void ResponseFileDepthExceeded()
        {
            if (IsEnabled(Diagnostic.ResponseFileDepthExceeded))
                Report(Diagnostic.ResponseFileDepthExceeded.Event([]));
        }

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorReadingFile(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.ErrorReadingFile))
                Report(Diagnostic.ErrorReadingFile.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// No targets found
        /// </remarks>
        public void NoTargetsFound()
        {
            if (IsEnabled(Diagnostic.NoTargetsFound))
                Report(Diagnostic.NoTargetsFound.Event([]));
        }

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// File format limitation exceeded: {arg0}.
        /// </remarks>
        public void FileFormatLimitationExceeded(string arg0)
        {
            if (IsEnabled(Diagnostic.FileFormatLimitationExceeded))
                Report(Diagnostic.FileFormatLimitationExceeded.Event([arg0]));
        }

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot specify both a key file and container.
        /// </remarks>
        public void CannotSpecifyBothKeyFileAndContainer()
        {
            if (IsEnabled(Diagnostic.CannotSpecifyBothKeyFileAndContainer))
                Report(Diagnostic.CannotSpecifyBothKeyFileAndContainer.Event([]));
        }

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot delay sign without a key file or container.
        /// </remarks>
        public void DelaySignRequiresKey()
        {
            if (IsEnabled(Diagnostic.DelaySignRequiresKey))
                Report(Diagnostic.DelaySignRequiresKey.Event([]));
        }

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        public void InvalidStrongNameKeyPair(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.InvalidStrongNameKeyPair))
                Report(Diagnostic.InvalidStrongNameKeyPair.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference not found: {arg0}
        /// </remarks>
        public void ReferenceNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.ReferenceNotFound))
                Report(Diagnostic.ReferenceNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// You can only specify options before any child levels.
        /// </remarks>
        public void OptionsMustPreceedChildLevels()
        {
            if (IsEnabled(Diagnostic.OptionsMustPreceedChildLevels))
                Report(Diagnostic.OptionsMustPreceedChildLevels.Event([]));
        }

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -target option.
        /// </remarks>
        public void UnrecognizedTargetType(string arg0)
        {
            if (IsEnabled(Diagnostic.UnrecognizedTargetType))
                Report(Diagnostic.UnrecognizedTargetType.Event([arg0]));
        }

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -platform option.
        /// </remarks>
        public void UnrecognizedPlatform(string arg0)
        {
            if (IsEnabled(Diagnostic.UnrecognizedPlatform))
                Report(Diagnostic.UnrecognizedPlatform.Event([arg0]));
        }

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        public void UnrecognizedApartment(string arg0)
        {
            if (IsEnabled(Diagnostic.UnrecognizedApartment))
                Report(Diagnostic.UnrecognizedApartment.Event([arg0]));
        }

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Missing file specification for '{arg0}' option.
        /// </remarks>
        public void MissingFileSpecification(string arg0)
        {
            if (IsEnabled(Diagnostic.MissingFileSpecification))
                Report(Diagnostic.MissingFileSpecification.Event([arg0]));
        }

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Path too long: {arg0}.
        /// </remarks>
        public void PathTooLong(string arg0)
        {
            if (IsEnabled(Diagnostic.PathTooLong))
                Report(Diagnostic.PathTooLong.Event([arg0]));
        }

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Path not found: {arg0}.
        /// </remarks>
        public void PathNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.PathNotFound))
                Report(Diagnostic.PathNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid path: {arg0}.
        /// </remarks>
        public void InvalidPath(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidPath))
                Report(Diagnostic.InvalidPath.Event([arg0]));
        }

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid option: {arg0}.
        /// </remarks>
        public void InvalidOptionSyntax(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidOptionSyntax))
                Report(Diagnostic.InvalidOptionSyntax.Event([arg0]));
        }

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file does not exist: {arg0}.
        /// </remarks>
        public void ExternalResourceNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.ExternalResourceNotFound))
                Report(Diagnostic.ExternalResourceNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file may not include path specification: {arg0}.
        /// </remarks>
        public void ExternalResourceNameInvalid(string arg0)
        {
            if (IsEnabled(Diagnostic.ExternalResourceNameInvalid))
                Report(Diagnostic.ExternalResourceNameInvalid.Event([arg0]));
        }

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid version specified: {arg0}.
        /// </remarks>
        public void InvalidVersionFormat(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidVersionFormat))
                Report(Diagnostic.InvalidVersionFormat.Event([arg0]));
        }

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        public void InvalidFileAlignment(string arg0)
        {
            if (IsEnabled(Diagnostic.InvalidFileAlignment))
                Report(Diagnostic.InvalidFileAlignment.Event([arg0]));
        }

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorWritingFile(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.ErrorWritingFile))
                Report(Diagnostic.ErrorWritingFile.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Unrecognized option: {arg0}.
        /// </remarks>
        public void UnrecognizedOption(string arg0)
        {
            if (IsEnabled(Diagnostic.UnrecognizedOption))
                Report(Diagnostic.UnrecognizedOption.Event([arg0]));
        }

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// No output file specified.
        /// </remarks>
        public void NoOutputFileSpecified()
        {
            if (IsEnabled(Diagnostic.NoOutputFileSpecified))
                Report(Diagnostic.NoOutputFileSpecified.Event([]));
        }

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        public void SharedClassLoaderCannotBeUsedOnModuleTarget()
        {
            if (IsEnabled(Diagnostic.SharedClassLoaderCannotBeUsedOnModuleTarget))
                Report(Diagnostic.SharedClassLoaderCannotBeUsedOnModuleTarget.Event([]));
        }

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime assembly.
        /// </remarks>
        public void RuntimeNotFound()
        {
            if (IsEnabled(Diagnostic.RuntimeNotFound))
                Report(Diagnostic.RuntimeNotFound.Event([]));
        }

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class cannot be specified for library or module.
        /// </remarks>
        public void MainClassRequiresExe()
        {
            if (IsEnabled(Diagnostic.MainClassRequiresExe))
                Report(Diagnostic.MainClassRequiresExe.Event([]));
        }

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// No main method found.
        /// </remarks>
        public void ExeRequiresMainClass()
        {
            if (IsEnabled(Diagnostic.ExeRequiresMainClass))
                Report(Diagnostic.ExeRequiresMainClass.Event([]));
        }

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Properties cannot be specified for library or module.
        /// </remarks>
        public void PropertiesRequireExe()
        {
            if (IsEnabled(Diagnostic.PropertiesRequireExe))
                Report(Diagnostic.PropertiesRequireExe.Event([]));
        }

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Cannot specify assembly class loader for modules.
        /// </remarks>
        public void ModuleCannotHaveClassLoader()
        {
            if (IsEnabled(Diagnostic.ModuleCannotHaveClassLoader))
                Report(Diagnostic.ModuleCannotHaveClassLoader.Event([]));
        }

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        public void ErrorParsingMapFile(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.ErrorParsingMapFile))
                Report(Diagnostic.ErrorParsingMapFile.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Bootstrap classes missing and core assembly not found.
        /// </remarks>
        public void BootstrapClassesMissing()
        {
            if (IsEnabled(Diagnostic.BootstrapClassesMissing))
                Report(Diagnostic.BootstrapClassesMissing.Event([]));
        }

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        public void StrongNameRequiresStrongNamedRefs()
        {
            if (IsEnabled(Diagnostic.StrongNameRequiresStrongNamedRefs))
                Report(Diagnostic.StrongNameRequiresStrongNamedRefs.Event([]));
        }

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class not found.
        /// </remarks>
        public void MainClassNotFound()
        {
            if (IsEnabled(Diagnostic.MainClassNotFound))
                Report(Diagnostic.MainClassNotFound.Event([]));
        }

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main method not found.
        /// </remarks>
        public void MainMethodNotFound()
        {
            if (IsEnabled(Diagnostic.MainMethodNotFound))
                Report(Diagnostic.MainMethodNotFound.Event([]));
        }

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Redirected main method not supported.
        /// </remarks>
        public void UnsupportedMainMethod()
        {
            if (IsEnabled(Diagnostic.UnsupportedMainMethod))
                Report(Diagnostic.UnsupportedMainMethod.Event([]));
        }

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// External main method must be public and in a public class.
        /// </remarks>
        public void ExternalMainNotAccessible()
        {
            if (IsEnabled(Diagnostic.ExternalMainNotAccessible))
                Report(Diagnostic.ExternalMainNotAccessible.Event([]));
        }

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class not found.
        /// </remarks>
        public void ClassLoaderNotFound()
        {
            if (IsEnabled(Diagnostic.ClassLoaderNotFound))
                Report(Diagnostic.ClassLoaderNotFound.Event([]));
        }

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is not accessible.
        /// </remarks>
        public void ClassLoaderNotAccessible()
        {
            if (IsEnabled(Diagnostic.ClassLoaderNotAccessible))
                Report(Diagnostic.ClassLoaderNotAccessible.Event([]));
        }

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is abstract.
        /// </remarks>
        public void ClassLoaderIsAbstract()
        {
            if (IsEnabled(Diagnostic.ClassLoaderIsAbstract))
                Report(Diagnostic.ClassLoaderIsAbstract.Event([]));
        }

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        public void ClassLoaderNotClassLoader()
        {
            if (IsEnabled(Diagnostic.ClassLoaderNotClassLoader))
                Report(Diagnostic.ClassLoaderNotClassLoader.Event([]));
        }

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader constructor is missing.
        /// </remarks>
        public void ClassLoaderConstructorMissing()
        {
            if (IsEnabled(Diagnostic.ClassLoaderConstructorMissing))
                Report(Diagnostic.ClassLoaderConstructorMissing.Event([]));
        }

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public void MapFileTypeNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.MapFileTypeNotFound))
                Report(Diagnostic.MapFileTypeNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public void MapFileClassNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.MapFileClassNotFound))
                Report(Diagnostic.MapFileClassNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Maximum error count reached.
        /// </remarks>
        public void MaximumErrorCountReached()
        {
            if (IsEnabled(Diagnostic.MaximumErrorCountReached))
                Report(Diagnostic.MaximumErrorCountReached.Event([]));
        }

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Link error: {arg0}
        /// </remarks>
        public void LinkageError(string arg0)
        {
            if (IsEnabled(Diagnostic.LinkageError))
                Report(Diagnostic.LinkageError.Event([arg0]));
        }

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly {referencedAssemblyPath} was compiled with an incompatible IKVM.Runtime version. Current runtime: {runtimeAssemblyName}. Referenced assembly runtime: {referencedAssemblyName}
        /// </remarks>
        public void RuntimeMismatch(string referencedAssemblyPath, string runtimeAssemblyName, string referencedAssemblyName)
        {
            if (IsEnabled(Diagnostic.RuntimeMismatch))
                Report(Diagnostic.RuntimeMismatch.Event([referencedAssemblyPath, runtimeAssemblyName, referencedAssemblyName]));
        }

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
///
        /// </remarks>
        public void RuntimeMismatchStrongName()
        {
            if (IsEnabled(Diagnostic.RuntimeMismatchStrongName))
                Report(Diagnostic.RuntimeMismatchStrongName.Event([]));
        }

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Failed to find core classes in core library.
        /// </remarks>
        public void CoreClassesMissing()
        {
            if (IsEnabled(Diagnostic.CoreClassesMissing))
                Report(Diagnostic.CoreClassesMissing.Event([]));
        }

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load critical class '{arg0}'.
        /// </remarks>
        public void CriticalClassNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.CriticalClassNotFound))
                Report(Diagnostic.CriticalClassNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        public void AssemblyContainsDuplicateClassNames(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.AssemblyContainsDuplicateClassNames))
                Report(Diagnostic.AssemblyContainsDuplicateClassNames.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        public void CallerIDRequiresHasCallerIDAnnotation()
        {
            if (IsEnabled(Diagnostic.CallerIDRequiresHasCallerIDAnnotation))
                Report(Diagnostic.CallerIDRequiresHasCallerIDAnnotation.Event([]));
        }

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        public void UnableToResolveInterface(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.UnableToResolveInterface))
                Report(Diagnostic.UnableToResolveInterface.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        public void MissingBaseType(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.MissingBaseType))
                Report(Diagnostic.MissingBaseType.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public void MissingBaseTypeReference(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.MissingBaseTypeReference))
                Report(Diagnostic.MissingBaseTypeReference.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// File not found: {arg0}.
        /// </remarks>
        public void FileNotFound(string arg0)
        {
            if (IsEnabled(Diagnostic.FileNotFound))
                Report(Diagnostic.FileNotFound.Event([arg0]));
        }

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Runtime method '{arg0}' not found.
        /// </remarks>
        public void RuntimeMethodMissing(string arg0)
        {
            if (IsEnabled(Diagnostic.RuntimeMethodMissing))
                Report(Diagnostic.RuntimeMethodMissing.Event([arg0]));
        }

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        public void MapFileFieldNotFound(string arg0, string arg1)
        {
            if (IsEnabled(Diagnostic.MapFileFieldNotFound))
                Report(Diagnostic.MapFileFieldNotFound.Event([arg0, arg1]));
        }

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        public void GhostInterfaceMethodMissing(string arg0, string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.GhostInterfaceMethodMissing))
                Report(Diagnostic.GhostInterfaceMethodMissing.Event([arg0, arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        public void ModuleInitializerMethodRequirements(string arg1, string arg2, string arg3)
        {
            if (IsEnabled(Diagnostic.ModuleInitializerMethodRequirements))
                Report(Diagnostic.ModuleInitializerMethodRequirements.Event([arg1, arg2, arg3]));
        }

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid zip: {name}.
        /// </remarks>
        public void InvalidZip(string name)
        {
            if (IsEnabled(Diagnostic.InvalidZip))
                Report(Diagnostic.InvalidZip.Event([name]));
        }

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericRuntimeTrace(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericRuntimeTrace))
                Report(Diagnostic.GenericRuntimeTrace.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericJniTrace(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericJniTrace))
                Report(Diagnostic.GenericJniTrace.Event([arg0]));
        }

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public void GenericCompilerTrace(string arg0)
        {
            if (IsEnabled(Diagnostic.GenericCompilerTrace))
                Report(Diagnostic.GenericCompilerTrace.Event([arg0]));
        }

    }

}
