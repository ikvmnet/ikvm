using System.Reflection.Emit;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class MethodHandleUtil
    {

        internal void EmitCallDelegateInvokeMethod(CodeEmitter ilgen, ITypeSymbol delegateType)
        {
            if (delegateType.IsGenericType)
            {
                // MONOBUG we don't look at the invoke method directly here, because Mono doesn't support GetParameters() on a builder instantiation
                var typeArgs = delegateType.GetGenericArguments();
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

        private void WrapArgs(CodeEmitter ilgen, ITypeSymbol type)
        {
            var last = type.GetGenericArguments()[MaxArity - 1];
            if (IsPackedArgsContainer(last))
                WrapArgs(ilgen, last);

            ilgen.Emit(OpCodes.Newobj, GetDelegateOrPackedArgsConstructor(type));
        }

        internal IMethodSymbol GetDelegateInvokeMethod(ITypeSymbol delegateType)
        {
            return delegateType.GetMethod("Invoke");
        }

        internal IConstructorSymbol GetDelegateConstructor(ITypeSymbol delegateType)
        {
            return GetDelegateOrPackedArgsConstructor(delegateType);
        }

        private IConstructorSymbol GetDelegateOrPackedArgsConstructor(ITypeSymbol type)
        {
            return type.GetConstructors()[0];
        }

        // for delegate types used for "ldc <MethodType>" we don't want ghost arrays to be erased
        internal ITypeSymbol CreateDelegateTypeForLoadConstant(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var typeArgs = new ITypeSymbol[args.Length];
            for (int i = 0; i < args.Length; i++)
                typeArgs[i] = TypeWrapperToTypeForLoadConstant(args[i]);

            return CreateDelegateType(typeArgs, TypeWrapperToTypeForLoadConstant(ret));
        }

        ITypeSymbol TypeWrapperToTypeForLoadConstant(RuntimeJavaType tw)
        {
            if (tw.IsGhostArray)
            {
                int dims = tw.ArrayRank;
                while (tw.IsArray)
                    tw = tw.ElementTypeWrapper;

                return RuntimeArrayJavaType.MakeArrayType(tw.TypeAsSignatureType, dims);
            }
            else
            {
                return tw.TypeAsSignatureType;
            }
        }

    }

}
