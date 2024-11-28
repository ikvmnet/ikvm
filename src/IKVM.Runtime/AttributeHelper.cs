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
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides methods to help in generating IKVM attributes.
    /// </summary>
    class AttributeHelper
    {

        readonly RuntimeContext context;

        CustomAttribute? compilerGeneratedAttribute;
        CustomAttribute? ghostInterfaceAttribute;
        CustomAttribute? deprecatedAttribute;
        CustomAttribute? editorBrowsableNever;
        MethodSymbol implementsAttribute;
        MethodSymbol throwsAttribute;
        MethodSymbol sourceFileAttribute;
        MethodSymbol lineNumberTableAttribute1;
        MethodSymbol lineNumberTableAttribute2;
        MethodSymbol enclosingMethodAttribute;
        MethodSymbol signatureAttribute;
        MethodSymbol methodParametersAttribute;
        MethodSymbol runtimeVisibleTypeAnnotationsAttribute;
        MethodSymbol constantPoolAttribute;
        CustomAttribute? paramArrayAttribute;
        MethodSymbol nonNestedInnerClassAttribute;
        MethodSymbol nonNestedOuterClassAttribute;

        TypeSymbol typeofModifiers;
        TypeSymbol typeofSourceFileAttribute;
        TypeSymbol typeofLineNumberTableAttribute;

        TypeSymbol typeofRemappedClassAttribute;
        TypeSymbol typeofRemappedTypeAttribute;
        TypeSymbol typeofModifiersAttribute;
        TypeSymbol typeofRemappedInterfaceMethodAttribute;
        TypeSymbol typeofNameSigAttribute;
        TypeSymbol typeofJavaModuleAttribute;
        TypeSymbol typeofSignatureAttribute;
        TypeSymbol typeofInnerClassAttribute;
        TypeSymbol typeofImplementsAttribute;
        TypeSymbol typeofGhostInterfaceAttribute;
        TypeSymbol typeofExceptionIsUnsafeForMappingAttribute;
        TypeSymbol typeofThrowsAttribute;
        TypeSymbol typeofHideFromJavaAttribute;
        TypeSymbol typeofHideFromJavaFlags;
        TypeSymbol typeofNoPackagePrefixAttribute;
        TypeSymbol typeofAnnotationAttributeAttribute;
        TypeSymbol typeofNonNestedInnerClassAttribute;
        TypeSymbol typeofNonNestedOuterClassAttribute;
        TypeSymbol typeofEnclosingMethodAttribute;
        TypeSymbol typeofMethodParametersAttribute;
        TypeSymbol typeofRuntimeVisibleTypeAnnotationsAttribute;
        TypeSymbol typeofConstantPoolAttribute;
        TypeSymbol typeofDebuggableAttribute;
        TypeSymbol typeofCustomAssemblyClassLoaderAttribute;
        CustomAttribute? hideFromJavaAttribute;
        CustomAttribute? hideFromReflection;
        MethodSymbol debuggableAttribute;

        TypeSymbol TypeOfModifiers => typeofModifiers ??= context.Resolver.ResolveRuntimeType(typeof(Modifiers).FullName);

        TypeSymbol TypeOfSourceFileAttribute => typeofSourceFileAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.SourceFileAttribute).FullName);

        TypeSymbol TypeOfLineNumberTableAttribute => typeofLineNumberTableAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.LineNumberTableAttribute).FullName);

        TypeSymbol TypeOfRemappedClassAttribute => typeofRemappedClassAttribute ??= context.Resolver.ResolveRuntimeType(typeof(RemappedClassAttribute).FullName);

        TypeSymbol TypeOfRemappedTypeAttribute => typeofRemappedTypeAttribute ??= context.Resolver.ResolveRuntimeType(typeof(RemappedTypeAttribute).FullName);

        TypeSymbol TypeOfModifiersAttribute => typeofModifiersAttribute ??= context.Resolver.ResolveRuntimeType(typeof(ModifiersAttribute).FullName);

        TypeSymbol TypeOfRemappedInterfaceMethodAttribute => typeofRemappedInterfaceMethodAttribute ??= context.Resolver.ResolveRuntimeType(typeof(RemappedInterfaceMethodAttribute).FullName);

        TypeSymbol TypeOfNameSigAttribute => typeofNameSigAttribute ??= context.Resolver.ResolveRuntimeType(typeof(NameSigAttribute).FullName);

        TypeSymbol TypeOfJavaModuleAttribute => typeofJavaModuleAttribute ??= context.Resolver.ResolveRuntimeType(typeof(JavaModuleAttribute).FullName);

        TypeSymbol TypeOfSignatureAttribute => typeofSignatureAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.SignatureAttribute).FullName);

        TypeSymbol TypeOfInnerClassAttribute => typeofInnerClassAttribute ??= context.Resolver.ResolveRuntimeType(typeof(InnerClassAttribute).FullName);

        TypeSymbol TypeOfImplementsAttribute => typeofImplementsAttribute ??= context.Resolver.ResolveRuntimeType(typeof(ImplementsAttribute).FullName);

        TypeSymbol TypeOfGhostInterfaceAttribute => typeofGhostInterfaceAttribute ??= context.Resolver.ResolveRuntimeType(typeof(GhostInterfaceAttribute).FullName);

        TypeSymbol TypeOfExceptionIsUnsafeForMappingAttribute => typeofExceptionIsUnsafeForMappingAttribute ??= context.Resolver.ResolveRuntimeType(typeof(ExceptionIsUnsafeForMappingAttribute).FullName);

        TypeSymbol TypeOfThrowsAttribute => typeofThrowsAttribute ??= context.Resolver.ResolveRuntimeType(typeof(ThrowsAttribute).FullName);

        TypeSymbol TypeOfHideFromJavaAttribute => typeofHideFromJavaAttribute ??= context.Resolver.ResolveRuntimeType(typeof(HideFromJavaAttribute).FullName);

        TypeSymbol TypeOfHideFromJavaFlags => typeofHideFromJavaFlags ??= context.Resolver.ResolveRuntimeType(typeof(HideFromJavaFlags).FullName);

        TypeSymbol TypeOfNoPackagePrefixAttribute => typeofNoPackagePrefixAttribute ??= context.Resolver.ResolveRuntimeType(typeof(NoPackagePrefixAttribute).FullName);

        TypeSymbol TypeOfAnnotationAttributeAttribute => typeofAnnotationAttributeAttribute ??= context.Resolver.ResolveRuntimeType(typeof(AnnotationAttributeAttribute).FullName);

        TypeSymbol TypeOfNonNestedInnerClassAttribute => typeofNonNestedInnerClassAttribute ??= context.Resolver.ResolveRuntimeType(typeof(NonNestedInnerClassAttribute).FullName);

        TypeSymbol TypeOfNonNestedOuterClassAttribute => typeofNonNestedOuterClassAttribute ??= context.Resolver.ResolveRuntimeType(typeof(NonNestedOuterClassAttribute).FullName);

        TypeSymbol TypeOfEnclosingMethodAttribute => typeofEnclosingMethodAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.EnclosingMethodAttribute).FullName);

        TypeSymbol TypeOfMethodParametersAttribute => typeofMethodParametersAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.MethodParametersAttribute).FullName);

        TypeSymbol TypeOfRuntimeVisibleTypeAnnotationsAttribute => typeofRuntimeVisibleTypeAnnotationsAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.RuntimeVisibleTypeAnnotationsAttribute).FullName);

        TypeSymbol TypeOfConstantPoolAttribute => typeofConstantPoolAttribute ??= context.Resolver.ResolveRuntimeType(typeof(ConstantPoolAttribute).FullName);

        TypeSymbol TypeOfDebuggableAttribute => typeofDebuggableAttribute ??= context.Resolver.ResolveSystemType(typeof(DebuggableAttribute).FullName);

        TypeSymbol TypeOfCustomAssemblyClassLoaderAttribute => typeofCustomAssemblyClassLoaderAttribute ??= context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.CustomAssemblyClassLoaderAttribute).FullName);

        CustomAttribute HideFromJavaAttributeBuilder => hideFromJavaAttribute ??= CustomAttribute.Create(TypeOfHideFromJavaAttribute.GetConstructor([]), []);

        CustomAttribute HideFromReflectionBuilder => hideFromReflection ??= CustomAttribute.Create(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]), [HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk]);

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
                throw new DiagnosticEventException(DiagnosticEvent.MapFileTypeNotFound(tw.Name));
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

        internal void SetCustomAttribute(RuntimeClassLoader loader, ICustomAttributeBuilder tb, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            tb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
        }

        void GetAttributeArgsAndTypes(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr, out ImmutableArray<TypeSymbol> argTypes, out ImmutableArray<object?> args)
        {
            // TODO add error handling
            var twargs = loader.ArgJavaTypeListFromSig(attr.Sig, LoadMode.Link);
            var argTypesBuilder = ImmutableArray.CreateBuilder<TypeSymbol>(twargs.Length);
            argTypesBuilder.Count = twargs.Length;
            var argsBuilder = ImmutableArray.CreateBuilder<object?>(twargs.Length);
            argsBuilder.Count = twargs.Length;
            for (int i = 0; i < twargs.Length; i++)
            {
                argTypesBuilder[i] = twargs[i].TypeAsSignatureType;

                var tw = twargs[i];
                if (tw == context.JavaBase.TypeOfJavaLangObject)
                    tw = loader.FieldTypeWrapperFromSig(attr.Params[i].Sig, LoadMode.Link);

                if (tw.IsArray)
                {
                    var arr = Array.CreateInstance(tw.ElementTypeWrapper.TypeAsArrayType.GetSystemType(), attr.Params[i].Elements.Length);
                    for (int j = 0; j < arr.Length; j++)
                        arr.SetValue(ParseValue(loader, tw.ElementTypeWrapper, attr.Params[i].Elements[j].Value), j);

                    argsBuilder[i] = arr;
                }
                else
                {
                    argsBuilder[i] = ParseValue(loader, tw, attr.Params[i].Value);
                }
            }

            argTypes = argTypesBuilder.DrainToImmutable();
            args = argsBuilder.DrainToImmutable();
        }

        CustomAttribute CreateCustomAttribute(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr)
        {
            // TODO add error handling
            GetAttributeArgsAndTypes(loader, attr, out var argTypes, out var args);
            if (attr.Type != null)
            {
                var t = context.Resolver.ResolveType(attr.Type);
                var ci = t.GetConstructor(argTypes);
                if (ci == null)
                    throw new InvalidOperationException($"Constructor missing: {attr.Type}::<init>{attr.Sig}");

                var namedProperties = ImmutableArray.CreateBuilder<PropertySymbol>();
                var propertyValues = ImmutableArray.CreateBuilder<object?>();

                if (attr.Properties != null)
                {
                    namedProperties = ImmutableArray.CreateBuilder<PropertySymbol>(attr.Properties.Length);
                    propertyValues = ImmutableArray.CreateBuilder<object>(attr.Properties.Length);
                    for (int i = 0; i < namedProperties.Count; i++)
                    {
                        namedProperties[i] = t.GetProperty(attr.Properties[i].Name);
                        propertyValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Properties[i].Sig, LoadMode.Link), attr.Properties[i].Value);
                    }
                }

                var namedFields = ImmutableArray.CreateBuilder<FieldSymbol>();
                var fieldValues = ImmutableArray.CreateBuilder<object?>();

                if (attr.Fields != null)
                {
                    namedFields = ImmutableArray.CreateBuilder<FieldSymbol>(attr.Fields.Length);
                    fieldValues = ImmutableArray.CreateBuilder<object?>(attr.Fields.Length);
                    for (int i = 0; i < namedFields.Count; i++)
                    {
                        namedFields[i] = t.GetField(attr.Fields[i].Name);
                        fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
                    }
                }

                return CustomAttribute.Create(ci, args, namedProperties.DrainToImmutable(), propertyValues.DrainToImmutable(), namedFields.DrainToImmutable(), fieldValues.DrainToImmutable());
            }
            else
            {
                if (attr.Properties != null)
                    throw new NotImplementedException("Setting property values on Java attributes is not implemented");

                var t = loader.LoadClassByName(attr.Class);

                var namedFields = ImmutableArray.CreateBuilder<FieldSymbol>();
                var fieldValues = ImmutableArray.CreateBuilder<object?>();

                if (attr.Fields != null)
                {
                    namedFields = ImmutableArray.CreateBuilder<FieldSymbol>(attr.Fields.Length);
                    fieldValues = ImmutableArray.CreateBuilder<object?>(attr.Fields.Length);
                    for (int i = 0; i < namedFields.Count; i++)
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

                return CustomAttribute.Create(mw.GetMethod(), args, namedFields.DrainToImmutable(), fieldValues.DrainToImmutable());
            }
        }

        CustomAttribute GetEditorBrowsableNever()
        {
            if (editorBrowsableNever == null)
            {
                var typeofEditorBrowsableAttribute = context.Resolver.ResolveSystemType(typeof(EditorBrowsableAttribute).FullName);
                var typeofEditorBrowsableState = context.Resolver.ResolveSystemType(typeof(EditorBrowsableState).FullName);
                var ctor = typeofEditorBrowsableAttribute.GetConstructor([typeofEditorBrowsableState]);
                editorBrowsableNever = CustomAttribute.Create(ctor, [EditorBrowsableState.Never]);
            }

            return editorBrowsableNever.Value;
        }

        internal void SetCompilerGenerated(ICustomAttributeBuilder tb)
        {
            tb.SetCustomAttribute(compilerGeneratedAttribute ??= CustomAttribute.Create(context.Resolver.ResolveSystemType(typeof(CompilerGeneratedAttribute).FullName).GetConstructor([]), []));
        }

        internal void SetEditorBrowsableNever(ICustomAttributeBuilder mb)
        {
            mb.SetCustomAttribute(GetEditorBrowsableNever());
        }

        internal void SetDeprecatedAttribute(ICustomAttributeBuilder tb)
        {
            tb.SetCustomAttribute(deprecatedAttribute ??= CustomAttribute.Create(context.Resolver.ResolveSystemType(typeof(ObsoleteAttribute).FullName).GetConstructor([]), []));
        }

        internal void SetThrowsAttribute(ICustomAttributeBuilder mb, string[] exceptions)
        {
            if (exceptions != null && exceptions.Length != 0)
            {
                throwsAttribute ??= TypeOfThrowsAttribute.GetConstructor([context.Types.String.MakeArrayType()]);
                exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
                mb.SetCustomAttribute(CustomAttribute.Create(throwsAttribute, [exceptions]));
            }
        }

        internal void SetGhostInterface(TypeSymbolBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(ghostInterfaceAttribute ??= CustomAttribute.Create(TypeOfGhostInterfaceAttribute.GetConstructor([]), []));
        }

        internal void SetNonNestedInnerClass(TypeSymbolBuilder typeBuilder, string className)
        {
            nonNestedInnerClassAttribute ??= TypeOfNonNestedInnerClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(CustomAttribute.Create(nonNestedInnerClassAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

        internal void SetNonNestedOuterClass(TypeSymbolBuilder typeBuilder, string className)
        {
            nonNestedOuterClassAttribute ??= TypeOfNonNestedOuterClassAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(CustomAttribute.Create(nonNestedOuterClassAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className)]));
        }

#endif // IMPORTER

        internal void HideFromReflection(ICustomAttributeBuilder mb)
        {
            mb.SetCustomAttribute(HideFromReflectionBuilder);
        }

        internal void HideFromJava(ICustomAttributeBuilder typeBuilder)
        {
            typeBuilder.SetCustomAttribute(HideFromJavaAttributeBuilder);
        }

        internal void HideFromJava(ICustomAttributeBuilder mb, HideFromJavaFlags flags)
        {
            mb.SetCustomAttribute(CustomAttribute.Create(TypeOfHideFromJavaAttribute.GetConstructor([TypeOfHideFromJavaFlags]), [flags]));
        }

        internal bool IsHideFromJava(TypeSymbol type)
        {
            return type.IsDefined(TypeOfHideFromJavaAttribute, false) || (type.IsNested && (type.DeclaringType.IsDefined(TypeOfHideFromJavaAttribute, false) || type.Name.StartsWith("__<", StringComparison.Ordinal)));
        }

        internal bool IsHideFromJava(MemberSymbol mi)
        {
            return (GetHideFromJavaFlags(mi) & HideFromJavaFlags.Code) != 0;
        }

        internal HideFromJavaFlags GetHideFromJavaFlags(MemberSymbol mi)
        {
            // NOTE all privatescope fields and methods are "hideFromJava"
            // because Java cannot deal with the potential name clashes
            var fi = mi as FieldSymbol;
            if (fi != null && (fi.Attributes & System.Reflection.FieldAttributes.FieldAccessMask) == System.Reflection.FieldAttributes.PrivateScope)
                return HideFromJavaFlags.All;

            var mb = mi as MethodSymbol;
            if (mb != null && (mb.Attributes & System.Reflection.MethodAttributes.MemberAccessMask) == System.Reflection.MethodAttributes.PrivateScope)
                return HideFromJavaFlags.All;
            if (mi.Name.StartsWith("__<", StringComparison.Ordinal))
                return HideFromJavaFlags.All;

            var attr = mi.GetCustomAttributes(TypeOfHideFromJavaAttribute).ToImmutableArray();
            if (attr.Length == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Length == 1)
                    return (HideFromJavaFlags)args[0].Value;

                return HideFromJavaFlags.All;
            }

            return HideFromJavaFlags.None;
        }

        internal void SetImplementsAttribute(TypeSymbolBuilder typeBuilder, RuntimeJavaType[] ifaceWrappers)
        {
            var interfaces = new string[ifaceWrappers.Length];
            for (int i = 0; i < interfaces.Length; i++)
                interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);

            if (implementsAttribute == null)
                implementsAttribute = TypeOfImplementsAttribute.GetConstructor([context.Types.String.MakeArrayType()]);

            typeBuilder.SetCustomAttribute(CustomAttribute.Create(implementsAttribute, [interfaces]));
        }

        internal bool IsGhostInterface(TypeSymbol type)
        {
            return type.IsDefined(TypeOfGhostInterfaceAttribute, false);
        }

        internal bool IsRemappedType(TypeSymbol type)
        {
            return type.IsDefined(TypeOfRemappedTypeAttribute, false);
        }

        internal bool IsExceptionIsUnsafeForMapping(TypeSymbol type)
        {
            return type.IsDefined(TypeOfExceptionIsUnsafeForMappingAttribute, false);
        }

        internal ModifiersAttribute GetModifiersAttribute(MemberSymbol member)
        {
            var attr = member.GetCustomAttributes(TypeOfModifiersAttribute, false).Take(3).ToImmutableArray();
            if (attr.Length == 1)
            {
                var args = attr[0].ConstructorArguments;
                if (args.Length == 2)
                    return new ModifiersAttribute((Modifiers)args[0].Value, (bool)args[1].Value);

                return new ModifiersAttribute((Modifiers)args[0].Value);
            }

            return null;
        }

        internal ExModifiers GetModifiers(MethodSymbol mb, bool assemblyIsPrivate)
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
                if ((mb.MethodImplementationFlags & System.Reflection.MethodImplAttributes.Synchronized) != 0)
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

            var parameters = mb.Parameters;
            if (parameters.Length > 0 && parameters[parameters.Length - 1].IsDefined(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName), false))
                modifiers |= Modifiers.VarArgs;

            return new ExModifiers(modifiers, false);
        }

        internal ExModifiers GetModifiers(FieldSymbol fi, bool assemblyIsPrivate)
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

            if (fi.GetRequiredCustomModifiers().IndexOf(context.Types.IsVolatile) != -1)
            {
                modifiers |= Modifiers.Volatile;
            }

            return new ExModifiers(modifiers, false);
        }

        internal void SetDebuggingModes(AssemblySymbolBuilder assemblyBuilder, DebuggableAttribute.DebuggingModes modes)
        {
            debuggableAttribute ??= TypeOfDebuggableAttribute.GetConstructor([TypeOfDebuggableAttribute.GetNestedType(nameof(DebuggableAttribute.DebuggingModes))]);
            assemblyBuilder.SetCustomAttribute(CustomAttribute.Create(debuggableAttribute, [modes]));
        }

        internal void SetModifiers(ICustomAttributeBuilder fb, Modifiers modifiers, bool isInternal)
        {
            if (isInternal)
                fb.SetCustomAttribute(CustomAttribute.Create(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers, context.Types.Boolean]), [modifiers, isInternal]));
            else
                fb.SetCustomAttribute(CustomAttribute.Create(TypeOfModifiersAttribute.GetConstructor([TypeOfModifiers]), [modifiers]));
        }

        internal void SetNameSig(ICustomAttributeBuilder mb, string name, string sig)
        {
            var customAttributeBuilder = CustomAttribute.Create(TypeOfNameSigAttribute.GetConstructor([context.Types.String, context.Types.String]), [UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig)]);
            mb.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetInnerClass(TypeSymbolBuilder typeBuilder, string innerClass, Modifiers modifiers)
        {
            var ci = TypeOfInnerClassAttribute.GetConstructor([context.Types.String, TypeOfModifiers]);
            var customAttributeBuilder = CustomAttribute.Create(ci, [UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers]);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        internal void SetSourceFile(ICustomAttributeBuilder typeBuilder, string filename)
        {
            sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor([context.Types.String]);
            typeBuilder.SetCustomAttribute(CustomAttribute.Create(sourceFileAttribute, [filename]));
        }

        internal void SetLineNumberTable(ICustomAttributeBuilder builder, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
        {
            object arg;
            MethodSymbol con;
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

            builder.SetCustomAttribute(CustomAttribute.Create(con, [arg]));
        }

        internal void SetEnclosingMethodAttribute(TypeSymbolBuilder tb, string className, string methodName, string methodSig)
        {
            enclosingMethodAttribute ??= TypeOfEnclosingMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String]);
            tb.SetCustomAttribute(CustomAttribute.Create(enclosingMethodAttribute, [UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig)]));
        }

        internal void SetSignatureAttribute(ICustomAttributeBuilder builder, string signature)
        {
            signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor([context.Types.String]);
            builder.SetCustomAttribute(CustomAttribute.Create(signatureAttribute, [UnicodeUtil.EscapeInvalidSurrogates(signature)]));
        }

        internal void SetMethodParametersAttribute(MethodSymbolBuilder mb, Modifiers[] modifiers)
        {
            methodParametersAttribute ??= TypeOfMethodParametersAttribute.GetConstructor([TypeOfModifiers.MakeArrayType()]);
            mb.SetCustomAttribute(CustomAttribute.Create(methodParametersAttribute, [modifiers]));
        }

        internal void SetRuntimeVisibleTypeAnnotationsAttribute(ICustomAttributeBuilder tb, ref readonly TypeAnnotationTable table)
        {
            var builder = new BlobBuilder();
            var encoder = new TypeAnnotationTableEncoder(builder);
            table.WriteTo(ref encoder);

            runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
            tb.SetCustomAttribute(CustomAttribute.Create(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
        }

        internal void SetConstantPoolAttribute(TypeSymbolBuilder tb, object[] constantPool)
        {
            constantPoolAttribute ??= TypeOfConstantPoolAttribute.GetConstructor([context.Types.Object.MakeArrayType()]);
            tb.SetCustomAttribute(CustomAttribute.Create(constantPoolAttribute, [constantPool]));
        }

        internal void SetParamArrayAttribute(ParameterSymbolBuilder pb)
        {
            pb.SetCustomAttribute(paramArrayAttribute ??= CustomAttribute.Create(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName).GetConstructor([]), []));
        }

        internal NameSigAttribute GetNameSig(MemberSymbol member)
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
            var elems = (ImmutableArray<IKVM.CoreLib.Symbols.CustomAttributeTypedArgument>)arg.Value;
            var arr = new T[elems.Length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (T)elems[i].Value;

            return arr;
        }

        internal ImplementsAttribute GetImplements(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfImplementsAttribute))
            {
                var args = cad.ConstructorArguments;
                return new ImplementsAttribute(DecodeArray<string>(args[0]));
            }

            return null;
        }

        internal ThrowsAttribute GetThrows(MethodSymbol mb)
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
                    return new ThrowsAttribute(DecodeArray<TypeSymbol>(args[0]).GetUnderlyingTypes());
                }
                else
                {
                    return new ThrowsAttribute(((TypeSymbol)args[0].Value).GetUnderlyingType());
                }
            }

            return null;
        }

        internal string[] GetNonNestedInnerClasses(TypeSymbol t)
        {
            var list = new List<string>();
            foreach (var cad in t.GetCustomAttributes(TypeOfNonNestedInnerClassAttribute))
                list.Add(UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value));

            return list.ToArray();
        }

        internal string GetNonNestedOuterClasses(TypeSymbol t)
        {
            foreach (var cad in t.GetCustomAttributes(TypeOfNonNestedOuterClassAttribute))
                return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal IKVM.Attributes.SignatureAttribute GetSignature(MemberSymbol member)
        {
            foreach (var cad in member.GetCustomAttributes(TypeOfSignatureAttribute))
                return new IKVM.Attributes.SignatureAttribute((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal IKVM.Attributes.MethodParametersAttribute GetMethodParameters(MethodSymbol method)
        {
            foreach (var cad in method.GetCustomAttributes(TypeOfMethodParametersAttribute))
                return new IKVM.Attributes.MethodParametersAttribute(DecodeArray<Modifiers>(cad.ConstructorArguments[0]));

            return null;
        }

        internal object[] GetConstantPool(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfConstantPoolAttribute))
                return ConstantPoolAttribute.Decompress(DecodeArray<object>(cad.ConstructorArguments[0]));

            return null;
        }

        internal byte[] GetRuntimeVisibleTypeAnnotations(MemberSymbol member)
        {
            foreach (var cad in member.GetCustomAttributes(TypeOfRuntimeVisibleTypeAnnotationsAttribute))
                return DecodeArray<byte>(cad.ConstructorArguments[0]);

            return null;
        }

        internal InnerClassAttribute GetInnerClass(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfInnerClassAttribute))
            {
                var args = cad.ConstructorArguments;
                return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
            }

            return null;
        }

        internal RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(TypeSymbol type)
        {
            var attrs = new List<RemappedInterfaceMethodAttribute>();
            foreach (var cad in type.GetCustomAttributes(TypeOfRemappedInterfaceMethodAttribute))
            {
                var args = cad.ConstructorArguments;
                attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value, DecodeArray<string>(args[2])));
            }

            return attrs.ToArray();
        }

        internal RemappedTypeAttribute GetRemappedType(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfRemappedTypeAttribute))
                return new RemappedTypeAttribute(((TypeSymbol)cad.ConstructorArguments[0].Value).GetUnderlyingType());

            return null;
        }

        internal RemappedClassAttribute[] GetRemappedClasses(AssemblySymbol coreAssembly)
        {
            if (coreAssembly == null)
                throw new ArgumentNullException(nameof(coreAssembly));

            var attrs = new List<RemappedClassAttribute>();

            foreach (var cad in coreAssembly.GetCustomAttributes(TypeOfRemappedClassAttribute))
            {
                var args = cad.ConstructorArguments;
                attrs.Add(new RemappedClassAttribute((string)args[0].Value, ((TypeSymbol)args[1].Value).GetUnderlyingType()));
            }

            return attrs.ToArray();
        }

        internal string GetAnnotationAttributeType(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfAnnotationAttributeAttribute))
                return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);

            return null;
        }

        internal AssemblyIdentity[] GetInternalsVisibleToAttributes(AssemblySymbol assembly)
        {
            var list = new List<AssemblyIdentity>();

            foreach (var cad in assembly.GetCustomAttributes())
            {
                if (cad.Constructor.DeclaringType == context.Resolver.ResolveSystemType(typeof(InternalsVisibleToAttribute).FullName))
                {
                    try
                    {
                        list.Add(new AssemblyName((string)cad.ConstructorArguments[0].Value).Pack());
                    }
                    catch
                    {
                        // HACK since there is no list of exception that the AssemblyName constructor can throw, we simply catch all
                    }
                }
            }

            return list.ToArray();
        }

        internal bool IsJavaModule(ModuleSymbol mod)
        {
            return mod.IsDefined(TypeOfJavaModuleAttribute, false);
        }

        internal object[] GetJavaModuleAttributes(ModuleSymbol mod)
        {
            var l = mod.GetCustomAttributes(TypeOfJavaModuleAttribute).ToImmutableArray();
            var a = new object[l.Length];

            for (int i = 0; i < l.Length; i++)
            {
                JavaModuleAttribute attr;

                var args = l[i].ConstructorArguments;
                if (args.Length == 0)
                    attr = new JavaModuleAttribute();
                else
                    attr = new JavaModuleAttribute(DecodeArray<string>(args[0]));

                foreach (var arg in l[i].NamedArguments)
                {
                    switch (arg.MemberName)
                    {
                        case nameof(JavaModuleAttribute.Jars):
                            attr.Jars = DecodeArray<string>(arg.TypedValue);
                            break;
                    }
                }

                a[i] = attr;
            }

            return a;
        }

        internal bool IsNoPackagePrefix(TypeSymbol type)
        {
            return type.IsDefined(TypeOfNoPackagePrefixAttribute, false) || type.Assembly.IsDefined(TypeOfNoPackagePrefixAttribute, false);
        }

        internal bool HasEnclosingMethodAttribute(TypeSymbol type)
        {
            return type.IsDefined(TypeOfEnclosingMethodAttribute, false);
        }

        internal IKVM.Attributes.EnclosingMethodAttribute GetEnclosingMethodAttribute(TypeSymbol type)
        {
            foreach (var cad in type.GetCustomAttributes(TypeOfEnclosingMethodAttribute))
                return new IKVM.Attributes.EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(context, type);

            return null;
        }

        internal void SetRemappedClass(AssemblySymbolBuilder assemblyBuilder, string name, TypeSymbol shadowType)
        {
            var remappedClassAttribute = TypeOfRemappedClassAttribute.GetConstructor([context.Types.String, context.Types.Type]);
            assemblyBuilder.SetCustomAttribute(CustomAttribute.Create(remappedClassAttribute, [name, shadowType]));
        }

        internal void SetRemappedType(TypeSymbolBuilder typeBuilder, TypeSymbol shadowType)
        {
            var remappedTypeAttribute = TypeOfRemappedTypeAttribute.GetConstructor([context.Types.Type]);
            typeBuilder.SetCustomAttribute(CustomAttribute.Create(remappedTypeAttribute, [shadowType]));
        }

        internal void SetRemappedInterfaceMethod(TypeSymbolBuilder typeBuilder, string name, string mappedTo, string[] throws)
        {
            var cab = CustomAttribute.Create(TypeOfRemappedInterfaceMethodAttribute.GetConstructor([context.Types.String, context.Types.String, context.Types.String.MakeArrayType()]), [name, mappedTo, throws]);
            typeBuilder.SetCustomAttribute(cab);
        }

        internal void SetExceptionIsUnsafeForMapping(TypeSymbolBuilder typeBuilder)
        {
            var cab = CustomAttribute.Create(TypeOfExceptionIsUnsafeForMappingAttribute.GetConstructor([]), []);
            typeBuilder.SetCustomAttribute(cab);
        }

        internal void SetRuntimeCompatibilityAttribute(AssemblySymbolBuilder assemblyBuilder)
        {
            var runtimeCompatibilityAttribute = context.Resolver.ResolveCoreType(typeof(RuntimeCompatibilityAttribute).FullName);
            assemblyBuilder.SetCustomAttribute(CustomAttribute.Create(runtimeCompatibilityAttribute.GetConstructor([]), [], [runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows")], [true], [], []));
        }

        internal void SetInternalsVisibleToAttribute(AssemblySymbolBuilder assemblyBuilder, string assemblyName)
        {
            var internalsVisibleToAttribute = context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName);
            var cab = CustomAttribute.Create(internalsVisibleToAttribute.GetConstructor([context.Types.String]), [assemblyName]);
            assemblyBuilder.SetCustomAttribute(cab);
        }

        internal void SetCustomAssemblyClassLoaderAttribute(AssemblySymbolBuilder assemblyBuilder, TypeSymbol classLoaderType)
        {
            var cab = CustomAttribute.Create(TypeOfCustomAssemblyClassLoaderAttribute.GetConstructor([context.Types.Type]), [classLoaderType]);
            assemblyBuilder.SetCustomAttribute(cab);
        }

    }

}
