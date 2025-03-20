using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;

using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Jar;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A <see cref="IModuleFinder"/> that locates modules on the file system by searching
    /// a sequence of directories or packaged modules. The <see cref="ModuleFinder"/> can be
    /// created to work in either the run-time or link-time phases. In both cases it
    /// locates modular JAR and exploded modules. The <see cref="ModuleFinder"/> can also
    /// optionally patch any modules that it locates with a ModulePatcher.
    /// </summary>
    internal class ModulePath : ModuleFinder
    {

        /// <summary>
        /// Returns a ModuleFinder that locates modules on the file system by searching a sequence of directories
        /// and/or packaged modules.
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public new static IModuleFinder Create(ImmutableArray<string> entries)
        {
            if (entries.IsDefault)
                throw new ArgumentNullException(nameof(entries));

            return new ModulePath(JarFile.RUNTIME_VERSION, entries);
        }

        /// <summary>
        /// Returns a ModuleFinder that locates modules on the file system by searching a sequence of directories
        /// and/or packaged modules.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static IModuleFinder Create(RuntimeVersion version, ImmutableArray<string> entries)
        {
            return new ModulePath(version, entries);
        }

        const string MODULE_INFO = "module-info.class";
        const string AUTOMATIC_MODULE_NAME = "Automatic-Module-Name";

        readonly RuntimeVersion _runtimeVersion;
        readonly ImmutableArray<string> _entries;
        readonly Dictionary<string, ModuleReference> _cachedModules = new();

        ImmutableHashSet<ModuleReference>? _all;
        int _next;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="releaseVersion"></param>
        /// <param name="entries"></param>
        public ModulePath(RuntimeVersion releaseVersion, ImmutableArray<string> entries)
        {
            if (entries.IsDefault)
                throw new ArgumentException("Uninitialized immutable array.", nameof(entries));

            _runtimeVersion = releaseVersion;
            _entries = entries;
        }

        /// <inheritdoc />
        public override ModuleReference? Find(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            var m = _cachedModules.GetValueOrDefault(name);
            if (m is not null)
                return m;

            while (HasNextEntry())
            {
                ScanNextEntry();
                m = _cachedModules.GetValueOrDefault(name);
                if (m is not null)
                    return m;
            }

            return null;
        }

        /// <inheritdoc />
        public override ImmutableHashSet<ModuleReference> FindAll()
        {
            if (_all is null)
            {
                ScanAll();
                _all = [.. _cachedModules.Values];
            }

            return _all;
        }

        /// <summary>
        /// Advances the module path through all available entries.
        /// </summary>
        void ScanAll()
        {
            while (HasNextEntry())
                ScanNextEntry();
        }

        /// <summary>
        /// Returns <c>true</c> if there are additional entries to scan.
        /// </summary>
        /// <returns></returns>
        bool HasNextEntry()
        {
            return _next < _entries.Length;
        }

        /// <summary>
        /// Scans the next entry on the module path. A no-op if all entries have already been scanned.
        /// </summary>
        void ScanNextEntry()
        {
            if (HasNextEntry())
            {
                var modules = Scan(_entries[_next]);
                _next++;

                // update cache, ignore duplicates
                foreach (var kvp in modules)
                    _cachedModules.TryAdd(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Scan the given module path entry. If the entry is a directory then it is a directory of modules or an
        /// exploded module. If the entry is a regular file then it is assumed to be a packaged module.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Dictionary<string, ModuleReference> Scan(string entry)
        {
            try
            {
                if (Directory.Exists(entry))
                {
                    var mi = Path.Combine(entry, MODULE_INFO);
                    if (File.Exists(mi) == false)
                        return ScanDirectory(entry);
                }

                var mref = ReadModule(entry);
                if (mref is not null)
                    return new Dictionary<string, ModuleReference>() { [mref.Descriptor.Name] = mref };

                throw new FindException($"Module format not recognized: {entry}");
            }
            catch (IOException e)
            {
                throw new FindException(e);
            }
        }

        /// <summary>
        /// Scans the given directory for packaged or exploded modules.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        /// <exception cref="FindException"></exception>
        Dictionary<string, ModuleReference> ScanDirectory(string dir)
        {
            var nameToReference = new Dictionary<string, ModuleReference>();

            foreach (var entry in Directory.EnumerateFileSystemEntries(dir))
            {
                var mref = ReadModule(entry);
                if (mref is not null)
                {
                    var name = mref.Descriptor.Name;
                    if (nameToReference.TryGetValue(name, out var prev))
                        throw new FindException($"Two versions of module {name} found in {dir} ({mref.Location} and {prev.Location}).");

                    nameToReference.Add(name, mref);
                }
            }

            return nameToReference;
        }

        /// <summary>
        /// Reads a packaged or exploded module, returning a <see cref="ModuleReference"/> to the module. Returns
        /// <c>null</c> if the entry is not recognized.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        /// <exception cref="FindException"></exception>
        ModuleReference? ReadModule(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            path = Path.GetFullPath(path);

            try
            {
                // entry is a directory
                if (Directory.Exists(path))
                    return ReadExplodedModule(path);

                // entry is a JAR file
                if (File.Exists(path))
                    if (Path.GetExtension(path) == ".jar")
                        return ReadJar(path);

                return null;
            }
            catch (InvalidModuleDescriptorException e)
            {
                throw new FindException($"Error reading module: {path}", e);
            }
        }

        /// <summary>
        /// Returns a <see cref="ModuleReference"/> to an exploded module on the file system or <c>null</c> if <c>module-info.class</c> not found.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        ModuleReference? ReadExplodedModule(string dir)
        {
            if (dir is null)
                throw new ArgumentNullException(nameof(dir));

            dir = Path.GetFullPath(dir);
            var moduleInfoFile = new FileInfo(Path.Combine(dir, MODULE_INFO));
            if (moduleInfoFile.Exists == false)
                return null;

            using var desc = moduleInfoFile.Open(FileMode.Open);
            using var clzz = ClassFile.Read(desc);
            return ModuleReference.CreateExplodedModule(ModuleDescriptor.Read(clzz), _runtimeVersion, dir);
        }

        /// <summary>
        /// Returns a <see cref="ModuleReference"/> to a module in a modular JAR file on the file system.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        ModuleReference ReadJar(string file)
        {
            using var jar = new JarFile(file, _runtimeVersion);

            var moduleInfoEntry = jar.GetEntry(MODULE_INFO);
            if (moduleInfoEntry is null)
            {
                try
                {
                    return ModuleReference.CreateJarModule(DeriveModuleDescriptor(jar, file), _runtimeVersion, file);
                }
                catch (Exception e)
                {
                    throw new FindException($"Unable to derive module descriptor for '{file}'.", e);
                }
            }
            else
            {
                using var desc = moduleInfoEntry.Value.Open();
                using var clzz = ClassFile.Read(desc);
                return ModuleReference.CreateJarModule(ModuleDescriptor.Read(clzz), _runtimeVersion, file);
            }
        }

        /// <summary>
        /// Treats the given JAR file as a module as follows:
        /// 
        /// <list type="number">
        ///     <item>The value of the Automatic-Module-Name attribute is the module name.</item>
        ///     <item>The version, and the module name when the  Automatic-Module-Name attribute is not present, is derived from the file name of the JAR file.</item>
        ///     <item>All packages are derived from the .class files in the JAR file.</item>
        ///     <item>The contents of any META-INF/services configuration files are mapped to "provides" declarations.</item>
        ///     <item>The Main-Class attribute in the main attributes of the JAR manifest is mapped to the module descriptor mainClass if possible.</item>
        /// </list>
        /// </summary>
        /// <param name="jar"></param>
        /// <returns></returns>
        ModuleDescriptor DeriveModuleDescriptor(JarFile jar, string file)
        {
            string? moduleName = null;

            // attempt to find automatic module name from manifest
            var man = jar.Manifest;
            if (man is not null && man.MainAttributes.TryGetValue(AUTOMATIC_MODULE_NAME, out var amn))
                moduleName = amn;

            // derive from file name
            GetModuleNameAndVersionFromFileName(file, out var fmn, out var version);
            if (moduleName is null && fmn is not null)
                moduleName = fmn;

            // at this point we should have a module name
            if (moduleName is null)
                throw new FindException($"Could not derive module name from '{file}'.");

            // build new automatic module
            var builder = ModuleDescriptor.CreateAutomaticModule(moduleName);
            if (version.IsValid)
                builder = builder.Version(version);


            // derive packages from class files
            var packages = new HashSet<string>();
            foreach (var entry in jar.GetEntries())
                if (entry.Name.EndsWith(".class"))
                    if (GetPackageName(entry) is { } pn)
                        if (packages.Add(pn))
                            builder = builder.Package(pn);

            // TODO service names

            // Main-Class attribute if it exists
            if (jar.Manifest != null && jar.Manifest.MainAttributes.TryGetValue("Main-Class", out var mainClass))
            {
                // may be stored in binary format
                mainClass = mainClass.Replace('/', '.');

                // check for valid class name
                if (Checks.IsClassName(mainClass))
                {
                    var pn = GetPackageName(mainClass);
                    if (pn is not null && packages.Contains(pn))
                        builder = builder.MainClass(mainClass);
                }
            }

            return builder.Build();
        }

        /// <summary>
        /// Extracts the package name from a JAR entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        string? GetPackageName(JarFileEntry entry)
        {
            return GetPackageName(entry.Name);
        }

        /// <summary>
        /// Extracts the package name from a JAR entry.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="InvalidModuleDescriptorException"></exception>
        string? GetPackageName(string className)
        {
            if (className.EndsWith('/'))
                throw new InvalidOperationException();

            int index = className.LastIndexOf('/');
            if (index == -1)
            {
                if (className.EndsWith(".class") && className != MODULE_INFO)
                    throw new InvalidModuleDescriptorException($"{className} found in top-level directory (unnamed package not allowed in module)");

                return null;
            }

            var pn = className.Substring(0, index).Replace('/', '.');
            return Checks.IsPackageName(pn) ? pn : null;
        }

        /// <summary>
        /// Derives the module name from the name of the file given by the path, as described by the specification.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static void GetModuleNameAndVersionFromFileName(string fileName, out string name, out ModuleVersion version)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            // The ".jar" suffix is removed.
            fileName = Path.GetFileNameWithoutExtension(fileName);
            var span = (Span<char>)stackalloc char[fileName.Length];
            fileName.AsSpan().CopyTo(span);

            // If the name matches the regular expression "-(\\d+(\\.|$))" then the module name will be derived from the subsequence preceding the hyphen of the first occurrence.
            if (Regex.Match(fileName, @"-(\d+(\.|$))") is Match m and { Success: true })
            {
                version = TryParseVersion(span.Slice(m.Index + 1));
                span = span.Slice(0, m.Index);
            }
            else
            {
                version = default;
            }

            // All non-alphanumeric characters ([^A-Za-z0-9]) in the module name are replaced with a dot ("."),
            for (var i = 0; i < span.Length; i++)
                if (char.IsLetterOrDigit(span[i]) == false)
                    span[i] = '.';

            // all repeating dots are replaced with one dot,
            fileName = span.ToString();
            while (fileName.IndexOf("..") is int i && i != -1)
                fileName = fileName.Remove(i, 1);

            // and all leading and trailing dots are removed.
            name = fileName.Trim('.');
        }

        /// <summary>
        /// Parses a Version-String.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        static ModuleVersion TryParseVersion(ReadOnlySpan<char> span)
        {
            return ModuleVersion.TryParse(span, out var v) ? v : default;
        }

    }

}
