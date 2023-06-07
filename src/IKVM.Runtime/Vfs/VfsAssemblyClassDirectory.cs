using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IKVM.ByteCode.Syntax;
using IKVM.Internal;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a virtual directory for Java classes form an assembly.
    /// </summary>
    internal sealed class VfsAssemblyClassDirectory : VfsDirectory
    {

        readonly Assembly assembly;
        readonly JavaPackageName package;
        readonly ConcurrentDictionary<string, VfsEntry> cache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="package"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public VfsAssemblyClassDirectory(VfsContext context, Assembly assembly, JavaPackageName package) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            this.package = package;
        }

        /// <summary>
        /// Gets the entry corresponding to the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            return cache.GetOrAdd(name, CreateEntry);
        }

        /// <summary>
        /// Creates a new entry for the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        VfsEntry CreateEntry(string name)
        {
            if (name.EndsWith(".class", StringComparison.Ordinal))
                return GetClassEntry(new JavaClassName(package, new JavaUnqualifiedClassName(name.Substring(0, name.Length - ".class".Length))));
            else
                return GetPackageEntry(new JavaPackageName(package, name));
        }

        /// <summary>
        /// Attempts to load the type information for the class name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        TypeWrapper TryLoadType(JavaClassName className)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            var acl = AssemblyClassLoader.FromAssembly(assembly);

            try
            {
                return acl.LoadClassByDottedNameFast(className);
            }
            catch
            {

            }

            return null;
#endif
        }

        /// <summary>
        /// Attempts to get an entry for the specified class.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        VfsEntry GetClassEntry(JavaClassName className)
        {
            return TryLoadType(className) is TypeWrapper tw && !tw.IsArray ? new VfsAssemblyClassFile(Context, tw) : null;
        }

        /// <summary>
        /// Gets all of the types for the current assembly.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetAssemblyTypes()
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }

        /// <summary>
        /// Attempts to get an entry for the specified package.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        VfsEntry GetPackageEntry(JavaPackageName packageName)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new PlatformNotSupportedException();
#else
            var acl = AssemblyClassLoader.FromAssembly(assembly);
            if (acl == null)
                throw new InvalidOperationException("Could not locate assembly loader.");

            // search for any type that would be within the package
            foreach (var type in GetAssemblyTypes())
            {
                // attempt to find Java type name
                var name = acl.GetTypeNameAndType(type, out var isJavaType);
                if (name is null)
                    continue;

                // annotation custom attributes are pseudo proxies and are not loadable by name (and should not exist in the file systems,
                // because proxies are, ostensibly, created on the fly)
                if (isJavaType && type.BaseType == typeof(global::ikvm.@internal.AnnotationAttributeBase) && name.Value.Value.Span.Contains(".$Proxy".AsSpan(), StringComparison.Ordinal))
                    continue;

                // found a class that is a member of the package, or whose package is a child of the package, it must exist
                if (name.Value.IsMemberOf(packageName) || name.Value.PackageName.IsChildOf(packageName))
                    return new VfsAssemblyClassDirectory(Context, assembly, packageName);
            }

            return null;
#endif
        }

        /// <summary>
        /// Lists the items within the package.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override string[] List()
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new PlatformNotSupportedException();
#else
            var lst = new HashSet<string>();

            var acl = AssemblyClassLoader.FromAssembly(assembly);
            if (acl == null)
                throw new InvalidOperationException("Could not locate assembly loader.");

            // search for any type that would be within the package
            foreach (var type in GetAssemblyTypes())
            {
                var name = acl.GetTypeNameAndType(type, out var isJavaType);
                if (name is null)
                    continue;

                // annotation custom attributes are pseudo proxies and are not loadable by name (and should not exist in the file systems,
                // because proxies are, ostensibly, created on the fly)
                if (isJavaType && type.BaseType == typeof(global::ikvm.@internal.AnnotationAttributeBase) && name.Value.Value.Span.Contains(".$Proxy".AsSpan(), StringComparison.Ordinal))
                    continue;

                // found a type that is within the package
                // its package's simple name is a directory
                if (name.Value.IsMemberOf(package))
                {
                    lst.Add(name.Value.UnqualifiedName.ToString() + ".class");
                    continue;
                }

                // found a type that is within a package which is a child of this package
                // its package's next segments are directories
                if (name.Value.PackageName.IsChildOf(package))
                {
                    var m = true;
                    var o = new ReadOnlySpanSeperatorEnumerator(package.Value.Span, '.');
                    var e = new ReadOnlySpanSeperatorEnumerator(name.Value.PackageName.Value.Span, '.');

                    // advance past the matching package name
                    while (o.MoveNext())
                        e.MoveNext();

                    // the next component of the package name is a directory
                    if (m && e.MoveNext())
                        lst.Add(e.Current.ToString());

                    continue;
                }
            }

            return lst.ToArray();
#endif
        }

    }

}
