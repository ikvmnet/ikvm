using System;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    partial class MethodHandleUtil
    {

        internal void EmitCallDelegateInvokeMethod(CodeEmitter ilgen, Type delegateType)
        {
            if (delegateType.IsGenericType)
            {
                // MONOBUG we don't look at the invoke method directly here, because Mono doesn't support GetParameters() on a builder instantiation
                Type[] typeArgs = delegateType.GetGenericArguments();
                if (IsPackedArgsContainer(typeArgs[typeArgs.Length - 1]))
                {
                    WrapArgs(ilgen, typeArgs[typeArgs.Length - 1]);
                }
                else if (typeArgs.Length > 2 && IsPackedArgsContainer(typeArgs[typeArgs.Length - 2]))
                {
                    WrapArgs(ilgen, typeArgs[typeArgs.Length - 2]);
                }
            }
            ilgen.Emit(OpCodes.Callvirt, GetDelegateInvokeMethod(delegateType));
        }

        private void WrapArgs(CodeEmitter ilgen, Type type)
        {
            Type last = type.GetGenericArguments()[MaxArity - 1];
            if (IsPackedArgsContainer(last))
            {
                WrapArgs(ilgen, last);
            }
            ilgen.Emit(OpCodes.Newobj, GetDelegateOrPackedArgsConstructor(type));
        }

        internal MethodInfo GetDelegateInvokeMethod(Type delegateType)
        {
            if (ReflectUtil.ContainsTypeBuilder(delegateType))
            {
                return TypeBuilder.GetMethod(delegateType, delegateType.GetGenericTypeDefinition().GetMethod("Invoke"));
            }
            else
            {
                return delegateType.GetMethod("Invoke");
            }
        }

        internal ConstructorInfo GetDelegateConstructor(Type delegateType)
        {
            return GetDelegateOrPackedArgsConstructor(delegateType);
        }

        private ConstructorInfo GetDelegateOrPackedArgsConstructor(Type type)
        {
            if (ReflectUtil.ContainsTypeBuilder(type))
            {
                return TypeBuilder.GetConstructor(type, type.GetGenericTypeDefinition().GetConstructors()[0]);
            }
            else
            {
                return type.GetConstructors()[0];
            }
        }

        // for delegate types used for "ldc <MethodType>" we don't want ghost arrays to be erased
        internal Type CreateDelegateTypeForLoadConstant(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            Type[] typeArgs = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                typeArgs[i] = TypeWrapperToTypeForLoadConstant(args[i]);
            }
            return CreateDelegateType(typeArgs, TypeWrapperToTypeForLoadConstant(ret));
        }

        private Type TypeWrapperToTypeForLoadConstant(RuntimeJavaType tw)
        {
            if (tw.IsGhostArray)
            {
                int dims = tw.ArrayRank;
                while (tw.IsArray)
                {
                    tw = tw.ElementTypeWrapper;
                }
                return RuntimeArrayJavaType.MakeArrayType(tw.TypeAsSignatureType, dims);
            }
            else
            {
                return tw.TypeAsSignatureType;
            }
        }

    }

}
