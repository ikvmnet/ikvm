using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    public abstract class AssemblySymbol : Symbol, ICustomAttributeProviderInternal
    {

        CustomAttributeImpl _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public AssemblySymbol(SymbolContext context) :
            base(context)
        {
            _customAttributes = new CustomAttributeImpl(context, this);
        }

        /// <summary>
        /// Gets an <see cref="AssemblyIdentity"/> for this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract AssemblyIdentity Identity { get; }

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        public string FullName => Identity.FullName;

        /// <summary>
        /// Gets a string representing the version of the common language runtime (CLR) saved in the file containing the manifest.
        /// </summary>
        public abstract string ImageRuntimeVersion { get; }

        /// <summary>
        /// Gets the full path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        public abstract string Location { get; }

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        public abstract ModuleSymbol ManifestModule { get; }

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        public abstract MethodSymbol? EntryPoint { get; }

        /// <summary>
        /// Gets a collection of the types defined in this assembly.
        /// </summary>
        public IEnumerable<TypeSymbol> DefinedTypes => GetTypes();

        /// <summary>
        /// Gets a collection of the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        public IEnumerable<TypeSymbol> ExportedTypes => GetExportedTypes();

        /// <summary>
        /// Gets a collection that contains the modules in this assembly.
        /// </summary>
        public ImmutableArray<ModuleSymbol> Modules => GetModules();

        /// <summary>
        /// Returns <c>true</c> if the symbol is missing.
        /// </summary>
        public abstract bool IsMissing { get; }

        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeSymbol> GetExportedTypes()
        {
            foreach (var type in GetTypes())
                if (IsVisibleOutsideAssembly(type))
                    yield return type;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified type is visible outside the assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsVisibleOutsideAssembly(TypeSymbol type)
        {
            var visibility = type.Attributes & TypeAttributes.VisibilityMask;
            if (visibility == TypeAttributes.Public)
                return true;

            if (visibility == TypeAttributes.NestedPublic)
                return IsVisibleOutsideAssembly(type.DeclaringType!);

            return false;
        }

        /// <summary>
        /// Gets the specified module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleSymbol? GetModule(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return GetModules().Where(i => i.Name == name).SingleOrDefaultOrThrow(() => new AmbiguousMatchException());
        }

        /// <summary>
        /// Gets all the modules that are part of this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<ModuleSymbol> GetModules();

        /// <summary>
        /// Gets the <see cref="AssemblyIdentity"/> objects for all the assemblies referenced by this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<AssemblyIdentity> GetReferencedAssemblies();

        /// <summary>
        /// Gets the Type object with the specified name in the assembly instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol? GetType(string name) => GetType(name, false);

        /// <summary>
        /// Gets the <see cref="TypeSymbol"/> object with the specified name in the assembly instance and optionally throws an exception if the type is not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public TypeSymbol? GetType(string name, bool throwOnError)
        {
            foreach (var module in GetModules())
                if (module.GetType(name, false) is TypeSymbol type)
                    return type;

            if (throwOnError)
                throw new TypeLoadException();

            return null;
        }

        /// <summary>
        /// Gets all types defined in this assembly.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeSymbol> GetTypes()
        {
            foreach (var module in GetModules())
                foreach (var type in module.GetTypes())
                    yield return type;
        }

        /// <summary>
        /// Loads the specified manifest resource from this assembly.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public abstract ManifestResourceInfo? GetManifestResourceInfo(string resourceName);

        /// <summary>
        /// Loads the specified manifest resource from this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract Stream? GetManifestResourceStream(string name);

        /// <summary>
        /// Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Stream? GetManifestResourceStream(TypeSymbol type, string name)
        {
            var sb = new ValueStringBuilder(stackalloc char[256]);

            if (type == null)
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(type));
            }
            else
            {
                var ns = type.Namespace;
                if (ns != null)
                {
                    sb.Append(ns);

                    if (name != null)
                        sb.Append(Type.Delimiter);
                }
            }

            if (name != null)
                sb.Append(name);

            return GetManifestResourceStream(sb.ToString());
        }

        /// <inheritdoc />
        internal abstract ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ImmutableArray<CustomAttribute> ICustomAttributeProviderInternal.GetDeclaredCustomAttributes() => GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ICustomAttributeProviderInternal? ICustomAttributeProviderInternal.GetInheritedCustomAttributeProvider() => null;

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(bool inherit = false) => _customAttributes.GetCustomAttributes(inherit);

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttributes(attributeType, inherit);

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttribute(attributeType, inherit);

        /// <inheritdoc />
        public bool IsDefined(TypeSymbol attributeType, bool inherit = false) => _customAttributes.IsDefined(attributeType, inherit);

        /// <inheritdoc />
        public override string ToString() => FullName ?? "";

    }

}
