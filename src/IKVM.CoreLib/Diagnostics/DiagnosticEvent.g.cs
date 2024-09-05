#nullable enable
using System;
using System.Text;

namespace IKVM.CoreLib.Diagnostics
{

    readonly partial struct DiagnosticEvent
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Found main method in class "{arg0}".
        /// </remarks>
        public static DiagnosticEvent MainMethodFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MainMethodFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Output file is "{arg0}".
        /// </remarks>
        public static DiagnosticEvent OutputFileIs(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.OutputFileIs.Event([arg0], exception, location);

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Automatically adding reference to "{arg0}".
        /// </remarks>
        public static DiagnosticEvent AutoAddRef(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.AutoAddRef.Event([arg0], exception, location);

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        public static DiagnosticEvent MainMethodFromManifest(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MainMethodFromManifest.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericCompilerInfo(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericCompilerInfo.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericClassLoadingInfo(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericClassLoadingInfo.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericVerifierInfo(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericVerifierInfo.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericRuntimeInfo(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericRuntimeInfo.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericJniInfo(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericJniInfo.Event([arg0], exception, location);

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class "{arg0}" not found.
        /// </remarks>
        public static DiagnosticEvent ClassNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        public static DiagnosticEvent ClassFormatError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassFormatError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate class name: "{arg0}".
        /// </remarks>
        public static DiagnosticEvent DuplicateClassName(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DuplicateClassName.Event([arg0], exception, location);

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        public static DiagnosticEvent IllegalAccessError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.IllegalAccessError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        public static DiagnosticEvent VerificationError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.VerificationError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        public static DiagnosticEvent NoClassDefFoundError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.NoClassDefFoundError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        public static DiagnosticEvent GenericUnableToCompileError(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericUnableToCompileError.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping resource (name clash): "{arg0}"
        /// </remarks>
        public static DiagnosticEvent DuplicateResourceName(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DuplicateResourceName.Event([arg0], exception, location);

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        public static DiagnosticEvent SkippingReferencedClass(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.SkippingReferencedClass.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime JNI assembly.
        /// </remarks>
        public static DiagnosticEvent NoJniRuntime(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.NoJniRuntime.Event([], exception, location);

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        public static DiagnosticEvent EmittedNoClassDefFoundError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedNoClassDefFoundError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedIllegalAccessError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedIllegalAccessError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedInstantiationError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedInstantiationError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedIncompatibleClassChangeError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedIncompatibleClassChangeError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedNoSuchFieldError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedNoSuchFieldError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedAbstractMethodError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedAbstractMethodError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedNoSuchMethodError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedNoSuchMethodError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedLinkageError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedLinkageError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedVerificationError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedVerificationError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent EmittedClassFormatError(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.EmittedClassFormatError.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent InvalidCustomAttribute(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidCustomAttribute.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent IgnoredCustomAttribute(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.IgnoredCustomAttribute.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        public static DiagnosticEvent AssumeAssemblyVersionMatch(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.AssumeAssemblyVersionMatch.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        public static DiagnosticEvent InvalidDirectoryInLibOptionPath(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidDirectoryInLibOptionPath.Event([arg0], exception, location);

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        public static DiagnosticEvent InvalidDirectoryInLibEnvironmentPath(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidDirectoryInLibEnvironmentPath.Event([arg0], exception, location);

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        public static DiagnosticEvent LegacySearchRule(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.LegacySearchRule.Event([arg0], exception, location);

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        public static DiagnosticEvent AssemblyLocationIgnored(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.AssemblyLocationIgnored.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static DiagnosticEvent InterfaceMethodCantBeInternal(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InterfaceMethodCantBeInternal.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate assembly reference "{arg0}"
        /// </remarks>
        public static DiagnosticEvent DuplicateAssemblyReference(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DuplicateAssemblyReference.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        public static DiagnosticEvent UnableToResolveType(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnableToResolveType.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        public static DiagnosticEvent StubsAreDeprecated(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.StubsAreDeprecated.Event([arg0], exception, location);

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        public static DiagnosticEvent WrongClassName(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.WrongClassName.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static DiagnosticEvent ReflectionCallerClassRequiresCallerID(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ReflectionCallerClassRequiresCallerID.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        public static DiagnosticEvent LegacyAssemblyAttributesFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.LegacyAssemblyAttributesFound.Event([], exception, location);

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create static lambda factory.
        /// </remarks>
        public static DiagnosticEvent UnableToCreateLambdaFactory(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnableToCreateLambdaFactory.Event([], exception, location);

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent UnknownWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnknownWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public static DiagnosticEvent DuplicateIkvmLangProperty(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DuplicateIkvmLangProperty.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public static DiagnosticEvent MalformedIkvmLangProperty(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MalformedIkvmLangProperty.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericCompilerWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericCompilerWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericClassLoadingWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericClassLoadingWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericVerifierWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericVerifierWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericRuntimeWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericRuntimeWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericJniWarning(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericJniWarning.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent UnableToCreateProxy(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnableToCreateProxy.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Duplicate proxy "{arg0}".
        /// </remarks>
        public static DiagnosticEvent DuplicateProxy(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DuplicateProxy.Event([arg0], exception, location);

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        public static DiagnosticEvent MapXmlUnableToResolveOpCode(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MapXmlUnableToResolveOpCode.Event([arg0], exception, location);

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error in remap file: {arg0}.
        /// </remarks>
        public static DiagnosticEvent MapXmlError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MapXmlError.Event([arg0], exception, location);

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Source file '{arg0}' not found.
        /// </remarks>
        public static DiagnosticEvent InputFileNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InputFileNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown file type: {arg0}.
        /// </remarks>
        public static DiagnosticEvent UnknownFileType(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnknownFileType.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public static DiagnosticEvent UnknownElementInMapFile(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnknownElementInMapFile.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public static DiagnosticEvent UnknownAttributeInMapFile(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnknownAttributeInMapFile.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        public static DiagnosticEvent InvalidMemberNameInMapFile(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidMemberNameInMapFile.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        public static DiagnosticEvent InvalidMemberSignatureInMapFile(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidMemberSignatureInMapFile.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public static DiagnosticEvent InvalidPropertyNameInMapFile(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidPropertyNameInMapFile.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public static DiagnosticEvent InvalidPropertySignatureInMapFile(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidPropertySignatureInMapFile.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        public static DiagnosticEvent NonPrimaryAssemblyReference(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.NonPrimaryAssemblyReference.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        public static DiagnosticEvent MissingType(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MissingType.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public static DiagnosticEvent MissingReference(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MissingReference.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static DiagnosticEvent CallerSensitiveOnUnsupportedMethod(string arg0, string arg1, string arg2, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.CallerSensitiveOnUnsupportedMethod.Event([arg0, arg1, arg2], exception, location);

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        public static DiagnosticEvent RemappedTypeMissingDefaultInterfaceMethod(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.RemappedTypeMissingDefaultInterfaceMethod.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericCompilerError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericCompilerError.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericClassLoadingError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericClassLoadingError.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericVerifierError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericVerifierError.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericRuntimeError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericRuntimeError.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericJniError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericJniError.Event([arg0], exception, location);

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Exporting previously imported assemblies is not supported.
        /// </remarks>
        public static DiagnosticEvent ExportingImportsNotSupported(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ExportingImportsNotSupported.Event([], exception, location);

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Response file nesting depth exceeded.
        /// </remarks>
        public static DiagnosticEvent ResponseFileDepthExceeded(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ResponseFileDepthExceeded.Event([], exception, location);

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        public static DiagnosticEvent ErrorReadingFile(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ErrorReadingFile.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// No targets found
        /// </remarks>
        public static DiagnosticEvent NoTargetsFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.NoTargetsFound.Event([], exception, location);

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// File format limitation exceeded: {arg0}.
        /// </remarks>
        public static DiagnosticEvent FileFormatLimitationExceeded(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.FileFormatLimitationExceeded.Event([arg0], exception, location);

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot specify both a key file and container.
        /// </remarks>
        public static DiagnosticEvent CannotSpecifyBothKeyFileAndContainer(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.CannotSpecifyBothKeyFileAndContainer.Event([], exception, location);

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// You cannot delay sign without a key file or container.
        /// </remarks>
        public static DiagnosticEvent DelaySignRequiresKey(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.DelaySignRequiresKey.Event([], exception, location);

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        public static DiagnosticEvent InvalidStrongNameKeyPair(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidStrongNameKeyPair.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Reference not found: {arg0}
        /// </remarks>
        public static DiagnosticEvent ReferenceNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ReferenceNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// You can only specify options before any child levels.
        /// </remarks>
        public static DiagnosticEvent OptionsMustPreceedChildLevels(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.OptionsMustPreceedChildLevels.Event([], exception, location);

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -target option.
        /// </remarks>
        public static DiagnosticEvent UnrecognizedTargetType(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnrecognizedTargetType.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -platform option.
        /// </remarks>
        public static DiagnosticEvent UnrecognizedPlatform(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnrecognizedPlatform.Event([arg0], exception, location);

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        public static DiagnosticEvent UnrecognizedApartment(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnrecognizedApartment.Event([arg0], exception, location);

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Missing file specification for '{arg0}' option.
        /// </remarks>
        public static DiagnosticEvent MissingFileSpecification(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MissingFileSpecification.Event([arg0], exception, location);

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Path too long: {arg0}.
        /// </remarks>
        public static DiagnosticEvent PathTooLong(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.PathTooLong.Event([arg0], exception, location);

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Path not found: {arg0}.
        /// </remarks>
        public static DiagnosticEvent PathNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.PathNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid path: {arg0}.
        /// </remarks>
        public static DiagnosticEvent InvalidPath(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidPath.Event([arg0], exception, location);

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid option: {arg0}.
        /// </remarks>
        public static DiagnosticEvent InvalidOptionSyntax(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidOptionSyntax.Event([arg0], exception, location);

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file does not exist: {arg0}.
        /// </remarks>
        public static DiagnosticEvent ExternalResourceNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ExternalResourceNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// External resource file may not include path specification: {arg0}.
        /// </remarks>
        public static DiagnosticEvent ExternalResourceNameInvalid(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ExternalResourceNameInvalid.Event([arg0], exception, location);

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid version specified: {arg0}.
        /// </remarks>
        public static DiagnosticEvent InvalidVersionFormat(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidVersionFormat.Event([arg0], exception, location);

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        public static DiagnosticEvent InvalidFileAlignment(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidFileAlignment.Event([arg0], exception, location);

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        public static DiagnosticEvent ErrorWritingFile(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ErrorWritingFile.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Unrecognized option: {arg0}.
        /// </remarks>
        public static DiagnosticEvent UnrecognizedOption(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnrecognizedOption.Event([arg0], exception, location);

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// No output file specified.
        /// </remarks>
        public static DiagnosticEvent NoOutputFileSpecified(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.NoOutputFileSpecified.Event([], exception, location);

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        public static DiagnosticEvent SharedClassLoaderCannotBeUsedOnModuleTarget(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.SharedClassLoaderCannotBeUsedOnModuleTarget.Event([], exception, location);

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load runtime assembly.
        /// </remarks>
        public static DiagnosticEvent RuntimeNotFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.RuntimeNotFound.Event([], exception, location);

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class cannot be specified for library or module.
        /// </remarks>
        public static DiagnosticEvent MainClassRequiresExe(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MainClassRequiresExe.Event([], exception, location);

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// No main method found.
        /// </remarks>
        public static DiagnosticEvent ExeRequiresMainClass(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ExeRequiresMainClass.Event([], exception, location);

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Properties cannot be specified for library or module.
        /// </remarks>
        public static DiagnosticEvent PropertiesRequireExe(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.PropertiesRequireExe.Event([], exception, location);

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Cannot specify assembly class loader for modules.
        /// </remarks>
        public static DiagnosticEvent ModuleCannotHaveClassLoader(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ModuleCannotHaveClassLoader.Event([], exception, location);

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        public static DiagnosticEvent ErrorParsingMapFile(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ErrorParsingMapFile.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Bootstrap classes missing and core assembly not found.
        /// </remarks>
        public static DiagnosticEvent BootstrapClassesMissing(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.BootstrapClassesMissing.Event([], exception, location);

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        public static DiagnosticEvent StrongNameRequiresStrongNamedRefs(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.StrongNameRequiresStrongNamedRefs.Event([], exception, location);

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main class not found.
        /// </remarks>
        public static DiagnosticEvent MainClassNotFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MainClassNotFound.Event([], exception, location);

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Main method not found.
        /// </remarks>
        public static DiagnosticEvent MainMethodNotFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MainMethodNotFound.Event([], exception, location);

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Redirected main method not supported.
        /// </remarks>
        public static DiagnosticEvent UnsupportedMainMethod(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnsupportedMainMethod.Event([], exception, location);

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// External main method must be public and in a public class.
        /// </remarks>
        public static DiagnosticEvent ExternalMainNotAccessible(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ExternalMainNotAccessible.Event([], exception, location);

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class not found.
        /// </remarks>
        public static DiagnosticEvent ClassLoaderNotFound(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassLoaderNotFound.Event([], exception, location);

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is not accessible.
        /// </remarks>
        public static DiagnosticEvent ClassLoaderNotAccessible(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassLoaderNotAccessible.Event([], exception, location);

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class is abstract.
        /// </remarks>
        public static DiagnosticEvent ClassLoaderIsAbstract(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassLoaderIsAbstract.Event([], exception, location);

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        public static DiagnosticEvent ClassLoaderNotClassLoader(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassLoaderNotClassLoader.Event([], exception, location);

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Custom assembly class loader constructor is missing.
        /// </remarks>
        public static DiagnosticEvent ClassLoaderConstructorMissing(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ClassLoaderConstructorMissing.Event([], exception, location);

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public static DiagnosticEvent MapFileTypeNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MapFileTypeNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public static DiagnosticEvent MapFileClassNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MapFileClassNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Maximum error count reached.
        /// </remarks>
        public static DiagnosticEvent MaximumErrorCountReached(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MaximumErrorCountReached.Event([], exception, location);

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Link error: {arg0}
        /// </remarks>
        public static DiagnosticEvent LinkageError(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.LinkageError.Event([arg0], exception, location);

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Referenced assembly {referencedAssemblyPath} was compiled with an incompatible IKVM.Runtime version. Current runtime: {runtimeAssemblyName}. Referenced assembly runtime: {referencedAssemblyName}
        /// </remarks>
        public static DiagnosticEvent RuntimeMismatch(string referencedAssemblyPath, string runtimeAssemblyName, string referencedAssemblyName, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.RuntimeMismatch.Event([referencedAssemblyPath, runtimeAssemblyName, referencedAssemblyName], exception, location);

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
///
        /// </remarks>
        public static DiagnosticEvent RuntimeMismatchStrongName(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.RuntimeMismatchStrongName.Event([], exception, location);

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Failed to find core classes in core library.
        /// </remarks>
        public static DiagnosticEvent CoreClassesMissing(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.CoreClassesMissing.Event([], exception, location);

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to load critical class '{arg0}'.
        /// </remarks>
        public static DiagnosticEvent CriticalClassNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.CriticalClassNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        public static DiagnosticEvent AssemblyContainsDuplicateClassNames(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.AssemblyContainsDuplicateClassNames.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        public static DiagnosticEvent CallerIDRequiresHasCallerIDAnnotation(Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.CallerIDRequiresHasCallerIDAnnotation.Event([], exception, location);

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        public static DiagnosticEvent UnableToResolveInterface(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.UnableToResolveInterface.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        public static DiagnosticEvent MissingBaseType(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MissingBaseType.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public static DiagnosticEvent MissingBaseTypeReference(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MissingBaseTypeReference.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// File not found: {arg0}.
        /// </remarks>
        public static DiagnosticEvent FileNotFound(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.FileNotFound.Event([arg0], exception, location);

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Runtime method '{arg0}' not found.
        /// </remarks>
        public static DiagnosticEvent RuntimeMethodMissing(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.RuntimeMethodMissing.Event([arg0], exception, location);

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        public static DiagnosticEvent MapFileFieldNotFound(string arg0, string arg1, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.MapFileFieldNotFound.Event([arg0, arg1], exception, location);

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        public static DiagnosticEvent GhostInterfaceMethodMissing(string arg0, string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GhostInterfaceMethodMissing.Event([arg0, arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        public static DiagnosticEvent ModuleInitializerMethodRequirements(string arg1, string arg2, string arg3, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.ModuleInitializerMethodRequirements.Event([arg1, arg2, arg3], exception, location);

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Invalid zip: {name}.
        /// </remarks>
        public static DiagnosticEvent InvalidZip(string name, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.InvalidZip.Event([name], exception, location);

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericRuntimeTrace(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericRuntimeTrace.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericJniTrace(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericJniTrace.Event([arg0], exception, location);

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// {arg0}
        /// </remarks>
        public static DiagnosticEvent GenericCompilerTrace(string arg0, Exception? exception = null, DiagnosticLocation location = default) => Diagnostic.GenericCompilerTrace.Event([arg0], exception, location);

    }

}
