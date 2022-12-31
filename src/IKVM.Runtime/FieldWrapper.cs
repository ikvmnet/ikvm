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
using System.Diagnostics;

using IKVM.Attributes;

using System.Threading;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{

    abstract class FieldWrapper : MemberWrapper
    {

#if !IMPORTER && !FIRST_PASS && !EXPORTER
        volatile java.lang.reflect.Field reflectionField;
        sun.reflect.FieldAccessor jniAccessor;
#endif
        internal static readonly FieldWrapper[] EmptyArray = new FieldWrapper[0];
        FieldInfo field;
        TypeWrapper fieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="fieldType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="modifiers"></param>
        /// <param name="field"></param>
        /// <param name="flags"></param>
        internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, MemberFlags flags) :
            base(declaringType, name, sig, modifiers, flags)
        {
            Debug.Assert(name != null);
            Debug.Assert(sig != null);
            this.fieldType = fieldType;
            this.field = field;
            UpdateNonPublicTypeInSignatureFlag();
#if IMPORTER
            if (IsFinal
                && DeclaringType.IsPublic
                && !DeclaringType.IsInterface
                && (IsPublic || (IsProtected && !DeclaringType.IsFinal))
                && !DeclaringType.GetClassLoader().StrictFinalFieldSemantics
                && DeclaringType.IsDynamic
                && !(this is ConstantFieldWrapper)
                && !(this is DynamicPropertyFieldWrapper))
            {
                SetType2FinalField();
            }
#endif
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="fieldType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="modifiers"></param>
        /// <param name="field"></param>
        internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, ExModifiers modifiers, FieldInfo field) :
            this(declaringType, fieldType, name, sig, modifiers.Modifiers, field, (modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None))
        {

        }

        private void UpdateNonPublicTypeInSignatureFlag()
        {
            if ((IsPublic || IsProtected) && fieldType != null && !IsAccessStub)
            {
                if (!fieldType.IsPublic && !fieldType.IsUnloadable)
                {
                    SetNonPublicTypeInSignatureFlag();
                }
            }
        }

        internal FieldInfo GetField()
        {
            AssertLinked();
            return field;
        }

        [Conditional("DEBUG")]
        internal void AssertLinked()
        {
            if (fieldType == null)
            {
                Tracer.Error(Tracer.Runtime, "AssertLinked failed: " + this.DeclaringType.Name + "::" + this.Name + " (" + this.Signature + ")");
            }
            Debug.Assert(fieldType != null, this.DeclaringType.Name + "::" + this.Name + " (" + this.Signature + ")");
        }

#if !IMPORTER && !EXPORTER

        internal static FieldWrapper FromField(java.lang.reflect.Field field)
        {
#if FIRST_PASS
            return null;
#else
            int slot = field._slot();
            if (slot == -1)
            {
                // it's a Field created by Unsafe.objectFieldOffset(Class,String) so we must resolve based on the name
                foreach (FieldWrapper fw in TypeWrapper.FromClass(field.getDeclaringClass()).GetFields())
                {
                    if (fw.Name == field.getName())
                    {
                        return fw;
                    }
                }
            }
            return TypeWrapper.FromClass(field.getDeclaringClass()).GetFields()[slot];
#endif
        }

        internal object ToField(bool copy)
        {
            return ToField(copy, null);
        }

        internal object ToField(bool copy, int? fieldIndex)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            java.lang.reflect.Field field = reflectionField;
            if (field == null)
            {
                const Modifiers ReflectionFieldModifiersMask = Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static
                    | Modifiers.Final | Modifiers.Volatile | Modifiers.Transient | Modifiers.Synthetic | Modifiers.Enum;
                Link();
                field = new java.lang.reflect.Field(
                    this.DeclaringType.ClassObject,
                    this.Name,
                    this.FieldTypeWrapper.EnsureLoadable(this.DeclaringType.GetClassLoader()).ClassObject,
                    (int)(this.Modifiers & ReflectionFieldModifiersMask) | (this.IsInternal ? 0x40000000 : 0),
                    fieldIndex ?? Array.IndexOf(this.DeclaringType.GetFields(), this),
                    this.DeclaringType.GetGenericFieldSignature(this),
                    null
                );
            }
            lock (this)
            {
                if (reflectionField == null)
                {
                    reflectionField = field;
                }
                else
                {
                    field = reflectionField;
                }
            }
            if (copy)
            {
                field = field.copy();
            }
            return field;
#endif
        }

#endif

        [System.Security.SecurityCritical]
        internal static FieldWrapper FromCookie(IntPtr cookie)
        {
            return (FieldWrapper)FromCookieImpl(cookie);
        }

        internal TypeWrapper FieldTypeWrapper
        {
            get
            {
                AssertLinked();
                return fieldType;
            }
        }

#if EMITTERS

        internal void EmitGet(CodeEmitter ilgen)
        {
            AssertLinked();
            EmitGetImpl(ilgen);
        }

        protected abstract void EmitGetImpl(CodeEmitter ilgen);

        internal void EmitSet(CodeEmitter ilgen)
        {
            AssertLinked();
            EmitSetImpl(ilgen);
        }

        protected abstract void EmitSetImpl(CodeEmitter ilgen);

#endif

#if IMPORTER
        internal bool IsLinked
        {
            get { return fieldType != null; }
        }
#endif

        internal void Link()
        {
            Link(LoadMode.Link);
        }

        internal void Link(LoadMode mode)
        {
            lock (this)
                if (fieldType != null)
                    return;

            var fld = DeclaringType.GetClassLoader().FieldTypeWrapperFromSig(Signature, mode);

            lock (this)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    if (fieldType == null)
                    {
                        fieldType = fld;
                        UpdateNonPublicTypeInSignatureFlag();

                        try
                        {
                            field = this.DeclaringType.LinkField(this);
                        }
                        catch
                        {
                            // HACK if linking fails, we unlink to make sure
                            // that the next link attempt will fail again
                            fieldType = null;
                            throw;
                        }
                    }
                }
            }
        }

        internal bool IsVolatile
        {
            get
            {
                return (Modifiers & Modifiers.Volatile) != 0;
            }
        }

        internal bool IsSerialVersionUID
        {
            get
            {
                // a serialVersionUID field must be static and final to be recognized (see ObjectStreamClass.getDeclaredSUID())
                return (Modifiers & (Modifiers.Static | Modifiers.Final)) == (Modifiers.Static | Modifiers.Final)
                    && Name == "serialVersionUID"
                    && (FieldTypeWrapper == PrimitiveTypeWrapper.LONG
                        || FieldTypeWrapper == PrimitiveTypeWrapper.INT
                        || FieldTypeWrapper == PrimitiveTypeWrapper.CHAR
                        || FieldTypeWrapper == PrimitiveTypeWrapper.SHORT
                        || FieldTypeWrapper == PrimitiveTypeWrapper.BYTE);
            }
        }

        internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
        {
            // volatile long & double field accesses must be made atomic
            if ((modifiers.Modifiers & Modifiers.Volatile) != 0 && (sig == "J" || sig == "D"))
                return new VolatileLongDoubleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);

            return new SimpleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
        }

#if !IMPORTER && !EXPORTER
        internal virtual void ResolveField()
        {
            var fb = field as FieldBuilder;
            if (fb != null)
            {
#if NETFRAMEWORK
                field = fb.Module.ResolveField(fb.GetToken().Token);
#else
                BindingFlags flags = BindingFlags.DeclaredOnly;
                flags |= fb.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
                flags |= fb.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                field = DeclaringType.TypeAsTBD.GetField(fb.Name, flags);
#endif
            }
        }

        internal object GetFieldAccessorJNI()
        {
#if FIRST_PASS
            return null;
#else
            if (jniAccessor == null)
                Interlocked.CompareExchange(ref jniAccessor, IKVM.Java.Externs.sun.reflect.ReflectionFactory.NewFieldAccessorJNI(this), null);

            return jniAccessor;
#endif
        }

#if !FIRST_PASS

        internal abstract object GetValue(object obj);

        internal abstract void SetValue(object obj, object value);

#endif

#endif

    }

}
