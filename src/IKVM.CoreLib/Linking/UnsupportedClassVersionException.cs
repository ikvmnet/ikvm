using IKVM.ByteCode;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Thrown during linking when an unsupported class version is encounted.
    /// </summary>
    internal class UnsupportedClassVersionException : ClassFormatException
    {

        /// <summary>
        /// The lowest class file version that can be loaded.
        /// </summary>
        internal static readonly ClassFormatVersion MinSupportedVersion = new ClassFormatVersion(45, 0);

        /// <summary>
        /// The highest class file version that can be loaded.
        /// </summary>
        internal static readonly ClassFormatVersion MaxSupportedVersion = new ClassFormatVersion(52, 0);

        /// <summary>
        /// Returns <c>true</c> if a class file of the given version can be loaded.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static bool IsSupported(ClassFormatVersion version)
        {
            return version >= MinSupportedVersion && version <= MaxSupportedVersion;
        }

        /// <summary>
        /// Gets the name of the Java release that produces class files of the given version, for example "Java 11" for 55.0.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static string GetJavaVersionName(ClassFormatVersion version)
        {
            var name = version.Major switch
            {
                < 45 => "unknown Java version",
                45 => version.Minor < 3 ? "Java 1.0" : "Java 1.1",
                46 => "Java 1.2",
                47 => "Java 1.3",
                48 => "Java 1.4",
                _ => "Java " + (version.Major - 44),
            };

            // class files that depend on preview features of their release set every bit of the minor version
            if (version.Major >= 56 && version.Minor == 0xFFFF)
                name += " preview";

            return name;
        }

        /// <summary>
        /// Formats the diagnostic message the importer reports when a class file of the given version cannot be compiled.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static string FormatMessage(ClassFormatVersion version)
        {
            if (version < MinSupportedVersion)
                return $"Class file version {version} ({GetJavaVersionName(version)}) is not supported; IKVM supports class files from version {MinSupportedVersion} ({GetJavaVersionName(MinSupportedVersion)}) up to version {MaxSupportedVersion} ({GetJavaVersionName(MaxSupportedVersion)})";
            else
                return $"Class file version {version} ({GetJavaVersionName(version)}) is not supported; IKVM supports class files up to version {MaxSupportedVersion} ({GetJavaVersionName(MaxSupportedVersion)})";
        }

        /// <summary>
        /// Formats the message of the exception thrown to Java code, which matches the message produced by HotSpot. HotSpot
        /// uses the same wording for versions that are too old.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="className">The binary name of the class being defined, or <c>null</c> if it was not given.</param>
        /// <returns></returns>
        internal static string FormatJavaMessage(ClassFormatVersion version, string? className)
        {
            if (className == null)
                return $"Unsupported class file version {version}, this version of the Java Runtime only recognizes class file versions up to {MaxSupportedVersion}";
            else
                return $"{className.Replace('.', '/')} has been compiled by a more recent version of the Java Runtime (class file version {version}), this version of the Java Runtime only recognizes class file versions up to {MaxSupportedVersion}";
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="className">The binary name of the class being defined, or <c>null</c> if it was not given.</param>
        public UnsupportedClassVersionException(ClassFormatVersion version, string? className = null) :
            base(FormatJavaMessage(version, className))
        {
            Version = version;
        }

        /// <summary>
        /// Gets the unsupported version of the class file.
        /// </summary>
        public ClassFormatVersion Version { get; }

    }

}
