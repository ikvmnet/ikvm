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
using System.Runtime.InteropServices;

using IKVM.Attributes;
using IKVM.Runtime;

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

namespace IKVM.Internal
{

    abstract class MemberWrapper
    {

        readonly TypeWrapper declaringType;
        readonly string name;
        readonly string sig;
        protected readonly Modifiers modifiers;
        HandleWrapper handle;
        MemberFlags flags;

        sealed class HandleWrapper
        {

            internal readonly IntPtr Value;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="obj"></param>
            [System.Security.SecurityCritical]
            internal HandleWrapper(MemberWrapper obj)
            {
                Value = (IntPtr)GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
            }

#if CLASSGC

            /// <summary>
            /// Finalizes the instance.
            /// </summary>
            [System.Security.SecuritySafeCritical]
            ~HandleWrapper()
            {
                if (!Environment.HasShutdownStarted)
                {
                    var h = (GCHandle)Value;
                    if (h.Target == null)
                        h.Free();
                    else
                        GC.ReRegisterForFinalize(this);
                }
            }

#endif

        }

        protected MemberWrapper(TypeWrapper declaringType, string name, string sig, Modifiers modifiers, MemberFlags flags)
        {
            Debug.Assert(declaringType != null);
            this.declaringType = declaringType;
            this.name = String.Intern(name);
            this.sig = String.Intern(sig);
            this.modifiers = modifiers;
            this.flags = flags;
        }

        internal IntPtr Cookie
        {
            get
            {
                lock (this)
                    handle ??= new HandleWrapper(this);

                return handle.Value;
            }
        }

        [System.Security.SecurityCritical]
        internal static MemberWrapper FromCookieImpl(IntPtr cookie)
        {
            return (MemberWrapper)GCHandle.FromIntPtr(cookie).Target;
        }

        internal TypeWrapper DeclaringType => declaringType;

        internal string Name => name;

        internal string Signature => sig;

        internal bool IsAccessibleFrom(TypeWrapper referencedType, TypeWrapper caller, TypeWrapper instance)
        {
            if (referencedType.IsAccessibleFrom(caller))
            {
                return (
                    caller == DeclaringType ||
                    IsPublicOrProtectedMemberAccessible(caller, instance) ||
                    (IsInternal && DeclaringType.InternalsVisibleTo(caller)) ||
                    (!IsPrivate && DeclaringType.IsPackageAccessibleFrom(caller)))
                    // The JVM supports accessing members that have non-public types in their signature from another package,
                    // but the CLI doesn't. It would be nice if we worked around that by emitting extra accessors, but for now
                    // we'll simply disallow such access across assemblies (unless the appropriate InternalsVisibleToAttribute exists).
                    && (!(HasNonPublicTypeInSignature || IsType2FinalField) || InPracticeInternalsVisibleTo(caller));
            }
            return false;
        }

        private bool IsPublicOrProtectedMemberAccessible(TypeWrapper caller, TypeWrapper instance)
        {
            if (IsPublic || (IsProtected && caller.IsSubTypeOf(DeclaringType) && (IsStatic || instance.IsUnloadable || instance.IsSubTypeOf(caller))))
            {
                return DeclaringType.IsPublic || InPracticeInternalsVisibleTo(caller);
            }
            return false;
        }

        private bool InPracticeInternalsVisibleTo(TypeWrapper caller)
        {
#if !IMPORTER
            if (DeclaringType.TypeAsTBD.Assembly.Equals(caller.TypeAsTBD.Assembly))
            {
                // both the caller and the declaring type are in the same assembly
                // so we know that the internals are visible
                // (this handles the case where we're running in dynamic mode)
                return true;
            }
#endif
#if CLASSGC
            if (DeclaringType.IsDynamic)
            {
                // if we are dynamic, we can just become friends with the caller
                DeclaringType.GetClassLoader().GetTypeWrapperFactory().AddInternalsVisibleTo(caller.TypeAsTBD.Assembly);
                return true;
            }
#endif
            return DeclaringType.InternalsVisibleTo(caller);
        }

        internal bool IsHideFromReflection
        {
            get
            {
                return (flags & MemberFlags.HideFromReflection) != 0;
            }
        }

        internal bool IsExplicitOverride
        {
            get
            {
                return (flags & MemberFlags.ExplicitOverride) != 0;
            }
        }

        internal bool IsMirandaMethod
        {
            get
            {
                return (flags & MemberFlags.MirandaMethod) != 0;
            }
        }

        internal bool IsAccessStub
        {
            get
            {
                return (flags & MemberFlags.AccessStub) != 0;
            }
        }

        internal bool IsPropertyAccessor
        {
            get
            {
                return (flags & MemberFlags.PropertyAccessor) != 0;
            }
            set
            {
                // this is unsynchronized, so it may only be called during the JavaTypeImpl constructor
                if (value)
                {
                    flags |= MemberFlags.PropertyAccessor;
                }
                else
                {
                    flags &= ~MemberFlags.PropertyAccessor;
                }
            }
        }

        internal bool IsIntrinsic
        {
            get
            {
                return (flags & MemberFlags.Intrinsic) != 0;
            }
        }

        protected void SetIntrinsicFlag()
        {
            flags |= MemberFlags.Intrinsic;
        }

        protected void SetNonPublicTypeInSignatureFlag()
        {
            flags |= MemberFlags.NonPublicTypeInSignature;
        }

        internal bool HasNonPublicTypeInSignature
        {
            get { return (flags & MemberFlags.NonPublicTypeInSignature) != 0; }
        }

        protected void SetType2FinalField()
        {
            flags |= MemberFlags.Type2FinalField;
        }

        internal bool IsType2FinalField
        {
            get { return (flags & MemberFlags.Type2FinalField) != 0; }
        }

        internal bool HasCallerID
        {
            get
            {
                return (flags & MemberFlags.CallerID) != 0;
            }
        }

        internal bool IsDelegateInvokeWithByRefParameter
        {
            get { return (flags & MemberFlags.DelegateInvokeWithByRefParameter) != 0; }
        }

        internal bool IsNoOp
        {
            get { return (flags & MemberFlags.NoOp) != 0; }
        }

        internal Modifiers Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        internal bool IsStatic
        {
            get
            {
                return (modifiers & Modifiers.Static) != 0;
            }
        }

        internal bool IsInternal
        {
            get
            {
                return (flags & MemberFlags.InternalAccess) != 0;
            }
        }

        internal bool IsPublic
        {
            get
            {
                return (modifiers & Modifiers.Public) != 0;
            }
        }

        internal bool IsPrivate
        {
            get
            {
                return (modifiers & Modifiers.Private) != 0;
            }
        }

        internal bool IsProtected
        {
            get
            {
                return (modifiers & Modifiers.Protected) != 0;
            }
        }

        internal bool IsFinal
        {
            get
            {
                return (modifiers & Modifiers.Final) != 0;
            }
        }
    }

    sealed class PrivateInterfaceMethodWrapper : SmartMethodWrapper
    {
        internal PrivateInterfaceMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
            : base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
        {
        }

#if EMITTERS

        protected override void CallImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

        protected override void CallvirtImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

#endif

    }

    sealed class SimpleFieldWrapper : FieldWrapper
    {
        internal SimpleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
            : base(declaringType, fieldType, name, sig, modifiers, fi)
        {
            Debug.Assert(!(fieldType == PrimitiveTypeWrapper.DOUBLE || fieldType == PrimitiveTypeWrapper.LONG) || !IsVolatile);
        }

#if EMITTERS
        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            if (!IsStatic && DeclaringType.IsNonPrimitiveValueType)
            {
                ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
            }
            if (IsVolatile)
            {
                ilgen.Emit(OpCodes.Volatile);
            }
            ilgen.Emit(IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, GetField());
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            FieldInfo fi = GetField();
            if (!IsStatic && DeclaringType.IsNonPrimitiveValueType)
            {
                CodeEmitterLocal temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
                ilgen.Emit(OpCodes.Stloc, temp);
                ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
                ilgen.Emit(OpCodes.Ldloc, temp);
            }
            if (IsVolatile)
            {
                ilgen.Emit(OpCodes.Volatile);
            }
            ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
            if (IsVolatile)
            {
                ilgen.EmitMemoryBarrier();
            }
        }
#endif // EMITTERS

#if !IMPORTER && !EXPORTER && !FIRST_PASS
        internal override object GetValue(object obj)
        {
            return GetField().GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            GetField().SetValue(obj, value);
        }
#endif // !IMPORTER && !EXPORTER && !FIRST_PASS
    }

    sealed class VolatileLongDoubleFieldWrapper : FieldWrapper
    {
        internal VolatileLongDoubleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
            : base(declaringType, fieldType, name, sig, modifiers, fi)
        {
            Debug.Assert(IsVolatile);
            Debug.Assert(sig == "J" || sig == "D");
        }

#if EMITTERS
        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            FieldInfo fi = GetField();
            if (fi.IsStatic)
            {
                ilgen.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                {
                    ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
                }
                ilgen.Emit(OpCodes.Ldflda, fi);
            }
            if (FieldTypeWrapper == PrimitiveTypeWrapper.DOUBLE)
            {
                ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileReadDouble);
            }
            else
            {
                Debug.Assert(FieldTypeWrapper == PrimitiveTypeWrapper.LONG);
                ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileReadLong);
            }
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            FieldInfo fi = GetField();
            CodeEmitterLocal temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
            ilgen.Emit(OpCodes.Stloc, temp);
            if (fi.IsStatic)
            {
                ilgen.Emit(OpCodes.Ldsflda, fi);
            }
            else
            {
                if (DeclaringType.IsNonPrimitiveValueType)
                {
                    ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
                }
                ilgen.Emit(OpCodes.Ldflda, fi);
            }
            ilgen.Emit(OpCodes.Ldloc, temp);
            if (FieldTypeWrapper == PrimitiveTypeWrapper.DOUBLE)
            {
                ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileWriteDouble);
            }
            else
            {
                Debug.Assert(FieldTypeWrapper == PrimitiveTypeWrapper.LONG);
                ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileWriteLong);
            }
        }
#endif // EMITTERS

#if !IMPORTER && !EXPORTER && !FIRST_PASS
#if NO_REF_EMIT
		internal static readonly object lockObject = new object();
#endif

        internal override object GetValue(object obj)
        {
#if NO_REF_EMIT
			lock (lockObject)
			{
				return GetField().GetValue(obj);
			}
#else
            throw new InvalidOperationException();
#endif
        }

        internal override void SetValue(object obj, object value)
        {
#if NO_REF_EMIT
			lock (lockObject)
			{
				GetField().SetValue(obj, value);
			}
#else
            throw new InvalidOperationException();
#endif
        }
#endif // !IMPORTER && !EXPORTER && !FIRST_PASS
    }

#if !EXPORTER

    /// <summary>
    /// Represents a .NET property defined in Java with the <see cref="ikvm.lang.Property"/> annotation.
    /// </summary>
    sealed class DynamicPropertyFieldWrapper : FieldWrapper
    {

        readonly MethodWrapper getter;
        readonly MethodWrapper setter;
        PropertyBuilder pb;

        MethodWrapper GetMethod(string name, string sig, bool isstatic)
        {
            if (name != null)
            {
                var mw = DeclaringType.GetMethodWrapper(name, sig, false);
                if (mw != null && mw.IsStatic == isstatic)
                {
                    mw.IsPropertyAccessor = true;
                    return mw;
                }

                Tracer.Error(Tracer.Compiler, "Property '{0}' accessor '{1}' not found in class '{2}'", this.Name, name, this.DeclaringType.Name);
            }

            return null;
        }

        internal DynamicPropertyFieldWrapper(TypeWrapper declaringType, ClassFile.Field fld) :
            base(declaringType, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal), null)
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
            AttributeHelper.SetModifiers(pb, this.Modifiers, this.IsInternal);
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

        internal static void EmitThrowNoSuchMethodErrorForGetter(CodeEmitter ilgen, TypeWrapper type, MemberWrapper member)
        {
#if IMPORTER
            StaticCompiler.IssueMessage(Message.EmittedNoSuchMethodError, "<unknown>", member.DeclaringType.Name + "." + member.Name + member.Signature);
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

        internal static void EmitThrowNoSuchMethodErrorForSetter(CodeEmitter ilgen, MemberWrapper member)
        {
#if IMPORTER
            StaticCompiler.IssueMessage(Message.EmittedNoSuchMethodError, "<unknown>", member.DeclaringType.Name + "." + member.Name + member.Signature);
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
#endif // EMITTERS

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
#endif // !EXPORTER

    // this class represents a .NET property defined in Java with the ikvm.lang.Property annotation
    sealed class CompiledPropertyFieldWrapper : FieldWrapper
    {
        private readonly PropertyInfo property;

        internal CompiledPropertyFieldWrapper(TypeWrapper declaringType, PropertyInfo property, ExModifiers modifiers)
            : base(declaringType, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType), property.Name, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType).SigName, modifiers, null)
        {
            this.property = property;
        }

#if EMITTERS
        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            MethodInfo getter = property.GetGetMethod(true);
            if (getter == null)
            {
                DynamicPropertyFieldWrapper.EmitThrowNoSuchMethodErrorForGetter(ilgen, this.FieldTypeWrapper, this);
            }
            else if (getter.IsStatic)
            {
                ilgen.Emit(OpCodes.Call, getter);
            }
            else
            {
                ilgen.Emit(OpCodes.Callvirt, getter);
            }
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            MethodInfo setter = property.GetSetMethod(true);
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
                    DynamicPropertyFieldWrapper.EmitThrowNoSuchMethodErrorForSetter(ilgen, this);
                }
            }
            else if (setter.IsStatic)
            {
                ilgen.Emit(OpCodes.Call, setter);
            }
            else
            {
                ilgen.Emit(OpCodes.Callvirt, setter);
            }
        }
#endif // EMITTERS

#if !IMPORTER && !EXPORTER && !FIRST_PASS
        internal override object GetValue(object obj)
        {
            MethodInfo getter = property.GetGetMethod(true);
            if (getter == null)
            {
                throw new java.lang.NoSuchMethodError();
            }
            return getter.Invoke(obj, new object[0]);
        }

        internal override void SetValue(object obj, object value)
        {
            MethodInfo setter = property.GetSetMethod(true);
            if (setter == null)
            {
                throw new java.lang.NoSuchMethodError();
            }
            setter.Invoke(obj, new object[] { value });
        }
#endif

        internal PropertyInfo GetProperty()
        {
            return property;
        }
    }

    sealed class ConstantFieldWrapper : FieldWrapper
    {

        // NOTE this field wrapper can resprent a .NET enum, but in that case "constant" contains the raw constant value (i.e. the boxed underlying primitive value, not a boxed enum)

        /// <summary>
        /// Reference to the constant. In the case of a primitive type or enum, this is the boxed underlying value.
        /// </summary>
        object constant;

        internal ConstantFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, object constant, MemberFlags flags) :
            base(declaringType, fieldType, name, sig, modifiers, field, flags)
        {
            if (IsStatic == false)
                throw new InternalException();

            Debug.Assert(IsStatic);
            Debug.Assert(constant == null || constant.GetType().IsPrimitive || constant is string);
            this.constant = constant;
        }

#if EMITTERS
        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            // Reading a field should trigger the cctor, but since we're inlining the value
            // we have to trigger it explicitly
            DeclaringType.EmitRunClassConstructor(ilgen);

            // NOTE even though you're not supposed to access a constant static final (the compiler is supposed
            // to inline them), we have to support it (because it does happen, e.g. if the field becomes final
            // after the referencing class was compiled, or when we're accessing an unsigned primitive .NET field)
            object v = GetConstantValue();
            if (v == null)
            {
                ilgen.Emit(OpCodes.Ldnull);
            }
            else if (constant is int ||
                constant is short ||
                constant is ushort ||
                constant is byte ||
                constant is sbyte ||
                constant is char ||
                constant is bool)
            {
                ilgen.EmitLdc_I4(((IConvertible)constant).ToInt32(null));
            }
            else if (constant is string)
            {
                ilgen.Emit(OpCodes.Ldstr, (string)constant);
            }
            else if (constant is float)
            {
                ilgen.EmitLdc_R4((float)constant);
            }
            else if (constant is double)
            {
                ilgen.EmitLdc_R8((double)constant);
            }
            else if (constant is long)
            {
                ilgen.EmitLdc_I8((long)constant);
            }
            else if (constant is uint)
            {
                ilgen.EmitLdc_I4(unchecked((int)((IConvertible)constant).ToUInt32(null)));
            }
            else if (constant is ulong)
            {
                ilgen.EmitLdc_I8(unchecked((long)(ulong)constant));
            }
            else
            {
                throw new InvalidOperationException(constant.GetType().FullName);
            }
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            // when constant static final fields are updated, the JIT normally doesn't see that (because the
            // constant value is inlined), so we emulate that behavior by emitting a Pop
            ilgen.Emit(OpCodes.Pop);
        }
#endif // EMITTERS

        internal object GetConstantValue()
        {
            if (constant == null)
            {
                FieldInfo field = GetField();
                constant = field.GetRawConstantValue();
            }
            return constant;
        }

#if !EXPORTER && !IMPORTER && !FIRST_PASS
        internal override object GetValue(object obj)
        {
            FieldInfo field = GetField();
            return FieldTypeWrapper.IsPrimitive || field == null
                ? GetConstantValue()
                : field.GetValue(null);
        }

        internal override void SetValue(object obj, object value)
        {
        }
#endif
    }

    sealed class CompiledAccessStubFieldWrapper : FieldWrapper
    {

        readonly MethodInfo getter;
        readonly MethodInfo setter;

        static Modifiers GetModifiers(PropertyInfo property)
        {
            // NOTE we only support the subset of modifiers that is expected for "access stub" properties
            var getter = property.GetGetMethod(true);
            var modifiers = getter.IsPublic ? Modifiers.Public : Modifiers.Protected;
            if (!property.CanWrite)
                modifiers |= Modifiers.Final;
            if (getter.IsStatic)
                modifiers |= Modifiers.Static;

            return modifiers;
        }

        /// <summary>
        /// Initializes a new instance. This constructor is used for type 1 access stubs.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="propertyType"></param>
        internal CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, TypeWrapper propertyType) :
            this(wrapper, property, null, propertyType, GetModifiers(property), MemberFlags.HideFromReflection | MemberFlags.AccessStub)
        {

        }

        /// <summary>
        /// Initializes a new instance. This constructor is used for type 2 access stubs.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="field"></param>
        /// <param name="propertyType"></param>
        internal CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, FieldInfo field, TypeWrapper propertyType) :
            this(wrapper, property, field, propertyType, AttributeHelper.GetModifiersAttribute(property).Modifiers, MemberFlags.AccessStub)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="property"></param>
        /// <param name="field"></param>
        /// <param name="propertyType"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        private CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, FieldInfo field, TypeWrapper propertyType, Modifiers modifiers, MemberFlags flags) :
            base(wrapper, propertyType, property.Name, propertyType.SigName, modifiers, field, flags)
        {
            this.getter = property.GetGetMethod(true);
            this.setter = property.GetSetMethod(true);
        }

#if EMITTERS

        protected override void EmitGetImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, getter);
        }

        protected override void EmitSetImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, setter);
        }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        internal override object GetValue(object obj)
        {
            // we can only be invoked on type 2 access stubs (because type 1 access stubs are HideFromReflection), so we know we have a field
            return GetField().GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            // we can only be invoked on type 2 access stubs (because type 1 access stubs are HideFromReflection), so we know we have a field
            GetField().SetValue(obj, value);
        }

#endif 

    }

}
