/*
  Copyright (C) 2008-2012 Jeroen Frijters

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

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

using System.Collections.Immutable;
using IKVM.CoreLib.Reflection;



#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    static class ReflectUtil
    {

        internal static bool IsSameAssembly(TypeSymbol type1, TypeSymbol type2)
        {
            return type1.Assembly.Equals(type2.Assembly);
        }

        internal static bool IsFromAssembly(TypeSymbol type, AssemblySymbol assembly)
        {
            return type.Assembly.Equals(assembly);
        }

        internal static AssemblySymbol GetAssembly(TypeSymbol type)
        {
            return type.Assembly;
        }

        internal static bool IsDynamicAssembly(AssemblySymbol asm)
        {
#if IMPORTER || EXPORTER
            return false;
#else
            return asm.GetUnderlyingAssembly().IsDynamic;
#endif
        }

        internal static bool IsReflectionOnly(TypeSymbol type)
        {
            while (type.HasElementType)
                type = type.GetElementType();

            var asm = type.Assembly;
            if (asm != null && asm.GetUnderlyingAssembly().ReflectionOnly)
                return true;

            if (!type.IsGenericType || type.IsGenericTypeDefinition)
                return false;

            // we have a generic type instantiation, it might have ReflectionOnly type arguments
            foreach (var arg in type.GenericArguments)
                if (IsReflectionOnly(arg))
                    return true;

            return false;
        }

        internal static bool IsConstructor(MethodSymbol method)
        {
            return method.IsSpecialName && method.Name == ConstructorInfo.ConstructorName;
        }

        /// <summary>
        /// Defines a constructor.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="attribs"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        internal static MethodSymbolBuilder DefineConstructor(TypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs, ImmutableArray<TypeSymbol> parameterTypes)
        {
            return tb.DefineConstructor(attribs | System.Reflection.MethodAttributes.SpecialName | System.Reflection.MethodAttributes.RTSpecialName, parameterTypes);
        }

        /// <summary>
        /// Returns <c>true</c> if the type can be the owner of a dynamic method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool CanOwnDynamicMethod(Type type)
        {
            return type != null && !type.IsInterface && !type.HasElementType && !type.IsGenericTypeDefinition && !type.IsGenericParameter;
        }

        internal static bool MatchParameterInfos(ParameterSymbol p1, ParameterSymbol p2)
        {
            if (p1.ParameterType != p2.ParameterType)
            {
                return false;
            }
            if (!MatchTypes(p1.GetOptionalCustomModifiers(), p2.GetOptionalCustomModifiers()))
            {
                return false;
            }
            if (!MatchTypes(p1.GetRequiredCustomModifiers(), p2.GetRequiredCustomModifiers()))
            {
                return false;
            }
            return true;
        }

        private static bool MatchTypes(ImmutableArray<TypeSymbol> t1, ImmutableArray<TypeSymbol> t2)
        {
            if (t1.Length == t2.Length)
            {
                for (int i = 0; i < t1.Length; i++)
                    if (t1[i] != t2[i])
                        return false;

                return true;
            }

            return false;
        }

#if IMPORTER

        internal static TypeSymbol GetMissingType(TypeSymbol type)
        {
            while (type.HasElementType)
                type = type.GetElementType();

            if (type.IsMissing)
            {
                return type;
            }
            else if (type.ContainsMissingType)
            {
                if (type.IsGenericType)
                {
                    foreach (var arg in type.GenericArguments)
                    {
                        var t1 = GetMissingType(arg);
                        if (t1.IsMissing)
                            return t1;
                    }
                }

                throw new NotImplementedException(type.FullName);
            }
            else
            {
                return type;
            }
        }

#endif

    }

}
