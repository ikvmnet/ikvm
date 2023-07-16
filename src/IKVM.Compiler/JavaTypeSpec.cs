using System;

using IKVM.ByteCode.Syntax;
using IKVM.Compiler.Type;

namespace IKVM.Compiler
{

    /// <summary>
    /// Describes a Java type specification.
    /// </summary>
    internal sealed class JavaTypeSpec
    {

        public static JavaTypeSpec Void = new JavaTypeSpec();
        public static JavaTypeSpec Boolean = new JavaTypeSpec(JavaPrimitiveTypeName.Boolean);
        public static JavaTypeSpec Byte = new JavaTypeSpec(JavaPrimitiveTypeName.Byte);
        public static JavaTypeSpec Char = new JavaTypeSpec(JavaPrimitiveTypeName.Char);
        public static JavaTypeSpec Short = new JavaTypeSpec(JavaPrimitiveTypeName.Short);
        public static JavaTypeSpec Int = new JavaTypeSpec(JavaPrimitiveTypeName.Int);
        public static JavaTypeSpec Long = new JavaTypeSpec(JavaPrimitiveTypeName.Long);
        public static JavaTypeSpec Float = new JavaTypeSpec(JavaPrimitiveTypeName.Float);
        public static JavaTypeSpec Double = new JavaTypeSpec(JavaPrimitiveTypeName.Double);

        readonly JavaPrimitiveTypeName? primitiveTypeName;
        readonly JavaType? clazz;
        readonly int arrayRank = 0;

        JavaTypeSignature? signature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="primitiveTypeName"></param>
        /// <param name="arrayRank"></param>
        internal JavaTypeSpec(JavaPrimitiveTypeName primitiveTypeName, int arrayRank = 0)
        {
            this.primitiveTypeName = primitiveTypeName;
            this.arrayRank = arrayRank;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classInfo"></param>
        /// <param name="arrayRank"></param>
        internal JavaTypeSpec(JavaType classInfo, int arrayRank = 0)
        {
            this.clazz = classInfo;
            this.arrayRank = arrayRank;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        JavaTypeSpec()
        {

        }

        /// <summary>
        /// Gets the signature of the Java type specification.
        /// </summary>
        public JavaTypeSignature Signature => signature ??= GetSignature();

        /// <summary>
        /// Gets the signature of the Java type specification.
        /// </summary>
        /// <returns></returns>
        JavaTypeSignature GetSignature()
        {
            if (primitiveTypeName != null)
                return new string('[', arrayRank) + GetPrimitiveSignature();
            else if (clazz != null)
                return new string('[', arrayRank) + clazz.Name;
            else
                return JavaTypeSignature.Void;
        }

        /// <summary>
        /// Gets the signature of the primitive Java type specification.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        JavaTypeSignature GetPrimitiveSignature() => primitiveTypeName switch
        {
            JavaPrimitiveTypeName.Boolean => JavaTypeSignature.Boolean,
            JavaPrimitiveTypeName.Byte => JavaTypeSignature.Byte,
            JavaPrimitiveTypeName.Char => JavaTypeSignature.Char,
            JavaPrimitiveTypeName.Short => JavaTypeSignature.Short,
            JavaPrimitiveTypeName.Int => JavaTypeSignature.Int,
            JavaPrimitiveTypeName.Long => JavaTypeSignature.Long,
            JavaPrimitiveTypeName.Float => JavaTypeSignature.Float,
            JavaPrimitiveTypeName.Double => JavaTypeSignature.Double,
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Returns <c>true</c> if the type represents void.
        /// </summary>
        public bool IsVoid => primitiveTypeName == null && clazz == null;

        /// <summary>
        /// Returns <c>true</c> if the type specification is a primitive java type.
        /// </summary>
        public bool IsPrimitive => primitiveTypeName != null;

        /// <summary>
        /// Gets the name of the primitive type refered to by the type specification.
        /// </summary>
        public JavaPrimitiveTypeName PrimitiveTypeName => primitiveTypeName ?? throw new NotSupportedException();

        /// <summary>
        /// Gets whether or not the primitive is a long or a double.
        /// </summary>
        public bool IsWidePrimitive => IsPrimitive && (primitiveTypeName == JavaPrimitiveTypeName.Long || primitiveTypeName == JavaPrimitiveTypeName.Double);

        /// <summary>
        /// Gets whether or not this Java type is primitive and appears on the stack as an integer value.
        /// </summary>
        internal bool IsIntOnStackPrimitive => IsPrimitive && (primitiveTypeName == JavaPrimitiveTypeName.Boolean || primitiveTypeName == JavaPrimitiveTypeName.Byte || primitiveTypeName == JavaPrimitiveTypeName.Char || primitiveTypeName == JavaPrimitiveTypeName.Short || primitiveTypeName == JavaPrimitiveTypeName.Int);

        /// <summary>
        /// Gets whether the type specification represents a class.
        /// </summary>
        public bool IsClass => clazz != null;

        /// <summary>
        /// Gets the class refered to by the type specification.
        /// </summary>
        public JavaType Class => clazz ?? throw new NotSupportedException();

        /// <summary>
        /// Returns <c>true</c> if this Java class represents an array.
        /// </summary>
        public bool IsArray => arrayRank != 0;

        /// <summary>
        /// Gets the array rank of the type.
        /// </summary>
        public int ArrayRank => arrayRank;

    }

}
