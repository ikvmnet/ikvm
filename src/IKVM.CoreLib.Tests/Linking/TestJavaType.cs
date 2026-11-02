using System;

namespace IKVM.CoreLib.Tests.Linking
{

    /// <summary>
    /// A type of the test type system. Instances are compared by reference, so each type must be created once.
    /// </summary>
    class TestJavaType
    {

        TestJavaType? _arrayType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baseType"></param>
        /// <param name="interfaces"></param>
        /// <param name="isInterface"></param>
        /// <param name="isPrimitive"></param>
        /// <param name="isValueType"></param>
        /// <param name="isUnloadable"></param>
        public TestJavaType(string name, TestJavaType? baseType = null, TestJavaType[]? interfaces = null, bool isInterface = false, bool isPrimitive = false, bool isValueType = false, bool isUnloadable = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BaseType = baseType;
            Interfaces = interfaces ?? [];
            IsInterface = isInterface;
            IsPrimitive = isPrimitive;
            IsValueType = isValueType;
            IsUnloadable = isUnloadable;
        }

        /// <summary>
        /// Initializes a new array type.
        /// </summary>
        /// <param name="elementType"></param>
        TestJavaType(TestJavaType elementType) :
            this(elementType.Name + "[]", TestTypes.Object, [TestTypes.Cloneable, TestTypes.Serializable])
        {
            ElementType = elementType;
        }

        public string Name { get; }

        public TestJavaType? BaseType { get; }

        public TestJavaType[] Interfaces { get; }

        public bool IsInterface { get; }

        public bool IsPrimitive { get; }

        public bool IsValueType { get; }

        public bool IsUnloadable { get; }

        public TestJavaType? ElementType { get; }

        public int ArrayRank => ElementType is null ? 0 : ElementType.ArrayRank + 1;

        /// <summary>
        /// Gets the innermost element type of an array type, or the type itself.
        /// </summary>
        public TestJavaType InnermostElementType => ElementType is null ? this : ElementType.InnermostElementType;

        /// <summary>
        /// Gets the single array type whose element type is this type.
        /// </summary>
        /// <returns></returns>
        public TestJavaType MakeArrayType() => _arrayType ??= new TestJavaType(this);

        /// <summary>
        /// Gets the single array type with the given number of dimensions whose element type is this type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public TestJavaType MakeArrayType(int rank)
        {
            var type = this;
            for (int i = 0; i < rank; i++)
                type = type.MakeArrayType();

            return type;
        }

        public override string ToString() => Name;

    }

}
