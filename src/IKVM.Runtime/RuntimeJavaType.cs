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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.Attributes;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Encapsulates the information about a type available to Java.
    /// </summary>
    internal abstract class RuntimeJavaType
    {

        internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
        internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

        static readonly object flagsLock = new object();

        readonly RuntimeContext context;
        readonly string name; // java name (e.g. java.lang.Object)
        readonly Modifiers modifiers;
        TypeFlags flags;
        RuntimeJavaMethod[] methods;
        RuntimeJavaField[] fields;
#if !IMPORTER && !EXPORTER
        java.lang.Class classObject;
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flags"></param>
        /// <param name="modifiers"></param>
        /// <param name="name"></param>
        /// <exception cref="InternalException"></exception>
        internal RuntimeJavaType(RuntimeContext context, TypeFlags flags, Modifiers modifiers, string name)
        {
            Profiler.Count("TypeWrapper");

            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.flags = flags;
            this.modifiers = modifiers;
            this.name = name == null ? null : string.Intern(name);
        }

        /// <summary>
        /// Gets a reference to the <see cref="RuntimeContext"/> this <see cref="RuntimeJavaType"/> is hosted within.
        /// </summary>
        public RuntimeContext Context => context;

        /// <summary>
        /// Gets the <see cref="IDiagnosticHandler"/> where events related to this type should be sent.
        /// </summary>
        public virtual IDiagnosticHandler Diagnostics => Context.Diagnostics;

#if EMITTERS

        internal void EmitClassLiteral(CodeEmitter ilgen)
        {
            Debug.Assert(!this.IsPrimitive);

            var type = GetClassLiteralType();

            // note that this has to be the same check as in LazyInitClass
            if (!this.IsFastClassLiteralSafe || IsForbiddenTypeParameterType(type))
            {
                int rank = 0;
                while (type.IsSZArray)
                {
                    rank++;
                    type = type.GetElementType();
                }

                if (rank == 0)
                {
                    ilgen.Emit(OpCodes.Ldtoken, type);
                    context.CompilerFactory.GetClassFromTypeHandle.EmitCall(ilgen);
                }
                else
                {
                    ilgen.Emit(OpCodes.Ldtoken, type);
                    ilgen.EmitLdc_I4(rank);
                    context.CompilerFactory.GetClassFromTypeHandle2.EmitCall(ilgen);
                }
            }
            else
            {
                var classLiteralType = Context.Resolver.ResolveRuntimeType("IKVM.Runtime.ClassLiteral`1").MakeGenericType(type);
                ilgen.Emit(OpCodes.Call, classLiteralType.GetProperty("Value").GetMethod);
            }
        }

#endif

        ITypeSymbol GetClassLiteralType()
        {
            Debug.Assert(IsPrimitive == false);

            var tw = this;
            if (tw.IsGhostArray)
            {
                var rank = tw.ArrayRank;
                while (tw.IsArray)
                    tw = tw.ElementTypeWrapper;

                return RuntimeArrayJavaType.MakeArrayType(tw.TypeAsTBD, rank);
            }
            else
            {
                return tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD;
            }
        }

        bool IsForbiddenTypeParameterType(ITypeSymbol type)
        {
            // these are the types that may not be used as a type argument when instantiating a generic type
            return type == context.Types.Void
                || type == context.Resolver.ResolveCoreType(typeof(ArgIterator).FullName)
                || type == context.Resolver.ResolveCoreType(typeof(RuntimeArgumentHandle).FullName)
                || type == context.Resolver.ResolveCoreType(typeof(TypedReference).FullName)
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
            if (this == context.PrimitiveJavaTypeFactory.BYTE)
            {
                return java.lang.Byte.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.CHAR)
            {
                return java.lang.Character.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                return java.lang.Double.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.FLOAT)
            {
                return java.lang.Float.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.INT)
            {
                return java.lang.Integer.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.LONG)
            {
                return java.lang.Long.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.SHORT)
            {
                return java.lang.Short.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                return java.lang.Boolean.TYPE;
            }
            else if (this == context.PrimitiveJavaTypeFactory.VOID)
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
                    if (!IsFastClassLiteralSafe)
                    {
                        if (IsPrimitive)
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
                        var type = GetClassLiteralType();
                        if (IsForbiddenTypeParameterType(type))
                            clazz = new java.lang.Class(type.AsReflection());
                        else
                            clazz = (java.lang.Class)typeof(ClassLiteral<>).MakeGenericType(type.AsReflection()).GetProperty("Value").GetGetMethod().Invoke(null, []);
                    }

                    clazz.typeWrapper = this;

                    System.Threading.Interlocked.CompareExchange(ref classObject, clazz, null);
#endif
                }
            }
        }

#if !FIRST_PASS

        static void ResolvePrimitiveTypeWrapperClasses(RuntimeContext context)
        {
            // note that we're evaluating all ClassObject properties for the side effect
            // (to initialize and associate the ClassObject with the TypeWrapper)
            if (context.PrimitiveJavaTypeFactory.BYTE.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.CHAR.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.DOUBLE.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.FLOAT.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.INT.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.LONG.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.SHORT.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.BOOLEAN.ClassObject == null ||
                context.PrimitiveJavaTypeFactory.VOID.ClassObject == null)
                throw new InvalidOperationException();
        }
#endif

        internal static RuntimeJavaType FromClass(java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // MONOBUG redundant cast to workaround mcs bug
            var tw = (RuntimeJavaType)(object)clazz.typeWrapper;
            if (tw == null)
            {
                var type = clazz.type;
                if (type == null)
                {
                    ResolvePrimitiveTypeWrapperClasses(JVM.Context);
                    return FromClass(clazz);
                }

                var symbol = JVM.Context.Resolver.GetSymbol(type);
                if (type == typeof(void) || type.IsPrimitive || JVM.Context.ClassLoaderFactory.IsRemappedType(symbol))
                    tw = JVM.Context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(symbol);
                else
                    tw = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(symbol);

                clazz.typeWrapper = tw;
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
        internal virtual RuntimeJavaType EnsureLoadable(RuntimeClassLoader loader)
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
                RuntimeJavaType baseWrapper = this.BaseTypeWrapper;
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
        internal bool IsGhostArray => !IsUnloadable && IsArray && (ElementTypeWrapper.IsGhost || ElementTypeWrapper.IsGhostArray);

        internal virtual IFieldSymbol GhostRefField => throw new InvalidOperationException();

        internal virtual bool IsRemapped => false;

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

        internal virtual RuntimeJavaType GetUltimateElementTypeWrapper()
        {
            throw new InvalidOperationException();
        }

        internal bool IsNonPrimitiveValueType
        {
            get
            {
                return this != context.VerifierJavaTypeFactory.Null && !IsPrimitive && !IsGhost && TypeAsTBD.IsValueType;
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
                return this == context.PrimitiveJavaTypeFactory.LONG || this == context.PrimitiveJavaTypeFactory.DOUBLE;
            }
        }

        internal bool IsIntOnStackPrimitive
        {
            get
            {
                return name == null &&
                    (this == context.PrimitiveJavaTypeFactory.BOOLEAN ||
                    this == context.PrimitiveJavaTypeFactory.BYTE ||
                    this == context.PrimitiveJavaTypeFactory.CHAR ||
                    this == context.PrimitiveJavaTypeFactory.SHORT ||
                    this == context.PrimitiveJavaTypeFactory.INT);
            }
        }

        private static bool IsJavaPrimitive(RuntimeContext context, ITypeSymbol type)
        {
            return type == context.PrimitiveJavaTypeFactory.BOOLEAN.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.BYTE.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.CHAR.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.DOUBLE.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.FLOAT.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.INT.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.LONG.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.SHORT.TypeAsTBD
                || type == context.PrimitiveJavaTypeFactory.VOID.TypeAsTBD;
        }

        internal bool IsBoxedPrimitive
        {
            get
            {
                return !IsPrimitive && IsJavaPrimitive(Context, TypeAsSignatureType);
            }
        }

        internal bool IsErasedOrBoxedPrimitiveOrRemapped
        {
            get
            {
                bool erased = IsUnloadable || IsGhostArray;
                return erased || IsBoxedPrimitive || (IsRemapped && this is RuntimeManagedJavaType);
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
                RuntimeJavaType tw = this;
                while (tw.IsArray)
                {
                    tw = tw.ElementTypeWrapper;
                }
                return tw.IsInterface;
            }
        }

        /// <summary>
        /// Gets the associated <see cref="RuntimeClassLoader"/>.
        /// </summary>
        internal abstract RuntimeClassLoader ClassLoader { get; }

        /// <summary>
        /// Searches for the <see cref="RuntimeJavaField"/> with the specified name and signature.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldSig"></param>
        /// <returns></returns>
        internal RuntimeJavaField GetFieldWrapper(string fieldName, string fieldSig)
        {
            foreach (var fw in GetFields())
                if (fw.Name == fieldName && fw.Signature == fieldSig)
                    return fw;

            foreach (var iface in Interfaces)
            {
                var fw = iface.GetFieldWrapper(fieldName, fieldSig);
                if (fw != null)
                    return fw;
            }

            var baseWrapper = BaseTypeWrapper;
            if (baseWrapper != null)
                return baseWrapper.GetFieldWrapper(fieldName, fieldSig);

            return null;
        }

        protected virtual void LazyPublishMembers()
        {
            methods ??= Array.Empty<RuntimeJavaMethod>();
            fields ??= Array.Empty<RuntimeJavaField>();
        }

        protected virtual void LazyPublishMethods()
        {
            LazyPublishMembers();
        }

        protected virtual void LazyPublishFields()
        {
            LazyPublishMembers();
        }

        /// <summary>
        /// Gets the set of methods defined by this type.
        /// </summary>
        /// <returns></returns>
        internal RuntimeJavaMethod[] GetMethods()
        {
            if (methods == null)
            {
                lock (this)
                {
                    if (methods == null)
                    {
#if IMPORTER
                        if (IsUnloadable || !CheckMissingBaseTypes(Context, TypeAsBaseType))
                            return methods = Array.Empty<RuntimeJavaMethod>();
#endif
                        LazyPublishMethods();
                    }
                }
            }

            return methods;
        }

        /// <summary>
        /// Gets the set of fields declared by this type.
        /// </summary>
        /// <returns></returns>
        internal RuntimeJavaField[] GetFields()
        {
            if (fields == null)
            {
                lock (this)
                {
                    if (fields == null)
                    {
#if IMPORTER
                        if (IsUnloadable || !CheckMissingBaseTypes(Context, TypeAsBaseType))
                            return fields = Array.Empty<RuntimeJavaField>();
#endif

                        LazyPublishFields();
                    }
                }
            }

            return fields;
        }

#if IMPORTER

        static bool CheckMissingBaseTypes(RuntimeContext context, ITypeSymbol type)
        {
            while (type != null)
            {
                if (type.ContainsMissing)
                {
                    context.StaticCompiler.IssueMissingTypeMessage(type);
                    return false;
                }

                var ok = true;
                foreach (var iface in type.GetInterfaces(false))
                    ok &= CheckMissingBaseTypes(context, iface);

                if (!ok)
                    return false;

                type = type.BaseType;
            }

            return true;
        }

#endif

        internal RuntimeJavaMethod GetMethodWrapper(string name, string sig, bool inherit)
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

        internal RuntimeJavaMethod GetInterfaceMethod(string name, string sig)
        {
            var method = GetMethodWrapper(name, sig, false);
            if (method != null)
            {
                return method;
            }

            var interfaces = Interfaces;
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

        internal void SetMethods(RuntimeJavaMethod[] methods)
        {
            Debug.Assert(methods != null);
            System.Threading.Thread.MemoryBarrier();
            this.methods = methods;
        }

        internal void SetFields(RuntimeJavaField[] fields)
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
        internal bool IsAccessibleFrom(RuntimeJavaType wrapper)
        {
            return IsPublic
                || (IsInternal && InternalsVisibleTo(wrapper))
                || IsPackageAccessibleFrom(wrapper);
        }

        internal bool InternalsVisibleTo(RuntimeJavaType wrapper)
        {
            return ClassLoader.InternalsVisibleToImpl(this, wrapper);
        }

        internal virtual bool IsPackageAccessibleFrom(RuntimeJavaType wrapper)
        {
            if (MatchingPackageNames(name, wrapper.name))
            {
#if IMPORTER
                ImportClassLoader ccl = ClassLoader as ImportClassLoader;
                if (ccl != null)
                {
                    // this is a hack for multi target -sharedclassloader compilation
                    // (during compilation we have multiple CompilerClassLoader instances to represent the single shared runtime class loader)
                    return ccl.IsEquivalentTo(wrapper.ClassLoader);
                }
#endif
                return ClassLoader == wrapper.ClassLoader;
            }
            else
            {
                return false;
            }
        }

        static bool MatchingPackageNames(string name1, string name2)
        {
            int index1 = name1.LastIndexOf('.');
            int index2 = name2.LastIndexOf('.');
            if (index1 == -1 && index2 == -1)
                return true;

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

            return string.CompareOrdinal(name1, skip1, name2, skip2, index1 - skip1) == 0;
        }

        internal abstract ITypeSymbol TypeAsTBD
        {
            get;
        }

        internal ITypeSymbol TypeAsSignatureType
        {
            get
            {
                if (IsUnloadable)
                    return ((RuntimeUnloadableJavaType)this).MissingType ?? context.Types.Object;

                if (IsGhostArray)
                    return RuntimeArrayJavaType.MakeArrayType(context.Types.Object, ArrayRank);

                return TypeAsTBD;
            }
        }

        internal ITypeSymbol TypeAsPublicSignatureType => (IsPublic ? this : GetPublicBaseTypeWrapper()).TypeAsSignatureType;

        internal virtual ITypeSymbol TypeAsBaseType => TypeAsTBD;

        internal ITypeSymbol TypeAsLocalOrStackType
        {
            get
            {
                if (IsUnloadable || IsGhost)
                    return context.Types.Object;

                if (IsNonPrimitiveValueType)
                {
                    // return either System.ValueType or System.Enum
                    return TypeAsTBD.BaseType;
                }

                if (IsGhostArray)
                    return RuntimeArrayJavaType.MakeArrayType(context.Types.Object, ArrayRank);

                return TypeAsTBD;
            }
        }

        /** <summary>Use this if the type is used as an array or array element</summary> */
        internal ITypeSymbol TypeAsArrayType
        {
            get
            {
                if (IsUnloadable || IsGhost)
                    return context.Types.Object;

                if (IsGhostArray)
                    return RuntimeArrayJavaType.MakeArrayType(context.Types.Object, ArrayRank);

                return TypeAsTBD;
            }
        }

        internal ITypeSymbol TypeAsExceptionType
        {
            get
            {
                if (IsUnloadable)
                    return context.Types.Exception;

                return TypeAsTBD;
            }
        }

        internal abstract RuntimeJavaType BaseTypeWrapper
        {
            get;
        }

        internal RuntimeJavaType ElementTypeWrapper
        {
            get
            {
                Debug.Assert(IsUnloadable == false);
                Debug.Assert(this == context.VerifierJavaTypeFactory.Null || IsArray);

                if (this == context.VerifierJavaTypeFactory.Null)
                {
                    return context.VerifierJavaTypeFactory.Null;
                }

                // TODO consider caching the element type
                switch (name[1])
                {
                    case '[':
                        // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
                        // (because the ultimate element type was already loaded when this type was created)
                        return ClassLoader.TryLoadClassByName(name.Substring(1));
                    case 'L':
                        // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
                        // (because the ultimate element type was already loaded when this type was created)
                        return ClassLoader.TryLoadClassByName(name.Substring(2, name.Length - 3));
                    case 'Z':
                        return context.PrimitiveJavaTypeFactory.BOOLEAN;
                    case 'B':
                        return context.PrimitiveJavaTypeFactory.BYTE;
                    case 'S':
                        return context.PrimitiveJavaTypeFactory.SHORT;
                    case 'C':
                        return context.PrimitiveJavaTypeFactory.CHAR;
                    case 'I':
                        return context.PrimitiveJavaTypeFactory.INT;
                    case 'J':
                        return context.PrimitiveJavaTypeFactory.LONG;
                    case 'F':
                        return context.PrimitiveJavaTypeFactory.FLOAT;
                    case 'D':
                        return context.PrimitiveJavaTypeFactory.DOUBLE;
                    default:
                        throw new InvalidOperationException(name);
                }
            }
        }

        internal RuntimeJavaType MakeArrayType(int rank)
        {
            Debug.Assert(rank != 0);

            // NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
            return ClassLoader.TryLoadClassByName(new string('[', rank) + SigName);
        }

        internal bool ImplementsInterface(RuntimeJavaType interfaceWrapper)
        {
            var typeWrapper = this;
            while (typeWrapper != null)
            {
                var interfaces = typeWrapper.Interfaces;
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

        internal bool IsSubTypeOf(RuntimeJavaType baseType)
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
            if (baseType == Context.JavaBase.TypeOfJavaLangObject)
            {
                return true;
            }
            RuntimeJavaType subType = this;
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

        internal bool IsAssignableTo(RuntimeJavaType wrapper)
        {
            if (this == wrapper)
            {
                return true;
            }
            if (this.IsPrimitive || wrapper.IsPrimitive)
            {
                return false;
            }
            if (this == context.VerifierJavaTypeFactory.Null)
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
                RuntimeJavaType elem1 = this.ElementTypeWrapper;
                RuntimeJavaType elem2 = wrapper.ElementTypeWrapper;
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
                RuntimeJavaType thisWrapper = this;
                RuntimeJavaType objWrapper = IKVM.Java.Externs.ikvm.runtime.Util.GetTypeWrapperFromObject(Context, obj);
                return objWrapper.IsAssignableTo(thisWrapper);
            }
            return false;
        }
#endif

        internal virtual RuntimeJavaType[] Interfaces => Array.Empty<RuntimeJavaType>();

        // NOTE this property can only be called for finished types!
        internal virtual RuntimeJavaType[] InnerClasses => Array.Empty<RuntimeJavaType>();

        // NOTE this property can only be called for finished types!
        internal virtual RuntimeJavaType DeclaringTypeWrapper => null;

        internal virtual void Finish()
        {

        }

        internal void LinkAll()
        {
            if ((flags & TypeFlags.Linked) == 0)
            {
                RuntimeJavaType tw = BaseTypeWrapper;
                if (tw != null)
                {
                    tw.LinkAll();
                }
                foreach (RuntimeJavaType iface in Interfaces)
                {
                    iface.LinkAll();
                }
                foreach (RuntimeJavaMethod mw in GetMethods())
                {
                    mw.Link();
                }
                foreach (RuntimeJavaField fw in GetFields())
                {
                    fw.Link();
                }
                SetTypeFlag(TypeFlags.Linked);
            }
        }

#if !IMPORTER

        [Conditional("DEBUG")]
        internal static void AssertFinished(ITypeSymbol type)
        {
            if (type != null)
            {
                while (type.HasElementType)
                    type = type.GetElementType();

                Debug.Assert(type.IsComplete);
            }
        }

#endif

#if !IMPORTER && !EXPORTER

        internal void RunClassInit()
        {
            var t = IsRemapped ? TypeAsBaseType : TypeAsTBD;
            if (t != null)
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.AsReflection().TypeHandle);
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

            ilgen.Emit(System.Reflection.Emit.OpCodes.Box, this.TypeAsTBD);
        }

        internal void EmitConvSignatureTypeToStackType(CodeEmitter ilgen)
        {
            if (IsUnloadable)
            {

            }
            else if (this == context.PrimitiveJavaTypeFactory.BYTE)
            {
                ilgen.Emit(System.Reflection.Emit.OpCodes.Conv_I1);
            }
            else if (IsNonPrimitiveValueType)
            {
                EmitBox(ilgen);
            }
            else if (IsGhost)
            {
                var local = ilgen.DeclareLocal(TypeAsSignatureType);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Stloc, local);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldloca, local);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldfld, GhostRefField);
            }
        }

        // NOTE sourceType is optional and only used for interfaces,
        // it is *not* used to automatically downcast
        internal void EmitConvStackTypeToSignatureType(CodeEmitter ilgen, RuntimeJavaType sourceType)
        {
            if (!IsUnloadable)
            {
                if (IsGhost)
                {
                    CodeEmitterLocal local1 = ilgen.DeclareLocal(TypeAsLocalOrStackType);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Stloc, local1);
                    CodeEmitterLocal local2 = ilgen.DeclareLocal(TypeAsSignatureType);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Ldloca, local2);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Ldloc, local1);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Stfld, GhostRefField);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Ldloca, local2);
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Ldobj, TypeAsSignatureType);
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
                    ilgen.Emit(System.Reflection.Emit.OpCodes.Castclass, TypeAsSignatureType);
                }
            }
        }

        internal virtual void EmitCheckcast(CodeEmitter ilgen)
        {
            if (IsGhost)
            {
                ilgen.Emit(System.Reflection.Emit.OpCodes.Dup);
                // TODO make sure we get the right "Cast" method and cache it
                // NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, TypeAsTBD.GetMethod("Cast"));
                ilgen.Emit(System.Reflection.Emit.OpCodes.Pop);
            }
            else if (IsGhostArray)
            {
                ilgen.Emit(System.Reflection.Emit.OpCodes.Dup);

                // TODO make sure we get the right "CastArray" method and cache it
                // NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                var tw = this;
                int rank = 0;
                while (tw.IsArray)
                {
                    rank++;
                    tw = tw.ElementTypeWrapper;
                }

                ilgen.EmitLdc_I4(rank);
                ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("CastArray"));
                ilgen.Emit(OpCodes.Castclass, RuntimeArrayJavaType.MakeArrayType(context.Types.Object, rank));
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
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, TypeAsTBD.GetMethod("IsInstance"));
            }
            else if (IsGhostArray)
            {
                // TODO make sure we get the right "IsInstanceArray" method and cache it
                // NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
                // so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
                RuntimeJavaType tw = this;
                int rank = 0;
                while (tw.IsArray)
                {
                    rank++;
                    tw = tw.ElementTypeWrapper;
                }
                ilgen.EmitLdc_I4(rank);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, tw.TypeAsTBD.GetMethod("IsInstanceArray"));
            }
            else
            {
                ilgen.Emit_instanceof(TypeAsTBD);
            }
        }

        /// <summary>
        /// Emits the appropriate instruction to do an indirect load of a reference to this type.
        /// </summary>
        /// <param name="il"></param>
        internal virtual void EmitLdind(CodeEmitter il)
        {
            if (this == context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_U1);
            else if (this == context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_U1);
            else if (this == context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_U2);
            else if (this == context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_I2);
            else if (this == context.PrimitiveJavaTypeFactory.INT)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_I4);
            else if (this == context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_I8);
            else if (this == context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_R4);
            else if (this == context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_R8);
            else
                il.Emit(System.Reflection.Emit.OpCodes.Ldind_Ref);
        }

        /// <summary>
        /// Emits the appropriate instruction to do an indirect store of a reference to this type.
        /// </summary>
        /// <param name="il"></param>
        internal virtual void EmitStind(CodeEmitter il)
        {
            if (this == context.PrimitiveJavaTypeFactory.BOOLEAN)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I1);
            else if (this == context.PrimitiveJavaTypeFactory.BYTE)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I1);
            else if (this == context.PrimitiveJavaTypeFactory.CHAR)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I2);
            else if (this == context.PrimitiveJavaTypeFactory.SHORT)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I2);
            else if (this == context.PrimitiveJavaTypeFactory.INT)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I4);
            else if (this == context.PrimitiveJavaTypeFactory.LONG)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_I8);
            else if (this == context.PrimitiveJavaTypeFactory.FLOAT)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_R4);
            else if (this == context.PrimitiveJavaTypeFactory.DOUBLE)
                il.Emit(System.Reflection.Emit.OpCodes.Stind_R8);
            else
                il.Emit(System.Reflection.Emit.OpCodes.Stind_Ref);
        }

#endif

        // NOTE don't call this method, call MethodWrapper.Link instead
        internal virtual IMethodBaseSymbol LinkMethod(RuntimeJavaMethod mw)
        {
            return mw.GetMethod();
        }

        // NOTE don't call this method, call FieldWrapper.Link instead
        internal virtual IFieldSymbol LinkField(RuntimeJavaField fw)
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

        internal virtual string GetGenericMethodSignature(RuntimeJavaMethod mw)
        {
            return null;
        }

        internal virtual string GetGenericFieldSignature(RuntimeJavaField fw)
        {
            return null;
        }

        internal virtual MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
        {
            return null;
        }

#if !IMPORTER && !EXPORTER

        internal virtual string[] GetEnclosingMethod()
        {
            return null;
        }

        /// <summary>
        /// Gets the annotations declared on this type. Expected to return actual runtime instances.
        /// </summary>
        /// <returns></returns>
        internal virtual object[] GetDeclaredAnnotations()
        {
            return null;
        }

        /// <summary>
        /// Gets the annotations declared on the specified method of this type. Expected to return actual runtime instances.
        /// </summary>
        /// <param name="mw"></param>
        /// <returns></returns>
        internal virtual object[] GetMethodAnnotations(RuntimeJavaMethod mw)
        {
            return null;
        }

        /// <summary>
        /// Gets the annotations declared on the specified parameters of the specified method of this type. Expected to return actual runtime instances.
        /// </summary>
        /// <param name="mw"></param>
        /// <returns></returns>
        internal virtual object[][] GetParameterAnnotations(RuntimeJavaMethod mw)
        {
            return null;
        }

        /// <summary>
        /// Gets the annotations declared on the specified fieldof this type. Expected to return actual runtime instances.
        /// </summary>
        /// <param name="fw"></param>
        /// <returns></returns>
        internal virtual object[] GetFieldAnnotations(RuntimeJavaField fw)
        {
            return null;
        }

        internal virtual string GetSourceFileName()
        {
            return null;
        }

        internal virtual int GetSourceLineNumber(IMethodBaseSymbol mb, int ilOffset)
        {
            return -1;
        }

        internal virtual object GetAnnotationDefault(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb != null)
            {
                var attr = mb.AsReflection().GetCustomAttribute<AnnotationDefaultAttribute>();
                if (attr != null)
                    return JVM.NewAnnotationElementValue(mw.DeclaringType.ClassLoader.GetJavaClassLoader(), mw.ReturnType.ClassObject, attr.Value);
            }

            return null;
        }

#endif // !IMPORTER && !EXPORTER

        internal virtual Annotation Annotation => null;

        internal virtual ITypeSymbol EnumType => null;

        static ITypeSymbol[] GetInterfaces(ITypeSymbol type)
        {
            var ifaces = type.GetInterfaces();
            if (ifaces.Any(i => i.IsMissing))
                ifaces = ifaces.Where(i => i.IsMissing == false).ToArray();

            return ifaces;
        }

        protected static RuntimeJavaType[] GetImplementedInterfacesAsTypeWrappers(RuntimeContext context, ITypeSymbol type)
        {
            var interfaceTypes = GetInterfaces(type);
            var interfaces = new RuntimeJavaType[interfaceTypes.Length];

            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                var decl = interfaceTypes[i].DeclaringType;
                if (decl != null && context.AttributeHelper.IsGhostInterface(decl))
                {
                    // we have to return the declaring type for ghost interfaces
                    interfaces[i] = context.ClassLoaderFactory.GetJavaTypeFromType(decl);
                }
                else
                {
                    interfaces[i] = context.ClassLoaderFactory.GetJavaTypeFromType(interfaceTypes[i]);
                }
            }

            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                if (interfaces[i].IsRemapped)
                {
                    // for remapped interfaces, we also return the original interface (Java types will ignore it, if it isn't listed in the ImplementsAttribute)
                    var twRemapped = interfaces[i];
                    var tw = context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(interfaceTypes[i]);
                    interfaces[i] = tw;
                    if (Array.IndexOf(interfaces, twRemapped) == -1)
                        interfaces = ArrayUtil.Concat(interfaces, twRemapped);
                }
            }

            return interfaces;
        }

        internal RuntimeJavaType GetPublicBaseTypeWrapper()
        {
            Debug.Assert(!IsPublic);

            if (IsUnloadable || IsInterface)
                return Context.JavaBase.TypeOfJavaLangObject;

            for (var tw = this; ; tw = tw.BaseTypeWrapper)
                if (tw.IsPublic)
                    return tw;
        }

#if !EXPORTER

        // return the constructor used for automagic .NET serialization
        internal virtual IMethodBaseSymbol GetSerializationConstructor()
        {
            return TypeAsBaseType.GetConstructor(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                [context.Resolver.ResolveCoreType(typeof(System.Runtime.Serialization.SerializationInfo).FullName), context.Resolver.ResolveCoreType(typeof(System.Runtime.Serialization.StreamingContext).FullName)]);
        }

        internal virtual IMethodBaseSymbol GetBaseSerializationConstructor()
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
            get { return this is RuntimeByteCodeJavaType; }
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

        internal virtual byte[] GetMethodRawTypeAnnotations(RuntimeJavaMethod mw)
        {
            return null;
        }

        internal virtual byte[] GetFieldRawTypeAnnotations(RuntimeJavaField fw)
        {
            return null;
        }

#if !IMPORTER && !EXPORTER

        internal virtual RuntimeJavaType Host
        {
            get { return null; }
        }

#endif

    }

}
