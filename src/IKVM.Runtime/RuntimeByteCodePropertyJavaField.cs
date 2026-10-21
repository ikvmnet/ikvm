/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

#if EXPORTER == false

    /// <summary>
    /// Represents a .NET property defined in Java with the <see cref="ikvm.lang.Property"/> annotation.
    /// </summary>
    sealed class RuntimeByteCodePropertyJavaField : RuntimeJavaField
    {

        readonly RuntimeJavaMethod getter;
        readonly RuntimeJavaMethod setter;
        PropertyBuilder pb;

        RuntimeJavaMethod GetMethod(string name, string sig, bool isstatic)
        {
            if (name != null)
            {
                var mw = DeclaringType.GetMethod(name, sig, false);
                if (mw != null && mw.IsStatic == isstatic)
                {
                    mw.IsPropertyAccessor = true;
                    return mw;
                }

                DeclaringType.ClassLoader.Diagnostics.GenericCompilerError($"Property '{Name}' accessor '{name}' not found in class '{DeclaringType.Name}'");
            }

            return null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="fld"></param>
        internal RuntimeByteCodePropertyJavaField(RuntimeJavaType declaringType, Field fld) :
            base(declaringType, null, fld.Name, fld.Signature, new ExModifiers((Modifiers)fld.AccessFlags, fld.IsInternal), null)
        {
            getter = GetMethod(fld.PropertyGetter, "()" + fld.Signature, fld.IsStatic);
            setter = GetMethod(fld.PropertySetter, "(" + fld.Signature + ")V", fld.IsStatic);
        }

#if !IMPORTER && !FIRST_PASS

        internal override void ResolveField()
        {
            getter?.ResolveMethod();
            setter?.ResolveMethod();
        }

#endif

        internal PropertyBuilder GetPropertyBuilder()
        {
            AssertLinked();
            return pb;
        }

        internal void DoLink(TypeBuilder tb)
        {
            if (getter != null)
            {
                getter.Link();
            }
            if (setter != null)
            {
                setter.Link();
            }
            pb = tb.DefineProperty(this.Name, PropertyAttributes.None, this.FieldTypeWrapper.TypeAsSignatureType, Type.EmptyTypes);
            if (getter != null)
            {
                pb.SetGetMethod((MethodBuilder)getter.GetMethod());
            }
            if (setter != null)
            {
                pb.SetSetMethod((MethodBuilder)setter.GetMethod());
            }
#if IMPORTER
            DeclaringType.Context.AttributeHelper.SetModifiers(pb, this.Modifiers, this.IsInternal);
#endif
        }

#if EMITTERS
        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            if (getter == null)
            {
                EmitThrowNoSuchMethodErrorForGetter(ilgen, this.FieldTypeWrapper, this);
            }
            else if (getter.IsStatic)
            {
                getter.EmitCall(ilgen);
            }
            else
            {
                getter.EmitCallvirt(ilgen);
            }
        }

        internal static void EmitThrowNoSuchMethodErrorForGetter(CodeEmitter ilgen, RuntimeJavaType type, RuntimeJavaMember member)
        {
#if IMPORTER
            type.ClassLoader.Diagnostics.EmittedNoSuchMethodError("<unknown>", member.DeclaringType.Name + "." + member.Name + member.Signature);
#endif
            // HACK the branch around the throw is to keep the verifier happy
            CodeEmitterLabel label = ilgen.DefineLabel();
            ilgen.Emit(OpCodes.Ldc_I4_0);
            ilgen.EmitBrtrue(label);
            ilgen.EmitThrow("java.lang.NoSuchMethodError");
            ilgen.MarkLabel(label);
            if (!member.IsStatic)
            {
                ilgen.Emit(OpCodes.Pop);
            }
            ilgen.Emit(OpCodes.Ldloc, ilgen.DeclareLocal(type.TypeAsLocalOrStackType));
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            if (setter == null)
            {
                if (this.IsFinal)
                {
                    ilgen.Emit(OpCodes.Pop);
                    if (!this.IsStatic)
                    {
                        ilgen.Emit(OpCodes.Pop);
                    }
                }
                else
                {
                    EmitThrowNoSuchMethodErrorForSetter(ilgen, this);
                }
            }
            else if (setter.IsStatic)
            {
                setter.EmitCall(ilgen);
            }
            else
            {
                setter.EmitCallvirt(ilgen);
            }
        }

        internal static void EmitThrowNoSuchMethodErrorForSetter(CodeEmitter ilgen, RuntimeJavaMember member)
        {
#if IMPORTER
            member.DeclaringType.ClassLoader.Diagnostics.EmittedNoSuchMethodError("<unknown>", member.DeclaringType.Name + "." + member.Name + member.Signature);
#endif
            // HACK the branch around the throw is to keep the verifier happy
            CodeEmitterLabel label = ilgen.DefineLabel();
            ilgen.Emit(OpCodes.Ldc_I4_0);
            ilgen.EmitBrtrue(label);
            ilgen.EmitThrow("java.lang.NoSuchMethodError");
            ilgen.MarkLabel(label);
            ilgen.Emit(OpCodes.Pop);
            if (!member.IsStatic)
            {
                ilgen.Emit(OpCodes.Pop);
            }
        }
#endif

#if !IMPORTER && !FIRST_PASS
        internal override object GetValue(object obj)
        {
            if (getter == null)
            {
                throw new java.lang.NoSuchMethodError();
            }
            return getter.Invoke(obj, new object[0]);
        }

        internal override void SetValue(object obj, object value)
        {
            if (setter == null)
            {
                throw new java.lang.NoSuchMethodError();
            }
            setter.Invoke(obj, new object[] { value });
        }
#endif

    }

#endif

}
