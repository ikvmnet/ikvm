using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class MethodHandleUtil
    {

        internal void EmitCallDelegateInvokeMethod(CodeEmitter ilgen, TypeSymbol delegateType)
        {
            if (delegateType.IsGenericType)
            {
                // MONOBUG we don't look at the invoke method directly here, because Mono doesn't support GetParameters() on a builder instantiation
                var typeArgs = delegateType.GenericArguments;
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

        private void WrapArgs(CodeEmitter ilgen, TypeSymbol type)
        {
            var last = type.GenericArguments[MaxArity - 1];
            if (IsPackedArgsContainer(last))
                WrapArgs(ilgen, last);

            ilgen.Emit(OpCodes.Newobj, GetDelegateOrPackedArgsConstructor(type));
        }

        internal MethodSymbol GetDelegateInvokeMethod(TypeSymbol delegateType)
        {
            return delegateType.GetMethod("Invoke");
        }

        internal MethodSymbol GetDelegateConstructor(TypeSymbol delegateType)
        {
            return GetDelegateOrPackedArgsConstructor(delegateType);
        }

        private MethodSymbol GetDelegateOrPackedArgsConstructor(TypeSymbol type)
        {
            return type.GetConstructors().First();
        }

        // for delegate types used for "ldc <MethodType>" we don't want ghost arrays to be erased
        internal TypeSymbol CreateDelegateTypeForLoadConstant(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var typeArgs = ImmutableArray.CreateBuilder<TypeSymbol>(args.Length);
            for (int i = 0; i < args.Length; i++)
                typeArgs[i] = TypeWrapperToTypeForLoadConstant(args[i]);

            return CreateDelegateType(typeArgs.DrainToImmutable(), TypeWrapperToTypeForLoadConstant(ret));
        }

        TypeSymbol TypeWrapperToTypeForLoadConstant(RuntimeJavaType tw)
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
