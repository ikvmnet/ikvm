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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.CoreLib.Diagnostics;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Symbols;

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

    /// <summary>
    /// Provides methods to help in generating IKVM attributes.
    /// </summary>
    class AttributeHelper
    {

        readonly RuntimeContext context;

#if IMPORTER

        CustomAttributeBuilder compilerGeneratedAttribute;
        CustomAttributeBuilder ghostInterfaceAttribute;
        CustomAttributeBuilder deprecatedAttribute;
        CustomAttributeBuilder editorBrowsableNever;
        IConstructorSymbol implementsAttribute;
        IConstructorSymbol throwsAttribute;
        IConstructorSymbol sourceFileAttribute;
        IConstructorSymbol lineNumberTableAttribute1;
        IConstructorSymbol lineNumberTableAttribute2;
        IConstructorSymbol enclosingMethodAttribute;
        IConstructorSymbol signatureAttribute;
        IConstructorSymbol methodParametersAttribute;
        IConstructorSymbol runtimeVisibleTypeAnnotationsAttribute;
        IConstructorSymbol constantPoolAttribute;
        CustomAttributeBuilder paramArrayAttribute;
        IConstructorSymbol nonNestedInnerClassAttribute;
        IConstructorSymbol nonNestedOuterClassAttribute;

        ITypeSymbol typeofModifiers;
        ITypeSymbol typeofSourceFileAttribute;
        ITypeSymbol typeofLineNumberTableAttribute;

#endif

        ITypeSymbol typeofRemappedClassAttribute;
        ITypeSymbol typeofRemappedTypeAttribute;
        ITypeSymbol typeofModifiersAttribute;
        ITypeSymbol typeofRemappedInterfaceMethodAttribute;
        ITypeSymbol typeofNameSigAttribute;
        ITypeSymbol typeofJavaModuleAttribute;
        ITypeSymbol typeofSignatureAttribute;
        ITypeSymbol typeofInnerClassAttribute;
        ITypeSymbol typeofImplementsAttribute;
        ITypeSymbol typeofGhostInterfaceAttribute;
        ITypeSymbol typeofExceptionIsUnsafeForMappingAttribute;
        ITypeSymbol typeofThrowsAttribute;
        ITypeSymbol typeofHideFromJavaAttribute;
        ITypeSymbol typeofHideFromJavaFlags;
        ITypeSymbol typeofNoPackagePrefixAttribute;
        ITypeSymbol typeofAnnotationAttributeAttribute;
        ITypeSymbol typeofNonNestedInnerClassAttribute;
        ITypeSymbol typeofNonNestedOuterClassAttribute;
        ITypeSymbol typeofEnclosingMethodAttribute;
        ITypeSymbol typeofMethodParametersAttribute;
        ITypeSymbol typeofRuntimeVisibleTypeAnnotationsAttribute;
        ITypeSymbol typeofConstantPoolAttribute;
        ITypeSymbol typeofDebuggableAttribute;
        CustomAttributeBuilder hideFromJavaAttribute;
        CustomAttributeBuilder hideFromReflection;
        IConstructorSymbol debuggableAttribute;

#if IMPORTER

        ITypeSymbol TypeOfModifiers => typeofModifiers ??= LoadType(typeof(Modifiers));

        ITypeSymbol TypeOfSourceFileAttribute => typeofSourceFileAttribute ??= LoadType(typeof(IKVM.Attributes.SourceFileAttribute));

        ITypeSymbol TypeOfLineNumberTableAttribute => typeofLineNumberTableAttribute ??= LoadType(typeof(IKVM.Attributes.LineNumberTableAttribute));

#endif

        ITypeSymbol TypeOfRemappedClassAttribute => typeofRemappedClassAttribute ??= LoadType(typeof(RemappedClassAttribute));

        ITypeSymbol TypeOfRemappedTypeAttribute => typeofRemappedTypeAttribute ??= LoadType(typeof(RemappedTypeAttribute));

        ITypeSymbol TypeOfModifiersAttribute => typeofModifiersAttribute ??= LoadType(typeof(ModifiersAttribute));

        ITypeSymbol TypeOfRemappedInterfaceMethodAttribute => typeofRemappedInterfaceMethodAttribute ??= LoadType(typeof(RemappedInterfaceMethodAttribute));

        ITypeSymbol TypeOfNameSigAttribute => typeofNameSigAttribute ??= LoadType(typeof(NameSigAttribute));

        ITypeSymbol TypeOfJavaModuleAttribute => typeofJavaModuleAttribute ??= LoadType(typeof(JavaModuleAttribute));

        ITypeSymbol TypeOfSignatureAttribute => typeofSignatureAttribute ??= LoadType(typeof(IKVM.Attributes.SignatureAttribute));

        ITypeSymbol TypeOfInnerClassAttribute => typeofInnerClassAttribute ??= LoadType(typeof(InnerClassAttribute));

        ITypeSymbol TypeOfImplementsAttribute => typeofImplementsAttribute ??= LoadType(typeof(ImplementsAttribute));

        ITypeSymbol TypeOfGhostInterfaceAttribute => typeofGhostInterfaceAttribute ??= LoadType(typeof(GhostInterfaceAttribute));

        ITypeSymbol TypeOfExceptionIsUnsafeForMappingAttribute => typeofExceptionIsUnsafeForMappingAttribute ??= LoadType(typeof(ExceptionIsUnsafeForMappingAttribute));

        ITypeSymbol TypeOfThrowsAttribute => typeofThrowsAttribute ??= LoadType(typeof(ThrowsAttribute));

        ITypeSymbol TypeOfHideFromJavaAttribute => typeofHideFromJavaAttribute ??= LoadType(typeof(HideFromJavaAttribute));

        ITypeSymbol TypeOfHideFromJavaFlags => typeofHideFromJavaFlags ??= LoadType(typeof(HideFromJavaFlags));

        ITypeSymbol TypeOfNoPackagePrefixAttribute => typeofNoPackagePrefixAttribute ??= LoadType(typeof(NoPackagePrefixAttribute));

        ITypeSymbol TypeOfAnnotationAttributeAttribute => typeofAnnotationAttributeAttribute ??= LoadType(typeof(AnnotationAttributeAttribute));

        ITypeSymbol TypeOfNonNestedInnerClassAttribute => typeofNonNestedInnerClassAttribute ??= LoadType(typeof(NonNestedInnerClassAttribute));

        ITypeSymbol TypeOfNonNestedOuterClassAttribute => typeofNonNestedOuterClassAttribute ??= LoadType(typeof(NonNestedOuterClassAttribute));

        ITypeSymbol TypeOfEnclosingMethodAttribute => typeofEnclosingMethodAttribute ??= LoadType(typeof(IKVM.Attributes.EnclosingMethodAttribute));

        ITypeSymbol TypeOfMethodParametersAttribute => typeofMethodParametersAttribute ??= LoadType(typeof(IKVM.Attributes.MethodParametersAttribute));

        ITypeSymbol TypeOfRuntimeVisibleTypeAnnotationsAttribute => typeofRuntimeVisibleTypeAnnotationsAttribute ??= LoadType(typeof(IKVM.Attributes.RuntimeVisibleTypeAnnotationsAttribute));

        ITypeSymbol TypeOfConstantPoolAttribute => typeofConstantPoolAttribute ??= LoadType(typeof(ConstantPoolAttribute));

        ITypeSymbol TypeOfDebuggableAttribute => typeofDebuggableAttribute ??= context.Resolver.ResolveCoreType(typeof(DebuggableAttribute).FullName);

        CustomAttributeBuilder HideFromJavaAttributeBuilder => hideFromJavaAttribute ??= new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor([]).AsReflection(), []);

        CustomAttributeBuilder HideFromReflectionBuilder => hideFromReflection ??= new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]).AsReflection(), [HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk]);

        /// <summary>
        /// Loads the given managed type from the runtime assembly.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        ITypeSymbol LoadType(System.Type t)
        {
            return context.Resolver.ResolveRuntimeType(t.FullName);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public AttributeHelper(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

#if IMPORTER

        object ParseValue(RuntimeClassLoader loader, RuntimeJavaType tw, string val)
        {
            if (tw == context.JavaBase.TypeOfJavaLangString)
            {
                return val;
            }
            else if (tw.IsUnloadable)
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.MapFileTypeNotFound(tw.Name));
            }
            else if (tw.TypeAsTBD.IsEnum)
            {
                return EnumHelper.Parse(context, tw.TypeAsTBD, val);
            }
            else if (tw.TypeAsTBD == context.Types.Type)
            {
                var valtw = loader.TryLoadClassByName(val);
                if (valtw != null)
                    return valtw.TypeAsBaseType;

                return context.StaticCompiler.Universe.GetType(val, true);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                return bool.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.BYTE)
            {
                return (byte)sbyte.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.CHAR)
            {
                return char.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.SHORT)
            {
                return short.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.INT)
            {
                return int.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.FLOAT)
            {
                return float.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.LONG)
            {
                return long.Parse(val);
            }
            else if (tw == context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                return double.Parse(val);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, TypeBuilder tb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            tb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, FieldBuilder fb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            fb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, ParameterBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, MethodBuilder mb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            mb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, PropertyBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, AssemblyBuilder ab, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            ab.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        void GetAttributeArgsAndTypes(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr, out ITypeSymbol[] argTypes, out object[] args)
        {
            // TODO add error handling
            var twargs = loader.ArgJavaTypeListFromSig(attr.Sig, LoadMode.Link);
            argTypes = new ITypeSymbol[twargs.Length];
            args = new object[argTypes.Length];
            for (int i = 0; i < twargs.Length; i++)
            {
                argTypes[i] = twargs[i].TypeAsSignatureType;

                var tw = twargs[i];
                if (tw == context.JavaBase.TypeOfJavaLangObject)
                    tw = loader.FieldTypeWrapperFromSig(attr.Params[i].Sig, LoadMode.Link);

                if (tw.IsArray)
                {
                    var arr = Array.CreateInstance(Type.__GetSystemType(tw.ElementTypeWrapper.TypeAsArrayType.TypeCode), attr.Params[i].Elements.Length);
                    for (int j = 0; j < arr.Length; j++)
                        arr.SetValue(ParseValue(loader, tw.ElementTypeWrapper, attr.Params[i].Elements[j].Value), j);

                    args[i] = arr;
                }
                else
                {
                    args[i] = ParseValue(loader, tw, attr.Params[i].Value);
                }
            }
        }

        CustomAttributeBuilder CreateCustomAttribute(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            // TODO add error handling
            GetAttributeArgsAndTypes(loader, attr, out var argTypes, out var args);
            if (attr.Type != null)
            {
                var t = context.Resolver.ResolveCoreType(attr.Type);
                var ci = t.GetConstructor(argTypes);
                if (ci == null)
                    throw new InvalidOperationException($"Constructor missing: {attr.Type}::<init>{attr.Sig}");

                IPropertySymbol[] namedProperties = [];
                object[] propertyValues = [];

                if (attr.Properties != null)
                {
                    namedProperties = new IPropertySymbol[attr.Properties.Length];
                    propertyValues = new object[attr.Properties.Length];
                    for (int i = 0; i < namedProperties.Length; i++)
                    {
                        namedProperties[i] = t.GetProperty(attr.Properties[i].Name);
                        propertyValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Properties[i].Sig, LoadMode.Link), attr.Properties[i].Value);
                    }
                }

                IFieldSymbol[] namedFields = [];
                object[] fieldValues = [];

                if (attr.Fields != null)
                {
                    namedFields = new IFieldSymbol[attr.Fields.Length];
                    fieldValues = new object[attr.Fields.Length];
                    for (int i = 0; i < namedFields.Length; i++)
                    {
                        namedFields[i] = t.GetField(attr.Fields[i].Name);
                        fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
                    }
                }

                return new CustomAttributeBuilder(ci.AsReflection(), args, namedProperties.AsReflection(), propertyValues, namedFields.AsReflection(), fieldValues);
            }
            else
            {
                if (attr.Properties != null)
                    throw new NotImplementedException("Setting property values on Java attributes is not implemented");

                var t = loader.LoadClassByName(attr.Class);

                IFieldSymbol[] namedFields = [];
                object[] fieldValues = [];

                if (attr.Fields != null)
                {
                    namedFields = new IFieldSymbol[attr.Fields.Length];
                    fieldValues = new object[attr.Fields.Length];
                    for (int i = 0; i < namedFields.Length; i++)
                    {
                        var fw = t.GetFieldWrapper(attr.Fields[i].Name, attr.Fields[i].Sig);
                        fw.Link();
                        namedFields[i] = fw.GetField();
                        fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
                    }
                }

                var mw = t.GetMethodWrapper("<init>", attr.Sig, false);
                if (mw == null)
                    throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));

                mw.Link();

                var ci = (mw.GetMethod() as IConstructorSymbol) ?? ((IConstructorSymbol)mw.GetMethod());
                return new CustomAttributeBuilder(ci.AsReflection(), args, namedFields.AsReflection(), fieldValues);
            }
        }

        CustomAttributeBuilder GetEditorBrowsableNever()
        {
            if (editorBrowsableNever == null)
            {
                var typeofEditorBrowsableAttribute = context.Resolver.ResolveCoreType(typeof(EditorBrowsableAttribute).FullName).AsReflection();
                var typeofEditorBrowsableState = context.Resolver.ResolveCoreType(typeof(EditorBrowsableState).FullName).AsReflection();
                var ctor = (ConstructorInfo)typeofEditorBrowsableAttribute.__CreateMissingMethod(ConstructorInfo.ConstructorName, CallingConventions.Standard | CallingConventions.HasThis, null, default, new Type[] { typeofEditorBrowsableState }, null);
                editorBrowsableNever = CustomAttributeBuilder.__FromBlob(ctor, new byte[] { 01, 00, 01, 00, 00, 00, 00, 00 });
            }

            return editorBrowsableNever;
        }

        internal void SetCompilerGenerated(TypeBuilder tb)
        {
            compilerGeneratedAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).AsReflection().GetConstructor([]), Array.Empty<object>());
            tb.SetCustomAttribute(compilerGeneratedAttribute);
        }

        internal void SetCompilerGenerated(MethodBuilder mb)
        {
            compilerGeneratedAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).AsReflection().GetConstructor([]), Array.Empty<object>());
            mb.SetCustomAttribute(compilerGeneratedAttribute);
        }

        internal void SetEditorBrowsableNever(TypeBuilder tb)
        {
            tb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetEditorBrowsableNever(MethodBuilder mb)
        {
            mb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetEditorBrowsableNever(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetDeprecatedAttribute(MethodBuilder mb)
        {
            if (deprecatedAttribute == null)
                deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), []);

            mb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(TypeBuilder tb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), []);
            }

            tb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(FieldBuilder fb)
        {
            if (deprecatedAttribute == null)
                deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), []);

            fb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(PropertyBuilder pb)
        {
            if (deprecatedAttribute == null)
            {
                deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), []);
            }
            pb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetThrowsAttribute(MethodBuilder mb, string[] exceptions)
        {
            if (exceptions != null && exceptions.Length != 0)
            {
                throwsAttribute ??= TypeOfThrowsAttribute.GetConstructor([context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType()]);
                exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
                mb.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute.AsReflection(), [exceptions]));
            }
        }

        internal void SetGhostInterface(TypeBuilder typeBuilder)
        {
            ghostInterfaceAttribute ??= new CustomAttributeBuilder(TypeOfGhostInterfaceAttribute.GetConstructor([]).AsReflection(), []);
            typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
        }

        internal void SetNonNestedInnerClass(TypeBuilder typeBuilder, string className)
        {
            nonNestedInnerClassAttribute ??= TypeOfNonNestedInnerClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedInnerClassAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

        internal void SetNonNestedOuterClass(TypeBuilder typeBuilder, string className)
        {
            nonNestedOuterClassAttribute ??= TypeOfNonNestedOuterClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedOuterClassAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

#endif // IMPORTER

        internal void HideFromReflection(MethodBuilder mb)
        {
            mb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromReflection(FieldBuilder fb)
        {
            fb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromReflection(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromJava(TypeBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

        internal void HideFromJava(MethodBuilder mb)
        {
            mb.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

        internal void HideFromJava(MethodBuilder mb, HideFromJavaFlags flags)
        {
            mb.SetCustomAttribute(new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]).AsReflection(), [flags]));
        }

        internal void HideFromJava(FieldBuilder fb)
        {
            fb.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

#if IMPORTER

        internal void HideFromJava(PropertyBuilder pb)
        {
            pb.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

#endif

        internal bool IsHideFromJava(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfHideFromJavaAttribute, false) || (type.IsNested && (type.DeclaringType.IsDefined(TypeOfHideFromJavaAttribute, false) || type.Name.StartsWith("__<", StringComparison.Ordinal)));
        }

        internal bool IsHideFromJava(IMemberSymbol mi)
        {
            return (GetHideFromJavaFlags(mi) & HideFromJavaFlags.Code) != 0;
        }

        internal HideFromJavaFlags GetHideFromJavaFlags(IMemberSymbol mi)
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

            var attr = mi.GetCustomAttributes(TypeOfHideFromJavaAttribute);
            if (attr.Length == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Length == 1)
                    return (HideFromJavaFlags)args[0].Value;

                return HideFromJavaFlags.All;
            }

            return HideFromJavaFlags.None;
        }

#if IMPORTER

        internal void SetImplementsAttribute(TypeBuilder typeBuilder, RuntimeJavaType[] ifaceWrappers)
        {
            var interfaces = new string[ifaceWrappers.Length];
            for (int i = 0; i < interfaces.Length; i++)
                interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);

            if (implementsAttribute == null)
                implementsAttribute = TypeOfImplementsAttribute.GetConstructor([context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType()]);

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute.AsReflection(), new object[] { interfaces }));
        }

#endif

        internal bool IsGhostInterface(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfGhostInterfaceAttribute, false);
        }

        internal bool IsRemappedType(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfRemappedTypeAttribute, false);
        }

        internal bool IsExceptionIsUnsafeForMapping(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfExceptionIsUnsafeForMappingAttribute, false);
        }

        internal ModifiersAttribute GetModifiersAttribute(IMemberSymbol member)
        {
            var attr = member.GetCustomAttributes(TypeOfModifiersAttribute, false);
            if (attr.Length == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Length == 2)
                    return new ModifiersAttribute((Modifiers)args[0].Value, (bool)args[1].Value);

                return new ModifiersAttribute((Modifiers)args[0].Value);
            }

            return null;
        }

        internal ExModifiers GetModifiers(IMethodBaseSymbol mb, bool assemblyIsPrivate)
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
                if ((mb.GetMethodImplementationFlags() & System.Reflection.MethodImplAttributes.Synchronized) != 0)
                {
                    modifiers |= Modifiers.Synchronized;
                }
            }

            if (mb.IsStatic)
            {
                modifiers |= Modifiers.Static;
            }

            if ((mb.Attributes & System.Reflection.MethodAttributes.PinvokeImpl) != 0)
            {
                modifiers |= Modifiers.Native;
            }

            var parameters = mb.GetParameters();
            if (parameters.Length > 0 && parameters[parameters.Length - 1].IsDefined(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName), false))
                modifiers |= Modifiers.VarArgs;

            return new ExModifiers(modifiers, false);
        }

        internal ExModifiers GetModifiers(IFieldSymbol fi, bool assemblyIsPrivate)
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

            if (Array.IndexOf(fi.GetRequiredCustomModifiers(), context.Types.IsVolatile) != -1)
            {
                modifiers |= Modifiers.Volatile;
            }

            return new ExModifiers(modifiers, false);
        }

        internal void SetDebuggingModes(AssemblyBuilder assemblyBuilder, DebuggableAttribute.DebuggingModes modes)
        {
            debuggableAttribute ??= TypeOfDebuggableAttribute.GetConstructor([TypeOfDebuggableAttribute.GetNestedType(nameof(DebuggableAttribute.DebuggingModes))]);
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(debuggableAttribute.AsReflection(), [modes]));
        }

#if IMPORTER

        internal void SetModifiers(MethodBuilder mb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]).AsReflection(), [modifiers, isInternal]);
            else
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]).AsReflection(), [modifiers]);

            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(FieldBuilder fb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]).AsReflection(), [modifiers, isInternal]);
            else
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]).AsReflection(), [modifiers]);

            fb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(PropertyBuilder pb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]).AsReflection(), [modifiers, isInternal]);
            else
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]).AsReflection(), [modifiers]);

            pb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(TypeBuilder tb, Modifiers modifiers, bool isInternal)
        {
            CustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]).AsReflection(), [modifiers, isInternal]);
            else
                customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]).AsReflection(), [modifiers]);

            tb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetNameSig(MethodBuilder mb, string name, string sig)
        {
            var customAttributeBuilder = new CustomAttributeBuilder(TypeOfNameSigAttribute.GetConstructor([context.Types.String, context.Types.String]).AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig)]);
            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetInnerClass(TypeBuilder typeBuilder, string innerClass, Modifiers modifiers)
        {
            var argTypes = new ITypeSymbol[] { context.Types.String, TypeOfModifiers };
            var args = new object[] { UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers };
            var ci = TypeOfInnerClassAttribute.GetConstructor(argTypes);
            var customAttributeBuilder = new CustomAttributeBuilder(ci.AsReflection(), args);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetSourceFile(TypeBuilder typeBuilder, string filename)
        {
            sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute.AsReflection(), [filename]));
        }

        internal void SetSourceFile(ModuleBuilder moduleBuilder, string filename)
        {
            sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor([context.Types.String]);
            moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute.AsReflection(), new object[] { filename }));
        }

        internal void SetLineNumberTable(MethodBuilder mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
        {
            object arg;
            IConstructorSymbol con;
            if (writer.Count == 1)
            {
                lineNumberTableAttribute2 ??= TypeOfLineNumberTableAttribute.GetConstructor([context.Types.UInt16]);
                con = lineNumberTableAttribute2;
                arg = (ushort)writer.LineNo;
            }
            else
            {
                lineNumberTableAttribute1 ??= TypeOfLineNumberTableAttribute.GetConstructor([context.Resolver.ResolveCoreType(typeof(byte).FullName).MakeArrayType()]);
                con = lineNumberTableAttribute1;
                arg = writer.ToArray();
            }
            mb.SetCustomAttribute(new CustomAttributeBuilder(con.AsReflection(), [arg]));
        }

        internal void SetEnclosingMethodAttribute(TypeBuilder tb, string className, string methodName, string methodSig)
        {
            if (enclosingMethodAttribute == null)
            {
                enclosingMethodAttribute = TypeOfEnclosingMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String]);
            }
            tb.SetCustomAttribute(new CustomAttributeBuilder(enclosingMethodAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig)]));
        }

        internal void SetSignatureAttribute(TypeBuilder tb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            tb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetSignatureAttribute(FieldBuilder fb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            fb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetSignatureAttribute(MethodBuilder mb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            mb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute.AsReflection(), [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetMethodParametersAttribute(MethodBuilder mb, Modifiers[] modifiers)
        {
            methodParametersAttribute ??= TypeOfMethodParametersAttribute.GetConstructor([TypeOfModifiers.MakeArrayType()]);
            mb.SetCustomAttribute(new CustomAttributeBuilder(methodParametersAttribute.AsReflection(), [modifiers]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(TypeBuilder tb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            tb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute.AsReflection(), [builder.ToArray()]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(FieldBuilder fb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            fb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute.AsReflection(), [builder.ToArray()]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(MethodBuilder mb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            mb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute.AsReflection(), [builder.ToArray()]));
        }

        internal void SetConstantPoolAttribute(TypeBuilder tb, object[] constantPool)
        {
            constantPoolAttribute ??= TypeOfConstantPoolAttribute.GetConstructor([context.Types.Object.MakeArrayType()]);
            tb.SetCustomAttribute(new CustomAttributeBuilder(constantPoolAttribute.AsReflection(), [constantPool]));
        }

        internal void SetParamArrayAttribute(ParameterBuilder pb)
        {
            paramArrayAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName).GetConstructor([]).AsReflection(), []);
            pb.SetCustomAttribute(paramArrayAttribute);
        }

#endif  // IMPORTER

        internal NameSigAttribute GetNameSig(IMemberSymbol member)
        {
            foreach (var cad in member.GetCustomAttributes(TypeOfNameSigAttribute))
            {
                var args = cad.ConstructorArguments;
                return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
            }

            return null;
        }

        internal T[] DecodeArray<T>(CoreLib.Symbols.CustomAttributeTypedArgument arg)
        {

/* Unmerged change from project 'IKVM.Tools.Exporter (net8.0)'
Before:
            var elems = (IList<CustomAttributeTypedArgument>)arg.Value;
            var arr = new T[elems.Count];
After:
            var elems = (IList<Reflection.CustomAttributeTypedArgument>)arg.Value;
            var arr = new T[elems.Count];
*/
            var elems = (IList<System.Reflection.CustomAttributeTypedArgument>)arg.Value;
            var arr = new T[elems.Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (T)elems[i].Value;

            return arr;
        }

        internal ImplementsAttribute GetImplements(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfImplementsAttribute))
            {
                var args = cad.ConstructorArguments;
                return new ImplementsAttribute(DecodeArray<string>(args[0]));
            }

            return null;
        }

        internal ThrowsAttribute GetThrows(IMethodBaseSymbol mb)
        {
            foreach (var cad in mb.GetCustomAttributes(TypeOfThrowsAttribute))
            {
                var args = cad.ConstructorArguments;
                if (args[0].ArgumentType == context.Types.String.MakeArrayType())
                {
                    return new ThrowsAttribute(DecodeArray<string>(args[0]));
                }
                else if (args[0].ArgumentType == context.Types.Type.MakeArrayType())
                {
                    return new ThrowsAttribute(DecodeArray<Type>(args[0]));
                }
                else
                {
                    return new ThrowsAttribute((Type)args[0].Value);
                }
            }

            return null;
        }

        internal string[] GetNonNestedInnerClasses(ITypeSymbol t)
        {
            var list = new List<string>();
            foreach (var cad in t.GetCustomAttributes(TypeOfNonNestedInnerClassAttribute))
                list.Add(UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value));

            return list.ToArray();
        }

        internal string GetNonNestedOuterClasses(ITypeSymbol t)
        {
            foreach (var cad in t.GetCustomAttributes(TypeOfNonNestedOuterClassAttribute))
                return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal IKVM.Attributes.SignatureAttribute GetSignature(IMemberSymbol member)
        {
            foreach (var cad in member.GetCustomAttributes(TypeOfSignatureAttribute))
                return new IKVM.Attributes.SignatureAttribute((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal IKVM.Attributes.MethodParametersAttribute GetMethodParameters(IMethodBaseSymbol method)
        {
            foreach (var cad in method.GetCustomAttributes(TypeOfMethodParametersAttribute))
                return new IKVM.Attributes.MethodParametersAttribute(DecodeArray<Modifiers>(cad.ConstructorArguments[0]));

            return null;
        }

        internal object[] GetConstantPool(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfConstantPoolAttribute))
                return ConstantPoolAttribute.Decompress(DecodeArray<object>(cad.ConstructorArguments[0]));

            return null;
        }

        internal byte[] GetRuntimeVisibleTypeAnnotations(IMemberSymbol member)
        {
            foreach (var cad in member.GetCustomAttributes(TypeOfRuntimeVisibleTypeAnnotationsAttribute))
                return DecodeArray<byte>(cad.ConstructorArguments[0]);

            return null;
        }

        internal InnerClassAttribute GetInnerClass(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfInnerClassAttribute))
            {
                var args = cad.ConstructorArguments;
                return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
            }

            return null;
        }

        internal RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(ITypeSymbol type)
        {
            var attrs = new List<RemappedInterfaceMethodAttribute>();
            foreach (var cad in type.GetCustomAttributes(TypeOfRemappedInterfaceMethodAttribute))
            {
                var args = cad.ConstructorArguments;
                attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value, DecodeArray<string>(args[2])));
            }

            return attrs.ToArray();
        }

        internal RemappedTypeAttribute GetRemappedType(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfRemappedTypeAttribute))
                return new RemappedTypeAttribute((Type)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal RemappedClassAttribute[] GetRemappedClasses(IAssemblySymbol coreAssembly)
        {
            var attrs = new List<RemappedClassAttribute>();

            foreach (var cad in coreAssembly.GetCustomAttributes(TypeOfRemappedClassAttribute))
            {
                var args = cad.ConstructorArguments;
                attrs.Add(new RemappedClassAttribute((string)args[0].Value, (Type)args[1].Value));
            }

            return attrs.ToArray();
        }

        internal string GetAnnotationAttributeType(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfAnnotationAttributeAttribute))
                return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal System.Reflection.AssemblyName[] GetInternalsVisibleToAttributes(IAssemblySymbol assembly)
        {
            var list = new List<System.Reflection.AssemblyName>();

            foreach (var cad in assembly.GetCustomAttributes())
            {
                if (cad.Constructor.DeclaringType == context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName))
                {
                    try
                    {
                        list.Add(new System.Reflection.AssemblyName((string)cad.ConstructorArguments[0].Value));
                    }
                    catch
                    {
                        // HACK since there is no list of exception that the AssemblyName constructor can throw, we simply catch all
                    }
                }
            }

            return list.ToArray();
        }

        internal bool IsJavaModule(IModuleSymbol mod)
        {
            return mod.IsDefined(TypeOfJavaModuleAttribute, false);
        }

        internal object[] GetJavaModuleAttributes(IModuleSymbol mod)
        {
            var attrs = new List<JavaModuleAttribute>();

            foreach (var cad in mod.GetCustomAttributes(TypeOfJavaModuleAttribute))
            {
                var args = cad.ConstructorArguments;
                if (args.Length == 0)
                    attrs.Add(new JavaModuleAttribute());
                else
                    attrs.Add(new JavaModuleAttribute(DecodeArray<string>(args[0])));
            }

            return attrs.ToArray();
        }

        internal bool IsNoPackagePrefix(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfNoPackagePrefixAttribute, false) || type.Assembly.IsDefined(TypeOfNoPackagePrefixAttribute, false);
        }

        internal bool HasEnclosingMethodAttribute(ITypeSymbol type)
        {
            return type.IsDefined(TypeOfEnclosingMethodAttribute, false);
        }

        internal IKVM.Attributes.EnclosingMethodAttribute GetEnclosingMethodAttribute(ITypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfEnclosingMethodAttribute))
                return new IKVM.Attributes.EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(context, type.AsReflection());

            return null;
        }

#if IMPORTER

        internal void SetRemappedClass(AssemblyBuilder assemblyBuilder, string name, ITypeSymbol shadowType)
        {
            var remappedClassAttribute = TypeOfRemappedClassAttribute.GetConstructor([context.Types.String, context.Types.Type]);
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute.AsReflection(), [name, shadowType]));
        }

        internal void SetRemappedType(TypeBuilder typeBuilder, ITypeSymbol shadowType)
        {
            var remappedTypeAttribute = TypeOfRemappedTypeAttribute.GetConstructor([context.Types.Type]);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute.AsReflection(), [shadowType]));
        }

        internal void SetRemappedInterfaceMethod(TypeBuilder typeBuilder, string name, string mappedTo, string[] throws)
        {
            var cab = new CustomAttributeBuilder(TypeOfRemappedInterfaceMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String.MakeArrayType()]).AsReflection(), [name, mappedTo, throws]);
            typeBuilder.SetCustomAttribute(cab);
        }

        internal void SetExceptionIsUnsafeForMapping(TypeBuilder typeBuilder)
        {
            var cab = new CustomAttributeBuilder(TypeOfExceptionIsUnsafeForMappingAttribute.GetConstructor([]).AsReflection(), Array.Empty<object>());
            typeBuilder.SetCustomAttribute(cab);
        }

#endif

        internal void SetRuntimeCompatibilityAttribute(AssemblyBuilder assemblyBuilder)
        {
            var runtimeCompatibilityAttribute = context.Resolver.ResolveCoreType(typeof(RuntimeCompatibilityAttribute).FullName);
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(runtimeCompatibilityAttribute.GetConstructor([]).AsReflection(), [], [runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows").AsReflection()], [true], [], []));
        }

        internal void SetInternalsVisibleToAttribute(AssemblyBuilder assemblyBuilder, string assemblyName)
        {
            var internalsVisibleToAttribute = context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName);
            var cab = new CustomAttributeBuilder(internalsVisibleToAttribute.GetConstructor([context.Types.String]).AsReflection(), [assemblyName]);
            assemblyBuilder.SetCustomAttribute(cab);
        }

    }

}
