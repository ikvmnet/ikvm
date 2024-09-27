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

        internal IMethodSymbolBuilder DefineMethod(RuntimeByteCodeJavaType context, ITypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs)
        {
            return DefineMethod(context.ClassLoader.GetTypeWrapperFactory(), tb, name, attribs, null, false);
        }

        internal IMethodSymbolBuilder DefineMethod(RuntimeJavaTypeFactory context, ITypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs)
        {
            return DefineMethod(context, tb, name, attribs, null, false);
        }

        internal IMethodSymbolBuilder DefineMethod(RuntimeJavaTypeFactory context, ITypeSymbolBuilder tb, string name, System.Reflection.MethodAttributes attribs, ITypeSymbol firstParameter, bool mustBePublic)
        {
            // we add optional modifiers to make the signature unique
            int firstParam = firstParameter == null ? 0 : 1;
            var parameters = mw.GetParameters();
            var parameterTypes = new ITypeSymbol[parameters.Length + (mw.HasCallerID ? 1 : 0) + firstParam];
            var modopt = new ITypeSymbol[parameterTypes.Length][];

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
                parameterTypes[parameterTypes.Length - 1] = mw.DeclaringType.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
                modopt[parameterTypes.Length - 1] = [];
            }

            var returnType = mustBePublic ? mw.ReturnType.TypeAsPublicSignatureType : mw.ReturnType.TypeAsSignatureType;
            var modoptReturnType = RuntimeByteCodeJavaType.GetModOpt(context, mw.ReturnType, mustBePublic);
            return tb.DefineMethod(name, attribs, System.Reflection.CallingConventions.Standard, returnType, null, modoptReturnType, parameterTypes, null, modopt);
        }

        internal IConstructorSymbolBuilder DefineConstructor(RuntimeByteCodeJavaType context, ITypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs)
        {
            return DefineConstructor(context.ClassLoader.GetTypeWrapperFactory(), tb, attribs, false);
        }

        internal IConstructorSymbolBuilder DefineConstructor(RuntimeJavaTypeFactory context, ITypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs)
        {
            return DefineConstructor(context, tb, attribs, false);
        }

        internal IConstructorSymbolBuilder DefineConstructor(RuntimeJavaTypeFactory context, ITypeSymbolBuilder tb, System.Reflection.MethodAttributes attribs, bool mustBePublic)
        {
            // we add optional modifiers to make the signature unique
            var parameters = mw.GetParameters();
            var parameterTypes = new ITypeSymbol[parameters.Length + (mw.HasCallerID ? 1 : 0)];
            var modopt = new ITypeSymbol[parameterTypes.Length][];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = mustBePublic ? parameters[i].TypeAsPublicSignatureType : parameters[i].TypeAsSignatureType;
                modopt[i] = RuntimeByteCodeJavaType.GetModOpt(context, parameters[i], mustBePublic);
            }

            if (mw.HasCallerID)
            {
                parameterTypes[parameterTypes.Length - 1] = mw.DeclaringType.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
                modopt[parameterTypes.Length - 1] = [];
            }

            return tb.DefineConstructor(attribs, System.Reflection.CallingConventions.Standard, parameterTypes, null, modopt);
        }

    }

}
