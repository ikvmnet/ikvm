#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Binding;
using System.ComponentModel;
using System.IO;
using System.Linq;

using IKVM.Tools.Core.CommandLine;

namespace IKVM.Tools.Importer
{

    class ImportOptionsBinding : BinderBase<ImportOptions>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ImportOptionsBinding()
        {

        }

        /// <inheritdoc />
        protected override ImportOptions GetBoundValue(BindingContext context)
        {
            var command = (ImportCommand)context.ParseResult.CommandResult.Command;

            return new ImportOptions()
            {
                Inputs = context.ParseResult.GetValueForArgument(command.InputsArgument),
                Output = context.ParseResult.GetValueForOption(command.OutputOption),
                AssemblyName = context.ParseResult.GetValueForOption(command.AssemblyNameOption),
                Target = GetEnumValueForOption(context, command.TargetOption, ImportTarget.Unspecified),
                Platform = GetEnumValueForOption(context, command.PlatformOption, ImportPlatform.Unspecified),
                Apartment = GetEnumValueForOption(context, command.ApartmentOption, ImportApartment.Unspecified),
                NoGlobbing = context.ParseResult.GetValueForOption(command.NoGlobbingOption),
                EnableAssertions = ParseStringForOption<string[]?>(context, command.EnableAssertionsOption, null),
                DisableAssertions = ParseStringForOption<string[]?>(context, command.DisableAssertionsOption, null),
                Properties = GetDictionaryValueForOption<string, string>(context, command.PropertiesOption, ""),
                RemoveAssertions = context.ParseResult.GetValueForOption(command.RemoveAssertionsOption),
                Main = context.ParseResult.GetValueForOption(command.MainClassOption),
                References = context.ParseResult.GetValueForOption(command.ReferenceOption) ?? [],
                Recurse = context.ParseResult.GetValueForOption(command.RecurseOption) ?? [],
                Resources = GetDictionaryValueForOption<string, FileInfo>(context, command.ResourceOption, null),
                ExternalResources = GetDictionaryValueForOption<string, FileInfo>(context, command.ExternalResourceOption, null),
                NoJNI = context.ParseResult.GetValueForOption(command.NoJNIOption),
                Exclude = context.ParseResult.GetValueForOption(command.ExcludeOption),
                Version = ParseStringForOption<Version?>(context, command.VersionOption, null),
                FileVersion = ParseStringForOption<Version?>(context, command.FileVersionOption, null),
                Win32Icon = context.ParseResult.GetValueForOption(command.Win32IconOption),
                Win32Manifest = context.ParseResult.GetValueForOption(command.Win32ManifestOption),
                KeyFile = context.ParseResult.GetValueForOption(command.KeyFileOption),
                Key = context.ParseResult.GetValueForOption(command.KeyOption),
                DelaySign = context.ParseResult.GetValueForOption(command.DelaySignOption),
                Debug = GetEnumValueForOption(context, command.DebugOption, ImportDebug.Unspecified),
                Deterministic = context.ParseResult.GetValueForOption(command.DeterministicOption),
                Optimize = context.ParseResult.GetValueForOption(command.OptimizeOption),
                SourcePath = context.ParseResult.GetValueForOption(command.SourcePathOption),
                Remap = context.ParseResult.GetValueForOption(command.RemapOption),
                NoStackTraceInfo = context.ParseResult.GetValueForOption(command.NoStackTraceInfoOption),
                RemoveUnusedPrivateFields = context.ParseResult.GetValueForOption(command.RemoveUnusedPrivateFieldsOption),
                CompressResources = context.ParseResult.GetValueForOption(command.CompressResourcesOption),
                StrictFinalFieldSemantics = context.ParseResult.GetValueForOption(command.StrictFinalFieldSemanticsOption),
                PrivatePackages = context.ParseResult.GetValueForOption(command.PrivatePackageOption) ?? [],
                PublicPackages = context.ParseResult.GetValueForOption(command.PublicPackageOption) ?? [],
                NoWarn = context.ParseResult.GetValueForOption(command.NoWarnOption) ?? [],
                WarnAsError = context.ParseResult.GetValueForOption(command.WarnAsErrorOption) ?? [],
                Runtime = context.ParseResult.GetValueForOption(command.RuntimeOption),
                Time = context.ParseResult.GetValueForOption(command.TimeOption),
                ClassLoader = context.ParseResult.GetValueForOption(command.ClassLoaderOption),
                SharedClassLoader = context.ParseResult.GetValueForOption(command.SharedClassLoaderOption),
                BaseAddress = context.ParseResult.GetValueForOption(command.BaseAddressOption),
                FileAlign = context.ParseResult.GetValueForOption(command.FileAlignOption),
                NoPeerCrossReference = context.ParseResult.GetValueForOption(command.NoPeerCrossReferenceOption),
                NoStdLib = context.ParseResult.GetValueForOption(command.NoStdLibOption),
                Libraries = context.ParseResult.GetValueForOption(command.LibraryOption) ?? [],
                NoAutoSerialization = context.ParseResult.GetValueForOption(command.NoAutoSerializationOption),
                HighEntropyVA = context.ParseResult.GetValueForOption(command.HighEntropyVAOption),
                Proxies = context.ParseResult.GetValueForOption(command.ProxyOption) ?? [],
                NoLogo = context.ParseResult.GetValueForOption(command.NoLogoOption),
                AllowNonVirtualCalls = context.ParseResult.GetValueForOption(command.AllowNonVirtualCallsOption),
                Static = context.ParseResult.GetValueForOption(command.StaticOption),
                NoJarStubs = context.ParseResult.GetValueForOption(command.NoJarStubsOption),
                AssemblyAttributes = context.ParseResult.GetValueForOption(command.AssemblyAttributesOption) ?? [],
                WarningLevel4Option = context.ParseResult.GetValueForOption(command.WarningLevel4Option),
                NoParameterReflection = context.ParseResult.GetValueForOption(command.NoParameterReflectionOption),
                Bootstrap = context.ParseResult.GetValueForOption(command.BootstrapOption),
                Log = context.ParseResult.GetValueForOption(command.LogOption),
            };
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