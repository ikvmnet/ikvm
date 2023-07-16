using System;

using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Represents one of the primitive Java types.
    /// </summary>
    internal sealed class JavaPrimitiveType : JavaType
    {

        public static readonly JavaPrimitiveType Boolean = new JavaPrimitiveType(JavaPrimitiveTypeName.Boolean);
        public static readonly JavaPrimitiveType Byte = new JavaPrimitiveType(JavaPrimitiveTypeName.Byte);
        public static readonly JavaPrimitiveType Char = new JavaPrimitiveType(JavaPrimitiveTypeName.Char);
        public static readonly JavaPrimitiveType Short = new JavaPrimitiveType(JavaPrimitiveTypeName.Short);
        public static readonly JavaPrimitiveType Int = new JavaPrimitiveType(JavaPrimitiveTypeName.Int);
        public static readonly JavaPrimitiveType Long = new JavaPrimitiveType(JavaPrimitiveTypeName.Long);
        public static readonly JavaPrimitiveType Float = new JavaPrimitiveType(JavaPrimitiveTypeName.Float);
        public static readonly JavaPrimitiveType Double = new JavaPrimitiveType(JavaPrimitiveTypeName.Double);
        public static readonly JavaPrimitiveType Void = new JavaPrimitiveType(JavaPrimitiveTypeName.Void);

        readonly JavaPrimitiveTypeName typeName;
        readonly JavaTypeSignature signature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeName"></param>
        public JavaPrimitiveType(JavaPrimitiveTypeName typeName)
        {
            this.typeName = typeName;
            this.signature = GetSignature();
        }

        /// <summary>
        /// Calculates the signature for this primitive type.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        JavaTypeSignature GetSignature() => typeName switch
        {
            JavaPrimitiveTypeName.Boolean => JavaTypeSignature.Boolean,
            JavaPrimitiveTypeName.Byte => JavaTypeSignature.Byte,
            JavaPrimitiveTypeName.Char => JavaTypeSignature.Char,
            JavaPrimitiveTypeName.Short => JavaTypeSignature.Short,
            JavaPrimitiveTypeName.Int => JavaTypeSignature.Int,
            JavaPrimitiveTypeName.Long => JavaTypeSignature.Long,
            JavaPrimitiveTypeName.Float => JavaTypeSignature.Float,
            JavaPrimitiveTypeName.Double => JavaTypeSignature.Double,
            JavaPrimitiveTypeName.Void => JavaTypeSignature.Void,
            _ => throw new NotImplementedException(),
        };

        /// <inheritdoc />
        public override JavaTypeSignature Signature => signature;

    }

}
