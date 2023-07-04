using System;
using System.Buffers;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides extension methods for working with a <see cref="ManagedType"/>.
    /// </summary>
    public static class ManagedTypeExtensions
    {

        /// <summary>
        /// Gets the field, if any, on the type with the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ManagedField? ResolveField(this ManagedType type, string name)
        {
            foreach (var f in type.Fields)
                if (f.Name == name)
                    return f;

            return null;
        }

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ManagedMethod? ResolveMethod(this ManagedType type, string name)
        {
            foreach (var m in type.Methods)
                if (m.Name == name)
                    return m;

            return null;
        }

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name and return type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ManagedMethod? ResolveMethod(this ManagedType type, string name, ReadOnlySequence<ManagedTypeSignature> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the first method, if any, on the type with the specified name, return type and parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ManagedMethod? ResolveMethod(this ManagedType type, string name, ManagedTypeSignature returnType, ReadOnlySequence<ManagedTypeSignature> parameters)
        {
            throw new NotImplementedException();
        }

    }

}
