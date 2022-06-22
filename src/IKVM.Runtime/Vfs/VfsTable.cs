using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

using IKVM.Internal;
using IKVM.Runtime.Extensions;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Collection of mounted virtual file systems.
    /// </summary>
    class VfsTable
    {

        /// <summary>
        /// Gets the default mount path of the global file system.
        /// </summary>
        public readonly static string HomePath = JVM.IsUnix ? "/.virtual-ikvm-home/" : @"C:\.virtual-ikvm-home\";

        /// <summary>
        /// Gets the default mount table.
        /// </summary>
        public readonly static VfsTable Default = BuildDefaultTable(VfsRuntimeContext.Instance);

        /// <summary>
        /// Builds the default IKVM mount table.
        /// </summary>
        /// <returns></returns>
        static VfsTable BuildDefaultTable(VfsContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var table = new VfsTable(context);
            table.AddMount(HomePath, BuildIkvmHomeRoot(context));
            return table;
        }

        /// <summary>
        /// Builds the default mount directory.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        static VfsDirectory BuildIkvmHomeRoot(VfsContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // start with a new root directory, with the 'vfs.zip' file expanded within it
            var home = new VfsEntryDirectory(context);
            ExtractZipArchive(home, new ZipArchive(typeof(VfsTable).Assembly.GetManifestResourceStream("vfs.zip"), ZipArchiveMode.Read));

            // dynamically generated directory full of assembly names
            home.AddEntry("assembly", new VfsAssemblyDirectory(context));

            // generate lib directory
            var lib = (VfsEntryDirectory)home.GetOrCreateDirectory("lib");

            // generate bin directory
            var bin = (VfsEntryDirectory)home.GetOrCreateDirectory("bin");
            AddDummyLibrary(bin, "zip");
            AddDummyLibrary(bin, "awt");
            AddDummyLibrary(bin, "rmi");
            AddDummyLibrary(bin, "w2k_lsa_auth");
            AddDummyLibrary(bin, "jaas_nt");
            AddDummyLibrary(bin, "jaas_unix");
            AddDummyLibrary(bin, "net");
            AddDummyLibrary(bin, "splashscreen");
            AddDummyLibrary(bin, "osx");
            AddDummyLibrary(bin, "management");

            // fake java executables
            bin.AddEntry("java", new VfsJavaExe(context));
            bin.AddEntry("javaw", new VfsJavaExe(context));
            bin.AddEntry("java.exe", new VfsJavaExe(context));
            bin.AddEntry("javaw.exe", new VfsJavaExe(context));

            // generate security directory
            var security = (VfsEntryDirectory)lib.GetOrCreateDirectory("security");
            security.AddEntry("cacerts", new VfsCacertsFile(context));
            security.AddEntry("local_policy.jar", security.GetEntry("US_export_policy.jar")); // link policies

            return home;
        }

        /// <summary>
        /// Adds a dummy library to the given directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name"></param>
        static void AddDummyLibrary(VfsEntryDirectory directory, string name)
        {
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));

#if FIRST_PASS
            throw new PlatformNotSupportedException();
#else
            directory.AddEntry(java.lang.System.mapLibraryName(name), new VfsEmptyFile(directory.Context));
#endif
        }

        /// <summary>
        /// Expands a zip archive into the directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="archive"></param>
        static void ExtractZipArchive(VfsEntryDirectory directory, ZipArchive archive)
        {
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));

            foreach (var entry in archive.Entries)
                AddEntryAtPath(directory, entry.FullName.Replace('/', Path.DirectorySeparatorChar), new VfsZipEntry(directory.Context, entry));
        }

        /// <summary>
        /// Adds a zip entry to the given directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <param name="entry"></param>
        static void AddEntryAtPath(VfsEntryDirectory directory, string fileName, VfsZipEntry entry)
        {
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));

            var filePath = fileName.Split(Path.DirectorySeparatorChar);
            var position = (VfsDirectory)directory;
            for (var i = 0; i < filePath.Length - 1; i++)
            {
                var existing = position.GetEntry(filePath[i]);
                if (existing == null)
                {
                    if (position is VfsEntryDirectory parent)
                        existing = parent.GetOrCreateDirectory(filePath[i]);
                    else
                        throw new InvalidOperationException("Cannot add a directory to a parent that is not an entry directory.");
                }

                position = (VfsDirectory)existing;
            }

            if (position is VfsEntryDirectory target)
                target.AddEntry(filePath[filePath.Length - 1], entry);
            else
                throw new InvalidOperationException("Cannot add a file to a parent that is not an entry directory.");
        }

        readonly VfsContext context;
        readonly LinkedList<VfsMount> mounts = new LinkedList<VfsMount>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public VfsTable(VfsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the mounted virtual file systems.
        /// </summary>
        public IReadOnlyCollection<VfsMount> Mounts => mounts;

        /// <summary>
        /// Adds the a new mount to the table.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public VfsMount AddMount(string path, VfsDirectory root)
        {
            return mounts.AddFirst(new VfsMount(path, root)).Value;
        }

        /// <summary>
        /// Removes the given mount from the table.
        /// </summary>
        /// <param name="path"></param>
        public void RemoveMount(string path)
        {
            var mount = mounts.FirstOrDefault(i => i.RootPath == path);
            if (mount != null)
                mounts.Remove(mount);
        }

        /// <summary>
        /// Gets the mount for the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public VfsMount GetMount(string path) => mounts.FirstOrDefault(i => i.IsPath(path));

        /// <summary>
        /// Returns <c>true</c> if the given path is a mounted virtual path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsPath(string path) => GetMount(path) is not null;

        /// <summary>
        /// Gets the entry for the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public VfsEntry GetPath(string path) => GetMount(path) is VfsMount mount ? mount.GetPath(path.Substring(mount.RootPath.Length)) : null;

        /// <summary>
        /// Gets the names of the entries within the directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] List(string path) => GetPath(path) is VfsDirectory directory ? directory.List() : null;

        /// <summary>
        /// Opens a file at the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            // VFS is currently completely read-only
            if (mode != FileMode.Open || access != FileAccess.Read)
                throw new UnauthorizedAccessException("Virtual file system is read-only.");

            // search for the entry in the file system and open it
            return GetPath(path) switch
            {
                VfsFile file => file.Open(mode, access),
                VfsDirectory => throw new UnauthorizedAccessException($"Access to '{path}' was denied."),
                _ => throw new FileNotFoundException("File not found.", path)
            };
        }

        /// <summary>
        /// Gets the length of the file at the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public long GetLength(string path)
        {
            return GetPath(path) is VfsFile entry ? entry.Size : 0;
        }

        /// <summary>
        /// Gets the boolean attributes of the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GetBooleanAttributes(string path)
        {
            const int BA_EXISTS = 0x01;
            const int BA_REGULAR = 0x02;
            const int BA_DIRECTORY = 0x04;

            return GetPath(path) switch
            {
                VfsDirectory => BA_EXISTS | BA_DIRECTORY,
                VfsFile => BA_EXISTS | BA_REGULAR,
                _ => 0,
            };
        }

        /// <summary>
        /// Gets the path within the virtual file system where the given assembly is mapped.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public string GetAssemblyClassesPath(Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            return PathExtensions.EnsureEndingDirectorySeparator(Path.Combine(mounts.First.Value.RootPath, "assembly", GetAssemblyDirectoryName(assembly), "classes"));
        }

        /// <summary>
        /// Gets the path within the default virtual file system where the given assembly resources are mapped.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public string GetAssemblyResourcesPath(Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            return PathExtensions.EnsureEndingDirectorySeparator(Path.Combine(mounts.First.Value.RootPath, "assembly", GetAssemblyDirectoryName(assembly), "resources"));
        }

        /// <summary>
        /// Returns <c>true</c> if the assembly is loadable by name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        bool IsLoadableAssembly(Assembly assembly)
        {
            if (assembly.ReflectionOnly)
                return false;

            if (assembly.GlobalAssemblyCache)
                return true;

            if (assembly.IsDynamic || assembly.Location == "")
                return false;

#if NETFRAMEWORK
            // optimization for the common case where the assembly was loaded from the base directory
            if (Path.GetDirectoryName(assembly.Location) == Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))
                return true;
#endif

            // attempt to load the assembly from the context
            if (context.GetAssembly(assembly.GetName()) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the dirctory name of the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public string GetAssemblyDirectoryName(Assembly assembly)
        {
            // if we can't load the assembly by name, then use the MVID as the directory
            if (IsLoadableAssembly(assembly) == false)
                return assembly.ManifestModule.ModuleVersionId.ToString("N");

            var name = assembly.GetName();
            var path = new StringBuilder();

            var simpleName = name.Name;
            for (var i = 0; i < simpleName.Length; i++)
            {
                if (simpleName[i] == '_')
                    path.Append("_!");
                else
                    path.Append(simpleName[i]);
            }

            var publicKeyToken = name.GetPublicKeyToken();
            if (publicKeyToken != null && publicKeyToken.Length != 0)
            {
                path.Append("__").Append(name.Version).Append("__");
                for (int i = 0; i < publicKeyToken.Length; i++)
                    path.AppendFormat("{0:x2}", publicKeyToken[i]);
            }

            if (name.CultureInfo != null && !string.IsNullOrEmpty(name.CultureInfo.Name))
                path.Append("__").Append(name.CultureInfo.Name);

            return path.ToString();
        }

    }

}
