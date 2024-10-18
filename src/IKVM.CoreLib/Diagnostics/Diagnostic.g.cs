#nullable enable

using System;
using System.Text;

namespace IKVM.CoreLib.Diagnostics
{

    partial record class Diagnostic
    {

        public static Diagnostic? GetById(int id)
        {
            switch (id)
            {
                case 1:
                    return MainMethodFound;
                case 2:
                    return OutputFileIs;
                case 3:
                    return AutoAddRef;
                case 4:
                    return MainMethodFromManifest;
                case 5:
                    return GenericCompilerInfo;
                case 6:
                    return GenericClassLoadingInfo;
                case 7:
                    return GenericVerifierInfo;
                case 8:
                    return GenericRuntimeInfo;
                case 9:
                    return GenericJniInfo;
                case 100:
                    return ClassNotFound;
                case 101:
                    return ClassFormatError;
                case 102:
                    return DuplicateClassName;
                case 103:
                    return IllegalAccessError;
                case 104:
                    return VerificationError;
                case 105:
                    return NoClassDefFoundError;
                case 106:
                    return GenericUnableToCompileError;
                case 107:
                    return DuplicateResourceName;
                case 109:
                    return SkippingReferencedClass;
                case 110:
                    return NoJniRuntime;
                case 111:
                    return EmittedNoClassDefFoundError;
                case 112:
                    return EmittedIllegalAccessError;
                case 113:
                    return EmittedInstantiationError;
                case 114:
                    return EmittedIncompatibleClassChangeError;
                case 115:
                    return EmittedNoSuchFieldError;
                case 116:
                    return EmittedAbstractMethodError;
                case 117:
                    return EmittedNoSuchMethodError;
                case 118:
                    return EmittedLinkageError;
                case 119:
                    return EmittedVerificationError;
                case 120:
                    return EmittedClassFormatError;
                case 121:
                    return InvalidCustomAttribute;
                case 122:
                    return IgnoredCustomAttribute;
                case 123:
                    return AssumeAssemblyVersionMatch;
                case 124:
                    return InvalidDirectoryInLibOptionPath;
                case 125:
                    return InvalidDirectoryInLibEnvironmentPath;
                case 126:
                    return LegacySearchRule;
                case 127:
                    return AssemblyLocationIgnored;
                case 128:
                    return InterfaceMethodCantBeInternal;
                case 132:
                    return DuplicateAssemblyReference;
                case 133:
                    return UnableToResolveType;
                case 134:
                    return StubsAreDeprecated;
                case 135:
                    return WrongClassName;
                case 136:
                    return ReflectionCallerClassRequiresCallerID;
                case 137:
                    return LegacyAssemblyAttributesFound;
                case 138:
                    return UnableToCreateLambdaFactory;
                case 999:
                    return UnknownWarning;
                case 139:
                    return DuplicateIkvmLangProperty;
                case 140:
                    return MalformedIkvmLangProperty;
                case 141:
                    return GenericCompilerWarning;
                case 142:
                    return GenericClassLoadingWarning;
                case 143:
                    return GenericVerifierWarning;
                case 144:
                    return GenericRuntimeWarning;
                case 145:
                    return GenericJniWarning;
                case 4001:
                    return UnableToCreateProxy;
                case 4002:
                    return DuplicateProxy;
                case 4003:
                    return MapXmlUnableToResolveOpCode;
                case 4004:
                    return MapXmlError;
                case 4005:
                    return InputFileNotFound;
                case 4006:
                    return UnknownFileType;
                case 4007:
                    return UnknownElementInMapFile;
                case 4008:
                    return UnknownAttributeInMapFile;
                case 4009:
                    return InvalidMemberNameInMapFile;
                case 4010:
                    return InvalidMemberSignatureInMapFile;
                case 4011:
                    return InvalidPropertyNameInMapFile;
                case 4012:
                    return InvalidPropertySignatureInMapFile;
                case 4013:
                    return NonPrimaryAssemblyReference;
                case 4014:
                    return MissingType;
                case 4015:
                    return MissingReference;
                case 4016:
                    return CallerSensitiveOnUnsupportedMethod;
                case 4017:
                    return RemappedTypeMissingDefaultInterfaceMethod;
                case 4018:
                    return GenericCompilerError;
                case 4019:
                    return GenericClassLoadingError;
                case 4020:
                    return GenericVerifierError;
                case 4021:
                    return GenericRuntimeError;
                case 4022:
                    return GenericJniError;
                case 4023:
                    return ExportingImportsNotSupported;
                case 5000:
                    return ResponseFileDepthExceeded;
                case 5001:
                    return ErrorReadingFile;
                case 5002:
                    return NoTargetsFound;
                case 5003:
                    return FileFormatLimitationExceeded;
                case 5004:
                    return CannotSpecifyBothKeyFileAndContainer;
                case 5005:
                    return DelaySignRequiresKey;
                case 5006:
                    return InvalidStrongNameKeyPair;
                case 5007:
                    return ReferenceNotFound;
                case 5008:
                    return OptionsMustPreceedChildLevels;
                case 5009:
                    return UnrecognizedTargetType;
                case 5010:
                    return UnrecognizedPlatform;
                case 5011:
                    return UnrecognizedApartment;
                case 5012:
                    return MissingFileSpecification;
                case 5013:
                    return PathTooLong;
                case 5014:
                    return PathNotFound;
                case 5015:
                    return InvalidPath;
                case 5016:
                    return InvalidOptionSyntax;
                case 5017:
                    return ExternalResourceNotFound;
                case 5018:
                    return ExternalResourceNameInvalid;
                case 5019:
                    return InvalidVersionFormat;
                case 5020:
                    return InvalidFileAlignment;
                case 5021:
                    return ErrorWritingFile;
                case 5022:
                    return UnrecognizedOption;
                case 5023:
                    return NoOutputFileSpecified;
                case 5024:
                    return SharedClassLoaderCannotBeUsedOnModuleTarget;
                case 5025:
                    return RuntimeNotFound;
                case 5026:
                    return MainClassRequiresExe;
                case 5027:
                    return ExeRequiresMainClass;
                case 5028:
                    return PropertiesRequireExe;
                case 5029:
                    return ModuleCannotHaveClassLoader;
                case 5030:
                    return ErrorParsingMapFile;
                case 5031:
                    return BootstrapClassesMissing;
                case 5032:
                    return StrongNameRequiresStrongNamedRefs;
                case 5033:
                    return MainClassNotFound;
                case 5034:
                    return MainMethodNotFound;
                case 5035:
                    return UnsupportedMainMethod;
                case 5036:
                    return ExternalMainNotAccessible;
                case 5037:
                    return ClassLoaderNotFound;
                case 5038:
                    return ClassLoaderNotAccessible;
                case 5039:
                    return ClassLoaderIsAbstract;
                case 5040:
                    return ClassLoaderNotClassLoader;
                case 5041:
                    return ClassLoaderConstructorMissing;
                case 5042:
                    return MapFileTypeNotFound;
                case 5043:
                    return MapFileClassNotFound;
                case 5044:
                    return MaximumErrorCountReached;
                case 5045:
                    return LinkageError;
                case 5046:
                    return RuntimeMismatch;
                case 5047:
                    return RuntimeMismatchStrongName;
                case 5048:
                    return CoreClassesMissing;
                case 5049:
                    return CriticalClassNotFound;
                case 5050:
                    return AssemblyContainsDuplicateClassNames;
                case 5051:
                    return CallerIDRequiresHasCallerIDAnnotation;
                case 5052:
                    return UnableToResolveInterface;
                case 5053:
                    return MissingBaseType;
                case 5054:
                    return MissingBaseTypeReference;
                case 5055:
                    return FileNotFound;
                case 5056:
                    return RuntimeMethodMissing;
                case 5057:
                    return MapFileFieldNotFound;
                case 5058:
                    return GhostInterfaceMethodMissing;
                case 5059:
                    return ModuleInitializerMethodRequirements;
                case 5060:
                    return InvalidZip;
                case 5061:
                    return CoreAssemblyVersionMismatch;
                case 6000:
                    return GenericRuntimeTrace;
                case 6001:
                    return GenericJniTrace;
                case 6002:
                    return GenericCompilerTrace;
                default:
                    return null;
            }
        }

        /// <summary>
        /// The 'MainMethodFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Found main method in class "{arg0}".
        /// </remarks>
        public static readonly Diagnostic MainMethodFound = new Diagnostic(1, nameof(MainMethodFound), "Found main method in class \"{0}\".", DiagnosticLevel.Info);

        /// <summary>
        /// The 'OutputFileIs' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Output file is "{arg0}".
        /// </remarks>
        public static readonly Diagnostic OutputFileIs = new Diagnostic(2, nameof(OutputFileIs), "Output file is \"{0}\".", DiagnosticLevel.Info);

        /// <summary>
        /// The 'AutoAddRef' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Automatically adding reference to "{arg0}".
        /// </remarks>
        public static readonly Diagnostic AutoAddRef = new Diagnostic(3, nameof(AutoAddRef), "Automatically adding reference to \"{0}\".", DiagnosticLevel.Info);

        /// <summary>
        /// The 'MainMethodFromManifest' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: Using main class "{arg0}" based on jar manifest.
        /// </remarks>
        public static readonly Diagnostic MainMethodFromManifest = new Diagnostic(4, nameof(MainMethodFromManifest), "Using main class \"{0}\" based on jar manifest.", DiagnosticLevel.Info);

        /// <summary>
        /// The 'GenericCompilerInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericCompilerInfo = new Diagnostic(5, nameof(GenericCompilerInfo), "{0}", DiagnosticLevel.Info);

        /// <summary>
        /// The 'GenericClassLoadingInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericClassLoadingInfo = new Diagnostic(6, nameof(GenericClassLoadingInfo), "{0}", DiagnosticLevel.Info);

        /// <summary>
        /// The 'GenericVerifierInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericVerifierInfo = new Diagnostic(7, nameof(GenericVerifierInfo), "{0}", DiagnosticLevel.Info);

        /// <summary>
        /// The 'GenericRuntimeInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericRuntimeInfo = new Diagnostic(8, nameof(GenericRuntimeInfo), "{0}", DiagnosticLevel.Info);

        /// <summary>
        /// The 'GenericJniInfo' diagnostic.
        /// </summary>
        /// <remarks>
/// Info: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericJniInfo = new Diagnostic(9, nameof(GenericJniInfo), "{0}", DiagnosticLevel.Info);

        /// <summary>
        /// The 'ClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Class "{arg0}" not found.
        /// </remarks>
        public static readonly Diagnostic ClassNotFound = new Diagnostic(100, nameof(ClassNotFound), "Class \"{0}\" not found.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'ClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (class format error "{arg1}")
        /// </remarks>
        public static readonly Diagnostic ClassFormatError = new Diagnostic(101, nameof(ClassFormatError), "Unable to compile class \"{0}\". (class format error \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'DuplicateClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Duplicate class name: "{arg0}".
        /// </remarks>
        public static readonly Diagnostic DuplicateClassName = new Diagnostic(102, nameof(DuplicateClassName), "Duplicate class name: \"{0}\".", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'IllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (illegal access error "{arg1}")
        /// </remarks>
        public static readonly Diagnostic IllegalAccessError = new Diagnostic(103, nameof(IllegalAccessError), "Unable to compile class \"{0}\". (illegal access error \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'VerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (verification error "{arg1}")
        /// </remarks>
        public static readonly Diagnostic VerificationError = new Diagnostic(104, nameof(VerificationError), "Unable to compile class \"{0}\". (verification error \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'NoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". (missing class "{arg1}")
        /// </remarks>
        public static readonly Diagnostic NoClassDefFoundError = new Diagnostic(105, nameof(NoClassDefFoundError), "Unable to compile class \"{0}\". (missing class \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericUnableToCompileError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile class "{arg0}". ("{arg1}": "{arg2}")
        /// </remarks>
        public static readonly Diagnostic GenericUnableToCompileError = new Diagnostic(106, nameof(GenericUnableToCompileError), "Unable to compile class \"{0}\". (\"{1}\": \"{2}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'DuplicateResourceName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Skipping resource (name clash): "{arg0}"
        /// </remarks>
        public static readonly Diagnostic DuplicateResourceName = new Diagnostic(107, nameof(DuplicateResourceName), "Skipping resource (name clash): \"{0}\"", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'SkippingReferencedClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Skipping class: "{arg0}". (class is already available in referenced assembly "{arg1}")
        /// </remarks>
        public static readonly Diagnostic SkippingReferencedClass = new Diagnostic(109, nameof(SkippingReferencedClass), "Skipping class: \"{0}\". (class is already available in referenced assembly \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'NoJniRuntime' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to load runtime JNI assembly.
        /// </remarks>
        public static readonly Diagnostic NoJniRuntime = new Diagnostic(110, nameof(NoJniRuntime), "Unable to load runtime JNI assembly.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedNoClassDefFoundError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoClassDefFoundError in "{arg0}". ("{arg1}").
        /// </remarks>
        public static readonly Diagnostic EmittedNoClassDefFoundError = new Diagnostic(111, nameof(EmittedNoClassDefFoundError), "Emitted java.lang.NoClassDefFoundError in \"{0}\". (\"{1}\").", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedIllegalAccessError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.IllegalAccessError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedIllegalAccessError = new Diagnostic(112, nameof(EmittedIllegalAccessError), "Emitted java.lang.IllegalAccessError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedInstantiationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.InstantiationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedInstantiationError = new Diagnostic(113, nameof(EmittedInstantiationError), "Emitted java.lang.InstantiationError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedIncompatibleClassChangeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.IncompatibleClassChangeError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedIncompatibleClassChangeError = new Diagnostic(114, nameof(EmittedIncompatibleClassChangeError), "Emitted java.lang.IncompatibleClassChangeError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedNoSuchFieldError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoSuchFieldError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedNoSuchFieldError = new Diagnostic(115, nameof(EmittedNoSuchFieldError), "Emitted java.lang.NoSuchFieldError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedAbstractMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.AbstractMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedAbstractMethodError = new Diagnostic(116, nameof(EmittedAbstractMethodError), "Emitted java.lang.AbstractMethodError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedNoSuchMethodError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.NoSuchMethodError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedNoSuchMethodError = new Diagnostic(117, nameof(EmittedNoSuchMethodError), "Emitted java.lang.NoSuchMethodError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedLinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.LinkageError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedLinkageError = new Diagnostic(118, nameof(EmittedLinkageError), "Emitted java.lang.LinkageError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedVerificationError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.VerificationError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedVerificationError = new Diagnostic(119, nameof(EmittedVerificationError), "Emitted java.lang.VerificationError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'EmittedClassFormatError' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Emitted java.lang.ClassFormatError in "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic EmittedClassFormatError = new Diagnostic(120, nameof(EmittedClassFormatError), "Emitted java.lang.ClassFormatError in \"{0}\". (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'InvalidCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Error emitting "{arg0}" custom attribute. ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic InvalidCustomAttribute = new Diagnostic(121, nameof(InvalidCustomAttribute), "Error emitting \"{0}\" custom attribute. (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'IgnoredCustomAttribute' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Custom attribute "{arg0}" was ignored. ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic IgnoredCustomAttribute = new Diagnostic(122, nameof(IgnoredCustomAttribute), "Custom attribute \"{0}\" was ignored. (\"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'AssumeAssemblyVersionMatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Assuming assembly reference "{arg0}" matches "{arg1}", you may need to supply runtime policy
        /// </remarks>
        public static readonly Diagnostic AssumeAssemblyVersionMatch = new Diagnostic(123, nameof(AssumeAssemblyVersionMatch), "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime p" +
    "olicy", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'InvalidDirectoryInLibOptionPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Directory "{arg0}" specified in -lib option is not valid.
        /// </remarks>
        public static readonly Diagnostic InvalidDirectoryInLibOptionPath = new Diagnostic(124, nameof(InvalidDirectoryInLibOptionPath), "Directory \"{0}\" specified in -lib option is not valid.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'InvalidDirectoryInLibEnvironmentPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Directory "{arg0}" specified in LIB environment is not valid.
        /// </remarks>
        public static readonly Diagnostic InvalidDirectoryInLibEnvironmentPath = new Diagnostic(125, nameof(InvalidDirectoryInLibEnvironmentPath), "Directory \"{0}\" specified in LIB environment is not valid.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'LegacySearchRule' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Found assembly "{arg0}" using legacy search rule, please append '.dll' to the reference.
        /// </remarks>
        public static readonly Diagnostic LegacySearchRule = new Diagnostic(126, nameof(LegacySearchRule), "Found assembly \"{0}\" using legacy search rule, please append \'.dll\' to the refere" +
    "nce.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'AssemblyLocationIgnored' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Assembly "{arg0}" is ignored as previously loaded assembly "{arg1}" has the same identity "{arg2}".
        /// </remarks>
        public static readonly Diagnostic AssemblyLocationIgnored = new Diagnostic(127, nameof(AssemblyLocationIgnored), "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identi" +
    "ty \"{2}\".", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'InterfaceMethodCantBeInternal' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring @ikvm.lang.Internal annotation on interface method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static readonly Diagnostic InterfaceMethodCantBeInternal = new Diagnostic(128, nameof(InterfaceMethodCantBeInternal), "Ignoring @ikvm.lang.Internal annotation on interface method. (\"{0}.{1}{2}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'DuplicateAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Duplicate assembly reference "{arg0}"
        /// </remarks>
        public static readonly Diagnostic DuplicateAssemblyReference = new Diagnostic(132, nameof(DuplicateAssemblyReference), "Duplicate assembly reference \"{0}\"", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'UnableToResolveType' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Reference in "{arg0}" to type "{arg1}" claims it is defined in "{arg2}", but it could not be found.
        /// </remarks>
        public static readonly Diagnostic UnableToResolveType = new Diagnostic(133, nameof(UnableToResolveType), "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not " +
    "be found.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'StubsAreDeprecated' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Compiling stubs is deprecated. Please add a reference to assembly "{arg0}" instead.
        /// </remarks>
        public static readonly Diagnostic StubsAreDeprecated = new Diagnostic(134, nameof(StubsAreDeprecated), "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'WrongClassName' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to compile "{arg0}" (wrong name: "{arg1}")
        /// </remarks>
        public static readonly Diagnostic WrongClassName = new Diagnostic(135, nameof(WrongClassName), "Unable to compile \"{0}\" (wrong name: \"{1}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'ReflectionCallerClassRequiresCallerID' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Reflection.getCallerClass() called from non-CallerID method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static readonly Diagnostic ReflectionCallerClassRequiresCallerID = new Diagnostic(136, nameof(ReflectionCallerClassRequiresCallerID), "Reflection.getCallerClass() called from non-CallerID method. (\"{0}.{1}{2}\")", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'LegacyAssemblyAttributesFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.
        /// </remarks>
        public static readonly Diagnostic LegacyAssemblyAttributesFound = new Diagnostic(137, nameof(LegacyAssemblyAttributesFound), "Legacy assembly attributes container found. Please use the -assemblyattributes:<f" +
    "ile> option.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'UnableToCreateLambdaFactory' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Unable to create static lambda factory.
        /// </remarks>
        public static readonly Diagnostic UnableToCreateLambdaFactory = new Diagnostic(138, nameof(UnableToCreateLambdaFactory), "Unable to create static lambda factory.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'UnknownWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic UnknownWarning = new Diagnostic(999, nameof(UnknownWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'DuplicateIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public static readonly Diagnostic DuplicateIkvmLangProperty = new Diagnostic(139, nameof(DuplicateIkvmLangProperty), "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'MalformedIkvmLangProperty' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: Ignoring duplicate ikvm.lang.Property annotation on {arg0}.{arg1}.
        /// </remarks>
        public static readonly Diagnostic MalformedIkvmLangProperty = new Diagnostic(140, nameof(MalformedIkvmLangProperty), "Ignoring duplicate ikvm.lang.Property annotation on {0}.{1}.", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericCompilerWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericCompilerWarning = new Diagnostic(141, nameof(GenericCompilerWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericClassLoadingWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericClassLoadingWarning = new Diagnostic(142, nameof(GenericClassLoadingWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericVerifierWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericVerifierWarning = new Diagnostic(143, nameof(GenericVerifierWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericRuntimeWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericRuntimeWarning = new Diagnostic(144, nameof(GenericRuntimeWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'GenericJniWarning' diagnostic.
        /// </summary>
        /// <remarks>
/// Warning: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericJniWarning = new Diagnostic(145, nameof(GenericJniWarning), "{0}", DiagnosticLevel.Warning);

        /// <summary>
        /// The 'UnableToCreateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unable to create proxy "{arg0}". ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic UnableToCreateProxy = new Diagnostic(4001, nameof(UnableToCreateProxy), "Unable to create proxy \"{0}\". (\"{1}\")", DiagnosticLevel.Error);

        /// <summary>
        /// The 'DuplicateProxy' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Duplicate proxy "{arg0}".
        /// </remarks>
        public static readonly Diagnostic DuplicateProxy = new Diagnostic(4002, nameof(DuplicateProxy), "Duplicate proxy \"{0}\".", DiagnosticLevel.Error);

        /// <summary>
        /// The 'MapXmlUnableToResolveOpCode' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unable to resolve opcode in remap file: {arg0}.
        /// </remarks>
        public static readonly Diagnostic MapXmlUnableToResolveOpCode = new Diagnostic(4003, nameof(MapXmlUnableToResolveOpCode), "Unable to resolve opcode in remap file: {0}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'MapXmlError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Error in remap file: {arg0}.
        /// </remarks>
        public static readonly Diagnostic MapXmlError = new Diagnostic(4004, nameof(MapXmlError), "Error in remap file: {0}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'InputFileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Source file '{arg0}' not found.
        /// </remarks>
        public static readonly Diagnostic InputFileNotFound = new Diagnostic(4005, nameof(InputFileNotFound), "Source file \'{0}\' not found.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'UnknownFileType' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown file type: {arg0}.
        /// </remarks>
        public static readonly Diagnostic UnknownFileType = new Diagnostic(4006, nameof(UnknownFileType), "Unknown file type: {0}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'UnknownElementInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown element {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public static readonly Diagnostic UnknownElementInMapFile = new Diagnostic(4007, nameof(UnknownElementInMapFile), "Unknown element {0} in remap file, line {1}, column {2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'UnknownAttributeInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Unknown attribute {arg0} in remap file, line {arg1}, column {arg2}.
        /// </remarks>
        public static readonly Diagnostic UnknownAttributeInMapFile = new Diagnostic(4008, nameof(UnknownAttributeInMapFile), "Unknown attribute {0} in remap file, line {1}, column {2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'InvalidMemberNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid {arg0} name '{arg1}' in remap file in class {arg2}.
        /// </remarks>
        public static readonly Diagnostic InvalidMemberNameInMapFile = new Diagnostic(4009, nameof(InvalidMemberNameInMapFile), "Invalid {0} name \'{1}\' in remap file in class {2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'InvalidMemberSignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid {arg0} signature '{arg3}' in remap file for {arg0} {arg1}.{arg2}.
        /// </remarks>
        public static readonly Diagnostic InvalidMemberSignatureInMapFile = new Diagnostic(4010, nameof(InvalidMemberSignatureInMapFile), "Invalid {0} signature \'{3}\' in remap file for {0} {1}.{2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'InvalidPropertyNameInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid property {arg0} name '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public static readonly Diagnostic InvalidPropertyNameInMapFile = new Diagnostic(4011, nameof(InvalidPropertyNameInMapFile), "Invalid property {0} name \'{3}\' in remap file for property {1}.{2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'InvalidPropertySignatureInMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Invalid property {arg0} signature '{arg3}' in remap file for property {arg1}.{arg2}.
        /// </remarks>
        public static readonly Diagnostic InvalidPropertySignatureInMapFile = new Diagnostic(4012, nameof(InvalidPropertySignatureInMapFile), "Invalid property {0} signature \'{3}\' in remap file for property {1}.{2}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'NonPrimaryAssemblyReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Referenced assembly "{arg0}" is not the primary assembly of a shared class loader group, please reference primary assembly "{arg1}" instead.
        /// </remarks>
        public static readonly Diagnostic NonPrimaryAssemblyReference = new Diagnostic(4013, nameof(NonPrimaryAssemblyReference), "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader gr" +
    "oup, please reference primary assembly \"{1}\" instead.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'MissingType' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Reference to type "{arg0}" claims it is defined in "{arg1}", but it could not be found.
        /// </remarks>
        public static readonly Diagnostic MissingType = new Diagnostic(4014, nameof(MissingType), "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found." +
    "", DiagnosticLevel.Error);

        /// <summary>
        /// The 'MissingReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: The type '{arg0}' is defined in an assembly that is notResponseFileDepthExceeded referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public static readonly Diagnostic MissingReference = new Diagnostic(4015, nameof(MissingReference), "The type \'{0}\' is defined in an assembly that is notResponseFileDepthExceeded ref" +
    "erenced. You must add a reference to assembly \'{1}\'.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'CallerSensitiveOnUnsupportedMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: CallerSensitive annotation on unsupported method. ("{arg0}.{arg1}{arg2}")
        /// </remarks>
        public static readonly Diagnostic CallerSensitiveOnUnsupportedMethod = new Diagnostic(4016, nameof(CallerSensitiveOnUnsupportedMethod), "CallerSensitive annotation on unsupported method. (\"{0}.{1}{2}\")", DiagnosticLevel.Error);

        /// <summary>
        /// The 'RemappedTypeMissingDefaultInterfaceMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0} does not implement default interface method {arg1}.
        /// </remarks>
        public static readonly Diagnostic RemappedTypeMissingDefaultInterfaceMethod = new Diagnostic(4017, nameof(RemappedTypeMissingDefaultInterfaceMethod), "{0} does not implement default interface method {1}.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'GenericCompilerError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericCompilerError = new Diagnostic(4018, nameof(GenericCompilerError), "{0}", DiagnosticLevel.Error);

        /// <summary>
        /// The 'GenericClassLoadingError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericClassLoadingError = new Diagnostic(4019, nameof(GenericClassLoadingError), "{0}", DiagnosticLevel.Error);

        /// <summary>
        /// The 'GenericVerifierError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericVerifierError = new Diagnostic(4020, nameof(GenericVerifierError), "{0}", DiagnosticLevel.Error);

        /// <summary>
        /// The 'GenericRuntimeError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericRuntimeError = new Diagnostic(4021, nameof(GenericRuntimeError), "{0}", DiagnosticLevel.Error);

        /// <summary>
        /// The 'GenericJniError' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericJniError = new Diagnostic(4022, nameof(GenericJniError), "{0}", DiagnosticLevel.Error);

        /// <summary>
        /// The 'ExportingImportsNotSupported' diagnostic.
        /// </summary>
        /// <remarks>
/// Error: Exporting previously imported assemblies is not supported.
        /// </remarks>
        public static readonly Diagnostic ExportingImportsNotSupported = new Diagnostic(4023, nameof(ExportingImportsNotSupported), "Exporting previously imported assemblies is not supported.", DiagnosticLevel.Error);

        /// <summary>
        /// The 'ResponseFileDepthExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Response file nesting depth exceeded.
        /// </remarks>
        public static readonly Diagnostic ResponseFileDepthExceeded = new Diagnostic(5000, nameof(ResponseFileDepthExceeded), "Response file nesting depth exceeded.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ErrorReadingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to read file: {arg0}. ({arg1})
        /// </remarks>
        public static readonly Diagnostic ErrorReadingFile = new Diagnostic(5001, nameof(ErrorReadingFile), "Unable to read file: {0}. ({1})", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'NoTargetsFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No targets found
        /// </remarks>
        public static readonly Diagnostic NoTargetsFound = new Diagnostic(5002, nameof(NoTargetsFound), "No targets found", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'FileFormatLimitationExceeded' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: File format limitation exceeded: {arg0}.
        /// </remarks>
        public static readonly Diagnostic FileFormatLimitationExceeded = new Diagnostic(5003, nameof(FileFormatLimitationExceeded), "File format limitation exceeded: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'CannotSpecifyBothKeyFileAndContainer' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You cannot specify both a key file and container.
        /// </remarks>
        public static readonly Diagnostic CannotSpecifyBothKeyFileAndContainer = new Diagnostic(5004, nameof(CannotSpecifyBothKeyFileAndContainer), "You cannot specify both a key file and container.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'DelaySignRequiresKey' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You cannot delay sign without a key file or container.
        /// </remarks>
        public static readonly Diagnostic DelaySignRequiresKey = new Diagnostic(5005, nameof(DelaySignRequiresKey), "You cannot delay sign without a key file or container.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidStrongNameKeyPair' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid key {arg0} specified. ("{arg1}")
        /// </remarks>
        public static readonly Diagnostic InvalidStrongNameKeyPair = new Diagnostic(5006, nameof(InvalidStrongNameKeyPair), "Invalid key {0} specified. (\"{1}\")", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ReferenceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Reference not found: {arg0}
        /// </remarks>
        public static readonly Diagnostic ReferenceNotFound = new Diagnostic(5007, nameof(ReferenceNotFound), "Reference not found: {0}", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'OptionsMustPreceedChildLevels' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: You can only specify options before any child levels.
        /// </remarks>
        public static readonly Diagnostic OptionsMustPreceedChildLevels = new Diagnostic(5008, nameof(OptionsMustPreceedChildLevels), "You can only specify options before any child levels.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnrecognizedTargetType' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -target option.
        /// </remarks>
        public static readonly Diagnostic UnrecognizedTargetType = new Diagnostic(5009, nameof(UnrecognizedTargetType), "Invalid value \'{0}\' for -target option.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnrecognizedPlatform' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -platform option.
        /// </remarks>
        public static readonly Diagnostic UnrecognizedPlatform = new Diagnostic(5010, nameof(UnrecognizedPlatform), "Invalid value \'{0}\' for -platform option.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnrecognizedApartment' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -apartment option.
        /// </remarks>
        public static readonly Diagnostic UnrecognizedApartment = new Diagnostic(5011, nameof(UnrecognizedApartment), "Invalid value \'{0}\' for -apartment option.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MissingFileSpecification' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Missing file specification for '{arg0}' option.
        /// </remarks>
        public static readonly Diagnostic MissingFileSpecification = new Diagnostic(5012, nameof(MissingFileSpecification), "Missing file specification for \'{0}\' option.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'PathTooLong' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Path too long: {arg0}.
        /// </remarks>
        public static readonly Diagnostic PathTooLong = new Diagnostic(5013, nameof(PathTooLong), "Path too long: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'PathNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Path not found: {arg0}.
        /// </remarks>
        public static readonly Diagnostic PathNotFound = new Diagnostic(5014, nameof(PathNotFound), "Path not found: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidPath' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid path: {arg0}.
        /// </remarks>
        public static readonly Diagnostic InvalidPath = new Diagnostic(5015, nameof(InvalidPath), "Invalid path: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidOptionSyntax' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid option: {arg0}.
        /// </remarks>
        public static readonly Diagnostic InvalidOptionSyntax = new Diagnostic(5016, nameof(InvalidOptionSyntax), "Invalid option: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ExternalResourceNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External resource file does not exist: {arg0}.
        /// </remarks>
        public static readonly Diagnostic ExternalResourceNotFound = new Diagnostic(5017, nameof(ExternalResourceNotFound), "External resource file does not exist: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ExternalResourceNameInvalid' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External resource file may not include path specification: {arg0}.
        /// </remarks>
        public static readonly Diagnostic ExternalResourceNameInvalid = new Diagnostic(5018, nameof(ExternalResourceNameInvalid), "External resource file may not include path specification: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidVersionFormat' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid version specified: {arg0}.
        /// </remarks>
        public static readonly Diagnostic InvalidVersionFormat = new Diagnostic(5019, nameof(InvalidVersionFormat), "Invalid version specified: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidFileAlignment' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid value '{arg0}' for -filealign option.
        /// </remarks>
        public static readonly Diagnostic InvalidFileAlignment = new Diagnostic(5020, nameof(InvalidFileAlignment), "Invalid value \'{0}\' for -filealign option.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ErrorWritingFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to write file: {arg0}. ({arg1})
        /// </remarks>
        public static readonly Diagnostic ErrorWritingFile = new Diagnostic(5021, nameof(ErrorWritingFile), "Unable to write file: {0}. ({1})", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnrecognizedOption' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unrecognized option: {arg0}.
        /// </remarks>
        public static readonly Diagnostic UnrecognizedOption = new Diagnostic(5022, nameof(UnrecognizedOption), "Unrecognized option: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'NoOutputFileSpecified' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No output file specified.
        /// </remarks>
        public static readonly Diagnostic NoOutputFileSpecified = new Diagnostic(5023, nameof(NoOutputFileSpecified), "No output file specified.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'SharedClassLoaderCannotBeUsedOnModuleTarget' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Incompatible options: -target:module and -sharedclassloader cannot be combined.
        /// </remarks>
        public static readonly Diagnostic SharedClassLoaderCannotBeUsedOnModuleTarget = new Diagnostic(5024, nameof(SharedClassLoaderCannotBeUsedOnModuleTarget), "Incompatible options: -target:module and -sharedclassloader cannot be combined.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'RuntimeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load runtime assembly.
        /// </remarks>
        public static readonly Diagnostic RuntimeNotFound = new Diagnostic(5025, nameof(RuntimeNotFound), "Unable to load runtime assembly.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MainClassRequiresExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main class cannot be specified for library or module.
        /// </remarks>
        public static readonly Diagnostic MainClassRequiresExe = new Diagnostic(5026, nameof(MainClassRequiresExe), "Main class cannot be specified for library or module.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ExeRequiresMainClass' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: No main method found.
        /// </remarks>
        public static readonly Diagnostic ExeRequiresMainClass = new Diagnostic(5027, nameof(ExeRequiresMainClass), "No main method found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'PropertiesRequireExe' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Properties cannot be specified for library or module.
        /// </remarks>
        public static readonly Diagnostic PropertiesRequireExe = new Diagnostic(5028, nameof(PropertiesRequireExe), "Properties cannot be specified for library or module.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ModuleCannotHaveClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Cannot specify assembly class loader for modules.
        /// </remarks>
        public static readonly Diagnostic ModuleCannotHaveClassLoader = new Diagnostic(5029, nameof(ModuleCannotHaveClassLoader), "Cannot specify assembly class loader for modules.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ErrorParsingMapFile' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to parse remap file: {arg0}. ({arg1})
        /// </remarks>
        public static readonly Diagnostic ErrorParsingMapFile = new Diagnostic(5030, nameof(ErrorParsingMapFile), "Unable to parse remap file: {0}. ({1})", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'BootstrapClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Bootstrap classes missing and core assembly not found.
        /// </remarks>
        public static readonly Diagnostic BootstrapClassesMissing = new Diagnostic(5031, nameof(BootstrapClassesMissing), "Bootstrap classes missing and core assembly not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'StrongNameRequiresStrongNamedRefs' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: All referenced assemblies must be strong named, to be able to sign the output assembly.
        /// </remarks>
        public static readonly Diagnostic StrongNameRequiresStrongNamedRefs = new Diagnostic(5032, nameof(StrongNameRequiresStrongNamedRefs), "All referenced assemblies must be strong named, to be able to sign the output ass" +
    "embly.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MainClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main class not found.
        /// </remarks>
        public static readonly Diagnostic MainClassNotFound = new Diagnostic(5033, nameof(MainClassNotFound), "Main class not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MainMethodNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Main method not found.
        /// </remarks>
        public static readonly Diagnostic MainMethodNotFound = new Diagnostic(5034, nameof(MainMethodNotFound), "Main method not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnsupportedMainMethod' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Redirected main method not supported.
        /// </remarks>
        public static readonly Diagnostic UnsupportedMainMethod = new Diagnostic(5035, nameof(UnsupportedMainMethod), "Redirected main method not supported.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ExternalMainNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: External main method must be public and in a public class.
        /// </remarks>
        public static readonly Diagnostic ExternalMainNotAccessible = new Diagnostic(5036, nameof(ExternalMainNotAccessible), "External main method must be public and in a public class.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ClassLoaderNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class not found.
        /// </remarks>
        public static readonly Diagnostic ClassLoaderNotFound = new Diagnostic(5037, nameof(ClassLoaderNotFound), "Custom assembly class loader class not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ClassLoaderNotAccessible' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class is not accessible.
        /// </remarks>
        public static readonly Diagnostic ClassLoaderNotAccessible = new Diagnostic(5038, nameof(ClassLoaderNotAccessible), "Custom assembly class loader class is not accessible.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ClassLoaderIsAbstract' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class is abstract.
        /// </remarks>
        public static readonly Diagnostic ClassLoaderIsAbstract = new Diagnostic(5039, nameof(ClassLoaderIsAbstract), "Custom assembly class loader class is abstract.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ClassLoaderNotClassLoader' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader class does not extend java.lang.ClassLoader.
        /// </remarks>
        public static readonly Diagnostic ClassLoaderNotClassLoader = new Diagnostic(5040, nameof(ClassLoaderNotClassLoader), "Custom assembly class loader class does not extend java.lang.ClassLoader.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ClassLoaderConstructorMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Custom assembly class loader constructor is missing.
        /// </remarks>
        public static readonly Diagnostic ClassLoaderConstructorMissing = new Diagnostic(5041, nameof(ClassLoaderConstructorMissing), "Custom assembly class loader constructor is missing.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MapFileTypeNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Type '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public static readonly Diagnostic MapFileTypeNotFound = new Diagnostic(5042, nameof(MapFileTypeNotFound), "Type \'{0}\' referenced in remap file was not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MapFileClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Class '{arg0}' referenced in remap file was not found.
        /// </remarks>
        public static readonly Diagnostic MapFileClassNotFound = new Diagnostic(5043, nameof(MapFileClassNotFound), "Class \'{0}\' referenced in remap file was not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MaximumErrorCountReached' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Maximum error count reached.
        /// </remarks>
        public static readonly Diagnostic MaximumErrorCountReached = new Diagnostic(5044, nameof(MaximumErrorCountReached), "Maximum error count reached.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'LinkageError' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Link error: {arg0}
        /// </remarks>
        public static readonly Diagnostic LinkageError = new Diagnostic(5045, nameof(LinkageError), "Link error: {0}", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'RuntimeMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Referenced assembly {referencedAssemblyPath} was compiled with an incompatible IKVM.Runtime version. Current runtime: {runtimeAssemblyName}. Referenced assembly runtime: {referencedAssemblyName}
        /// </remarks>
        public static readonly Diagnostic RuntimeMismatch = new Diagnostic(5046, nameof(RuntimeMismatch), "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version. C" +
    "urrent runtime: {1}. Referenced assembly runtime: {2}", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'RuntimeMismatchStrongName' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal:
        /// </remarks>
        public static readonly Diagnostic RuntimeMismatchStrongName = new Diagnostic(5047, nameof(RuntimeMismatchStrongName), "", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'CoreClassesMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Failed to find core classes in core library.
        /// </remarks>
        public static readonly Diagnostic CoreClassesMissing = new Diagnostic(5048, nameof(CoreClassesMissing), "Failed to find core classes in core library.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'CriticalClassNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load critical class '{arg0}'.
        /// </remarks>
        public static readonly Diagnostic CriticalClassNotFound = new Diagnostic(5049, nameof(CriticalClassNotFound), "Unable to load critical class \'{0}\'.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'AssemblyContainsDuplicateClassNames' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Type '{arg0}' and '{arg1}' both map to the same name '{arg2}'. ({arg3})
        /// </remarks>
        public static readonly Diagnostic AssemblyContainsDuplicateClassNames = new Diagnostic(5050, nameof(AssemblyContainsDuplicateClassNames), "Type \'{0}\' and \'{1}\' both map to the same name \'{2}\'. ({3})", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'CallerIDRequiresHasCallerIDAnnotation' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: CallerID.getCallerID() requires a HasCallerID annotation.
        /// </remarks>
        public static readonly Diagnostic CallerIDRequiresHasCallerIDAnnotation = new Diagnostic(5051, nameof(CallerIDRequiresHasCallerIDAnnotation), "CallerID.getCallerID() requires a HasCallerID annotation.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'UnableToResolveInterface' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to resolve interface '{arg0}' on type '{arg1}'.
        /// </remarks>
        public static readonly Diagnostic UnableToResolveInterface = new Diagnostic(5052, nameof(UnableToResolveInterface), "Unable to resolve interface \'{0}\' on type \'{1}\'.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MissingBaseType' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: The base class or interface '{arg0}' in assembly '{arg1}' referenced by type '{arg2}' in '{arg3}' could not be resolved.
        /// </remarks>
        public static readonly Diagnostic MissingBaseType = new Diagnostic(5053, nameof(MissingBaseType), "The base class or interface \'{0}\' in assembly \'{1}\' referenced by type \'{2}\' in \'" +
    "{3}\' could not be resolved.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MissingBaseTypeReference' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: The type '{arg0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{arg1}'.
        /// </remarks>
        public static readonly Diagnostic MissingBaseTypeReference = new Diagnostic(5054, nameof(MissingBaseTypeReference), "The type \'{0}\' is defined in an assembly that is not referenced. You must add a r" +
    "eference to assembly \'{1}\'.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'FileNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: File not found: {arg0}.
        /// </remarks>
        public static readonly Diagnostic FileNotFound = new Diagnostic(5055, nameof(FileNotFound), "File not found: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'RuntimeMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Runtime method '{arg0}' not found.
        /// </remarks>
        public static readonly Diagnostic RuntimeMethodMissing = new Diagnostic(5056, nameof(RuntimeMethodMissing), "Runtime method \'{0}\' not found.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'MapFileFieldNotFound' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Field '{arg0}' referenced in remap file was not found in class '{arg1}'.
        /// </remarks>
        public static readonly Diagnostic MapFileFieldNotFound = new Diagnostic(5057, nameof(MapFileFieldNotFound), "Field \'{0}\' referenced in remap file was not found in class \'{1}\'.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'GhostInterfaceMethodMissing' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Remapped class '{arg0}' does not implement ghost interface method. ({arg1}.{arg2}{arg3})
        /// </remarks>
        public static readonly Diagnostic GhostInterfaceMethodMissing = new Diagnostic(5058, nameof(GhostInterfaceMethodMissing), "Remapped class \'{0}\' does not implement ghost interface method. ({1}.{2}{3})", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'ModuleInitializerMethodRequirements' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Method '{arg1}.{arg2}{arg3}' does not meet the requirements of a module initializer.
        /// </remarks>
        public static readonly Diagnostic ModuleInitializerMethodRequirements = new Diagnostic(5059, nameof(ModuleInitializerMethodRequirements), "Method \'{0}.{1}{2}\' does not meet the requirements of a module initializer.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'InvalidZip' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Invalid zip: {name}.
        /// </remarks>
        public static readonly Diagnostic InvalidZip = new Diagnostic(5060, nameof(InvalidZip), "Invalid zip: {0}.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'CoreAssemblyVersionMismatch' diagnostic.
        /// </summary>
        /// <remarks>
/// Fatal: Unable to load assembly '{0}' as it depends on a higher version of {1} than the one currently loaded.
        /// </remarks>
        public static readonly Diagnostic CoreAssemblyVersionMismatch = new Diagnostic(5061, nameof(CoreAssemblyVersionMismatch), "Unable to load assembly \'{-1}\' as it depends on a higher version of {-1} than the" +
    " one currently loaded.", DiagnosticLevel.Fatal);

        /// <summary>
        /// The 'GenericRuntimeTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericRuntimeTrace = new Diagnostic(6000, nameof(GenericRuntimeTrace), "{0}", DiagnosticLevel.Trace);

        /// <summary>
        /// The 'GenericJniTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericJniTrace = new Diagnostic(6001, nameof(GenericJniTrace), "{0}", DiagnosticLevel.Trace);

        /// <summary>
        /// The 'GenericCompilerTrace' diagnostic.
        /// </summary>
        /// <remarks>
/// Trace: {arg0}
        /// </remarks>
        public static readonly Diagnostic GenericCompilerTrace = new Diagnostic(6002, nameof(GenericCompilerTrace), "{0}", DiagnosticLevel.Trace);

    }

}
