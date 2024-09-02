#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Importer
{

    class ImportOptions
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
        public bool CompressResources { get; set; }

        /// <summary>
        /// How debug symbols should be produced along with the assembly.
        /// </summary>
        public ImportDebug Debug { get; set; } = ImportDebug.Unspecified;

        public bool NoAutoSerialization { get; set; } = false;

        public bool NoGlobbing { get; set; } = false;

        public bool NoJNI { get; set; } = false;

        public bool RemoveUnusedPrivateFields { get; set; } = false;

        public string[]? EnableAssertions { get; set; }

        public string[]? DisableAssertions { get; set; }

        public bool RemoveAssertions { get; set; } = false;

        public bool StrictFinalFieldSemantics { get; set; } = false;

        public Diagnostic[]? NoWarn { get; set; }

        public Diagnostic[]? WarnAsError { get; set; }

        public string? Main { get; set; }

        public DirectoryInfo? SourcePath { get; set; }

        public ImportApartment Apartment { get; set; } = ImportApartment.Unspecified;

        public IReadOnlyDictionary<string, string> Properties { get; set; } = ImmutableDictionary<string, string>.Empty;

        public bool NoStackTraceInfo { get; set; }

        public string[] PrivatePackages { get; set; } = [];

        public string[] PublicPackages { get; set; } = [];

        public string? ClassLoader { get; set; }

        public bool SharedClassLoader { get; set; } = false;

        public string? BaseAddress { get; set; }

        public string? FileAlign { get; set; }

        public bool NoPeerCrossReference { get; set; } = false;

        public bool NoStdLib { get; set; } = false;

        public DirectoryInfo[] Libraries { get; set; } = [];

        public bool HighEntropyVA { get; set; } = false;

        public bool Static { get; set; } = false;

        public FileInfo[] AssemblyAttributes { get; set; } = [];

        public FileInfo? Runtime { get; set; }

        public bool WarningLevel4Option { get; set; } = false;

        public bool NoParameterReflection { get; set; } = false;

        public FileInfo? Remap { get; set; }

        public bool NoLogo { get; set; } = false;

        public bool Time { get; set; } = false;

        public string[] Proxies { get; set; } = [];

        public bool AllowNonVirtualCalls { get; set; } = false;

        public bool NoJarStubs { get; set; } = false;

        public string? Log { get; set; }

        public bool Bootstrap { get; set; } = false;

        public bool Optimize { get; set; } = false;

        public bool Deterministic { get; set; } = false;

    }

}
