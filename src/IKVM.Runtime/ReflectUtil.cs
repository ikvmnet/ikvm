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
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    static class ReflectUtil
    {

        internal static bool IsSameAssembly(ITypeSymbol type1, ITypeSymbol type2)
        {
            return type1.Assembly.Equals(type2.Assembly);
        }

        internal static bool IsFromAssembly(ITypeSymbol type, IAssemblySymbol assembly)
        {
            return type.Assembly.Equals(assembly);
        }

        internal static IAssemblySymbol GetAssembly(ITypeSymbol type)
        {
            return type.Assembly;
        }

        internal static bool IsDynamicAssembly(IAssemblySymbol asm)
        {
#if IMPORTER || EXPORTER
            return false;
#else
            return asm.AsReflection().IsDynamic;
#endif
        }

        internal static bool IsReflectionOnly(ITypeSymbol type)
        {
            while (type.HasElementType)
                type = type.GetElementType();

            var asm = type.Assembly;
            if (asm != null && asm.AsReflection().ReflectionOnly)
                return true;

            if (!type.IsGenericType || type.IsGenericTypeDefinition)
                return false;

            // we have a generic type instantiation, it might have ReflectionOnly type arguments
            foreach (var arg in type.GetGenericArguments())
                if (IsReflectionOnly(arg))
                    return true;

            return false;
        }

        internal static bool ContainsTypeBuilder(ITypeSymbol type)
        {
            while (type.HasElementType)
                type = type.GetElementType();

            if (!type.IsGenericType || type.IsGenericTypeDefinition)
                return type is ITypeSymbolBuilder;

            foreach (var arg in type.GetGenericArguments())
                if (ContainsTypeBuilder(arg))
                    return true;

            return type.GetGenericTypeDefinition() is ITypeSymbolBuilder;
        }

        internal static bool IsDynamicMethod(IMethodSymbol method)
        {
            // there's no way to distinguish a baked DynamicMethod from a RuntimeMethodInfo and
            // on top of that Mono behaves completely different from .NET
            try
            {
                // on Mono 2.10 the MetadataToken property returns zero instead of throwing InvalidOperationException
                return method.MetadataToken == 0;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }

        internal static bool IsConstructor(IMethodBaseSymbol method)
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
        internal static IConstructorSymbolBuilder DefineConstructor(ITypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs, ITypeSymbol[] parameterTypes)
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

        internal static bool MatchParameterInfos(IParameterSymbol p1, IParameterSymbol p2)
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

        private static bool MatchTypes(ITypeSymbol[] t1, ITypeSymbol[] t2)
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

        internal static bool IsVector(ITypeSymbol type)
        {
            return type.IsSZArray;
        }

#if IMPORTER

        internal static ITypeSymbol GetMissingType(ITypeSymbol type)
        {
            while (type.HasElementType)
                type = type.GetElementType();

            if (type.IsMissing)
            {
                return type;
            }
            else if (type.ContainsMissing)
            {
                if (type.IsGenericType)
                {
                    foreach (var arg in type.GetGenericArguments())
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
