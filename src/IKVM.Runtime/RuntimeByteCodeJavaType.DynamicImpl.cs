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

using IKVM.CoreLib.Runtime;
using IKVM.Attributes;

#if IMPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        private abstract class DynamicImpl
        {

            internal abstract Type Type { get; }
            internal abstract RuntimeJavaType[] InnerClasses { get; }
            internal abstract RuntimeJavaType DeclaringTypeWrapper { get; }
            internal abstract Modifiers ReflectiveModifiers { get; }
            internal abstract DynamicImpl Finish();
            internal abstract MethodBase LinkMethod(RuntimeJavaMethod mw);
            internal abstract FieldInfo LinkField(RuntimeJavaField fw);
            internal abstract void EmitRunClassConstructor(CodeEmitter ilgen);
            internal abstract string GetGenericSignature();
            internal abstract string[] GetEnclosingMethod();
            internal abstract string GetGenericMethodSignature(int index);
            internal abstract string GetGenericFieldSignature(int index);
            internal abstract object[] GetDeclaredAnnotations();
            internal abstract object GetMethodDefaultValue(int index);
            internal abstract object[] GetMethodAnnotations(int index);
            internal abstract object[][] GetParameterAnnotations(int index);
            internal abstract MethodParametersEntry[] GetMethodParameters(int index);
            internal abstract object[] GetFieldAnnotations(int index);
            internal abstract MethodInfo GetFinalizeMethod();
            internal abstract object[] GetConstantPool();
            internal abstract byte[] GetRawTypeAnnotations();
            internal abstract byte[] GetMethodRawTypeAnnotations(int index);
            internal abstract byte[] GetFieldRawTypeAnnotations(int index);
            internal abstract RuntimeJavaType Host { get; }

        }

    }

}