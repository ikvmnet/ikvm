using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Tests.Linking
{

    /// <summary>
    /// The types known to <see cref="TestJvmTypeSystem"/>: a small model of the Java class hierarchy.
    /// </summary>
    static class TestTypes
    {

        public static readonly TestJavaType Object = new("java.lang.Object");

        public static readonly TestJavaType Cloneable = new("java.lang.Cloneable", isInterface: true);
        public static readonly TestJavaType Serializable = new("java.io.Serializable", isInterface: true);
        public static readonly TestJavaType Comparable = new("java.lang.Comparable", isInterface: true);
        public static readonly TestJavaType CharSequence = new("java.lang.CharSequence", isInterface: true);
        public static readonly TestJavaType Collection = new("java.util.Collection", isInterface: true);
        public static readonly TestJavaType List = new("java.util.List", interfaces: [Collection], isInterface: true);

        public static readonly TestJavaType String = new("java.lang.String", Object, [Serializable, Comparable, CharSequence]);
        public static readonly TestJavaType Number = new("java.lang.Number", Object, [Serializable]);
        public static readonly TestJavaType Integer = new("java.lang.Integer", Number, [Comparable]);
        public static readonly TestJavaType Double = new("java.lang.Double", Number, [Comparable]);
        public static readonly TestJavaType ArrayList = new("java.util.ArrayList", Object, [List]);
        public static readonly TestJavaType Class = new("java.lang.Class", Object, [Serializable]);
        public static readonly TestJavaType MethodHandle = new("java.lang.invoke.MethodHandle", Object);
        public static readonly TestJavaType MethodType = new("java.lang.invoke.MethodType", Object, [Serializable]);

        public static readonly TestJavaType Throwable = new("java.lang.Throwable", Object, [Serializable]);
        public static readonly TestJavaType Exception = new("java.lang.Exception", Throwable);
        public static readonly TestJavaType RuntimeException = new("java.lang.RuntimeException", Exception);
        public static readonly TestJavaType Error = new("java.lang.Error", Throwable);

        public static readonly TestJavaType PrimitiveVoid = new("void", isPrimitive: true) { PrimitiveSignatureName = "V" };
        public static readonly TestJavaType PrimitiveBoolean = new("boolean", isPrimitive: true) { PrimitiveSignatureName = "Z" };
        public static readonly TestJavaType PrimitiveByte = new("byte", isPrimitive: true) { PrimitiveSignatureName = "B" };
        public static readonly TestJavaType PrimitiveChar = new("char", isPrimitive: true) { PrimitiveSignatureName = "C" };
        public static readonly TestJavaType PrimitiveShort = new("short", isPrimitive: true) { PrimitiveSignatureName = "S" };
        public static readonly TestJavaType PrimitiveInt = new("int", isPrimitive: true) { PrimitiveSignatureName = "I" };
        public static readonly TestJavaType PrimitiveLong = new("long", isPrimitive: true) { PrimitiveSignatureName = "J" };
        public static readonly TestJavaType PrimitiveFloat = new("float", isPrimitive: true) { PrimitiveSignatureName = "F" };
        public static readonly TestJavaType PrimitiveDouble = new("double", isPrimitive: true) { PrimitiveSignatureName = "D" };

        /// <summary>
        /// A .NET value type that is not a Java primitive.
        /// </summary>
        public static readonly TestJavaType ValueType = new("cli.System.Guid", Object, isValueType: true);

        /// <summary>
        /// A type that could not be loaded.
        /// </summary>
        public static readonly TestJavaType Missing = new("missing.Missing", isUnloadable: true);

        /// <summary>
        /// A loadable type whose base type could not be loaded.
        /// </summary>
        public static readonly TestJavaType ExtendsMissing = new("missing.ExtendsMissing", Missing);

        public static readonly TestJavaType VerifierNull = new("<verifier>null");
        public static readonly TestJavaType VerifierUnloadable = new("<verifier>", isUnloadable: true);

        /// <summary>
        /// Gets every type above that can be loaded by name: all but the primitives.
        /// </summary>
        public static TestJavaType[] Classes { get; } = typeof(TestTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(i => i.FieldType == typeof(TestJavaType))
            .Select(i => (TestJavaType)i.GetValue(null)!)
            .Where(i => i.IsPrimitive == false)
            .ToArray();

    }

}
