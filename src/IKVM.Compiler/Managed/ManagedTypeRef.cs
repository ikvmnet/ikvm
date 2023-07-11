using System;
using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed type.
    /// </summary>
    internal readonly struct ManagedTypeRef : IEquatable<ManagedTypeRef>
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

        readonly AssemblyName assemblyName;
        readonly string typeName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        public ManagedTypeRef(AssemblyName assemblyName, string typeName)
        {
            this.assemblyName = assemblyName;
            this.typeName = typeName;
        }

        /// <summary>
        /// Gets the name of the assembly of the type.
        /// </summary>
        public AssemblyName AssemblyName => assemblyName;

        /// <summary>
        /// Gets the name of the type being referenced.
        /// </summary>
        public string TypeName => typeName;

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
