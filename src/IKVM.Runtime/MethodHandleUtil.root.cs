/*
  Copyright (C) 2011-2014 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Immutable;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class MethodHandleUtil
    {

        internal const int MaxArity = 8;

        readonly RuntimeContext context;

        readonly TypeSymbol typeofMHA;
        readonly ImmutableArray<TypeSymbol> typeofMHV;
        readonly ImmutableArray<TypeSymbol> typeofMH;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MethodHandleUtil(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            typeofMHA = context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHA`8");
            typeofMHV = [
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`1"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`2"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`3"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`4"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`5"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`6"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`7"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MHV`8"),
            ];
            typeofMH = [
                null,
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`1"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`2"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`3"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`4"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`5"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`6"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`7"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`8"),
                context.Resolver.ResolveRuntimeType("IKVM.Runtime.MH`9"),
            ];
        }

        internal bool IsPackedArgsContainer(TypeSymbol type)
        {
            return type.IsGenericType && type.GenericTypeDefinition == typeofMHA;
        }

        internal TypeSymbol CreateMethodHandleDelegateType(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var typeArgs = ImmutableArray.CreateBuilder<TypeSymbol>(args.Length);
            for (int i = 0; i < args.Length; i++)
                typeArgs.Add(args[i].TypeAsSignatureType);

            return CreateDelegateType(typeArgs.DrainToImmutable(), ret.TypeAsSignatureType);
        }

        internal TypeSymbol CreateMemberWrapperDelegateType(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var typeArgs = ImmutableArray.CreateBuilder<TypeSymbol>(args.Length);
            for (int i = 0; i < args.Length; i++)
                typeArgs.Add(AsBasicType(args[i]));

            return CreateDelegateType(typeArgs.DrainToImmutable(), AsBasicType(ret));
        }

        TypeSymbol CreateDelegateType(ImmutableArray<TypeSymbol> types, TypeSymbol retType)
        {
            if (types.Length == 0 && retType == context.Types.Void)
            {
                return typeofMHV[0];
            }
            else if (types.Length > MaxArity)
            {
                int arity = types.Length;
                int remainder = (arity - 8) % 7;
                int count = (arity - 8) / 7;
                if (remainder == 0)
                {
                    remainder = 7;
                    count--;
                }

                var last = typeofMHA.MakeGenericType(SubArray(types, types.Length - 8, 8));
                for (int i = 0; i < count; i++)
                {
                    var temp = SubArray(types, types.Length - 8 - 7 * (i + 1), 8);
                    temp = temp.SetItem(7, last);
                    last = typeofMHA.MakeGenericType(temp);
                }

                types = SubArray(types, 0, remainder + 1);
                types.SetItem(remainder, last);
            }

            if (retType == context.Types.Void)
            {
                return typeofMHV[types.Length].MakeGenericType(types);
            }
            else
            {
                types = types.Add(retType);
                return typeofMH[types.Length].MakeGenericType(types);
            }
        }

        ImmutableArray<TypeSymbol> SubArray(ImmutableArray<TypeSymbol> inArray, int start, int length)
        {
            return inArray.Slice(start, length);
        }

        internal TypeSymbol AsBasicType(RuntimeJavaType tw)
        {
            if (tw == context.PrimitiveJavaTypeFactory.BOOLEAN || tw == context.PrimitiveJavaTypeFactory.BYTE || tw == context.PrimitiveJavaTypeFactory.CHAR || tw == context.PrimitiveJavaTypeFactory.SHORT || tw == context.PrimitiveJavaTypeFactory.INT)
                return context.Types.Int32;
            else if (tw == context.PrimitiveJavaTypeFactory.LONG || tw == context.PrimitiveJavaTypeFactory.FLOAT || tw == context.PrimitiveJavaTypeFactory.DOUBLE || tw == context.PrimitiveJavaTypeFactory.VOID)
                return tw.TypeAsSignatureType;
            else
                return context.Types.Object;
        }

        internal bool HasOnlyBasicTypes(RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            foreach (var tw in args)
                if (!IsBasicType(tw))
                    return false;

            return IsBasicType(ret);
        }

        bool IsBasicType(RuntimeJavaType tw)
        {
            return tw == context.PrimitiveJavaTypeFactory.INT
                || tw == context.PrimitiveJavaTypeFactory.LONG
                || tw == context.PrimitiveJavaTypeFactory.FLOAT
                || tw == context.PrimitiveJavaTypeFactory.DOUBLE
                || tw == context.PrimitiveJavaTypeFactory.VOID
                || tw == context.JavaBase.TypeOfJavaLangObject;
        }

        internal int SlotCount(RuntimeJavaType[] parameters)
        {
            int count = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                count += parameters[i].IsWidePrimitive ? 2 : 1;
            }
            return count;
        }

    }

}
