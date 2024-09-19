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
                return type.AsReflection() is TypeBuilder;

            foreach (var arg in type.GetGenericArguments())
                if (ContainsTypeBuilder(arg))
                    return true;

            return type.GetGenericTypeDefinition().AsReflection() is TypeBuilder;
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

        internal static IMethodSymbolBuilder DefineTypeInitializer(ITypeSymbolBuilder typeBuilder, RuntimeClassLoader loader)
        {
            var attr = System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.RTSpecialName | System.Reflection.MethodAttributes.SpecialName | System.Reflection.MethodAttributes.Private;
            return typeBuilder.DefineMethod(ConstructorInfo.TypeConstructorName, attr, null, []);
        }

        internal static bool MatchNameAndPublicKeyToken(System.Reflection.AssemblyName name1, System.Reflection.AssemblyName name2)
        {
            return name1.Name.Equals(name2.Name, StringComparison.OrdinalIgnoreCase) && CompareKeys(name1.GetPublicKeyToken(), name2.GetPublicKeyToken());
        }

        static bool CompareKeys(byte[] b1, byte[] b2)
        {
            int len1 = b1 == null ? 0 : b1.Length;
            int len2 = b2 == null ? 0 : b2.Length;
            if (len1 != len2)
                return false;

            for (int i = 0; i < len1; i++)
                if (b1[i] != b2[i])
                    return false;

            return true;
        }

        internal static bool IsConstructor(IMethodBaseSymbol method)
        {
            return method.IsSpecialName && method.Name == ConstructorInfo.ConstructorName;
        }

        internal static IMethodSymbolBuilder DefineConstructor(ITypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs, ITypeSymbol[] parameterTypes)
        {
            return tb.DefineMethod(ConstructorInfo.ConstructorName, attribs | System.Reflection.MethodAttributes.SpecialName | System.Reflection.MethodAttributes.RTSpecialName, null, parameterTypes);
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

        internal static bool IsVector(Type type)
        {
#if NET
            return type.IsSZArray;
#else
            return type.IsArray && type.Name.EndsWith("[]");
#endif
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
