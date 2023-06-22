using System;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Represents a managed assembly acquired through reflection.
    /// </summary>
    internal class ReflectionAssembly : ReflectionBase, IManagedAssembly
    {

        readonly Assembly assembly;
        readonly ReadOnlyListMap<Type, ReflectionType> types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        internal ReflectionAssembly(ReflectionContext context, Assembly assembly) :
            base(context)
        {
            this.assembly = assembly;

            types = new ReadOnlyListMap<Type, ReflectionType>(assembly.GetTypes(), (t, i) => new ReflectionType(this, t));
        }

        /// <inheritdoc />
        public string Name => assembly.FullName!;

        /// <inheritdoc />
        public IReadOnlyList<byte>? PublicKey => assembly.GetName().GetPublicKeyToken()!;

        /// <inheritdoc />
        public Version Version => assembly.GetName().Version!;

        /// <inheritdoc />
        public string? Culture => assembly.GetName().CultureName;

        /// <inheritdoc />
        public IReadOnlyList<IManagedType> Types => types;

    }

}
