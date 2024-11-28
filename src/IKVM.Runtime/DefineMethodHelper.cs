/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Collections.Immutable;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.Runtime
{

    sealed class DefineMethodHelper
    {

        readonly RuntimeJavaMethod mw;

        internal DefineMethodHelper(RuntimeJavaMethod mw)
        {
            this.mw = mw;
        }

        internal int ParameterCount
        {
            get { return mw.GetParameters().Length + (mw.HasCallerID ? 1 : 0); }
        }

        internal MethodSymbolBuilder DefineMethod(RuntimeByteCodeJavaType context, TypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs)
        {
            return DefineMethod(context.ClassLoader.GetTypeWrapperFactory(), tb, name, attribs, null, false);
        }

        internal MethodSymbolBuilder DefineMethod(RuntimeJavaTypeFactory context, TypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs)
        {
            return DefineMethod(context, tb, name, attribs, null, false);
        }

        internal MethodSymbolBuilder DefineMethod(RuntimeJavaTypeFactory context, TypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs, TypeSymbol firstParameter, bool mustBePublic)
        {
            // we add optional modifiers to make the signature unique
            int firstParam = firstParameter == null ? 0 : 1;
            var parameters = mw.GetParameters();
            var parameterTypes = ImmutableArray.CreateBuilder<TypeSymbol>(parameters.Length + (mw.HasCallerID ? 1 : 0) + firstParam);
            parameterTypes.Count = parameters.Length + (mw.HasCallerID ? 1 : 0) + firstParam;
            var modopt = ImmutableArray.CreateBuilder<ImmutableArray<TypeSymbol>>(parameterTypes.Count);
            modopt.Count = parameterTypes.Count;

            if (firstParameter != null)
            {
                parameterTypes[0] = firstParameter;
                modopt[0] = [];
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i + firstParam] = mustBePublic ? parameters[i].TypeAsPublicSignatureType : parameters[i].TypeAsSignatureType;
                modopt[i + firstParam] = RuntimeByteCodeJavaType.GetModOpt(context, parameters[i], mustBePublic);
            }

            if (mw.HasCallerID)
            {
                parameterTypes[parameterTypes.Count - 1] = mw.DeclaringType.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
                modopt[parameterTypes.Count - 1] = [];
            }

            var returnType = mustBePublic ? mw.ReturnType.TypeAsPublicSignatureType : mw.ReturnType.TypeAsSignatureType;
            var modoptReturnType = RuntimeByteCodeJavaType.GetModOpt(context, mw.ReturnType, mustBePublic);
            return tb.DefineMethod(name, attribs, System.Reflection.CallingConventions.Standard, returnType, [], modoptReturnType, parameterTypes.DrainToImmutable(), [], modopt.DrainToImmutable());
        }

        internal MethodSymbolBuilder DefineConstructor(RuntimeByteCodeJavaType context, TypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs)
        {
            return DefineConstructor(context.ClassLoader.GetTypeWrapperFactory(), tb, attribs, false);
        }

        internal MethodSymbolBuilder DefineConstructor(RuntimeJavaTypeFactory context, TypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs)
        {
            return DefineConstructor(context, tb, attribs, false);
        }

        /// <summary>
        /// Helper method to define a constructor on a type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tb"></param>
        /// <param name="attribs"></param>
        /// <param name="mustBePublic"></param>
        /// <returns></returns>
        internal MethodSymbolBuilder DefineConstructor(RuntimeJavaTypeFactory context, TypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs, bool mustBePublic)
        {
            // we add optional modifiers to make the signature unique
            var parameters = mw.GetParameters();
            var parameterTypes = ImmutableArray.CreateBuilder<TypeSymbol>(parameters.Length + (mw.HasCallerID ? 1 : 0));
            parameterTypes.Count = parameters.Length + (mw.HasCallerID ? 1 : 0);
            var modopt = ImmutableArray.CreateBuilder<ImmutableArray<TypeSymbol>>(parameterTypes.Count);
            modopt.Count = parameterTypes.Count;

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = mustBePublic ? parameters[i].TypeAsPublicSignatureType : parameters[i].TypeAsSignatureType;
                modopt[i] = RuntimeByteCodeJavaType.GetModOpt(context, parameters[i], mustBePublic);
            }

            if (mw.HasCallerID)
            {
                parameterTypes[parameterTypes.Count - 1] = mw.DeclaringType.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
                modopt[parameterTypes.Count - 1] = [];
            }

            return tb.DefineConstructor(attribs, System.Reflection.CallingConventions.Standard, parameterTypes.DrainToImmutable(), [], modopt.DrainToImmutable());
        }

    }

}
