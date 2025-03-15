using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A module descriptor describes a named module and defines methods to obtain each of its components.
    /// </summary>
    public readonly partial struct ModuleDescriptor : IComparable<ModuleDescriptor>
    {

        /// <summary>
        /// Reads a <see cref="ModuleDescriptor"/> from a <see cref="ClassFile"/> loaded from a 'module-info.class'.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static ModuleDescriptor Read(ClassFile clazz)
        {
            if (clazz is null)
                throw new ArgumentNullException(nameof(clazz));

            try
            {
                if (clazz.Version < 53)
                    throw new InvalidModuleDescriptorException($"Unsupported class file version '{clazz.Version}'.");

                if ((clazz.AccessFlags & AccessFlag.Module) != AccessFlag.Module)
                    throw new InvalidModuleDescriptorException("AccessFlags should be ACC_MODULE.");

                var clazzName = clazz.Constants.Get(clazz.This).Name;
                if (clazzName != "module-info")
                    throw new InvalidModuleDescriptorException("Class name should be 'module-info'.");

                if (clazz.Super.IsNotNil)
                    throw new InvalidModuleDescriptorException("Bad super class.");

                if (clazz.Fields.Count > 0)
                    throw new InvalidModuleDescriptorException("Bad fields.");

                if (clazz.Methods.Count > 0)
                    throw new InvalidModuleDescriptorException("Bad methods.");

                if (clazz.Interfaces.Count > 0)
                    throw new InvalidModuleDescriptorException("Bad interfaces.");

                var builder = ReadModuleAttribute(clazz);
                ReadModulePackagesAttribute(clazz, builder);
                ReadModuleMainClassAttribute(clazz, builder);

                return builder.Build();
            }
            catch (ByteCodeException e)
            {
                throw new InvalidModuleDescriptorException("Unable to read module descriptor.", e);
            }
        }

        /// <summary>
        /// Reads the Module attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static Builder ReadModuleAttribute(ClassFile clazz)
        {
            var attribute = clazz.Attributes.FirstOrDefault(i => i.IsNotNil && i.Name.IsNotNil && clazz.Constants.Get(i.Name).Value == AttributeName.Module);
            if (attribute.IsNil)
                throw new InvalidModuleDescriptorException($"Attribute '{AttributeName.Module}' not found.");

            return ReadModuleAttribute(clazz, (ModuleAttribute)attribute);
        }

        /// <summary>
        /// Reads the Module attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static Builder ReadModuleAttribute(ClassFile clazz, in ModuleAttribute attribute)
        {
            var moduleName = clazz.Constants.Get(attribute.Name).Name;
            if (moduleName is null || string.IsNullOrEmpty(moduleName))
                throw new InvalidModuleDescriptorException("Module name not found.");

            var modifiers = default(ModuleFlag);
            if ((attribute.Flags & ModuleFlag.Open) != 0)
                modifiers |= ModuleFlag.Open;
            if ((attribute.Flags & ModuleFlag.Synthetic) != 0)
                modifiers |= ModuleFlag.Synthetic;
            if ((attribute.Flags & ModuleFlag.Mandated) != 0)
                modifiers |= ModuleFlag.Mandated;

            var builder = CreateModule(moduleName, modifiers);

            if (attribute.Version.IsNotNil)
                builder = builder.Version(clazz.Constants.Get(attribute.Version).Value);

            ReadModuleRequires(clazz, attribute, builder);
            ReadModuleExports(clazz, attribute, builder);
            ReadModuleOpens(clazz, attribute, builder);
            ReadModuleUses(clazz, attribute, builder);
            ReadModuleProvides(clazz, attribute, builder);

            return builder;
        }

        /// <summary>
        /// Reads the ModulePackages attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="builder"></param>
        static void ReadModulePackagesAttribute(ClassFile clazz, Builder builder)
        {
            var attribute = clazz.Attributes.FirstOrDefault(i => i.IsNotNil && i.Name.IsNotNil && clazz.Constants.Get(i.Name).Value == AttributeName.ModulePackages);
            if (attribute.IsNil)
                return;

            ReadModulePackagesAttribute(clazz, (ModulePackagesAttribute)attribute, builder);
        }

        /// <summary>
        /// Reads the ModulePackages attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        static void ReadModulePackagesAttribute(ClassFile clazz, in ModulePackagesAttribute attribute, Builder builder)
        {
            foreach (var package in attribute.Packages)
            {
                var packageName = clazz.Constants.Get(package).Name;
                if (packageName is null || string.IsNullOrEmpty(packageName))
                    throw new InvalidModuleDescriptorException("Bad package name on module packages.");

                builder.Package(packageName);
            }
        }

        /// <summary>
        /// Reads the ModuleMainClass attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="builder"></param>
        static void ReadModuleMainClassAttribute(ClassFile clazz, Builder builder)
        {
            var attribute = clazz.Attributes.FirstOrDefault(i => i.IsNotNil && i.Name.IsNotNil && clazz.Constants.Get(i.Name).Value == AttributeName.ModuleMainClass);
            if (attribute.IsNil)
                return;

            ReadModuleMainClassAttribute(clazz, (ModuleMainClassAttribute)attribute, builder);
        }

        /// <summary>
        /// Reads the ModulePackages attribute.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        static void ReadModuleMainClassAttribute(ClassFile clazz, in ModuleMainClassAttribute attribute, Builder builder)
        {
            var mainClassName = clazz.Constants.Get(attribute.MainClass).Name;
            if (mainClassName is null || string.IsNullOrEmpty(mainClassName))
                throw new InvalidModuleDescriptorException("Bad main class name on module main class attribute.");

            builder.MainClass(mainClassName);
        }

        /// <summary>
        /// Reads the module requires values.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static void ReadModuleRequires(ClassFile clazz, in ModuleAttribute attribute, Builder builder)
        {
            foreach (var requires in attribute.Requires)
            {
                var moduleName = clazz.Constants.Get(requires.Module).Name;
                if (moduleName is null || string.IsNullOrEmpty(moduleName))
                    throw new InvalidModuleDescriptorException("Bad module name on requires.");

                var modifiers = default(ModuleRequiresFlag);
                if ((requires.Flag & ModuleRequiresFlag.Transitive) != 0)
                    modifiers |= ModuleRequiresFlag.Transitive;
                if ((requires.Flag & ModuleRequiresFlag.StaticPhase) != 0)
                    modifiers |= ModuleRequiresFlag.StaticPhase;
                if ((requires.Flag & ModuleRequiresFlag.Synthetic) != 0)
                    modifiers |= ModuleRequiresFlag.Synthetic;
                if ((requires.Flag & ModuleRequiresFlag.Mandated) != 0)
                    modifiers |= ModuleRequiresFlag.Mandated;

                if (moduleName == "java.base")
                {
                    if ((modifiers & ModuleRequiresFlag.Synthetic) != 0)
                        throw new InvalidModuleDescriptorException("The requires entry for java.base has ACC_SYNTHETIC set");

                    // requires transitive java.base is illegal unless:
                    // - the major version is 53 (JDK 9), or:
                    // - the classfile is a preview classfile, or:
                    // - the module is deemed to be participating in preview
                    //   (i.e. the module is a java.* module)
                    // requires static java.base is illegal unless:
                    // - the major version is 53 (JDK 9), or:
                    if (clazz.Version.Major >= 54)
                    {
                        var hasTransitive = (modifiers & ModuleRequiresFlag.Transitive) != 0;
                        var hasStatic = (modifiers & ModuleRequiresFlag.StaticPhase) != 0;
                        if ((hasTransitive && "java.se" != moduleName) || hasStatic)
                            throw new InvalidModuleDescriptorException($"The requires entry for java.base has {(hasStatic ? "ACC_STATIC_PHASE" : "ACC_TRANSITIVE")} set.");
                    }
                }

                var version = clazz.Constants.Get(requires.Version).Value;
                builder = builder.Requires(modifiers, moduleName, version);
            }
        }

        /// <summary>
        /// Reads the module exports values.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static void ReadModuleExports(ClassFile clazz, in ModuleAttribute attribute, Builder builder)
        {
            foreach (var exports in attribute.Exports)
            {
                var packageName = clazz.Constants.Get(exports.Package).Name;
                if (packageName is null || string.IsNullOrEmpty(packageName))
                    throw new InvalidModuleDescriptorException("Bad package name on module exports.");

                var modifiers = default(ModuleExportsFlag);
                if ((exports.Flags & ModuleExportsFlag.Synthetic) != 0)
                    modifiers |= ModuleExportsFlag.Synthetic;
                if ((exports.Flags & ModuleExportsFlag.Mandated) != 0)
                    modifiers |= ModuleExportsFlag.Mandated;

                builder = builder.Exports(modifiers, packageName, ToHashSet(clazz, exports.Modules));
            }
        }

        /// <summary>
        /// Reads the module opens values.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static void ReadModuleOpens(ClassFile clazz, in ModuleAttribute attribute, Builder builder)
        {
            if (builder.IsOpen)
                if (attribute.Opens.Count > 0)
                    throw new InvalidModuleDescriptorException("The opens table for an open module must be 0 length.");

            foreach (var opens in attribute.Opens)
            {
                var packageName = clazz.Constants.Get(opens.Package).Name;
                if (packageName is null || string.IsNullOrEmpty(packageName))
                    throw new InvalidModuleDescriptorException("Bad package name on module opens.");

                var modifiers = default(ModuleOpensFlag);
                if ((opens.Flags & ModuleOpensFlag.Synthetic) != 0)
                    modifiers |= ModuleOpensFlag.Synthetic;
                if ((opens.Flags & ModuleOpensFlag.Mandated) != 0)
                    modifiers |= ModuleOpensFlag.Mandated;

                builder = builder.Opens(opens.Flags, packageName, ToHashSet(clazz, opens.Modules));
            }
        }

        /// <summary>
        /// Reads the module uses values.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static void ReadModuleUses(ClassFile clazz, in ModuleAttribute attribute, Builder builder)
        {
            foreach (var uses in attribute.Uses)
            {
                var className = clazz.Constants.Get(uses).Name;
                if (className is null || string.IsNullOrEmpty(className))
                    throw new InvalidModuleDescriptorException("Bad class name on module uses.");

                builder = builder.Uses(className);
            }
        }

        /// <summary>
        /// Reads the module provides values.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attribute"></param>
        /// <param name="builder"></param>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        static void ReadModuleProvides(ClassFile clazz, in ModuleAttribute attribute, Builder builder)
        {
            foreach (var provides in attribute.Provides)
            {
                var serviceName = clazz.Constants.Get(provides.Class).Name;
                if (serviceName is null || string.IsNullOrEmpty(serviceName))
                    throw new InvalidModuleDescriptorException("Bad service name on module provides.");

                builder = builder.Provides(serviceName, ToArray(clazz, provides.With));
            }
        }

        /// <summary>
        /// Extracts the module names from the table into a <see cref="ImmutableHashSet{T}"/>.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        static ImmutableHashSet<string> ToHashSet(ClassFile clazz, in ModuleConstantHandleTable modules)
        {
            var hs = ImmutableHashSet.CreateBuilder<string>();
            foreach (var i in modules)
                if (clazz.Constants.Get(i).Name is { } name)
                    hs.Add(name);

            return hs.ToImmutable();
        }

        /// <summary>
        /// Extracts the class names from the table into a <see cref="ImmutableHashSet{T}"/>.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        static ImmutableArray<string> ToArray(ClassFile clazz, in ClassConstantHandleTable classes)
        {
            var ar = ImmutableArray.CreateBuilder<string>(classes.Count);
            foreach (var i in classes)
                if (clazz.Constants.Get(i).Name is { } name)
                    ar.Add(name);

            return ar.ToImmutable();
        }

        readonly string _name;
        readonly ModuleVersion _version;
        readonly bool _automatic;
        readonly ModuleFlag _modifiers;
        readonly ImmutableHashSet<ModuleRequires> _requires;
        readonly ImmutableHashSet<ModuleExports> _exports;
        readonly ImmutableHashSet<ModuleOpens> _opens;
        readonly ImmutableHashSet<string> _uses;
        readonly ImmutableHashSet<ModuleProvides> _provides;
        readonly ImmutableHashSet<string> _packages;
        readonly string? _mainClass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="automatic"></param>
        /// <param name="modifiers"></param>
        /// <param name="requires"></param>
        /// <param name="exports"></param>
        /// <param name="opens"></param>
        /// <param name="uses"></param>
        /// <param name="provides"></param>
        /// <param name="packages"></param>
        /// <param name="mainClass"></param>
        internal ModuleDescriptor(
            string name,
            ModuleVersion version,
            bool automatic,
            ModuleFlag modifiers,
            ImmutableHashSet<ModuleRequires> requires,
            ImmutableHashSet<ModuleExports> exports,
            ImmutableHashSet<ModuleOpens> opens,
            ImmutableHashSet<string> uses,
            ImmutableHashSet<ModuleProvides> provides,
            ImmutableHashSet<string> packages,
            string? mainClass)
        {
            _name = name;
            _version = version;
            _automatic = automatic;
            _modifiers = modifiers;
            _requires = requires;
            _exports = exports;
            _opens = opens;
            _uses = uses;
            _provides = provides;
            _packages = packages;
            _mainClass = mainClass;
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public readonly string Name => _name;

        /// <summary>
        /// Gets the version of the module.
        /// </summary>
        public readonly ModuleVersion Version => _version;

        /// <summary>
        /// Gets the modifier applied to the module.
        /// </summary>
        public readonly ModuleFlag Modifiers => _modifiers;

        /// <summary>
        /// Gets whether this is an open module or not.
        /// </summary>
        public readonly bool IsOpen => (Modifiers & ModuleFlag.Open) != 0;

        /// <summary>
        /// Gets whether this is an automatic module or not.
        /// </summary>
        public readonly bool IsAutomatic => _automatic;

        /// <summary>
        /// Returns the set of <see cref="ModuleRequires"/> objects representing the module dependences.
        /// </summary>
        public readonly ImmutableHashSet<ModuleRequires> Requires => _requires;

        /// <summary>
        /// Returns the set of <see cref="ModuleExports"/> objects representing the exported packages.
        /// </summary>
        public readonly ImmutableHashSet<ModuleExports> Exports => _exports;

        /// <summary>
        /// Returns the set of <see cref="ModuleOpens"> objects representing the open packages.
        /// </summary>
        public readonly ImmutableHashSet<ModuleOpens> Opens => _opens;

        /// <summary>
        /// Returns the set of service dependences.
        /// </summary>
        public readonly ImmutableHashSet<string> Uses => _uses;

        /// <summary>
        /// Returns the set of Provides objects representing the services that the module provides.
        /// </summary>
        public readonly ImmutableHashSet<ModuleProvides> Provides => _provides;

        /// <summary>
        /// Returns the set of packages in the module.
        /// </summary>
        /// <remarks>
        /// The set of packages includes all exported and open packages, as well as the packages of any service providers, and the package for the main class.
        /// </remarks>
        public readonly ImmutableHashSet<string> Packages => _packages;

        /// <summary>
        /// Returns the module main class.
        /// </summary>
        public readonly string? MainClass => _mainClass;

        /// <summary>
        /// Compares this module descriptor to another.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ModuleDescriptor"/> objects are compared by comparing their module names lexicographically.
        /// Where the module names are equal then the module versions are compared. When comparing the module versions
        /// then a module descriptor with a version is considered to succeed a module descriptor that does not have a
        /// version. If both versions are unparseable then the raw version strings are compared lexicographically.
        /// Where the module names are equal and the versions are equal (or not present in both), then the set of
        /// modifiers are compared. Sets of modifiers are compared by comparing a binary value computed for each set.
        /// If a modifier is present in the set then the bit at the position of its ordinal is 1 in the binary value,
        /// otherwise 0. If the two set of modifiers are also equal then the other components of the module
        /// descriptors are compared in a manner that is consistent with equals.
        /// </remarks>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly int CompareTo(ModuleDescriptor other)
        {
            int c = _name.CompareTo(other._name);
            if (c != 0)
                return c;

            c = _version.CompareTo(other._version);
            if (c != 0)
                return c;

            c = _modifiers.CompareTo(other._modifiers);
            if (c != 0)
                return c;

            c = CompareTo(_requires, other._requires);
            if (c != 0)
                return c;

            c = CompareTo(_packages, other._packages);
            if (c != 0)
                return c;

            c = CompareTo(_exports, other._exports);
            if (c != 0)
                return c;

            c = CompareTo(_opens, other._opens);
            if (c != 0)
                return c;

            c = CompareTo(_uses, other._uses);
            if (c != 0)
                return c;

            c = CompareTo(_provides, other._provides);
            if (c != 0)
                return c;

            c = Comparer.Default.Compare(_mainClass, other._mainClass);
            if (c != 0)
                return c;

            return 0;
        }

        /// <summary>
        /// Lexicographically compares the two sets. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        static int CompareTo<T>(ImmutableHashSet<T> s1, ImmutableHashSet<T> s2)
        {
            var a1 = s1.ToArray();
            var a2 = s2.ToArray();
            Array.Sort(a1);
            Array.Sort(a2);
            return LexicographicListComparer<T, IComparer<T>, T[]>.Default.Compare(a1, a2);
        }

        /// <inheritdoc />
        public readonly override bool Equals(object? obj)
        {
            return obj is ModuleDescriptor other && Equals(other);
        }

        /// <summary>
        /// Tests this module descriptor for equality with the given object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(in ModuleDescriptor other)
        {
            return
                _name.Equals(other._name) &&
                _version.Equals(other._version) &&
                _modifiers.Equals(other._modifiers) &&
                _requires.SetEquals(other._requires) &&
                _exports.SetEquals(other._exports) &&
                _opens.SetEquals(other._opens) &&
                _uses.SetEquals(other._uses) &&
                _provides.SetEquals(other._provides) &&
                _packages.SetEquals(other._packages) &&
                _mainClass == other._mainClass;
        }

        /// <inheritdoc />
        public readonly override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(_name);
            hc.Add(_version);
            hc.Add(_modifiers);
            hc.AddRange(_requires);
            hc.AddRange(_exports);
            hc.AddRange(_opens);
            hc.AddRange(_uses);
            hc.AddRange(_provides);
            hc.AddRange(_packages);
            hc.Add(_mainClass);
            return hc.ToHashCode();
        }

        /// <inheritdoc />
        public override string? ToString()
        {
            return _version.IsValid ? _name + " (@" + _version + ")" : _name;
        }

    }

}