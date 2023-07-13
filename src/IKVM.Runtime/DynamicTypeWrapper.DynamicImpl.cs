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

using IKVM.Attributes;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using DynamicOrAotTypeWrapper = IKVM.Tools.Importer.AotTypeWrapper;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
#endif

namespace IKVM.Internal
{

#if IMPORTER
    abstract partial class DynamicTypeWrapper : TypeWrapper
#else
#pragma warning disable 628 // don't complain about protected members in sealed type
    sealed partial class DynamicTypeWrapper
#endif
    {
        private abstract class DynamicImpl
        {
            internal abstract Type Type { get; }
            internal abstract TypeWrapper[] InnerClasses { get; }
            internal abstract TypeWrapper DeclaringTypeWrapper { get; }
            internal abstract Modifiers ReflectiveModifiers { get; }
            internal abstract DynamicImpl Finish();
            internal abstract MethodBase LinkMethod(MethodWrapper mw);
            internal abstract FieldInfo LinkField(FieldWrapper fw);
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
            internal abstract TypeWrapper Host { get; }
        }
    }

}