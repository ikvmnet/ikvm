using System.Text;

namespace IKVM.CoreLib.Diagnostics
{

    readonly record struct MainMethodFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MainMethodFound = new Diagnostic(new DiagnosticId(1), nameof(MainMethodFound), CompositeFormat.Parse("Found main method in class \"{0}\""), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic MainMethodFound = new Diagnostic(new DiagnosticId(1), nameof(MainMethodFound), "Found main method in class \"{0}\"", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct OutputFileIsData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic OutputFileIs = new Diagnostic(new DiagnosticId(2), nameof(OutputFileIs), CompositeFormat.Parse("Output file is \"{0}\""), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic OutputFileIs = new Diagnostic(new DiagnosticId(2), nameof(OutputFileIs), "Output file is \"{0}\"", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct AutoAddRefData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic AutoAddRef = new Diagnostic(new DiagnosticId(3), nameof(AutoAddRef), CompositeFormat.Parse("Automatically adding reference to \"{0}\""), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic AutoAddRef = new Diagnostic(new DiagnosticId(3), nameof(AutoAddRef), "Automatically adding reference to \"{0}\"", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct MainMethodFromManifestData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MainMethodFromManifest = new Diagnostic(new DiagnosticId(4), nameof(MainMethodFromManifest), CompositeFormat.Parse("Using main class \"{0}\" based on jar manifest"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic MainMethodFromManifest = new Diagnostic(new DiagnosticId(4), nameof(MainMethodFromManifest), "Using main class \"{0}\" based on jar manifest", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct GenericCompilerInfoData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericCompilerInfo = new Diagnostic(new DiagnosticId(5), nameof(GenericCompilerInfo), CompositeFormat.Parse("{0}"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic GenericCompilerInfo = new Diagnostic(new DiagnosticId(5), nameof(GenericCompilerInfo), "{0}", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct GenericClassLoadingInfoData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericClassLoadingInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericClassLoadingInfo), CompositeFormat.Parse("{0}"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic GenericClassLoadingInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericClassLoadingInfo), "{0}", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct GenericVerifierInfoData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericVerifierInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericVerifierInfo), CompositeFormat.Parse("{0}"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic GenericVerifierInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericVerifierInfo), "{0}", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct GenericRuntimeInfoData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericRuntimeInfo = new Diagnostic(new DiagnosticId(7), nameof(GenericRuntimeInfo), CompositeFormat.Parse("{0}"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic GenericRuntimeInfo = new Diagnostic(new DiagnosticId(7), nameof(GenericRuntimeInfo), "{0}", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct GenericJniInfoData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericJniInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericJniInfo), CompositeFormat.Parse("{0}"), DiagnosticLevel.Informational);
#else
        public static readonly Diagnostic GenericJniInfo = new Diagnostic(new DiagnosticId(6), nameof(GenericJniInfo), "{0}", DiagnosticLevel.Informational);
#endif

    }

    readonly record struct ClassNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassNotFound = new Diagnostic(new DiagnosticId(100), nameof(ClassNotFound), CompositeFormat.Parse("Class \"{0}\" not found"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic ClassNotFound = new Diagnostic(new DiagnosticId(100), nameof(ClassNotFound), "Class \"{0}\" not found", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct ClassFormatErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassFormatError = new Diagnostic(new DiagnosticId(101), nameof(ClassFormatError), CompositeFormat.Parse("Unable to compile class \"{0}\" \n    (class format error \"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic ClassFormatError = new Diagnostic(new DiagnosticId(101), nameof(ClassFormatError), "Unable to compile class \"{0}\" \n    (class format error \"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct DuplicateClassNameData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DuplicateClassName = new Diagnostic(new DiagnosticId(102), nameof(DuplicateClassName), CompositeFormat.Parse("Duplicate class name: \"{0}\""), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic DuplicateClassName = new Diagnostic(new DiagnosticId(102), nameof(DuplicateClassName), "Duplicate class name: \"{0}\"", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct IllegalAccessErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic IllegalAccessError = new Diagnostic(new DiagnosticId(103), nameof(IllegalAccessError), CompositeFormat.Parse("Unable to compile class \"{0}\" \n    (illegal access error \"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic IllegalAccessError = new Diagnostic(new DiagnosticId(103), nameof(IllegalAccessError), "Unable to compile class \"{0}\" \n    (illegal access error \"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct VerificationErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic VerificationError = new Diagnostic(new DiagnosticId(104), nameof(VerificationError), CompositeFormat.Parse("Unable to compile class \"{0}\" \n    (verification error \"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic VerificationError = new Diagnostic(new DiagnosticId(104), nameof(VerificationError), "Unable to compile class \"{0}\" \n    (verification error \"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct NoClassDefFoundErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic NoClassDefFoundError = new Diagnostic(new DiagnosticId(105), nameof(NoClassDefFoundError), CompositeFormat.Parse("Unable to compile class \"{0}\" \n    (missing class \"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic NoClassDefFoundError = new Diagnostic(new DiagnosticId(105), nameof(NoClassDefFoundError), "Unable to compile class \"{0}\" \n    (missing class \"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericUnableToCompileErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericUnableToCompileError = new Diagnostic(new DiagnosticId(106), nameof(GenericUnableToCompileError), CompositeFormat.Parse("Unable to compile class \"{0}\" \n    (\"{1}\": \"{2}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericUnableToCompileError = new Diagnostic(new DiagnosticId(106), nameof(GenericUnableToCompileError), "Unable to compile class \"{0}\" \n    (\"{1}\": \"{2}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct DuplicateResourceNameData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DuplicateResourceName = new Diagnostic(new DiagnosticId(107), nameof(DuplicateResourceName), CompositeFormat.Parse("Skipping resource (name clash): \"{0}\""), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic DuplicateResourceName = new Diagnostic(new DiagnosticId(107), nameof(DuplicateResourceName), "Skipping resource (name clash): \"{0}\"", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct SkippingReferencedClassData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic SkippingReferencedClass = new Diagnostic(new DiagnosticId(109), nameof(SkippingReferencedClass), CompositeFormat.Parse("Skipping class: \"{0}\"\n    (class is already available in referenced assembly \"{1}" +
    "\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic SkippingReferencedClass = new Diagnostic(new DiagnosticId(109), nameof(SkippingReferencedClass), "Skipping class: \"{0}\"\n    (class is already available in referenced assembly \"{1}" +
    "\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct NoJniRuntimeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic NoJniRuntime = new Diagnostic(new DiagnosticId(110), nameof(NoJniRuntime), CompositeFormat.Parse("Unable to load runtime JNI assembly"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic NoJniRuntime = new Diagnostic(new DiagnosticId(110), nameof(NoJniRuntime), "Unable to load runtime JNI assembly", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedNoClassDefFoundErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedNoClassDefFoundError = new Diagnostic(new DiagnosticId(111), nameof(EmittedNoClassDefFoundError), CompositeFormat.Parse("Emitted java.lang.NoClassDefFoundError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedNoClassDefFoundError = new Diagnostic(new DiagnosticId(111), nameof(EmittedNoClassDefFoundError), "Emitted java.lang.NoClassDefFoundError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedIllegalAccessErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedIllegalAccessError = new Diagnostic(new DiagnosticId(112), nameof(EmittedIllegalAccessError), CompositeFormat.Parse("Emitted java.lang.IllegalAccessError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedIllegalAccessError = new Diagnostic(new DiagnosticId(112), nameof(EmittedIllegalAccessError), "Emitted java.lang.IllegalAccessError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedInstantiationErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedInstantiationError = new Diagnostic(new DiagnosticId(113), nameof(EmittedInstantiationError), CompositeFormat.Parse("Emitted java.lang.InstantiationError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedInstantiationError = new Diagnostic(new DiagnosticId(113), nameof(EmittedInstantiationError), "Emitted java.lang.InstantiationError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedIncompatibleClassChangeErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedIncompatibleClassChangeError = new Diagnostic(new DiagnosticId(114), nameof(EmittedIncompatibleClassChangeError), CompositeFormat.Parse("Emitted java.lang.IncompatibleClassChangeError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedIncompatibleClassChangeError = new Diagnostic(new DiagnosticId(114), nameof(EmittedIncompatibleClassChangeError), "Emitted java.lang.IncompatibleClassChangeError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedNoSuchFieldErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedNoSuchFieldError = new Diagnostic(new DiagnosticId(115), nameof(EmittedNoSuchFieldError), CompositeFormat.Parse("Emitted java.lang.NoSuchFieldError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedNoSuchFieldError = new Diagnostic(new DiagnosticId(115), nameof(EmittedNoSuchFieldError), "Emitted java.lang.NoSuchFieldError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedAbstractMethodErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedAbstractMethodError = new Diagnostic(new DiagnosticId(116), nameof(EmittedAbstractMethodError), CompositeFormat.Parse("Emitted java.lang.AbstractMethodError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedAbstractMethodError = new Diagnostic(new DiagnosticId(116), nameof(EmittedAbstractMethodError), "Emitted java.lang.AbstractMethodError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedNoSuchMethodErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedNoSuchMethodError = new Diagnostic(new DiagnosticId(117), nameof(EmittedNoSuchMethodError), CompositeFormat.Parse("Emitted java.lang.NoSuchMethodError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedNoSuchMethodError = new Diagnostic(new DiagnosticId(117), nameof(EmittedNoSuchMethodError), "Emitted java.lang.NoSuchMethodError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedLinkageErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedLinkageError = new Diagnostic(new DiagnosticId(118), nameof(EmittedLinkageError), CompositeFormat.Parse("Emitted java.lang.LinkageError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedLinkageError = new Diagnostic(new DiagnosticId(118), nameof(EmittedLinkageError), "Emitted java.lang.LinkageError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedVerificationErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedVerificationError = new Diagnostic(new DiagnosticId(119), nameof(EmittedVerificationError), CompositeFormat.Parse("Emitted java.lang.VerificationError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedVerificationError = new Diagnostic(new DiagnosticId(119), nameof(EmittedVerificationError), "Emitted java.lang.VerificationError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct EmittedClassFormatErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic EmittedClassFormatError = new Diagnostic(new DiagnosticId(120), nameof(EmittedClassFormatError), CompositeFormat.Parse("Emitted java.lang.ClassFormatError in \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic EmittedClassFormatError = new Diagnostic(new DiagnosticId(120), nameof(EmittedClassFormatError), "Emitted java.lang.ClassFormatError in \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct InvalidCustomAttributeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidCustomAttribute = new Diagnostic(new DiagnosticId(121), nameof(InvalidCustomAttribute), CompositeFormat.Parse("Error emitting \"{0}\" custom attribute\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic InvalidCustomAttribute = new Diagnostic(new DiagnosticId(121), nameof(InvalidCustomAttribute), "Error emitting \"{0}\" custom attribute\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct IgnoredCustomAttributeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic IgnoredCustomAttribute = new Diagnostic(new DiagnosticId(122), nameof(IgnoredCustomAttribute), CompositeFormat.Parse("Custom attribute \"{0}\" was ignored\n    (\"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic IgnoredCustomAttribute = new Diagnostic(new DiagnosticId(122), nameof(IgnoredCustomAttribute), "Custom attribute \"{0}\" was ignored\n    (\"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct AssumeAssemblyVersionMatchData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic AssumeAssemblyVersionMatch = new Diagnostic(new DiagnosticId(123), nameof(AssumeAssemblyVersionMatch), CompositeFormat.Parse("Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime p" +
    "olicy"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic AssumeAssemblyVersionMatch = new Diagnostic(new DiagnosticId(123), nameof(AssumeAssemblyVersionMatch), "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime p" +
    "olicy", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct InvalidDirectoryInLibOptionPathData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidDirectoryInLibOptionPath = new Diagnostic(new DiagnosticId(124), nameof(InvalidDirectoryInLibOptionPath), CompositeFormat.Parse("Directory \"{0}\" specified in -lib option is not valid"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic InvalidDirectoryInLibOptionPath = new Diagnostic(new DiagnosticId(124), nameof(InvalidDirectoryInLibOptionPath), "Directory \"{0}\" specified in -lib option is not valid", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct InvalidDirectoryInLibEnvironmentPathData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidDirectoryInLibEnvironmentPath = new Diagnostic(new DiagnosticId(125), nameof(InvalidDirectoryInLibEnvironmentPath), CompositeFormat.Parse("Directory \"{0}\" specified in LIB environment is not valid"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic InvalidDirectoryInLibEnvironmentPath = new Diagnostic(new DiagnosticId(125), nameof(InvalidDirectoryInLibEnvironmentPath), "Directory \"{0}\" specified in LIB environment is not valid", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct LegacySearchRuleData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic LegacySearchRule = new Diagnostic(new DiagnosticId(126), nameof(LegacySearchRule), CompositeFormat.Parse("Found assembly \"{0}\" using legacy search rule, please append \'.dll\' to the refere" +
    "nce"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic LegacySearchRule = new Diagnostic(new DiagnosticId(126), nameof(LegacySearchRule), "Found assembly \"{0}\" using legacy search rule, please append \'.dll\' to the refere" +
    "nce", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct AssemblyLocationIgnoredData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic AssemblyLocationIgnored = new Diagnostic(new DiagnosticId(127), nameof(AssemblyLocationIgnored), CompositeFormat.Parse("Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identi" +
    "ty \"{2}\""), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic AssemblyLocationIgnored = new Diagnostic(new DiagnosticId(127), nameof(AssemblyLocationIgnored), "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identi" +
    "ty \"{2}\"", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct InterfaceMethodCantBeInternalData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InterfaceMethodCantBeInternal = new Diagnostic(new DiagnosticId(128), nameof(InterfaceMethodCantBeInternal), CompositeFormat.Parse("Ignoring @ikvm.lang.Internal annotation on interface method\n    (\"{0}.{1}{2}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic InterfaceMethodCantBeInternal = new Diagnostic(new DiagnosticId(128), nameof(InterfaceMethodCantBeInternal), "Ignoring @ikvm.lang.Internal annotation on interface method\n    (\"{0}.{1}{2}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct DuplicateAssemblyReferenceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DuplicateAssemblyReference = new Diagnostic(new DiagnosticId(132), nameof(DuplicateAssemblyReference), CompositeFormat.Parse("Duplicate assembly reference \"{0}\""), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic DuplicateAssemblyReference = new Diagnostic(new DiagnosticId(132), nameof(DuplicateAssemblyReference), "Duplicate assembly reference \"{0}\"", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct UnableToResolveTypeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnableToResolveType = new Diagnostic(new DiagnosticId(133), nameof(UnableToResolveType), CompositeFormat.Parse("Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not " +
    "be found"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic UnableToResolveType = new Diagnostic(new DiagnosticId(133), nameof(UnableToResolveType), "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not " +
    "be found", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct StubsAreDeprecatedData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic StubsAreDeprecated = new Diagnostic(new DiagnosticId(134), nameof(StubsAreDeprecated), CompositeFormat.Parse("Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead."), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic StubsAreDeprecated = new Diagnostic(new DiagnosticId(134), nameof(StubsAreDeprecated), "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct WrongClassNameData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic WrongClassName = new Diagnostic(new DiagnosticId(135), nameof(WrongClassName), CompositeFormat.Parse("Unable to compile \"{0}\" (wrong name: \"{1}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic WrongClassName = new Diagnostic(new DiagnosticId(135), nameof(WrongClassName), "Unable to compile \"{0}\" (wrong name: \"{1}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct ReflectionCallerClassRequiresCallerIDData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ReflectionCallerClassRequiresCallerID = new Diagnostic(new DiagnosticId(136), nameof(ReflectionCallerClassRequiresCallerID), CompositeFormat.Parse("Reflection.getCallerClass() called from non-CallerID method\n    (\"{0}.{1}{2}\")"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic ReflectionCallerClassRequiresCallerID = new Diagnostic(new DiagnosticId(136), nameof(ReflectionCallerClassRequiresCallerID), "Reflection.getCallerClass() called from non-CallerID method\n    (\"{0}.{1}{2}\")", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct LegacyAssemblyAttributesFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic LegacyAssemblyAttributesFound = new Diagnostic(new DiagnosticId(137), nameof(LegacyAssemblyAttributesFound), CompositeFormat.Parse("Legacy assembly attributes container found. Please use the -assemblyattributes:<f" +
    "ile> option."), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic LegacyAssemblyAttributesFound = new Diagnostic(new DiagnosticId(137), nameof(LegacyAssemblyAttributesFound), "Legacy assembly attributes container found. Please use the -assemblyattributes:<f" +
    "ile> option.", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct UnableToCreateLambdaFactoryData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnableToCreateLambdaFactory = new Diagnostic(new DiagnosticId(138), nameof(UnableToCreateLambdaFactory), CompositeFormat.Parse("Unable to create static lambda factory."), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic UnableToCreateLambdaFactory = new Diagnostic(new DiagnosticId(138), nameof(UnableToCreateLambdaFactory), "Unable to create static lambda factory.", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct UnknownWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnknownWarning = new Diagnostic(new DiagnosticId(999), nameof(UnknownWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic UnknownWarning = new Diagnostic(new DiagnosticId(999), nameof(UnknownWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct DuplicateIkvmLangPropertyData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DuplicateIkvmLangProperty = new Diagnostic(new DiagnosticId(139), nameof(DuplicateIkvmLangProperty), CompositeFormat.Parse("Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic DuplicateIkvmLangProperty = new Diagnostic(new DiagnosticId(139), nameof(DuplicateIkvmLangProperty), "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct MalformedIkvmLangPropertyData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MalformedIkvmLangProperty = new Diagnostic(new DiagnosticId(140), nameof(MalformedIkvmLangProperty), CompositeFormat.Parse("Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic MalformedIkvmLangProperty = new Diagnostic(new DiagnosticId(140), nameof(MalformedIkvmLangProperty), "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericCompilerWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericCompilerWarning = new Diagnostic(new DiagnosticId(5), nameof(GenericCompilerWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericCompilerWarning = new Diagnostic(new DiagnosticId(5), nameof(GenericCompilerWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericClassLoadingWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericClassLoadingWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericClassLoadingWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericClassLoadingWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericClassLoadingWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericVerifierWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericVerifierWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericVerifierWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericVerifierWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericVerifierWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericRuntimeWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericRuntimeWarning = new Diagnostic(new DiagnosticId(7), nameof(GenericRuntimeWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericRuntimeWarning = new Diagnostic(new DiagnosticId(7), nameof(GenericRuntimeWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct GenericJniWarningData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericJniWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericJniWarning), CompositeFormat.Parse("{0}"), DiagnosticLevel.Warning);
#else
        public static readonly Diagnostic GenericJniWarning = new Diagnostic(new DiagnosticId(6), nameof(GenericJniWarning), "{0}", DiagnosticLevel.Warning);
#endif

    }

    readonly record struct UnableToCreateProxyData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnableToCreateProxy = new Diagnostic(new DiagnosticId(4001), nameof(UnableToCreateProxy), CompositeFormat.Parse("Unable to create proxy \"{0}\"\n    (\"{1}\")"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic UnableToCreateProxy = new Diagnostic(new DiagnosticId(4001), nameof(UnableToCreateProxy), "Unable to create proxy \"{0}\"\n    (\"{1}\")", DiagnosticLevel.Error);
#endif

    }

    readonly record struct DuplicateProxyData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DuplicateProxy = new Diagnostic(new DiagnosticId(4002), nameof(DuplicateProxy), CompositeFormat.Parse("Duplicate proxy \"{0}\""), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic DuplicateProxy = new Diagnostic(new DiagnosticId(4002), nameof(DuplicateProxy), "Duplicate proxy \"{0}\"", DiagnosticLevel.Error);
#endif

    }

    readonly record struct MapXmlUnableToResolveOpCodeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MapXmlUnableToResolveOpCode = new Diagnostic(new DiagnosticId(4003), nameof(MapXmlUnableToResolveOpCode), CompositeFormat.Parse("Unable to resolve opcode in remap file: {0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic MapXmlUnableToResolveOpCode = new Diagnostic(new DiagnosticId(4003), nameof(MapXmlUnableToResolveOpCode), "Unable to resolve opcode in remap file: {0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct MapXmlErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MapXmlError = new Diagnostic(new DiagnosticId(4004), nameof(MapXmlError), CompositeFormat.Parse("Error in remap file: {0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic MapXmlError = new Diagnostic(new DiagnosticId(4004), nameof(MapXmlError), "Error in remap file: {0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct InputFileNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InputFileNotFound = new Diagnostic(new DiagnosticId(4005), nameof(InputFileNotFound), CompositeFormat.Parse("Source file \'{0}\' not found"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic InputFileNotFound = new Diagnostic(new DiagnosticId(4005), nameof(InputFileNotFound), "Source file \'{0}\' not found", DiagnosticLevel.Error);
#endif

    }

    readonly record struct UnknownFileTypeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnknownFileType = new Diagnostic(new DiagnosticId(4006), nameof(UnknownFileType), CompositeFormat.Parse("Unknown file type: {0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic UnknownFileType = new Diagnostic(new DiagnosticId(4006), nameof(UnknownFileType), "Unknown file type: {0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct UnknownElementInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnknownElementInMapFile = new Diagnostic(new DiagnosticId(4007), nameof(UnknownElementInMapFile), CompositeFormat.Parse("Unknown element {0} in remap file, line {1}, column {2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic UnknownElementInMapFile = new Diagnostic(new DiagnosticId(4007), nameof(UnknownElementInMapFile), "Unknown element {0} in remap file, line {1}, column {2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct UnknownAttributeInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnknownAttributeInMapFile = new Diagnostic(new DiagnosticId(4008), nameof(UnknownAttributeInMapFile), CompositeFormat.Parse("Unknown attribute {0} in remap file, line {1}, column {2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic UnknownAttributeInMapFile = new Diagnostic(new DiagnosticId(4008), nameof(UnknownAttributeInMapFile), "Unknown attribute {0} in remap file, line {1}, column {2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct InvalidMemberNameInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidMemberNameInMapFile = new Diagnostic(new DiagnosticId(4009), nameof(InvalidMemberNameInMapFile), CompositeFormat.Parse("Invalid {0} name \'{1}\' in remap file in class {2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic InvalidMemberNameInMapFile = new Diagnostic(new DiagnosticId(4009), nameof(InvalidMemberNameInMapFile), "Invalid {0} name \'{1}\' in remap file in class {2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct InvalidMemberSignatureInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidMemberSignatureInMapFile = new Diagnostic(new DiagnosticId(4010), nameof(InvalidMemberSignatureInMapFile), CompositeFormat.Parse("Invalid {0} signature \'{3}\' in remap file for {0} {1}.{2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic InvalidMemberSignatureInMapFile = new Diagnostic(new DiagnosticId(4010), nameof(InvalidMemberSignatureInMapFile), "Invalid {0} signature \'{3}\' in remap file for {0} {1}.{2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct InvalidPropertyNameInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidPropertyNameInMapFile = new Diagnostic(new DiagnosticId(4011), nameof(InvalidPropertyNameInMapFile), CompositeFormat.Parse("Invalid property {0} name \'{3}\' in remap file for property {1}.{2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic InvalidPropertyNameInMapFile = new Diagnostic(new DiagnosticId(4011), nameof(InvalidPropertyNameInMapFile), "Invalid property {0} name \'{3}\' in remap file for property {1}.{2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct InvalidPropertySignatureInMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidPropertySignatureInMapFile = new Diagnostic(new DiagnosticId(4012), nameof(InvalidPropertySignatureInMapFile), CompositeFormat.Parse("Invalid property {0} signature \'{3}\' in remap file for property {1}.{2}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic InvalidPropertySignatureInMapFile = new Diagnostic(new DiagnosticId(4012), nameof(InvalidPropertySignatureInMapFile), "Invalid property {0} signature \'{3}\' in remap file for property {1}.{2}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct NonPrimaryAssemblyReferenceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic NonPrimaryAssemblyReference = new Diagnostic(new DiagnosticId(4013), nameof(NonPrimaryAssemblyReference), CompositeFormat.Parse("Referenced assembly \"{0}\" is not the primary assembly of a shared class loader gr" +
    "oup, please reference primary assembly \"{1}\" instead"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic NonPrimaryAssemblyReference = new Diagnostic(new DiagnosticId(4013), nameof(NonPrimaryAssemblyReference), "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader gr" +
    "oup, please reference primary assembly \"{1}\" instead", DiagnosticLevel.Error);
#endif

    }

    readonly record struct MissingTypeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MissingType = new Diagnostic(new DiagnosticId(4014), nameof(MissingType), CompositeFormat.Parse("Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic MissingType = new Diagnostic(new DiagnosticId(4014), nameof(MissingType), "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found", DiagnosticLevel.Error);
#endif

    }

    readonly record struct MissingReferenceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MissingReference = new Diagnostic(new DiagnosticId(4015), nameof(MissingReference), CompositeFormat.Parse("The type \'{0}\' is defined in an assembly that is notResponseFileDepthExceeded ref" +
    "erenced. You must add a reference to assembly \'{1}\'"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic MissingReference = new Diagnostic(new DiagnosticId(4015), nameof(MissingReference), "The type \'{0}\' is defined in an assembly that is notResponseFileDepthExceeded ref" +
    "erenced. You must add a reference to assembly \'{1}\'", DiagnosticLevel.Error);
#endif

    }

    readonly record struct CallerSensitiveOnUnsupportedMethodData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic CallerSensitiveOnUnsupportedMethod = new Diagnostic(new DiagnosticId(4016), nameof(CallerSensitiveOnUnsupportedMethod), CompositeFormat.Parse("CallerSensitive annotation on unsupported method\n    (\"{0}.{1}{2}\")"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic CallerSensitiveOnUnsupportedMethod = new Diagnostic(new DiagnosticId(4016), nameof(CallerSensitiveOnUnsupportedMethod), "CallerSensitive annotation on unsupported method\n    (\"{0}.{1}{2}\")", DiagnosticLevel.Error);
#endif

    }

    readonly record struct RemappedTypeMissingDefaultInterfaceMethodData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic RemappedTypeMissingDefaultInterfaceMethod = new Diagnostic(new DiagnosticId(4017), nameof(RemappedTypeMissingDefaultInterfaceMethod), CompositeFormat.Parse("{0} does not implement default interface method {1}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic RemappedTypeMissingDefaultInterfaceMethod = new Diagnostic(new DiagnosticId(4017), nameof(RemappedTypeMissingDefaultInterfaceMethod), "{0} does not implement default interface method {1}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct GenericCompilerErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericCompilerError = new Diagnostic(new DiagnosticId(4018), nameof(GenericCompilerError), CompositeFormat.Parse("{0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic GenericCompilerError = new Diagnostic(new DiagnosticId(4018), nameof(GenericCompilerError), "{0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct GenericClassLoadingErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericClassLoadingError = new Diagnostic(new DiagnosticId(4019), nameof(GenericClassLoadingError), CompositeFormat.Parse("{0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic GenericClassLoadingError = new Diagnostic(new DiagnosticId(4019), nameof(GenericClassLoadingError), "{0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct GenericVerifierErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericVerifierError = new Diagnostic(new DiagnosticId(4020), nameof(GenericVerifierError), CompositeFormat.Parse("{0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic GenericVerifierError = new Diagnostic(new DiagnosticId(4020), nameof(GenericVerifierError), "{0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct GenericRuntimeErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericRuntimeError = new Diagnostic(new DiagnosticId(4021), nameof(GenericRuntimeError), CompositeFormat.Parse("{0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic GenericRuntimeError = new Diagnostic(new DiagnosticId(4021), nameof(GenericRuntimeError), "{0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct GenericJniErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericJniError = new Diagnostic(new DiagnosticId(4022), nameof(GenericJniError), CompositeFormat.Parse("{0}"), DiagnosticLevel.Error);
#else
        public static readonly Diagnostic GenericJniError = new Diagnostic(new DiagnosticId(4022), nameof(GenericJniError), "{0}", DiagnosticLevel.Error);
#endif

    }

    readonly record struct ResponseFileDepthExceededData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ResponseFileDepthExceeded = new Diagnostic(new DiagnosticId(5000), nameof(ResponseFileDepthExceeded), CompositeFormat.Parse("Response file nesting depth exceeded"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ResponseFileDepthExceeded = new Diagnostic(new DiagnosticId(5000), nameof(ResponseFileDepthExceeded), "Response file nesting depth exceeded", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ErrorReadingFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ErrorReadingFile = new Diagnostic(new DiagnosticId(5001), nameof(ErrorReadingFile), CompositeFormat.Parse("Unable to read file: {0}\n\t({1})"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ErrorReadingFile = new Diagnostic(new DiagnosticId(5001), nameof(ErrorReadingFile), "Unable to read file: {0}\n\t({1})", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct NoTargetsFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic NoTargetsFound = new Diagnostic(new DiagnosticId(5002), nameof(NoTargetsFound), CompositeFormat.Parse("No targets found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic NoTargetsFound = new Diagnostic(new DiagnosticId(5002), nameof(NoTargetsFound), "No targets found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct FileFormatLimitationExceededData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic FileFormatLimitationExceeded = new Diagnostic(new DiagnosticId(5003), nameof(FileFormatLimitationExceeded), CompositeFormat.Parse("File format limitation exceeded: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic FileFormatLimitationExceeded = new Diagnostic(new DiagnosticId(5003), nameof(FileFormatLimitationExceeded), "File format limitation exceeded: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct CannotSpecifyBothKeyFileAndContainerData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic CannotSpecifyBothKeyFileAndContainer = new Diagnostic(new DiagnosticId(5004), nameof(CannotSpecifyBothKeyFileAndContainer), CompositeFormat.Parse("You cannot specify both a key file and container"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic CannotSpecifyBothKeyFileAndContainer = new Diagnostic(new DiagnosticId(5004), nameof(CannotSpecifyBothKeyFileAndContainer), "You cannot specify both a key file and container", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct DelaySignRequiresKeyData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic DelaySignRequiresKey = new Diagnostic(new DiagnosticId(5005), nameof(DelaySignRequiresKey), CompositeFormat.Parse("You cannot delay sign without a key file or container"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic DelaySignRequiresKey = new Diagnostic(new DiagnosticId(5005), nameof(DelaySignRequiresKey), "You cannot delay sign without a key file or container", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct InvalidStrongNameKeyPairData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidStrongNameKeyPair = new Diagnostic(new DiagnosticId(5006), nameof(InvalidStrongNameKeyPair), CompositeFormat.Parse("Invalid key {0} specified.\n\t(\"{1}\")"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic InvalidStrongNameKeyPair = new Diagnostic(new DiagnosticId(5006), nameof(InvalidStrongNameKeyPair), "Invalid key {0} specified.\n\t(\"{1}\")", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ReferenceNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ReferenceNotFound = new Diagnostic(new DiagnosticId(5007), nameof(ReferenceNotFound), CompositeFormat.Parse("Reference not found: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ReferenceNotFound = new Diagnostic(new DiagnosticId(5007), nameof(ReferenceNotFound), "Reference not found: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct OptionsMustPreceedChildLevelsData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic OptionsMustPreceedChildLevels = new Diagnostic(new DiagnosticId(5008), nameof(OptionsMustPreceedChildLevels), CompositeFormat.Parse("You can only specify options before any child levels"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic OptionsMustPreceedChildLevels = new Diagnostic(new DiagnosticId(5008), nameof(OptionsMustPreceedChildLevels), "You can only specify options before any child levels", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnrecognizedTargetTypeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnrecognizedTargetType = new Diagnostic(new DiagnosticId(5009), nameof(UnrecognizedTargetType), CompositeFormat.Parse("Invalid value \'{0}\' for -target option"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnrecognizedTargetType = new Diagnostic(new DiagnosticId(5009), nameof(UnrecognizedTargetType), "Invalid value \'{0}\' for -target option", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnrecognizedPlatformData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnrecognizedPlatform = new Diagnostic(new DiagnosticId(5010), nameof(UnrecognizedPlatform), CompositeFormat.Parse("Invalid value \'{0}\' for -platform option"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnrecognizedPlatform = new Diagnostic(new DiagnosticId(5010), nameof(UnrecognizedPlatform), "Invalid value \'{0}\' for -platform option", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnrecognizedApartmentData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnrecognizedApartment = new Diagnostic(new DiagnosticId(5011), nameof(UnrecognizedApartment), CompositeFormat.Parse("Invalid value \'{0}\' for -apartment option"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnrecognizedApartment = new Diagnostic(new DiagnosticId(5011), nameof(UnrecognizedApartment), "Invalid value \'{0}\' for -apartment option", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MissingFileSpecificationData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MissingFileSpecification = new Diagnostic(new DiagnosticId(5012), nameof(MissingFileSpecification), CompositeFormat.Parse("Missing file specification for \'{0}\' option"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MissingFileSpecification = new Diagnostic(new DiagnosticId(5012), nameof(MissingFileSpecification), "Missing file specification for \'{0}\' option", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct PathTooLongData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic PathTooLong = new Diagnostic(new DiagnosticId(5013), nameof(PathTooLong), CompositeFormat.Parse("Path too long: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic PathTooLong = new Diagnostic(new DiagnosticId(5013), nameof(PathTooLong), "Path too long: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct PathNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic PathNotFound = new Diagnostic(new DiagnosticId(5014), nameof(PathNotFound), CompositeFormat.Parse("Path not found: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic PathNotFound = new Diagnostic(new DiagnosticId(5014), nameof(PathNotFound), "Path not found: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct InvalidPathData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidPath = new Diagnostic(new DiagnosticId(5015), nameof(InvalidPath), CompositeFormat.Parse("Invalid path: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic InvalidPath = new Diagnostic(new DiagnosticId(5015), nameof(InvalidPath), "Invalid path: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct InvalidOptionSyntaxData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidOptionSyntax = new Diagnostic(new DiagnosticId(5016), nameof(InvalidOptionSyntax), CompositeFormat.Parse("Invalid option: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic InvalidOptionSyntax = new Diagnostic(new DiagnosticId(5016), nameof(InvalidOptionSyntax), "Invalid option: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ExternalResourceNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ExternalResourceNotFound = new Diagnostic(new DiagnosticId(5017), nameof(ExternalResourceNotFound), CompositeFormat.Parse("External resource file does not exist: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ExternalResourceNotFound = new Diagnostic(new DiagnosticId(5017), nameof(ExternalResourceNotFound), "External resource file does not exist: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ExternalResourceNameInvalidData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ExternalResourceNameInvalid = new Diagnostic(new DiagnosticId(5018), nameof(ExternalResourceNameInvalid), CompositeFormat.Parse("External resource file may not include path specification: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ExternalResourceNameInvalid = new Diagnostic(new DiagnosticId(5018), nameof(ExternalResourceNameInvalid), "External resource file may not include path specification: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct InvalidVersionFormatData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidVersionFormat = new Diagnostic(new DiagnosticId(5019), nameof(InvalidVersionFormat), CompositeFormat.Parse("Invalid version specified: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic InvalidVersionFormat = new Diagnostic(new DiagnosticId(5019), nameof(InvalidVersionFormat), "Invalid version specified: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct InvalidFileAlignmentData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic InvalidFileAlignment = new Diagnostic(new DiagnosticId(5020), nameof(InvalidFileAlignment), CompositeFormat.Parse("Invalid value \'{0}\' for -filealign option"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic InvalidFileAlignment = new Diagnostic(new DiagnosticId(5020), nameof(InvalidFileAlignment), "Invalid value \'{0}\' for -filealign option", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ErrorWritingFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ErrorWritingFile = new Diagnostic(new DiagnosticId(5021), nameof(ErrorWritingFile), CompositeFormat.Parse("Unable to write file: {0}\n\t({1})"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ErrorWritingFile = new Diagnostic(new DiagnosticId(5021), nameof(ErrorWritingFile), "Unable to write file: {0}\n\t({1})", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnrecognizedOptionData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnrecognizedOption = new Diagnostic(new DiagnosticId(5022), nameof(UnrecognizedOption), CompositeFormat.Parse("Unrecognized option: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnrecognizedOption = new Diagnostic(new DiagnosticId(5022), nameof(UnrecognizedOption), "Unrecognized option: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct NoOutputFileSpecifiedData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic NoOutputFileSpecified = new Diagnostic(new DiagnosticId(5023), nameof(NoOutputFileSpecified), CompositeFormat.Parse("No output file specified"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic NoOutputFileSpecified = new Diagnostic(new DiagnosticId(5023), nameof(NoOutputFileSpecified), "No output file specified", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct SharedClassLoaderCannotBeUsedOnModuleTargetData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic SharedClassLoaderCannotBeUsedOnModuleTarget = new Diagnostic(new DiagnosticId(5024), nameof(SharedClassLoaderCannotBeUsedOnModuleTarget), CompositeFormat.Parse("Incompatible options: -target:module and -sharedclassloader cannot be combined"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic SharedClassLoaderCannotBeUsedOnModuleTarget = new Diagnostic(new DiagnosticId(5024), nameof(SharedClassLoaderCannotBeUsedOnModuleTarget), "Incompatible options: -target:module and -sharedclassloader cannot be combined", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct RuntimeNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic RuntimeNotFound = new Diagnostic(new DiagnosticId(5025), nameof(RuntimeNotFound), CompositeFormat.Parse("Unable to load runtime assembly"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic RuntimeNotFound = new Diagnostic(new DiagnosticId(5025), nameof(RuntimeNotFound), "Unable to load runtime assembly", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MainClassRequiresExeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MainClassRequiresExe = new Diagnostic(new DiagnosticId(5026), nameof(MainClassRequiresExe), CompositeFormat.Parse("Main class cannot be specified for library or module"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MainClassRequiresExe = new Diagnostic(new DiagnosticId(5026), nameof(MainClassRequiresExe), "Main class cannot be specified for library or module", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ExeRequiresMainClassData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ExeRequiresMainClass = new Diagnostic(new DiagnosticId(5027), nameof(ExeRequiresMainClass), CompositeFormat.Parse("No main method found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ExeRequiresMainClass = new Diagnostic(new DiagnosticId(5027), nameof(ExeRequiresMainClass), "No main method found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct PropertiesRequireExeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic PropertiesRequireExe = new Diagnostic(new DiagnosticId(5028), nameof(PropertiesRequireExe), CompositeFormat.Parse("Properties cannot be specified for library or module"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic PropertiesRequireExe = new Diagnostic(new DiagnosticId(5028), nameof(PropertiesRequireExe), "Properties cannot be specified for library or module", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ModuleCannotHaveClassLoaderData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ModuleCannotHaveClassLoader = new Diagnostic(new DiagnosticId(5029), nameof(ModuleCannotHaveClassLoader), CompositeFormat.Parse("Cannot specify assembly class loader for modules"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ModuleCannotHaveClassLoader = new Diagnostic(new DiagnosticId(5029), nameof(ModuleCannotHaveClassLoader), "Cannot specify assembly class loader for modules", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ErrorParsingMapFileData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ErrorParsingMapFile = new Diagnostic(new DiagnosticId(5030), nameof(ErrorParsingMapFile), CompositeFormat.Parse("Unable to parse remap file: {0}\n\t({1})"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ErrorParsingMapFile = new Diagnostic(new DiagnosticId(5030), nameof(ErrorParsingMapFile), "Unable to parse remap file: {0}\n\t({1})", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct BootstrapClassesMissingData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic BootstrapClassesMissing = new Diagnostic(new DiagnosticId(5031), nameof(BootstrapClassesMissing), CompositeFormat.Parse("Bootstrap classes missing and core assembly not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic BootstrapClassesMissing = new Diagnostic(new DiagnosticId(5031), nameof(BootstrapClassesMissing), "Bootstrap classes missing and core assembly not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct StrongNameRequiresStrongNamedRefsData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic StrongNameRequiresStrongNamedRefs = new Diagnostic(new DiagnosticId(5032), nameof(StrongNameRequiresStrongNamedRefs), CompositeFormat.Parse("All referenced assemblies must be strong named, to be able to sign the output ass" +
    "embly"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic StrongNameRequiresStrongNamedRefs = new Diagnostic(new DiagnosticId(5032), nameof(StrongNameRequiresStrongNamedRefs), "All referenced assemblies must be strong named, to be able to sign the output ass" +
    "embly", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MainClassNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MainClassNotFound = new Diagnostic(new DiagnosticId(5033), nameof(MainClassNotFound), CompositeFormat.Parse("Main class not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MainClassNotFound = new Diagnostic(new DiagnosticId(5033), nameof(MainClassNotFound), "Main class not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MainMethodNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MainMethodNotFound = new Diagnostic(new DiagnosticId(5034), nameof(MainMethodNotFound), CompositeFormat.Parse("Main method not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MainMethodNotFound = new Diagnostic(new DiagnosticId(5034), nameof(MainMethodNotFound), "Main method not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnsupportedMainMethodData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnsupportedMainMethod = new Diagnostic(new DiagnosticId(5035), nameof(UnsupportedMainMethod), CompositeFormat.Parse("Redirected main method not supported"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnsupportedMainMethod = new Diagnostic(new DiagnosticId(5035), nameof(UnsupportedMainMethod), "Redirected main method not supported", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ExternalMainNotAccessibleData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ExternalMainNotAccessible = new Diagnostic(new DiagnosticId(5036), nameof(ExternalMainNotAccessible), CompositeFormat.Parse("External main method must be public and in a public class"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ExternalMainNotAccessible = new Diagnostic(new DiagnosticId(5036), nameof(ExternalMainNotAccessible), "External main method must be public and in a public class", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ClassLoaderNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassLoaderNotFound = new Diagnostic(new DiagnosticId(5037), nameof(ClassLoaderNotFound), CompositeFormat.Parse("Custom assembly class loader class not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ClassLoaderNotFound = new Diagnostic(new DiagnosticId(5037), nameof(ClassLoaderNotFound), "Custom assembly class loader class not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ClassLoaderNotAccessibleData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassLoaderNotAccessible = new Diagnostic(new DiagnosticId(5038), nameof(ClassLoaderNotAccessible), CompositeFormat.Parse("Custom assembly class loader class is not accessible"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ClassLoaderNotAccessible = new Diagnostic(new DiagnosticId(5038), nameof(ClassLoaderNotAccessible), "Custom assembly class loader class is not accessible", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ClassLoaderIsAbstractData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassLoaderIsAbstract = new Diagnostic(new DiagnosticId(5039), nameof(ClassLoaderIsAbstract), CompositeFormat.Parse("Custom assembly class loader class is abstract"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ClassLoaderIsAbstract = new Diagnostic(new DiagnosticId(5039), nameof(ClassLoaderIsAbstract), "Custom assembly class loader class is abstract", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ClassLoaderNotClassLoaderData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassLoaderNotClassLoader = new Diagnostic(new DiagnosticId(5040), nameof(ClassLoaderNotClassLoader), CompositeFormat.Parse("Custom assembly class loader class does not extend java.lang.ClassLoader"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ClassLoaderNotClassLoader = new Diagnostic(new DiagnosticId(5040), nameof(ClassLoaderNotClassLoader), "Custom assembly class loader class does not extend java.lang.ClassLoader", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ClassLoaderConstructorMissingData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ClassLoaderConstructorMissing = new Diagnostic(new DiagnosticId(5041), nameof(ClassLoaderConstructorMissing), CompositeFormat.Parse("Custom assembly class loader constructor is missing"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ClassLoaderConstructorMissing = new Diagnostic(new DiagnosticId(5041), nameof(ClassLoaderConstructorMissing), "Custom assembly class loader constructor is missing", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MapFileTypeNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MapFileTypeNotFound = new Diagnostic(new DiagnosticId(5042), nameof(MapFileTypeNotFound), CompositeFormat.Parse("Type \'{0}\' referenced in remap file was not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MapFileTypeNotFound = new Diagnostic(new DiagnosticId(5042), nameof(MapFileTypeNotFound), "Type \'{0}\' referenced in remap file was not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MapFileClassNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MapFileClassNotFound = new Diagnostic(new DiagnosticId(5043), nameof(MapFileClassNotFound), CompositeFormat.Parse("Class \'{0}\' referenced in remap file was not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MapFileClassNotFound = new Diagnostic(new DiagnosticId(5043), nameof(MapFileClassNotFound), "Class \'{0}\' referenced in remap file was not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MaximumErrorCountReachedData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MaximumErrorCountReached = new Diagnostic(new DiagnosticId(5044), nameof(MaximumErrorCountReached), CompositeFormat.Parse("Maximum error count reached"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MaximumErrorCountReached = new Diagnostic(new DiagnosticId(5044), nameof(MaximumErrorCountReached), "Maximum error count reached", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct LinkageErrorData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic LinkageError = new Diagnostic(new DiagnosticId(5045), nameof(LinkageError), CompositeFormat.Parse("Link error: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic LinkageError = new Diagnostic(new DiagnosticId(5045), nameof(LinkageError), "Link error: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct RuntimeMismatchData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic RuntimeMismatch = new Diagnostic(new DiagnosticId(5046), nameof(RuntimeMismatch), CompositeFormat.Parse("Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version\n\tC" +
    "urrent runtime: {1}\n\tReferenced assembly runtime: {2}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic RuntimeMismatch = new Diagnostic(new DiagnosticId(5046), nameof(RuntimeMismatch), "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version\n\tC" +
    "urrent runtime: {1}\n\tReferenced assembly runtime: {2}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct RuntimeMismatchStrongNameData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic RuntimeMismatchStrongName = new Diagnostic(new DiagnosticId(5047), nameof(RuntimeMismatchStrongName), CompositeFormat.Parse(""), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic RuntimeMismatchStrongName = new Diagnostic(new DiagnosticId(5047), nameof(RuntimeMismatchStrongName), "", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct CoreClassesMissingData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic CoreClassesMissing = new Diagnostic(new DiagnosticId(5048), nameof(CoreClassesMissing), CompositeFormat.Parse("Failed to find core classes in core library"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic CoreClassesMissing = new Diagnostic(new DiagnosticId(5048), nameof(CoreClassesMissing), "Failed to find core classes in core library", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct CriticalClassNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic CriticalClassNotFound = new Diagnostic(new DiagnosticId(5049), nameof(CriticalClassNotFound), CompositeFormat.Parse("Unable to load critical class \'{0}\'"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic CriticalClassNotFound = new Diagnostic(new DiagnosticId(5049), nameof(CriticalClassNotFound), "Unable to load critical class \'{0}\'", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct AssemblyContainsDuplicateClassNamesData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic AssemblyContainsDuplicateClassNames = new Diagnostic(new DiagnosticId(5050), nameof(AssemblyContainsDuplicateClassNames), CompositeFormat.Parse("Type \'{0}\' and \'{1}\' both map to the same name \'{2}\'\n\t({3})"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic AssemblyContainsDuplicateClassNames = new Diagnostic(new DiagnosticId(5050), nameof(AssemblyContainsDuplicateClassNames), "Type \'{0}\' and \'{1}\' both map to the same name \'{2}\'\n\t({3})", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct CallerIDRequiresHasCallerIDAnnotationData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic CallerIDRequiresHasCallerIDAnnotation = new Diagnostic(new DiagnosticId(5051), nameof(CallerIDRequiresHasCallerIDAnnotation), CompositeFormat.Parse("CallerID.getCallerID() requires a HasCallerID annotation"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic CallerIDRequiresHasCallerIDAnnotation = new Diagnostic(new DiagnosticId(5051), nameof(CallerIDRequiresHasCallerIDAnnotation), "CallerID.getCallerID() requires a HasCallerID annotation", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct UnableToResolveInterfaceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic UnableToResolveInterface = new Diagnostic(new DiagnosticId(5052), nameof(UnableToResolveInterface), CompositeFormat.Parse("Unable to resolve interface \'{0}\' on type \'{1}\'"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic UnableToResolveInterface = new Diagnostic(new DiagnosticId(5052), nameof(UnableToResolveInterface), "Unable to resolve interface \'{0}\' on type \'{1}\'", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MissingBaseTypeData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MissingBaseType = new Diagnostic(new DiagnosticId(5053), nameof(MissingBaseType), CompositeFormat.Parse("The base class or interface \'{0}\' in assembly \'{1}\' referenced by type \'{2}\' in \'" +
    "{3}\' could not be resolved"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MissingBaseType = new Diagnostic(new DiagnosticId(5053), nameof(MissingBaseType), "The base class or interface \'{0}\' in assembly \'{1}\' referenced by type \'{2}\' in \'" +
    "{3}\' could not be resolved", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MissingBaseTypeReferenceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MissingBaseTypeReference = new Diagnostic(new DiagnosticId(5054), nameof(MissingBaseTypeReference), CompositeFormat.Parse("The type \'{0}\' is defined in an assembly that is not referenced. You must add a r" +
    "eference to assembly \'{1}\'"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MissingBaseTypeReference = new Diagnostic(new DiagnosticId(5054), nameof(MissingBaseTypeReference), "The type \'{0}\' is defined in an assembly that is not referenced. You must add a r" +
    "eference to assembly \'{1}\'", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct FileNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic FileNotFound = new Diagnostic(new DiagnosticId(5055), nameof(FileNotFound), CompositeFormat.Parse("File not found: {0}"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic FileNotFound = new Diagnostic(new DiagnosticId(5055), nameof(FileNotFound), "File not found: {0}", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct RuntimeMethodMissingData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic RuntimeMethodMissing = new Diagnostic(new DiagnosticId(5056), nameof(RuntimeMethodMissing), CompositeFormat.Parse("Runtime method \'{0}\' not found"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic RuntimeMethodMissing = new Diagnostic(new DiagnosticId(5056), nameof(RuntimeMethodMissing), "Runtime method \'{0}\' not found", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct MapFileFieldNotFoundData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic MapFileFieldNotFound = new Diagnostic(new DiagnosticId(5057), nameof(MapFileFieldNotFound), CompositeFormat.Parse("Field \'{0}\' referenced in remap file was not found in class \'{1}\'"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic MapFileFieldNotFound = new Diagnostic(new DiagnosticId(5057), nameof(MapFileFieldNotFound), "Field \'{0}\' referenced in remap file was not found in class \'{1}\'", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct GhostInterfaceMethodMissingData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GhostInterfaceMethodMissing = new Diagnostic(new DiagnosticId(5058), nameof(GhostInterfaceMethodMissing), CompositeFormat.Parse("Remapped class \'{0}\' does not implement ghost interface method\n\t({1}.{2}{3})"), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic GhostInterfaceMethodMissing = new Diagnostic(new DiagnosticId(5058), nameof(GhostInterfaceMethodMissing), "Remapped class \'{0}\' does not implement ghost interface method\n\t({1}.{2}{3})", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct ModuleInitializerMethodRequirementsData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic ModuleInitializerMethodRequirements = new Diagnostic(new DiagnosticId(5059), nameof(ModuleInitializerMethodRequirements), CompositeFormat.Parse("Method \'{1}.{2}{3}\' does not meet the requirements of a module initializer."), DiagnosticLevel.Fatal);
#else
        public static readonly Diagnostic ModuleInitializerMethodRequirements = new Diagnostic(new DiagnosticId(5059), nameof(ModuleInitializerMethodRequirements), "Method \'{1}.{2}{3}\' does not meet the requirements of a module initializer.", DiagnosticLevel.Fatal);
#endif

    }

    readonly record struct GenericRuntimeTraceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericRuntimeTrace = new Diagnostic(new DiagnosticId(6000), nameof(GenericRuntimeTrace), CompositeFormat.Parse("{0}"), DiagnosticLevel.Trace);
#else
        public static readonly Diagnostic GenericRuntimeTrace = new Diagnostic(new DiagnosticId(6000), nameof(GenericRuntimeTrace), "{0}", DiagnosticLevel.Trace);
#endif

    }

    readonly record struct GenericJniTraceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericJniTrace = new Diagnostic(new DiagnosticId(6001), nameof(GenericJniTrace), CompositeFormat.Parse("{0}"), DiagnosticLevel.Trace);
#else
        public static readonly Diagnostic GenericJniTrace = new Diagnostic(new DiagnosticId(6001), nameof(GenericJniTrace), "{0}", DiagnosticLevel.Trace);
#endif

    }

    readonly record struct GenericCompilerTraceData()
    {
    
    }

    partial record class Diagnostic
    {

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
#if NET8_0_OR_GREATER
        public static readonly Diagnostic GenericCompilerTrace = new Diagnostic(new DiagnosticId(6002), nameof(GenericCompilerTrace), CompositeFormat.Parse("{0}"), DiagnosticLevel.Trace);
#else
        public static readonly Diagnostic GenericCompilerTrace = new Diagnostic(new DiagnosticId(6002), nameof(GenericCompilerTrace), "{0}", DiagnosticLevel.Trace);
#endif

    }


}
