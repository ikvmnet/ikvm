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
            options.Inputs = context.ParseResult.GetValueForArgument(command.InputsArgument);
            options.Output = context.ParseResult.GetValueForOption(command.OutputOption);
            options.AssemblyName = context.ParseResult.GetValueForOption(command.AssemblyNameOption);
            options.Target = GetEnumValueForOption(context, command.TargetOption, ImportTarget.Unspecified);
            options.Platform = GetEnumValueForOption(context, command.PlatformOption, ImportPlatform.Unspecified);
            options.Apartment = GetEnumValueForOption(context, command.ApartmentOption, ImportApartment.Unspecified);
            options.NoGlobbing = context.ParseResult.GetValueForOption(command.NoGlobbingOption);
            options.EnableAssertions = ParseStringForOption<string[]?>(context, command.EnableAssertionsOption, null);
            options.DisableAssertions = ParseStringForOption<string[]?>(context, command.DisableAssertionsOption, null);
            options.Properties = GetDictionaryValueForOption<string, string>(context, command.PropertiesOption, "");
            options.RemoveAssertions = context.ParseResult.GetValueForOption(command.RemoveAssertionsOption);
            options.Main = context.ParseResult.GetValueForOption(command.MainClassOption);
            options.References = context.ParseResult.GetValueForOption(command.ReferenceOption) ?? [];
            options.Recurse = context.ParseResult.GetValueForOption(command.RecurseOption) ?? [];
            options.Resources = GetDictionaryValueForOption<string, FileInfo>(context, command.ResourceOption, null);
            options.ExternalResources = GetDictionaryValueForOption<string, FileInfo>(context, command.ExternalResourceOption, null);
            options.NoJNI = context.ParseResult.GetValueForOption(command.NoJNIOption);
            options.Exclude = context.ParseResult.GetValueForOption(command.ExcludeOption);
            options.Version = ParseStringForOption<Version?>(context, command.VersionOption, null);
            options.FileVersion = ParseStringForOption<Version?>(context, command.FileVersionOption, null);
            options.Win32Icon = context.ParseResult.GetValueForOption(command.Win32IconOption);
            options.Win32Manifest = context.ParseResult.GetValueForOption(command.Win32ManifestOption);
            options.KeyFile = context.ParseResult.GetValueForOption(command.KeyFileOption);
            options.Key = context.ParseResult.GetValueForOption(command.KeyOption);
            options.DelaySign = context.ParseResult.GetValueForOption(command.DelaySignOption);
            options.Debug = GetEnumValueForOption(context, command.DebugOption, ImportDebug.Unspecified);
            options.Deterministic = context.ParseResult.GetValueForOption(command.DeterministicOption);
            options.Optimize = context.ParseResult.GetValueForOption(command.OptimizeOption);
            options.SourcePath = context.ParseResult.GetValueForOption(command.SourcePathOption);
            options.Remap = context.ParseResult.GetValueForOption(command.RemapOption);
            options.NoStackTraceInfo = context.ParseResult.GetValueForOption(command.NoStackTraceInfoOption);
            options.RemoveUnusedPrivateFields = context.ParseResult.GetValueForOption(command.RemoveUnusedPrivateFieldsOption);
            options.CompressResources = context.ParseResult.GetValueForOption(command.CompressResourcesOption);
            options.StrictFinalFieldSemantics = context.ParseResult.GetValueForOption(command.StrictFinalFieldSemanticsOption);
            options.PrivatePackages = context.ParseResult.GetValueForOption(command.PrivatePackageOption) ?? [];
            options.PublicPackages = context.ParseResult.GetValueForOption(command.PublicPackageOption) ?? [];
            options.NoWarn = context.ParseResult.GetValueForOption(command.NoWarnOption) ?? [];
            options.WarnAsError = context.ParseResult.GetValueForOption(command.WarnAsErrorOption) ?? [];
            options.Runtime = context.ParseResult.GetValueForOption(command.RuntimeOption);
            options.Time = context.ParseResult.GetValueForOption(command.TimeOption);
            options.ClassLoader = context.ParseResult.GetValueForOption(command.ClassLoaderOption);
            options.SharedClassLoader = context.ParseResult.GetValueForOption(command.SharedClassLoaderOption);
            options.BaseAddress = context.ParseResult.GetValueForOption(command.BaseAddressOption);
            options.FileAlign = context.ParseResult.GetValueForOption(command.FileAlignOption);
            options.NoPeerCrossReference = context.ParseResult.GetValueForOption(command.NoPeerCrossReferenceOption);
            options.NoStdLib = context.ParseResult.GetValueForOption(command.NoStdLibOption);
            options.Libraries = context.ParseResult.GetValueForOption(command.LibraryOption) ?? [];
            options.NoAutoSerialization = context.ParseResult.GetValueForOption(command.NoAutoSerializationOption);
            options.HighEntropyVA = context.ParseResult.GetValueForOption(command.HighEntropyVAOption);
            options.Proxies = context.ParseResult.GetValueForOption(command.ProxyOption) ?? [];
            options.NoLogo = context.ParseResult.GetValueForOption(command.NoLogoOption);
            options.AllowNonVirtualCalls = context.ParseResult.GetValueForOption(command.AllowNonVirtualCallsOption);
            options.Static = context.ParseResult.GetValueForOption(command.StaticOption);
            options.NoJarStubs = context.ParseResult.GetValueForOption(command.NoJarStubsOption);
            options.AssemblyAttributes = context.ParseResult.GetValueForOption(command.AssemblyAttributesOption) ?? [];
            options.WarningLevel4Option = context.ParseResult.GetValueForOption(command.WarningLevel4Option);
            options.NoParameterReflection = context.ParseResult.GetValueForOption(command.NoParameterReflectionOption);
            options.Bootstrap = context.ParseResult.GetValueForOption(command.BootstrapOption);
            options.Log = context.ParseResult.GetValueForOption(command.LogOption);

            // for each nested level, run a command to capture the options, but otherwise does nothing
            foreach (var level in _levels)
            {
                var nestedCommand = new ImportCommand();
                var nestedOptions = new List<ImportOptions>(level.Args.Count);
                nestedCommand.SetHandler(nestedOptions.Add, new ImportOptionsBinding(level.Nested.ToArray(), options));
                nestedCommand.Invoke(level.Args.ToArray());
                options.Nested = nestedOptions.ToArray();
            }

            return options;
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