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
        public readonly static string RootPath = JVM.IsUnix ? "/mnt/.ikvm/" : @"\\.ikvm\";

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
            table.AddMount(RootPath, BuildIkvmHomeRoot(context));
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

            var home = new VfsEntryDirectory(context);
            home.AddEntry("assembly", new VfsAssemblyDirectory(context));
            home.AddEntry("cacerts", new VfsCacertsFile(context));
            return home;
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

#if NETFRAMEWORK
            if (assembly.GlobalAssemblyCache)
                return true;
#endif

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
