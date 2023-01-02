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
using IKVM.Runtime.Syntax;

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

    abstract class Annotation
    {

#if IMPORTER

        internal static Annotation LoadAssemblyCustomAttribute(ClassLoaderWrapper loader, object[] def)
        {
            if (def.Length == 0)
                throw new ArgumentException("LoadAssemblyCustomAttribute did not receive any definitions.");
            if (object.Equals(def[0], AnnotationDefaultAttribute.TAG_ANNOTATION) == false)
                throw new InternalException("LoadAssemblyCustomAttribute did not receive AnnotationDefaultAttribute.TAG_ANNOTATION.");

            string annotationClass = (string)def[1];
            if (ClassFile.IsValidFieldSig(annotationClass))
            {
                try
                {
                    return loader.RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.LoadOrThrow).Annotation;
                }
                catch (RetargetableJavaException)
                {

                }
            }
            return null;
        }

#endif

#if !EXPORTER
        // NOTE this method returns null if the type could not be found
        // or if the type is not a Custom Attribute and we're not in the static compiler
        internal static Annotation Load(TypeWrapper owner, object[] def)
        {
            Debug.Assert(def[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION));
            string annotationClass = (string)def[1];
#if !IMPORTER
            if (!annotationClass.EndsWith("$Annotation;")
                && !annotationClass.EndsWith("$Annotation$__ReturnValue;")
                && !annotationClass.EndsWith("$Annotation$__Multiple;"))
            {
                // we don't want to try to load an annotation in dynamic mode,
                // unless it is a .NET custom attribute (which can affect runtime behavior)
                return null;
            }
#endif
            if (ClassFile.IsValidFieldSig(annotationClass))
            {
                TypeWrapper tw = owner.GetClassLoader().RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.Link);
                // Java allows inaccessible annotations to be used, so when the annotation isn't visible
                // we fall back to using the DynamicAnnotationAttribute.
                if (!tw.IsUnloadable && tw.IsAccessibleFrom(owner))
                {
                    return tw.Annotation;
                }
            }
            Tracer.Warning(Tracer.Compiler, "Unable to load annotation class {0}", annotationClass);
#if IMPORTER
            return new CompiledTypeWrapper.CompiledAnnotation(StaticCompiler.GetRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute"));
#else
            return null;
#endif
        }
#endif

        private static object LookupEnumValue(Type enumType, string value)
        {
            FieldInfo field = enumType.GetField(value, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                return field.GetRawConstantValue();
            }
            // both __unspecified and missing values end up here
            return EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(enumType), 0);
        }

        protected static object ConvertValue(ClassLoaderWrapper loader, Type targetType, object obj)
        {
            if (targetType.IsEnum)
            {
                // TODO check the obj descriptor matches the type we expect
                if (((object[])obj)[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
                {
                    object[] arr = (object[])obj;
                    object value = null;
                    for (int i = 1; i < arr.Length; i++)
                    {
                        // TODO check the obj descriptor matches the type we expect
                        string s = ((object[])arr[i])[2].ToString();
                        object newval = LookupEnumValue(targetType, s);
                        if (value == null)
                        {
                            value = newval;
                        }
                        else
                        {
                            value = EnumHelper.OrBoxedIntegrals(value, newval);
                        }
                    }
                    return value;
                }
                else
                {
                    string s = ((object[])obj)[2].ToString();
                    if (s == "__unspecified")
                    {
                        // TODO we should probably return null and handle that
                    }
                    return LookupEnumValue(targetType, s);
                }
            }
            else if (targetType == Types.Type)
            {
                // TODO check the obj descriptor matches the type we expect
                return loader.FieldTypeWrapperFromSig(((string)((object[])obj)[1]).Replace('/', '.'), LoadMode.LoadOrThrow).TypeAsTBD;
            }
            else if (targetType.IsArray)
            {
                // TODO check the obj descriptor matches the type we expect
                object[] arr = (object[])obj;
                Type elementType = targetType.GetElementType();
                object[] targetArray = new object[arr.Length - 1];
                for (int i = 1; i < arr.Length; i++)
                {
                    targetArray[i - 1] = ConvertValue(loader, elementType, arr[i]);
                }
                return targetArray;
            }
            else
            {
                return obj;
            }
        }

        internal static bool HasRetentionPolicyRuntime(object[] annotations)
        {
            if (annotations != null)
            {
                foreach (object[] def in annotations)
                {
                    if (def[1].Equals("Ljava/lang/annotation/Retention;"))
                    {
                        for (int i = 2; i < def.Length; i += 2)
                        {
                            if (def[i].Equals("value"))
                            {
                                object[] val = def[i + 1] as object[];
                                if (val != null
                                    && val.Length == 3
                                    && val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
                                    && val[1].Equals("Ljava/lang/annotation/RetentionPolicy;")
                                    && val[2].Equals("RUNTIME"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        internal static bool HasObsoleteAttribute(object[] annotations)
        {
            if (annotations != null)
            {
                foreach (object[] def in annotations)
                {
                    if (def[1].Equals("Lcli/System/ObsoleteAttribute$Annotation;"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected static object QualifyClassNames(ClassLoaderWrapper loader, object annotation)
        {
            bool copy = false;
            object[] def = (object[])annotation;
            for (int i = 3; i < def.Length; i += 2)
            {
                object[] val = def[i] as object[];
                if (val != null)
                {
                    object[] newval = ValueQualifyClassNames(loader, val);
                    if (newval != val)
                    {
                        if (!copy)
                        {
                            copy = true;
                            object[] newdef = new object[def.Length];
                            Array.Copy(def, newdef, def.Length);
                            def = newdef;
                        }
                        def[i] = newval;
                    }
                }
            }
            return def;
        }

        private static object[] ValueQualifyClassNames(ClassLoaderWrapper loader, object[] val)
        {
            if (val[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
            {
                return (object[])QualifyClassNames(loader, val);
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_CLASS))
            {
                string sig = (string)val[1];
                if (sig.StartsWith("L"))
                {
                    TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
                    if (tw != null)
                    {
                        return new object[] { AnnotationDefaultAttribute.TAG_CLASS, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";" };
                    }
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM))
            {
                string sig = (string)val[1];
                TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
                if (tw != null)
                {
                    return new object[] { AnnotationDefaultAttribute.TAG_ENUM, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";", val[2] };
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
            {
                bool copy = false;
                for (int i = 1; i < val.Length; i++)
                {
                    object[] nval = val[i] as object[];
                    if (nval != null)
                    {
                        object newnval = ValueQualifyClassNames(loader, nval);
                        if (newnval != nval)
                        {
                            if (!copy)
                            {
                                copy = true;
                                object[] newval = new object[val.Length];
                                Array.Copy(val, newval, val.Length);
                                val = newval;
                            }
                            val[i] = newnval;
                        }
                    }
                }
                return val;
            }
            else if (val[0].Equals(AnnotationDefaultAttribute.TAG_ERROR))
            {
                return val;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        internal abstract void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation);
        internal abstract void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation);

        internal virtual void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
        {
        }

        internal abstract bool IsCustomAttribute { get; }
    }

    [Flags]
    enum TypeFlags : ushort
    {

        None = 0,
        HasIncompleteInterfaceImplementation = 1,
        InternalAccess = 2,
        HasStaticInitializer = 4,
        VerifyError = 8,
        ClassFormatError = 16,
        HasUnsupportedAbstractMethods = 32,
        Anonymous = 64,
        Linked = 128,

    }

    static class NamePrefix
    {

        internal const string Type2AccessStubBackingField = "__<>";
        internal const string AccessStub = "<accessstub>";
        internal const string NonVirtual = "<nonvirtual>";
        internal const string Bridge = "<bridge>";
        internal const string Incomplete = "<incomplete>";
        internal const string DefaultMethod = "<default>";
        internal const string PrivateInterfaceInstanceMethod = "<piim>";

    }

    static class NestedTypeName
    {

        internal const string CallerID = "__<CallerID>";
        internal const string InterfaceHelperMethods = "__<>IHM";
        internal const string PrivateInterfaceMethods = "__<>PIM";

        // interop types (mangled if necessary)
        internal const string Fields = "__Fields";
        internal const string Methods = "__Methods";
        internal const string DefaultMethods = "__DefaultMethods";

        // prefixes
        internal const string ThreadLocal = "__<tls>_";
        internal const string AtomicReferenceFieldUpdater = "__<ARFU>_";
        internal const string IndyCallSite = "__<>IndyCS";
        internal const string MethodHandleConstant = "__<>MHC";
        internal const string MethodTypeConstant = "__<>MTC";
        internal const string IntrinsifiedAnonymousClass = "__<>Anon";

    }

    internal abstract class TypeWrapper
    {

        private static readonly object flagsLock = new object();
        private readonly string name; // java name (e.g. java.lang.Object)
        private readonly Modifiers modifiers;
        private TypeFlags flags;
        private MethodWrapper[] methods;
        private FieldWrapper[] fields;
#if !IMPORTER && !EXPORTER
        private java.lang.Class classObject;
#endif
        internal static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
        internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
        internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

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

    sealed class UnloadableTypeWrapper : TypeWrapper
    {

        internal const string ContainerTypeName = "__<Unloadable>";
        readonly Type missingType;
        Type customModifier;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        internal UnloadableTypeWrapper(string name) :
            base(TypeFlags.None, TypeWrapper.UnloadableModifiersHack, name)
        {

        }

        internal UnloadableTypeWrapper(Type missingType) :
            this(missingType.FullName) // TODO demangle and re-mangle appropriately
        {
            this.missingType = missingType;
        }

        internal UnloadableTypeWrapper(string name, Type customModifier) :
            this(name)
        {
            this.customModifier = customModifier;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return null; }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return null;
        }

        internal override TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
        {
            var tw = loader.LoadClassByDottedNameFast(this.Name);
            if (tw == null)
                throw new NoClassDefFoundError(this.Name);

            return tw;
        }

        internal override string SigName
        {
            get
            {
                var name = Name;
                if (name.StartsWith("["))
                    return name;

                return "L" + name + ";";
            }
        }

        protected override void LazyPublishMembers()
        {
            throw new InvalidOperationException("LazyPublishMembers called on UnloadableTypeWrapper: " + Name);
        }

        internal override Type TypeAsTBD
        {
            get
            {
                throw new InvalidOperationException("get_Type called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
#if IMPORTER
                if (missingType != null)
                {
                    StaticCompiler.IssueMissingTypeMessage(missingType);
                    return TypeWrapper.EmptyArray;
                }
#endif

                throw new InvalidOperationException("get_Interfaces called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                throw new InvalidOperationException("get_InnerClasses called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                throw new InvalidOperationException("get_DeclaringTypeWrapper called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override void Finish()
        {
            throw new InvalidOperationException("Finish called on UnloadableTypeWrapper: " + Name);
        }

        internal Type MissingType
        {
            get { return missingType; }
        }

        internal Type CustomModifier
        {
            get { return customModifier; }
        }

        internal void SetCustomModifier(Type type)
        {
            this.customModifier = type;
        }

#if EMITTERS

        internal Type GetCustomModifier(TypeWrapperFactory context)
        {
            // we don't need to lock, because we're only supposed to be called while holding the finish lock
            return customModifier ?? (customModifier = context.DefineUnloadable(this.Name));
        }

        internal override void EmitCheckcast(CodeEmitter ilgen)
        {
            throw new InvalidOperationException("EmitCheckcast called on UnloadableTypeWrapper: " + Name);
        }

        internal override void EmitInstanceOf(CodeEmitter ilgen)
        {
            throw new InvalidOperationException("EmitInstanceOf called on UnloadableTypeWrapper: " + Name);
        }

#endif // EMITTERS

    }

    sealed class PrimitiveTypeWrapper : TypeWrapper
    {

        internal static readonly PrimitiveTypeWrapper BYTE = new PrimitiveTypeWrapper(Types.Byte, "B");
        internal static readonly PrimitiveTypeWrapper CHAR = new PrimitiveTypeWrapper(Types.Char, "C");
        internal static readonly PrimitiveTypeWrapper DOUBLE = new PrimitiveTypeWrapper(Types.Double, "D");
        internal static readonly PrimitiveTypeWrapper FLOAT = new PrimitiveTypeWrapper(Types.Single, "F");
        internal static readonly PrimitiveTypeWrapper INT = new PrimitiveTypeWrapper(Types.Int32, "I");
        internal static readonly PrimitiveTypeWrapper LONG = new PrimitiveTypeWrapper(Types.Int64, "J");
        internal static readonly PrimitiveTypeWrapper SHORT = new PrimitiveTypeWrapper(Types.Int16, "S");
        internal static readonly PrimitiveTypeWrapper BOOLEAN = new PrimitiveTypeWrapper(Types.Boolean, "Z");
        internal static readonly PrimitiveTypeWrapper VOID = new PrimitiveTypeWrapper(Types.Void, "V");

        readonly Type type;
        readonly string sigName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sigName"></param>
        private PrimitiveTypeWrapper(Type type, string sigName) :
            base(TypeFlags.None, Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null)
        {
            this.type = type;
            this.sigName = sigName;
        }

        internal override TypeWrapper BaseTypeWrapper => null;

        internal static bool IsPrimitiveType(Type type)
        {
            return type == BYTE.type
                || type == CHAR.type
                || type == DOUBLE.type
                || type == FLOAT.type
                || type == INT.type
                || type == LONG.type
                || type == SHORT.type
                || type == BOOLEAN.type
                || type == VOID.type;
        }

        internal override string SigName => sigName;

        internal override ClassLoaderWrapper GetClassLoader() => ClassLoaderWrapper.GetBootstrapClassLoader();

        internal override Type TypeAsTBD => type;

        public override string ToString() => "PrimitiveTypeWrapper[" + sigName + "]";

    }

    class CompiledTypeWrapper : TypeWrapper
    {

        readonly Type type;

        TypeWrapper baseTypeWrapper = VerifierTypeWrapper.Null;
        volatile TypeWrapper[] interfaces;
        MethodInfo clinitMethod;
        volatile bool clinitMethodSet;
        Modifiers reflectiveModifiers;

        internal static CompiledTypeWrapper newInstance(string name, Type type)
        {
            // TODO since ghost and remapped types can only exist in the core library assembly, we probably
            // should be able to remove the Type.IsDefined() tests in most cases
            if (type.IsValueType && AttributeHelper.IsGhostInterface(type))
            {
                return new CompiledGhostTypeWrapper(name, type);
            }
            else if (AttributeHelper.IsRemappedType(type))
            {
                return new CompiledRemappedTypeWrapper(name, type);
            }
            else
            {
                return new CompiledTypeWrapper(name, type);
            }
        }

        private sealed class CompiledRemappedTypeWrapper : CompiledTypeWrapper
        {
            private readonly Type remappedType;

            internal CompiledRemappedTypeWrapper(string name, Type type)
                : base(name, type)
            {
                RemappedTypeAttribute attr = AttributeHelper.GetRemappedType(type);
                if (attr == null)
                {
                    throw new InvalidOperationException();
                }
                remappedType = attr.Type;
            }

            internal override Type TypeAsTBD
            {
                get
                {
                    return remappedType;
                }
            }

            internal override bool IsRemapped
            {
                get
                {
                    return true;
                }
            }

            protected override void LazyPublishMethods()
            {
                List<MethodWrapper> list = new List<MethodWrapper>();
                const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
                foreach (ConstructorInfo ctor in type.GetConstructors(bindingFlags))
                {
                    AddMethod(list, ctor);
                }
                foreach (MethodInfo method in type.GetMethods(bindingFlags))
                {
                    AddMethod(list, method);
                }
                // if we're a remapped interface, we need to get the methods from the real interface
                if (remappedType.IsInterface)
                {
                    Type nestedHelper = type.GetNestedType("__Helper", BindingFlags.Public | BindingFlags.Static);
                    foreach (RemappedInterfaceMethodAttribute m in AttributeHelper.GetRemappedInterfaceMethods(type))
                    {
                        MethodInfo method = remappedType.GetMethod(m.MappedTo);
                        MethodInfo mbHelper = method;
                        ExModifiers modifiers = AttributeHelper.GetModifiers(method, false);
                        string name;
                        string sig;
                        TypeWrapper retType;
                        TypeWrapper[] paramTypes;
                        MemberFlags flags = MemberFlags.None;
                        GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
                        if (nestedHelper != null)
                        {
                            mbHelper = nestedHelper.GetMethod(m.Name);
                            if (mbHelper == null)
                            {
                                mbHelper = method;
                            }
                        }
                        MethodWrapper mw = new CompiledRemappedMethodWrapper(this, m.Name, sig, method, retType, paramTypes, modifiers, false, mbHelper, null);
                        mw.SetDeclaredExceptions(m.Throws);
                        list.Add(mw);
                    }
                }
                SetMethods(list.ToArray());
            }

            private void AddMethod(List<MethodWrapper> list, MethodBase method)
            {
                HideFromJavaFlags flags = AttributeHelper.GetHideFromJavaFlags(method);
                if ((flags & HideFromJavaFlags.Code) == 0
                    && (remappedType.IsSealed || !method.Name.StartsWith("instancehelper_"))
                    && (!remappedType.IsSealed || method.IsStatic))
                {
                    list.Add(CreateRemappedMethodWrapper(method, flags));
                }
            }

            protected override void LazyPublishFields()
            {
                List<FieldWrapper> list = new List<FieldWrapper>();
                FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
                    if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                    {
                        list.Add(CreateFieldWrapper(field, hideFromJavaFlags));
                    }
                }
                SetFields(list.ToArray());
            }

            private MethodWrapper CreateRemappedMethodWrapper(MethodBase mb, HideFromJavaFlags hideFromJavaflags)
            {
                ExModifiers modifiers = AttributeHelper.GetModifiers(mb, false);
                string name;
                string sig;
                TypeWrapper retType;
                TypeWrapper[] paramTypes;
                MemberFlags flags = MemberFlags.None;
                GetNameSigFromMethodBase(mb, out name, out sig, out retType, out paramTypes, ref flags);
                MethodInfo mbHelper = mb as MethodInfo;
                bool hideFromReflection = mbHelper != null && (hideFromJavaflags & HideFromJavaFlags.Reflection) != 0;
                MethodInfo mbNonvirtualHelper = null;
                if (!mb.IsStatic && !mb.IsConstructor)
                {
                    ParameterInfo[] parameters = mb.GetParameters();
                    Type[] argTypes = new Type[parameters.Length + 1];
                    argTypes[0] = remappedType;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        argTypes[i + 1] = parameters[i].ParameterType;
                    }
                    MethodInfo helper = type.GetMethod("instancehelper_" + mb.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, argTypes, null);
                    if (helper != null)
                    {
                        mbHelper = helper;
                    }
                    mbNonvirtualHelper = type.GetMethod("nonvirtualhelper/" + mb.Name, BindingFlags.NonPublic | BindingFlags.Static, null, argTypes, null);
                }
                return new CompiledRemappedMethodWrapper(this, name, sig, mb, retType, paramTypes, modifiers, hideFromReflection, mbHelper, mbNonvirtualHelper);
            }
        }

        private sealed class CompiledGhostTypeWrapper : CompiledTypeWrapper
        {
            private volatile FieldInfo ghostRefField;
            private volatile Type typeAsBaseType;

            internal CompiledGhostTypeWrapper(string name, Type type)
                : base(name, type)
            {
            }

            internal override Type TypeAsBaseType
            {
                get
                {
                    if (typeAsBaseType == null)
                    {
                        typeAsBaseType = type.GetNestedType("__Interface");
                    }
                    return typeAsBaseType;
                }
            }

            internal override FieldInfo GhostRefField
            {
                get
                {
                    if (ghostRefField == null)
                    {
                        ghostRefField = type.GetField("__<ref>");
                    }
                    return ghostRefField;
                }
            }

            internal override bool IsGhost
            {
                get
                {
                    return true;
                }
            }

#if !IMPORTER && !EXPORTER && !FIRST_PASS
            internal override object GhostWrap(object obj)
            {
                return type.GetMethod("Cast").Invoke(null, new object[] { obj });
            }

            internal override object GhostUnwrap(object obj)
            {
                return type.GetMethod("ToObject").Invoke(obj, new object[0]);
            }
#endif
        }

        /// <summary>
        /// Gets the Java type name of a Java type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        internal static JavaTypeName? GetName(Type type)
        {
            if (type.HasElementType)
                return null;
            if (type.IsGenericType)
                return null;
            if (AttributeHelper.IsJavaModule(type.Module) == false)
                return null;

            // look for our custom attribute, that contains the real name of the type (for inner classes)
            var attr = AttributeHelper.GetInnerClass(type);
            if (attr != null)
            {
                var name = attr.InnerClassName;
                if (name != null)
                    return name;
            }

            // type is an inner type
            if (type.DeclaringType != null)
                return GetName(type.DeclaringType) + "$" + TypeNameUtil.Unescape(type.Name);

            return TypeNameUtil.Unescape(type.FullName);
        }

        static TypeWrapper GetBaseTypeWrapper(Type type)
        {
            if (type.IsInterface || AttributeHelper.IsGhostInterface(type))
            {
                return null;
            }
            else if (type.BaseType == null)
            {
                // System.Object must appear to be derived from java.lang.Object
                return CoreClasses.java.lang.Object.Wrapper;
            }
            else
            {
                var attr = AttributeHelper.GetRemappedType(type);
                if (attr != null)
                {
                    if (attr.Type == Types.Object)
                        return null;
                    else
                        return CoreClasses.java.lang.Object.Wrapper;
                }
                else if (ClassLoaderWrapper.IsRemappedType(type.BaseType))
                {
                    // if we directly extend System.Object or System.Exception, the base class must be cli.System.Object or cli.System.Exception
                    return DotNetTypeWrapper.GetWrapperFromDotNetType(type.BaseType);
                }

                TypeWrapper tw = null;
                while (tw == null)
                {
                    type = type.BaseType;
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);
                }

                return tw;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="exmod"></param>
        /// <param name="name"></param>
        CompiledTypeWrapper(ExModifiers exmod, string name) :
            base(exmod.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, exmod.Modifiers, name)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        CompiledTypeWrapper(string name, Type type) :
            this(GetModifiers(type), name)
        {
            Debug.Assert(!(type is TypeBuilder));
            Debug.Assert(!type.Name.EndsWith("[]"));

            this.type = type;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get
            {
                if (baseTypeWrapper != VerifierTypeWrapper.Null)
                    return baseTypeWrapper;

                return baseTypeWrapper = GetBaseTypeWrapper(type);
            }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return AssemblyClassLoader.FromAssembly(type.Assembly);
        }

        private static ExModifiers GetModifiers(Type type)
        {
            ModifiersAttribute attr = AttributeHelper.GetModifiersAttribute(type);
            if (attr != null)
            {
                return new ExModifiers(attr.Modifiers, attr.IsInternal);
            }
            // only returns public, protected, private, final, static, abstract and interface (as per
            // the documentation of Class.getModifiers())
            Modifiers modifiers = 0;
            if (type.IsPublic || type.IsNestedPublic)
            {
                modifiers |= Modifiers.Public;
            }
            if (type.IsSealed)
            {
                modifiers |= Modifiers.Final;
            }
            if (type.IsAbstract)
            {
                modifiers |= Modifiers.Abstract;
            }
            if (type.IsInterface)
            {
                modifiers |= Modifiers.Interface;
            }
            else
            {
                modifiers |= Modifiers.Super;
            }

            return new ExModifiers(modifiers, false);
        }

        internal override bool HasStaticInitializer
        {
            get
            {
                if (!clinitMethodSet)
                {
                    try
                    {
                        clinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    }
#if IMPORTER
                    catch (IKVM.Reflection.MissingMemberException) { }
#endif
                    finally { }
                    clinitMethodSet = true;
                }
                return clinitMethod != null;
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                if (interfaces == null)
                {
                    interfaces = GetInterfaces();
                }
                return interfaces;
            }
        }

        private TypeWrapper[] GetInterfaces()
        {
            // NOTE instead of getting the interfaces list from Type, we use a custom
            // attribute to list the implemented interfaces, because Java reflection only
            // reports the interfaces *directly* implemented by the type, not the inherited
            // interfaces. This is significant for serialVersionUID calculation (for example).
            ImplementsAttribute attr = AttributeHelper.GetImplements(type);
            if (attr == null)
            {
                if (BaseTypeWrapper == CoreClasses.java.lang.Object.Wrapper)
                {
                    return GetImplementedInterfacesAsTypeWrappers(type);
                }
                return TypeWrapper.EmptyArray;
            }
            string[] interfaceNames = attr.Interfaces;
            TypeWrapper[] interfaceWrappers = new TypeWrapper[interfaceNames.Length];
            if (this.IsRemapped)
            {
                for (int i = 0; i < interfaceWrappers.Length; i++)
                {
                    interfaceWrappers[i] = ClassLoaderWrapper.LoadClassCritical(interfaceNames[i]);
                }
            }
            else
            {
                TypeWrapper[] typeWrappers = GetImplementedInterfacesAsTypeWrappers(type);
                for (int i = 0; i < interfaceWrappers.Length; i++)
                {
                    for (int j = 0; j < typeWrappers.Length; j++)
                    {
                        if (typeWrappers[j].Name == interfaceNames[i])
                        {
                            interfaceWrappers[i] = typeWrappers[j];
                            break;
                        }
                    }
                    if (interfaceWrappers[i] == null)
                    {
#if IMPORTER
                        throw new FatalCompilerErrorException(Message.UnableToResolveInterface, interfaceNames[i], this);
#else
                        throw new InternalException($"Unable to resolve interface {interfaceNames[i]} on type {this}");
#endif
                    }
                }
            }
            return interfaceWrappers;
        }

        private static bool IsNestedTypeAnonymousOrLocalClass(Type type)
        {
            switch (type.Attributes & (TypeAttributes.SpecialName | TypeAttributes.VisibilityMask))
            {
                case TypeAttributes.SpecialName | TypeAttributes.NestedPublic:
                case TypeAttributes.SpecialName | TypeAttributes.NestedAssembly:
                    return AttributeHelper.HasEnclosingMethodAttribute(type);
                default:
                    return false;
            }
        }

        private static bool IsAnnotationAttribute(Type type)
        {
            return type.Name.EndsWith("Attribute", StringComparison.Ordinal)
                && type.IsClass
                && type.BaseType.FullName == "ikvm.internal.AnnotationAttributeBase";
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                List<TypeWrapper> wrappers = new List<TypeWrapper>();
                foreach (Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (IsAnnotationAttribute(nested))
                    {
                        // HACK it's the custom attribute we generated for a corresponding annotation, so we shouldn't surface it as an inner classes
                        // (we can't put a HideFromJavaAttribute on it, because we do want the class to be visible as a $Proxy)
                    }
                    else if (IsNestedTypeAnonymousOrLocalClass(nested))
                    {
                        // anonymous and local classes are not reported as inner classes
                    }
                    else if (AttributeHelper.IsHideFromJava(nested))
                    {
                        // ignore
                    }
                    else
                    {
                        wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(nested));
                    }
                }
                foreach (string s in AttributeHelper.GetNonNestedInnerClasses(type))
                {
                    wrappers.Add(GetClassLoader().LoadClassByDottedName(s));
                }
                return wrappers.ToArray();
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                if (IsNestedTypeAnonymousOrLocalClass(type))
                {
                    return null;
                }
                Type declaringType = type.DeclaringType;
                if (declaringType != null)
                {
                    return ClassLoaderWrapper.GetWrapperFromType(declaringType);
                }
                string decl = AttributeHelper.GetNonNestedOuterClasses(type);
                if (decl != null)
                {
                    return GetClassLoader().LoadClassByDottedName(decl);
                }
                return null;
            }
        }

        // returns true iff name is of the form "...$<n>"
        private static bool IsAnonymousClassName(string name)
        {
            int index = name.LastIndexOf('$') + 1;
            if (index > 1 && index < name.Length)
            {
                while (index < name.Length)
                {
                    if ("0123456789".IndexOf(name[index++]) == -1)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        // This method uses some heuristics to predict the reflective modifiers and if the prediction matches
        // we can avoid storing the InnerClassesAttribute to record the modifiers.
        // The heuristics are based on javac from Java 7.
        internal static Modifiers PredictReflectiveModifiers(TypeWrapper tw)
        {
            Modifiers modifiers = Modifiers.Static | (tw.Modifiers & (Modifiers.Public | Modifiers.Abstract | Modifiers.Interface));
            // javac marks anonymous classes as final, but the InnerClasses attribute access_flags does not have the ACC_FINAL flag set
            if (tw.IsFinal && !IsAnonymousClassName(tw.Name))
            {
                modifiers |= Modifiers.Final;
            }
            // javac uses the this$0 field to store the outer instance reference for non-static inner classes
            foreach (FieldWrapper fw in tw.GetFields())
            {
                if (fw.Name == "this$0")
                {
                    modifiers &= ~Modifiers.Static;
                    break;
                }
            }
            return modifiers;
        }

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                if (reflectiveModifiers == 0)
                {
                    Modifiers mods;
                    InnerClassAttribute attr = AttributeHelper.GetInnerClass(type);
                    if (attr != null)
                    {
                        // the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
                        // (minus ACC_SUPER)
                        mods = attr.Modifiers & (Modifiers)0x761F;
                    }
                    else if (type.DeclaringType != null)
                    {
                        mods = PredictReflectiveModifiers(this);
                    }
                    else
                    {
                        // the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
                        // (minus ACC_SUPER)
                        mods = Modifiers & (Modifiers)0x7611;
                    }
                    if (IsInterface)
                    {
                        mods |= Modifiers.Abstract;
                    }
                    reflectiveModifiers = mods;
                }
                return reflectiveModifiers;
            }
        }

        internal override Type TypeAsBaseType
        {
            get
            {
                return type;
            }
        }

        private void SigTypePatchUp(string sigtype, ref TypeWrapper type)
        {
            if (sigtype != type.SigName)
            {
                // if type is an array, we know that it is a ghost array, because arrays of unloadable are compiled
                // as object (not as arrays of object)
                if (type.IsArray)
                {
                    type = GetClassLoader().FieldTypeWrapperFromSig(sigtype, LoadMode.LoadOrThrow);
                }
                else if (type.IsPrimitive)
                {
                    type = DotNetTypeWrapper.GetWrapperFromDotNetType(type.TypeAsTBD);
                    if (sigtype != type.SigName)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (type.IsNonPrimitiveValueType)
                {
                    // this can't happen and even if it does happen we cannot return
                    // UnloadableTypeWrapper because that would result in incorrect code
                    // being generated
                    throw new InvalidOperationException();
                }
                else
                {
                    if (sigtype[0] == 'L')
                    {
                        sigtype = sigtype.Substring(1, sigtype.Length - 2);
                    }
                    try
                    {
                        TypeWrapper tw = GetClassLoader().LoadClassByDottedNameFast(sigtype);
                        if (tw != null && tw.IsRemapped)
                        {
                            type = tw;
                            return;
                        }
                    }
                    catch (RetargetableJavaException)
                    {
                    }
                    type = new UnloadableTypeWrapper(sigtype);
                }
            }
        }

        private static void ParseSig(string sig, out string[] sigparam, out string sigret)
        {
            List<string> list = new List<string>();
            int pos = 1;
            for (; ; )
            {
                switch (sig[pos])
                {
                    case 'L':
                        {
                            int end = sig.IndexOf(';', pos) + 1;
                            list.Add(sig.Substring(pos, end - pos));
                            pos = end;
                            break;
                        }
                    case '[':
                        {
                            int skip = 1;
                            while (sig[pos + skip] == '[') skip++;
                            if (sig[pos + skip] == 'L')
                            {
                                int end = sig.IndexOf(';', pos) + 1;
                                list.Add(sig.Substring(pos, end - pos));
                                pos = end;
                            }
                            else
                            {
                                skip++;
                                list.Add(sig.Substring(pos, skip));
                                pos += skip;
                            }
                            break;
                        }
                    case ')':
                        sigparam = list.ToArray();
                        sigret = sig.Substring(pos + 1);
                        return;
                    default:
                        list.Add(sig.Substring(pos, 1));
                        pos++;
                        break;
                }
            }
        }

        private static bool IsCallerID(Type type)
        {
#if EXPORTER
            return type.FullName == "ikvm.internal.CallerID";
#else
            return type == CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
#endif
        }

        private static bool IsCallerSensitive(MethodBase mb)
        {
#if FIRST_PASS
            return false;
#elif IMPORTER || EXPORTER
            foreach (CustomAttributeData cad in mb.GetCustomAttributesData())
            {
                if (cad.AttributeType.FullName == "sun.reflect.CallerSensitiveAttribute")
                {
                    return true;
                }
            }
            return false;
#else
            return mb.IsDefined(typeof(global::sun.reflect.CallerSensitiveAttribute), false);
#endif
        }

        private void GetNameSigFromMethodBase(MethodBase method, out string name, out string sig, out TypeWrapper retType, out TypeWrapper[] paramTypes, ref MemberFlags flags)
        {
            retType = method is ConstructorInfo ? PrimitiveTypeWrapper.VOID : GetParameterTypeWrapper(((MethodInfo)method).ReturnParameter);
            ParameterInfo[] parameters = method.GetParameters();
            int len = parameters.Length;
            if (len > 0
                && IsCallerID(parameters[len - 1].ParameterType)
                && GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader()
                && IsCallerSensitive(method))
            {
                len--;
                flags |= MemberFlags.CallerID;
            }
            paramTypes = new TypeWrapper[len];
            for (int i = 0; i < len; i++)
            {
                paramTypes[i] = GetParameterTypeWrapper(parameters[i]);
            }
            NameSigAttribute attr = AttributeHelper.GetNameSig(method);
            if (attr != null)
            {
                name = attr.Name;
                sig = attr.Sig;
                string[] sigparams;
                string sigret;
                ParseSig(sig, out sigparams, out sigret);
                // HACK newhelper methods have a return type, but it should be void
                if (name == "<init>")
                {
                    retType = PrimitiveTypeWrapper.VOID;
                }
                SigTypePatchUp(sigret, ref retType);
                // if we have a remapped method, the paramTypes array contains an additional entry for "this" so we have
                // to remove that
                if (paramTypes.Length == sigparams.Length + 1)
                {
                    paramTypes = ArrayUtil.DropFirst(paramTypes);
                }
                Debug.Assert(sigparams.Length == paramTypes.Length);
                for (int i = 0; i < sigparams.Length; i++)
                {
                    SigTypePatchUp(sigparams[i], ref paramTypes[i]);
                }
            }
            else
            {
                if (method is ConstructorInfo)
                {
                    name = method.IsStatic ? "<clinit>" : "<init>";
                }
                else
                {
                    name = method.Name;
                    if (name.StartsWith(NamePrefix.Bridge, StringComparison.Ordinal))
                    {
                        name = name.Substring(NamePrefix.Bridge.Length);
                    }
                    if (method.IsSpecialName)
                    {
                        name = UnicodeUtil.UnescapeInvalidSurrogates(name);
                    }
                }
                if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
                {
                    paramTypes = ArrayUtil.DropFirst(paramTypes);
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
                foreach (TypeWrapper tw in paramTypes)
                {
                    sb.Append(tw.SigName);
                }
                sb.Append(")");
                sb.Append(retType.SigName);
                sig = sb.ToString();
            }
        }

        private sealed class DelegateConstructorMethodWrapper : MethodWrapper
        {
            private readonly ConstructorInfo constructor;
            private MethodInfo invoke;

            private DelegateConstructorMethodWrapper(TypeWrapper tw, TypeWrapper iface, ExModifiers mods)
                : base(tw, StringConstants.INIT, "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, mods.Modifiers, mods.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None)
            {
            }

            internal DelegateConstructorMethodWrapper(TypeWrapper tw, MethodBase method)
                : this(tw, tw.GetClassLoader().LoadClassByDottedName(tw.Name + DotNetTypeWrapper.DelegateInterfaceSuffix), AttributeHelper.GetModifiers(method, false))
            {
                constructor = (ConstructorInfo)method;
            }

            protected override void DoLinkMethod()
            {
                MethodWrapper mw = GetParameters()[0].GetMethods()[0];
                mw.Link();
                invoke = (MethodInfo)mw.GetMethod();
            }

#if EMITTERS
            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Ldvirtftn, invoke);
                ilgen.Emit(OpCodes.Newobj, constructor);
            }
#endif // EMITTERS
        }

        protected override void LazyPublishMethods()
        {
            bool isDelegate = type.BaseType == Types.MulticastDelegate;
            List<MethodWrapper> methods = new List<MethodWrapper>();
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (ConstructorInfo ctor in type.GetConstructors(flags))
            {
                HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(ctor);
                if (isDelegate && !ctor.IsStatic && (hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                {
                    methods.Add(new DelegateConstructorMethodWrapper(this, ctor));
                }
                else
                {
                    AddMethodOrConstructor(ctor, hideFromJavaFlags, methods);
                }
            }
            AddMethods(type.GetMethods(flags), methods);
            if (type.IsInterface && (type.IsPublic || type.IsNestedPublic))
            {
                Type privateInterfaceMethods = type.GetNestedType(NestedTypeName.PrivateInterfaceMethods, BindingFlags.NonPublic);
                if (privateInterfaceMethods != null)
                {
                    AddMethods(privateInterfaceMethods.GetMethods(flags), methods);
                }
            }
            SetMethods(methods.ToArray());
        }

        private void AddMethods(MethodInfo[] add, List<MethodWrapper> methods)
        {
            foreach (MethodInfo method in add)
            {
                AddMethodOrConstructor(method, AttributeHelper.GetHideFromJavaFlags(method), methods);
            }
        }

        private void AddMethodOrConstructor(MethodBase method, HideFromJavaFlags hideFromJavaFlags, List<MethodWrapper> methods)
        {
            if ((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
            {
                if (method.Name.StartsWith(NamePrefix.Incomplete, StringComparison.Ordinal))
                {
                    SetHasIncompleteInterfaceImplementation();
                }
            }
            else
            {
                if (method.IsSpecialName && (method.Name.StartsWith("__<", StringComparison.Ordinal) || method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)))
                {
                    // skip
                }
                else
                {
                    string name;
                    string sig;
                    TypeWrapper retType;
                    TypeWrapper[] paramTypes;
                    MethodInfo mi = method as MethodInfo;
                    bool hideFromReflection = mi != null && (hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0;
                    MemberFlags flags = hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None;
                    GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
                    ExModifiers mods = AttributeHelper.GetModifiers(method, false);
                    if (mods.IsInternal)
                    {
                        flags |= MemberFlags.InternalAccess;
                    }
                    if (hideFromReflection && name.StartsWith(NamePrefix.AccessStub, StringComparison.Ordinal))
                    {
                        int id = Int32.Parse(name.Substring(NamePrefix.AccessStub.Length, name.IndexOf('|', NamePrefix.AccessStub.Length) - NamePrefix.AccessStub.Length));
                        name = name.Substring(name.IndexOf('|', NamePrefix.AccessStub.Length) + 1);
                        flags |= MemberFlags.AccessStub;
                        MethodInfo nonvirt = type.GetMethod(NamePrefix.NonVirtual + id, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                        methods.Add(new AccessStubMethodWrapper(this, name, sig, mi, mi, nonvirt ?? mi, retType, paramTypes, mods.Modifiers & ~Modifiers.Final, flags));
                        return;
                    }
                    MethodInfo impl;
                    MethodWrapper mw;
                    if (IsGhost && (mods.Modifiers & (Modifiers.Static | Modifiers.Private)) == 0)
                    {
                        Type[] types = new Type[paramTypes.Length];
                        for (int i = 0; i < types.Length; i++)
                        {
                            types[i] = paramTypes[i].TypeAsSignatureType;
                        }
                        MethodInfo ifmethod = TypeAsBaseType.GetMethod(method.Name, types);
                        mw = new GhostMethodWrapper(this, name, sig, ifmethod, (MethodInfo)method, retType, paramTypes, mods.Modifiers, flags);
                        if (!mw.IsAbstract)
                        {
                            ((GhostMethodWrapper)mw).SetDefaultImpl(TypeAsSignatureType.GetMethod(NamePrefix.DefaultMethod + method.Name, types));
                        }
                    }
                    else if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.PrivateInterfaceInstanceMethod, StringComparison.Ordinal))
                    {
                        mw = new PrivateInterfaceMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else if (IsInterface && method.IsAbstract && (mods.Modifiers & Modifiers.Abstract) == 0 && (impl = GetDefaultInterfaceMethodImpl(mi, sig)) != null)
                    {
                        mw = new DefaultInterfaceMethodWrapper(this, name, sig, mi, impl, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else
                    {
                        mw = new TypicalMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    if (mw.HasNonPublicTypeInSignature)
                    {
                        if (mi != null)
                        {
                            MethodInfo stubVirt;
                            MethodInfo stubNonVirt;
                            if (GetType2AccessStubs(name, sig, out stubVirt, out stubNonVirt))
                            {
                                mw = new AccessStubMethodWrapper(this, name, sig, mi, stubVirt, stubNonVirt ?? stubVirt, retType, paramTypes, mw.Modifiers, flags);
                            }
                        }
                        else
                        {
                            ConstructorInfo stub;
                            if (GetType2AccessStub(sig, out stub))
                            {
                                mw = new AccessStubConstructorMethodWrapper(this, sig, (ConstructorInfo)method, stub, paramTypes, mw.Modifiers, flags);
                            }
                        }
                    }
                    methods.Add(mw);
                }
            }
        }

        private MethodInfo GetDefaultInterfaceMethodImpl(MethodInfo method, string expectedSig)
        {
            foreach (MethodInfo candidate in method.DeclaringType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (candidate.IsSpecialName
                    && candidate.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)
                    && candidate.Name.Length == method.Name.Length + NamePrefix.DefaultMethod.Length
                    && candidate.Name.EndsWith(method.Name, StringComparison.Ordinal))
                {
                    string name;
                    string sig;
                    TypeWrapper retType;
                    TypeWrapper[] paramTypes;
                    MemberFlags flags = MemberFlags.None;
                    GetNameSigFromMethodBase(candidate, out name, out sig, out retType, out paramTypes, ref flags);
                    if (sig == expectedSig)
                    {
                        return candidate;
                    }
                }
            }
            return null;
        }

        private bool GetType2AccessStubs(string name, string sig, out MethodInfo stubVirt, out MethodInfo stubNonVirt)
        {
            stubVirt = null;
            stubNonVirt = null;
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (MethodInfo method in type.GetMethods(flags))
            {
                if (AttributeHelper.IsHideFromJava(method))
                {
                    NameSigAttribute attr = AttributeHelper.GetNameSig(method);
                    if (attr != null && attr.Name == name && attr.Sig == sig)
                    {
                        if (method.Name.StartsWith(NamePrefix.NonVirtual, StringComparison.Ordinal))
                        {
                            stubNonVirt = method;
                        }
                        else
                        {
                            stubVirt = method;
                        }
                    }
                }
            }
            return stubVirt != null;
        }

        private bool GetType2AccessStub(string sig, out ConstructorInfo stub)
        {
            stub = null;
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            foreach (ConstructorInfo ctor in type.GetConstructors(flags))
            {
                if (AttributeHelper.IsHideFromJava(ctor))
                {
                    NameSigAttribute attr = AttributeHelper.GetNameSig(ctor);
                    if (attr != null && attr.Sig == sig)
                    {
                        stub = ctor;
                    }
                }
            }
            return stub != null;
        }

        private static int SortFieldByToken(FieldInfo field1, FieldInfo field2)
        {
            return field1.MetadataToken.CompareTo(field2.MetadataToken);
        }

        protected override void LazyPublishFields()
        {
            List<FieldWrapper> fields = new List<FieldWrapper>();
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            FieldInfo[] rawfields = type.GetFields(flags);
            Array.Sort(rawfields, SortFieldByToken);
            // FXBUG on .NET 3.5 and Mono Type.GetProperties() will not return "duplicate" properties (i.e. that have the same name and type, but differ in custom modifiers).
            // .NET 4.0 works as expected. We don't have a workaround, because that would require name mangling again and this situation is very unlikely anyway.
            PropertyInfo[] properties = type.GetProperties(flags);
            foreach (FieldInfo field in rawfields)
            {
                HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
                if ((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
                {
                    if (field.Name.StartsWith(NamePrefix.Type2AccessStubBackingField, StringComparison.Ordinal))
                    {
                        TypeWrapper tw = GetFieldTypeWrapper(field);
                        string name = field.Name.Substring(NamePrefix.Type2AccessStubBackingField.Length);
                        for (int i = 0; i < properties.Length; i++)
                        {
                            if (properties[i] != null
                                && name == properties[i].Name
                                && MatchTypes(tw, GetPropertyTypeWrapper(properties[i])))
                            {
                                fields.Add(new CompiledAccessStubFieldWrapper(this, properties[i], field, tw));
                                properties[i] = null;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (field.IsSpecialName && field.Name.StartsWith("__<", StringComparison.Ordinal))
                    {
                        // skip
                    }
                    else
                    {
                        fields.Add(CreateFieldWrapper(field, hideFromJavaFlags));
                    }
                }
            }
            foreach (PropertyInfo property in properties)
            {
                if (property != null)
                {
                    AddPropertyFieldWrapper(fields, property, null);
                }
            }
            SetFields(fields.ToArray());
        }

        private static bool MatchTypes(TypeWrapper tw1, TypeWrapper tw2)
        {
            return tw1 == tw2 || (tw1.IsUnloadable && tw2.IsUnloadable && tw1.Name == tw2.Name);
        }

        private void AddPropertyFieldWrapper(List<FieldWrapper> fields, PropertyInfo property, FieldInfo field)
        {
            // NOTE explictly defined properties (in map.xml) are decorated with HideFromJava,
            // so we don't need to worry about them here
            HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(property);
            if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
            {
                // is it a type 1 access stub?
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    fields.Add(new CompiledAccessStubFieldWrapper(this, property, GetPropertyTypeWrapper(property)));
                }
                else
                {
                    // It must be an explicit property
                    // (defined in Java source by an @ikvm.lang.Property annotation)
                    ModifiersAttribute mods = AttributeHelper.GetModifiersAttribute(property);
                    fields.Add(new CompiledPropertyFieldWrapper(this, property, new ExModifiers(mods.Modifiers, mods.IsInternal)));
                }
            }
        }

        private sealed class CompiledRemappedMethodWrapper : SmartMethodWrapper
        {
            private readonly MethodInfo mbHelper;
#if !IMPORTER
            private readonly MethodInfo mbNonvirtualHelper;
#endif

            internal CompiledRemappedMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, ExModifiers modifiers, bool hideFromReflection, MethodInfo mbHelper, MethodInfo mbNonvirtualHelper)
                : base(declaringType, name, sig, method, returnType, parameterTypes, modifiers.Modifiers,
                        (modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None))
            {
                this.mbHelper = mbHelper;
#if !IMPORTER
                this.mbNonvirtualHelper = mbNonvirtualHelper;
#endif
            }

#if EMITTERS
            protected override void CallImpl(CodeEmitter ilgen)
            {
                MethodBase mb = GetMethod();
                MethodInfo mi = mb as MethodInfo;
                if (mi != null)
                {
                    if (!IsStatic && IsFinal)
                    {
                        // When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
                        // then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
                        // is final, this won't affect the semantics.
                        CallvirtImpl(ilgen);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Call, mi);
                    }
                }
                else
                {
                    ilgen.Emit(OpCodes.Call, mb);
                }
            }

            protected override void CallvirtImpl(CodeEmitter ilgen)
            {
                Debug.Assert(!mbHelper.IsStatic || mbHelper.Name.StartsWith("instancehelper_") || mbHelper.DeclaringType.Name == "__Helper");
                if (mbHelper.IsPublic)
                {
                    ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
                }
                else
                {
                    // HACK the helper is not public, this means that we're dealing with finalize or clone
                    ilgen.Emit(OpCodes.Callvirt, GetMethod());
                }
            }

            protected override void NewobjImpl(CodeEmitter ilgen)
            {
                MethodBase mb = GetMethod();
                MethodInfo mi = mb as MethodInfo;
                if (mi != null)
                {
                    Debug.Assert(mi.Name == "newhelper");
                    ilgen.Emit(OpCodes.Call, mi);
                }
                else
                {
                    ilgen.Emit(OpCodes.Newobj, mb);
                }
            }
#endif // EMITTERS

#if !IMPORTER && !FIRST_PASS && !EXPORTER
            [HideFromJava]
            internal override object Invoke(object obj, object[] args)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                if (mb.IsStatic && !IsStatic)
                {
                    args = ArrayUtil.Concat(obj, args);
                    obj = null;
                }
                return InvokeAndUnwrapException(mb, obj, args);
            }

            [HideFromJava]
            internal override object CreateInstance(object[] args)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                if (mb.IsStatic)
                {
                    return InvokeAndUnwrapException(mb, null, args);
                }
                return base.CreateInstance(args);
            }

            [HideFromJava]
            internal override object InvokeNonvirtualRemapped(object obj, object[] args)
            {
                MethodInfo mi = mbNonvirtualHelper;
                if (mi == null)
                {
                    mi = mbHelper;
                }
                return mi.Invoke(null, ArrayUtil.Concat(obj, args));
            }
#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

#if EMITTERS
            internal override void EmitCallvirtReflect(CodeEmitter ilgen)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                ilgen.Emit(mb.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mb);
            }
#endif // EMITTERS

            internal string GetGenericSignature()
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(mbHelper != null ? mbHelper : GetMethod());
                if (attr != null)
                {
                    return attr.Signature;
                }
                return null;
            }
        }

        private static TypeWrapper TypeWrapperFromModOpt(Type[] modopt)
        {
            int rank = 0;
            TypeWrapper tw = null;
            foreach (Type type in modopt)
            {
                if (type == JVM.LoadType(typeof(IKVM.Attributes.AccessStub)))
                {
                    // ignore
                }
                else if (type == Types.Array)
                {
                    rank++;
                }
                else if (type == Types.Void || type.IsPrimitive || ClassLoaderWrapper.IsRemappedType(type))
                {
                    tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
                }
                else if (type.DeclaringType != null && type.DeclaringType.FullName == UnloadableTypeWrapper.ContainerTypeName)
                {
                    tw = new UnloadableTypeWrapper(TypeNameUtil.UnmangleNestedTypeName(type.Name), type);
                }
                else
                {
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);
                }
            }
            if (rank != 0)
            {
                tw = tw.MakeArrayType(rank);
            }
            return tw;
        }

        private static TypeWrapper GetPropertyTypeWrapper(PropertyInfo property)
        {
            return TypeWrapperFromModOpt(property.GetOptionalCustomModifiers())
                ?? ClassLoaderWrapper.GetWrapperFromType(property.PropertyType);
        }

        internal static TypeWrapper GetFieldTypeWrapper(FieldInfo field)
        {
            return TypeWrapperFromModOpt(field.GetOptionalCustomModifiers())
                ?? ClassLoaderWrapper.GetWrapperFromType(field.FieldType);
        }

        internal static TypeWrapper GetParameterTypeWrapper(ParameterInfo param)
        {
            TypeWrapper tw = TypeWrapperFromModOpt(param.GetOptionalCustomModifiers());
            if (tw != null)
            {
                return tw;
            }
            Type parameterType = param.ParameterType;
            if (parameterType.IsByRef)
            {
                // we only support ByRef parameters for automatically generated delegate invoke stubs
                parameterType = parameterType.GetElementType().MakeArrayType();
            }
            return ClassLoaderWrapper.GetWrapperFromType(parameterType);
        }

        private FieldWrapper CreateFieldWrapper(FieldInfo field, HideFromJavaFlags hideFromJavaFlags)
        {
            ExModifiers modifiers = AttributeHelper.GetModifiers(field, false);
            TypeWrapper type = GetFieldTypeWrapper(field);
            string name = field.Name;

            if (field.IsSpecialName)
            {
                name = UnicodeUtil.UnescapeInvalidSurrogates(name);
            }

            if (field.IsLiteral)
            {
                MemberFlags flags = MemberFlags.None;
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    flags |= MemberFlags.HideFromReflection;
                }
                if (modifiers.IsInternal)
                {
                    flags |= MemberFlags.InternalAccess;
                }
                return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers.Modifiers, field, null, flags);
            }
            else
            {
                return FieldWrapper.Create(this, type, field, name, type.SigName, modifiers);
            }
        }

        internal override Type TypeAsTBD
        {
            get
            {
                return type;
            }
        }

        internal override bool IsMapUnsafeException
        {
            get
            {
                return AttributeHelper.IsExceptionIsUnsafeForMapping(type);
            }
        }

#if EMITTERS
        internal override void EmitRunClassConstructor(CodeEmitter ilgen)
        {
            if (HasStaticInitializer)
            {
                ilgen.Emit(OpCodes.Call, clinitMethod);
            }
        }
#endif // EMITTERS

        internal override string GetGenericSignature()
        {
            SignatureAttribute attr = AttributeHelper.GetSignature(type);
            if (attr != null)
            {
                return attr.Signature;
            }
            return null;
        }

        internal override string GetGenericMethodSignature(MethodWrapper mw)
        {
            if (mw is CompiledRemappedMethodWrapper)
            {
                return ((CompiledRemappedMethodWrapper)mw).GetGenericSignature();
            }
            MethodBase mb = mw.GetMethod();
            if (mb != null)
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(mb);
                if (attr != null)
                {
                    return attr.Signature;
                }
            }
            return null;
        }

        internal override string GetGenericFieldSignature(FieldWrapper fw)
        {
            FieldInfo fi = fw.GetField();
            if (fi != null)
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(fi);
                if (attr != null)
                {
                    return attr.Signature;
                }
            }
            return null;
        }

        internal override MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            MethodParametersAttribute attr = AttributeHelper.GetMethodParameters(mb);
            if (attr == null)
            {
                return null;
            }
            if (attr.IsMalformed)
            {
                return MethodParametersEntry.Malformed;
            }
            ParameterInfo[] parameters = mb.GetParameters();
            MethodParametersEntry[] mp = new MethodParametersEntry[attr.Modifiers.Length];
            for (int i = 0; i < mp.Length; i++)
            {
                mp[i].name = i < parameters.Length ? parameters[i].Name : null;
                mp[i].flags = (ushort)attr.Modifiers[i];
            }
            return mp;
        }

#if !IMPORTER && !EXPORTER
        internal override string[] GetEnclosingMethod()
        {
            EnclosingMethodAttribute enc = AttributeHelper.GetEnclosingMethodAttribute(type);
            if (enc != null)
            {
                return new string[] { enc.ClassName, enc.MethodName, enc.MethodSignature };
            }
            return null;
        }

        internal override object[] GetDeclaredAnnotations()
        {
            return type.GetCustomAttributes(false);
        }

        internal override object[] GetMethodAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            return mb.GetCustomAttributes(false);
        }

        internal override object[][] GetParameterAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            ParameterInfo[] parameters = mb.GetParameters();
            int skip = 0;
            if (mb.IsStatic && !mw.IsStatic && mw.Name != "<init>")
            {
                skip = 1;
            }
            int skipEnd = 0;
            if (mw.HasCallerID)
            {
                skipEnd = 1;
            }
            object[][] attribs = new object[parameters.Length - skip - skipEnd][];
            for (int i = skip; i < parameters.Length - skipEnd; i++)
            {
                attribs[i - skip] = parameters[i].GetCustomAttributes(false);
            }
            return attribs;
        }

        internal override object[] GetFieldAnnotations(FieldWrapper fw)
        {
            FieldInfo field = fw.GetField();
            if (field != null)
            {
                return field.GetCustomAttributes(false);
            }
            CompiledPropertyFieldWrapper prop = fw as CompiledPropertyFieldWrapper;
            if (prop != null)
            {
                return prop.GetProperty().GetCustomAttributes(false);
            }
            return new object[0];
        }
#endif

        internal sealed class CompiledAnnotation : Annotation
        {
            private readonly ConstructorInfo constructor;

            internal CompiledAnnotation(Type type)
            {
                constructor = type.GetConstructor(new Type[] { JVM.Import(typeof(object[])) });
            }

            private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
            {
                return new CustomAttributeBuilder(constructor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
            }

            internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
            {
                tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
            {
                mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
            {
                fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
            {
                pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
            {
                ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
            {
                pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override bool IsCustomAttribute
            {
                get { return false; }
            }
        }

        internal override Annotation Annotation
        {
            get
            {
                string annotationAttribute = AttributeHelper.GetAnnotationAttributeType(type);
                if (annotationAttribute != null)
                {
                    return new CompiledAnnotation(type.Assembly.GetType(annotationAttribute, true));
                }
                return null;
            }
        }

        internal override Type EnumType
        {
            get
            {
                if ((this.Modifiers & Modifiers.Enum) != 0)
                {
                    return type.GetNestedType("__Enum");
                }
                return null;
            }
        }

#if !IMPORTER && !EXPORTER
        internal override string GetSourceFileName()
        {
            object[] attr = type.GetCustomAttributes(typeof(SourceFileAttribute), false);
            if (attr.Length == 1)
            {
                return ((SourceFileAttribute)attr[0]).SourceFile;
            }
            if (DeclaringTypeWrapper != null)
            {
                return DeclaringTypeWrapper.GetSourceFileName();
            }
            if (IsNestedTypeAnonymousOrLocalClass(type))
            {
                return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetSourceFileName();
            }
            if (type.Module.IsDefined(typeof(SourceFileAttribute), false))
            {
                return type.Name + ".java";
            }
            return null;
        }

        internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
        {
            object[] attr = mb.GetCustomAttributes(typeof(LineNumberTableAttribute), false);
            if (attr.Length == 1)
            {
                return ((LineNumberTableAttribute)attr[0]).GetLineNumber(ilOffset);
            }
            return -1;
        }
#endif

        internal override bool IsFastClassLiteralSafe
        {
            get { return true; }
        }

        internal override object[] GetConstantPool()
        {
            return AttributeHelper.GetConstantPool(type);
        }

        internal override byte[] GetRawTypeAnnotations()
        {
            return AttributeHelper.GetRuntimeVisibleTypeAnnotations(type);
        }

        internal override byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            return mb == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(mb);
        }

        internal override byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
        {
            FieldInfo fi = fw.GetField();
            return fi == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(fi);
        }
    }

    sealed class ArrayTypeWrapper : TypeWrapper
    {

        static volatile TypeWrapper[] interfaces;
        static volatile MethodInfo clone;
        readonly TypeWrapper ultimateElementTypeWrapper;
        Type arrayType;
        bool finished;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ultimateElementTypeWrapper"></param>
        /// <param name="name"></param>
        internal ArrayTypeWrapper(TypeWrapper ultimateElementTypeWrapper, string name) :
            base(ultimateElementTypeWrapper.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.Modifiers & Modifiers.Public), name)
        {
            Debug.Assert(!ultimateElementTypeWrapper.IsArray);
            this.ultimateElementTypeWrapper = ultimateElementTypeWrapper;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return CoreClasses.java.lang.Object.Wrapper; }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return ultimateElementTypeWrapper.GetClassLoader();
        }

        internal static MethodInfo CloneMethod
        {
            get
            {
                if (clone == null)
                    clone = Types.Array.GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

                return clone;
            }
        }

        protected override void LazyPublishMembers()
        {
            var mw = new SimpleCallMethodWrapper(this, "clone", "()Ljava.lang.Object;", CloneMethod, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray, Modifiers.Public, MemberFlags.HideFromReflection, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt);
            mw.Link();
            SetMethods(new MethodWrapper[] { mw });
            SetFields(FieldWrapper.EmptyArray);
        }

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                return Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.ReflectiveModifiers & Modifiers.AccessMask);
            }
        }

        internal override string SigName
        {
            get
            {
                // for arrays the signature name is the same as the normal name
                return Name;
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                if (interfaces == null)
                {
                    TypeWrapper[] tw = new TypeWrapper[2];
                    tw[0] = CoreClasses.java.lang.Cloneable.Wrapper;
                    tw[1] = CoreClasses.java.io.Serializable.Wrapper;
                    interfaces = tw;
                }
                return interfaces;
            }
        }

        internal override Type TypeAsTBD
        {
            get
            {
                while (arrayType == null)
                {
                    bool prevFinished = finished;
                    Type type = MakeArrayType(ultimateElementTypeWrapper.TypeAsArrayType, this.ArrayRank);
                    if (prevFinished)
                    {
                        // We were already finished prior to the call to MakeArrayType, so we can safely
                        // set arrayType to the finished type.
                        // Note that this takes advantage of the fact that once we've been finished,
                        // we can never become unfinished.
                        arrayType = type;
                    }
                    else
                    {
                        lock (this)
                        {
                            // To prevent a race with Finish, we can only set arrayType in this case
                            // (inside the locked region) if we've not already finished. If we have
                            // finished, we need to rerun MakeArrayType on the now finished element type.
                            // Note that there is a benign race left, because it is possible that another
                            // thread finishes right after we've set arrayType and exited the locked
                            // region. This is not problem, because TypeAsTBD is only guaranteed to
                            // return a finished type *after* Finish has been called.
                            if (!finished)
                            {
                                arrayType = type;
                            }
                        }
                    }
                }

                return arrayType;
            }
        }

        internal override void Finish()
        {
            if (!finished)
            {
                ultimateElementTypeWrapper.Finish();
                lock (this)
                {
                    // Now that we've finished the element type, we must clear arrayType,
                    // because it may still refer to a TypeBuilder. Note that we have to
                    // do this atomically with setting "finished", to prevent a race
                    // with TypeAsTBD.
                    finished = true;
                    arrayType = null;
                }
            }
        }

        internal override bool IsFastClassLiteralSafe
        {
            // here we have to deal with the somewhat strange fact that in Java you cannot represent primitive type class literals,
            // but you can represent arrays of primitive types as a class literal
            get { return ultimateElementTypeWrapper.IsFastClassLiteralSafe || ultimateElementTypeWrapper.IsPrimitive; }
        }

        internal override TypeWrapper GetUltimateElementTypeWrapper()
        {
            return ultimateElementTypeWrapper;
        }

        internal static Type MakeArrayType(Type type, int dims)
        {
            // NOTE this is not just an optimization, but it is also required to
            // make sure that ReflectionOnly types stay ReflectionOnly types
            // (in particular instantiations of generic types from mscorlib that
            // have ReflectionOnly type parameters).
            for (int i = 0; i < dims; i++)
            {
                type = type.MakeArrayType();
            }
            return type;
        }
    }

    // this is a container for the special verifier TypeWrappers
    sealed class VerifierTypeWrapper : TypeWrapper
    {

        // the TypeWrapper constructor interns the name, so we have to pre-intern here to make sure we have the same string object
        // (if it has only been interned previously)
        static readonly string This = string.Intern("this");
        static readonly string New = string.Intern("new");
        static readonly string Fault = string.Intern("<fault>");
        internal static readonly TypeWrapper Invalid = null;
        internal static readonly TypeWrapper Null = new VerifierTypeWrapper("null", 0, null, null);
        internal static readonly TypeWrapper UninitializedThis = new VerifierTypeWrapper("uninitialized-this", 0, null, null);
        internal static readonly TypeWrapper Unloadable = new UnloadableTypeWrapper("<verifier>");
        internal static readonly TypeWrapper ExtendedFloat = new VerifierTypeWrapper("<extfloat>", 0, null, null);
        internal static readonly TypeWrapper ExtendedDouble = new VerifierTypeWrapper("<extdouble>", 0, null, null);

        readonly int index;
        readonly TypeWrapper underlyingType;
        readonly MethodAnalyzer methodAnalyzer;

#if EXPORTER

        internal class MethodAnalyzer
        {
            internal void ClearFaultBlockException(int dummy) { }
        }

#endif

        public override string ToString()
        {
            return GetType().Name + "[" + Name + "," + index + "," + underlyingType + "]";
        }

        internal static TypeWrapper MakeNew(TypeWrapper type, int bytecodeIndex)
        {
            return new VerifierTypeWrapper(New, bytecodeIndex, type, null);
        }

        internal static TypeWrapper MakeFaultBlockException(MethodAnalyzer ma, int handlerIndex)
        {
            return new VerifierTypeWrapper(Fault, handlerIndex, null, ma);
        }

        // NOTE the "this" type is special, it can only exist in local[0] and on the stack
        // as soon as the type on the stack is merged or popped it turns into its underlying type.
        // It exists to capture the verification rules for non-virtual base class method invocation in .NET 2.0,
        // which requires that the invocation is done on a "this" reference that was directly loaded onto the
        // stack (using ldarg_0).
        internal static TypeWrapper MakeThis(TypeWrapper type)
        {
            return new VerifierTypeWrapper(This, 0, type, null);
        }

        internal static bool IsNotPresentOnStack(TypeWrapper w)
        {
            return IsNew(w) || IsFaultBlockException(w);
        }

        internal static bool IsNew(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, New);
        }

        internal static bool IsFaultBlockException(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, Fault);
        }

        internal static bool IsNullOrUnloadable(TypeWrapper w)
        {
            return w == Null || w.IsUnloadable;
        }

        internal static bool IsThis(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, This);
        }

        internal static void ClearFaultBlockException(TypeWrapper w)
        {
            VerifierTypeWrapper vtw = (VerifierTypeWrapper)w;
            vtw.methodAnalyzer.ClearFaultBlockException(vtw.Index);
        }

        internal int Index
        {
            get
            {
                return index;
            }
        }

        internal TypeWrapper UnderlyingType
        {
            get
            {
                return underlyingType;
            }
        }

        private VerifierTypeWrapper(string name, int index, TypeWrapper underlyingType, MethodAnalyzer methodAnalyzer)
            : base(TypeFlags.None, TypeWrapper.VerifierTypeModifiersHack, name)
        {
            this.index = index;
            this.underlyingType = underlyingType;
            this.methodAnalyzer = methodAnalyzer;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return null; }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return null;
        }

        protected override void LazyPublishMembers()
        {
            throw new InvalidOperationException("LazyPublishMembers called on " + this);
        }

        internal override Type TypeAsTBD
        {
            get
            {
                throw new InvalidOperationException("get_Type called on " + this);
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                throw new InvalidOperationException("get_Interfaces called on " + this);
            }
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                throw new InvalidOperationException("get_InnerClasses called on " + this);
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                throw new InvalidOperationException("get_DeclaringTypeWrapper called on " + this);
            }
        }

        internal override void Finish()
        {
            throw new InvalidOperationException("Finish called on " + this);
        }
    }

#if !IMPORTER && !EXPORTER

    /// <summary>
    /// Represents an intrinsified anonymous class. Currently only used by LambdaMetafactory.
    /// </summary>
    sealed class AnonymousTypeWrapper : TypeWrapper
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal AnonymousTypeWrapper(Type type) :
            base(TypeFlags.Anonymous, Modifiers.Final | Modifiers.Synthetic, GetName(type))
        {
            this.type = type;
        }

        internal static bool IsAnonymous(Type type)
        {
            return type.IsSpecialName
                && type.Name.StartsWith(NestedTypeName.IntrinsifiedAnonymousClass, StringComparison.Ordinal)
                && AttributeHelper.IsJavaModule(type.Module);
        }

        private static string GetName(Type type)
        {
            return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).Name
                + type.Name.Replace(NestedTypeName.IntrinsifiedAnonymousClass, "$$Lambda$");
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetClassLoader();
        }

        internal override Type TypeAsTBD
        {
            get { return type; }
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return CoreClasses.java.lang.Object.Wrapper; }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                TypeWrapper[] interfaces = GetImplementedInterfacesAsTypeWrappers(type);
                if (type.IsSerializable)
                {
                    // we have to remove the System.Runtime.Serialization.ISerializable interface
                    List<TypeWrapper> list = new List<TypeWrapper>(interfaces);
                    list.RemoveAll(Serialization.IsISerializable);
                    return list.ToArray();
                }
                return interfaces;
            }
        }

        protected override void LazyPublishMembers()
        {
            List<MethodWrapper> methods = new List<MethodWrapper>();
            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (mi.IsSpecialName)
                {
                    // we use special name to hide default methods
                }
                else if (mi.IsPublic)
                {
                    TypeWrapper returnType;
                    TypeWrapper[] parameterTypes;
                    string signature;
                    GetSig(mi, out returnType, out parameterTypes, out signature);
                    methods.Add(new TypicalMethodWrapper(this, mi.Name, signature, mi, returnType, parameterTypes, Modifiers.Public, MemberFlags.None));
                }
                else if (mi.Name == "writeReplace")
                {
                    methods.Add(new TypicalMethodWrapper(this, "writeReplace", "()Ljava.lang.Object;", mi, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray,
                        Modifiers.Private | Modifiers.Final, MemberFlags.None));
                }
            }
            SetMethods(methods.ToArray());
            List<FieldWrapper> fields = new List<FieldWrapper>();
            foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                TypeWrapper fieldType = CompiledTypeWrapper.GetFieldTypeWrapper(fi);
                fields.Add(new SimpleFieldWrapper(this, fieldType, fi, fi.Name, fieldType.SigName, new ExModifiers(Modifiers.Private | Modifiers.Final, false)));
            }
            SetFields(fields.ToArray());
        }

        private void GetSig(MethodInfo mi, out TypeWrapper returnType, out TypeWrapper[] parameterTypes, out string signature)
        {
            returnType = CompiledTypeWrapper.GetParameterTypeWrapper(mi.ReturnParameter);
            ParameterInfo[] parameters = mi.GetParameters();
            parameterTypes = new TypeWrapper[parameters.Length];
            System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = CompiledTypeWrapper.GetParameterTypeWrapper(parameters[i]);
                sb.Append(parameterTypes[i].SigName);
            }
            sb.Append(')');
            sb.Append(returnType.SigName);
            signature = sb.ToString();
        }
    }

#endif

}
