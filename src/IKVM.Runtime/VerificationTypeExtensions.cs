using System;

using IKVM.CoreLib.Jvm;

namespace IKVM.Runtime
{

    /// <summary>
    /// Converts between <see cref="RuntimeJavaType"/> and <see cref="VerificationType{TType}"/>.
    /// </summary>
    static class VerificationTypeExtensions
    {

        /// <summary>
        /// Gets the verification type of a value of the given type: an int for every type that is an int on the stack,
        /// null for the verifier's null type, and otherwise a reference.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal static VerificationType<RuntimeJavaType> ToVerificationType(this RuntimeJavaType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var context = type.Context;
            if (type.IsPrimitive)
            {
                if (type.IsIntOnStackPrimitive)
                    return VerificationType<RuntimeJavaType>.Int;
                if (type == context.PrimitiveJavaTypeFactory.LONG)
                    return VerificationType<RuntimeJavaType>.Long;
                if (type == context.PrimitiveJavaTypeFactory.FLOAT)
                    return VerificationType<RuntimeJavaType>.Float;
                if (type == context.PrimitiveJavaTypeFactory.DOUBLE)
                    return VerificationType<RuntimeJavaType>.Double;

                throw new ArgumentException($"No value can have the type {type.Name}.", nameof(type));
            }

            if (type == context.VerifierJavaTypeFactory.Null)
                return VerificationType<RuntimeJavaType>.Null;

            return VerificationType<RuntimeJavaType>.Reference(type);
        }

        /// <summary>
        /// Gets the runtime type of a value of the given verification type: the primitive type, the verifier's null type,
        /// or the class type of a reference without its annotations. Returns <c>null</c> for top.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The type is uninitialized, and so has no value yet.</exception>
        internal static RuntimeJavaType ToRuntimeJavaType(this VerificationType<RuntimeJavaType> type, RuntimeContext context)
        {
            return type.Kind switch
            {
                VerificationTypeKind.Top => null,
                VerificationTypeKind.Int => context.PrimitiveJavaTypeFactory.INT,
                VerificationTypeKind.Float => context.PrimitiveJavaTypeFactory.FLOAT,
                VerificationTypeKind.Long => context.PrimitiveJavaTypeFactory.LONG,
                VerificationTypeKind.Double => context.PrimitiveJavaTypeFactory.DOUBLE,
                VerificationTypeKind.Null => context.VerifierJavaTypeFactory.Null,
                VerificationTypeKind.Reference => type.Type,
                _ => throw new InvalidOperationException($"The verification type {type} has no runtime type."),
            };
        }

        /// <summary>
        /// Gets whether the type is null, or a reference to an unloadable type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsNullOrUnloadable(this VerificationType<RuntimeJavaType> type)
        {
            return type.IsNull || type.IsUnloadable();
        }

        /// <summary>
        /// Gets whether the type is a reference to an unloadable type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsUnloadable(this VerificationType<RuntimeJavaType> type)
        {
            return type.IsReference && type.Type.IsUnloadable;
        }

        /// <summary>
        /// Gets whether the type is the result of a 'new' that has not been initialized yet, or the exception of a fault
        /// block, neither of which the compiler keeps on the CLR stack.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsNotPresentOnStack(this VerificationType<RuntimeJavaType> type)
        {
            return type.IsUninitializedNew || type.IsFaultBlockException;
        }

    }

}
