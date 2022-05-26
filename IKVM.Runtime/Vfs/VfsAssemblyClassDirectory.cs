/*
  Copyright (C) 2007-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IKVM.Internal;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a virtual directory for Java classes form an assembly.
    /// </summary>
    sealed class VfsAssemblyClassDirectory : VfsDirectory
    {

        readonly Assembly assembly;
        readonly string package;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="package"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public VfsAssemblyClassDirectory(VfsContext context, Assembly assembly, string package) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            this.package = package ?? throw new ArgumentNullException(nameof(package));
        }

        /// <summary>
        /// Gets the entry corresponding to the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            if (name.EndsWith(".class", StringComparison.Ordinal))
            {
                var className = name.Substring(0, name.Length - ".class".Length);
                return package == "" ? GetClassEntry($"{className}") : GetClassEntry($"{package}.{className}");
            }
            else
                return GetPackageEntry(name);
        }

        /// <summary>
        /// Attempts to load the type information for the class name.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        TypeWrapper TryLoadType(string className)
        {
            var acl = AssemblyClassLoader.FromAssembly(assembly);

            try
            {
                return acl.LoadClassByDottedNameFast(className);
            }
            catch
            {

            }

            return null;
        }

        /// <summary>
        /// Attempts to get an entry for the specified class.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        VfsEntry GetClassEntry(string className)
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
        VfsEntry GetPackageEntry(string packageName)
        {
            var acl = AssemblyClassLoader.FromAssembly(assembly);
            if (acl == null)
                throw new InvalidOperationException("Could not locate assembly loader.");

            // search for any type that would be within the package
            foreach (var type in GetAssemblyTypes())
            {
                var name = acl.GetTypeNameAndType(type, out var isJavaType);

#if FIRST_PASS
                throw new PlatformNotSupportedException();
#else
                // annotation custom attributes are pseudo proxies and are not loadable by name (and should not exist in the file systems,
                // because proxies are, ostensibly, created on the fly)
                if (isJavaType && type.BaseType == typeof(global::ikvm.@internal.AnnotationAttributeBase) && name.Contains(".$Proxy"))
                    continue;
#endif

                // if the type is within the requested package, the package exists
                var next = package + "." + packageName;
                if (name.StartsWith(package + "."))
                    return new VfsAssemblyClassDirectory(Context, assembly, next);
            }

            return null;
        }

        /// <summary>
        /// Lists the items within the package.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override string[] List()
        {
            var lst = new HashSet<string>();

            var acl = AssemblyClassLoader.FromAssembly(assembly);
            if (acl == null)
                throw new InvalidOperationException("Could not locate assembly loader.");

            // search for any type that would be within the package
            foreach (var type in GetAssemblyTypes())
            {
                var name = acl.GetTypeNameAndType(type, out var isJavaType);

#if FIRST_PASS
                throw new PlatformNotSupportedException();
#else
                // annotation custom attributes are pseudo proxies and are not loadable by name (and should not exist in the file systems,
                // because proxies are, ostensibly, created on the fly)
                if (isJavaType && type.BaseType == typeof(global::ikvm.@internal.AnnotationAttributeBase) && name.Contains(".$Proxy"))
                    continue;
#endif

                // if the type is within the requested package, the package exists
                var next = package + ".";
                if (name.StartsWith(package + "."))
                    lst.Add(name);
            }

            return lst.ToArray();
        }

    }

}
