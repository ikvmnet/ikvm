#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.IO;
using System.Linq;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.CommandLine;

namespace IKVM.Tools.Importer
{

    class ImportOptionsBinding : BinderBase<ImportOptions>
    {

        readonly ImportArgLevel[] _levels;
        readonly ImportOptions _parent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ImportOptionsBinding(ImportArgLevel[] nested, ImportOptions parent)
        {
            _levels = nested ?? throw new ArgumentNullException(nameof(nested));
            _parent = parent;
        }

        /// <inheritdoc />
        protected override ImportOptions GetBoundValue(BindingContext context)
        {
            var command = (ImportCommand)context.ParseResult.CommandResult.Command;

            var options = _parent?.Clone() ?? new ImportOptions();

            if (context.ParseResult.GetValueForArgument(command.InputsArgument) is FileInfo[] _inputs)
                options.Inputs = AppendArray(options.Inputs, _inputs);

            if (context.ParseResult.GetValueForOption(command.RecurseOption) is string[] _recurse)
                options.Recurse = AppendArray(options.Recurse, _recurse);

            if (context.ParseResult.GetValueForOption(command.AssemblyAttributesOption) is FileInfo[] _assemblyAttributes)
                options.AssemblyAttributes = AppendArray(options.AssemblyAttributes, _assemblyAttributes);

            if (GetDictionaryValueForOption<string, FileInfo>(context, command.ResourceOption, null) is IReadOnlyDictionary<string, FileInfo> _resources)
                options.Resources = AppendDictionaries(options.Resources, _resources);

            if (GetDictionaryValueForOption<string, FileInfo>(context, command.ExternalResourceOption, null) is IReadOnlyDictionary<string, FileInfo> _externalresources)
                options.ExternalResources = AppendDictionaries(options.ExternalResources, _externalresources);

            if (context.ParseResult.GetValueForOption(command.OutputOption) is FileInfo _output)
                options.Output = _output;

            if (context.ParseResult.GetValueForOption(command.AssemblyNameOption) is string _assemblyName)
                options.AssemblyName = _assemblyName;

            if (GetEnumValueForOption(context, command.TargetOption, ImportTarget.Unspecified) is ImportTarget _target and not ImportTarget.Unspecified)
                options.Target = _target;

            if (GetEnumValueForOption(context, command.PlatformOption, ImportPlatform.Unspecified) is ImportPlatform _platform and not ImportPlatform.Unspecified)
                options.Platform = _platform;

            if (GetEnumValueForOption(context, command.ApartmentOption, ImportApartment.Unspecified) is ImportApartment _apartment and not ImportApartment.Unspecified)
                options.Apartment = _apartment;

            if (context.ParseResult.GetValueForOption(command.NoGlobbingOption) is true)
                options.NoGlobbing = true;

            if (ParseStringForOption<string[]?>(context, command.EnableAssertionsOption, null) is string[] _ea)
                options.EnableAssertions = _ea;

            if (ParseStringForOption<string[]?>(context, command.DisableAssertionsOption, null) is string[] _da)
                options.DisableAssertions = _da;

            if (GetDictionaryValueForOption<string, string>(context, command.PropertiesOption, "") is IReadOnlyDictionary<string, string> _properties)
                options.Properties = AppendDictionaries(options.Properties, _properties);

            if (context.ParseResult.GetValueForOption(command.RemoveAssertionsOption) is true)
                options.RemoveAssertions = true;

            if (context.ParseResult.GetValueForOption(command.MainClassOption) is string _main)
                options.Main = _main;

            if (context.ParseResult.GetValueForOption(command.ReferenceOption) is string[] _references)
                options.References = AppendArray(options.References, _references);

            if (context.ParseResult.GetValueForOption(command.NoJNIOption) is true)
                options.NoJNI = true;

            if (context.ParseResult.GetValueForOption(command.ExcludeOption) is FileInfo _exclude)
                options.Exclude = _exclude;

            if (ParseStringForOption<Version?>(context, command.VersionOption, null) is Version _version)
                options.Version = _version;

            if (ParseStringForOption<Version?>(context, command.FileVersionOption, null) is Version _fileversion)
                options.FileVersion = _fileversion;

            if (context.ParseResult.GetValueForOption(command.Win32IconOption) is FileInfo _win32icon)
                options.Win32Icon = _win32icon;

            if (context.ParseResult.GetValueForOption(command.Win32ManifestOption) is FileInfo _win32manifest)
                options.Win32Manifest = _win32manifest;

            if (context.ParseResult.GetValueForOption(command.KeyFileOption) is FileInfo _keyfile)
                options.KeyFile = _keyfile;

            if (context.ParseResult.GetValueForOption(command.KeyOption) is string _keyoption)
                options.Key = _keyoption;

            if (context.ParseResult.GetValueForOption(command.DelaySignOption) is true)
                options.DelaySign = true;

            if (GetEnumValueForOption(context, command.DebugOption, ImportDebug.Unspecified) is ImportDebug _debug and not ImportDebug.Unspecified)
                options.Debug = _debug;

            if (context.ParseResult.GetValueForOption(command.DeterministicOption) is false)
                options.Deterministic = false;

            if (context.ParseResult.GetValueForOption(command.OptimizeOption) is true)
                options.Optimize = true;

            if (context.ParseResult.GetValueForOption(command.SourcePathOption) is DirectoryInfo _sourcePath)
                options.SourcePath = _sourcePath;

            if (context.ParseResult.GetValueForOption(command.RemapOption) is FileInfo _remap)
                options.Remap = _remap;

            if (context.ParseResult.GetValueForOption(command.NoStackTraceInfoOption) is true)
                options.NoStackTraceInfo = true;

            if (context.ParseResult.GetValueForOption(command.RemoveUnusedPrivateFieldsOption) is true)
                options.RemoveUnusedPrivateFields = true;

            if (context.ParseResult.GetValueForOption(command.CompressResourcesOption) is true)
                options.CompressResources = true;

            if (context.ParseResult.GetValueForOption(command.StrictFinalFieldSemanticsOption) is true)
                options.StrictFinalFieldSemantics = true;

            if (context.ParseResult.GetValueForOption(command.PrivatePackageOption) is string[] _privatepackages)
                options.PrivatePackages = AppendArray(options.PrivatePackages, _privatepackages);

            if (context.ParseResult.GetValueForOption(command.PublicPackageOption) is string[] _publicpackages)
                options.PublicPackages = AppendArray(options.PublicPackages, _publicpackages);

            if (context.ParseResult.GetValueForOption(command.NoWarnOption) is Diagnostic[] _nowarn)
                options.NoWarn = options.NoWarn != null ? AppendArray(options.NoWarn, _nowarn) : _nowarn;

            if (context.ParseResult.GetValueForOption(command.WarnAsErrorOption) is Diagnostic[] _warnaserror)
                options.WarnAsError = options.WarnAsError != null ? AppendArray(options.WarnAsError, _warnaserror) : _warnaserror;

            if (context.ParseResult.GetValueForOption(command.RuntimeOption) is FileInfo _runtime)
                options.Runtime = _runtime;

            if (context.ParseResult.GetValueForOption(command.TimeOption) is true)
                options.Time = true;

            if (context.ParseResult.GetValueForOption(command.ClassLoaderOption) is string _classloader)
                options.ClassLoader = _classloader;

            if (context.ParseResult.GetValueForOption(command.SharedClassLoaderOption) is true)
                options.SharedClassLoader = true;

            if (context.ParseResult.GetValueForOption(command.BaseAddressOption) is string _baseaddress)
                options.BaseAddress = _baseaddress;

            if (context.ParseResult.GetValueForOption(command.FileAlignOption) is string _filealign)
                options.FileAlign = _filealign;

            if (context.ParseResult.GetValueForOption(command.NoPeerCrossReferenceOption) is true)
                options.NoPeerCrossReference = true;

            if (context.ParseResult.GetValueForOption(command.NoStdLibOption) is true)
                options.NoStdLib = true;

            if (context.ParseResult.GetValueForOption(command.LibraryOption) is DirectoryInfo[] _libs)
                options.Libraries = AppendArray(options.Libraries, _libs);

            if (context.ParseResult.GetValueForOption(command.NoAutoSerializationOption) is true)
                options.NoAutoSerialization = true;

            if (context.ParseResult.GetValueForOption(command.HighEntropyVAOption) is true)
                options.HighEntropyVA = true;

            if (context.ParseResult.GetValueForOption(command.ProxyOption) is string[] _proxies)
                options.Proxies = _proxies;

            if (context.ParseResult.GetValueForOption(command.NoLogoOption) is true)
                options.NoLogo = true;

            if (context.ParseResult.GetValueForOption(command.AllowNonVirtualCallsOption) is true)
                options.AllowNonVirtualCalls = true;

            if (context.ParseResult.GetValueForOption(command.StaticOption) is true)
                options.Static = true;

            if (context.ParseResult.GetValueForOption(command.NoJarStubsOption) is true)
                options.NoJarStubs = true;

            if (context.ParseResult.GetValueForOption(command.WarningLevel4Option) is true)
                options.WarningLevel4Option = true;

            if (context.ParseResult.GetValueForOption(command.NoParameterReflectionOption) is true)
                options.NoParameterReflection = true;

            if (context.ParseResult.GetValueForOption(command.BootstrapOption) is true)
                options.Bootstrap = true;

            if (context.ParseResult.GetValueForOption(command.LogOption) is string _log)
                options.Log = _log;

            // for each nested level, run a command to capture the options, but otherwise does nothing
            foreach (var level in _levels)
            {
                var nestedCommand = new ImportCommand(false);
                var nestedOptions = new List<ImportOptions>(level.Args.Count);
                nestedCommand.SetHandler(nestedOptions.Add, new ImportOptionsBinding(level.Nested.ToArray(), options));
                new CommandLineBuilder(new ImportCommand(false)).Build().Invoke(level.Args.ToArray());
                options.Nested = nestedOptions.ToArray();
            }

            return options;
        }

        /// <summary>
        /// Returns a new dictionary with the first and second values where the second values replace the first.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        T[] AppendArray<T>(T[] first, T[] second)
        {
            var a = new T[first.Length + second.Length];
            Array.Copy(first, 0, a, 0, first.Length);
            Array.Copy(second, 0, a, first.Length, second.Length);
            return a;
        }

        /// <summary>
        /// Returns a new dictionary with the first and second values where the second values replace the first.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        IReadOnlyDictionary<TKey, TValue> AppendDictionaries<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> first, IReadOnlyDictionary<TKey, TValue> second)
            where TKey : notnull
        {
            var d = new Dictionary<TKey, TValue>();

            foreach (var kvp in first)
                d.Add(kvp.Key, kvp.Value);

            foreach (var kvp in second)
                d[kvp.Key] = kvp.Value;

            return d;
        }

        /// <summary>
        /// Parses the specified string option as a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T ParseStringForOption<T>(BindingContext context, Option<string?> option, T defaultValue)
        {
            return context.ParseResult.GetValueForOption(option) is string v ? Parse<T>(v) ?? defaultValue : defaultValue;
        }

        /// <summary>
        /// Parses the specified string option as a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string[]? ParseStringToStringArrayForOption(BindingContext context, Option<string?> option, char[] separator, string[]? defaultValue)
        {
            return context.ParseResult.GetValueForOption(option) is string v ? v.Split(separator, StringSplitOptions.RemoveEmptyEntries) : defaultValue;
        }

        /// <summary>
        /// Parses the specified string option as a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int[]? ParseDiagnosticIdListForOption(BindingContext context, Option<string?> option, char[] separator, int[]? defaultValue)
        {
            return context.ParseResult.GetValueForOption(option) is string v ? v.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(SafeParseDiagnosticId).Where(i => i != null).OfType<int>().ToArray() : defaultValue;
        }

        /// <summary>
        /// Parses a string value into a Diagnostic ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        int? SafeParseDiagnosticId(string value)
        {
            if (int.TryParse(value, out var i))
                return i;

            if (value.StartsWith("IKVM", StringComparison.OrdinalIgnoreCase))
                return SafeParseDiagnosticId(value["IKVM".Length..]);

            return null;
        }

        /// <summary>
        /// Gets the value for the string option parsed as an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetEnumValueForOption<T>(BindingContext context, Option<string?> option, T defaultValue)
            where T : notnull, Enum
        {
            return context.ParseResult.GetValueForOption(option) is string v ? (T)Enum.Parse(typeof(T), v, true) : defaultValue;
        }

        /// <summary>
        /// Gets the value for the string option parsed as an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        IReadOnlyDictionary<TKey, TValue> GetDictionaryValueForOption<TKey, TValue>(BindingContext context, Option<string[]> option, TValue? defaultValue)
            where TKey : notnull
        {
            return context.ParseResult.GetValueForOption(option) is string[] v ? ParseDictionaryValues<TKey, TValue>(context, option, v, defaultValue) : ImmutableDictionary<TKey, TValue>.Empty;
        }

        /// <summary>
        /// Parses the dictionary values.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="context"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IReadOnlyDictionary<TKey, TValue> ParseDictionaryValues<TKey, TValue>(BindingContext context, Option option, string[] values, TValue? defaultValue)
            where TKey : notnull
        {
            var dict = new Dictionary<TKey, TValue>();

            foreach (var kvp in values.Select(i => ParseDictionaryValue<TKey, TValue>(context, i)))
            {
                if (dict.ContainsKey(kvp.Key))
                    throw new CommandErrorException($"Option name '{option.Name}' contains multiple values for key '{kvp.Key}'.");

                dict[kvp.Key] = kvp.Value ?? defaultValue ?? throw new InvalidOperationException();
            }

            return dict;
        }

        /// <summary>
        /// Parses a single dictionary key and value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="context"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue?> ParseDictionaryValue<TKey, TValue>(BindingContext context, string opt)
            where TKey : notnull
        {
            var c = opt.Split(['='], 2, StringSplitOptions.None);

            // parse key value
            var k = Parse<TKey>(c[0].Trim());
            if (k == null)
                throw new InvalidOperationException();

            if (c.Length == 2)
                return new KeyValuePair<TKey, TValue?>(k, Parse<TValue>(c[1]));

            if (c.Length == 1)
                return new KeyValuePair<TKey, TValue?>(k, default);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Parses the given source value into the type specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        T? Parse<T>(string source)
        {
            if (source is T t)
                return t;

            if (typeof(T) == typeof(FileInfo))
                return string.IsNullOrWhiteSpace(source) == false ? (T)(object)new FileInfo(source) : default;

            if (typeof(T) == typeof(DirectoryInfo))
                return string.IsNullOrWhiteSpace(source) == false ? (T)(object)new DirectoryInfo(source) : default;

            if (typeof(T) == typeof(Version))
                return (T)(object)Version.Parse(source);

            if (typeof(T) == typeof(string[]))
                return (T)(object)source.Split([';', ','], StringSplitOptions.RemoveEmptyEntries);

            // fall back to type converter
            if (TypeDescriptor.GetConverter(typeof(T)) is TypeConverter c && c.CanConvertFrom(typeof(string)))
                return (T?)c.ConvertFromString(source);

            throw new InvalidOperationException();
        }

    }

}