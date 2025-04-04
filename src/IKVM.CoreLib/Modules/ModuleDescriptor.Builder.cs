using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

using IKVM.ByteCode;

namespace IKVM.CoreLib.Modules
{

    public readonly partial struct ModuleDescriptor
    {

        /// <summary>
        /// Instantiates a builder to build a module descriptor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public static Builder CreateModule(string name, ModuleFlag modifiers)
        {
            return new Builder(name, modifiers);
        }

        /// <summary>
        /// Instantiates a builder to build a module descriptor for a <em>normal</em> module. This method is equivalent to invoking <see cref="CreateModule(string, ModuleFlag)"/> with the default value of <see cref="ModuleFlag"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Builder CreateModule(string name)
        {
            return new Builder(name, false);
        }

        /// <summary>
        /// Instantiates a builder to build a module descriptor for an automatic module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Builder CreateAutomaticModule(string name)
        {
            return new Builder(name, true);
        }

        /// <summary>
        /// A builder for building <see cref="ModuleDescriptor" /> objects.
        /// </summary>
        public class Builder
        {

            readonly string _name;
            readonly ModuleFlag _modifiers;
            readonly bool _automatic;
            ModuleVersion _version;
            HashSet<string>? _requiresKeys;
            ImmutableHashSet<ModuleRequires>.Builder? _requires;
            HashSet<string>? _exportsKeys;
            ImmutableHashSet<ModuleExports>.Builder? _exports;
            HashSet<string>? _opensKeys;
            ImmutableHashSet<ModuleOpens>.Builder? _opens;
            ImmutableHashSet<string>.Builder? _uses;
            HashSet<string>? _providesKeys;
            ImmutableHashSet<ModuleProvides>.Builder? _provides;
            ImmutableHashSet<string>.Builder? _packages;
            string? _mainClass;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            internal Builder(string name, bool automatic)
            {
                _name = name;
                _modifiers = default;
                _automatic = automatic;
                _version = default;
                Debug.Assert(!IsOpen || !IsAutomatic);
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            internal Builder(string name, ModuleFlag modifiers)
            {
                _name = name;
                _modifiers = modifiers;
                _automatic = false;
                _version = default;
                Debug.Assert(!IsOpen || !IsAutomatic);
            }

            /// <summary>
            /// Gets the module name.
            /// </summary>
            public string Name => _name;

            /// <summary>
            /// Gets the modifiers.
            /// </summary>
            public ModuleFlag Modifiers => _modifiers;

            /// <summary>
            /// Gets whether this is an automatic module or not.
            /// </summary>
            public bool IsAutomatic => _automatic;

            /// <summary>
            /// Gets whether this is an open module or not.
            /// </summary>
            public bool IsOpen => (_modifiers & ModuleFlag.Open) != 0;

            /// <summary>
            /// Adds a dependence on a module.
            /// </summary>
            /// <param name="requires"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder Requires(ModuleRequires requires)
            {
                if (_automatic)
                    throw new InvalidOperationException("Automatic modules cannot declare dependences");
                if (_name.Equals(requires.Name))
                    throw new ArgumentException("Dependence on self.");
                if (_requiresKeys != null && _requiresKeys.Contains(requires.Name))
                    throw new InvalidOperationException($"Dependence upon {requires.Name} already declared.");

                _requiresKeys ??= new HashSet<string>();
                _requiresKeys.Add(requires.Name);
                _requires ??= ImmutableHashSet.CreateBuilder<ModuleRequires>();
                _requires.Add(requires);

                return this;
            }

            /// <summary>
            /// Adds a dependence on a module with the given (and possibly empty) set of modifiers. The dependence
            /// includes the version of the module that was recorded at compile-time.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <param name="version"></param>
            /// <returns></returns>
            public Builder Requires(ModuleRequiresFlag modifiers, string name, ModuleVersion version)
            {
                return Requires(new ModuleRequires(modifiers, name, version));
            }

            /// <summary>
            /// Adds a dependence on a module with the given (and possibly empty) set of modifiers. The dependence
            /// includes the version of the module that was recorded at compile-time.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <param name="version"></param>
            /// <returns></returns>
            public Builder Requires(ModuleRequiresFlag modifiers, string name, string? version)
            {
                var v = default(ModuleVersion);
                if (version is not null)
                    ModuleVersion.TryParse(version, out v);

                return Requires(new ModuleRequires(modifiers, name, v));
            }

            /// <summary>
            /// Adds a dependence on a module with the given (and possibly empty) set of modifiers. The dependence
            /// includes the version of the module that was recorded at compile-time.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder Requires(ModuleRequiresFlag modifiers, string name)
            {
                return Requires(new ModuleRequires(modifiers, name, default));
            }

            /// <summary>
            /// Adds a dependence on a module with the given (and possibly empty) set of modifiers. The dependence
            /// includes the version of the module that was recorded at compile-time.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder Requires(string name)
            {
                return Requires(new ModuleRequires(default, name, default));
            }

            /// <summary>
            /// Adds an exported package.
            /// </summary>
            /// <param name="exports"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder Exports(ModuleExports exports)
            {
                if (_automatic)
                    throw new InvalidOperationException("Automatic modules cannot declare dependences.");
                if (_exportsKeys != null && _exportsKeys.Contains(exports.Source))
                    throw new InvalidOperationException($"Exported package {exports.Source} already declared.");

                _exportsKeys ??= new HashSet<string>();
                _exportsKeys.Add(exports.Source);
                _exports ??= ImmutableHashSet.CreateBuilder<ModuleExports>();
                _exports.Add(exports);

                return this;
            }

            /// <summary>
            /// Adds an exported package with the given (and possibly empty) set of modifiers. The package is exported to a set of target modules.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <param name="targets"></param>
            /// <returns></returns>
            public Builder Exports(ModuleExportsFlag modifiers, string name, ImmutableHashSet<string> targets)
            {
                return Exports(new ModuleExports(modifiers, name, targets));
            }

            /// <summary>
            /// Adds an exported package with the given (and possibly empty) set of modifiers.The package is exported to all modules.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder Exports(ModuleExportsFlag modifiers, string name)
            {
                return Exports(modifiers, name, []);
            }

            /// <summary>
            /// Adds an exported package. The package is exported to a set of target modules.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="targets"></param>
            /// <returns></returns>
            public Builder Exports(string name, ImmutableHashSet<string> targets)
            {
                return Exports(default, name, targets);
            }

            /// <summary>
            /// Adds an exported package. The package is exported to all modules.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder Exports(string name)
            {
                return Exports(default, name, []);
            }

            /// <summary>
            /// Adds an open package.
            /// </summary>
            /// <param name="opens"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder Opens(ModuleOpens opens)
            {
                if (IsOpen || IsAutomatic)
                    throw new InvalidOperationException("Open or automatic modules cannot declare open packages.");
                if (_opensKeys != null && _opensKeys.Contains(opens.Source))
                    throw new InvalidOperationException($"Open package {opens.Source} already declared.");

                _opensKeys ??= new HashSet<string>();
                _opensKeys.Add(opens.Source);
                _opens ??= ImmutableHashSet.CreateBuilder<ModuleOpens>();
                _opens.Add(opens);
                _packages ??= ImmutableHashSet.CreateBuilder<string>();
                _packages.Add(opens.Source);

                return this;
            }

            /// <summary>
            /// Adds an open package with the given(and possibly empty) set of modifiers. The package is open to a set of target modules.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="packageName"></param>
            /// <param name="targets"></param>
            /// <returns></returns>
            public Builder Opens(ModuleOpensFlag modifiers, string packageName, ImmutableHashSet<string> targets)
            {
                return Opens(new ModuleOpens(modifiers, packageName, targets));
            }

            /// <summary>
            /// Adds an open package with the given (and possibly empty) set of modifiers. The package is open to all modules.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="packageName"></param>
            /// <returns></returns>
            public Builder Opens(ModuleOpensFlag modifiers, string packageName)
            {
                return Opens(modifiers, packageName, []);
            }

            /// <summary>
            /// Adds an open package. The package is open to a set of target modules.
            /// </summary>
            /// <param name="packageName"></param>
            /// <param name="targets"></param>
            /// <returns></returns>
            public Builder Opens(string packageName, ImmutableHashSet<string> targets)
            {
                return Opens(default, packageName, targets);
            }

            /// <summary>
            /// Adds an open package. The package is open to all modules.
            /// </summary>
            /// <param name="packageName"></param>
            /// <returns></returns>
            public Builder Opens(string packageName)
            {
                return Opens(default, packageName, []);
            }

            /// <summary>
            /// Adds a service dependence.
            /// </summary>
            /// <param name="opens"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder Uses(string service)
            {
                if (service is null)
                    throw new ArgumentNullException(nameof(service));

                if (_automatic)
                    throw new InvalidOperationException("Automatic modules cannot declare service dependencies.");
                if (_uses != null && _uses.Contains(service))
                    throw new InvalidOperationException($"Dependence upon service {service} already declared.");

                _uses ??= ImmutableHashSet.CreateBuilder<string>();
                _uses.Add(service);

                return this;
            }

            /// <summary>
            /// Provides a service with one or more implementations. The package for each <see cref="ModuleProvides"/> (or provider factory) is added to the module if not already added.
            /// </summary>
            /// <param name="opens"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder Provides(ModuleProvides provides)
            {
                if (_providesKeys != null && _providesKeys.Contains(provides.Service))
                    throw new InvalidOperationException($"Providers of service {provides.Service} already declared.");

                _providesKeys ??= new HashSet<string>();
                _providesKeys.Add(provides.Service);
                _provides ??= ImmutableHashSet.CreateBuilder<ModuleProvides>();
                _provides.Add(provides);

                _packages ??= ImmutableHashSet.CreateBuilder<string>();
                foreach (var p in provides.Providers)
                    _packages.Add(GetPackageName(p));

                return this;
            }

            /// <summary>
            /// Provides implementations of a service. The package for each provider (or provider factory) is added to the module if not already added.
            /// </summary>
            /// <param name="service"></param>
            /// <param name="providers"></param>
            /// <returns></returns>
            public Builder Provides(string service, ImmutableArray<string> providers)
            {
                return Provides(new ModuleProvides(service, providers));
            }

            /// <summary>
            /// Gets a package name for the given provider.
            /// </summary>
            /// <param name="cn"></param>
            /// <returns></returns>
            string GetPackageName(string cn)
            {
                if (cn is null)
                    throw new ArgumentNullException(nameof(cn));

                int index = cn.LastIndexOf('.');
                return (index == -1) ? "" : cn[..index];
            }

            /// <summary>
            /// Adds a package to the module.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder Package(string name)
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));

                _packages ??= ImmutableHashSet.CreateBuilder<string>();
                _packages.Add(name);
                return this;
            }

            /// <summary>
            /// Sets the module version.
            /// </summary>
            /// <param name="version"></param>
            /// <returns></returns>
            public Builder Version(ModuleVersion version)
            {
                _version = version;
                return this;
            }

            /// <summary>
            /// Sets the module version.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Builder Version(string value)
            {
                if (value is not null && ModuleVersion.TryParse(value.AsSpan(), out _version))
                    return this;

                _version = default;
                return this;
            }

            /// <summary>
            /// Sets the module main class. The package for the main class is added to the module if not already added
            /// In other words, this method is equivalent to first invoking this builder's <see cref="Packages"/>
            /// method to add the package name of the main class.
            /// </summary>
            /// <param name="mainClass"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            public Builder MainClass(string mainClass)
            {
                var packageName = GetPackageName(mainClass);
                if (string.IsNullOrEmpty(packageName))
                    throw new ArgumentException($"{mainClass}: unnamed package", nameof(mainClass));

                _packages ??= ImmutableHashSet.CreateBuilder<string>();
                _packages.Add(packageName);
                _mainClass = mainClass;

                return this;
            }

            /// <summary>
            /// Builds and returns a <see cref="ModuleDescriptor"/> from its components.
            /// </summary>
            /// <returns></returns>
            public ModuleDescriptor Build()
            {
                return new ModuleDescriptor(
                    _name,
                    _version,
                    IsAutomatic,
                    _modifiers,
                    _requires?.ToImmutable() ?? [],
                    _exports?.ToImmutable() ?? [],
                    _opens?.ToImmutable() ?? [],
                    _uses?.ToImmutable() ?? [],
                    _provides?.ToImmutable() ?? [],
                    _packages?.ToImmutable() ?? [],
                    _mainClass);
            }

        }

    }

}