using System;
using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed type.
    /// </summary>
    public readonly struct ManagedTypeRef : IEquatable<ManagedTypeRef>
    {

        /// <summary>
        /// Returns <c>true</c> if the two types are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(ManagedTypeRef x, ManagedTypeRef y) => x.Equals(y);

        /// <summary>
        /// Returns <c>false</c> if the two types are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(ManagedTypeRef x, ManagedTypeRef y) => x.Equals(y) == false;

        readonly IManagedAssemblyContext context;
        readonly AssemblyName assemblyName;
        readonly string typeName;

        readonly ManagedType? managedType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="managedType"></param>
        public ManagedTypeRef(IManagedAssemblyContext context, AssemblyName assemblyName, string typeName, ManagedType? managedType = null)
        {
            this.context = context;
            this.assemblyName = assemblyName;
            this.typeName = typeName;
            this.managedType = managedType;
        }

        /// <summary>
        /// Gets the assembly context responsible for loading this type reference.
        /// </summary>
        public IManagedAssemblyContext Context => context;

        /// <summary>
        /// Gets the name of the assembly of the type.
        /// </summary>
        public AssemblyName AssemblyName => assemblyName;

        /// <summary>
        /// Gets the name of the type being referenced.
        /// </summary>
        public string TypeName => typeName;

        /// <summary>
        /// Gets the optional managed type included along with this type reference. This may be populated if the
        /// reference was generated from the same assembly as the referenced type itself.
        /// </summary>
        public ManagedType? ManagedType => managedType;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is ManagedTypeRef r && Equals(r);

        /// <summary>
        /// Returns <c>true</c> if the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(ManagedTypeRef obj) => obj.assemblyName == assemblyName && obj.typeName == typeName;

        /// <inheritdoc />
        public override int GetHashCode() => -334489951 ^ assemblyName.GetHashCode() ^ typeName.GetHashCode();

    }

}
