using System;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Jvm;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Tests.Linking
{

    readonly struct TestJvmTypeSystem : IJvmTypeSystem<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        readonly bool _importer;

        public TestJvmTypeSystem(bool importer)
        {
            _importer = importer;
        }

        public bool IsImporter => _importer;

        public IDiagnosticHandler Diagnostics => throw new NotImplementedException();

        public bool AllowNonVirtualCalls => throw new NotImplementedException();

        public TestJavaType TypeOfJavaLangObject => TestTypes.Object;

        public TestJavaType TypeOfJavaLangThrowable => TestTypes.Throwable;

        public TestJavaType TypeOfVoid => TestTypes.PrimitiveVoid;

        public TestJavaType TypeOfInt => TestTypes.PrimitiveInt;

        public TestJavaType TypeOfLong => TestTypes.PrimitiveLong;

        public TestJavaType TypeOfFloat => TestTypes.PrimitiveFloat;

        public TestJavaType TypeOfDouble => TestTypes.PrimitiveDouble;

        public TestJavaType TypeOfVerifierNull => TestTypes.VerifierNull;

        public TestJavaType TypeOfVerifierUnloadable => TestTypes.VerifierUnloadable;

        public TestJavaType CreateUnloadableType(string name) => new TestJavaType(name, isUnloadable: true);

        public string GetName(TestJavaType type) => type.Name;

        public bool IsUnloadable(TestJavaType type) => type.IsUnloadable;

        public bool IsInterface(TestJavaType type) => type.IsInterface;

        public bool IsPrimitive(TestJavaType type) => type.IsPrimitive;

        public bool IsNonPrimitiveValueType(TestJavaType type) => type.IsValueType;

        public bool IsInterfaceOrInterfaceArray(TestJavaType type) => type.InnermostElementType.IsInterface;

        public int GetArrayRank(TestJavaType type) => type.ArrayRank;

        public TestJavaType GetElementType(TestJavaType type) => type.ElementType ?? throw new ArgumentException("Not an array type.", nameof(type));

        public TestJavaType MakeArrayType(TestJavaType type, int rank) => type.MakeArrayType(rank);

        public ClassFileAccessFlags GetAccessFlags(TestJavaType type) => throw new NotImplementedException();

        public TestJavaType? GetBaseType(TestJavaType type) => type.BaseType;

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

        public bool CheckPackageAccess(TestJavaType type, TestJavaType target) => throw new NotImplementedException();

        public TestJavaField? GetField(TestJavaType type, string name, string signature) => throw new NotImplementedException();

        public TestJavaMethod? GetMethod(TestJavaType type, string name, string signature, bool inherit) => throw new NotImplementedException();

        public TestJavaMethod? GetInterfaceMethod(TestJavaType type, string name, string signature) => throw new NotImplementedException();

        public TestJavaType LoadType(TestJavaType context, string name, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType GetFieldTypeFromSignature(TestJavaType context, string signature, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType[] GetArgTypeListFromSignature(TestJavaType context, string descriptor, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType GetReturnTypeFromSignature(TestJavaType context, string descriptor, LoadMode mode) => throw new NotImplementedException();

        public void EnsureLinked(TestJavaField field, LoadMode mode) => throw new NotImplementedException();

        public void EnsureLinked(TestJavaMethod method, LoadMode mode) => throw new NotImplementedException();

    }

}
