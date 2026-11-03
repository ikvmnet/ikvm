using System;
using System.Collections.Generic;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Jvm;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Tests.Linking
{

    readonly struct TestJvmTypeSystem : IJvmTypeSystem<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        readonly bool _importer;
        readonly Dictionary<string, TestJavaType>? _types;

        /// <summary>
        /// Initializes a new instance that can load the types in <see cref="TestTypes"/> and the given types by name, and
        /// loads every other name as an unloadable type.
        /// </summary>
        /// <param name="importer"></param>
        /// <param name="types"></param>
        public TestJvmTypeSystem(bool importer, params TestJavaType[] types)
        {
            _importer = importer;
            _types = new Dictionary<string, TestJavaType>(StringComparer.Ordinal);
            foreach (var type in TestTypes.Classes)
                _types[type.Name] = type;
            foreach (var type in types)
                _types[type.Name] = type;
        }

        public bool IsImporter => _importer;

        public IDiagnosticHandler Diagnostics => throw new NotImplementedException();

        public bool AllowNonVirtualCalls => false;

        public TestJavaType TypeOfJavaLangObject => TestTypes.Object;

        public TestJavaType TypeOfJavaLangThrowable => TestTypes.Throwable;

        public TestJavaType TypeOfJavaLangString => TestTypes.String;

        public TestJavaType TypeOfJavaLangClass => TestTypes.Class;

        public TestJavaType TypeOfJavaLangInvokeMethodHandle => TestTypes.MethodHandle;

        public TestJavaType TypeOfJavaLangInvokeMethodType => TestTypes.MethodType;

        public TestJavaType TypeOfVoid => TestTypes.PrimitiveVoid;

        public TestJavaType TypeOfBoolean => TestTypes.PrimitiveBoolean;

        public TestJavaType TypeOfByte => TestTypes.PrimitiveByte;

        public TestJavaType TypeOfChar => TestTypes.PrimitiveChar;

        public TestJavaType TypeOfShort => TestTypes.PrimitiveShort;

        public TestJavaType TypeOfInt => TestTypes.PrimitiveInt;

        public TestJavaType TypeOfLong => TestTypes.PrimitiveLong;

        public TestJavaType TypeOfFloat => TestTypes.PrimitiveFloat;

        public TestJavaType TypeOfDouble => TestTypes.PrimitiveDouble;

        public TestJavaType TypeOfVerifierNull => TestTypes.VerifierNull;

        public TestJavaType TypeOfVerifierUnloadable => TestTypes.VerifierUnloadable;

        public TestJavaType CreateUnloadableType(string name) => new TestJavaType(name, isUnloadable: true);

        public string GetName(TestJavaType type) => type.Name;

        public string GetSignatureName(TestJavaType type) => type.SignatureName;

        public bool IsUnloadable(TestJavaType type) => type.IsUnloadable;

        public bool IsInterface(TestJavaType type) => type.IsInterface;

        public bool IsPrimitive(TestJavaType type) => type.IsPrimitive;

        public bool IsNonPrimitiveValueType(TestJavaType type) => type.IsValueType;

        public bool IsInterfaceOrInterfaceArray(TestJavaType type) => type.InnermostElementType.IsInterface;

        public int GetArrayRank(TestJavaType type) => type.ArrayRank;

        public TestJavaType GetElementType(TestJavaType type) => type.ElementType ?? throw new ArgumentException("Not an array type.", nameof(type));

        public TestJavaType MakeArrayType(TestJavaType type, int rank) => type.MakeArrayType(rank);

        public ClassFileAccessFlags GetAccessFlags(TestJavaType type) => ClassFileAccessFlags.Public | ClassFileAccessFlags.Super | (type.IsInterface ? ClassFileAccessFlags.Interface | ClassFileAccessFlags.Abstract : 0);

        public TestJavaType? GetBaseType(TestJavaType type) => type.BaseType;

        public TestJavaType[] GetInterfaces(TestJavaType type) => type.Interfaces;

        public bool IsSubTypeOf(TestJavaType type, TestJavaType baseType)
        {
            if (baseType.IsInterface)
                return type == baseType || ImplementsInterface(type, baseType);

            if (baseType == TestTypes.Object)
                return true;

            for (TestJavaType? t = type; t != null; t = t.BaseType)
                if (t == baseType)
                    return true;

            return false;
        }

        public bool ImplementsInterface(TestJavaType type, TestJavaType interfaceType)
        {
            for (TestJavaType? t = type; t != null; t = t.BaseType)
                foreach (var i in t.Interfaces)
                    if (i == interfaceType || ImplementsInterface(i, interfaceType))
                        return true;

            return false;
        }

        public bool IsAssignableTo(TestJavaType type, TestJavaType target)
        {
            if (type == target)
                return true;

            if (type.IsPrimitive || target.IsPrimitive)
                return false;

            if (type == TestTypes.VerifierNull)
                return true;

            if (target.IsInterface)
                return ImplementsInterface(type, target);

            if (type.ArrayRank > 0 && target.ArrayRank > 0)
            {
                var elem1 = type.ElementType!;
                var elem2 = target.ElementType!;
                while (elem1.ArrayRank > 0 && elem2.ArrayRank > 0)
                {
                    elem1 = elem1.ElementType!;
                    elem2 = elem2.ElementType!;
                }

                if (elem1.IsPrimitive || elem2.IsPrimitive)
                    return false;

                return !elem1.IsValueType && IsSubTypeOf(elem1, elem2);
            }

            return IsSubTypeOf(type, target);
        }

        public bool HasUnloadableBaseType(TestJavaType type)
        {
            for (var t = type.BaseType; t != null; t = t.BaseType)
                if (t.IsUnloadable)
                    return true;

            return false;
        }

        public bool CheckPackageAccess(TestJavaType type, TestJavaType target) => true;

        public TestJavaField? GetField(TestJavaType type, string name, string signature) => null;

        public TestJavaMethod? GetMethod(TestJavaType type, string name, string signature, bool inherit) => null;

        public TestJavaMethod? GetInterfaceMethod(TestJavaType type, string name, string signature) => null;

        public TestJavaType GetFieldType(TestJavaField field) => field.FieldType;

        public TestJavaType GetReturnType(TestJavaMethod method) => method.ReturnType;

        public TestJavaType LoadType(TestJavaType context, string name, LoadMode mode)
        {
            if (name.StartsWith("[", StringComparison.Ordinal))
                return GetFieldTypeFromSignature(context, name, mode);

            return GetOrAddUnloadableType(name);
        }

        /// <summary>
        /// Gets the known type with the given name, or the single unloadable type with that name.
        /// </summary>
        TestJavaType GetOrAddUnloadableType(string name)
        {
            var types = _types ?? throw new InvalidOperationException("This instance cannot load types.");
            lock (types)
            {
                if (types.TryGetValue(name, out var type) == false)
                    types[name] = type = new TestJavaType(name, isUnloadable: true);

                return type;
            }
        }

        public TestJavaType GetFieldTypeFromSignature(TestJavaType context, string signature, LoadMode mode)
        {
            int index = 0;
            var type = ParseFieldType(context, signature, ref index, mode);
            if (index != signature.Length)
                throw new ArgumentException($"Invalid field descriptor '{signature}'.", nameof(signature));

            return type;
        }

        public TestJavaType[] GetArgTypeListFromSignature(TestJavaType context, string descriptor, LoadMode mode)
        {
            var types = new List<TestJavaType>();
            int index = 1;
            while (descriptor[index] != ')')
                types.Add(ParseFieldType(context, descriptor, ref index, mode));

            return types.ToArray();
        }

        public TestJavaType GetReturnTypeFromSignature(TestJavaType context, string descriptor, LoadMode mode)
        {
            int index = descriptor.IndexOf(')') + 1;
            if (descriptor[index] == 'V')
                return TestTypes.PrimitiveVoid;

            return ParseFieldType(context, descriptor, ref index, mode);
        }

        /// <summary>
        /// Parses the field type that starts at the given index of a descriptor, and advances the index past it.
        /// </summary>
        TestJavaType ParseFieldType(TestJavaType context, string descriptor, ref int index, LoadMode mode)
        {
            switch (descriptor[index++])
            {
                case 'Z': return TestTypes.PrimitiveBoolean;
                case 'B': return TestTypes.PrimitiveByte;
                case 'C': return TestTypes.PrimitiveChar;
                case 'S': return TestTypes.PrimitiveShort;
                case 'I': return TestTypes.PrimitiveInt;
                case 'J': return TestTypes.PrimitiveLong;
                case 'F': return TestTypes.PrimitiveFloat;
                case 'D': return TestTypes.PrimitiveDouble;
                case 'L':
                    {
                        var end = descriptor.IndexOf(';', index);
                        var name = descriptor.Substring(index, end - index).Replace('/', '.');
                        index = end + 1;
                        return LoadType(context, name, mode);
                    }
                case '[':
                    {
                        var start = index - 1;
                        var elementType = ParseFieldType(context, descriptor, ref index, mode);

                        // like the runtime, an array of an unloadable type is itself unloadable
                        if (elementType.IsUnloadable)
                            return GetOrAddUnloadableType(descriptor.Substring(start, index - start).Replace('/', '.'));

                        return elementType.MakeArrayType();
                    }
                default:
                    throw new ArgumentException($"Invalid descriptor '{descriptor}'.", nameof(descriptor));
            }
        }

        public void EnsureLinked(TestJavaField field, LoadMode mode) { }

        public void EnsureLinked(TestJavaMethod method, LoadMode mode) { }

    }

}
