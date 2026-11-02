using System;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Jvm;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.Runtime
{

    /// <summary>
    /// Implements <see cref="IJvmTypeSystem{TType, TMember, TField, TMethod}"/> over the runtime's Java types.
    /// </summary>
    readonly struct RuntimeJvmTypeSystem : IJvmTypeSystem<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>
    {

        readonly RuntimeContext _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public RuntimeJvmTypeSystem(RuntimeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

#if IMPORTER

        /// <inheritdoc />
        public bool IsImporter => true;

#else

        /// <inheritdoc />
        public bool IsImporter => false;

#endif

        /// <inheritdoc />
        public IDiagnosticHandler Diagnostics => _context.Diagnostics;

#if EXPORTER == false

        /// <inheritdoc />
        public bool AllowNonVirtualCalls => JVM.AllowNonVirtualCalls;

#else

        /// <inheritdoc />
        public bool AllowNonVirtualCalls => false;

#endif

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangObject => _context.JavaBase.TypeOfJavaLangObject;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangThrowable => _context.JavaBase.TypeOfjavaLangThrowable;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangString => _context.JavaBase.TypeOfJavaLangString;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangClass => _context.JavaBase.TypeOfJavaLangClass;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangInvokeMethodHandle => _context.JavaBase.TypeOfJavaLangInvokeMethodHandle;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfJavaLangInvokeMethodType => _context.JavaBase.TypeOfJavaLangInvokeMethodType;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfVoid => _context.PrimitiveJavaTypeFactory.VOID;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfBoolean => _context.PrimitiveJavaTypeFactory.BOOLEAN;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfByte => _context.PrimitiveJavaTypeFactory.BYTE;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfChar => _context.PrimitiveJavaTypeFactory.CHAR;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfShort => _context.PrimitiveJavaTypeFactory.SHORT;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfInt => _context.PrimitiveJavaTypeFactory.INT;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfLong => _context.PrimitiveJavaTypeFactory.LONG;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfFloat => _context.PrimitiveJavaTypeFactory.FLOAT;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfDouble => _context.PrimitiveJavaTypeFactory.DOUBLE;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfVerifierNull => _context.VerifierJavaTypeFactory.Null;

        /// <inheritdoc />
        public RuntimeJavaType TypeOfVerifierUnloadable => _context.VerifierJavaTypeFactory.Unloadable;

        /// <inheritdoc />
        public RuntimeJavaType CreateUnloadableType(string name) => new RuntimeUnloadableJavaType(_context, name);

        /// <inheritdoc />
        public string GetName(RuntimeJavaType type) => type.Name;

        /// <inheritdoc />
        public string GetSignatureName(RuntimeJavaType type) => type.SignatureName;

        /// <inheritdoc />
        public bool IsUnloadable(RuntimeJavaType type) => type.IsUnloadable;

        /// <inheritdoc />
        public bool IsInterface(RuntimeJavaType type) => type.IsInterface;

        /// <inheritdoc />
        public bool IsPrimitive(RuntimeJavaType type) => type.IsPrimitive;

        /// <inheritdoc />
        public bool IsNonPrimitiveValueType(RuntimeJavaType type) => type.IsNonPrimitiveValueType;

        /// <inheritdoc />
        public bool IsInterfaceOrInterfaceArray(RuntimeJavaType type) => type.IsInterfaceOrInterfaceArray;

        /// <inheritdoc />
        public int GetArrayRank(RuntimeJavaType type) => type.ArrayRank;

        /// <inheritdoc />
        public RuntimeJavaType GetElementType(RuntimeJavaType type) => type.ElementTypeWrapper;

        /// <inheritdoc />
        public RuntimeJavaType MakeArrayType(RuntimeJavaType type, int rank)
        {
            // the verifier asks for the primitive array types often, so avoid looking them up by name
            if (rank == 1 && type.IsPrimitive)
            {
                var factory = _context.MethodAnalyzerFactory;
                var primitives = _context.PrimitiveJavaTypeFactory;
                if (type == primitives.BYTE)
                    return factory.ByteArrayType;
                if (type == primitives.BOOLEAN)
                    return factory.BooleanArrayType;
                if (type == primitives.SHORT)
                    return factory.ShortArrayType;
                if (type == primitives.CHAR)
                    return factory.CharArrayType;
                if (type == primitives.INT)
                    return factory.IntArrayType;
                if (type == primitives.FLOAT)
                    return factory.FloatArrayType;
                if (type == primitives.DOUBLE)
                    return factory.DoubleArrayType;
                if (type == primitives.LONG)
                    return factory.LongArrayType;
            }

            return type.MakeArrayType(rank);
        }

        /// <inheritdoc />
        public ClassFileAccessFlags GetAccessFlags(RuntimeJavaType type) => (ClassFileAccessFlags)type.Modifiers;

        /// <inheritdoc />
        public RuntimeJavaType GetBaseType(RuntimeJavaType type) => type.BaseTypeWrapper;

        /// <inheritdoc />
        public RuntimeJavaType[] GetInterfaces(RuntimeJavaType type) => type.Interfaces;

        /// <inheritdoc />
        public bool IsSubTypeOf(RuntimeJavaType type, RuntimeJavaType baseType) => type.IsSubTypeOf(baseType);

        /// <inheritdoc />
        public bool ImplementsInterface(RuntimeJavaType type, RuntimeJavaType interfaceType) => type.ImplementsInterface(interfaceType);

        /// <inheritdoc />
        public bool IsAssignableTo(RuntimeJavaType type, RuntimeJavaType target) => type.IsAssignableTo(target);

        /// <inheritdoc />
        public bool HasUnloadableBaseType(RuntimeJavaType type)
        {
            for (RuntimeJavaType baseType; (baseType = type.BaseTypeWrapper) != null; type = baseType)
            {
                if (baseType.IsUnloadable)
                {
#if IMPORTER
                    type.Context.StaticCompiler.IssueMissingTypeMessage(type.TypeAsBaseType.BaseType);
#endif
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool CheckPackageAccess(RuntimeJavaType type, RuntimeJavaType target)
        {
#if IMPORTER == false && EXPORTER == false && FIRST_PASS == false
            try
            {
                type.ClassLoader.CheckPackageAccess(target, type.ClassObject.pd);
                return true;
            }
            catch (java.lang.SecurityException)
            {
                return false;
            }
#else
            return false;
#endif
        }

        /// <inheritdoc />
        public RuntimeJavaField GetField(RuntimeJavaType type, string name, string signature) => type.GetFieldWrapper(name, signature);

        /// <inheritdoc />
        public RuntimeJavaMethod GetMethod(RuntimeJavaType type, string name, string signature, bool inherit) => type.GetMethod(name, signature, inherit);

        /// <inheritdoc />
        public RuntimeJavaMethod GetInterfaceMethod(RuntimeJavaType type, string name, string signature) => type.GetInterfaceMethod(name, signature);

        /// <inheritdoc />
        public RuntimeJavaType GetFieldType(RuntimeJavaField field) => field.FieldTypeWrapper;

        /// <inheritdoc />
        public RuntimeJavaType GetReturnType(RuntimeJavaMethod method) => method.ReturnType;

        /// <inheritdoc />
        public RuntimeJavaType LoadType(RuntimeJavaType context, string name, LoadMode mode) => context.ClassLoader.LoadClass(name, mode);

        /// <inheritdoc />
        public RuntimeJavaType GetFieldTypeFromSignature(RuntimeJavaType context, string signature, LoadMode mode) => context.ClassLoader.FieldTypeWrapperFromSig(signature, mode);

        /// <inheritdoc />
        public RuntimeJavaType[] GetArgTypeListFromSignature(RuntimeJavaType context, string descriptor, LoadMode mode) => context.ClassLoader.ArgJavaTypeListFromSig(descriptor, mode);

        /// <inheritdoc />
        public RuntimeJavaType GetReturnTypeFromSignature(RuntimeJavaType context, string descriptor, LoadMode mode) => context.ClassLoader.RetTypeWrapperFromSig(descriptor, mode);

        /// <inheritdoc />
        public void EnsureLinked(RuntimeJavaField field, LoadMode mode) => field.Link(mode);

        /// <inheritdoc />
        public void EnsureLinked(RuntimeJavaMethod method, LoadMode mode) => method.Link(mode);

    }

}
