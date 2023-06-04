﻿/*
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
using System.CodeDom;
using System.Linq;
using IKVM.Runtime;

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
            if (name == null)
                throw new ArgumentNullException();
            if (sig == null)
                throw new ArgumentNullException();

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

        void UpdateNonPublicTypeInSignatureFlag()
        {
            if ((IsPublic || IsProtected) && fieldType != null && !IsAccessStub)
            {
                if (!fieldType.IsPublic && !fieldType.IsUnloadable)
                {
                    SetNonPublicTypeInSignatureFlag();
                }
            }
        }

        /// <summary>
        /// Gets the CLR <see cref="FieldInfo"/> that forms the implementation of the field.
        /// </summary>
        /// <returns></returns>
        internal FieldInfo GetField()
        {
            AssertLinked();
            return field;
        }

        /// <summary>
        /// Fails if the field has not yet been linked.
        /// </summary>
        [Conditional("DEBUG")]
        internal void AssertLinked()
        {
            if (fieldType == null)
                throw new InternalException($"Field is not linked: {DeclaringType.Name}::{Name} ({Signature})");

            Debug.Assert(fieldType != null, this.DeclaringType.Name + "::" + this.Name + " (" + this.Signature + ")");
        }

#if !IMPORTER && !EXPORTER

        /// <summary>
        /// Gets the <see cref="FieldWrapper"/> for the given <see cref="java.lang.reflect.Field"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static FieldWrapper FromField(java.lang.reflect.Field field)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            int slot = field._slot();
            if (slot == -1)
            {
                // it's a Field created by Unsafe.objectFieldOffset(Class,String) so we must resolve based on the name
                foreach (var fw in TypeWrapper.FromClass(field.getDeclaringClass()).GetFields())
                    if (fw.Name == field.getName())
                        return fw;
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
                const Modifiers ReflectionFieldModifiersMask = Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static | Modifiers.Final | Modifiers.Volatile | Modifiers.Transient | Modifiers.Synthetic | Modifiers.Enum;
                Link();
                field = new java.lang.reflect.Field(
                    DeclaringType.ClassObject,
                    Name,
                    FieldTypeWrapper.EnsureLoadable(DeclaringType.GetClassLoader()).ClassObject,
                    (int)(Modifiers & ReflectionFieldModifiersMask) | (IsInternal ? 0x40000000 : 0),
                    fieldIndex ?? Array.IndexOf(DeclaringType.GetFields(), this),
                    DeclaringType.GetGenericFieldSignature(this),
                    null
                );
            }

            lock (this)
            {
                if (reflectionField == null)
                    reflectionField = field;
                else
                    field = reflectionField;
            }

            if (copy)
                field = field.copy();

            return field;
#endif
        }

#endif

        /// <summary>
        /// Resolves the <see cref="FieldWrapper"/> instance referenced by the specified cookie.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Emits the IL code to set the value of the field.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitGet(CodeEmitter il)
        {
            AssertLinked();
            EmitGetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to set the value of the field.
        /// </summary>
        /// <param name="il"></param>
        protected abstract void EmitGetImpl(CodeEmitter il);

        /// <summary>
        /// Emits the IL code to implement the unsafe get operation.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitUnsafeGet(CodeEmitter il)
        {
            AssertLinked();
            EmitUnsafeGetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe get operation.
        /// </summary>
        /// <param name="il"></param>
        protected virtual void EmitUnsafeGetImpl(CodeEmitter il)
        {
            EmitGetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe volatile get operation.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitUnsafeVolatileGet(CodeEmitter il)
        {
            AssertLinked();
            EmitUnsafeVolatileGetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe volatile get operation.
        /// </summary>
        /// <param name="il"></param>
        protected virtual void EmitUnsafeVolatileGetImpl(CodeEmitter il)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Emits the IL code to implement the set operation.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitSet(CodeEmitter il)
        {
            AssertLinked();
            EmitSetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the set operation.
        /// </summary>
        /// <param name="il"></param>
        protected abstract void EmitSetImpl(CodeEmitter il);

        /// <summary>
        /// Emits the IL code to implement the unsafe set operation.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitUnsafeSet(CodeEmitter il)
        {
            AssertLinked();
            EmitSetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe set operation.
        /// </summary>
        /// <param name="il"></param>
        protected virtual void EmitUnsafeSetImpl(CodeEmitter il)
        {
            EmitSetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe volatile set operation.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitUnsafeVolatileSet(CodeEmitter il)
        {
            AssertLinked();
            EmitUnsafeVolatileSetImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement the unsafe volatile set operation.
        /// </summary>
        /// <param name="il"></param>
        protected virtual void EmitUnsafeVolatileSetImpl(CodeEmitter il)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Emits the IL code to implement an unsafe compare/exchange operation. The stack is expected to contain the object,
        /// the comparand and the value. The applied value is placed on the stack after execution.
        /// </summary>
        /// <param name="il"></param>
        internal void EmitUnsafeCompareAndSwap(CodeEmitter il)
        {
            AssertLinked();
            EmitUnsafeCompareAndSwapImpl(il);
        }

        /// <summary>
        /// Emits the IL code to implement an unsafe compare/exchange operation. The stack is expected to contain the object,
        /// the comparand and the value. The applied value is placed on the stack after execution.
        /// </summary>
        /// <param name="il"></param>
        protected virtual void EmitUnsafeCompareAndSwapImpl(CodeEmitter il)
        {
            throw new NotSupportedException();
        }

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
                field = fb.DeclaringType.Module.ResolveField(fb.GetToken().Token);
#else
                BindingFlags flags = BindingFlags.DeclaredOnly;
                flags |= fb.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
                flags |= fb.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                field = DeclaringType.TypeAsTBD.GetField(fb.Name, flags);
#endif
            }
        }

#if !FIRST_PASS

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal abstract object GetValue(object obj);

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        internal abstract void SetValue(object obj, object value);

        Delegate unsafeGetValueDelegate;
        Delegate unsafeSetValueDelegate;
        Delegate unsafeVolatileGetDelegate;
        Delegate unsafeVolatileSetDelegate;
        Delegate unsafeCompareExchangeDelegate;

        /// <summary>
        /// Performs an unsafe get operation on the field.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal virtual TField UnsafeGetValue<TField>(object obj)
        {
            return ((Func<object, TField>)GetUnsafeGetDelegate())(obj);
        }

        /// <summary>
        /// Gets a delegate capable of performing an unsafe get operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate GetUnsafeGetDelegate()
        {
            if (unsafeGetValueDelegate == null)
                Interlocked.CompareExchange(ref unsafeGetValueDelegate, CreateUnsafeGetDelegate(), null);

            return unsafeGetValueDelegate;
        }

        /// <summary>
        /// Creates a delegate capable of performing an unsafe get operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate CreateUnsafeGetDelegate()
        {
            DeclaringType.Finish();
            FieldTypeWrapper.Finish();
            ResolveField();

            var ft = FieldTypeWrapper.IsPrimitive ? FieldTypeWrapper.TypeAsSignatureType : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafeGet>__{DeclaringType.Name.Replace(".", "_")}__{Name}", DeclaringType.TypeAsTBD, true, ft, new[] { typeof(object) });
            var il = CodeEmitter.Create(dm);

            if (IsStatic == false)
                il.Emit(OpCodes.Ldarg_0);

            EmitUnsafeGet(il);

            il.Emit(OpCodes.Ret);
            il.DoEmit();
            return dm.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Performs an unsafe set operation on the field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        internal virtual void UnsafeSetValue<TField>(object obj, TField value)
        {
            ((Action<object, TField>)GetUnsafeSetDelegate())(obj, value);
        }

        /// <summary>
        /// Gets a delegate capable of performing an unsafe set operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate GetUnsafeSetDelegate()
        {
            if (unsafeSetValueDelegate == null)
                Interlocked.CompareExchange(ref unsafeSetValueDelegate, CreateUnsafeSetDelegate(), null);

            return unsafeSetValueDelegate;
        }

        /// <summary>
        /// Creates a delegate capable of performing an unsafe set operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate CreateUnsafeSetDelegate()
        {
            DeclaringType.Finish();
            FieldTypeWrapper.Finish();
            ResolveField();

            var ft = FieldTypeWrapper.IsPrimitive ? FieldTypeWrapper.TypeAsSignatureType : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafeSet>__{DeclaringType.Name.Replace(".", "_")}__{Name}", DeclaringType.TypeAsTBD, true, typeof(void), new[] { typeof(object), ft });
            var il = CodeEmitter.Create(dm);

            if (IsStatic == false)
                il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Ldarg_1);
            EmitUnsafeSet(il);

            il.Emit(OpCodes.Ret);
            il.DoEmit();
            return dm.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Performs an unsafe volatile get operation on the field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal virtual TField UnsafeVolatileGet<TField>(object obj)
        {
            return ((Func<object, TField>)GetUnsafeVolatileGetDelegate())(obj);
        }

        /// <summary>
        /// Gets a delegate capable of performing an unsafe volatile get operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate GetUnsafeVolatileGetDelegate()
        {
            if (unsafeVolatileGetDelegate == null)
                Interlocked.CompareExchange(ref unsafeVolatileGetDelegate, CreateUnsafeVolatileGetDelegate(), null);

            return unsafeVolatileGetDelegate;
        }

        /// <summary>
        /// Creates a delegate capable of performing an unsafe volatile get operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate CreateUnsafeVolatileGetDelegate()
        {
            ResolveField();
            var ft = FieldTypeWrapper.IsPrimitive ? FieldTypeWrapper.TypeAsSignatureType : typeof(object);
            var dm = new DynamicMethod($"__<UnsafeVolatileGet>__{DeclaringType.Name.Replace(".", "_")}__{Name}", ft, new[] { typeof(object) }, DeclaringType.TypeAsTBD.Module, true);
            var il = CodeEmitter.Create(dm);

            if (IsStatic == false)
                il.Emit(OpCodes.Ldarg_0);

            EmitUnsafeVolatileGet(il);

            il.Emit(OpCodes.Ret);
            il.DoEmit();
            return dm.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Performs an unsafe volatile set operation on the field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal virtual void UnsafeVolatileSet<TField>(object obj, TField value)
        {
            ((Action<object, TField>)GetUnsafeVolatileSetDelegate())(obj, value);
        }

        /// <summary>
        /// Gets a delegate capable of performing an unsafe volatile set operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate GetUnsafeVolatileSetDelegate()
        {
            if (unsafeVolatileSetDelegate == null)
                Interlocked.CompareExchange(ref unsafeVolatileSetDelegate, CreateUnsafeVolatileSetDelegate(), null);

            return unsafeVolatileSetDelegate;
        }

        /// <summary>
        /// Creates a delegate capable of performing an unsafe volatile set operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate CreateUnsafeVolatileSetDelegate()
        {
            ResolveField();
            var ft = FieldTypeWrapper.IsPrimitive ? FieldTypeWrapper.TypeAsSignatureType : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafeVolatileSet>__{DeclaringType.Name.Replace(".", "_")}__{Name}", DeclaringType.TypeAsTBD, true, typeof(void), new[] { typeof(object), ft });
            var il = CodeEmitter.Create(dm);

            if (IsStatic == false)
                il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Ldarg_1);
            EmitUnsafeVolatileSet(il);

            il.Emit(OpCodes.Ret);
            il.DoEmit();
            return dm.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Unsafe compare and swap implementation for the field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal virtual bool UnsafeCompareAndSwap<TField>(object obj, TField expected, TField value)
        {
            return ((Func<object, TField, TField, bool>)GetUnsafeCompareAndSwapDelegate())(obj, expected, value);
        }


        /// <summary>
        /// Gets a delegate implementing the unsafe compare and swap operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate GetUnsafeCompareAndSwapDelegate()
        {
            if (unsafeCompareExchangeDelegate == null)
                Interlocked.CompareExchange(ref unsafeCompareExchangeDelegate, CreateUnsafeCompareAndSwapDelegate(), null);

            return unsafeCompareExchangeDelegate;
        }

        /// <summary>
        /// Creates a delegate implementing the unsafe compare and swap operation on the field.
        /// </summary>
        /// <returns></returns>
        Delegate CreateUnsafeCompareAndSwapDelegate()
        {
            ResolveField();
            var ft = FieldTypeWrapper.IsPrimitive ? FieldTypeWrapper.TypeAsSignatureType : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafeCompareAndSwap>__{DeclaringType.Name.Replace(".", "_")}__{Name}", DeclaringType.TypeAsTBD, true, typeof(bool), new[] { typeof(object), ft, ft });
            var il = CodeEmitter.Create(dm);

            if (IsStatic == false)
                il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            EmitUnsafeCompareAndSwap(il);

            il.Emit(OpCodes.Ret);
            il.DoEmit();
            return dm.CreateDelegate(typeof(Func<,,,>).MakeGenericType(typeof(object), ft, ft, typeof(bool)));
        }

#endif

#endif

    }

}
