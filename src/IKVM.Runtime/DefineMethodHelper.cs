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
using System;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using DynamicOrAotTypeWrapper = IKVM.Tools.Importer.RuntimeImportByteCodeJavaType;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed class DefineMethodHelper
    {

        private readonly RuntimeJavaMethod mw;

        internal DefineMethodHelper(RuntimeJavaMethod mw)
        {
            this.mw = mw;
        }

        internal int ParameterCount
        {
            get { return mw.GetParameters().Length + (mw.HasCallerID ? 1 : 0); }
        }

        internal MethodBuilder DefineMethod(RuntimeByteCodeJavaType context, TypeBuilder tb, string name, MethodAttributes attribs)
        {
            return DefineMethod(context.ClassLoader.GetTypeWrapperFactory(), tb, name, attribs, null, false);
        }

        internal MethodBuilder DefineMethod(RuntimeJavaTypeFactory context, TypeBuilder tb, string name, MethodAttributes attribs)
        {
            return DefineMethod(context, tb, name, attribs, null, false);
        }

        internal MethodBuilder DefineMethod(RuntimeJavaTypeFactory context, TypeBuilder tb, string name, MethodAttributes attribs, Type firstParameter, bool mustBePublic)
        {
            // we add optional modifiers to make the signature unique
            int firstParam = firstParameter == null ? 0 : 1;
            RuntimeJavaType[] parameters = mw.GetParameters();
            Type[] parameterTypes = new Type[parameters.Length + (mw.HasCallerID ? 1 : 0) + firstParam];
            Type[][] modopt = new Type[parameterTypes.Length][];
            if (firstParameter != null)
            {
                parameterTypes[0] = firstParameter;
                modopt[0] = Type.EmptyTypes;
            }
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i + firstParam] = mustBePublic
                    ? parameters[i].TypeAsPublicSignatureType
                    : parameters[i].TypeAsSignatureType;
                modopt[i + firstParam] = RuntimeByteCodeJavaType.GetModOpt(context, parameters[i], mustBePublic);
            }
            if (mw.HasCallerID)
            {
                parameterTypes[parameterTypes.Length - 1] = mw.DeclaringType.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
            }
            Type returnType = mustBePublic
                ? mw.ReturnType.TypeAsPublicSignatureType
                : mw.ReturnType.TypeAsSignatureType;
            Type[] modoptReturnType = RuntimeByteCodeJavaType.GetModOpt(context, mw.ReturnType, mustBePublic);
            return tb.DefineMethod(name, attribs, CallingConventions.Standard, returnType, null, modoptReturnType, parameterTypes, null, modopt);
        }

        internal MethodBuilder DefineConstructor(RuntimeByteCodeJavaType context, TypeBuilder tb, MethodAttributes attribs)
        {
            return DefineConstructor(context.ClassLoader.GetTypeWrapperFactory(), tb, attribs);
        }

        internal MethodBuilder DefineConstructor(RuntimeJavaTypeFactory context, TypeBuilder tb, MethodAttributes attribs)
        {
            return DefineMethod(context, tb, ConstructorInfo.ConstructorName, attribs | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }

    }

}
