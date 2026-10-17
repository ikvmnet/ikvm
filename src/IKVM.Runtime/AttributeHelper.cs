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
using IKVM.ByteCode.Decoding;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Runtime;

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
		ConstructorInfo implementsAttribute;
		ConstructorInfo throwsAttribute;
		ConstructorInfo sourceFileAttribute;
		ConstructorInfo lineNumberTableAttribute1;
		ConstructorInfo lineNumberTableAttribute2;
		ConstructorInfo enclosingMethodAttribute;
		ConstructorInfo signatureAttribute;
		ConstructorInfo methodParametersAttribute;
		ConstructorInfo runtimeVisibleTypeAnnotationsAttribute;
		ConstructorInfo constantPoolAttribute;
		CustomAttributeBuilder paramArrayAttribute;
		ConstructorInfo nonNestedInnerClassAttribute;
		ConstructorInfo nonNestedOuterClassAttribute;

		Type typeofModifiers;
		Type typeofSourceFileAttribute;
		Type typeofLineNumberTableAttribute;

#endif

        Type typeofRemappedClassAttribute;
        Type typeofRemappedTypeAttribute;
        Type typeofModifiersAttribute;
        Type typeofRemappedInterfaceMethodAttribute;
        Type typeofNameSigAttribute;
        Type typeofJavaModuleAttribute;
        Type typeofSignatureAttribute;
        Type typeofInnerClassAttribute;
        Type typeofImplementsAttribute;
        Type typeofGhostInterfaceAttribute;
        Type typeofExceptionIsUnsafeForMappingAttribute;
        Type typeofThrowsAttribute;
        Type typeofHideFromJavaAttribute;
        Type typeofHideFromJavaFlags;
        Type typeofNoPackagePrefixAttribute;
        Type typeofAnnotationAttributeAttribute;
        Type typeofNonNestedInnerClassAttribute;
        Type typeofNonNestedOuterClassAttribute;
        Type typeofEnclosingMethodAttribute;
        Type typeofMethodParametersAttribute;
        Type typeofRuntimeVisibleTypeAnnotationsAttribute;
        Type typeofConstantPoolAttribute;
        Type typeofDebuggableAttribute;
        CustomAttributeBuilder hideFromJavaAttribute;
        CustomAttributeBuilder hideFromReflection;
        ConstructorInfo debuggableAttribute;

#if IMPORTER

		Type TypeOfModifiers => typeofModifiers ??= LoadType(typeof(Modifiers));

		Type TypeOfSourceFileAttribute => typeofSourceFileAttribute ??= LoadType(typeof(IKVM.Attributes.SourceFileAttribute));

		Type TypeOfLineNumberTableAttribute => typeofLineNumberTableAttribute ??= LoadType(typeof(IKVM.Attributes.LineNumberTableAttribute));

#endif

        Type TypeOfRemappedClassAttribute => typeofRemappedClassAttribute ??= LoadType(typeof(RemappedClassAttribute));

        Type TypeOfRemappedTypeAttribute => typeofRemappedTypeAttribute ??= LoadType(typeof(RemappedTypeAttribute));

        Type TypeOfModifiersAttribute => typeofModifiersAttribute ??= LoadType(typeof(ModifiersAttribute));

        Type TypeOfRemappedInterfaceMethodAttribute => typeofRemappedInterfaceMethodAttribute ??= LoadType(typeof(RemappedInterfaceMethodAttribute));

        Type TypeOfNameSigAttribute => typeofNameSigAttribute ??= LoadType(typeof(NameSigAttribute));

        Type TypeOfJavaModuleAttribute => typeofJavaModuleAttribute ??= LoadType(typeof(JavaModuleAttribute));

        Type TypeOfSignatureAttribute => typeofSignatureAttribute ??= LoadType(typeof(IKVM.Attributes.SignatureAttribute));

        Type TypeOfInnerClassAttribute => typeofInnerClassAttribute ??= LoadType(typeof(InnerClassAttribute));

        Type TypeOfImplementsAttribute => typeofImplementsAttribute ??= LoadType(typeof(ImplementsAttribute));

        Type TypeOfGhostInterfaceAttribute => typeofGhostInterfaceAttribute ??= LoadType(typeof(GhostInterfaceAttribute));

        Type TypeOfExceptionIsUnsafeForMappingAttribute => typeofExceptionIsUnsafeForMappingAttribute ??= LoadType(typeof(ExceptionIsUnsafeForMappingAttribute));

        Type TypeOfThrowsAttribute => typeofThrowsAttribute ??= LoadType(typeof(ThrowsAttribute));

        Type TypeOfHideFromJavaAttribute => typeofHideFromJavaAttribute ??= LoadType(typeof(HideFromJavaAttribute));

        Type TypeOfHideFromJavaFlags => typeofHideFromJavaFlags ??= LoadType(typeof(HideFromJavaFlags));

        Type TypeOfNoPackagePrefixAttribute => typeofNoPackagePrefixAttribute ??= LoadType(typeof(NoPackagePrefixAttribute));

        Type TypeOfAnnotationAttributeAttribute => typeofAnnotationAttributeAttribute ??= LoadType(typeof(AnnotationAttributeAttribute));

        Type TypeOfNonNestedInnerClassAttribute => typeofNonNestedInnerClassAttribute ??= LoadType(typeof(NonNestedInnerClassAttribute));

        Type TypeOfNonNestedOuterClassAttribute => typeofNonNestedOuterClassAttribute ??= LoadType(typeof(NonNestedOuterClassAttribute));

        Type TypeOfEnclosingMethodAttribute => typeofEnclosingMethodAttribute ??= LoadType(typeof(IKVM.Attributes.EnclosingMethodAttribute));

        Type TypeOfMethodParametersAttribute => typeofMethodParametersAttribute ??= LoadType(typeof(IKVM.Attributes.MethodParametersAttribute));

        Type TypeOfRuntimeVisibleTypeAnnotationsAttribute => typeofRuntimeVisibleTypeAnnotationsAttribute ??= LoadType(typeof(IKVM.Attributes.RuntimeVisibleTypeAnnotationsAttribute));

        Type TypeOfConstantPoolAttribute => typeofConstantPoolAttribute ??= LoadType(typeof(ConstantPoolAttribute));

        Type TypeOfDebuggableAttribute => typeofDebuggableAttribute ??= context.Resolver.ResolveCoreType(typeof(DebuggableAttribute).FullName).AsReflection();

        CustomAttributeBuilder HideFromJavaAttributeBuilder => hideFromJavaAttribute ??= new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), Array.Empty<object>());

        CustomAttributeBuilder HideFromReflectionBuilder => hideFromReflection ??= new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor(new[] { TypeOfHideFromJavaFlags }), new object[] { HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk });

        /// <summary>
        /// Loads the given managed type from the runtime assembly.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Type LoadType(System.Type t)
        {
            return context.Resolver.ResolveRuntimeType(t.FullName).AsReflection();
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

		void GetAttributeArgsAndTypes(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr, out Type[] argTypes, out object[] args)
		{
			// TODO add error handling
			var twargs = loader.ArgJavaTypeListFromSig(attr.Sig, LoadMode.Link);
			argTypes = new Type[twargs.Length];
			args = new object[argTypes.Length];
			for (int i = 0; i < twargs.Length; i++)
			{
				argTypes[i] = twargs[i].TypeAsSignatureType;
				RuntimeJavaType tw = twargs[i];
				if (tw == context.JavaBase.TypeOfJavaLangObject)
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

		CustomAttributeBuilder CreateCustomAttribute(RuntimeClassLoader loader, IKVM.Tools.Importer.MapXml.Attribute attr)
		{
			// TODO add error handling
			Type[] argTypes;
			object[] args;
			GetAttributeArgsAndTypes(loader, attr, out argTypes, out args);
			if (attr.Type != null)
			{
				var t = context.Resolver.ResolveCoreType(attr.Type).AsReflection();
				var ci = t.GetConstructor(argTypes);
				if (ci == null)
					throw new InvalidOperationException($"Constructor missing: {attr.Type}::<init>{attr.Sig}");

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
				RuntimeJavaType t = loader.LoadClassByName(attr.Class);
				FieldInfo[] namedFields;
				object[] fieldValues;
				if (attr.Fields != null)
				{
					namedFields = new FieldInfo[attr.Fields.Length];
					fieldValues = new object[attr.Fields.Length];
					for (int i = 0; i < namedFields.Length; i++)
					{
						RuntimeJavaField fw = t.GetFieldWrapper(attr.Fields[i].Name, attr.Fields[i].Sig);
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
				var mw = t.GetMethod("<init>", attr.Sig, false);
				if (mw == null)
				{
					throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));
				}
				mw.Link();
				ConstructorInfo ci = (mw.GetMethod() as ConstructorInfo) ?? ((MethodInfo)mw.GetMethod()).__AsConstructorInfo();
				return new CustomAttributeBuilder(ci, args, namedFields, fieldValues);
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
			compilerGeneratedAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).AsReflection().GetConstructor(Type.EmptyTypes), Array.Empty<object>());
			tb.SetCustomAttribute(compilerGeneratedAttribute);
		}

		internal void SetCompilerGenerated(MethodBuilder mb)
		{
			compilerGeneratedAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(CompilerGeneratedAttribute).FullName).AsReflection().GetConstructor(Type.EmptyTypes), Array.Empty<object>());
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
				deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());

			mb.SetCustomAttribute(deprecatedAttribute);
		}

		internal void SetDeprecatedAttribute(TypeBuilder tb)
		{
			if (deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());
			}

			tb.SetCustomAttribute(deprecatedAttribute);
		}

		internal void SetDeprecatedAttribute(FieldBuilder fb)
		{
			if (deprecatedAttribute == null)
				deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());

			fb.SetCustomAttribute(deprecatedAttribute);
		}

		internal void SetDeprecatedAttribute(PropertyBuilder pb)
		{
			if (deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ObsoleteAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());
			}
			pb.SetCustomAttribute(deprecatedAttribute);
		}

		internal void SetThrowsAttribute(MethodBuilder mb, string[] exceptions)
		{
			if (exceptions != null && exceptions.Length != 0)
			{
				throwsAttribute ??= TypeOfThrowsAttribute.GetConstructor(new Type[] { context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType().AsReflection() });
				exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
				mb.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute, new object[] { exceptions }));
			}
		}

		internal void SetGhostInterface(TypeBuilder typeBuilder)
		{
			ghostInterfaceAttribute ??= new CustomAttributeBuilder(TypeOfGhostInterfaceAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
		}

		internal void SetNonNestedInnerClass(TypeBuilder typeBuilder, string className)
		{
			nonNestedInnerClassAttribute ??= TypeOfNonNestedInnerClassAttribute.GetConstructor(new Type[] { context.Types.String });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedInnerClassAttribute, new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
		}

		internal void SetNonNestedOuterClass(TypeBuilder typeBuilder, string className)
		{
			nonNestedOuterClassAttribute ??= TypeOfNonNestedOuterClassAttribute.GetConstructor(new Type[] { context.Types.String });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedOuterClassAttribute, new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
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
            CustomAttributeBuilder cab = new CustomAttributeBuilder(TypeOfHideFromJavaAttribute.GetConstructor(new Type[] { TypeOfHideFromJavaFlags }), new object[] { flags });
            mb.SetCustomAttribute(cab);
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

        internal bool IsHideFromJava(Type type)
        {
            return type.IsDefined(TypeOfHideFromJavaAttribute, false) || (type.IsNested && (type.DeclaringType.IsDefined(TypeOfHideFromJavaAttribute, false) || type.Name.StartsWith("__<", StringComparison.Ordinal)));
        }

        internal bool IsHideFromJava(MemberInfo mi)
        {
            return (GetHideFromJavaFlags(mi) & HideFromJavaFlags.Code) != 0;
        }

        internal HideFromJavaFlags GetHideFromJavaFlags(MemberInfo mi)
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
			var attr = CustomAttributeData.__GetCustomAttributes(mi, TypeOfHideFromJavaAttribute, false);
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

		internal void SetImplementsAttribute(TypeBuilder typeBuilder, RuntimeJavaType[] ifaceWrappers)
		{
			var interfaces = new string[ifaceWrappers.Length];
			for (int i = 0; i < interfaces.Length; i++)
				interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);

			if (implementsAttribute == null)
				implementsAttribute = TypeOfImplementsAttribute.GetConstructor(new Type[] { context.Resolver.ResolveCoreType(typeof(string).FullName).MakeArrayType().AsReflection() });

			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { interfaces }));
		}

#endif

        internal bool IsGhostInterface(Type type)
        {
            return type.IsDefined(TypeOfGhostInterfaceAttribute, false);
        }

        internal bool IsRemappedType(Type type)
        {
            return type.IsDefined(TypeOfRemappedTypeAttribute, false);
        }

        internal bool IsExceptionIsUnsafeForMapping(Type type)
        {
            return type.IsDefined(TypeOfExceptionIsUnsafeForMappingAttribute, false);
        }

        internal ModifiersAttribute GetModifiersAttribute(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            var attr = member.GetCustomAttributes(typeof(ModifiersAttribute), false);
            return attr.Length == 1 ? (ModifiersAttribute)attr[0] : null;
#else
			var attr = CustomAttributeData.__GetCustomAttributes(member, TypeOfModifiersAttribute, false);
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

        internal ExModifiers GetModifiers(MethodBase mb, bool assemblyIsPrivate)
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
            if (parameters.Length > 0 && parameters[parameters.Length - 1].IsDefined(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName).AsReflection(), false))
                modifiers |= Modifiers.VarArgs;

            return new ExModifiers(modifiers, false);
        }

        internal ExModifiers GetModifiers(FieldInfo fi, bool assemblyIsPrivate)
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
            debuggableAttribute ??= TypeOfDebuggableAttribute.GetConstructor(new[] { TypeOfDebuggableAttribute.GetNestedType(nameof(DebuggableAttribute.DebuggingModes)) });
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(debuggableAttribute, new object[] { modes }));
        }

#if IMPORTER

		internal void SetModifiers(MethodBuilder mb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers, context.Types.Boolean }), new object[] { modifiers, isInternal });
			else
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers }), new object[] { modifiers });

			mb.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetModifiers(FieldBuilder fb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers, context.Types.Boolean }), new object[] { modifiers, isInternal });
			else
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers }), new object[] { modifiers });

			fb.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetModifiers(PropertyBuilder pb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers, context.Types.Boolean }), new object[] { modifiers, isInternal });
			else
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers }), new object[] { modifiers });

			pb.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetModifiers(TypeBuilder tb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers, context.Types.Boolean }), new object[] { modifiers, isInternal });
			else
				customAttributeBuilder = new CustomAttributeBuilder(TypeOfModifiersAttribute.GetConstructor(new Type[] { TypeOfModifiers }), new object[] { modifiers });

			tb.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetNameSig(MethodBuilder mb, string name, string sig)
		{
			var customAttributeBuilder = new CustomAttributeBuilder(TypeOfNameSigAttribute.GetConstructor(new Type[] { context.Types.String, context.Types.String }), new object[] { UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig) });
			mb.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetInnerClass(TypeBuilder typeBuilder, string innerClass, Modifiers modifiers)
		{
			var argTypes = new Type[] { context.Types.String, TypeOfModifiers };
			var args = new object[] { UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers };
			var ci = TypeOfInnerClassAttribute.GetConstructor(argTypes);
			var customAttributeBuilder = new CustomAttributeBuilder(ci, args);
			typeBuilder.SetCustomAttribute(customAttributeBuilder);
		}

		internal void SetSourceFile(TypeBuilder typeBuilder, string filename)
		{
			sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor(new Type[] { context.Types.String });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal void SetSourceFile(ModuleBuilder moduleBuilder, string filename)
		{
			sourceFileAttribute ??= TypeOfSourceFileAttribute.GetConstructor(new Type[] { context.Types.String });
			moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal void SetLineNumberTable(MethodBuilder mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
		{
			object arg;
			ConstructorInfo con;
			if (writer.Count == 1)
			{
				lineNumberTableAttribute2 ??= TypeOfLineNumberTableAttribute.GetConstructor(new Type[] { context.Types.UInt16 });
				con = lineNumberTableAttribute2;
				arg = (ushort)writer.LineNo;
			}
			else
			{
				lineNumberTableAttribute1 ??= TypeOfLineNumberTableAttribute.GetConstructor(new Type[] { context.Resolver.ResolveCoreType(typeof(byte).FullName).MakeArrayType().AsReflection() });
				con = lineNumberTableAttribute1;
				arg = writer.ToArray();
			}
			mb.SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { arg }));
		}

		internal void SetEnclosingMethodAttribute(TypeBuilder tb, string className, string methodName, string methodSig)
		{
			if (enclosingMethodAttribute == null)
			{
				enclosingMethodAttribute = TypeOfEnclosingMethodAttribute.GetConstructor(new Type[] { context.Types.String, context.Types.String, context.Types.String });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(enclosingMethodAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig) }));
		}

		internal void SetSignatureAttribute(TypeBuilder tb, string signature)
		{
			signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor(new Type[] { context.Types.String });
			tb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal void SetSignatureAttribute(FieldBuilder fb, string signature)
		{
			signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor(new Type[] { context.Types.String });
			fb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal void SetSignatureAttribute(MethodBuilder mb, string signature)
		{
			signatureAttribute ??= TypeOfSignatureAttribute.GetConstructor(new Type[] { context.Types.String });
			mb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal void SetMethodParametersAttribute(MethodBuilder mb, Modifiers[] modifiers)
		{
			methodParametersAttribute ??= TypeOfMethodParametersAttribute.GetConstructor(new[] { TypeOfModifiers.MakeArrayType() });
			mb.SetCustomAttribute(new CustomAttributeBuilder(methodParametersAttribute, new[] { modifiers }));
		}

		internal void SetRuntimeVisibleTypeAnnotationsAttribute(TypeBuilder tb, ref readonly TypeAnnotationTable table)
		{
			var builder = new BlobBuilder();
			var encoder = new TypeAnnotationTableEncoder(builder);
			table.WriteTo(ref encoder);

			runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
			tb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
		}

		internal void SetRuntimeVisibleTypeAnnotationsAttribute(FieldBuilder fb, ref readonly TypeAnnotationTable table)
		{
			var builder = new BlobBuilder();
			var encoder = new TypeAnnotationTableEncoder(builder);
			table.WriteTo(ref encoder);

			runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
			fb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
		}

		internal void SetRuntimeVisibleTypeAnnotationsAttribute(MethodBuilder mb, ref readonly TypeAnnotationTable table)
		{
			var builder = new BlobBuilder();
			var encoder = new TypeAnnotationTableEncoder(builder);
			table.WriteTo(ref encoder);

			runtimeVisibleTypeAnnotationsAttribute ??= TypeOfRuntimeVisibleTypeAnnotationsAttribute.GetConstructor([context.Types.Byte.MakeArrayType()]);
			mb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, [builder.ToArray()]));
		}

		internal void SetConstantPoolAttribute(TypeBuilder tb, object[] constantPool)
		{
			constantPoolAttribute ??= TypeOfConstantPoolAttribute.GetConstructor([context.Types.Object.MakeArrayType()]);
			tb.SetCustomAttribute(new CustomAttributeBuilder(constantPoolAttribute, [constantPool]));
		}

		internal void SetParamArrayAttribute(ParameterBuilder pb)
		{
			paramArrayAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ParamArrayAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());
			pb.SetCustomAttribute(paramArrayAttribute);
		}

#endif  // IMPORTER

        internal NameSigAttribute GetNameSig(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            var attr = member.GetCustomAttributes(typeof(NameSigAttribute), false);
            return attr.Length == 1 ? (NameSigAttribute)attr[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(member, TypeOfNameSigAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
			}

			return null;
#endif
        }

        internal T[] DecodeArray<T>(CustomAttributeTypedArgument arg)
        {
            var elems = (IList<CustomAttributeTypedArgument>)arg.Value;
            var arr = new T[elems.Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (T)elems[i].Value;

            return arr;
        }

        internal ImplementsAttribute GetImplements(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attribs = type.GetCustomAttributes(typeof(ImplementsAttribute), false);
            return attribs.Length == 1 ? (ImplementsAttribute)attribs[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfImplementsAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return new ImplementsAttribute(DecodeArray<string>(args[0]));
			}

			return null;
#endif
        }

        internal ThrowsAttribute GetThrows(MethodBase mb)
        {
#if !IMPORTER && !EXPORTER
            var attribs = mb.GetCustomAttributes(typeof(ThrowsAttribute), false);
            return attribs.Length == 1 ? (ThrowsAttribute)attribs[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(mb, TypeOfThrowsAttribute, false))
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
#endif
        }

        internal string[] GetNonNestedInnerClasses(Type t)
        {
#if !IMPORTER && !EXPORTER
            var attribs = t.GetCustomAttributes(typeof(NonNestedInnerClassAttribute), false);
            var classes = new string[attribs.Length];
            for (int i = 0; i < attribs.Length; i++)
                classes[i] = ((NonNestedInnerClassAttribute)attribs[i]).InnerClassName;

            return classes;
#else
			var list = new List<string>();
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(t, TypeOfNonNestedInnerClassAttribute, false))
			{
				var args = cad.ConstructorArguments;
				list.Add(UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value));
			}

			return list.ToArray();
#endif
        }

        internal string GetNonNestedOuterClasses(Type t)
        {
#if !IMPORTER && !EXPORTER
            var attribs = t.GetCustomAttributes(typeof(NonNestedOuterClassAttribute), false);
            return attribs.Length == 1 ? ((NonNestedOuterClassAttribute)attribs[0]).OuterClassName : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(t, TypeOfNonNestedOuterClassAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value);
			}
			return null;
#endif
        }

        internal IKVM.Attributes.SignatureAttribute GetSignature(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            var attribs = member.GetCustomAttributes(typeof(IKVM.Attributes.SignatureAttribute), false);
            return attribs.Length == 1 ? (IKVM.Attributes.SignatureAttribute)attribs[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(member, TypeOfSignatureAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return new IKVM.Attributes.SignatureAttribute((string)args[0].Value);
			}

			return null;
#endif
        }

        internal IKVM.Attributes.MethodParametersAttribute GetMethodParameters(MethodBase method)
        {
#if !IMPORTER && !EXPORTER
            var attribs = method.GetCustomAttributes(typeof(IKVM.Attributes.MethodParametersAttribute), false);
            return attribs.Length == 1 ? (IKVM.Attributes.MethodParametersAttribute)attribs[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(method, TypeOfMethodParametersAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return new IKVM.Attributes.MethodParametersAttribute(DecodeArray<Modifiers>(args[0]));
			}
			return null;
#endif
        }

        internal object[] GetConstantPool(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attribs = type.GetCustomAttributes(typeof(ConstantPoolAttribute), false);
            return attribs.Length == 1 ? ((ConstantPoolAttribute)attribs[0]).constantPool : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfConstantPoolAttribute, false))
				return ConstantPoolAttribute.Decompress(DecodeArray<object>(cad.ConstructorArguments[0]));

			return null;
#endif
        }

        internal byte[] GetRuntimeVisibleTypeAnnotations(MemberInfo member)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = member.GetCustomAttributes(typeof(IKVM.Attributes.RuntimeVisibleTypeAnnotationsAttribute), false);
            return attribs.Length == 1 ? ((IKVM.Attributes.RuntimeVisibleTypeAnnotationsAttribute)attribs[0]).data : null;
#else
			foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, TypeOfRuntimeVisibleTypeAnnotationsAttribute, false))
			{
				return DecodeArray<byte>(cad.ConstructorArguments[0]);
			}

			return null;
#endif
        }

        internal InnerClassAttribute GetInnerClass(Type type)
        {
#if !IMPORTER && !EXPORTER
            object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
            return attribs.Length == 1 ? (InnerClassAttribute)attribs[0] : null;
#else
			foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfInnerClassAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
			}
			return null;
#endif
        }

        internal RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attr = type.GetCustomAttributes(typeof(RemappedInterfaceMethodAttribute), false);
            var attr1 = new RemappedInterfaceMethodAttribute[attr.Length];
            Array.Copy(attr, attr1, attr.Length);
            return attr1;
#else
			var attrs = new List<RemappedInterfaceMethodAttribute>();
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfRemappedInterfaceMethodAttribute, false))
			{
				var args = cad.ConstructorArguments;
				attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value, DecodeArray<string>(args[2])));
			}

			return attrs.ToArray();
#endif
        }

        internal RemappedTypeAttribute GetRemappedType(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
            return attribs.Length == 1 ? (RemappedTypeAttribute)attribs[0] : null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfRemappedTypeAttribute, false))
			{
				var args = cad.ConstructorArguments;
				return new RemappedTypeAttribute((Type)args[0].Value);
			}

			return null;
#endif
        }

        internal RemappedClassAttribute[] GetRemappedClasses(Assembly coreAssembly)
        {
#if !IMPORTER && !EXPORTER
            var attr = coreAssembly.GetCustomAttributes(typeof(RemappedClassAttribute), false);
            var attr1 = new RemappedClassAttribute[attr.Length];
            Array.Copy(attr, attr1, attr.Length);
            return attr1;
#else
			var attrs = new List<RemappedClassAttribute>();
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(coreAssembly, TypeOfRemappedClassAttribute, false))
			{
				var args = cad.ConstructorArguments;
				attrs.Add(new RemappedClassAttribute((string)args[0].Value, (Type)args[1].Value));
			}

			return attrs.ToArray();
#endif
        }

        internal string GetAnnotationAttributeType(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attr = type.GetCustomAttributes(typeof(AnnotationAttributeAttribute), false);
            if (attr.Length == 1)
                return ((AnnotationAttributeAttribute)attr[0]).AttributeType;

            return null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfAnnotationAttributeAttribute, false))
				return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);

			return null;
#endif
        }

        internal AssemblyName[] GetInternalsVisibleToAttributes(Assembly assembly)
        {
            var list = new List<AssemblyName>();
            foreach (var cad in CustomAttributeData.GetCustomAttributes(assembly))
            {
                if (cad.Constructor.DeclaringType == context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName).AsReflection())
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

        internal bool IsJavaModule(Module mod)
        {
            return mod.IsDefined(TypeOfJavaModuleAttribute, false);
        }

        internal object[] GetJavaModuleAttributes(Module mod)
        {
#if !IMPORTER && !EXPORTER
            return mod.GetCustomAttributes(TypeOfJavaModuleAttribute, false);
#else
			List<JavaModuleAttribute> attrs = new List<JavaModuleAttribute>();
			foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(mod, TypeOfJavaModuleAttribute, false))
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

        internal bool IsNoPackagePrefix(Type type)
        {
            return type.IsDefined(TypeOfNoPackagePrefixAttribute, false) || type.Assembly.IsDefined(TypeOfNoPackagePrefixAttribute, false);
        }

        internal bool HasEnclosingMethodAttribute(Type type)
        {
            return type.IsDefined(TypeOfEnclosingMethodAttribute, false);
        }

        internal IKVM.Attributes.EnclosingMethodAttribute GetEnclosingMethodAttribute(Type type)
        {
#if !IMPORTER && !EXPORTER
            var attr = type.GetCustomAttributes(typeof(IKVM.Attributes.EnclosingMethodAttribute), false);
            if (attr.Length == 1)
                return ((IKVM.Attributes.EnclosingMethodAttribute)attr[0]).SetClassName(context, type);

            return null;
#else
			foreach (var cad in CustomAttributeData.__GetCustomAttributes(type, TypeOfEnclosingMethodAttribute, false))
				return new IKVM.Attributes.EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(context, type);

			return null;
#endif
        }

#if IMPORTER

		internal void SetRemappedClass(AssemblyBuilder assemblyBuilder, string name, Type shadowType)
		{
			var remappedClassAttribute = TypeOfRemappedClassAttribute.GetConstructor(new Type[] { context.Types.String, context.Types.Type });
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, shadowType }));
		}

		internal void SetRemappedType(TypeBuilder typeBuilder, Type shadowType)
		{
			var remappedTypeAttribute = TypeOfRemappedTypeAttribute.GetConstructor(new Type[] { context.Types.Type });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { shadowType }));
		}

		internal void SetRemappedInterfaceMethod(TypeBuilder typeBuilder, string name, string mappedTo, string[] throws)
		{
			var cab = new CustomAttributeBuilder(TypeOfRemappedInterfaceMethodAttribute.GetConstructor(new Type[] { context.Types.String, context.Types.String, context.Types.String.MakeArrayType() }), new object[] { name, mappedTo, throws });
			typeBuilder.SetCustomAttribute(cab);
		}

		internal void SetExceptionIsUnsafeForMapping(TypeBuilder typeBuilder)
		{
			var cab = new CustomAttributeBuilder(TypeOfExceptionIsUnsafeForMappingAttribute.GetConstructor(Type.EmptyTypes), Array.Empty<object>());
			typeBuilder.SetCustomAttribute(cab);
		}

#endif

        internal void SetRuntimeCompatibilityAttribute(AssemblyBuilder assemblyBuilder)
        {
            var runtimeCompatibilityAttribute = context.Resolver.ResolveCoreType(typeof(RuntimeCompatibilityAttribute).FullName).AsReflection();
            assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(runtimeCompatibilityAttribute.GetConstructor(Type.EmptyTypes), Array.Empty<object>(), new PropertyInfo[] { runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows") }, new object[] { true }, Array.Empty<FieldInfo>(), Array.Empty<object>()));
        }

        internal void SetInternalsVisibleToAttribute(AssemblyBuilder assemblyBuilder, string assemblyName)
        {
            var internalsVisibleToAttribute = context.Resolver.ResolveCoreType(typeof(InternalsVisibleToAttribute).FullName).AsReflection();
            var cab = new CustomAttributeBuilder(internalsVisibleToAttribute.GetConstructor(new Type[] { context.Types.String }), new object[] { assemblyName });
            assemblyBuilder.SetCustomAttribute(cab);
        }

    }

}
