/*
  Copyright (C) 2002-2009 Jeroen Frijters

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
using System.Reflection;
#if IKVM_REF_EMIT
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;

namespace IKVM.Internal
{
	class AotTypeWrapper : DynamicTypeWrapper
	{
		private FieldInfo ghostRefField;
		private MethodBuilder ghostIsInstanceMethod;
		private MethodBuilder ghostIsInstanceArrayMethod;
		private MethodBuilder ghostCastMethod;
		private MethodBuilder ghostCastArrayMethod;
		private TypeBuilder typeBuilderGhostInterface;
		private Annotation annotation;
		private MethodWrapper[] replacedMethods;
		private WorkaroundBaseClass workaroundBaseClass;

		internal AotTypeWrapper(ClassFile f, CompilerClassLoader loader)
			: base(f, loader)
		{
		}

		protected override Type GetBaseTypeForDefineType()
		{
			TypeWrapper baseTypeWrapper = BaseTypeWrapper;
			if (this.IsPublic && this.IsAbstract && baseTypeWrapper.IsPublic && baseTypeWrapper.IsAbstract)
			{
				// FXBUG
				// if the current class widens access on an abstract base class method,
				// we need to inject an artificial base class to workaround a C# compiler bug
				List<MethodWrapper> methods = null;
				foreach (MethodWrapper mw in GetMethods())
				{
					if (!mw.IsStatic && mw.IsPublic)
					{
						MethodWrapper baseMethod = baseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
						if (baseMethod != null && baseMethod.IsAbstract && baseMethod.IsProtected)
						{
							if (methods == null)
							{
								methods = new List<MethodWrapper>();
							}
							methods.Add(baseMethod);
						}
					}
				}
				if (methods != null)
				{
					string name = "__WorkaroundBaseClass__." + Name;
					while (!classLoader.GetTypeWrapperFactory().ReserveName(name))
					{
						name = "_" + name;
					}
					TypeBuilder typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name, TypeAttributes.Public | TypeAttributes.Abstract, base.GetBaseTypeForDefineType());
					AttributeHelper.HideFromJava(typeBuilder);
					AttributeHelper.SetEditorBrowsableNever(typeBuilder);
					workaroundBaseClass = new WorkaroundBaseClass(typeBuilder, methods.ToArray());
					List<MethodWrapper> constructors = new List<MethodWrapper>();
					foreach (MethodWrapper mw in baseTypeWrapper.GetMethods())
					{
						if (ReferenceEquals(mw.Name, StringConstants.INIT) && mw.IsAccessibleFrom(baseTypeWrapper, this, this))
						{
							constructors.Add(new ConstructorForwarder(typeBuilder, mw));
						}
					}
					Serialization.AddAutomagicSerializationToWorkaroundBaseClass(typeBuilder);
					replacedMethods = constructors.ToArray();
					return typeBuilder;
				}
			}
			return base.GetBaseTypeForDefineType();
		}

		internal override void Finish()
		{
			base.Finish();
			lock (this)
			{
				if (workaroundBaseClass != null)
				{
					workaroundBaseClass.Finish();
					workaroundBaseClass = null;
				}
			}
		}

		private sealed class WorkaroundBaseClass
		{
			private readonly TypeBuilder typeBuilder;
			private readonly MethodWrapper[] methods;

			internal WorkaroundBaseClass(TypeBuilder typeBuilder, MethodWrapper[] methods)
			{
				this.typeBuilder = typeBuilder;
				this.methods = methods;
			}

			internal void Finish()
			{
				foreach (MethodWrapper mw in methods)
				{
					MethodBuilder mb = typeBuilder.DefineMethod(mw.Name, MethodAttributes.FamORAssem | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.CheckAccessOnOverride, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
					AttributeHelper.HideFromJava(mb);
					CodeEmitter ilgen = CodeEmitter.Create(mb);
					ilgen.EmitThrow("java.lang.AbstractMethodError");
				}
				typeBuilder.CreateType();
			}
		}

		private sealed class ConstructorForwarder : MethodWrapper
		{
			private readonly TypeBuilder typeBuilder;
			private readonly MethodWrapper ctor;
			private ConstructorBuilder constructorBuilder;

			internal ConstructorForwarder(TypeBuilder typeBuilder, MethodWrapper ctor)
				: base(ctor.DeclaringType, ctor.Name, ctor.Signature, null, null, null, ctor.Modifiers, MemberFlags.None)
			{
				this.typeBuilder = typeBuilder;
				this.ctor = ctor;
			}

			protected override void DoLinkMethod()
			{
				ctor.Link();
				Type[] parameters = ctor.GetParametersForDefineMethod();
				constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.PrivateScope, CallingConventions.Standard, parameters);
				AttributeHelper.HideFromJava(constructorBuilder);
				CodeEmitter ilgen = CodeEmitter.Create(constructorBuilder);
				ilgen.Emit(OpCodes.Ldarg_0);
				for (int i = 1; i <= parameters.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
				}
				ctor.EmitCall(ilgen);
				ilgen.Emit(OpCodes.Ret);
			}

			internal override void EmitCall(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Call, constructorBuilder);
			}
		}

		internal override bool IsGhost
		{
			get
			{
				return classLoader.IsGhost(this);
			}
		}

		internal override bool IsMapUnsafeException
		{
			get
			{
				return classLoader.IsMapUnsafeException(this);
			}
		}

		internal override Type TypeAsBaseType
		{
			get
			{
				return typeBuilderGhostInterface != null ? typeBuilderGhostInterface : base.TypeAsBaseType;
			}
		}

		internal void GetParameterNamesFromXml(string methodName, string methodSig, string[] parameterNames)
		{
			IKVM.Internal.MapXml.Param[] parameters = classLoader.GetXmlMapParameters(Name, methodName, methodSig);
			if(parameters != null)
			{
				for(int i = 0; i < parameters.Length; i++)
				{
					if(parameters[i].Name != null)
					{
						parameterNames[i] = parameters[i].Name;
					}
				}
			}
		}

		internal void AddXmlMapParameterAttributes(MethodBase method, string className, string methodName, string methodSig, ref ParameterBuilder[] pbs)
		{
			IKVM.Internal.MapXml.Param[] parameters = classLoader.GetXmlMapParameters(className, methodName, methodSig);
			if(parameters != null)
			{
				if(pbs == null)
				{
					// let's hope that the parameters array is the right length
					pbs = GetParameterBuilders(method, parameters.Length, null);
				}
				for(int i = 0; i < pbs.Length; i++)
				{
					if(parameters[i].Attributes != null)
					{
						foreach(IKVM.Internal.MapXml.Attribute attr in parameters[i].Attributes)
						{
							AttributeHelper.SetCustomAttribute(classLoader, pbs[i], attr);
						}
					}
				}
			}
		}

		private void AddParameterMetadata(MethodBuilder method, MethodWrapper mw)
		{
			ParameterBuilder[] pbs;
			if((mw.DeclaringType.IsPublic && (mw.IsPublic || mw.IsProtected)) || classLoader.EmitDebugInfo)
			{
				string[] parameterNames = new string[mw.GetParameters().Length];
				GetParameterNamesFromXml(mw.Name, mw.Signature, parameterNames);
				GetParameterNamesFromSig(mw.Signature, parameterNames);
				pbs = GetParameterBuilders(method, parameterNames.Length, parameterNames);
			}
			else
			{
				pbs = GetParameterBuilders(method, mw.GetParameters().Length, null);
			}
			if((mw.Modifiers & Modifiers.VarArgs) != 0 && pbs.Length > 0)
			{
				AttributeHelper.SetParamArrayAttribute(pbs[pbs.Length - 1]);
			}
			AddXmlMapParameterAttributes(method, Name, mw.Name, mw.Signature, ref pbs);
		}

		protected override void AddMapXmlFields(ref FieldWrapper[] fields)
		{
			Dictionary<string, IKVM.Internal.MapXml.Class> mapxml = classLoader.GetMapXmlClasses();
			if(mapxml != null)
			{
				IKVM.Internal.MapXml.Class clazz;
				if(mapxml.TryGetValue(this.Name, out clazz))
				{
					if(clazz.Fields != null)
					{
						foreach(IKVM.Internal.MapXml.Field field in clazz.Fields)
						{
							// are we adding a new field?
							bool found = false;
							foreach(FieldWrapper fw in fields)
							{
								if(fw.Name == field.Name && fw.Signature == field.Sig)
								{
									found = true;
									break;
								}
							}
							if(!found)
							{
								FieldWrapper[] newFields = new FieldWrapper[fields.Length + 1];
								Array.Copy(fields, newFields, fields.Length);
								fields = newFields;
								fields[fields.Length - 1] = FieldWrapper.Create(this, null, null, field.Name, field.Sig, new ExModifiers((Modifiers)field.Modifiers, false));
							}
						}
					}
				}				
			}
		}

		protected override bool EmitMapXmlMethodBody(CodeEmitter ilgen, ClassFile f, ClassFile.Method m)
		{
			Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList> mapxml = classLoader.GetMapXmlMethodBodies();
			if(mapxml != null)
			{
				IKVM.Internal.MapXml.InstructionList opcodes;
				if(mapxml.TryGetValue(new MethodKey(f.Name, m.Name, m.Signature), out opcodes))
				{
					opcodes.Emit(classLoader, ilgen);
					return true;
				}
			}
			return false;
		}

		private void PublishAttributes(TypeBuilder typeBuilder, IKVM.Internal.MapXml.Class clazz)
		{
			foreach(IKVM.Internal.MapXml.Attribute attr in clazz.Attributes)
			{
				AttributeHelper.SetCustomAttribute(classLoader, typeBuilder, attr);
			}
		}

		private static bool CheckPropertyArgs(Type[] args1, Type[] args2)
		{
			if(args1.Length == args2.Length)
			{
				for(int i = 0; i < args1.Length; i++)
				{
					if(args1[i] != args2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private static MethodAttributes GetPropertyMethodAttributes(MethodWrapper mw, bool final)
		{
			MethodAttributes attribs = MethodAttributes.HideBySig;
			if(mw.IsStatic)
			{
				attribs |= MethodAttributes.Static;
			}
			else
			{
				// NOTE in order for IntelliSense to consider the property a "real" property,
				// the getter and setter methods need to have substantially the same method attributes,
				// so we may need to look at our peer to determine whether we should be final
				// or not (and vice versa).
				attribs |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
				if(final)
				{
					attribs |= MethodAttributes.Final;
				}
			}
			// TODO what happens if accessibility doesn't match our peer?
			if(mw.IsPublic)
			{
				attribs |= MethodAttributes.Public;
			}
			else if(mw.IsProtected)
			{
				attribs |= MethodAttributes.FamORAssem;
			}
			else if(mw.IsPrivate)
			{
				attribs |= MethodAttributes.Private;
			}
			else
			{
				attribs |= MethodAttributes.Assembly;
			}
			return attribs;
		}

		private void PublishProperties(TypeBuilder typeBuilder, IKVM.Internal.MapXml.Class clazz)
		{
			foreach(IKVM.Internal.MapXml.Property prop in clazz.Properties)
			{
				TypeWrapper typeWrapper = ClassFile.RetTypeWrapperFromSig(GetClassLoader(), prop.Sig);
				TypeWrapper[] propargs = ClassFile.ArgTypeWrapperListFromSig(GetClassLoader(), prop.Sig);
				Type[] indexer = new Type[propargs.Length];
				for(int i = 0; i < propargs.Length; i++)
				{
					indexer[i] = propargs[i].TypeAsSignatureType;
				}
				PropertyBuilder propbuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.None, typeWrapper.TypeAsSignatureType, indexer);
				AttributeHelper.HideFromJava(propbuilder);
				if(prop.Attributes != null)
				{
					foreach(IKVM.Internal.MapXml.Attribute attr in prop.Attributes)
					{
						AttributeHelper.SetCustomAttribute(classLoader, propbuilder, attr);
					}
				}
				MethodWrapper getter = null;
				MethodWrapper setter = null;
				if(prop.getter != null)
				{
					getter = GetMethodWrapper(prop.getter.Name, prop.getter.Sig, true);
					if(getter == null)
					{
						Console.Error.WriteLine("Warning: getter not found for {0}::{1}", clazz.Name, prop.Name);
					}
				}
				if(prop.setter != null)
				{
					setter = GetMethodWrapper(prop.setter.Name, prop.setter.Sig, true);
					if(setter == null)
					{
						Console.Error.WriteLine("Warning: setter not found for {0}::{1}", clazz.Name, prop.Name);
					}
				}
				bool final = (getter != null && getter.IsFinal) || (setter != null && setter.IsFinal);
				if(getter != null)
				{
					MethodWrapper mw = getter;
					if(!CheckPropertyArgs(mw.GetParametersForDefineMethod(), indexer) || mw.ReturnType != typeWrapper)
					{
						Console.Error.WriteLine("Warning: ignoring invalid property getter for {0}::{1}", clazz.Name, prop.Name);
					}
					else
					{
						MethodBuilder mb = mw.GetMethod() as MethodBuilder;
						if(mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
						{
							mb = typeBuilder.DefineMethod(GenerateUniqueMethodName("get_" + prop.Name, mw), GetPropertyMethodAttributes(mw, final), typeWrapper.TypeAsSignatureType, indexer);
							AttributeHelper.HideFromJava(mb);
							CodeEmitter ilgen = CodeEmitter.Create(mb);
							if(mw.IsStatic)
							{
								for(int i = 0; i < indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								mw.EmitCall(ilgen);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i < indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								mw.EmitCallvirt(ilgen);
							}
							ilgen.Emit(OpCodes.Ret);
						}
						propbuilder.SetGetMethod(mb);
					}
				}
				if(setter != null)
				{
					MethodWrapper mw = setter;
					Type[] args = new Type[indexer.Length + 1];
					indexer.CopyTo(args, 0);
					args[args.Length - 1] = typeWrapper.TypeAsSignatureType;
					if(!CheckPropertyArgs(args, mw.GetParametersForDefineMethod()))
					{
						Console.Error.WriteLine("Warning: ignoring invalid property setter for {0}::{1}", clazz.Name, prop.Name);
					}
					else
					{
						MethodBuilder mb = mw.GetMethod() as MethodBuilder;
						if(mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
						{
							mb = typeBuilder.DefineMethod(GenerateUniqueMethodName("set_" + prop.Name, mw), GetPropertyMethodAttributes(mw, final), mw.ReturnTypeForDefineMethod, args);
							AttributeHelper.HideFromJava(mb);
							CodeEmitter ilgen = CodeEmitter.Create(mb);
							if(mw.IsStatic)
							{
								for(int i = 0; i <= indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								mw.EmitCall(ilgen);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i <= indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								mw.EmitCallvirt(ilgen);
							}
							ilgen.Emit(OpCodes.Ret);
						}
						propbuilder.SetSetMethod(mb);
					}
				}
			}
		}

		protected override bool IsPInvokeMethod(ClassFile.Method m)
		{
			Dictionary<string, IKVM.Internal.MapXml.Class> mapxml = classLoader.GetMapXmlClasses();
			if(mapxml != null)
			{
				IKVM.Internal.MapXml.Class clazz;
				if(mapxml.TryGetValue(this.Name, out clazz) && clazz.Methods != null)
				{
					foreach(IKVM.Internal.MapXml.Method method in clazz.Methods)
					{
						if(method.Name == m.Name && method.Sig == m.Signature)
						{
							if(method.Attributes != null)
							{
								foreach(IKVM.Internal.MapXml.Attribute attr in method.Attributes)
								{
									if(JVM.GetType(attr.Type, false) == JVM.Import(typeof(System.Runtime.InteropServices.DllImportAttribute)))
									{
										return true;
									}
								}
							}
							break;
						}
					}
				}
			}
			return base.IsPInvokeMethod(m);
		}

		private static void MapModifiers(MapXml.MapModifiers mapmods, bool isConstructor, out bool setmodifiers, ref MethodAttributes attribs)
		{
			setmodifiers = false;
			Modifiers modifiers = (Modifiers)mapmods;
			if((modifiers & Modifiers.Public) != 0)
			{
				attribs |= MethodAttributes.Public;
			}
			else if((modifiers & Modifiers.Protected) != 0)
			{
				attribs |= MethodAttributes.FamORAssem;
			}
			else if((modifiers & Modifiers.Private) != 0)
			{
				attribs |= MethodAttributes.Private;
			}
			else
			{
				attribs |= MethodAttributes.Assembly;
			}
			if((modifiers & Modifiers.Static) != 0)
			{
				attribs |= MethodAttributes.Static;
				if((modifiers & Modifiers.Final) != 0)
				{
					setmodifiers = true;
				}
			}
			else if(!isConstructor)
			{
				// NOTE we're abusing the MethodAttributes.NewSlot and Modifiers.Final combination to mean non-virtual
				if((modifiers & Modifiers.Final) != 0 && (attribs & MethodAttributes.NewSlot) != 0 && (attribs & MethodAttributes.Virtual) == 0)
				{
					// remove NewSlot, because it doesn't make sense on a non-virtual method
					attribs &= ~MethodAttributes.NewSlot;
				}
				else
				{
					attribs |= MethodAttributes.Virtual;
					if((modifiers & Modifiers.Final) != 0)
					{
						attribs |= MethodAttributes.Final;
					}
					else if((modifiers & Modifiers.Abstract) != 0)
					{
						attribs |= MethodAttributes.Abstract;
					}
				}
			}
			if((modifiers & Modifiers.Synchronized) != 0)
			{
				throw new NotImplementedException();
			}
		}

		private void MapSignature(string sig, out Type returnType, out Type[] parameterTypes)
		{
			returnType = ClassFile.RetTypeWrapperFromSig(GetClassLoader(), sig).TypeAsSignatureType;
			TypeWrapper[] parameterTypeWrappers = ClassFile.ArgTypeWrapperListFromSig(GetClassLoader(), sig);
			parameterTypes = new Type[parameterTypeWrappers.Length];
			for(int i = 0; i < parameterTypeWrappers.Length; i++)
			{
				parameterTypes[i] = parameterTypeWrappers[i].TypeAsSignatureType;
			}
		}

		protected override void EmitMapXmlMetadata(TypeBuilder typeBuilder, ClassFile classFile, FieldWrapper[] fields, MethodWrapper[] methods)
		{
			Dictionary<string, IKVM.Internal.MapXml.Class> mapxml = classLoader.GetMapXmlClasses();
			if(mapxml != null)
			{
				IKVM.Internal.MapXml.Class clazz;
				if(mapxml.TryGetValue(classFile.Name, out clazz))
				{
					if(clazz.Attributes != null)
					{
						PublishAttributes(typeBuilder, clazz);
					}
					if(clazz.Properties != null)
					{
						PublishProperties(typeBuilder, clazz);
					}
					if(clazz.Fields != null)
					{
						foreach(IKVM.Internal.MapXml.Field field in clazz.Fields)
						{
							if(field.Attributes != null)
							{
								foreach(FieldWrapper fw in fields)
								{
									if(fw.Name == field.Name && fw.Signature == field.Sig)
									{
										FieldBuilder fb = fw.GetField() as FieldBuilder;
										if(fb != null)
										{
											foreach(IKVM.Internal.MapXml.Attribute attr in field.Attributes)
											{
												AttributeHelper.SetCustomAttribute(classLoader, fb, attr);
											}
										}
									}
								}
							}
						}
					}
					if(clazz.Constructors != null)
					{
						// HACK this isn't the right place to do this, but for now it suffices
						foreach(IKVM.Internal.MapXml.Constructor constructor in clazz.Constructors)
						{
							// are we adding a new constructor?
							if(GetMethodWrapper(StringConstants.INIT, constructor.Sig, false) == null)
							{
								if(constructor.body == null)
								{
									Console.Error.WriteLine("Error: Constructor {0}.<init>{1} in xml remap file doesn't have a body.", clazz.Name, constructor.Sig);
									continue;
								}
								bool setmodifiers = false;
								MethodAttributes attribs = 0;
								MapModifiers(constructor.Modifiers, true, out setmodifiers, ref attribs);
								Type returnType;
								Type[] parameterTypes;
								MapSignature(constructor.Sig, out returnType, out parameterTypes);
								ConstructorBuilder cb = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, parameterTypes);
								if(setmodifiers)
								{
									AttributeHelper.SetModifiers(cb, (Modifiers)constructor.Modifiers, false);
								}
								CodeEmitter ilgen = CodeEmitter.Create(cb);
								constructor.body.Emit(classLoader, ilgen);
								if(constructor.Attributes != null)
								{
									foreach(IKVM.Internal.MapXml.Attribute attr in constructor.Attributes)
									{
										AttributeHelper.SetCustomAttribute(classLoader, cb, attr);
									}
								}
							}
						}
						foreach(IKVM.Internal.MapXml.Constructor constructor in clazz.Constructors)
						{
							if(constructor.Attributes != null)
							{
								foreach(MethodWrapper mw in methods)
								{
									if(mw.Name == "<init>" && mw.Signature == constructor.Sig)
									{
										ConstructorBuilder mb = mw.GetMethod() as ConstructorBuilder;
										if(mb != null)
										{
											foreach(IKVM.Internal.MapXml.Attribute attr in constructor.Attributes)
											{
												AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
											}
										}
									}
								}
							}
						}
					}
					if(clazz.Methods != null)
					{
						// HACK this isn't the right place to do this, but for now it suffices
						foreach(IKVM.Internal.MapXml.Method method in clazz.Methods)
						{
							// are we adding a new method?
							if(GetMethodWrapper(method.Name, method.Sig, false) == null)
							{
								if(method.body == null)
								{
									Console.Error.WriteLine("Error: Method {0}.{1}{2} in xml remap file doesn't have a body.", clazz.Name, method.Name, method.Sig);
									continue;
								}
								bool setmodifiers = false;
								MethodAttributes attribs = method.MethodAttributes;
								MapModifiers(method.Modifiers, false, out setmodifiers, ref attribs);
								Type returnType;
								Type[] parameterTypes;
								MapSignature(method.Sig, out returnType, out parameterTypes);
								MethodBuilder mb = typeBuilder.DefineMethod(method.Name, attribs, returnType, parameterTypes);
								if(setmodifiers)
								{
									AttributeHelper.SetModifiers(mb, (Modifiers)method.Modifiers, false);
								}
								if(method.@override != null)
								{
									MethodWrapper mw = GetClassLoader().LoadClassByDottedName(method.@override.Class).GetMethodWrapper(method.@override.Name, method.Sig, true);
									mw.Link();
									typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
								}
								CodeEmitter ilgen = CodeEmitter.Create(mb);
								method.body.Emit(classLoader, ilgen);
								if(method.Attributes != null)
								{
									foreach(IKVM.Internal.MapXml.Attribute attr in method.Attributes)
									{
										AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
									}
								}
							}
						}
						foreach(IKVM.Internal.MapXml.Method method in clazz.Methods)
						{
							if(method.Attributes != null)
							{
								foreach(MethodWrapper mw in methods)
								{
									if(mw.Name == method.Name && mw.Signature == method.Sig)
									{
										MethodBuilder mb = mw.GetMethod() as MethodBuilder;
										if(mb != null)
										{
											foreach(IKVM.Internal.MapXml.Attribute attr in method.Attributes)
											{
												AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
											}
										}
									}
								}
							}
						}
					}
					if(clazz.Interfaces != null)
					{
						foreach(IKVM.Internal.MapXml.Interface iface in clazz.Interfaces)
						{
							TypeWrapper tw = GetClassLoader().LoadClassByDottedName(iface.Name);
							// NOTE since this interface won't be part of the list in the ImplementAttribute,
							// it won't be visible from Java that the type implements this interface.
							typeBuilder.AddInterfaceImplementation(tw.TypeAsBaseType);
							if(iface.Methods != null)
							{
								foreach(IKVM.Internal.MapXml.Method m in iface.Methods)
								{
									MethodWrapper mw = tw.GetMethodWrapper(m.Name, m.Sig, false);
									if(mw == null)
									{
										throw new InvalidOperationException("Method " + m.Name + m.Sig + " not found in interface " + tw.Name);
									}
									mw.Link();
									MethodBuilder mb = typeBuilder.DefineMethod(tw.Name + "/" + m.Name, MethodAttributes.Private | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.CheckAccessOnOverride, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
									AttributeHelper.HideFromJava(mb);
									typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
									m.Emit(classLoader, CodeEmitter.Create(mb));
								}
							}
						}
					}
				}
			}
		}

		protected override MethodBuilder DefineGhostMethod(string name, MethodAttributes attribs, MethodWrapper mw)
		{
			if(typeBuilderGhostInterface != null)
			{
				return typeBuilderGhostInterface.DefineMethod(name, attribs, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
			}
			return null;
		}

		protected override void FinishGhost(TypeBuilder typeBuilder, MethodWrapper[] methods)
		{
			if(typeBuilderGhostInterface != null)
			{
				// TODO consider adding methods from base interface and java.lang.Object as well
				for(int i = 0; i < methods.Length; i++)
				{
					// skip <clinit>
					if(!methods[i].IsStatic)
					{
						TypeWrapper[] args = methods[i].GetParameters();
						MethodBuilder stub = typeBuilder.DefineMethod(methods[i].Name, MethodAttributes.Public, methods[i].ReturnTypeForDefineMethod, methods[i].GetParametersForDefineMethod());
						AddParameterMetadata(stub, methods[i]);
						AttributeHelper.SetModifiers(stub, methods[i].Modifiers, methods[i].IsInternal);
						CodeEmitter ilgen = CodeEmitter.Create(stub);
						CodeEmitterLabel end = ilgen.DefineLabel();
						TypeWrapper[] implementers = classLoader.GetGhostImplementers(this);
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Ldfld, ghostRefField);
						ilgen.Emit(OpCodes.Dup);
						ilgen.Emit(OpCodes.Isinst, typeBuilderGhostInterface);
						CodeEmitterLabel label = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brfalse_S, label);
						ilgen.Emit(OpCodes.Castclass, typeBuilderGhostInterface);
						for(int k = 0; k < args.Length; k++)
						{
							ilgen.Emit(OpCodes.Ldarg_S, (byte)(k + 1));
						}
						ilgen.Emit(OpCodes.Callvirt, (MethodInfo)methods[i].GetMethod());
						ilgen.Emit(OpCodes.Br, end);
						ilgen.MarkLabel(label);
						for(int j = 0; j < implementers.Length; j++)
						{
							ilgen.Emit(OpCodes.Dup);
							ilgen.Emit(OpCodes.Isinst, implementers[j].TypeAsTBD);
							label = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Brfalse_S, label);
							ilgen.Emit(OpCodes.Castclass, implementers[j].TypeAsTBD);
							for(int k = 0; k < args.Length; k++)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)(k + 1));
							}
							MethodWrapper mw = implementers[j].GetMethodWrapper(methods[i].Name, methods[i].Signature, true);
							mw.EmitCallvirt(ilgen);
							ilgen.Emit(OpCodes.Br, end);
							ilgen.MarkLabel(label);
						}
						// we need to do a null check (null fails all the isinst checks)
						ilgen.EmitNullCheck();
						ilgen.EmitThrow("java.lang.IncompatibleClassChangeError", Name);
						ilgen.MarkLabel(end);
						ilgen.Emit(OpCodes.Ret);
					}
				}
				// HACK create a scope to enable reuse of "implementers" name
				if(true)
				{
					MethodBuilder mb;
					CodeEmitter ilgen;
					LocalBuilder local;
					// add implicit conversions for all the ghost implementers
					TypeWrapper[] implementers = classLoader.GetGhostImplementers(this);
					for(int i = 0; i < implementers.Length; i++)
					{
						mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, TypeAsSignatureType, new Type[] { implementers[i].TypeAsSignatureType });
						ilgen = CodeEmitter.Create(mb);
						local = ilgen.DeclareLocal(TypeAsSignatureType);
						ilgen.Emit(OpCodes.Ldloca, local);
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Stfld, ghostRefField);
						ilgen.Emit(OpCodes.Ldloca, local);
						ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);			
						ilgen.Emit(OpCodes.Ret);
					}
					// Implement the "IsInstance" method
					mb = ghostIsInstanceMethod;
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					CodeEmitterLabel end = ilgen.DefineLabel();
					for(int i = 0; i < implementers.Length; i++)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
						CodeEmitterLabel label = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brfalse_S, label);
						ilgen.Emit(OpCodes.Ldc_I4_1);
						ilgen.Emit(OpCodes.Br, end);
						ilgen.MarkLabel(label);
					}
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, typeBuilderGhostInterface);
					ilgen.Emit(OpCodes.Ldnull);
					ilgen.Emit(OpCodes.Cgt_Un);
					ilgen.MarkLabel(end);
					ilgen.Emit(OpCodes.Ret);
					// Implement the "IsInstanceArray" method
					mb = ghostIsInstanceArrayMethod;
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					LocalBuilder localType = ilgen.DeclareLocal(Types.Type);
					LocalBuilder localRank = ilgen.DeclareLocal(Types.Int32);
					ilgen.Emit(OpCodes.Ldarg_0);
					CodeEmitterLabel skip = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Brtrue_S, skip);
					ilgen.Emit(OpCodes.Ldc_I4_0);
					ilgen.Emit(OpCodes.Ret);
					ilgen.MarkLabel(skip);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Call, Compiler.getTypeMethod);
					ilgen.Emit(OpCodes.Stloc, localType);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Stloc, localRank);
					skip = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Br_S, skip);
					CodeEmitterLabel iter = ilgen.DefineLabel();
					ilgen.MarkLabel(iter);
					ilgen.Emit(OpCodes.Ldloc, localType);
					ilgen.Emit(OpCodes.Callvirt, Types.Type.GetMethod("GetElementType"));
					ilgen.Emit(OpCodes.Stloc, localType);
					ilgen.Emit(OpCodes.Ldloc, localRank);
					ilgen.Emit(OpCodes.Ldc_I4_1);
					ilgen.Emit(OpCodes.Sub);
					ilgen.Emit(OpCodes.Stloc, localRank);
					ilgen.Emit(OpCodes.Ldloc, localRank);
					CodeEmitterLabel typecheck = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Brfalse_S, typecheck);
					ilgen.MarkLabel(skip);
					ilgen.Emit(OpCodes.Ldloc, localType);
					ilgen.Emit(OpCodes.Callvirt, Types.Type.GetMethod("get_IsArray"));
					ilgen.Emit(OpCodes.Brtrue_S, iter);
					ilgen.Emit(OpCodes.Ldc_I4_0);
					ilgen.Emit(OpCodes.Ret);
					ilgen.MarkLabel(typecheck);
					for(int i = 0; i < implementers.Length; i++)
					{
						ilgen.Emit(OpCodes.Ldtoken, implementers[i].TypeAsTBD);
						ilgen.Emit(OpCodes.Call, Types.Type.GetMethod("GetTypeFromHandle"));
						ilgen.Emit(OpCodes.Ldloc, localType);
						ilgen.Emit(OpCodes.Callvirt, Types.Type.GetMethod("IsAssignableFrom"));
						CodeEmitterLabel label = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brfalse_S, label);
						ilgen.Emit(OpCodes.Ldc_I4_1);
						ilgen.Emit(OpCodes.Ret);
						ilgen.MarkLabel(label);
					}
					ilgen.Emit(OpCodes.Ldtoken, typeBuilderGhostInterface);
					ilgen.Emit(OpCodes.Call, Types.Type.GetMethod("GetTypeFromHandle"));
					ilgen.Emit(OpCodes.Ldloc, localType);
					ilgen.Emit(OpCodes.Callvirt, Types.Type.GetMethod("IsAssignableFrom"));
					skip = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Brfalse_S, skip);
					ilgen.Emit(OpCodes.Ldc_I4_1);
					ilgen.Emit(OpCodes.Ret);
					ilgen.MarkLabel(skip);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldtoken, typeBuilder);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Call, StaticCompiler.GetType("IKVM.Runtime.GhostTag").GetMethod("IsGhostArrayInstance", BindingFlags.NonPublic | BindingFlags.Static));
					ilgen.Emit(OpCodes.Ret);
						
					// Implement the "Cast" method
					mb = ghostCastMethod;
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					end = ilgen.DefineLabel();
					for(int i = 0; i < implementers.Length; i++)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
						ilgen.Emit(OpCodes.Brtrue, end);
					}
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Castclass, typeBuilderGhostInterface);
					ilgen.Emit(OpCodes.Pop);
					ilgen.MarkLabel(end);
					local = ilgen.DeclareLocal(TypeAsSignatureType);
					ilgen.Emit(OpCodes.Ldloca, local);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Stfld, ghostRefField);
					ilgen.Emit(OpCodes.Ldloca, local);
					ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);	
					ilgen.Emit(OpCodes.Ret);
					// Add "ToObject" methods
					mb = typeBuilder.DefineMethod("ToObject", MethodAttributes.HideBySig | MethodAttributes.Public, Types.Object, Type.EmptyTypes);
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ret);

					// Implement the "CastArray" method
					// NOTE unlike "Cast" this doesn't return anything, it just throws a ClassCastException if the
					// cast is unsuccessful. Also, because of the complexity of this test, we call IsInstanceArray
					// instead of reimplementing the check here.
					mb = ghostCastArrayMethod;
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					end = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Brfalse_S, end);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Call, ghostIsInstanceArrayMethod);
					ilgen.Emit(OpCodes.Brtrue_S, end);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldtoken, typeBuilder);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Call, StaticCompiler.GetType("IKVM.Runtime.GhostTag").GetMethod("ThrowClassCastException", BindingFlags.NonPublic | BindingFlags.Static));
					ilgen.MarkLabel(end);
					ilgen.Emit(OpCodes.Ret);

					// Implement the "Equals" method
					mb = typeBuilder.DefineMethod("Equals", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual, Types.Boolean, new Type[] { Types.Object });
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ldarg_1);
					ilgen.Emit(OpCodes.Ceq);
					ilgen.Emit(OpCodes.Ret);

					// Implement the "GetHashCode" method
					mb = typeBuilder.DefineMethod("GetHashCode", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual, Types.Int32, Type.EmptyTypes);
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Callvirt, Types.Object.GetMethod("GetHashCode"));
					ilgen.Emit(OpCodes.Ret);

					// Implement the "op_Equality" method
					mb = typeBuilder.DefineMethod("op_Equality", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, Types.Boolean, new Type[] { typeBuilder, typeBuilder });
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					ilgen.Emit(OpCodes.Ldarga_S, (byte)0);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ldarga_S, (byte)1);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ceq);
					ilgen.Emit(OpCodes.Ret);

					// Implement the "op_Inequality" method
					mb = typeBuilder.DefineMethod("op_Inequality", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, Types.Boolean, new Type[] { typeBuilder, typeBuilder });
					AttributeHelper.HideFromJava(mb);
					ilgen = CodeEmitter.Create(mb);
					ilgen.Emit(OpCodes.Ldarga_S, (byte)0);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ldarga_S, (byte)1);
					ilgen.Emit(OpCodes.Ldfld, ghostRefField);
					ilgen.Emit(OpCodes.Ceq);
					ilgen.Emit(OpCodes.Ldc_I4_0);
					ilgen.Emit(OpCodes.Ceq);
					ilgen.Emit(OpCodes.Ret);
				}
			}
		}

		protected override void FinishGhostStep2()
		{
			if(typeBuilderGhostInterface != null)
			{
				typeBuilderGhostInterface.CreateType();
			}
		}

		protected override TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs)
		{
			typeAttribs &= ~(TypeAttributes.Interface | TypeAttributes.Abstract);
			typeAttribs |= TypeAttributes.Class | TypeAttributes.Sealed;
			TypeBuilder typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs, Types.ValueType);
			AttributeHelper.SetGhostInterface(typeBuilder);
			AttributeHelper.SetModifiers(typeBuilder, Modifiers, IsInternal);
			ghostRefField = typeBuilder.DefineField("__<ref>", Types.Object, FieldAttributes.Public | FieldAttributes.SpecialName);
			typeBuilderGhostInterface = typeBuilder.DefineNestedType("__Interface", TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.NestedPublic);
			AttributeHelper.HideFromJava(typeBuilderGhostInterface);
			ghostIsInstanceMethod = typeBuilder.DefineMethod("IsInstance", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Types.Boolean, new Type[] { Types.Object });
			ghostIsInstanceMethod.DefineParameter(1, ParameterAttributes.None, "obj");
			ghostIsInstanceArrayMethod = typeBuilder.DefineMethod("IsInstanceArray", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Types.Boolean, new Type[] { Types.Object, Types.Int32 });
			ghostIsInstanceArrayMethod.DefineParameter(1, ParameterAttributes.None, "obj");
			ghostIsInstanceArrayMethod.DefineParameter(2, ParameterAttributes.None, "rank");
			ghostCastMethod = typeBuilder.DefineMethod("Cast", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, typeBuilder, new Type[] { Types.Object });
			ghostCastMethod.DefineParameter(1, ParameterAttributes.None, "obj");
			ghostCastArrayMethod = typeBuilder.DefineMethod("CastArray", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Types.Void, new Type[] { Types.Object, Types.Int32 });
			ghostCastArrayMethod.DefineParameter(1, ParameterAttributes.None, "obj");
			ghostCastArrayMethod.DefineParameter(2, ParameterAttributes.None, "rank");
			return typeBuilder;
		}

		internal override FieldInfo GhostRefField
		{
			get
			{
				return ghostRefField;
			}
		}

		internal override void EmitCheckcast(TypeWrapper context, CodeEmitter ilgen)
		{
			if(IsGhost)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Call, ghostCastMethod);
				ilgen.Emit(OpCodes.Pop);
			}
			else if(IsGhostArray)
			{
				ilgen.Emit(OpCodes.Dup);
				TypeWrapper tw = this;
				int rank = 0;
				while(tw.IsArray)
				{
					rank++;
					tw = tw.ElementTypeWrapper;
				}
				ilgen.Emit(OpCodes.Ldc_I4, rank);
				ilgen.Emit(OpCodes.Call, ghostCastArrayMethod);
				ilgen.Emit(OpCodes.Castclass, ArrayTypeWrapper.MakeArrayType(Types.Object, rank));
			}
			else
			{
				base.EmitCheckcast(context, ilgen);
			}
		}

		internal override void EmitInstanceOf(TypeWrapper context, CodeEmitter ilgen)
		{
			if(IsGhost)
			{
				ilgen.Emit(OpCodes.Call, ghostIsInstanceMethod);
			}
			else if(IsGhostArray)
			{
				ilgen.Emit(OpCodes.Call, ghostIsInstanceArrayMethod);
			}
			else
			{
				base.EmitInstanceOf(context, ilgen);
			}
		}

		internal void SetAnnotation(Annotation annotation)
		{
			this.annotation = annotation;
		}

		internal override Annotation Annotation
		{
			get
			{
				return annotation;
			}
		}

		private sealed class ReplacedMethodWrapper : MethodWrapper
		{
			private IKVM.Internal.MapXml.InstructionList code;

			internal ReplacedMethodWrapper(TypeWrapper tw, string name, string sig, IKVM.Internal.MapXml.InstructionList code)
				: base(tw, name, sig, null, null, null, Modifiers.Public, MemberFlags.None)
			{
				this.code = code;
			}

			internal ReplacedMethodWrapper(ClassFile.ConstantPoolItemMI cpi, IKVM.Internal.MapXml.InstructionList code)
				: base(cpi.GetClassType(), cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
			{
				this.code = code;
			}

			protected override void DoLinkMethod()
			{
			}

			private void DoEmit(CodeEmitter ilgen)
			{
				IKVM.Internal.MapXml.CodeGenContext context = new IKVM.Internal.MapXml.CodeGenContext(this.DeclaringType.GetClassLoader());
				// we don't want the line numbers from map.xml, so we have our own emit loop
				for (int i = 0; i < code.invoke.Length; i++)
				{
					code.invoke[i].Generate(context, ilgen);
				}
			}

			internal override void EmitCall(CodeEmitter ilgen)
			{
				DoEmit(ilgen);
			}

			internal override void EmitCallvirt(CodeEmitter ilgen)
			{
				DoEmit(ilgen);
			}

			internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
			{
				DoEmit(ilgen);
			}
		}

		internal override MethodWrapper[] GetReplacedMethodsFor(MethodWrapper mw)
		{
			IKVM.Internal.MapXml.ReplaceMethodCall[] replacedMethods = ((CompilerClassLoader)GetClassLoader()).GetReplacedMethodsFor(mw);
			MethodWrapper[] baseReplacedMethodWrappers = base.GetReplacedMethodsFor(mw);
			if (replacedMethods != null || baseReplacedMethodWrappers != null || this.replacedMethods != null)
			{
				List<MethodWrapper> list = new List<MethodWrapper>();
				if (replacedMethods != null)
				{
					for (int i = 0; i < replacedMethods.Length; i++)
					{
						list.Add(new ReplacedMethodWrapper(GetClassLoader().LoadClassByDottedName(replacedMethods[i].Class), replacedMethods[i].Name, replacedMethods[i].Sig, replacedMethods[i].code));
					}
				}
				if (baseReplacedMethodWrappers != null)
				{
					list.AddRange(baseReplacedMethodWrappers);
				}
				if (this.replacedMethods != null)
				{
					list.AddRange(this.replacedMethods);
				}
				return list.ToArray();
			}
			return null;
		}

		internal override bool IsFastClassLiteralSafe
		{
			get { return true; }
		}
	}
}
