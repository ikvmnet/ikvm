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
using System.Collections.Generic;
using System.Diagnostics;

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

    /// <summary>
    /// Encapsulates the information about a type available to Java.
    /// </summary>
    internal abstract class TypeWrapper
    {

        internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
        internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

        internal static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
        static readonly object flagsLock = new object();

        readonly string name; // java name (e.g. java.lang.Object)
        readonly Modifiers modifiers;
        TypeFlags flags;
        MethodWrapper[] methods;
        FieldWrapper[] fields;
#if !IMPORTER && !EXPORTER
        java.lang.Class classObject;
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="modifiers"></param>
        /// <param name="name"></param>
        /// <exception cref="InternalException"></exception>
        internal TypeWrapper(TypeFlags flags, Modifiers modifiers, string name)
        {
            Profiler.Count("TypeWrapper");

            this.flags = flags;
            this.modifiers = modifiers;
            this.name = name == null ? null : String.Intern(name);
        }

#if EMITTERS

        internal void EmitClassLiteral(CodeEmitter ilgen)
        {
            Debug.Assert(!this.IsPrimitive);

            Type type = GetClassLiteralType();

            // note that this has to be the same check as in LazyInitClass
            if (!this.IsFastClassLiteralSafe || IsForbiddenTypeParameterType(type))
            {
                int rank = 0;
                while (ReflectUtil.IsVector(type))
                {
                    rank++;
                    type = type.GetElementType();
                }
                if (rank == 0)
                {
                    ilgen.Emit(OpCodes.Ldtoken, type);
                    Compiler.getClassFromTypeHandle.EmitCall(ilgen);
                }
                else
                {
                    ilgen.Emit(OpCodes.Ldtoken, type);
                    ilgen.EmitLdc_I4(rank);
                    Compiler.getClassFromTypeHandle2.EmitCall(ilgen);
                }
            }
            else
            {
                ilgen.Emit(OpCodes.Ldsfld, RuntimeHelperTypes.GetClassLiteralField(type));
            }
        }
#endif // EMITTERS

        private Type GetClassLiteralType()
        {
            Debug.Assert(!this.IsPrimitive);

            TypeWrapper tw = this;
            if (tw.IsGhostArray)
            {
                var rank = tw.ArrayRank;
                while (tw.IsArray)
                    tw = tw.ElementTypeWrapper;

                return ArrayTypeWrapper.MakeArrayType(tw.TypeAsTBD, rank);
            }
            else
            {
                return tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD;
            }
        }

        private static bool IsForbiddenTypeParameterType(Type type)
        {
            // these are the types that may not be used as a type argument when instantiating a generic type
            return type == Types.Void
#if NETFRAMEWORK
                || type == JVM.Import(typeof(ArgIterator))
#endif
                || type == JVM.Import(typeof(RuntimeArgumentHandle))
                || type == JVM.Import(typeof(TypedReference))
                || type.ContainsGenericParameters
                || type.IsByRef;
        }

        internal virtual bool IsFastClassLiteralSafe
        {
            get { return false; }
        }

#if !IMPORTER && !EXPORTER

        internal void SetClassObject(java.lang.Class classObject)
        {
            this.classObject = classObject;
        }

        internal java.lang.Class ClassObject
        {
            get
            {
                Debug.Assert(!IsUnloadable && !IsVerifierType);
                if (classObject == null)
                    LazyInitClass();

                return classObject;
            }
        }

#if !FIRST_PASS

        private java.lang.Class GetPrimitiveClass()
        {
            if (this == PrimitiveTypeWrapper.BYTE)
            {
                return java.lang.Byte.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.CHAR)
            {
                return java.lang.Character.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.DOUBLE)
            {
                return java.lang.Double.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.FLOAT)
            {
                return java.lang.Float.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.INT)
            {
                return java.lang.Integer.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.LONG)
            {
                return java.lang.Long.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.SHORT)
            {
                return java.lang.Short.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.BOOLEAN)
            {
                return java.lang.Boolean.TYPE;
            }
            else if (this == PrimitiveTypeWrapper.VOID)
            {
                return java.lang.Void.TYPE;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
#endif

        private void LazyInitClass()
        {
            lock (this)
            {
                if (classObject == null)
                {
                    // DynamicTypeWrapper should haved already had SetClassObject explicitly
                    Debug.Assert(!IsDynamic);
#if !FIRST_PASS
                    java.lang.Class clazz;
                    // note that this has to be the same check as in EmitClassLiteral
                    if (!this.IsFastClassLiteralSafe)
                    {
                        if (this.IsPrimitive)
                        {
                            clazz = GetPrimitiveClass();
                        }
                        else
                        {
                            clazz = new java.lang.Class(null);
                        }
                    }
                    else
                    {
                        Type type = GetClassLiteralType();
                        if (IsForbiddenTypeParameterType(type))
                        {
                            clazz = new java.lang.Class(type);
                        }
                        else
                        {
                            clazz = (java.lang.Class)typeof(ikvm.@internal.ClassLiteral<>).MakeGenericType(type).GetField("Value").GetValue(null);
                        }
                    }
#if __MonoCS__
					SetTypeWrapperHack(clazz, this);
#else
                    clazz.typeWrapper = this;
#endif
                    // MONOBUG Interlocked.Exchange is broken on Mono, so we use CompareExchange
                    System.Threading.Interlocked.CompareExchange(ref classObject, clazz, null);
#endif
                }
            }
        }

#if __MonoCS__
		// MONOBUG this method is to work around an mcs bug
		internal static void SetTypeWrapperHack(object clazz, TypeWrapper type)
		{
#if !FIRST_PASS
			typeof(java.lang.Class).GetField("typeWrapper", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(clazz, type);
#endif
		}
#endif

#if !FIRST_PASS

        private static void ResolvePrimitiveTypeWrapperClasses()
        {
            // note that we're evaluating all ClassObject properties for the side effect
            // (to initialize and associate the ClassObject with the TypeWrapper)
            if (PrimitiveTypeWrapper.BYTE.ClassObject == null
                || PrimitiveTypeWrapper.CHAR.ClassObject == null
                || PrimitiveTypeWrapper.DOUBLE.ClassObject == null
                || PrimitiveTypeWrapper.FLOAT.ClassObject == null
                || PrimitiveTypeWrapper.INT.ClassObject == null
                || PrimitiveTypeWrapper.LONG.ClassObject == null
                || PrimitiveTypeWrapper.SHORT.ClassObject == null
                || PrimitiveTypeWrapper.BOOLEAN.ClassObject == null
                || PrimitiveTypeWrapper.VOID.ClassObject == null)
            {
                throw new InvalidOperationException();
            }
        }
#endif

        internal static TypeWrapper FromClass(java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // MONOBUG redundant cast to workaround mcs bug
            var tw = (TypeWrapper)(object)clazz.typeWrapper;
            if (tw == null)
            {
                var type = clazz.type;
                if (type == null)
                {
                    ResolvePrimitiveTypeWrapperClasses();
                    return FromClass(clazz);
                }

                if (type == typeof(void) || type.IsPrimitive || ClassLoaderWrapper.IsRemappedType(type))
                    tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
                else
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);

#if __MonoCS__
				SetTypeWrapperHack(clazz, tw);
#else
                clazz.typeWrapper = tw;
#endif
            }

            return tw;
#endif
        }

#endif // !IMPORTER && !EXPORTER

        public override string ToString()
        {
            return GetType().Name + "[" + name + "]";
        }

        // For UnloadableTypeWrapper it tries to load the type through the specified loader
        // and if that fails it throw a NoClassDefFoundError (not a java.lang.NoClassDefFoundError),
        // for all other types this is a no-op.
        internal virtual TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
        {
            return this;
        }

        private void SetTypeFlag(TypeFlags flag)
        {
            // we use a global lock object, since the chance of contention is very small
            lock (flagsLock)
            {
                flags |= flag;
            }
        }

        internal bool HasIncompleteInterfaceImplementation
        {
            get
            {
                TypeWrapper baseWrapper = this.BaseTypeWrapper;
                return (flags & TypeFlags.HasIncompleteInterfaceImplementation) != 0 || (baseWrapper != null && baseWrapper.HasIncompleteInterfaceImplementation);
            }
        }

        internal void SetHasIncompleteInterfaceImplementation()
        {
            SetTypeFlag(TypeFlags.HasIncompleteInterfaceImplementation);
        }

        internal bool HasUnsupportedAbstractMethods
        {
            get
            {
                foreach (var iface in this.Interfaces)
                    if (iface.HasUnsupportedAbstractMethods)
                        return true;

                var baseWrapper = this.BaseTypeWrapper;
                return (flags & TypeFlags.HasUnsupportedAbstractMethods) != 0 || (baseWrapper != null && baseWrapper.HasUnsupportedAbstractMethods);
            }
        }

        internal void SetHasUnsupportedAbstractMethods()
        {
            SetTypeFlag(TypeFlags.HasUnsupportedAbstractMethods);
        }

        internal virtual bool HasStaticInitializer
        {
            get
            {
                return (flags & TypeFlags.HasStaticInitializer) != 0;
            }
        }

        internal void SetHasStaticInitializer()
        {
            SetTypeFlag(TypeFlags.HasStaticInitializer);
        }

        internal bool HasVerifyError
        {
            get
            {
                return (flags & TypeFlags.VerifyError) != 0;
            }
        }

        internal void SetHasVerifyError()
        {
            SetTypeFlag(TypeFlags.VerifyError);
        }

        internal bool HasClassFormatError
        {
            get
            {
                return (flags & TypeFlags.ClassFormatError) != 0;
            }
        }

        internal void SetHasClassFormatError()
        {
            SetTypeFlag(TypeFlags.ClassFormatError);
        }

        internal virtual bool IsFakeTypeContainer
        {
            get
            {
                return false;
            }
        }

        internal virtual bool IsFakeNestedType
        {
            get
            {
                return false;
            }
        }

        // is this an anonymous class (in the sense of Unsafe.defineAnonymousClass(), not the JLS)
        internal bool IsUnsafeAnonymous
        {
            get { return (flags & TypeFlags.Anonymous) != 0; }
        }

        // a ghost is an interface that appears to be implemented by a .NET type
        // (e.g. System.String (aka java.lang.String) appears to implement java.lang.CharSequence,
        // so java.lang.CharSequence is a ghost)
        internal virtual bool IsGhost
        {
            get
            {
                return false;
            }
        }

        // is this an array type of which the ultimate element type is a ghost?
        internal bool IsGhostArray
        {
            get
            {
                return !IsUnloadable && IsArray && (ElementTypeWrapper.IsGhost || ElementTypeWrapper.IsGhostArray);
            }
        }

        internal virtual FieldInfo GhostRefField
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        internal virtual bool IsRemapped
        {
            get
            {
                return false;
            }
        }

        internal bool IsArray
        {
            get
            {
                return name != null && name[0] == '[';
            }
        }

        // NOTE for non-array types this returns 0
        internal int ArrayRank
        {
            get
            {
                int i = 0;
                if (name != null)
                {
                    while (name[i] == '[')
                    {
                        i++;
                    }
                }
                return i;
            }
        }

        internal virtual TypeWrapper GetUltimateElementTypeWrapper()
        {
            throw new InvalidOperationException();
        }

        internal bool IsNonPrimitiveValueType
        {
            get
            {
                return this != VerifierTypeWrapper.Null && !IsPrimitive && !IsGhost && TypeAsTBD.IsValueType;
            }
        }

        internal bool IsPrimitive
        {
            get
            {
                return name == null;
            }
        }

        internal bool IsWidePrimitive
        {
            get
            {
                return this == PrimitiveTypeWrapper.LONG || this == PrimitiveTypeWrapper.DOUBLE;
            }
        }

        internal bool IsIntOnStackPrimitive
        {
            get
            {
                return name == null &&
                    (this == PrimitiveTypeWrapper.BOOLEAN ||
                    this == PrimitiveTypeWrapper.BYTE ||
                    this == PrimitiveTypeWrapper.CHAR ||
                    this == PrimitiveTypeWrapper.SHORT ||
                    this == PrimitiveTypeWrapper.INT);
            }
        }

        private static bool IsJavaPrimitive(Type type)
        {
            return type == PrimitiveTypeWrapper.BOOLEAN.TypeAsTBD
                || type == PrimitiveTypeWrapper.BYTE.TypeAsTBD
                || type == PrimitiveTypeWrapper.CHAR.TypeAsTBD
                || type == PrimitiveTypeWrapper.DOUBLE.TypeAsTBD
                || type == PrimitiveTypeWrapper.FLOAT.TypeAsTBD
                || type == PrimitiveTypeWrapper.INT.TypeAsTBD
                || type == PrimitiveTypeWrapper.LONG.TypeAsTBD
                || type == PrimitiveTypeWrapper.SHORT.TypeAsTBD
                || type == PrimitiveTypeWrapper.VOID.TypeAsTBD;
        }

        internal bool IsBoxedPrimitive
        {
            get
            {
                return !IsPrimitive && IsJavaPrimitive(TypeAsSignatureType);
            }
        }

        internal bool IsErasedOrBoxedPrimitiveOrRemapped
        {
            get
            {
                bool erased = IsUnloadable || IsGhostArray;
                return erased || IsBoxedPrimitive || (IsRemapped && this is DotNetTypeWrapper);
            }
        }

        internal bool IsUnloadable
        {
            get
            {
                // NOTE we abuse modifiers to note unloadable classes
                return modifiers == UnloadableModifiersHack;
            }
        }

        internal bool IsVerifierType
        {
            get
            {
                // NOTE we abuse modifiers to note verifier types
                return modifiers == VerifierTypeModifiersHack;
            }
        }

        internal virtual bool IsMapUnsafeException
        {
            get
            {
                return false;
            }
        }

        internal Modifiers Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        // since for inner classes, the modifiers returned by Class.getModifiers are different from the actual
        // modifiers (as used by the VM access control mechanism), we have this additional property
        internal virtual Modifiers ReflectiveModifiers
        {
            get
            {
                return modifiers;
            }
        }

        internal bool IsInternal
        {
            get
            {
                return (flags & TypeFlags.InternalAccess) != 0;
            }
        }

        internal bool IsPublic
        {
            get
            {
                return (modifiers & Modifiers.Public) != 0;
            }
        }

        internal bool IsAbstract
        {
            get
            {
                // interfaces don't need to marked abstract explicitly (and javac 1.1 didn't do it)
                return (modifiers & (Modifiers.Abstract | Modifiers.Interface)) != 0;
            }
        }

        internal bool IsFinal
        {
            get
            {
                return (modifiers & Modifiers.Final) != 0;
            }
        }

        internal bool IsInterface
        {
            get
            {
                Debug.Assert(!IsUnloadable && !IsVerifierType);
                return (modifiers & Modifiers.Interface) != 0;
            }
        }

        // this exists because interfaces and arrays of interfaces are treated specially
        // by the verifier, interfaces don't have a common base (other than java.lang.Object)
        // so any object reference or object array reference can be used where an interface
        // or interface array reference is expected (the compiler will insert the required casts).
        internal bool IsInterfaceOrInterfaceArray
        {
            get
            {
                TypeWrapper tw = this;
                while (tw.IsArray)
                {
                    tw = tw.ElementTypeWrapper;
                }
                return tw.IsInterface;
            }
        }

        internal abstract ClassLoaderWrapper GetClassLoader();

        internal FieldWrapper GetFieldWrapper(string fieldName, string fieldSig)
        {
            foreach (FieldWrapper fw in GetFields())
            {
                if (fw.Name == fieldName && fw.Signature == fieldSig)
                {
                    return fw;
                }
            }
            foreach (TypeWrapper iface in this.Interfaces)
            {
                FieldWrapper fw = iface.GetFieldWrapper(fieldName, fieldSig);
                if (fw != null)
                {
                    return fw;
                }
            }
            TypeWrapper baseWrapper = this.BaseTypeWrapper;
            if (baseWrapper != null)
            {
                return baseWrapper.GetFieldWrapper(fieldName, fieldSig);
            }
            return null;
        }

        protected virtual void LazyPublishMembers()
        {
            if (methods == null)
            {
                methods = MethodWrapper.EmptyArray;
            }
            if (fields == null)
            {
                fields = FieldWrapper.EmptyArray;
            }
        }

        protected virtual void LazyPublishMethods()
        {
            LazyPublishMembers();
        }

        protected virtual void LazyPublishFields()
        {
            LazyPublishMembers();
        }

        internal MethodWrapper[] GetMethods()
        {
            if (methods == null)
            {
                lock (this)
                {
                    if (methods == null)
                    {
#if IMPORTER
                        if (IsUnloadable || !CheckMissingBaseTypes(TypeAsBaseType))
                        {
                            return methods = MethodWrapper.EmptyArray;
                        }
#endif
                        LazyPublishMethods();
                    }
                }
            }
            return methods;
        }

        internal FieldWrapper[] GetFields()
        {
            if (fields == null)
            {
                lock (this)
                {
                    if (fields == null)
                    {
#if IMPORTER
                        if (IsUnloadable || !CheckMissingBaseTypes(TypeAsBaseType))
                        {
                            return fields = FieldWrapper.EmptyArray;
                        }
#endif
                        LazyPublishFields();
                    }
                }
            }
            return fields;
        }

#if IMPORTER
        private static bool CheckMissingBaseTypes(Type type)
        {
            while (type != null)
            {
                if (type.__ContainsMissingType)
                {
                    StaticCompiler.IssueMissingTypeMessage(type);
                    return false;
                }
                bool ok = true;
                foreach (Type iface in type.__GetDeclaredInterfaces())
                {
                    ok &= CheckMissingBaseTypes(iface);
                }
                if (!ok)
                {
                    return false;
                }
                type = type.BaseType;
            }
            return true;
        }
#endif

        internal MethodWrapper GetMethodWrapper(string name, string sig, bool inherit)
        {
            // we need to get the methods before calling string.IsInterned, because getting them might cause the strings to be interned
            var methods = GetMethods();

            var _name = string.IsInterned(name);
            var _sig = string.IsInterned(sig);
            foreach (var mw in methods)
            {
                // NOTE we can use ref equality, because names and signatures are always interned by MemberWrapper
                if (ReferenceEquals(mw.Name, _name) && ReferenceEquals(mw.Signature, _sig))
                    return mw;
            }

            var baseWrapper = BaseTypeWrapper;
            if (inherit && baseWrapper != null)
                return baseWrapper.GetMethodWrapper(name, sig, inherit);

            return null;
        }

        internal MethodWrapper GetInterfaceMethod(string name, string sig)
        {
            MethodWrapper method = GetMethodWrapper(name, sig, false);
            if (method != null)
            {
                return method;
            }
            TypeWrapper[] interfaces = Interfaces;
            for (int i = 0; i < interfaces.Length; i++)
            {
                method = interfaces[i].GetInterfaceMethod(name, sig);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        internal void SetMethods(MethodWrapper[] methods)
        {
            Debug.Assert(methods != null);
            System.Threading.Thread.MemoryBarrier();
            this.methods = methods;
        }

        internal void SetFields(FieldWrapper[] fields)
        {
            Debug.Assert(fields != null);
            System.Threading.Thread.MemoryBarrier();
            this.fields = fields;
        }

        internal string Name => name;

        /// <summary>
        /// The name of the type as it appears in a Java signature string (e.g. "Ljava.lang.Object;" or "I").
        /// </summary>
        internal virtual string SigName => "L" + Name + ";";

        // returns true iff wrapper is allowed to access us
        internal bool IsAccessibleFrom(TypeWrapper wrapper)
        {
            return IsPublic
                || (IsInternal && InternalsVisibleTo(wrapper))
                || IsPackageAccessibleFrom(wrapper);
        }

        internal bool InternalsVisibleTo(TypeWrapper wrapper)
        {
            return GetClassLoader().InternalsVisibleToImpl(this, wrapper);
        }

        internal virtual bool IsPackageAccessibleFrom(TypeWrapper wrapper)
        {
            if (MatchingPackageNames(name, wrapper.name))
            {
#if IMPORTER
                CompilerClassLoader ccl = GetClassLoader() as CompilerClassLoader;
                if (ccl != null)
                {
                    // this is a hack for multi target -sharedclassloader compilation
                    // (during compilation we have multiple CompilerClassLoader instances to represent the single shared runtime class loader)
                    return ccl.IsEquivalentTo(wrapper.GetClassLoader());
                }
#endif
                return GetClassLoader() == wrapper.GetClassLoader();
            }
            else
            {
                return false;
            }
        }

        private static bool MatchingPackageNames(string name1, string name2)
        {
            int index1 = name1.LastIndexOf('.');
            int index2 = name2.LastIndexOf('.');
            if (index1 == -1 && index2 == -1)
            {
                return true;
            }
            // for array types we need to skip the brackets
            int skip1 = 0;
            int skip2 = 0;
            while (name1[skip1] == '[')
            {
                skip1++;
            }
            while (name2[skip2] == '[')
            {
                skip2++;
            }
            if (skip1 > 0)
            {
                // skip over the L that follows the brackets
                skip1++;
            }
            if (skip2 > 0)
            {
                // skip over the L that follows the brackets
                skip2++;
            }
            if ((index1 - skip1) != (index2 - skip2))
            {
                return false;
            }
            return String.CompareOrdinal(name1, skip1, name2, skip2, index1 - skip1) == 0;
        }

        internal abstract Type TypeAsTBD
        {
            get;
        }

        internal Type TypeAsSignatureType
        {
            get
            {
                if (IsUnloadable)
                {
                    return ((UnloadableTypeWrapper)this).MissingType ?? Types.Object;
                }
                if (IsGhostArray)
                {
                    return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
                }
                return TypeAsTBD;
            }
        }

        internal Type TypeAsPublicSignatureType
        {
            get
            {
                return (IsPublic ? this : GetPublicBaseTypeWrapper()).TypeAsSignatureType;
            }
        }

        internal virtual Type TypeAsBaseType
        {
            get
            {
                return TypeAsTBD;
            }
        }

        internal Type TypeAsLocalOrStackType
        {
            get
            {
                if (IsUnloadable || IsGhost)
                {
                    return Types.Object;
                }
                if (IsNonPrimitiveValueType)
                {
                    // return either System.ValueType or System.Enum
                    return TypeAsTBD.BaseType;
                }
                if (IsGhostArray)
                {
                    return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
                }
                return TypeAsTBD;
            }
        }

        /** <summary>Use this if the type is used as an array or array element</summary> */
        internal Type TypeAsArrayType
        {
            get
            {
                if (IsUnloadable || IsGhost)
                {
                    return Types.Object;
                }
                if (IsGhostArray)
                {
                    return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
                }
                return TypeAsTBD;
            }
        }

        internal Type TypeAsExceptionType
        {
            get
            {
                if (IsUnloadable)
                {
                    return Types.Exception;
                }
                return TypeAsTBD;
            }
        }

        internal abstract TypeWrapper BaseTypeWrapper
        {
            get;
        }

        internal TypeWrapper ElementTypeWrapper
        {
            get
            {
                Debug.Assert(!this.IsUnloadable);
                Debug.Assert(this == VerifierTypeWrapper.Null || this.IsArray);

                if (this == VerifierTypeWrapper.Null)
                {
                    return VerifierTypeWrapper.Null;
                }

                // TODO consider caching the element type
                switch (name[1])
                {
                    case '[':
                        // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
                        // (because the ultimate element type was already loaded when this type was created)
                        return GetClassLoader().LoadClassByDottedNameFast(name.Substring(1));
                    case 'L':
                        // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
                        // (because the ultimate element type was already loaded when this type was created)
                        return GetClassLoader().LoadClassByDottedNameFast(name.Substring(2, name.Length - 3));
                    case 'Z':
                        return PrimitiveTypeWrapper.BOOLEAN;
                    case 'B':
                        return PrimitiveTypeWrapper.BYTE;
                    case 'S':
                        return PrimitiveTypeWrapper.SHORT;
                    case 'C':
                        return PrimitiveTypeWrapper.CHAR;
                    case 'I':
                        return PrimitiveTypeWrapper.INT;
                    case 'J':
                        return PrimitiveTypeWrapper.LONG;
                    case 'F':
                        return PrimitiveTypeWrapper.FLOAT;
                    case 'D':
                        return PrimitiveTypeWrapper.DOUBLE;
                    default:
                        throw new InvalidOperationException(name);
                }
            }
        }

        internal TypeWrapper MakeArrayType(int rank)
        {
            Debug.Assert(rank != 0);
            // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
            return GetClassLoader().LoadClassByDottedNameFast(new String('[', rank) + this.SigName);
        }

        internal bool ImplementsInterface(TypeWrapper interfaceWrapper)
        {
            TypeWrapper typeWrapper = this;
            while (typeWrapper != null)
            {
                TypeWrapper[] interfaces = typeWrapper.Interfaces;
                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (interfaces[i] == interfaceWrapper)
                    {
                        return true;
                    }
                    if (interfaces[i].ImplementsInterface(interfaceWrapper))
                    {
                        return true;
                    }
                }
                typeWrapper = typeWrapper.BaseTypeWrapper;
            }
            return false;
        }

        internal bool IsSubTypeOf(TypeWrapper baseType)
        {
            // make sure IsSubTypeOf isn't used on primitives
            Debug.Assert(!this.IsPrimitive);
            Debug.Assert(!baseType.IsPrimitive);
            // can't be used on Unloadable
            Debug.Assert(!this.IsUnloadable);
            Debug.Assert(!baseType.IsUnloadable);

            if (baseType.IsInterface)
            {
                if (baseType == this)
                {
                    return true;
                }
                return ImplementsInterface(baseType);
            }
            // NOTE this isn't just an optimization, it is also required when this is an interface
            if (baseType == CoreClasses.java.lang.Object.Wrapper)
            {
                return true;
            }
            TypeWrapper subType = this;
            while (subType != baseType)
            {
                subType = subType.BaseTypeWrapper;
                if (subType == null)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool IsAssignableTo(TypeWrapper wrapper)
        {
            if (this == wrapper)
            {
                return true;
            }
            if (this.IsPrimitive || wrapper.IsPrimitive)
            {
                return false;
            }
            if (this == VerifierTypeWrapper.Null)
            {
                return true;
            }
            if (wrapper.IsInterface)
            {
                return ImplementsInterface(wrapper);
            }
            int rank1 = this.ArrayRank;
            int rank2 = wrapper.ArrayRank;
            if (rank1 > 0 && rank2 > 0)
            {
                rank1--;
                rank2--;
                TypeWrapper elem1 = this.ElementTypeWrapper;
                TypeWrapper elem2 = wrapper.ElementTypeWrapper;
                while (rank1 != 0 && rank2 != 0)
                {
                    elem1 = elem1.ElementTypeWrapper;
                    elem2 = elem2.ElementTypeWrapper;
                    rank1--;
                    rank2--;
                }
                if (elem1.IsPrimitive || elem2.IsPrimitive)
                {
                    return false;
                }
                return (!elem1.IsNonPrimitiveValueType && elem1.IsSubTypeOf(elem2));
            }
            return this.IsSubTypeOf(wrapper);
        }

#if !IMPORTER && !EXPORTER
        internal bool IsInstance(object obj)
        {
            if (obj != null)
            {
                TypeWrapper thisWrapper = this;
                TypeWrapper objWrapper = IKVM.Java.Externs.ikvm.runtime.Util.GetTypeWrapperFromObject(obj);
                return objWrapper.IsAssignableTo(thisWrapper);
            }
            return false;
        }
#endif

        internal virtual TypeWrapper[] Interfaces
        {
            get { return EmptyArray; }
        }

        // NOTE this property can only be called for finished types!
        internal virtual TypeWrapper[] InnerClasses
        {
            get { return EmptyArray; }
        }

        // NOTE this property can only be called for finished types!
        internal virtual TypeWrapper DeclaringTypeWrapper
        {
            get { return null; }
        }

        internal virtual void Finish()
        {
        }

        internal void LinkAll()
        {
            if ((flags & TypeFlags.Linked) == 0)
            {
                TypeWrapper tw = BaseTypeWrapper;
                if (tw != null)
                {
                    tw.LinkAll();
                }
                foreach (TypeWrapper iface in Interfaces)
                {
                    iface.LinkAll();
                }
                foreach (MethodWrapper mw in GetMethods())
                {
                    mw.Link();
                }
                foreach (FieldWrapper fw in GetFields())
                {
                    fw.Link();
                }
                SetTypeFlag(TypeFlags.Linked);
            }
        }

#if !IMPORTER
        [Conditional("DEBUG")]
        internal static void AssertFinished(Type type)
        {
            if (type != null)
            {
                while (type.HasElementType)
                {
                    type = type.GetElementType();
                }
                Debug.Assert(!(type is TypeBuilder));
            }
        }
#endif

#if !IMPORTER && !EXPORTER
        internal void RunClassInit()
        {
            Type t = IsRemapped ? TypeAsBaseType : TypeAsTBD;
            if (t != null)
            {
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.TypeHandle);
            }
        }
#endif

#if EMITTERS
        internal void EmitUnbox(CodeEmitter ilgen)
        {
            Debug.Assert(this.IsNonPrimitiveValueType);

            ilgen.EmitUnboxSpecial(this.TypeAsTBD);
        }

        internal void EmitBox(CodeEmitter ilgen)
        {
            Debug.Assert(this.IsNonPrimitiveValueType);

            ilgen.Emit(OpCodes.Box, this.TypeAsTBD);
        }

        internal void EmitConvSignatureTypeToStackType(CodeEmitter ilgen)
        {
            if (IsUnloadable)
            {
            }
            else if (this == PrimitiveTypeWrapper.BYTE)
            {
                ilgen.Emit(OpCodes.Conv_I1);
            }
            else if (IsNonPrimitiveValueType)
            {
                EmitBox(ilgen);
            }
            else if (IsGhost)
            {
                CodeEmitterLocal local = ilgen.DeclareLocal(TypeAsSignatureType);
                ilgen.Emit(OpCodes.Stloc, local);
                ilgen.Emit(OpCodes.Ldloca, local);
                ilgen.Emit(OpCodes.Ldfld, GhostRefField);
            }
        }

        // NOTE sourceType is optional and only used for interfaces,
        // it is *not* used to automatically downcast
        internal void EmitConvStackTypeToSignatureType(CodeEmitter ilgen, TypeWrapper sourceType)
        {
            if (!IsUnloadable)
            {
                if (IsGhost)
                {
                    CodeEmitterLocal local1 = ilgen.DeclareLocal(TypeAsLocalOrStackType);
                    ilgen.Emit(OpCodes.Stloc, local1);
                    CodeEmitterLocal local2 = ilgen.DeclareLocal(TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Ldloca, local2);
                    ilgen.Emit(OpCodes.Ldloc, local1);
                    ilgen.Emit(OpCodes.Stfld, GhostRefField);
                    ilgen.Emit(OpCodes.Ldloca, local2);
                    ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);
                }
                // because of the way interface merging works, any reference is valid
                // for any interface reference
                else if (IsInterfaceOrInterfaceArray && (sourceType == null || sourceType.IsUnloadable || !sourceType.IsAssignableTo(this)))
                {
                    ilgen.EmitAssertType(TypeAsTBD);
                    Profiler.Count("InterfaceDownCast");
                }
                else if (IsNonPrimitiveValueType)
                {
                    EmitUnbox(ilgen);
                }
                else if (sourceType != null && sourceType.IsUnloadable)
                {
                    ilgen.Emit(OpCodes.Castclass, TypeAsSignatureType);
                }
            }
        }

        internal virtual void EmitCheckcast(CodeEmitter ilgen)
        {
            if (IsGhost)
            {
                ilgen.Emit(OpCodes.Dup);
                // TODO make sure we get the right "Cast" method and cache it
                // NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("Cast"));
                ilgen.Emit(OpCodes.Pop);
            }
            else if (IsGhostArray)
            {
                ilgen.Emit(OpCodes.Dup);
                // TODO make sure we get the right "CastArray" method and cache it
                // NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                TypeWrapper tw = this;
                int rank = 0;
                while (tw.IsArray)
                {
                    rank++;
                    tw = tw.ElementTypeWrapper;
                }
                ilgen.EmitLdc_I4(rank);
                ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("CastArray"));
                ilgen.Emit(OpCodes.Castclass, ArrayTypeWrapper.MakeArrayType(Types.Object, rank));
            }
            else
            {
                ilgen.EmitCastclass(TypeAsTBD);
            }
        }

        internal virtual void EmitInstanceOf(CodeEmitter ilgen)
        {
            if (IsGhost)
            {
                // TODO make sure we get the right "IsInstance" method and cache it
                // NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("IsInstance"));
            }
            else if (IsGhostArray)
            {
                // TODO make sure we get the right "IsInstanceArray" method and cache it
                // NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                TypeWrapper tw = this;
                int rank = 0;
                while (tw.IsArray)
                {
                    rank++;
                    tw = tw.ElementTypeWrapper;
                }
                ilgen.EmitLdc_I4(rank);
                ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("IsInstanceArray"));
            }
            else
            {
                ilgen.Emit_instanceof(TypeAsTBD);
            }
        }
#endif // EMITTERS

        // NOTE don't call this method, call MethodWrapper.Link instead
        internal virtual MethodBase LinkMethod(MethodWrapper mw)
        {
            return mw.GetMethod();
        }

        // NOTE don't call this method, call FieldWrapper.Link instead
        internal virtual FieldInfo LinkField(FieldWrapper fw)
        {
            return fw.GetField();
        }

#if EMITTERS
        internal virtual void EmitRunClassConstructor(CodeEmitter ilgen)
        {
        }
#endif // EMITTERS

        internal virtual string GetGenericSignature()
        {
            return null;
        }

        internal virtual string GetGenericMethodSignature(MethodWrapper mw)
        {
            return null;
        }

        internal virtual string GetGenericFieldSignature(FieldWrapper fw)
        {
            return null;
        }

        internal virtual MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
        {
            return null;
        }

#if !IMPORTER && !EXPORTER
        internal virtual string[] GetEnclosingMethod()
        {
            return null;
        }

        internal virtual object[] GetDeclaredAnnotations()
        {
            return null;
        }

        internal virtual object[] GetMethodAnnotations(MethodWrapper mw)
        {
            return null;
        }

        internal virtual object[][] GetParameterAnnotations(MethodWrapper mw)
        {
            return null;
        }

        internal virtual object[] GetFieldAnnotations(FieldWrapper fw)
        {
            return null;
        }

        internal virtual string GetSourceFileName()
        {
            return null;
        }

        internal virtual int GetSourceLineNumber(MethodBase mb, int ilOffset)
        {
            return -1;
        }

        internal virtual object GetAnnotationDefault(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb != null)
            {
                object[] attr = mb.GetCustomAttributes(typeof(AnnotationDefaultAttribute), false);
                if (attr.Length == 1)
                {
                    return JVM.NewAnnotationElementValue(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), mw.ReturnType.ClassObject, ((AnnotationDefaultAttribute)attr[0]).Value);
                }
            }
            return null;
        }
#endif // !IMPORTER && !EXPORTER

        internal virtual Annotation Annotation
        {
            get
            {
                return null;
            }
        }

        internal virtual Type EnumType
        {
            get
            {
                return null;
            }
        }

        private static Type[] GetInterfaces(Type type)
        {
#if IMPORTER || EXPORTER
            List<Type> list = new List<Type>();
            for (; type != null && !type.__IsMissing; type = type.BaseType)
            {
                AddInterfaces(list, type);
            }
            return list.ToArray();
#else
            return type.GetInterfaces();
#endif
        }

#if IMPORTER || EXPORTER
        private static void AddInterfaces(List<Type> list, Type type)
        {
            foreach (Type iface in type.__GetDeclaredInterfaces())
            {
                if (!list.Contains(iface))
                {
                    list.Add(iface);
                    if (!iface.__IsMissing)
                    {
                        AddInterfaces(list, iface);
                    }
                }
            }
        }
#endif

        protected static TypeWrapper[] GetImplementedInterfacesAsTypeWrappers(Type type)
        {
            Type[] interfaceTypes = GetInterfaces(type);
            TypeWrapper[] interfaces = new TypeWrapper[interfaceTypes.Length];
            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                Type decl = interfaceTypes[i].DeclaringType;
                if (decl != null && AttributeHelper.IsGhostInterface(decl))
                {
                    // we have to return the declaring type for ghost interfaces
                    interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(decl);
                }
                else
                {
                    interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(interfaceTypes[i]);
                }
            }
            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                if (interfaces[i].IsRemapped)
                {
                    // for remapped interfaces, we also return the original interface (Java types will ignore it, if it isn't listed in the ImplementsAttribute)
                    TypeWrapper twRemapped = interfaces[i];
                    TypeWrapper tw = DotNetTypeWrapper.GetWrapperFromDotNetType(interfaceTypes[i]);
                    interfaces[i] = tw;
                    if (Array.IndexOf(interfaces, twRemapped) == -1)
                    {
                        interfaces = ArrayUtil.Concat(interfaces, twRemapped);
                    }
                }
            }
            return interfaces;
        }

        internal TypeWrapper GetPublicBaseTypeWrapper()
        {
            Debug.Assert(!this.IsPublic);
            if (this.IsUnloadable || this.IsInterface)
            {
                return CoreClasses.java.lang.Object.Wrapper;
            }
            for (TypeWrapper tw = this; ; tw = tw.BaseTypeWrapper)
            {
                if (tw.IsPublic)
                {
                    return tw;
                }
            }
        }

#if !EXPORTER
        // return the constructor used for automagic .NET serialization
        internal virtual MethodBase GetSerializationConstructor()
        {
            return this.TypeAsBaseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] {
                        JVM.Import(typeof(System.Runtime.Serialization.SerializationInfo)), JVM.Import(typeof(System.Runtime.Serialization.StreamingContext)) }, null);
        }

        internal virtual MethodBase GetBaseSerializationConstructor()
        {
            return BaseTypeWrapper.GetSerializationConstructor();
        }
#endif

#if !IMPORTER && !EXPORTER
        internal virtual object GhostWrap(object obj)
        {
            return obj;
        }

        internal virtual object GhostUnwrap(object obj)
        {
            return obj;
        }
#endif

        internal bool IsDynamic
        {
#if EXPORTER
            get { return false; }
#else
            get { return this is DynamicTypeWrapper; }
#endif
        }

        internal virtual object[] GetConstantPool()
        {
            return null;
        }

        internal virtual byte[] GetRawTypeAnnotations()
        {
            return null;
        }

        internal virtual byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
        {
            return null;
        }

        internal virtual byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
        {
            return null;
        }

#if !IMPORTER && !EXPORTER
        internal virtual TypeWrapper Host
        {
            get { return null; }
        }
#endif
    }

}
