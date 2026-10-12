/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Text;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents an assembly, which is a reusable, versionable, and self-describing building block of a common
    /// language runtime application.
    /// </summary>
    abstract class Assembly : ICustomAttributeProvider
    {

        readonly Universe _universe;

        /// <summary>
        /// The cached full name of the assembly. AssemblyBuilder needs access to this field to clear it when the name
        /// changes.
        /// </summary>
        protected string fullName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe">The universe that holds the assembly.</param>
        /// <exception cref="ArgumentNullException"><paramref name="universe"/> is <see langword="null"/>.</exception>
        internal Assembly(Universe universe)
        {
            _universe = universe ?? throw new ArgumentNullException(nameof(universe));
        }

        /// <summary>
        /// Gets the universe of types that holds this assembly.
        /// </summary>
        public Universe Universe => _universe;

        /// <summary>
        /// Returns the full name of the assembly, also known as the display name.
        /// </summary>
        /// <returns>The full name of the assembly, or the class name if the full name of the assembly cannot be determined.</returns>
        public sealed override string ToString() => FullName;

        /// <summary>
        /// Gets the types defined in this assembly.
        /// </summary>
        /// <returns>An array that contains all the types that are defined in this assembly.</returns>
        public abstract Type[] GetTypes();

        /// <summary>
        /// Gets an <see cref="AssemblyName"/> for this assembly.
        /// </summary>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        public abstract AssemblyName GetName();

        /// <summary>
        /// Gets a string representing the version of the common language runtime (CLR) saved in the file containing
        /// the manifest.
        /// </summary>
        public abstract string ImageRuntimeVersion { get; }

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        public abstract Module ManifestModule { get; }

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        public abstract MethodInfo EntryPoint { get; }

        /// <summary>
        /// Gets the full path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        public abstract string Location { get; }

        /// <summary>
        /// Gets the <see cref="AssemblyName"/> objects for all the assemblies referenced by this assembly.
        /// </summary>
        /// <returns>An array that contains the fully parsed display names of all the assemblies referenced by this assembly.</returns>
        public abstract AssemblyName[] GetReferencedAssemblies();

        /// <summary>
        /// Gets all the modules that are part of this assembly, specifying whether to include resource modules.
        /// </summary>
        /// <param name="getResourceModules"><see langword="true"/> to include resource modules; otherwise, <see langword="false"/>.</param>
        /// <returns>An array of modules.</returns>
        public abstract Module[] GetModules(bool getResourceModules);

        /// <summary>
        /// Gets all the loaded modules that are part of this assembly, specifying whether to include resource modules.
        /// </summary>
        /// <param name="getResourceModules"><see langword="true"/> to include resource modules; otherwise, <see langword="false"/>.</param>
        /// <returns>An array of modules.</returns>
        public abstract Module[] GetLoadedModules(bool getResourceModules);

        /// <summary>
        /// Gets the specified module in this assembly.
        /// </summary>
        /// <param name="name">The name of the module being requested.</param>
        /// <returns>The module being requested, or <see langword="null"/> if the module is not found.</returns>
        public abstract Module GetModule(string name);

        /// <summary>
        /// Returns information about how the given resource has been persisted.
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the resource.</param>
        /// <returns>An object that is populated with information about the resource's topology, or <see langword="null"/> if the resource is not found.</returns>
        public abstract ManifestResourceInfo GetManifestResourceInfo(string resourceName);

        /// <summary>
        /// Loads the specified manifest resource from this assembly.
        /// </summary>
        /// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
        /// <returns>The manifest resource, or <see langword="null"/> if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
        public abstract System.IO.Stream GetManifestResourceStream(string name);

        /// <summary>
        /// Finds a type defined in or forwarded by this assembly.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
        internal abstract Type FindType(TypeName name);

        /// <summary>
        /// Finds a type defined in or forwarded by this assembly, ignoring case.
        /// </summary>
        /// <param name="lowerCaseName">The name of the type, converted to lower case.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
        internal abstract Type FindTypeIgnoreCase(TypeName lowerCaseName);

        /// <summary>
        /// Resolves a type that another module's metadata claims this assembly contains. Unlike
        /// <see cref="FindType"/>, this is only used when the type is assumed to exist, and it returns a missing type
        /// (or throws, depending on the universe's options) when the type is not found.
        /// </summary>
        /// <param name="requester">The module that references the type.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The type, or a missing type placeholder.</returns>
        internal Type ResolveType(Module requester, TypeName typeName) =>
            FindType(typeName) ?? _universe.GetMissingTypeOrThrow(requester, ManifestModule, null, typeName);

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        public string FullName => fullName ??= GetName().FullName;

        /// <summary>
        /// Gets all the modules that are part of this assembly.
        /// </summary>
        /// <returns>An array of modules.</returns>
        public Module[] GetModules() => GetModules(true);

        /// <summary>
        /// Gets a collection that contains the modules in this assembly.
        /// </summary>
        public IEnumerable<Module> Modules => GetLoadedModules();

        /// <summary>
        /// Gets all the loaded modules that are part of this assembly.
        /// </summary>
        /// <returns>An array of modules.</returns>
        public Module[] GetLoadedModules() => GetLoadedModules(true);

        /// <summary>
        /// Gets a value indicating whether this assembly was loaded into the reflection-only context, which is always
        /// the case.
        /// </summary>
        public bool ReflectionOnly => true;

        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        /// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
        public Type[] GetExportedTypes()
        {
            var list = new List<Type>();
            foreach (var type in GetTypes())
                if (type.IsVisible)
                    list.Add(type);

            return list.ToArray();
        }

        /// <summary>
        /// Gets a collection of the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        public IEnumerable<Type> ExportedTypes => GetExportedTypes();

        /// <summary>
        /// Gets a collection of the types defined in this assembly.
        /// </summary>
        public IEnumerable<TypeInfo> DefinedTypes
        {
            get
            {
                var types = GetTypes();
                var typeInfos = new TypeInfo[types.Length];
                for (var i = 0; i < types.Length; i++)
                    typeInfos[i] = types[i].GetTypeInfo();

                return typeInfos;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> object with the specified name in the assembly instance.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <returns>An object that represents the specified class, or <see langword="null"/> if the class is not found.</returns>
        public Type GetType(string name) => GetType(name, false);

        /// <summary>
        /// Gets the <see cref="Type"/> object with the specified name in the assembly instance and optionally throws
        /// an exception if the type is not found.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw an exception if the type is not found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <returns>An object that represents the specified class.</returns>
        public Type GetType(string name, bool throwOnError) => GetType(name, throwOnError, false);

        /// <summary>
        /// Gets the <see cref="Type"/> object with the specified name in the assembly instance, with the options of
        /// ignoring the case, and of throwing an exception if the type is not found.
        /// </summary>
        /// <param name="name">The full name of the type.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw an exception if the type is not found; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore the case of the type name; otherwise, <see langword="false"/>.</param>
        /// <returns>An object that represents the specified class.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> specifies an assembly and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
        /// <exception cref="MissingAssemblyException">This assembly is missing and the type was not found.</exception>
        public Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            var parser = TypeNameParser.Parse(name, throwOnError);
            if (parser.Error)
                return null;

            if (parser.AssemblyName != null)
            {
                if (throwOnError)
                    throw new ArgumentException("Type names passed to Assembly.GetType() must not specify an assembly.");
                else
                    return null;
            }

            var typeName = TypeName.Split(TypeNameParser.Unescape(parser.FirstNamePart));
            var type = ignoreCase ? FindTypeIgnoreCase(typeName.ToLowerInvariant()) : FindType(typeName);
            if (type == null && __IsMissing)
                throw new MissingAssemblyException((MissingAssembly)this);

            return parser.Expand(type, ManifestModule, throwOnError, name, false, ignoreCase);
        }

        /// <summary>
        /// Indicates whether or not a specified attribute has been applied to the assembly.
        /// </summary>
        /// <param name="attributeType">The type of the attribute to be checked for this assembly.</param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns><see langword="true"/> if the attribute has been applied to the assembly; otherwise, <see langword="false"/>.</returns>
        public bool IsDefined(Type attributeType, bool inherit) => CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;

        /// <summary>
        /// Returns the custom attributes of the specified type, or of its derived types, applied to this assembly.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to search for, or <see langword="null"/> for all attributes.</param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>The matching custom attribute data.</returns>
        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit) =>
            CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the current assembly, expressed as
        /// <see cref="CustomAttributeData"/> objects.
        /// </summary>
        /// <returns>A generic list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the current assembly.</returns>
        public IList<CustomAttributeData> GetCustomAttributesData() => CustomAttributeData.GetCustomAttributes(this);

        /// <summary>
        /// Gets the location of the assembly as a <c>file://</c> URI built from <see cref="Location"/>.
        /// </summary>
        public string CodeBase
        {
            get
            {
                var path = Location.Replace(System.IO.Path.DirectorySeparatorChar, '/');
                if (path.StartsWith("/") == false)
                    path = "/" + path;

                return "file://" + path;
            }
        }

        /// <summary>
        /// Gets whether this assembly is a placeholder for an assembly that could not be resolved.
        /// </summary>
        public virtual bool __IsMissing => false;

        /// <summary>
        /// Gets the custom attributes applied to the assembly, optionally filtered by attribute type.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter on, or <see langword="null"/> for all.</param>
        /// <returns>The matching custom attribute data.</returns>
        internal abstract IList<CustomAttributeData> GetCustomAttributesData(Type attributeType);

    }

}
