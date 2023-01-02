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

namespace IKVM.Internal
{

    static class AttributeHelper
    {

#if IMPORTER

        static CustomAttributeBuilder ghostInterfaceAttribute;
        static CustomAttributeBuilder deprecatedAttribute;
        static CustomAttributeBuilder editorBrowsableNever;
        static ConstructorInfo implementsAttribute;
        static ConstructorInfo throwsAttribute;
        static ConstructorInfo sourceFileAttribute;
        static ConstructorInfo lineNumberTableAttribute1;
        static ConstructorInfo lineNumberTableAttribute2;
        static ConstructorInfo enclosingMethodAttribute;
        static ConstructorInfo signatureAttribute;
        static ConstructorInfo methodParametersAttribute;
        static ConstructorInfo runtimeVisibleTypeAnnotationsAttribute;
        static ConstructorInfo constantPoolAttribute;
        static CustomAttributeBuilder paramArrayAttribute;
        static ConstructorInfo nonNestedInnerClassAttribute;
        static ConstructorInfo nonNestedOuterClassAttribute;
        static readonly Type typeofModifiers = JVM.LoadType(typeof(Modifiers));
        static readonly Type typeofSourceFileAttribute = JVM.LoadType(typeof(SourceFileAttribute));
        static readonly Type typeofLineNumberTableAttribute = JVM.LoadType(typeof(LineNumberTableAttribute));
#endif // IMPORTER
        static readonly Type typeofRemappedClassAttribute = JVM.LoadType(typeof(RemappedClassAttribute));
        static readonly Type typeofRemappedTypeAttribute = JVM.LoadType(typeof(RemappedTypeAttribute));
        static readonly Type typeofModifiersAttribute = JVM.LoadType(typeof(ModifiersAttribute));
        static readonly Type typeofRemappedInterfaceMethodAttribute = JVM.LoadType(typeof(RemappedInterfaceMethodAttribute));
        static readonly Type typeofNameSigAttribute = JVM.LoadType(typeof(NameSigAttribute));
        static readonly Type typeofJavaModuleAttribute = JVM.LoadType(typeof(JavaModuleAttribute));
        static readonly Type typeofSignatureAttribute = JVM.LoadType(typeof(SignatureAttribute));
        static readonly Type typeofInnerClassAttribute = JVM.LoadType(typeof(InnerClassAttribute));
        static readonly Type typeofImplementsAttribute = JVM.LoadType(typeof(ImplementsAttribute));
        static readonly Type typeofGhostInterfaceAttribute = JVM.LoadType(typeof(GhostInterfaceAttribute));
        static readonly Type typeofExceptionIsUnsafeForMappingAttribute = JVM.LoadType(typeof(ExceptionIsUnsafeForMappingAttribute));
        static readonly Type typeofThrowsAttribute = JVM.LoadType(typeof(ThrowsAttribute));
        static readonly Type typeofHideFromJavaAttribute = JVM.LoadType(typeof(HideFromJavaAttribute));
        static readonly Type typeofHideFromJavaFlags = JVM.LoadType(typeof(HideFromJavaFlags));
        static readonly Type typeofNoPackagePrefixAttribute = JVM.LoadType(typeof(NoPackagePrefixAttribute));
        static readonly Type typeofAnnotationAttributeAttribute = JVM.LoadType(typeof(AnnotationAttributeAttribute));
        static readonly Type typeofNonNestedInnerClassAttribute = JVM.LoadType(typeof(NonNestedInnerClassAttribute));
        static readonly Type typeofNonNestedOuterClassAttribute = JVM.LoadType(typeof(NonNestedOuterClassAttribute));
        static readonly Type typeofEnclosingMethodAttribute = JVM.LoadType(typeof(EnclosingMethodAttribute));
        static readonly Type typeofMethodParametersAttribute = JVM.LoadType(typeof(MethodParametersAttribute));
        static readonly Type typeofRuntimeVisibleTypeAnnotationsAttribute = JVM.LoadType(typeof(RuntimeVisibleTypeAnnotationsAttribute));
        static readonly Type typeofConstantPoolAttribute = JVM.LoadType(typeof(ConstantPoolAttribute));
        static readonly CustomAttributeBuilder hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
        static readonly CustomAttributeBuilder hideFromReflection = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(new Type[] { typeofHideFromJavaFlags }), new object[] { HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk });

        // we don't want beforefieldinit
        static AttributeHelper()
        {

        }

#if IMPORTER

        private static object ParseValue(ClassLoaderWrapper loader, TypeWrapper tw, string val)
        {
            if (tw == CoreClasses.java.lang.String.Wrapper)
            {
                return val;
            }
            else if (tw.IsUnloadable)
            {
                throw new FatalCompilerErrorException(Message.MapFileTypeNotFound, tw.Name);
            }
            else if (tw.TypeAsTBD.IsEnum)
            {
                return EnumHelper.Parse(tw.TypeAsTBD, val);
            }
            else if (tw.TypeAsTBD == Types.Type)
            {
                TypeWrapper valtw = loader.LoadClassByDottedNameFast(val);
                if (valtw != null)
                {
                    return valtw.TypeAsBaseType;
                }
                return StaticCompiler.Universe.GetType(val, true);
            }
            else if (tw == PrimitiveTypeWrapper.BOOLEAN)
            {
                return bool.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.BYTE)
            {
                return (byte)sbyte.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.CHAR)
            {
                return char.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.SHORT)
            {
                return short.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.INT)
            {
                return int.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.FLOAT)
            {
                return float.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.LONG)
            {
                return long.Parse(val);
            }
            else if (tw == PrimitiveTypeWrapper.DOUBLE)
            {
                return double.Parse(val);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, TypeBuilder tb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            tb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, FieldBuilder fb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            fb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, ParameterBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, MethodBuilder mb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            mb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, PropertyBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal static void SetCustomAttribute(ClassLoaderWrapper loader, AssemblyBuilder ab, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            ab.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        private static void GetAttributeArgsAndTypes(ClassLoaderWrapper loader, IKVM.Tools.Importer.MapXml.Attribute attr, out Type[] argTypes, out object[] args)
        {
            // TODO add error handling
            TypeWrapper[] twargs = loader.ArgTypeWrapperListFromSig(attr.Sig, LoadMode.Link);
            argTypes = new Type[twargs.Length];
            args = new object[argTypes.Length];
            for (int i = 0; i < twargs.Length; i++)
            {
                argTypes[i] = twargs[i].TypeAsSignatureType;
                TypeWrapper tw = twargs[i];
                if (tw == CoreClasses.java.lang.Object.Wrapper)
                {
                    tw = loader.FieldTypeWrapperFromSig(attr.Params[i].Sig, LoadMode.Link);
                }
                if (tw.IsArray)
                {
                    Array arr = Array.CreateInstance(Type.__GetSystemType(Type.GetTypeCode(tw.ElementTypeWrapper.TypeAsArrayType)), attr.Params[i].Elements.Length);
                    for (int j = 0; j < arr.Length; j++)
                    {
                        arr.SetValue(ParseValue(loader, tw.ElementTypeWrapper, attr.Params[i].Elements[j].Value), j);
                    }
                    args[i] = arr;
                }
                else
                {
                    args[i] = ParseValue(loader, tw, attr.Params[i].Value);
                }
            }
        }

        static CustomAttributeBuilder CreateCustomAttribute(ClassLoaderWrapper loader, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            // TODO add error handling
            Type[] argTypes;
            object[] args;
            GetAttributeArgsAndTypes(loader, attr, out argTypes, out args);
            if (attr.Type != null)
            {
                Type t = StaticCompiler.GetTypeForMapXml(loader, attr.Type);
                ConstructorInfo ci = t.GetConstructor(argTypes);
                if (ci == null)
                {
                    throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Type, attr.Sig));
                }
                PropertyInfo[] namedProperties;
                object[] propertyValues;
                if (attr.Properties != null)
                {
                    namedProperties = new PropertyInfo[attr.Properties.Length];
                    propertyValues = new object[attr.Properties.Length];
                    for (int i = 0; i < namedProperties.Length; i++)
                    {
                        namedProperties[i] = t.GetProperty(attr.Properties[i].Name);
                        propertyValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Properties[i].Sig, LoadMode.Link), attr.Properties[i].Value);
                    }
                }
                else
                {
                    namedProperties = new PropertyInfo[0];
                    propertyValues = new object[0];
                }
                FieldInfo[] namedFields;
                object[] fieldValues;
                if (attr.Fields != null)
                {
                    namedFields = new FieldInfo[attr.Fields.Length];
                    fieldValues = new object[attr.Fields.Length];
                    for (int i = 0; i < namedFields.Length; i++)
                    {
                        namedFields[i] = t.GetField(attr.Fields[i].Name);
                        fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
                    }
                }
                else
                {
                    namedFields = new FieldInfo[0];
                    fieldValues = new object[0];
                }
                return new CustomAttributeBuilder(ci, args, namedProperties, propertyValues, namedFields, fieldValues);
            }
            else
            {
                if (attr.Properties != null)
                {
                    throw new NotImplementedException("Setting property values on Java attributes is not implemented");
                }
                TypeWrapper t = loader.LoadClassByDottedName(attr.Class);
                FieldInfo[] namedFields;
                object[] fieldValues;
                if (attr.Fields != null)
                {
                    namedFields = new FieldInfo[attr.Fields.Length];
                    fieldValues = new object[attr.Fields.Length];
                    for (int i = 0; i < namedFields.Length; i++)
                    {
                        FieldWrapper fw = t.GetFieldWrapper(attr.Fields[i].Name, attr.Fields[i].Sig);
                        fw.Link();
                        namedFields[i] = fw.GetField();
                        fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
                    }
                }
                else
                {
                    namedFields = new FieldInfo[0];
                    fieldValues = new object[0];
                }
                MethodWrapper mw = t.GetMethodWrapper("<init>", attr.Sig, false);
                if (mw == null)
                {
                    throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));
                }
                mw.Link();
                ConstructorInfo ci = (mw.GetMethod() as ConstructorInfo) ?? ((MethodInfo)mw.GetMethod()).__AsConstructorInfo();
                return new CustomAttributeBuilder(ci, args, namedFields, fieldValues);
            }
        }

        private static CustomAttributeBuilder GetEditorBrowsableNever()
        {
            if (editorBrowsableNever == null)
            {
                // to avoid having to load (and find) System.dll, we construct a symbolic CustomAttributeBuilder
                AssemblyName name = Types.Object.Assembly.GetName();
#if NETFRAMEWORK
                name.Name = "System";
#endif
                Universe u = StaticCompiler.Universe;
                Type typeofEditorBrowsableAttribute = u.ResolveType(Types.Object.Assembly, "System.ComponentModel.EditorBrowsableAttribute, " + name.FullName);
                Type typeofEditorBrowsableState = u.ResolveType(Types.Object.Assembly, "System.ComponentModel.EditorBrowsableState, " + name.FullName);
                u.MissingTypeIsValueType += delegate (Type type) { return type == typeofEditorBrowsableState; };
                ConstructorInfo ctor = (ConstructorInfo)typeofEditorBrowsableAttribute.__CreateMissingMethod(ConstructorInfo.ConstructorName,
                    CallingConventions.Standard | CallingConventions.HasThis, null, default(CustomModifiers), new Type[] { typeofEditorBrowsableState }, null);
                editorBrowsableNever = CustomAttributeBuilder.__FromBlob(ctor, new byte[] { 01, 00, 01, 00, 00, 00, 00, 00 });
            }
            return editorBrowsableNever;
        }

        internal static void SetEditorBrowsableNever(TypeBuilder tb)
        {
            tb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal static void SetEditorBrowsableNever(MethodBuilder mb)
        {
            mb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal static void SetEditorBrowsableNever(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal static void SetDeprecatedAttribute(MethodBuilder mb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
            }
            mb.SetCustomAttribute(deprecatedAttribute);
        }

        internal static void SetDeprecatedAttribute(TypeBuilder tb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
            }
            tb.SetCustomAttribute(deprecatedAttribute);
        }

        internal static void SetDeprecatedAttribute(FieldBuilder fb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
            }
            fb.SetCustomAttribute(deprecatedAttribute);
        }

        internal static void SetDeprecatedAttribute(PropertyBuilder pb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
            }
            pb.SetCustomAttribute(deprecatedAttribute);
        }

        internal static void SetThrowsAttribute(MethodBuilder mb, string[] exceptions)
        {
            if (exceptions != null && exceptions.Length != 0)
            {
                if (throwsAttribute == null)
                {
                    throwsAttribute = typeofThrowsAttribute.GetConstructor(new Type[] { JVM.Import(typeof(string[])) });
                }
                exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
                mb.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute, new object[] { exceptions }));
            }
        }

        internal static void SetGhostInterface(TypeBuilder typeBuilder)
        {
            if (ghostInterfaceAttribute == null)
            {
                ghostInterfaceAttribute = new CustomAttributeBuilder(typeofGhostInterfaceAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
            }
            typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
        }

        internal static void SetNonNestedInnerClass(TypeBuilder typeBuilder, string className)
        {
            if (nonNestedInnerClassAttribute == null)
            {
                nonNestedInnerClassAttribute = typeofNonNestedInnerClassAttribute.GetConstructor(new Type[] { Types.String });
            }
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedInnerClassAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
        }

        internal static void SetNonNestedOuterClass(TypeBuilder typeBuilder, string className)
        {
            if (nonNestedOuterClassAttribute == null)
            {
                nonNestedOuterClassAttribute = typeofNonNestedOuterClassAttribute.GetConstructor(new Type[] { Types.String });
            }
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedOuterClassAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
        }
#endif // IMPORTER

        internal static void HideFromReflection(MethodBuilder mb)
        {
            mb.SetCustomAttribute(hideFromReflection);
        }

        internal static void HideFromReflection(FieldBuilder fb)
        {
            fb.SetCustomAttribute(hideFromReflection);
        }

        internal static void HideFromReflection(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(hideFromReflection);
        }

        internal static void HideFromJava(TypeBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(hideFromJavaAttribute);
        }

        internal static void HideFromJava(MethodBuilder mb)
        {
            mb.SetCustomAttribute(hideFromJavaAttribute);
        }

        internal static void HideFromJava(MethodBuilder mb, HideFromJavaFlags flags)
        {
            CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(new Type[] { typeofHideFromJavaFlags }), new object[] { flags });
            mb.SetCustomAttribute(cab);
        }

        internal static void HideFromJava(FieldBuilder fb)
        {
            fb.SetCustomAttribute(hideFromJavaAttribute);
        }

#if IMPORTER

        internal static void HideFromJava(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(hideFromJavaAttribute);
        }

#endif // IMPORTER

        internal static bool IsHideFromJava(Type type)
        {
            return type.IsDefined(typeofHideFromJavaAttribute, false) || (type.IsNested && (type.DeclaringType.IsDefined(typeofHideFromJavaAttribute, false) || type.Name.StartsWith("__<", StringComparison.Ordinal)));
        }

        internal static bool IsHideFromJava(MemberInfo mi)
        {
            return (GetHideFromJavaFlags(mi) & HideFromJavaFlags.Code) != 0;
        }

        internal static HideFromJavaFlags GetHideFromJavaFlags(MemberInfo mi)
        {
            // NOTE all privatescope fields and methods are "hideFromJava"
            // because Java cannot deal with the potential name clashes
            var fi = mi as FieldInfo;
            if (fi != null && (fi.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
                return HideFromJavaFlags.All;

            var mb = mi as MethodBase;
            if (mb != null && (mb.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope)
                return HideFromJavaFlags.All;
            if (mi.Name.StartsWith("__<", StringComparison.Ordinal))
                return HideFromJavaFlags.All;

#if !IMPORTER && !EXPORTER

            var attr = mi.GetCustomAttributes(typeofHideFromJavaAttribute, false);
            if (attr.Length == 1)
                return ((HideFromJavaAttribute)attr[0]).Flags;

#else
            var attr = CustomAttributeData.__GetCustomAttributes(mi, typeofHideFromJavaAttribute, false);
            if (attr.Count == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Count == 1)
                    return (HideFromJavaFlags)args[0].Value;

                return HideFromJavaFlags.All;
            }
#endif

            return HideFromJavaFlags.None;
        }

#if IMPORTER

        internal static void SetImplementsAttribute(TypeBuilder typeBuilder, TypeWrapper[] ifaceWrappers)
        {
            var interfaces = new string[ifaceWrappers.Length];
            for (int i = 0; i < interfaces.Length; i++)
                interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);

            if (implementsAttribute == null)
                implementsAttribute = typeofImplementsAttribute.GetConstructor(new Type[] { JVM.Import(typeof(string[])) });

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { interfaces }));
        }

#endif

        internal static bool IsGhostInterface(Type type)
        {
            return type.IsDefined(typeofGhostInterfaceAttribute, false);
        }

        internal static bool IsRemappedType(Type type)
        {
            return type.IsDefined(typeofRemappedTypeAttribute, false);
        }

        internal static bool IsExceptionIsUnsafeForMapping(Type type)
        {
            return type.IsDefined(typeofExceptionIsUnsafeForMappingAttribute, false);
        }

        internal static ModifiersAttribute GetModifiersAttribute(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            var attr = member.GetCustomAttributes(typeof(ModifiersAttribute), false);
            return attr.Length == 1 ? (ModifiersAttribute)attr[0] : null;
#else
            var attr = CustomAttributeData.__GetCustomAttributes(member, typeofModifiersAttribute, false);
            if (attr.Count == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Count == 2)
                    return new ModifiersAttribute((Modifiers)args[0].Value, (bool)args[1].Value);

                return new ModifiersAttribute((Modifiers)args[0].Value);
            }

            return null;
#endif
        }

        internal static ExModifiers GetModifiers(MethodBase mb, bool assemblyIsPrivate)
        {
            var attr = GetModifiersAttribute(mb);
            if (attr != null)
                return new ExModifiers(attr.Modifiers, attr.IsInternal);

            Modifiers modifiers = 0;

            if (mb.IsPublic)
            {
                modifiers |= Modifiers.Public;
            }
            else if (mb.IsPrivate)
            {
                modifiers |= Modifiers.Private;
            }
            else if (mb.IsFamily || mb.IsFamilyOrAssembly)
            {
                modifiers |= Modifiers.Protected;
            }
            else if (assemblyIsPrivate)
            {
                modifiers |= Modifiers.Private;
            }

            // NOTE Java doesn't support non-virtual methods, but we set the Final modifier for
            // non-virtual methods to approximate the semantics
            if ((mb.IsFinal || (!mb.IsVirtual && ((modifiers & Modifiers.Private) == 0))) && !mb.IsStatic && !mb.IsConstructor)
            {
                modifiers |= Modifiers.Final;
            }

            if (mb.IsAbstract)
            {
                modifiers |= Modifiers.Abstract;
            }
            else
            {
                // Some .NET interfaces (like System._AppDomain) have synchronized methods,
                // Java doesn't allow synchronized on an abstract methods, so we ignore it for
                // abstract methods.
                if ((mb.GetMethodImplementationFlags() & MethodImplAttributes.Synchronized) != 0)
                {
                    modifiers |= Modifiers.Synchronized;
                }
            }

            if (mb.IsStatic)
            {
                modifiers |= Modifiers.Static;
            }

            if ((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
            {
                modifiers |= Modifiers.Native;
            }

            var parameters = mb.GetParameters();
            if (parameters.Length > 0 && parameters[parameters.Length - 1].IsDefined(JVM.Import(typeof(ParamArrayAttribute)), false))
                modifiers |= Modifiers.VarArgs;

            return new ExModifiers(modifiers, false);
        }

        internal static ExModifiers GetModifiers(FieldInfo fi, bool assemblyIsPrivate)
        {
            var attr = GetModifiersAttribute(fi);
            if (attr != null)
                return new ExModifiers(attr.Modifiers, attr.IsInternal);

            Modifiers modifiers = 0;
            if (fi.IsPublic)
            {
                modifiers |= Modifiers.Public;
            }
            else if (fi.IsPrivate)
            {
                modifiers |= Modifiers.Private;
            }
            else if (fi.IsFamily || fi.IsFamilyOrAssembly)
            {
                modifiers |= Modifiers.Protected;
            }
            else if (assemblyIsPrivate)
            {
                modifiers |= Modifiers.Private;
            }

            if (fi.IsInitOnly || fi.IsLiteral)
            {
                modifiers |= Modifiers.Final;
            }

            if (fi.IsNotSerialized)
            {
                modifiers |= Modifiers.Transient;
            }

            if (fi.IsStatic)
            {
                modifiers |= Modifiers.Static;
            }

            if (Array.IndexOf(fi.GetRequiredCustomModifiers(), Types.IsVolatile) != -1)
            {
                modifiers |= Modifiers.Volatile;
            }

            return new ExModifiers(modifiers, false);
        }

#if IMPORTER
        internal static void SetModifiers(MethodBuilder mb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
            else
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });

            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetModifiers(FieldBuilder fb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
            else
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });

            fb.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetModifiers(PropertyBuilder pb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
            else
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });

            pb.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetModifiers(TypeBuilder tb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
            else
                customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });

            tb.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetNameSig(MethodBuilder mb, string name, string sig)
        {
            var customAttributeBuilder = new CustomAttributeBuilder(typeofNameSigAttribute.GetConstructor(new Type[] { Types.String, Types.String }), new object[] { UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig) });
            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetInnerClass(TypeBuilder typeBuilder, string innerClass, Modifiers modifiers)
        {
            var argTypes = new Type[] { Types.String, typeofModifiers };
            var args = new object[] { UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers };
            var ci = typeofInnerClassAttribute.GetConstructor(argTypes);
            var customAttributeBuilder = new CustomAttributeBuilder(ci, args);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        internal static void SetSourceFile(TypeBuilder typeBuilder, string filename)
        {
            if (sourceFileAttribute == null)
                sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { Types.String });

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
        }

        internal static void SetSourceFile(ModuleBuilder moduleBuilder, string filename)
        {
            if (sourceFileAttribute == null)
            {
                sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { Types.String });
            }
            moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
        }

        internal static void SetLineNumberTable(MethodBuilder mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
        {
            object arg;
            ConstructorInfo con;
            if (writer.Count == 1)
            {
                if (lineNumberTableAttribute2 == null)
                {
                    lineNumberTableAttribute2 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { Types.UInt16 });
                }
                con = lineNumberTableAttribute2;
                arg = (ushort)writer.LineNo;
            }
            else
            {
                if (lineNumberTableAttribute1 == null)
                {
                    lineNumberTableAttribute1 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { JVM.Import(typeof(byte[])) });
                }
                con = lineNumberTableAttribute1;
                arg = writer.ToArray();
            }
            mb.SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { arg }));
        }

        internal static void SetEnclosingMethodAttribute(TypeBuilder tb, string className, string methodName, string methodSig)
        {
            if (enclosingMethodAttribute == null)
            {
                enclosingMethodAttribute = typeofEnclosingMethodAttribute.GetConstructor(new Type[] { Types.String, Types.String, Types.String });
            }
            tb.SetCustomAttribute(new CustomAttributeBuilder(enclosingMethodAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig) }));
        }

        internal static void SetSignatureAttribute(TypeBuilder tb, string signature)
        {
            if (signatureAttribute == null)
            {
                signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
            }
            tb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
        }

        internal static void SetSignatureAttribute(FieldBuilder fb, string signature)
        {
            if (signatureAttribute == null)
            {
                signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
            }
            fb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
        }

        internal static void SetSignatureAttribute(MethodBuilder mb, string signature)
        {
            if (signatureAttribute == null)
            {
                signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
            }
            mb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
                new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
        }

        internal static void SetMethodParametersAttribute(MethodBuilder mb, Modifiers[] modifiers)
        {
            if (methodParametersAttribute == null)
            {
                methodParametersAttribute = typeofMethodParametersAttribute.GetConstructor(new Type[] { typeofModifiers.MakeArrayType() });
            }
            mb.SetCustomAttribute(new CustomAttributeBuilder(methodParametersAttribute, new object[] { modifiers }));
        }

        internal static void SetRuntimeVisibleTypeAnnotationsAttribute(TypeBuilder tb, byte[] data)
        {
            if (runtimeVisibleTypeAnnotationsAttribute == null)
            {
                runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
            }
            tb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
        }

        internal static void SetRuntimeVisibleTypeAnnotationsAttribute(FieldBuilder fb, byte[] data)
        {
            if (runtimeVisibleTypeAnnotationsAttribute == null)
            {
                runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
            }
            fb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
        }

        internal static void SetRuntimeVisibleTypeAnnotationsAttribute(MethodBuilder mb, byte[] data)
        {
            if (runtimeVisibleTypeAnnotationsAttribute == null)
            {
                runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
            }
            mb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
        }

        internal static void SetConstantPoolAttribute(TypeBuilder tb, object[] constantPool)
        {
            if (constantPoolAttribute == null)
            {
                constantPoolAttribute = typeofConstantPoolAttribute.GetConstructor(new Type[] { Types.Object.MakeArrayType() });
            }
            tb.SetCustomAttribute(new CustomAttributeBuilder(constantPoolAttribute, new object[] { constantPool }));
        }

        internal static void SetParamArrayAttribute(ParameterBuilder pb)
        {
            if (paramArrayAttribute == null)
            {
                paramArrayAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ParamArrayAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
            }
            pb.SetCustomAttribute(paramArrayAttribute);
        }
#endif  // IMPORTER

        internal static NameSigAttribute GetNameSig(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            object[] attr = member.GetCustomAttributes(typeof(NameSigAttribute), false);
            return attr.Length == 1 ? (NameSigAttribute)attr[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofNameSigAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
            }
            return null;
#endif
        }

        internal static T[] DecodeArray<T>(CustomAttributeTypedArgument arg)
        {
            IList<CustomAttributeTypedArgument> elems = (IList<CustomAttributeTypedArgument>)arg.Value;
            T[] arr = new T[elems.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (T)elems[i].Value;
            }
            return arr;
        }

        internal static ImplementsAttribute GetImplements(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = type.GetCustomAttributes(typeof(ImplementsAttribute), false);
            return attribs.Length == 1 ? (ImplementsAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofImplementsAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new ImplementsAttribute(DecodeArray<string>(args[0]));
            }
            return null;
#endif
        }

        internal static ThrowsAttribute GetThrows(MethodBase mb)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = mb.GetCustomAttributes(typeof(ThrowsAttribute), false);
            return attribs.Length == 1 ? (ThrowsAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(mb, typeofThrowsAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                if (args[0].ArgumentType == Types.String.MakeArrayType())
                {
                    return new ThrowsAttribute(DecodeArray<string>(args[0]));
                }
                else if (args[0].ArgumentType == Types.Type.MakeArrayType())
                {
                    return new ThrowsAttribute(DecodeArray<Type>(args[0]));
                }
                else
                {
                    return new ThrowsAttribute((Type)args[0].Value);
                }
            }
            return null;
#endif
        }

        internal static string[] GetNonNestedInnerClasses(Type t)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = t.GetCustomAttributes(typeof(NonNestedInnerClassAttribute), false);
            string[] classes = new string[attribs.Length];
            for (int i = 0; i < attribs.Length; i++)
            {
                classes[i] = ((NonNestedInnerClassAttribute)attribs[i]).InnerClassName;
            }
            return classes;
#else
            List<string> list = new List<string>();
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(t, typeofNonNestedInnerClassAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                list.Add(UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value));
            }
            return list.ToArray();
#endif
        }

        internal static string GetNonNestedOuterClasses(Type t)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = t.GetCustomAttributes(typeof(NonNestedOuterClassAttribute), false);
            return attribs.Length == 1 ? ((NonNestedOuterClassAttribute)attribs[0]).OuterClassName : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(t, typeofNonNestedOuterClassAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value);
            }
            return null;
#endif
        }

        internal static SignatureAttribute GetSignature(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = member.GetCustomAttributes(typeof(SignatureAttribute), false);
            return attribs.Length == 1 ? (SignatureAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofSignatureAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new SignatureAttribute((string)args[0].Value);
            }
            return null;
#endif
        }

        internal static MethodParametersAttribute GetMethodParameters(MethodBase method)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = method.GetCustomAttributes(typeof(MethodParametersAttribute), false);
            return attribs.Length == 1 ? (MethodParametersAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(method, typeofMethodParametersAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new MethodParametersAttribute(DecodeArray<Modifiers>(args[0]));
            }
            return null;
#endif
        }

        internal static object[] GetConstantPool(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = type.GetCustomAttributes(typeof(ConstantPoolAttribute), false);
            return attribs.Length == 1 ? ((ConstantPoolAttribute)attribs[0]).constantPool : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofConstantPoolAttribute, false))
            {
                return ConstantPoolAttribute.Decompress(DecodeArray<object>(cad.ConstructorArguments[0]));
            }
            return null;
#endif
        }

        internal static byte[] GetRuntimeVisibleTypeAnnotations(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = member.GetCustomAttributes(typeof(RuntimeVisibleTypeAnnotationsAttribute), false);
            return attribs.Length == 1 ? ((RuntimeVisibleTypeAnnotationsAttribute)attribs[0]).data : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofRuntimeVisibleTypeAnnotationsAttribute, false))
            {
                return DecodeArray<byte>(cad.ConstructorArguments[0]);
            }
            return null;
#endif
        }

        internal static InnerClassAttribute GetInnerClass(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
            return attribs.Length == 1 ? (InnerClassAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofInnerClassAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
            }
            return null;
#endif
        }

        internal static RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attr = type.GetCustomAttributes(typeof(RemappedInterfaceMethodAttribute), false);
            RemappedInterfaceMethodAttribute[] attr1 = new RemappedInterfaceMethodAttribute[attr.Length];
            Array.Copy(attr, attr1, attr.Length);
            return attr1;
#else
            List<RemappedInterfaceMethodAttribute> attrs = new List<RemappedInterfaceMethodAttribute>();
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofRemappedInterfaceMethodAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value, DecodeArray<string>(args[2])));
            }
            return attrs.ToArray();
#endif
        }

        internal static RemappedTypeAttribute GetRemappedType(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
            return attribs.Length == 1 ? (RemappedTypeAttribute)attribs[0] : null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofRemappedTypeAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                return new RemappedTypeAttribute((Type)args[0].Value);
            }
            return null;
#endif
        }

        internal static RemappedClassAttribute[] GetRemappedClasses(Assembly coreAssembly)
        {
#if !IMPORTER && !EXPORTER
            object[] attr = coreAssembly.GetCustomAttributes(typeof(RemappedClassAttribute), false);
            RemappedClassAttribute[] attr1 = new RemappedClassAttribute[attr.Length];
            Array.Copy(attr, attr1, attr.Length);
            return attr1;
#else
            List<RemappedClassAttribute> attrs = new List<RemappedClassAttribute>();
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(coreAssembly, typeofRemappedClassAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                attrs.Add(new RemappedClassAttribute((string)args[0].Value, (Type)args[1].Value));
            }
            return attrs.ToArray();
#endif
        }

        internal static string GetAnnotationAttributeType(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attr = type.GetCustomAttributes(typeof(AnnotationAttributeAttribute), false);
            if (attr.Length == 1)
            {
                return ((AnnotationAttributeAttribute)attr[0]).AttributeType;
            }
            return null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofAnnotationAttributeAttribute, false))
            {
                return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);
            }
            return null;
#endif
        }

        internal static AssemblyName[] GetInternalsVisibleToAttributes(Assembly assembly)
        {
            List<AssemblyName> list = new List<AssemblyName>();
            foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(assembly))
            {
                if (cad.Constructor.DeclaringType == JVM.Import(typeof(System.Runtime.CompilerServices.InternalsVisibleToAttribute)))
                {
                    try
                    {
                        list.Add(new AssemblyName((string)cad.ConstructorArguments[0].Value));
                    }
                    catch
                    {
                        // HACK since there is no list of exception that the AssemblyName constructor can throw, we simply catch all
                    }
                }
            }
            return list.ToArray();
        }

        internal static bool IsJavaModule(Module mod)
        {
            return mod.IsDefined(typeofJavaModuleAttribute, false);
        }

        internal static object[] GetJavaModuleAttributes(Module mod)
        {
#if !IMPORTER && !EXPORTER
            return mod.GetCustomAttributes(typeofJavaModuleAttribute, false);
#else
            List<JavaModuleAttribute> attrs = new List<JavaModuleAttribute>();
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(mod, typeofJavaModuleAttribute, false))
            {
                IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
                if (args.Count == 0)
                {
                    attrs.Add(new JavaModuleAttribute());
                }
                else
                {
                    attrs.Add(new JavaModuleAttribute(DecodeArray<string>(args[0])));
                }
            }
            return attrs.ToArray();
#endif
        }

        internal static bool IsNoPackagePrefix(Type type)
        {
            return type.IsDefined(typeofNoPackagePrefixAttribute, false) || type.Assembly.IsDefined(typeofNoPackagePrefixAttribute, false);
        }

        internal static bool HasEnclosingMethodAttribute(Type type)
        {
            return type.IsDefined(typeofEnclosingMethodAttribute, false);
        }

        internal static EnclosingMethodAttribute GetEnclosingMethodAttribute(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attr = type.GetCustomAttributes(typeof(EnclosingMethodAttribute), false);
            if (attr.Length == 1)
            {
                return ((EnclosingMethodAttribute)attr[0]).SetClassName(type);
            }
            return null;
#else
            foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofEnclosingMethodAttribute, false))
            {
                return new EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(type);
            }
            return null;
#endif
        }

#if IMPORTER
        internal static void SetRemappedClass(AssemblyBuilder assemblyBuilder, string name, Type shadowType)
        {
            ConstructorInfo remappedClassAttribute = typeofRemappedClassAttribute.GetConstructor(new Type[] { Types.String, Types.Type });
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, shadowType }));
        }

        internal static void SetRemappedType(TypeBuilder typeBuilder, Type shadowType)
        {
            ConstructorInfo remappedTypeAttribute = typeofRemappedTypeAttribute.GetConstructor(new Type[] { Types.Type });
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { shadowType }));
        }

        internal static void SetRemappedInterfaceMethod(TypeBuilder typeBuilder, string name, string mappedTo, string[] throws)
        {
            CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofRemappedInterfaceMethodAttribute.GetConstructor(new Type[] { Types.String, Types.String, Types.String.MakeArrayType() }), new object[] { name, mappedTo, throws });
            typeBuilder.SetCustomAttribute(cab);
        }

        internal static void SetExceptionIsUnsafeForMapping(TypeBuilder typeBuilder)
        {
            CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofExceptionIsUnsafeForMappingAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
            typeBuilder.SetCustomAttribute(cab);
        }
#endif // IMPORTER

        internal static void SetRuntimeCompatibilityAttribute(AssemblyBuilder assemblyBuilder)
        {
            Type runtimeCompatibilityAttribute = JVM.Import(typeof(System.Runtime.CompilerServices.RuntimeCompatibilityAttribute));
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                runtimeCompatibilityAttribute.GetConstructor(Type.EmptyTypes), new object[0],
                new PropertyInfo[] { runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows") }, new object[] { true },
                new FieldInfo[0], new object[0]));
        }

        internal static void SetInternalsVisibleToAttribute(AssemblyBuilder assemblyBuilder, string assemblyName)
        {
            Type internalsVisibleToAttribute = JVM.Import(typeof(System.Runtime.CompilerServices.InternalsVisibleToAttribute));
            CustomAttributeBuilder cab = new CustomAttributeBuilder(
                internalsVisibleToAttribute.GetConstructor(new Type[] { Types.String }), new object[] { assemblyName });
            assemblyBuilder.SetCustomAttribute(cab);
        }
    }

}
