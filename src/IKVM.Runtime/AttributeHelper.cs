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
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection;


#if IMPORTER || EXPORTER
#else
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

        ICustomAttributeBuilder compilerGeneratedAttribute;
        ICustomAttributeBuilder ghostInterfaceAttribute;
        ICustomAttributeBuilder deprecatedAttribute;
        ICustomAttributeBuilder editorBrowsableNever;
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
        ICustomAttributeBuilder paramArrayAttribute;
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
        ICustomAttributeBuilder hideFromJavaAttribute;
        ICustomAttributeBuilder hideFromReflection;
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

        ICustomAttributeBuilder HideFromJavaAttributeBuilder => hideFromJavaAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(TypeOfHideFromJavaAttribute.GetConstructor([]), []);

        ICustomAttributeBuilder HideFromReflectionBuilder => hideFromReflection ??= context.Resolver.Symbols.CreateCustomAttribute(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]), [HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk]);

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

        internal void SetCustomAttribute(RuntimeClassLoader loader, ITypeSymbolBuilder tb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            tb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, IFieldSymbolBuilder fb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            fb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, IParameterSymbolBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, IMethodSymbolBuilder mb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            mb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, IPropertySymbolBuilder pb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        internal void SetCustomAttribute(RuntimeClassLoader loader, IAssemblySymbolBuilder ab, IKVM.Tools.Importer.MapXml.Attribute attr)
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

        ICustomAttributeBuilder CreateCustomAttribute(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr)
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

                return context.Resolver.Symbols.CreateCustomAttribute(ci, args, namedProperties, propertyValues, namedFields, fieldValues);
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
                return context.Resolver.Symbols.CreateCustomAttribute(ci, args, namedFields, fieldValues);
            }
        }

        ICustomAttributeBuilder GetEditorBrowsableNever()
        {
            if (editorBrowsableNever == null)
            {
                var typeofEditorBrowsableAttribute = context.Resolver.ResolveCoreType(typeof(EditorBrowsableAttribute).FullName);
                var typeofEditorBrowsableState = context.Resolver.ResolveCoreType(typeof(EditorBrowsableState).FullName);
                var ctor = typeofEditorBrowsableAttribute.GetConstructor([typeofEditorBrowsableState]);
                editorBrowsableNever = context.Resolver.Symbols.CreateCustomAttribute(ctor, [EditorBrowsableState.Never]);
            }

            return editorBrowsableNever;
        }

        internal void SetCompilerGenerated(ITypeSymbolBuilder tb)
        {
            compilerGeneratedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).GetConstructor([]), []);
            tb.SetCustomAttribute(compilerGeneratedAttribute);
        }

        internal void SetCompilerGenerated(IMethodSymbolBuilder mb)
        {
            compilerGeneratedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).GetConstructor([]), []);
            mb.SetCustomAttribute(compilerGeneratedAttribute);
        }

        internal void SetEditorBrowsableNever(ITypeSymbolBuilder tb)
        {
            tb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetEditorBrowsableNever(IMethodSymbolBuilder mb)
        {
            mb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetEditorBrowsableNever(IPropertySymbolBuilder pb)
        {
            pb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetDeprecatedAttribute(IMethodSymbolBuilder mb)
        {
            deprecatedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]), []);
            mb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(ITypeSymbolBuilder tb)
        {
            deprecatedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]), []);
            tb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(IFieldSymbolBuilder fb)
        {
            deprecatedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]), []);
            fb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetDeprecatedAttribute(IPropertySymbolBuilder pb)
        {
            deprecatedAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]), []);
            pb.SetCustomAttribute(deprecatedAttribute);
        }

        internal void SetThrowsAttribute(IMethodSymbolBuilder mb, string[] exceptions)
        {
            if (exceptions != null && exceptions.Length != 0)
            {
                throwsAttribute ??= TypeOfThrowsAttribute.GetConstructor([context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType()]);
                exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
                mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(throwsAttribute, [exceptions]));
            }
        }

        internal void SetGhostInterface(ITypeSymbolBuilder typeBuilder)
        {
            ghostInterfaceAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(TypeOfGhostInterfaceAttribute.GetConstructor([]), []);
            typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
        }

        internal void SetNonNestedInnerClass(ITypeSymbolBuilder typeBuilder, string className)
        {
            nonNestedInnerClassAttribute ??= TypeOfNonNestedInnerClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(nonNestedInnerClassAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

        internal void SetNonNestedOuterClass(ITypeSymbolBuilder typeBuilder, string className)
        {
            nonNestedOuterClassAttribute ??= TypeOfNonNestedOuterClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(nonNestedOuterClassAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

#endif // IMPORTER

        internal void HideFromReflection(IMethodSymbolBuilder mb)
        {
            mb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromReflection(IFieldSymbolBuilder fb)
        {
            fb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromReflection(IPropertySymbolBuilder pb)
        {
            pb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromJava(ITypeSymbolBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

        internal void HideFromJava(IMethodSymbolBuilder mb)
        {
            mb.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

        internal void HideFromJava(IMethodSymbolBuilder mb, HideFromJavaFlags flags)
        {
            mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]), [flags]));
        }

        internal void HideFromJava(IFieldSymbolBuilder fb)
        {
            fb.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

#if IMPORTER

        internal void HideFromJava(IPropertySymbolBuilder pb)
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
            var fi = mi as IFieldSymbol;
            if (fi != null && (fi.Attributes & System.Reflection.FieldAttributes.FieldAccessMask) == System.Reflection.FieldAttributes.PrivateScope)
                return HideFromJavaFlags.All;

            var mb = mi as IMethodBaseSymbol;
            if (mb != null && (mb.Attributes & System.Reflection.MethodAttributes.MemberAccessMask) == System.Reflection.MethodAttributes.PrivateScope)
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

        internal void SetImplementsAttribute(ITypeSymbolBuilder typeBuilder, RuntimeJavaType[] ifaceWrappers)
        {
            var interfaces = new string[ifaceWrappers.Length];
            for (int i = 0; i < interfaces.Length; i++)
                interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);

            if (implementsAttribute == null)
                implementsAttribute = TypeOfImplementsAttribute.GetConstructor([context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType()]);

            typeBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(implementsAttribute, [interfaces]));
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

        internal void SetDebuggingModes(IAssemblySymbolBuilder assemblyBuilder, DebuggableAttribute.DebuggingModes modes)
        {
            debuggableAttribute ??= TypeOfDebuggableAttribute.GetConstructor([TypeOfDebuggableAttribute.GetNestedType(nameof(DebuggableAttribute.DebuggingModes))]);
            assemblyBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(debuggableAttribute, [modes]));
        }

#if IMPORTER

        internal void SetModifiers(IMethodSymbolBuilder mb, Modifiers modifiers, bool isInternal)
        {
            ICustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]), [modifiers, isInternal]);
            else
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]), [modifiers]);

            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(IFieldSymbolBuilder fb, Modifiers modifiers, bool isInternal)
        {
            ICustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]), [modifiers, isInternal]);
            else
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]), [modifiers]);

            fb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(IPropertySymbolBuilder pb, Modifiers modifiers, bool isInternal)
        {
            ICustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]), [modifiers, isInternal]);
            else
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]), [modifiers]);

            pb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetModifiers(ITypeSymbolBuilder tb, Modifiers modifiers, bool isInternal)
        {
            ICustomAttributeBuilder customAttributeBuilder;
            if (isInternal)
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]), [modifiers, isInternal]);
            else
                customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]), [modifiers]);

            tb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetNameSig(IMethodSymbolBuilder mb, string name, string sig)
        {
            var customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(TypeOfNameSigAttribute.GetConstructor([context.Types.String, context.Types.String]), [UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig)]);
            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetInnerClass(ITypeSymbolBuilder typeBuilder, string innerClass, Modifiers modifiers)
        {
            var argTypes = new ITypeSymbol[] { context.Types.String, TypeOfModifiers };
            var args = new object[] { UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers };
            var ci = TypeOfInnerClassAttribute.GetConstructor(argTypes);
            var customAttributeBuilder = context.Resolver.Symbols.CreateCustomAttribute(ci, args);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetSourceFile(ITypeSymbolBuilder typeBuilder, string filename)
        {
            sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(sourceFileAttribute, [filename]));
        }

        internal void SetSourceFile(IMethodSymbolBuilder moduleBuilder, string filename)
        {
            sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor([context.Types.String]);
            moduleBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(sourceFileAttribute, [filename]));
        }

        internal void SetLineNumberTable(IMethodSymbolBuilder mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
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

            mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(con, [arg]));
        }

        internal void SetEnclosingMethodAttribute(ITypeSymbolBuilder tb, string className, string methodName, string methodSig)
        {
            enclosingMethodAttribute ??= TypeOfEnclosingMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String]);
            tb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(enclosingMethodAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig)]));
        }

        internal void SetSignatureAttribute(ITypeSymbolBuilder tb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            tb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(signatureAttribute, [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetSignatureAttribute(IFieldSymbolBuilder fb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            fb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(signatureAttribute, [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetSignatureAttribute(IMethodSymbolBuilder mb, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(signatureAttribute, [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetMethodParametersAttribute(IMethodSymbolBuilder mb, Modifiers[] modifiers)
        {
            methodParametersAttribute ??= TypeOfMethodParametersAttribute.GetConstructor([TypeOfModifiers.MakeArrayType()]);
            mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(methodParametersAttribute, [modifiers]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(ITypeSymbolBuilder tb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            tb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(IFieldSymbolBuilder fb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            fb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(IMethodSymbolBuilder mb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            mb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
        }

        internal void SetConstantPoolAttribute(ITypeSymbolBuilder tb, object[] constantPool)
        {
            constantPoolAttribute ??= TypeOfConstantPoolAttribute.GetConstructor([context.Types.Object.MakeArrayType()]);
            tb.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(constantPoolAttribute, [constantPool]));
        }

        internal void SetParamArrayAttribute(IParameterSymbolBuilder pb)
        {
            paramArrayAttribute ??= context.Resolver.Symbols.CreateCustomAttribute(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName).GetConstructor([]), []);
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

        internal T[] DecodeArray<T>(IKVM.CoreLib.Symbols.CustomAttributeTypedArgument arg)
        {
            var elems = (CustomAttributeTypedArgument[])arg.Value;
            var arr = new T[elems.Length];
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
                    return new ThrowsAttribute(DecodeArray<ITypeSymbol>(args[0]).Unpack());
                }
                else
                {
                    return new ThrowsAttribute(((ITypeSymbol)args[0].Value).AsReflection());
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
                return new IKVM.Attributes.EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(context, type);

            return null;
        }

#if IMPORTER

        internal void SetRemappedClass(IAssemblySymbolBuilder assemblyBuilder, string name, ITypeSymbol shadowType)
        {
            var remappedClassAttribute = TypeOfRemappedClassAttribute.GetConstructor([context.Types.String, context.Types.Type]);
            assemblyBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(remappedClassAttribute, [name, shadowType]));
        }

        internal void SetRemappedType(ITypeSymbolBuilder typeBuilder, ITypeSymbol shadowType)
        {
            var remappedTypeAttribute = TypeOfRemappedTypeAttribute.GetConstructor([context.Types.Type]);
            typeBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(remappedTypeAttribute, [shadowType]));
        }

        internal void SetRemappedInterfaceMethod(ITypeSymbolBuilder typeBuilder, string name, string mappedTo, string[] throws)
        {
            var cab = context.Resolver.Symbols.CreateCustomAttribute(TypeOfRemappedInterfaceMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String.MakeArrayType()]), [name, mappedTo, throws]);
            typeBuilder.SetCustomAttribute(cab);
        }

        internal void SetExceptionIsUnsafeForMapping(ITypeSymbolBuilder typeBuilder)
        {
            var cab = context.Resolver.Symbols.CreateCustomAttribute(TypeOfExceptionIsUnsafeForMappingAttribute.GetConstructor([]), []);
            typeBuilder.SetCustomAttribute(cab);
        }

#endif

        internal void SetRuntimeCompatibilityAttribute(IAssemblySymbolBuilder assemblyBuilder)
        {
            var runtimeCompatibilityAttribute = context.Resolver.ResolveCoreType(typeof(RuntimeCompatibilityAttribute).FullName);
            assemblyBuilder.SetCustomAttribute(context.Resolver.Symbols.CreateCustomAttribute(runtimeCompatibilityAttribute.GetConstructor([]), [], [runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows")], [true], [], []));
        }

        internal void SetInternalsVisibleToAttribute(IAssemblySymbolBuilder assemblyBuilder, string assemblyName)
        {
            var internalsVisibleToAttribute = context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName);
            var cab = context.Resolver.Symbols.CreateCustomAttribute(internalsVisibleToAttribute.GetConstructor([context.Types.String]), [assemblyName]);
            assemblyBuilder.SetCustomAttribute(cab);
        }

    }

}
