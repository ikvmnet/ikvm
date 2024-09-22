#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Importer
{

    class ImportOptions : ICloneable
    {

        /// <summary>
        /// Input files to be compiled.
        /// </summary>
        public FileInfo[] Inputs { get; set; } = [];

        /// <summary>
        /// Path of the output assembly.
        /// </summary>
        public FileInfo? Output { get; set; }

        /// <summary>
        /// Name of the output assembly.
        /// </summary>
        public string? AssemblyName { get; set; }

        /// <summary>
        /// Version of the output assembly.
        /// </summary>
        public Version? Version { get; set; }

        /// <summary>
        /// Target to build.
        /// </summary>
        public ImportTarget Target { get; set; } = ImportTarget.Unspecified;

        /// <summary>
        /// Platform to build.
        /// </summary>
        public ImportPlatform Platform { get; set; } = ImportPlatform.Unspecified;

        /// <summary>
        /// Path to the key to use for strong name signing.
        /// </summary>
        public FileInfo? KeyFile { get; set; }

        /// <summary>
        /// Identity of the key to use for signing.
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Whether the assembly should be delay signed.
        /// </summary>
        public bool DelaySign { get; set; }

        /// <summary>
        /// Set of paths or names to assemblies to add as references.
        /// </summary>
        public string[] References { get; set; } = [];

        /// <summary>
        /// Directories to recurse into to collect files.
        /// </summary>
        public string[] Recurse { get; set; } = [];

        /// <summary>
        /// Path to file containing exclude expressions.
        /// </summary>
        public FileInfo? Exclude { get; set; }

        /// <summary>
        /// File version to configure on the assembly.
        /// </summary>
        public Version? FileVersion { get; set; }

        /// <summary>
        /// Win32 icon to configure on the assembly.
        /// </summary>
        public FileInfo? Win32Icon { get; set; }

        /// <summary>
        /// Win32 manifest to embed within the assembly.
        /// </summary>
        public FileInfo? Win32Manifest { get; set; }

        /// <summary>
        /// Set of resources to embed within the assembly.
        /// </summary>
        public IReadOnlyDictionary<string, FileInfo> Resources { get; set; } = ImmutableDictionary<string, FileInfo>.Empty;

        /// <summary>
        /// Set of external resource to add to the assembly.
        /// </summary>
        public IReadOnlyDictionary<string, FileInfo> ExternalResources { get; set; } = ImmutableDictionary<string, FileInfo>.Empty;

        /// <summary>
        /// Whether resources should be compressed.
        /// </summary>
        public bool CompressResources { get; set; } = false;

        /// <summary>
        /// How debug symbols should be produced along with the assembly.
        /// </summary>
        public ImportDebug Debug { get; set; } = ImportDebug.Unspecified;

        /// <summary>
        /// Whether to disable auto generation of serialization capabilities.
        /// </summary>
        public bool NoAutoSerialization { get; set; } = false;

        /// <summary>
        /// Disables globbing of Java command line arguments in the generated assembly.
        /// </summary>
        public bool NoGlobbing { get; set; } = false;

        /// <summary>
        /// Disables the usage of JNI in the generated assembly.
        /// </summary>
        public bool NoJNI { get; set; } = false;

        /// <summary>
        /// Enables removing unused fields from Java classes.
        /// </summary>
        public bool RemoveUnusedPrivateFields { get; set; } = false;

        /// <summary>
        /// If null, no assertions are enabled. If an empty array, all assertions are enabled. Else only assertions of the specified granularity are enabled.
        /// </summary>
        public string[]? EnableAssertions { get; set; }

        /// <summary>
        /// If null, no assertions are disables. If an empty array, all assertions are disabled. Else only assertions of the specified granularity are disabled.
        /// </summary>
        public string[]? DisableAssertions { get; set; }

        /// <summary>
        /// Removes assertions.
        /// </summary>
        public bool RemoveAssertions { get; set; } = false;

        public bool StrictFinalFieldSemantics { get; set; } = false;

        /// <summary>
        /// List of warning Diagnostic types that should not issue warnings.
        /// </summary>
        public Diagnostic[]? NoWarn { get; set; }

        /// <summary>
        /// List of warning Diagnostic types that should be elevated to an error.
        /// </summary>
        public Diagnostic[]? WarnAsError { get; set; }

        /// <summary>
        /// Name of the main entry point class.
        /// </summary>
        public string? Main { get; set; }

        /// <summary>
        /// Root of the original source file path for debugging information.
        /// </summary>
        public DirectoryInfo? SourcePath { get; set; }

        public ImportApartment Apartment { get; set; } = ImportApartment.Unspecified;

        /// <summary>
        /// Properties to be set in the JVM upon entry of the executable assembly.
        /// </summary>
        public IReadOnlyDictionary<string, string> Properties { get; set; } = ImmutableDictionary<string, string>.Empty;

        public bool NoStackTraceInfo { get; set; }

        public string[] PrivatePackages { get; set; } = [];

        public string[] PublicPackages { get; set; } = [];

        /// <summary>
        /// Set the name of a custom class loader for this assembly.
        /// </summary>
        public string? ClassLoader { get; set; }

        /// <summary>
        /// Enable the shared class loader.
        /// </summary>
        public bool SharedClassLoader { get; set; } = false;

        public string? BaseAddress { get; set; }

        public string? FileAlign { get; set; }

        public bool NoPeerCrossReference { get; set; } = false;

        /// <summary>
        /// Disables lookup of the standard library. This option is irrelevent since lookup of the standard library is unsupported.
        /// </summary>
        public bool NoStdLib { get; set; } = true;

        public DirectoryInfo[] Libraries { get; set; } = [];

        public bool HighEntropyVA { get; set; } = false;

        /// <summary>
        /// Enable static mode. This prevents generation of dynamic invocation delegates for missing classes.
        /// </summary>
        public bool Static { get; set; } = false;

        public FileInfo[] AssemblyAttributes { get; set; } = [];

        public bool WarningLevel4Option { get; set; } = false;

        public bool NoParameterReflection { get; set; } = false;

        public FileInfo? Remap { get; set; }

        public bool Time { get; set; } = false;

        public string[] Proxies { get; set; } = [];

        public bool AllowNonVirtualCalls { get; set; } = false;

        public bool NoJarStubs { get; set; } = false;

        /// <summary>
        /// Path to the IKVM.Runtime assembly.
        /// </summary>
        public FileInfo? Runtime { get; set; }

        /// <summary>
        /// Hidden option to enable bootstrap mode for compiling the BCL.
        /// </summary>
        public bool Bootstrap { get; set; } = false;

        public string? Log { get; set; }

        /// <summary>
        /// Enables optimizations.
        /// </summary>
        public bool Optimize { get; set; } = false;

        /// <summary>
        /// Enables a deterministic assembly.
        /// </summary>
        public bool Deterministic { get; set; } = true;

        /// <summary>
        /// Set of nested options when compiling multiple assemblies using the shared class loader.
        /// </summary>
        public ImportOptions[] Nested { get; set; } = [];

        /// <inheritdoc />
        public ImportOptions Clone()
        {
            return (ImportOptions)MemberwiseClone();
        }

        /// <summary>
        /// Creates a clone of this object.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

    }

}