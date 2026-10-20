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
using System.Diagnostics;

using IKVM.Attributes;
using IKVM.CoreLib.Runtime;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        sealed class FinishedTypeImpl : DynamicImpl
        {

            readonly Type type;
            readonly RuntimeJavaType[] innerclasses;
            readonly RuntimeJavaType declaringTypeWrapper;
            readonly Modifiers reflectiveModifiers;
            readonly MethodInfo clinitMethod;
            readonly MethodInfo finalizeMethod;
            readonly Metadata metadata;
            readonly RuntimeJavaType host;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="innerclasses"></param>
            /// <param name="declaringTypeWrapper"></param>
            /// <param name="reflectiveModifiers"></param>
            /// <param name="metadata"></param>
            /// <param name="clinitMethod"></param>
            /// <param name="finalizeMethod"></param>
            /// <param name="host"></param>
            internal FinishedTypeImpl(Type type, RuntimeJavaType[] innerclasses, RuntimeJavaType declaringTypeWrapper, Modifiers reflectiveModifiers, Metadata metadata, MethodInfo clinitMethod, MethodInfo finalizeMethod, RuntimeJavaType host)
            {
                this.type = type;
                this.innerclasses = innerclasses;
                this.declaringTypeWrapper = declaringTypeWrapper;
                this.reflectiveModifiers = reflectiveModifiers;
                this.clinitMethod = clinitMethod;
                this.finalizeMethod = finalizeMethod;
                this.metadata = metadata;
                this.host = host;
            }

            internal override RuntimeJavaType[] InnerClasses
            {
                get
                {
                    // TODO compute the innerclasses lazily (and fix JavaTypeImpl to not always compute them)
                    return innerclasses;
                }
            }

            internal override RuntimeJavaType DeclaringTypeWrapper
            {
                get
                {
                    // TODO compute lazily (and fix JavaTypeImpl to not always compute it)
                    return declaringTypeWrapper;
                }
            }

            internal override Modifiers ReflectiveModifiers => reflectiveModifiers;

            internal override Type Type => type;

            internal override void EmitRunClassConstructor(CodeEmitter ilgen)
            {
                if (clinitMethod != null)
                    ilgen.Emit(OpCodes.Call, clinitMethod);
            }

            internal override DynamicImpl Finish()
            {
                return this;
            }

            internal override MethodBase LinkMethod(RuntimeJavaMethod mw)
            {
                // we should never be called, because all methods on a finished type are already linked
                Debug.Assert(false);
                return mw.GetMethod();
            }

            internal override FieldInfo LinkField(RuntimeJavaField fw)
            {
                // we should never be called, because all fields on a finished type are already linked
                Debug.Assert(false);
                return fw.GetField();
            }

            internal override string GetGenericSignature()
            {
                return Metadata.GetGenericSignature(metadata);
            }

            internal override string[] GetEnclosingMethod()
            {
                return Metadata.GetEnclosingMethod(metadata);
            }

            internal override string GetGenericMethodSignature(int index)
            {
                return Metadata.GetGenericMethodSignature(metadata, index);
            }

            internal override string GetGenericFieldSignature(int index)
            {
                return Metadata.GetGenericFieldSignature(metadata, index);
            }

            internal override object[] GetDeclaredAnnotations()
            {
                return Metadata.GetAnnotations(metadata);
            }

            internal override object GetMethodDefaultValue(int index)
            {
                return Metadata.GetMethodDefaultValue(metadata, index);
            }

            internal override object[] GetMethodAnnotations(int index)
            {
                return Metadata.GetMethodAnnotations(metadata, index);
            }

            internal override object[][] GetParameterAnnotations(int index)
            {
                return Metadata.GetMethodParameterAnnotations(metadata, index);
            }

            internal override MethodParametersEntry[] GetMethodParameters(int index)
            {
                return Metadata.GetMethodParameters(metadata, index);
            }

            internal override object[] GetFieldAnnotations(int index)
            {
                return Metadata.GetFieldAnnotations(metadata, index);
            }

            internal override MethodInfo GetFinalizeMethod()
            {
                return finalizeMethod;
            }

            internal override object[] GetConstantPool()
            {
                return Metadata.GetConstantPool(metadata);
            }

            internal override byte[] GetRawTypeAnnotations()
            {
                return Metadata.GetRawTypeAnnotations(metadata);
            }

            internal override byte[] GetMethodRawTypeAnnotations(int index)
            {
                return Metadata.GetMethodRawTypeAnnotations(metadata, index);
            }

            internal override byte[] GetFieldRawTypeAnnotations(int index)
            {
                return Metadata.GetFieldRawTypeAnnotations(metadata, index);
            }

            internal override RuntimeJavaType Host
            {
                get { return host; }
            }

        }

    }

}