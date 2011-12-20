/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
#if STATIC_COMPILER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;

namespace IKVM.Internal
{
#if STATIC_COMPILER
	abstract class DynamicTypeWrapper : TypeWrapper
#else
	class DynamicTypeWrapper : TypeWrapper
#endif
	{
#if STATIC_COMPILER
		protected readonly CompilerClassLoader classLoader;
#else
		protected readonly ClassLoaderWrapper classLoader;
#endif
		private volatile DynamicImpl impl;
		private TypeWrapper[] interfaces;
		private readonly string sourceFileName;
#if !STATIC_COMPILER
		private byte[][] lineNumberTables;
#endif
		private ConstructorInfo automagicSerializationCtor;

		private static TypeWrapper LoadTypeWrapper(ClassLoaderWrapper classLoader, string name)
		{
			TypeWrapper tw = classLoader.LoadClassByDottedNameFast(name);
			if (tw == null)
			{
				throw new NoClassDefFoundError(name);
			}
			return tw;
		}

#if STATIC_COMPILER
		internal DynamicTypeWrapper(ClassFile f, CompilerClassLoader classLoader)
#else
		internal DynamicTypeWrapper(ClassFile f, ClassLoaderWrapper classLoader)
#endif
			: base(f.Modifiers, f.Name, f.IsInterface ? null : LoadTypeWrapper(classLoader, f.SuperClass))
		{
			Profiler.Count("DynamicTypeWrapper");
			this.classLoader = classLoader;
			this.IsInternal = f.IsInternal;
			this.sourceFileName = f.SourceFileAttribute;
			if (BaseTypeWrapper != null)
			{
				if (!BaseTypeWrapper.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superclass " + BaseTypeWrapper.Name);
				}
#if !STATIC_COMPILER
				if (!BaseTypeWrapper.IsPublic && !ReflectUtil.IsFromAssembly(BaseTypeWrapper.TypeAsBaseType, classLoader.GetTypeWrapperFactory().ModuleBuilder.Assembly))
				{
					// NOTE this can only happen if evil code calls ClassLoader.defineClass() on an assembly class loader (which we allow for compatibility with other slightly less evil code)
					throw new IllegalAccessError("Class " + f.Name + " cannot access its non-public superclass " + BaseTypeWrapper.Name + " from another assembly");
				}
#endif
				if (BaseTypeWrapper.IsFinal)
				{
					throw new VerifyError("Class " + f.Name + " extends final class " + BaseTypeWrapper.Name);
				}
				if (BaseTypeWrapper.IsInterface)
				{
					throw new IncompatibleClassChangeError("Class " + f.Name + " has interface " + BaseTypeWrapper.Name + " as superclass");
				}
				if (BaseTypeWrapper.TypeAsTBD == Types.Delegate)
				{
					throw new VerifyError(BaseTypeWrapper.Name + " cannot be used as a base class");
				}
				// NOTE defining value types, enums is not supported in IKVM v1
				if (BaseTypeWrapper.TypeAsTBD == Types.ValueType || BaseTypeWrapper.TypeAsTBD == Types.Enum)
				{
					throw new VerifyError("Defining value types in Java is not implemented in IKVM v1");
				}
				if (IsDelegate)
				{
					VerifyDelegate(f);
				}
#if CLASSGC
				if (JVM.classUnloading && BaseTypeWrapper.TypeAsBaseType == typeof(ContextBoundObject))
				{
					throw new VerifyError("Extending ContextBoundObject is not supported in dynamic mode with class GC enabled.");
				}
#endif
			}

#if CLASSGC
			if (JVM.classUnloading)
			{
				VerifyRunAndCollect(f);
			}
#endif

			ClassFile.ConstantPoolItemClass[] interfaces = f.Interfaces;
			this.interfaces = new TypeWrapper[interfaces.Length];
			for (int i = 0; i < interfaces.Length; i++)
			{
				TypeWrapper iface = LoadTypeWrapper(classLoader, interfaces[i].Name);
				if (!iface.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superinterface " + iface.Name);
				}
#if !STATIC_COMPILER
				if (!iface.IsPublic && !ReflectUtil.IsFromAssembly(iface.TypeAsBaseType, classLoader.GetTypeWrapperFactory().ModuleBuilder.Assembly))
				{
					string proxyName = DynamicClassLoader.GetProxyHelperName(iface.TypeAsTBD);
					Type proxyType = ReflectUtil.GetAssembly(iface.TypeAsBaseType).GetType(proxyName);
					// FXBUG we need to check if the type returned is actually correct, because .NET 2.0 has a bug that
					// causes it to return typeof(IFoo) for GetType("__<Proxy>+IFoo")
					if (proxyType == null || proxyType.FullName != proxyName)
					{
						// NOTE this happens when you call Proxy.newProxyInstance() on a non-public .NET interface
						// (for ikvmc compiled Java types, ikvmc generates public proxy stubs).
						// NOTE we don't currently check interfaces inherited from other interfaces because mainstream .NET languages
						// don't allow public interfaces extending non-public interfaces.
						throw new IllegalAccessError("Class " + f.Name + " cannot access its non-public superinterface " + iface.Name + " from another assembly");
					}
				}
#endif
				if (!iface.IsInterface)
				{
					throw new IncompatibleClassChangeError("Implementing class");
				}
				this.interfaces[i] = iface;
			}

			impl = new JavaTypeImpl(f, this);
		}

#if CLASSGC
		private static void VerifyRunAndCollect(ClassFile f)
		{
			if (f.Annotations != null)
			{
				foreach (object[] ann in f.Annotations)
				{
					if (ann[1].Equals("Lcli/System/Runtime/InteropServices/ComImportAttribute$Annotation;"))
					{
						throw new VerifyError("ComImportAttribute is not supported in dynamic mode with class GC enabled.");
					}
				}
			}
			foreach (ClassFile.Field field in f.Fields)
			{
				if (field.Annotations != null)
				{
					foreach (object[] ann in field.Annotations)
					{
						if (ann[1].Equals("Lcli/System/ThreadStaticAttribute$Annotation;"))
						{
							throw new VerifyError("ThreadStaticAttribute is not supported in dynamic mode with class GC enabled.");
						}
						if (ann[1].Equals("Lcli/System/ContextStaticAttribute$Annotation;"))
						{
							throw new VerifyError("ContextStaticAttribute is not supported in dynamic mode with class GC enabled.");
						}
					}
				}
			}
			foreach (ClassFile.Method method in f.Methods)
			{
				if (method.Annotations != null)
				{
					foreach (object[] ann in method.Annotations)
					{
						if (ann[1].Equals("Lcli/System/Runtime/InteropServices/DllImportAttribute$Annotation;"))
						{
							throw new VerifyError("DllImportAttribute is not supported in dynamic mode with class GC enabled.");
						}
					}
				}
			}
		}
#endif

		private void VerifyDelegate(ClassFile f)
		{
			if (!f.IsFinal)
			{
				throw new VerifyError("Delegate must be final");
			}
			ClassFile.Method invoke = null;
			ClassFile.Method beginInvoke = null;
			ClassFile.Method endInvoke = null;
			ClassFile.Method constructor = null;
			foreach (ClassFile.Method m in f.Methods)
			{
				if (m.Name == "Invoke")
				{
					if (invoke != null)
					{
						throw new VerifyError("Delegate may only have a single Invoke method");
					}
					invoke = m;
				}
				else if (m.Name == "BeginInvoke")
				{
					if (beginInvoke != null)
					{
						throw new VerifyError("Delegate may only have a single BeginInvoke method");
					}
					beginInvoke = m;
				}
				else if (m.Name == "EndInvoke")
				{
					if (endInvoke != null)
					{
						throw new VerifyError("Delegate may only have a single EndInvoke method");
					}
					endInvoke = m;
				}
				else if (m.Name == "<init>")
				{
					if (constructor != null)
					{
						throw new VerifyError("Delegate may only have a single constructor");
					}
					constructor = m;
				}
				else if (m.IsNative)
				{
					throw new VerifyError("Delegate may not have any native methods besides Invoke, BeginInvoke and EndInvoke");
				}
			}
			if (invoke == null || constructor == null)
			{
				throw new VerifyError("Delegate must have a constructor and an Invoke method");
			}
			if (!invoke.IsPublic || !invoke.IsNative || invoke.IsFinal || invoke.IsStatic)
			{
				throw new VerifyError("Delegate Invoke method must be a public native non-final instance method");
			}
			if ((beginInvoke != null && endInvoke == null) || (beginInvoke == null && endInvoke != null))
			{
				throw new VerifyError("Delegate must have both BeginInvoke and EndInvoke or neither");
			}
			if (!constructor.IsPublic)
			{
				throw new VerifyError("Delegate constructor must be public");
			}
			if (constructor.Instructions.Length < 3
				|| constructor.Instructions[0].NormalizedOpCode != NormalizedByteCode.__aload
				|| constructor.Instructions[0].NormalizedArg1 != 0
				|| constructor.Instructions[1].NormalizedOpCode != NormalizedByteCode.__invokespecial
				|| constructor.Instructions[2].NormalizedOpCode != NormalizedByteCode.__return)
			{
				throw new VerifyError("Delegate constructor must be empty");
			}
			if (f.Fields.Length != 0)
			{
				throw new VerifyError("Delegate may not declare any fields");
			}
			TypeWrapper iface;
#if STATIC_COMPILER
			iface = classLoader.LoadCircularDependencyHack(this, f.Name + DotNetTypeWrapper.DelegateInterfaceSuffix);
#else
			iface = classLoader.LoadClassByDottedNameFast(f.Name + DotNetTypeWrapper.DelegateInterfaceSuffix);
#endif
			DelegateInnerClassCheck(iface != null);
			DelegateInnerClassCheck(iface.IsInterface);
			DelegateInnerClassCheck(iface.IsPublic);
			DelegateInnerClassCheck(iface.GetClassLoader() == classLoader);
			MethodWrapper[] methods = iface.GetMethods();
			DelegateInnerClassCheck(methods.Length == 1 && methods[0].Name == "Invoke");
			if (methods[0].Signature != invoke.Signature)
			{
				throw new VerifyError("Delegate Invoke method signature must be identical to inner interface Invoke method signature");
			}
			if (iface.Interfaces.Length != 0)
			{
				throw new VerifyError("Delegate inner interface may not extend any interfaces");
			}
			if (constructor.Signature != "(" + iface.SigName + ")V")
			{
				throw new VerifyError("Delegate constructor must take a single argument of type inner Method interface");
			}
			if (beginInvoke != null && beginInvoke.Signature != invoke.Signature.Substring(0, invoke.Signature.IndexOf(')')) + "Lcli.System.AsyncCallback;Ljava.lang.Object;)Lcli.System.IAsyncResult;")
			{
				throw new VerifyError("Delegate BeginInvoke method has incorrect signature");
			}
			if (endInvoke != null && endInvoke.Signature != "(Lcli.System.IAsyncResult;)" + invoke.Signature.Substring(invoke.Signature.IndexOf(')') + 1))
			{
				throw new VerifyError("Delegate EndInvoke method has incorrect signature");
			}
		}

		private static void DelegateInnerClassCheck(bool cond)
		{
			if (!cond)
			{
				throw new VerifyError("Delegate must have a public inner interface named Method with a single method named Invoke");
			}
		}

		private bool IsDelegate
		{
			get
			{
				TypeWrapper baseTypeWrapper = BaseTypeWrapper;
				return baseTypeWrapper != null && baseTypeWrapper.TypeAsTBD == Types.MulticastDelegate;
			}
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return classLoader;
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				return impl.ReflectiveModifiers;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				return interfaces;
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				return impl.InnerClasses;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				return impl.DeclaringTypeWrapper;
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return impl.Type;
			}
		}

#if STATIC_COMPILER
		internal override Annotation Annotation
		{
			get
			{
				return impl.Annotation;
			}
		}

		internal override Type EnumType
		{
			get
			{
				return impl.EnumType;
			}
		}
#endif // STATIC_COMPILER

		internal override void Finish()
		{
			// we don't need locking, because Finish is Thread safe
			impl = impl.Finish();
		}

		// NOTE can only be used if the type hasn't been finished yet!
		protected string GenerateUniqueMethodName(string basename, MethodWrapper mw)
		{
			return ((JavaTypeImpl)impl).GenerateUniqueMethodName(basename, mw);
		}

		// NOTE can only be used if the type hasn't been finished yet!
		internal string GenerateUniqueMethodName(string basename, Type returnType, Type[] parameterTypes)
		{
			return ((JavaTypeImpl)impl).GenerateUniqueMethodName(basename, returnType, parameterTypes);
		}

		internal void CreateStep1(out bool hasclinit)
		{
			((JavaTypeImpl)impl).CreateStep1(out hasclinit);
		}

		internal void CreateStep2NoFail(bool hasclinit, string mangledTypeName)
		{
			((JavaTypeImpl)impl).CreateStep2NoFail(hasclinit, mangledTypeName);
		}

		private bool IsSerializable
		{
			get
			{
				return this.IsSubTypeOf(ClassLoaderWrapper.LoadClassCritical("java.io.Serializable"));
			}
		}

		private abstract class DynamicImpl
		{
			internal abstract Type Type { get; }
			internal abstract TypeWrapper[] InnerClasses { get; }
			internal abstract TypeWrapper DeclaringTypeWrapper { get; }
			internal abstract Modifiers ReflectiveModifiers { get; }
#if STATIC_COMPILER
			internal abstract Annotation Annotation { get; }
			internal abstract Type EnumType { get; }
#endif
			internal abstract DynamicImpl Finish();
			internal abstract MethodBase LinkMethod(MethodWrapper mw);
			internal abstract FieldInfo LinkField(FieldWrapper fw);
			internal abstract void EmitRunClassConstructor(CodeEmitter ilgen);
			internal abstract string GetGenericSignature();
			internal abstract string[] GetEnclosingMethod();
			internal abstract string GetGenericMethodSignature(int index);
			internal abstract string GetGenericFieldSignature(int index);
			internal abstract object[] GetDeclaredAnnotations();
			internal abstract object GetMethodDefaultValue(int index);
			internal abstract object[] GetMethodAnnotations(int index);
			internal abstract object[][] GetParameterAnnotations(int index);
			internal abstract object[] GetFieldAnnotations(int index);
			internal abstract MethodInfo GetFinalizeMethod();
		}

		private sealed class JavaTypeImpl : DynamicImpl
		{
			private readonly ClassFile classFile;
			private readonly DynamicTypeWrapper wrapper;
			private TypeBuilder typeBuilder;
			private MethodWrapper[] methods;
			private MethodWrapper[] baseMethods;
			private FieldWrapper[] fields;
			private FinishedTypeImpl finishedType;
			private bool finishInProgress;
			private Dictionary<string, string> memberclashtable;
			private MethodBuilder clinitMethod;
			private MethodBuilder finalizeMethod;
#if STATIC_COMPILER
			private DynamicTypeWrapper outerClassWrapper;
			private AnnotationBuilder annotationBuilder;
			private TypeBuilder enumBuilder;
#endif

			internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper)
			{
				Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
				this.classFile = f;
				this.wrapper = wrapper;
			}

			internal void CreateStep1(out bool hasclinit)
			{
				// process all methods
				hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
				methods = new MethodWrapper[classFile.Methods.Length];
				baseMethods = new MethodWrapper[classFile.Methods.Length];
				for (int i = 0; i < methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					if (m.IsClassInitializer)
					{
#if STATIC_COMPILER
						bool noop;
						if (IsSideEffectFreeStaticInitializerOrNoop(m, out noop))
						{
							// because we cannot affect serialVersionUID computation (which is the only way the presence of a <clinit> can surface)
							// we cannot do this optimization if the class is serializable but doesn't have a serialVersionUID
							if (noop && (!wrapper.IsSerializable || classFile.HasSerialVersionUID))
							{
								methods[i] = new DummyMethodWrapper(wrapper);
								continue;
							}
						}
						else
						{
							hasclinit = true;
						}
#else
						hasclinit = true;
#endif
					}
					MemberFlags flags = MemberFlags.None;
					if (m.IsInternal)
					{
						flags |= MemberFlags.InternalAccess;
					}
					// we only support HasCallerID instance methods on final types, because we don't support interface stubs with CallerID
					if (m.HasCallerIDAnnotation
						&& (m.IsStatic || classFile.IsFinal)
						&& CoreClasses.java.lang.Object.Wrapper.InternalsVisibleTo(wrapper))
					{
						flags |= MemberFlags.CallerID;
					}
					if (wrapper.IsGhost)
					{
						methods[i] = new MethodWrapper.GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
					}
					else if (ReferenceEquals(m.Name, StringConstants.INIT) && wrapper.IsDelegate)
					{
						methods[i] = new DelegateConstructorMethodWrapper(wrapper, m);
					}
					else if (ReferenceEquals(m.Name, StringConstants.INIT) || m.IsClassInitializer)
					{
						methods[i] = new SmartConstructorMethodWrapper(wrapper, m.Name, m.Signature, null, null, m.Modifiers, flags);
					}
					else
					{
						if (!classFile.IsInterface && !m.IsStatic && !m.IsPrivate)
						{
							bool explicitOverride = false;
							baseMethods[i] = FindBaseMethod(m.Name, m.Signature, out explicitOverride);
							if (explicitOverride)
							{
								flags |= MemberFlags.ExplicitOverride;
							}
						}
						methods[i] = new SmartCallMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags, SimpleOpCode.Call, SimpleOpCode.Callvirt);
					}
				}
				wrapper.HasStaticInitializer = hasclinit;
				if (!wrapper.IsInterface || wrapper.IsPublic)
				{
					List<MethodWrapper> methodsArray = null;
					List<MethodWrapper> baseMethodsArray = null;
					if (wrapper.IsAbstract)
					{
						methodsArray = new List<MethodWrapper>(methods);
						baseMethodsArray = new List<MethodWrapper>(baseMethods);
						AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
					}
#if STATIC_COMPILER || NET_4_0
					if (!wrapper.IsInterface && wrapper.IsPublic)
					{
						TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
						while (baseTypeWrapper != null && !baseTypeWrapper.IsPublic)
						{
							if (methodsArray == null)
							{
								methodsArray = new List<MethodWrapper>(methods);
								baseMethodsArray = new List<MethodWrapper>(baseMethods);
							}
							AddAccessStubMethods(methodsArray, baseMethodsArray, baseTypeWrapper);
							baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
						}
					}
#endif
					if (methodsArray != null)
					{
						this.methods = methodsArray.ToArray();
						this.baseMethods = baseMethodsArray.ToArray();
					}
				}
				wrapper.SetMethods(methods);

				fields = new FieldWrapper[classFile.Fields.Length];
				for (int i = 0; i < fields.Length; i++)
				{
					ClassFile.Field fld = classFile.Fields[i];
					if (fld.IsStatic && fld.IsFinal && fld.ConstantValue != null)
					{
						TypeWrapper fieldType = null;
#if !STATIC_COMPILER
						fieldType = ClassLoaderWrapper.GetBootstrapClassLoader().FieldTypeWrapperFromSig(fld.Signature);
#endif
						fields[i] = new ConstantFieldWrapper(wrapper, fieldType, fld.Name, fld.Signature, fld.Modifiers, null, fld.ConstantValue, MemberFlags.None);
					}
					else if (fld.IsProperty)
					{
						fields[i] = new DynamicPropertyFieldWrapper(wrapper, fld);
					}
					else
					{
						fields[i] = FieldWrapper.Create(wrapper, null, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal));
					}
				}
#if STATIC_COMPILER
				((AotTypeWrapper)wrapper).AddMapXmlFields(ref fields);
#endif
				wrapper.SetFields(fields);
			}

			internal void CreateStep2NoFail(bool hasclinit, string mangledTypeName)
			{
				// this method is not allowed to throw exceptions (if it does, the runtime will abort)
				ClassFile f = classFile;
				try
				{
					TypeAttributes typeAttribs = 0;
					if (f.IsAbstract)
					{
						typeAttribs |= TypeAttributes.Abstract;
					}
					if (f.IsFinal)
					{
						typeAttribs |= TypeAttributes.Sealed;
					}
					if (!hasclinit)
					{
						typeAttribs |= TypeAttributes.BeforeFieldInit;
					}
#if STATIC_COMPILER
					bool cantNest = false;
					bool setModifiers = false;
					TypeBuilder outer = null;
					// we only compile inner classes as nested types in the static compiler, because it has a higher cost
					// and doesn't buy us anything in dynamic mode (and if fact, due to an FXBUG it would make handling
					// the TypeResolve event very hard)
					ClassFile.InnerClass outerClass = getOuterClass();
					if (outerClass.outerClass != 0)
					{
						string outerClassName = classFile.GetConstantPoolClass(outerClass.outerClass);
						if (!CheckInnerOuterNames(f.Name, outerClassName))
						{
							Tracer.Warning(Tracer.Compiler, "Incorrect InnerClasses attribute on {0}", f.Name);
						}
						else
						{
							try
							{
								outerClassWrapper = wrapper.GetClassLoader().LoadCircularDependencyHack(wrapper, outerClassName) as DynamicTypeWrapper;
							}
							catch (RetargetableJavaException x)
							{
								Tracer.Warning(Tracer.Compiler, "Unable to load outer class {0} for inner class {1} ({2}: {3})", outerClassName, f.Name, x.GetType().Name, x.Message);
							}
							if (outerClassWrapper != null)
							{
								// make sure the relationship is reciprocal (otherwise we run the risk of
								// baking the outer type before the inner type) and that the inner and outer
								// class live in the same class loader (when doing a multi target compilation,
								// it is possible to split the two classes acros assemblies)
								if (outerClassWrapper.impl is JavaTypeImpl && outerClassWrapper.GetClassLoader() == wrapper.GetClassLoader())
								{
									ClassFile outerClassFile = ((JavaTypeImpl)outerClassWrapper.impl).classFile;
									ClassFile.InnerClass[] outerInnerClasses = outerClassFile.InnerClasses;
									if (outerInnerClasses == null)
									{
										outerClassWrapper = null;
									}
									else
									{
										bool ok = false;
										for (int i = 0; i < outerInnerClasses.Length; i++)
										{
											if (outerInnerClasses[i].outerClass != 0
												&& outerClassFile.GetConstantPoolClass(outerInnerClasses[i].outerClass) == outerClassFile.Name
												&& outerInnerClasses[i].innerClass != 0
												&& outerClassFile.GetConstantPoolClass(outerInnerClasses[i].innerClass) == f.Name)
											{
												ok = true;
												break;
											}
										}
										if (!ok)
										{
											outerClassWrapper = null;
										}
									}
								}
								else
								{
									outerClassWrapper = null;
								}
								if (outerClassWrapper != null)
								{
									outer = outerClassWrapper.TypeAsBuilder;
								}
								else
								{
									Tracer.Warning(Tracer.Compiler, "Non-reciprocal inner class {0}", f.Name);
								}
							}
						}
					}
					if (f.IsPublic)
					{
						if (outer != null)
						{
							if (outerClassWrapper.IsPublic)
							{
								typeAttribs |= TypeAttributes.NestedPublic;
							}
							else
							{
								// We're a public type nested inside a non-public type, this means that we can't compile this type as a nested type,
								// because that would mean it wouldn't be visible outside the assembly.
								cantNest = true;
								typeAttribs |= TypeAttributes.Public;
							}
						}
						else
						{
							typeAttribs |= TypeAttributes.Public;
						}
					}
					else if (outer != null)
					{
						typeAttribs |= TypeAttributes.NestedAssembly;
					}
#else // STATIC_COMPILER
					if (f.IsPublic)
					{
						typeAttribs |= TypeAttributes.Public;
					}
#endif // STATIC_COMPILER
					if (f.IsInterface)
					{
						typeAttribs |= TypeAttributes.Interface | TypeAttributes.Abstract;
#if STATIC_COMPILER
						if (outer != null && !cantNest)
						{
							if (wrapper.IsGhost)
							{
								// TODO this is low priority, since the current Java class library doesn't define any ghost interfaces
								// as inner classes
								throw new NotImplementedException();
							}
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs);
						}
						else
						{
							if (wrapper.IsGhost)
							{
								typeBuilder = wrapper.DefineGhostType(mangledTypeName, typeAttribs);
							}
							else
							{
								typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
							}
						}
#else // STATIC_COMPILER
						typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
#endif // STATIC_COMPILER
					}
					else
					{
						typeAttribs |= TypeAttributes.Class;
#if STATIC_COMPILER
						if (f.IsEffectivelyFinal)
						{
							if (outer == null)
							{
								setModifiers = true;
							}
							else
							{
								// we don't need a ModifiersAttribute, because the InnerClassAttribute already records
								// the modifiers
							}
							typeAttribs |= TypeAttributes.Sealed;
							Tracer.Info(Tracer.Compiler, "Sealing type {0}", f.Name);
						}
						if (outer != null && !cantNest)
						{
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs, wrapper.GetBaseTypeForDefineType());
						}
						else
#endif // STATIC_COMPILER
						{
							typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs, wrapper.GetBaseTypeForDefineType());
						}
					}
#if STATIC_COMPILER
					// When we're statically compiling, we associate the typeBuilder with the wrapper. This enables types in referenced assemblies to refer back to
					// types that we're currently compiling (i.e. a cyclic dependency between the currently assembly we're compiling and a referenced assembly).
					wrapper.GetClassLoader().SetWrapperForType(typeBuilder, wrapper);
					if (outer != null && cantNest)
					{
						AttributeHelper.SetNonNestedOuterClass(typeBuilder, outerClassWrapper.Name);
						AttributeHelper.SetNonNestedInnerClass(outer, f.Name);
					}
					if (outer == null && mangledTypeName != wrapper.Name)
					{
						// HACK we abuse the InnerClassAttribute to record to real name
						AttributeHelper.SetInnerClass(typeBuilder, wrapper.Name, wrapper.Modifiers);
					}
					if (typeBuilder.FullName != wrapper.Name
						&& wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
					{
						((CompilerClassLoader)wrapper.GetClassLoader()).AddNameMapping(wrapper.Name, typeBuilder.FullName);
					}
					if (f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
					{
						annotationBuilder = new AnnotationBuilder(this);
						((AotTypeWrapper)wrapper).SetAnnotation(annotationBuilder);
					}
					// For Java 5 Enum types, we generate a nested .NET enum.
					// This is primarily to support annotations that take enum parameters.
					if (f.IsEnum && f.IsPublic)
					{
						CompilerClassLoader ccl = (CompilerClassLoader)wrapper.GetClassLoader();
						string name = "__Enum";
						while (!ccl.ReserveName(f.Name + "$" + name))
						{
							name += "_";
						}
						enumBuilder = wrapper.TypeAsBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, Types.Enum);
						AttributeHelper.HideFromJava(enumBuilder);
						enumBuilder.DefineField("value__", Types.Int32, FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
						for (int i = 0; i < f.Fields.Length; i++)
						{
							if (f.Fields[i].IsEnum)
							{
								FieldBuilder fieldBuilder = enumBuilder.DefineField(f.Fields[i].Name, enumBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
								fieldBuilder.SetConstant(i);
							}
						}
					}
					TypeWrapper[] interfaces = wrapper.Interfaces;
					string[] implements = new string[interfaces.Length];
					for (int i = 0; i < implements.Length; i++)
					{
						implements[i] = interfaces[i].Name;
					}
					if (outer != null)
					{
						Modifiers innerClassModifiers = outerClass.accessFlags;
						string innerClassName = classFile.GetConstantPoolClass(outerClass.innerClass);
						if (innerClassName == classFile.Name && innerClassName == outerClassWrapper.Name + "$" + typeBuilder.Name)
						{
							innerClassName = null;
						}
						AttributeHelper.SetInnerClass(typeBuilder, innerClassName, innerClassModifiers);
					}
					else if (outerClass.innerClass != 0)
					{
						AttributeHelper.SetInnerClass(typeBuilder, null, outerClass.accessFlags);
					}
					AttributeHelper.SetImplementsAttribute(typeBuilder, interfaces);
					if (classFile.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(typeBuilder);
					}
					if (classFile.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(typeBuilder, classFile.GenericSignature);
					}
					if (classFile.EnclosingMethod != null)
					{
						AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, classFile.EnclosingMethod[0], classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
					}
					if (wrapper.classLoader.EmitStackTraceInfo)
					{
						if (f.SourceFileAttribute != null)
						{
							if (f.SourceFileAttribute != typeBuilder.Name + ".java")
							{
								AttributeHelper.SetSourceFile(typeBuilder, f.SourceFileAttribute);
							}
						}
						else
						{
							AttributeHelper.SetSourceFile(typeBuilder, null);
						}
					}
					// NOTE in Whidbey we can (and should) use CompilerGeneratedAttribute to mark Synthetic types
					if (setModifiers || classFile.IsInternal || (classFile.Modifiers & (Modifiers.Synthetic | Modifiers.Annotation | Modifiers.Enum)) != 0)
					{
						AttributeHelper.SetModifiers(typeBuilder, classFile.Modifiers, classFile.IsInternal);
					}
#endif // STATIC_COMPILER
					if (hasclinit)
					{
						// We create a empty method that we can use to trigger our .cctor
						// (previously we used RuntimeHelpers.RunClassConstructor, but that is slow and requires additional privileges)
						MethodAttributes attribs = MethodAttributes.Static | MethodAttributes.SpecialName;
						if (classFile.IsAbstract)
						{
							bool hasfields = false;
							// If we have any public static fields, the cctor trigger must (and may) be public as well
							foreach (ClassFile.Field fld in classFile.Fields)
							{
								if (fld.IsPublic && fld.IsStatic)
								{
									hasfields = true;
									break;
								}
							}
							attribs |= hasfields ? MethodAttributes.Public : MethodAttributes.FamORAssem;
						}
						else
						{
							attribs |= MethodAttributes.Public;
						}
						clinitMethod = typeBuilder.DefineMethod("__<clinit>", attribs, null, null);
						clinitMethod.GetILGenerator().Emit(OpCodes.Ret);
						// FXBUG on .NET 2.0 RTM x64 the JIT sometimes throws an InvalidProgramException while trying to inline this method,
						// so we prevent inlining for now (it also turns out that on x86 not inlining this method actually has a positive perf impact in some cases...)
						// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=285772
						clinitMethod.SetImplementationFlags(clinitMethod.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
					}
					if (HasStructLayoutAttributeAnnotation(classFile))
					{
						// when we have a StructLayoutAttribute, field order is significant,
						// so we link all fields here to make sure they are created in class file order.
						foreach (FieldWrapper fw in fields)
						{
							fw.Link();
						}
					}
				}
				catch (Exception x)
				{
					JVM.CriticalFailure("Exception during JavaTypeImpl.CreateStep2NoFail", x);
				}
			}

			private sealed class DelegateConstructorMethodWrapper : MethodWrapper
			{
				private ConstructorBuilder constructor;
				private MethodInfo invoke;

				internal DelegateConstructorMethodWrapper(DynamicTypeWrapper tw, ClassFile.Method m)
					: base(tw, m.Name, m.Signature, null, null, null, m.Modifiers, MemberFlags.None)
				{
				}

				protected override void DoLinkMethod()
				{
					MethodAttributes attribs = MethodAttributes.HideBySig | MethodAttributes.Public;
					constructor = this.DeclaringType.TypeAsBuilder.DefineConstructor(attribs, CallingConventions.Standard, new Type[] { Types.Object, Types.IntPtr }, null, null);
					constructor.SetImplementationFlags(MethodImplAttributes.Runtime);
					MethodWrapper mw = GetParameters()[0].GetMethods()[0];
					mw.Link();
					invoke = (MethodInfo)mw.GetMethod();
				}

				internal override void EmitNewobj(CodeEmitter ilgen)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldvirtftn, invoke);
					ilgen.Emit(OpCodes.Newobj, constructor);
				}
			}

			private static bool HasStructLayoutAttributeAnnotation(ClassFile c)
			{
				if (c.Annotations != null)
				{
					foreach (object[] annot in c.Annotations)
					{
						if ("Lcli/System/Runtime/InteropServices/StructLayoutAttribute$Annotation;".Equals(annot[1]))
						{
							return true;
						}
					}
				}
				return false;
			}

#if STATIC_COMPILER
			private ClassFile.InnerClass getOuterClass()
			{
				ClassFile.InnerClass[] innerClasses = classFile.InnerClasses;
				if (innerClasses != null)
				{
					for (int j = 0; j < innerClasses.Length; j++)
					{
						if (innerClasses[j].innerClass != 0
							&& classFile.GetConstantPoolClass(innerClasses[j].innerClass) == classFile.Name)
						{
							return innerClasses[j];
						}
					}
				}
				return new ClassFile.InnerClass();
			}

			private bool IsSideEffectFreeStaticInitializerOrNoop(ClassFile.Method m, out bool noop)
			{
				if (m.ExceptionTable.Length != 0)
				{
					noop = false;
					return false;
				}
				noop = true;
				for (int i = 0; i < m.Instructions.Length; i++)
				{
					NormalizedByteCode bc = m.Instructions[i].NormalizedOpCode;
					if (bc == NormalizedByteCode.__getstatic || bc == NormalizedByteCode.__putstatic)
					{
						ClassFile.ConstantPoolItemFieldref fld = classFile.SafeGetFieldref(m.Instructions[i].Arg1);
						if (fld == null || fld.Class != classFile.Name)
						{
							noop = false;
							return false;
						}
						// don't allow getstatic to load non-primitive fields, because that would
						// cause the verifier to try to load the type
						if (bc == NormalizedByteCode.__getstatic && "L[".IndexOf(fld.Signature[0]) != -1)
						{
							noop = false;
							return false;
						}
						if (bc == NormalizedByteCode.__putstatic)
						{
							ClassFile.Field field = classFile.GetField(fld.Name, fld.Signature);
							if (field == null)
							{
								noop = false;
								return false;
							}
							if (!field.IsFinal || !field.IsStatic || !field.IsProperty || field.PropertySetter != null)
							{
								noop = false;
							}
						}
					}
					else if (bc == NormalizedByteCode.__areturn ||
						bc == NormalizedByteCode.__ireturn ||
						bc == NormalizedByteCode.__lreturn ||
						bc == NormalizedByteCode.__freturn ||
						bc == NormalizedByteCode.__dreturn)
					{
						noop = false;
						return false;
					}
					else if (ByteCodeMetaData.CanThrowException(bc))
					{
						noop = false;
						return false;
					}
					else if (bc == NormalizedByteCode.__ldc
						&& classFile.SafeIsConstantPoolClass(m.Instructions[i].Arg1))
					{
						noop = false;
						return false;
					}
					else if (bc == NormalizedByteCode.__aconst_null
						|| bc == NormalizedByteCode.__return
						|| bc == NormalizedByteCode.__nop)
					{
						// valid instructions in a potential noop <clinit>
					}
					else
					{
						noop = false;
					}
				}
				// the method needs to be verifiable to be side effect free, since we already analysed it,
				// we know that the verifier won't try to load any types (which isn't allowed at this time)
				try
				{
					new MethodAnalyzer(wrapper, null, classFile, m, wrapper.classLoader);
					return true;
				}
				catch (VerifyError)
				{
					return false;
				}
			}
#endif // STATIC_COMPILER

#if STATIC_COMPILER || NET_4_0
			private static bool ContainsMemberWrapper(List<FieldWrapper> members, string name, string sig)
			{
				foreach (MemberWrapper mw in members)
				{
					if (mw.Name == name && mw.Signature == sig)
					{
						return true;
					}
				}
				return false;
			}

			private static bool ContainsMemberWrapper(List<MethodWrapper> members, string name, string sig)
			{
				foreach (MemberWrapper mw in members)
				{
					if (mw.Name == name && mw.Signature == sig)
					{
						return true;
					}
				}
				return false;
			}
#endif // STATIC_COMPILER || NET_4_0

			private MethodWrapper GetMethodWrapperDuringCtor(TypeWrapper lookup, List<MethodWrapper> methods, string name, string sig)
			{
				if (lookup == wrapper)
				{
					foreach (MethodWrapper mw in methods)
					{
						if (mw.Name == name && mw.Signature == sig)
						{
							return mw;
						}
					}
					if (lookup.BaseTypeWrapper == null)
					{
						return null;
					}
					else
					{
						return lookup.BaseTypeWrapper.GetMethodWrapper(name, sig, true);
					}
				}
				else
				{
					return lookup.GetMethodWrapper(name, sig, true);
				}
			}

			private void AddMirandaMethods(List<MethodWrapper> methods, List<MethodWrapper> baseMethods, TypeWrapper tw)
			{
				foreach (TypeWrapper iface in tw.Interfaces)
				{
					if (iface.IsPublic && this.wrapper.IsInterface)
					{
						// for interfaces, we only need miranda methods for non-public interfaces that we extend
						continue;
					}
					AddMirandaMethods(methods, baseMethods, iface);
					foreach (MethodWrapper ifmethod in iface.GetMethods())
					{
						// skip <clinit>
						if (!ifmethod.IsStatic)
						{
							TypeWrapper lookup = wrapper;
							while (lookup != null)
							{
								MethodWrapper mw = GetMethodWrapperDuringCtor(lookup, methods, ifmethod.Name, ifmethod.Signature);
								if (mw == null)
								{
									mw = new SmartCallMethodWrapper(wrapper, ifmethod.Name, ifmethod.Signature, null, null, null, Modifiers.Public | Modifiers.Abstract, MemberFlags.HideFromReflection | MemberFlags.MirandaMethod, SimpleOpCode.Call, SimpleOpCode.Callvirt);
									methods.Add(mw);
									baseMethods.Add(ifmethod);
									break;
								}
								if (!mw.IsStatic || mw.DeclaringType == wrapper)
								{
									break;
								}
								lookup = mw.DeclaringType.BaseTypeWrapper;
							}
						}
					}
				}
			}

#if STATIC_COMPILER || NET_4_0
			private void AddAccessStubMethods(List<MethodWrapper> methods, List<MethodWrapper> baseMethods, TypeWrapper tw)
			{
				foreach (MethodWrapper mw in tw.GetMethods())
				{
					if ((mw.IsPublic || mw.IsProtected)
						&& mw.Name != "<init>"
						&& !ContainsMemberWrapper(methods, mw.Name, mw.Signature))
					{
						MethodWrapper stub = new SmartCallMethodWrapper(wrapper, mw.Name, mw.Signature, null, null, null, mw.Modifiers, MemberFlags.HideFromReflection | MemberFlags.AccessStub, SimpleOpCode.Call, SimpleOpCode.Callvirt);
						methods.Add(stub);
						baseMethods.Add(mw);
					}
				}
			}
#endif // STATIC_COMPILER || NET_4_0

#if STATIC_COMPILER
			private static bool CheckInnerOuterNames(string inner, string outer)
			{
				// do some sanity checks on the inner/outer class names
				return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.StartsWith(outer);
			}

			private static string GetInnerClassName(string outer, string inner)
			{
				Debug.Assert(CheckInnerOuterNames(inner, outer));
				return DynamicClassLoader.EscapeName(inner.Substring(outer.Length + 1));
			}
#endif // STATIC_COMPILER

			private int GetMethodIndex(MethodWrapper mw)
			{
				for (int i = 0; i < methods.Length; i++)
				{
					if (methods[i] == mw)
					{
						return i;
					}
				}
				throw new InvalidOperationException();
			}

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				Debug.Assert(mw != null);
				bool unloadableOverrideStub = false;
				int index = GetMethodIndex(mw);
				MethodWrapper baseMethod = baseMethods[index];
				if (baseMethod != null)
				{
					baseMethod.Link();
					// check the loader constraints
					if (mw.ReturnType != baseMethod.ReturnType)
					{
						if (baseMethod.ReturnType.IsUnloadable || JVM.FinishingForDebugSave)
						{
							if (!mw.ReturnType.IsUnloadable || (!baseMethod.ReturnType.IsUnloadable && JVM.FinishingForDebugSave))
							{
								unloadableOverrideStub = true;
							}
						}
						else
						{
#if STATIC_COMPILER
							StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has a return type \"{1}\"", mw.ReturnType, baseMethod.ReturnType, mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#endif
							throw new LinkageError("Loader constraints violated");
						}
					}
					TypeWrapper[] here = mw.GetParameters();
					TypeWrapper[] there = baseMethod.GetParameters();
					for (int i = 0; i < here.Length; i++)
					{
						if (here[i] != there[i])
						{
							if (there[i].IsUnloadable || JVM.FinishingForDebugSave)
							{
								if (!here[i].IsUnloadable || (!there[i].IsUnloadable && JVM.FinishingForDebugSave))
								{
									unloadableOverrideStub = true;
								}
							}
							else
							{
#if STATIC_COMPILER
								StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has an argument type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has an argument type \"{1}\"", here[i], there[i], mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#endif
								throw new LinkageError("Loader constraints violated");
							}
						}
					}
				}
				Debug.Assert(mw.GetMethod() == null);
				MethodBase mb = GenerateMethod(index, unloadableOverrideStub);
				if ((mw.Modifiers & (Modifiers.Synchronized | Modifiers.Static)) == Modifiers.Synchronized)
				{
					// note that constructors cannot be synchronized in Java
					MethodBuilder mbld = (MethodBuilder)mb;
					mbld.SetImplementationFlags(mbld.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);
				}
				return mb;
			}

			private int GetFieldIndex(FieldWrapper fw)
			{
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i] == fw)
					{
						return i;
					}
				}
				throw new InvalidOperationException();
			}

			internal override FieldInfo LinkField(FieldWrapper fw)
			{
				if (fw is DynamicPropertyFieldWrapper)
				{
					((DynamicPropertyFieldWrapper)fw).DoLink(typeBuilder);
					return null;
				}
				int fieldIndex = GetFieldIndex(fw);
#if STATIC_COMPILER
				if (fieldIndex >= classFile.Fields.Length)
				{
					// this must be a field defined in map.xml
					FieldAttributes fieldAttribs = 0;
					if (fw.IsPublic)
					{
						fieldAttribs |= FieldAttributes.Public;
					}
					else if (fw.IsProtected)
					{
						fieldAttribs |= FieldAttributes.FamORAssem;
					}
					else if (fw.IsPrivate)
					{
						fieldAttribs |= FieldAttributes.Private;
					}
					else
					{
						fieldAttribs |= FieldAttributes.Assembly;
					}
					if (fw.IsStatic)
					{
						fieldAttribs |= FieldAttributes.Static;
					}
					if (fw.IsFinal)
					{
						fieldAttribs |= FieldAttributes.InitOnly;
					}
					return typeBuilder.DefineField(fw.Name, fw.FieldTypeWrapper.TypeAsSignatureType, fieldAttribs);
				}
#endif // STATIC_COMPILER
				FieldBuilder field;
				ClassFile.Field fld = classFile.Fields[fieldIndex];
				string fieldName = fld.Name;
				TypeWrapper typeWrapper = fw.FieldTypeWrapper;
				Type type = typeWrapper.TypeAsSignatureType;
				bool setNameSig = typeWrapper.IsErasedOrBoxedPrimitiveOrRemapped;
				if (setNameSig)
				{
					// TODO use clashtable
					// the field name is mangled here, because otherwise it can (theoretically)
					// conflict with another unloadable or object or ghost array field
					// (fields can be overloaded on type)
					fieldName += "/" + typeWrapper.Name;
				}
				FieldAttributes attribs = 0;
				MethodAttributes methodAttribs = MethodAttributes.HideBySig;
#if STATIC_COMPILER
				bool setModifiers = fld.IsInternal || (fld.Modifiers & (Modifiers.Synthetic | Modifiers.Enum)) != 0;
				bool hideFromJava = false;
#endif
				bool isWrappedFinal = false;
				if (fld.IsPrivate)
				{
					attribs |= FieldAttributes.Private;
				}
				else if (fld.IsProtected)
				{
					attribs |= FieldAttributes.FamORAssem;
					methodAttribs |= MethodAttributes.FamORAssem;
				}
				else if (fld.IsPublic)
				{
					attribs |= FieldAttributes.Public;
					methodAttribs |= MethodAttributes.Public;
				}
				else
				{
					attribs |= FieldAttributes.Assembly;
					methodAttribs |= MethodAttributes.Assembly;
				}

#if STATIC_COMPILER
				if (wrapper.IsPublic && (fw.IsPublic || fw.IsProtected) && !fw.FieldTypeWrapper.IsPublic)
				{
					// this field is going to get a type 2 access stub, so we hide the actual field
					attribs &= ~FieldAttributes.FieldAccessMask;
					attribs |= FieldAttributes.Assembly;
					hideFromJava = true;
				}
#endif

				if (fld.IsStatic)
				{
					attribs |= FieldAttributes.Static;
					methodAttribs |= MethodAttributes.Static;
				}
				// NOTE "constant" static finals are converted into literals
				// TODO it would be possible for Java code to change the value of a non-blank static final, but I don't
				// know if we want to support this (since the Java JITs don't really support it either)
				object constantValue = fld.ConstantValue;
				if (fld.IsStatic && fld.IsFinal && constantValue != null)
				{
					Profiler.Count("Static Final Constant");
					attribs |= FieldAttributes.Literal;
					field = typeBuilder.DefineField(fieldName, type, attribs);
					field.SetConstant(constantValue);
				}
				else
				{
					if (fld.IsFinal)
					{
						isWrappedFinal = fw is GetterFieldWrapper;
						if (isWrappedFinal)
						{
							// NOTE public/protected blank final fields get converted into a read-only property with a private field
							// backing store
							attribs &= ~FieldAttributes.FieldAccessMask;
							attribs |= FieldAttributes.PrivateScope;
						}
						else if (wrapper.IsInterface || wrapper.classLoader.StrictFinalFieldSemantics)
						{
							attribs |= FieldAttributes.InitOnly;
						}
						else
						{
#if STATIC_COMPILER
							setModifiers = true;
#endif
						}
					}
					Type[] modreq = Type.EmptyTypes;
					if (fld.IsVolatile)
					{
						modreq = new Type[] { Types.IsVolatile };
					}
					string realFieldName = fieldName;
#if STATIC_COMPILER
					if (hideFromJava)
					{
						// instead of adding HideFromJava we rename the field to avoid confusing broken compilers
						// see https://sourceforge.net/tracker/?func=detail&atid=525264&aid=3056721&group_id=69637
						realFieldName = "__<>" + fieldName;
					}
#endif // STATIC_COMPILER
					field = typeBuilder.DefineField(realFieldName, type, modreq, Type.EmptyTypes, attribs);
					if (fld.IsTransient)
					{
						CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(JVM.Import(typeof(NonSerializedAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
						field.SetCustomAttribute(transientAttrib);
					}
					if (isWrappedFinal)
					{
						methodAttribs |= MethodAttributes.SpecialName;
						MethodBuilder getter = typeBuilder.DefineMethod(GenerateUniqueMethodName("get_" + fieldName, type, Type.EmptyTypes), methodAttribs, CallingConventions.Standard, type, Type.EmptyTypes);
						AttributeHelper.HideFromJava(getter);
						CodeEmitter ilgen = CodeEmitter.Create(getter);
						if (fld.IsStatic)
						{
							ilgen.Emit(OpCodes.Ldsfld, field);
						}
						else
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldfld, field);
						}
						ilgen.Emit(OpCodes.Ret);
						ilgen.DoEmit();

						PropertyBuilder pb = typeBuilder.DefineProperty(fieldName, PropertyAttributes.None, type, Type.EmptyTypes);
						pb.SetGetMethod(getter);
						if (!fld.IsStatic)
						{
							// this method exist for use by reflection only
							// (that's why it only exists for instance fields, final static fields are not settable by reflection)
							MethodBuilder setter = typeBuilder.DefineMethod("__<set>", MethodAttributes.PrivateScope, CallingConventions.Standard, Types.Void, new Type[] { type });
							ilgen = CodeEmitter.Create(setter);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldarg_1);
							ilgen.Emit(OpCodes.Stfld, field);
							ilgen.Emit(OpCodes.Ret);
							pb.SetSetMethod(setter);
							ilgen.DoEmit();
						}
						((GetterFieldWrapper)fw).SetGetter(getter);
#if STATIC_COMPILER
						if (setNameSig)
						{
							AttributeHelper.SetNameSig(getter, fld.Name, fld.Signature);
						}
						if (setModifiers || fld.IsTransient)
						{
							AttributeHelper.SetModifiers(getter, fld.Modifiers, fld.IsInternal);
						}
						if (fld.DeprecatedAttribute)
						{
							// NOTE for better interop with other languages, we set the ObsoleteAttribute on the property itself
							AttributeHelper.SetDeprecatedAttribute(pb);
						}
						if (fld.GenericSignature != null)
						{
							AttributeHelper.SetSignatureAttribute(getter, fld.GenericSignature);
						}
#endif // STATIC_COMPILER
					}
				}
#if STATIC_COMPILER
				if (!isWrappedFinal)
				{
					// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
					// the Java modifiers
					if (setModifiers)
					{
						AttributeHelper.SetModifiers(field, fld.Modifiers, fld.IsInternal);
					}
					if (setNameSig)
					{
						AttributeHelper.SetNameSig(field, fld.Name, fld.Signature);
					}
					if (fld.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(field);
					}
					if (fld.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(field, fld.GenericSignature);
					}
				}
#endif // STATIC_COMPILER
				return field;
			}

			internal override void EmitRunClassConstructor(CodeEmitter ilgen)
			{
				if (clinitMethod != null)
				{
					ilgen.Emit(OpCodes.Call, clinitMethod);
				}
			}

			internal override DynamicImpl Finish()
			{
				if (wrapper.BaseTypeWrapper != null)
				{
					wrapper.BaseTypeWrapper.Finish();
				}
#if STATIC_COMPILER
				if (outerClassWrapper != null)
				{
					outerClassWrapper.Finish();
				}
#endif // STATIC_COMPILER
				// NOTE there is a bug in the CLR (.NET 1.0 & 1.1 [1.2 is not yet available]) that
				// causes the AppDomain.TypeResolve event to receive the incorrect type name for nested types.
				// The Name in the ResolveEventArgs contains only the nested type name, not the full type name,
				// for example, if the type being resolved is "MyOuterType+MyInnerType", then the event only
				// receives "MyInnerType" as the name. Since we only compile inner classes as nested types
				// when we're statically compiling, we can only run into this bug when we're statically compiling.
				// NOTE To work around this bug, we have to make sure that all types that are going to be
				// required in finished form, are finished explicitly here. It isn't clear what other types are
				// required to be finished. I instrumented a static compilation of classpath.dll and this
				// turned up no other cases of the TypeResolve event firing.
				for (int i = 0; i < wrapper.Interfaces.Length; i++)
				{
					wrapper.Interfaces[i].Finish();
				}
				// make sure all classes are loaded, before we start finishing the type. During finishing, we
				// may not run any Java code, because that might result in a request to finish the type that we
				// are in the process of finishing, and this would be a problem.
				classFile.Link(wrapper);
				for (int i = 0; i < fields.Length; i++)
				{
					fields[i].Link();
				}
				for (int i = 0; i < methods.Length; i++)
				{
					methods[i].Link();
				}
				// this is the correct lock, FinishCore doesn't call any user code and mutates global state,
				// so it needs to be protected by a lock.
				lock (this)
				{
					return FinishCore();
				}
			}

			private FinishedTypeImpl FinishCore()
			{
				// it is possible that the loading of the referenced classes triggered a finish of us,
				// if that happens, we just return
				if (finishedType != null)
				{
					return finishedType;
				}
				if (finishInProgress)
				{
					throw new InvalidOperationException("Recursive finish attempt for " + wrapper.Name);
				}
				finishInProgress = true;
				Tracer.Info(Tracer.Compiler, "Finishing: {0}", wrapper.Name);
				Profiler.Enter("JavaTypeImpl.Finish.Core");
				try
				{
					TypeWrapper declaringTypeWrapper = null;
					TypeWrapper[] innerClassesTypeWrappers = TypeWrapper.EmptyArray;
					// if we're an inner class, we need to attach an InnerClass attribute
					ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
					if (innerclasses != null)
					{
						// TODO consider not pre-computing innerClassesTypeWrappers and declaringTypeWrapper here
						List<TypeWrapper> wrappers = new List<TypeWrapper>();
						for (int i = 0; i < innerclasses.Length; i++)
						{
							if (innerclasses[i].innerClass != 0 && innerclasses[i].outerClass != 0)
							{
								if (classFile.GetConstantPoolClassType(innerclasses[i].outerClass) == wrapper)
								{
									wrappers.Add(classFile.GetConstantPoolClassType(innerclasses[i].innerClass));
								}
								if (classFile.GetConstantPoolClassType(innerclasses[i].innerClass) == wrapper)
								{
									declaringTypeWrapper = classFile.GetConstantPoolClassType(innerclasses[i].outerClass);
								}
							}
						}
						innerClassesTypeWrappers = wrappers.ToArray();
#if STATIC_COMPILER
						// before we bake our type, we need to link any inner annotations to allow them to create their attribute type (as a nested type)
						foreach (TypeWrapper tw in innerClassesTypeWrappers)
						{
							DynamicTypeWrapper dtw = tw as DynamicTypeWrapper;
							if (dtw != null)
							{
								JavaTypeImpl impl = dtw.impl as JavaTypeImpl;
								if (impl != null)
								{
									if (impl.annotationBuilder != null)
									{
										impl.annotationBuilder.Link();
									}
								}
							}
						}
#endif //STATIC_COMPILER
					}
					FinishContext context = new FinishContext(classFile, wrapper, typeBuilder);
#if STATIC_COMPILER
					if (annotationBuilder != null)
					{
						CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(AnnotationAttributeAttribute)).GetConstructor(new Type[] { Types.String }), new object[] { annotationBuilder.AttributeTypeName });
						typeBuilder.SetCustomAttribute(cab);
					}
					context.RegisterPostFinishProc(delegate
					{
						if (enumBuilder != null)
						{
							enumBuilder.CreateType();
						}
						if (annotationBuilder != null)
						{
							annotationBuilder.Finish(this);
						}
					});
#endif
					Type type = context.FinishImpl();
					MethodInfo finishedClinitMethod = clinitMethod;
#if !STATIC_COMPILER
					if (finishedClinitMethod != null)
					{
						// In dynamic mode, we may need to emit a call to this method from a DynamicMethod which doesn't support calling unfinished methods,
						// so we must resolve to the real method here.
						finishedClinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					}
#endif
					finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, wrapper.ReflectiveModifiers, Metadata.Create(classFile), finishedClinitMethod, finalizeMethod
#if STATIC_COMPILER
, annotationBuilder, enumBuilder
#endif
);
					return finishedType;
				}
#if STATIC_COMPILER
				catch (FileFormatLimitationExceededException)
				{
					throw;
				}
#endif
				catch (Exception x)
				{
					JVM.CriticalFailure("Exception during finishing of: " + wrapper.Name, x);
					return null;
				}
				finally
				{
					Profiler.Leave("JavaTypeImpl.Finish.Core");
				}
			}

#if STATIC_COMPILER
			private bool IsValidAnnotationElementType(string type)
			{
				if (type[0] == '[')
				{
					type = type.Substring(1);
				}
				switch (type)
				{
					case "Z":
					case "B":
					case "S":
					case "C":
					case "I":
					case "J":
					case "F":
					case "D":
					case "Ljava.lang.String;":
					case "Ljava.lang.Class;":
						return true;
				}
				if (type.StartsWith("L") && type.EndsWith(";"))
				{
					try
					{
						TypeWrapper tw = wrapper.GetClassLoader().LoadClassByDottedNameFast(type.Substring(1, type.Length - 2));
						if (tw != null)
						{
							if ((tw.Modifiers & Modifiers.Annotation) != 0)
							{
								return true;
							}
							if ((tw.Modifiers & Modifiers.Enum) != 0)
							{
								TypeWrapper enumType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("java.lang.Enum");
								if (enumType != null && tw.IsSubTypeOf(enumType))
								{
									return true;
								}
							}
						}
					}
					catch
					{
					}
				}
				return false;
			}

			sealed class AnnotationBuilder : Annotation
			{
				private JavaTypeImpl impl;
				private TypeBuilder annotationTypeBuilder;
				private TypeBuilder attributeTypeBuilder;
				private ConstructorBuilder defineConstructor;

				internal AnnotationBuilder(JavaTypeImpl o)
				{
					this.impl = o;
				}

				internal void Link()
				{
					if (impl == null)
					{
						return;
					}
					JavaTypeImpl o = impl;
					impl = null;

					// Make sure the annotation type only has valid methods
					for (int i = 0; i < o.methods.Length; i++)
					{
						if (!o.methods[i].IsStatic)
						{
							if (!o.methods[i].Signature.StartsWith("()"))
							{
								return;
							}
							if (!o.IsValidAnnotationElementType(o.methods[i].Signature.Substring(2)))
							{
								return;
							}
						}
					}

					// we only set annotationTypeBuilder if we're valid
					annotationTypeBuilder = o.typeBuilder;

					TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");

					// make sure we don't clash with another class name
					CompilerClassLoader ccl = (CompilerClassLoader)o.wrapper.GetClassLoader();
					string name = o.classFile.Name;
					while (!ccl.ReserveName(name + "Attribute"))
					{
						name += "_";
					}

					// TODO attribute should be .NET serializable
					TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Sealed;
					if (o.outerClassWrapper != null)
					{
						if (o.wrapper.IsPublic)
						{
							typeAttributes |= TypeAttributes.NestedPublic;
						}
						else
						{
							typeAttributes |= TypeAttributes.NestedAssembly;
						}
						attributeTypeBuilder = o.outerClassWrapper.TypeAsBuilder.DefineNestedType(GetInnerClassName(o.outerClassWrapper.Name, name) + "Attribute", typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
					}
					else
					{
						if (o.wrapper.IsPublic)
						{
							typeAttributes |= TypeAttributes.Public;
						}
						else
						{
							typeAttributes |= TypeAttributes.NotPublic;
						}
						attributeTypeBuilder = o.wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name + "Attribute", typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
					}
					if (o.wrapper.IsPublic)
					{
						// In the Java world, the class appears as a non-public proxy class
						AttributeHelper.SetModifiers(attributeTypeBuilder, Modifiers.Final, false);
					}
					// NOTE we "abuse" the InnerClassAttribute to add a custom attribute to name the class "$Proxy[Annotation]" in the Java world
					int dotindex = o.classFile.Name.LastIndexOf('.') + 1;
					AttributeHelper.SetInnerClass(attributeTypeBuilder, o.classFile.Name.Substring(0, dotindex) + "$Proxy" + o.classFile.Name.Substring(dotindex), Modifiers.Final);
					attributeTypeBuilder.AddInterfaceImplementation(o.typeBuilder);
					AttributeHelper.SetImplementsAttribute(attributeTypeBuilder, new TypeWrapper[] { o.wrapper });

					if (o.classFile.Annotations != null)
					{
						foreach (object[] def in o.classFile.Annotations)
						{
							if (def[1].Equals("Ljava/lang/annotation/Target;"))
							{
								for (int i = 2; i < def.Length; i += 2)
								{
									if (def[i].Equals("value"))
									{
										object[] val = def[i + 1] as object[];
										if (val != null
											&& val.Length > 0
											&& val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
										{
											AttributeTargets targets = 0;
											for (int j = 1; j < val.Length; j++)
											{
												object[] eval = val[j] as object[];
												if (eval != null
													&& eval.Length == 3
													&& eval[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
													&& eval[1].Equals("Ljava/lang/annotation/ElementType;"))
												{
													switch ((string)eval[2])
													{
														case "ANNOTATION_TYPE":
															targets |= AttributeTargets.Interface;
															break;
														case "CONSTRUCTOR":
															targets |= AttributeTargets.Constructor;
															break;
														case "FIELD":
															targets |= AttributeTargets.Field;
															break;
														case "LOCAL_VARIABLE":
															break;
														case "METHOD":
															targets |= AttributeTargets.Method;
															break;
														case "PACKAGE":
															targets |= AttributeTargets.Interface;
															break;
														case "PARAMETER":
															targets |= AttributeTargets.Parameter;
															break;
														case "TYPE":
															targets |= AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Delegate | AttributeTargets.Enum;
															break;
													}
												}
											}
											CustomAttributeBuilder cab2 = new CustomAttributeBuilder(JVM.Import(typeof(AttributeUsageAttribute)).GetConstructor(new Type[] { JVM.Import(typeof(AttributeTargets)) }), new object[] { targets });
											attributeTypeBuilder.SetCustomAttribute(cab2);
										}
									}
								}
							}
						}
					}

					defineConstructor = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { JVM.Import(typeof(object[])) });
					AttributeHelper.SetEditorBrowsableNever(defineConstructor);
				}

				private static Type TypeWrapperToAnnotationParameterType(TypeWrapper tw)
				{
					bool isArray = false;
					if (tw.IsArray)
					{
						isArray = true;
						tw = tw.ElementTypeWrapper;
					}
					if (tw.Annotation != null)
					{
						// we don't support Annotation args
						return null;
					}
					else
					{
						Type argType;
						if (tw == CoreClasses.java.lang.Class.Wrapper)
						{
							argType = Types.Type;
						}
						else if (tw.EnumType != null)
						{
							argType = tw.EnumType;
						}
						else
						{
							argType = tw.TypeAsSignatureType;
						}
						if (isArray)
						{
							argType = ArrayTypeWrapper.MakeArrayType(argType, 1);
						}
						return argType;
					}
				}

				internal string AttributeTypeName
				{
					get
					{
						Link();
						if (attributeTypeBuilder != null)
						{
							return attributeTypeBuilder.FullName;
						}
						return null;
					}
				}

				private static void EmitSetValueCall(TypeWrapper annotationAttributeBaseType, CodeEmitter ilgen, string name, TypeWrapper tw, int argIndex)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldstr, name);
					ilgen.Emit(OpCodes.Ldarg_S, (byte)argIndex);
					if (tw.TypeAsSignatureType.IsValueType)
					{
						ilgen.Emit(OpCodes.Box, tw.TypeAsSignatureType);
					}
					else if (tw.EnumType != null)
					{
						ilgen.Emit(OpCodes.Box, tw.EnumType);
					}
					MethodWrapper setValueMethod = annotationAttributeBaseType.GetMethodWrapper("setValue", "(Ljava.lang.String;Ljava.lang.Object;)V", false);
					setValueMethod.Link();
					setValueMethod.EmitCall(ilgen);
				}

				internal void Finish(JavaTypeImpl o)
				{
					Link();
					if (annotationTypeBuilder == null)
					{
						// not a valid annotation type
						return;
					}
					TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");
					annotationAttributeBaseType.Finish();

					int requiredArgCount = 0;
					int valueArg = -1;
					bool unsupported = false;
					for (int i = 0; i < o.methods.Length; i++)
					{
						if (!o.methods[i].IsStatic)
						{
							if (valueArg == -1 && o.methods[i].Name == "value")
							{
								valueArg = i;
							}
							if (o.classFile.Methods[i].AnnotationDefault == null)
							{
								if (TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType) == null)
								{
									unsupported = true;
									break;
								}
								requiredArgCount++;
							}
						}
					}

					ConstructorBuilder defaultConstructor = attributeTypeBuilder.DefineConstructor(unsupported || requiredArgCount > 0 ? MethodAttributes.Private : MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
					CodeEmitter ilgen;

					if (!unsupported)
					{
						if (requiredArgCount > 0)
						{
							Type[] args = new Type[requiredArgCount];
							for (int i = 0, j = 0; i < o.methods.Length; i++)
							{
								if (!o.methods[i].IsStatic)
								{
									if (o.classFile.Methods[i].AnnotationDefault == null)
									{
										args[j++] = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
									}
								}
							}
							ConstructorBuilder reqArgConstructor = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, args);
							AttributeHelper.HideFromJava(reqArgConstructor);
							ilgen = CodeEmitter.Create(reqArgConstructor);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Call, defaultConstructor);
							for (int i = 0, j = 0; i < o.methods.Length; i++)
							{
								if (!o.methods[i].IsStatic)
								{
									if (o.classFile.Methods[i].AnnotationDefault == null)
									{
										reqArgConstructor.DefineParameter(++j, ParameterAttributes.None, o.methods[i].Name);
										EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, j);
									}
								}
							}
							ilgen.Emit(OpCodes.Ret);
							ilgen.DoEmit();
						}
						else if (valueArg != -1)
						{
							// We don't have any required parameters, but we do have an optional "value" parameter,
							// so we create an additional constructor (the default constructor will be public in this case)
							// that accepts the value parameter.
							Type argType = TypeWrapperToAnnotationParameterType(o.methods[valueArg].ReturnType);
							if (argType != null)
							{
								ConstructorBuilder cb = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { argType });
								AttributeHelper.HideFromJava(cb);
								cb.DefineParameter(1, ParameterAttributes.None, "value");
								ilgen = CodeEmitter.Create(cb);
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Call, defaultConstructor);
								EmitSetValueCall(annotationAttributeBaseType, ilgen, "value", o.methods[valueArg].ReturnType, 1);
								ilgen.Emit(OpCodes.Ret);
								ilgen.DoEmit();
							}
						}
					}

					ilgen = CodeEmitter.Create(defaultConstructor);
					ilgen.Emit(OpCodes.Ldarg_0);
					o.wrapper.EmitClassLiteral(ilgen);
					annotationAttributeBaseType.GetMethodWrapper("<init>", "(Ljava.lang.Class;)V", false).EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ret);
					ilgen.DoEmit();

					ilgen = CodeEmitter.Create(defineConstructor);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Call, defaultConstructor);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					annotationAttributeBaseType.GetMethodWrapper("setDefinition", "([Ljava.lang.Object;)V", false).EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ret);
					ilgen.DoEmit();

					MethodWrapper getValueMethod = annotationAttributeBaseType.GetMethodWrapper("getValue", "(Ljava.lang.String;)Ljava.lang.Object;", false);
					MethodWrapper getByteValueMethod = annotationAttributeBaseType.GetMethodWrapper("getByteValue", "(Ljava.lang.String;)B", false);
					MethodWrapper getBooleanValueMethod = annotationAttributeBaseType.GetMethodWrapper("getBooleanValue", "(Ljava.lang.String;)Z", false);
					MethodWrapper getCharValueMethod = annotationAttributeBaseType.GetMethodWrapper("getCharValue", "(Ljava.lang.String;)C", false);
					MethodWrapper getShortValueMethod = annotationAttributeBaseType.GetMethodWrapper("getShortValue", "(Ljava.lang.String;)S", false);
					MethodWrapper getIntValueMethod = annotationAttributeBaseType.GetMethodWrapper("getIntValue", "(Ljava.lang.String;)I", false);
					MethodWrapper getFloatValueMethod = annotationAttributeBaseType.GetMethodWrapper("getFloatValue", "(Ljava.lang.String;)F", false);
					MethodWrapper getLongValueMethod = annotationAttributeBaseType.GetMethodWrapper("getLongValue", "(Ljava.lang.String;)J", false);
					MethodWrapper getDoubleValueMethod = annotationAttributeBaseType.GetMethodWrapper("getDoubleValue", "(Ljava.lang.String;)D", false);
					for (int i = 0; i < o.methods.Length; i++)
					{
						// skip <clinit>
						if (!o.methods[i].IsStatic)
						{
							MethodBuilder mb = attributeTypeBuilder.DefineMethod(o.methods[i].Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot, o.methods[i].ReturnTypeForDefineMethod, o.methods[i].GetParametersForDefineMethod());
							attributeTypeBuilder.DefineMethodOverride(mb, (MethodInfo)o.methods[i].GetMethod());
							ilgen = CodeEmitter.Create(mb);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldstr, o.methods[i].Name);
							if (o.methods[i].ReturnType.IsPrimitive)
							{
								if (o.methods[i].ReturnType == PrimitiveTypeWrapper.BYTE)
								{
									getByteValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.BOOLEAN)
								{
									getBooleanValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.CHAR)
								{
									getCharValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.SHORT)
								{
									getShortValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.INT)
								{
									getIntValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.FLOAT)
								{
									getFloatValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.LONG)
								{
									getLongValueMethod.EmitCall(ilgen);
								}
								else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.DOUBLE)
								{
									getDoubleValueMethod.EmitCall(ilgen);
								}
								else
								{
									throw new InvalidOperationException();
								}
							}
							else
							{
								getValueMethod.EmitCall(ilgen);
								o.methods[i].ReturnType.EmitCheckcast(null, ilgen);
							}
							ilgen.Emit(OpCodes.Ret);
							ilgen.DoEmit();

							if (o.classFile.Methods[i].AnnotationDefault != null
								&& !(o.methods[i].Name == "value" && requiredArgCount == 0))
							{
								// now add a .NET property for this annotation optional parameter
								Type argType = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
								if (argType != null)
								{
									PropertyBuilder pb = attributeTypeBuilder.DefineProperty(o.methods[i].Name, PropertyAttributes.None, argType, Type.EmptyTypes);
									AttributeHelper.HideFromJava(pb);
									MethodBuilder setter = attributeTypeBuilder.DefineMethod("set_" + o.methods[i].Name, MethodAttributes.Public, Types.Void, new Type[] { argType });
									AttributeHelper.HideFromJava(setter);
									pb.SetSetMethod(setter);
									ilgen = CodeEmitter.Create(setter);
									EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, 1);
									ilgen.Emit(OpCodes.Ret);
									ilgen.DoEmit();
									MethodBuilder getter = attributeTypeBuilder.DefineMethod("get_" + o.methods[i].Name, MethodAttributes.Public, argType, Type.EmptyTypes);
									AttributeHelper.HideFromJava(getter);
									pb.SetGetMethod(getter);
									// TODO implement the getter method
									ilgen = CodeEmitter.Create(getter);
									ilgen.ThrowException(JVM.Import(typeof(NotImplementedException)));
									ilgen.DoEmit();
								}
							}
						}
					}
					attributeTypeBuilder.CreateType();
				}

				internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						tb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						mb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						cb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						fb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						pb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						ab.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						pb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}
			}
#endif // STATIC_COMPILER

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					throw new InvalidOperationException("InnerClasses is only available for finished types");
				}
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					throw new InvalidOperationException("DeclaringTypeWrapper is only available for finished types");
				}
			}

			internal override Modifiers ReflectiveModifiers
			{
				get
				{
					ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
					if (innerclasses != null)
					{
						for (int i = 0; i < innerclasses.Length; i++)
						{
							if (innerclasses[i].innerClass != 0)
							{
								if (classFile.GetConstantPoolClass(innerclasses[i].innerClass) == wrapper.Name)
								{
									return innerclasses[i].accessFlags;
								}
							}
						}
					}
					return classFile.Modifiers;
				}
			}

			private void UpdateClashTable()
			{
				lock (this)
				{
					if (memberclashtable == null)
					{
						memberclashtable = new Dictionary<string, string>();
						for (int i = 0; i < methods.Length; i++)
						{
							// TODO at the moment we don't support constructor signature clash resolving, so we better
							// not put them in the clash table
							if (methods[i].IsLinked && methods[i].GetMethod() != null && methods[i].Name != "<init>")
							{
								string key = GenerateClashKey("method", methods[i].RealName, methods[i].ReturnTypeForDefineMethod, methods[i].GetParametersForDefineMethod());
								memberclashtable.Add(key, key);
							}
						}
					}
				}
			}

			private static string GenerateClashKey(string type, string name, Type retOrFieldType, Type[] args)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder(type);
				sb.Append(':').Append(name).Append(':').Append(retOrFieldType.FullName);
				if (args != null)
				{
					foreach (Type t in args)
					{
						sb.Append(':').Append(t.FullName);
					}
				}
				return sb.ToString();
			}

			internal static ConstructorBuilder DefineClassInitializer(TypeBuilder typeBuilder)
			{
				if (typeBuilder.IsInterface)
				{
					// LAMESPEC the ECMA spec says (part. I, sect. 8.5.3.2) that all interface members must be public, so we make
					// the class constructor public.
					// NOTE it turns out that on .NET 2.0 this isn't necessary anymore (neither Ref.Emit nor the CLR verifier complain about it),
					// but the C# compiler still considers interfaces with non-public methods to be invalid, so to keep interop with C# we have
					// to keep making the .cctor method public.
					return typeBuilder.DefineConstructor(MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
				}
				// NOTE we don't need to record the modifiers here, because they aren't visible from Java reflection
				return typeBuilder.DefineTypeInitializer();
			}

			// this finds the method that md is going to be overriding
			private MethodWrapper FindBaseMethod(string name, string sig, out bool explicitOverride)
			{
				Debug.Assert(!classFile.IsInterface);
				Debug.Assert(name != "<init>");

				explicitOverride = false;
				TypeWrapper tw = wrapper.BaseTypeWrapper;
				while (tw != null)
				{
					MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
					if (baseMethod == null)
					{
						return null;
					}
					// here are the complex rules for determining whether this method overrides the method we found
					// RULE 1: final methods may not be overridden
					// (note that we intentionally not check IsStatic here!)
					if (baseMethod.IsFinal
						&& !baseMethod.IsPrivate
						&& (baseMethod.IsPublic || baseMethod.IsProtected || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
					{
						throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overriden in " + wrapper.Name);
					}
					// RULE 1a: static methods are ignored (other than the RULE 1 check)
					if (baseMethod.IsStatic)
					{
					}
					// RULE 2: public & protected methods can be overridden (package methods are handled by RULE 4)
					// (by public, protected & *package* methods [even if they are in a different package])
					else if (baseMethod.IsPublic || baseMethod.IsProtected)
					{
						// if we already encountered a package method, we cannot override the base method of
						// that package method
						if (explicitOverride)
						{
							explicitOverride = false;
							return null;
						}
						return baseMethod;
					}
					// RULE 3: private and static methods are ignored
					else if (!baseMethod.IsPrivate)
					{
						// RULE 4: package methods can only be overridden in the same package
						if (baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)
							|| (baseMethod.IsInternal && baseMethod.DeclaringType.InternalsVisibleTo(wrapper)))
						{
							return baseMethod;
						}
						// since we encountered a method with the same name/signature that we aren't overriding,
						// we need to specify an explicit override
						// NOTE we only do this if baseMethod isn't private, because if it is, Reflection.Emit
						// will complain about the explicit MethodOverride (possibly a bug)
						explicitOverride = true;
					}
					tw = baseMethod.DeclaringType.BaseTypeWrapper;
				}
				return null;
			}

			internal string GenerateUniqueMethodName(string basename, MethodWrapper mw)
			{
				return GenerateUniqueMethodName(basename, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
			}

			internal string GenerateUniqueMethodName(string basename, Type returnType, Type[] parameterTypes)
			{
				string name = basename;
				string key = GenerateClashKey("method", name, returnType, parameterTypes);
				UpdateClashTable();
				lock (memberclashtable)
				{
					for (int clashcount = 0; memberclashtable.ContainsKey(key); clashcount++)
					{
						name = basename + "_" + clashcount;
						key = GenerateClashKey("method", name, returnType, parameterTypes);
					}
					memberclashtable.Add(key, key);
				}
				return name;
			}

			private static MethodInfo GetBaseFinalizeMethod(TypeWrapper wrapper)
			{
				for (; ; )
				{
					// HACK we get called during method linking (which is probably a bad idea) and
					// it is possible for the base type not to be finished yet, so we look at the
					// private state of the unfinished base types to find the finalize method.
					DynamicTypeWrapper dtw = wrapper as DynamicTypeWrapper;
					if (dtw == null)
					{
						break;
					}
					MethodWrapper mw = dtw.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, false);
					if (mw != null)
					{
						mw.Link();
					}
					MethodInfo finalizeImpl = dtw.impl.GetFinalizeMethod();
					if (finalizeImpl != null)
					{
						return finalizeImpl;
					}
					wrapper = wrapper.BaseTypeWrapper;
				}
				if (wrapper == CoreClasses.java.lang.Object.Wrapper || wrapper == CoreClasses.java.lang.Throwable.Wrapper)
				{
					return Types.Object.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance);
				}
				Type type = wrapper.TypeAsBaseType;
				MethodInfo baseFinalize = type.GetMethod("__<Finalize>", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
				if (baseFinalize != null)
				{
					return baseFinalize;
				}
				while (type != null)
				{
					foreach (MethodInfo m in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
					{
						if (m.Name == "Finalize"
							&& m.ReturnType == Types.Void
							&& m.GetParameters().Length == 0)
						{
							if (m.GetBaseDefinition().DeclaringType == Types.Object)
							{
								return m;
							}
						}
					}
					type = type.BaseType;
				}
				return null;
			}

			private MethodAttributes GetPropertyAccess(MethodWrapper mw)
			{
				string sig = mw.ReturnType.SigName;
				if (sig == "V")
				{
					sig = mw.GetParameters()[0].SigName;
				}
				int access = -1;
				foreach (ClassFile.Field field in classFile.Fields)
				{
					if (field.IsProperty
						&& field.IsStatic == mw.IsStatic
						&& field.Signature == sig
						&& (field.PropertyGetter == mw.Name || field.PropertySetter == mw.Name))
					{
						int nacc;
						if (field.IsPublic)
						{
							nacc = 3;
						}
						else if (field.IsProtected)
						{
							nacc = 2;
						}
						else if (field.IsPrivate)
						{
							nacc = 0;
						}
						else
						{
							nacc = 1;
						}
						if (nacc > access)
						{
							access = nacc;
						}
					}
				}
				switch (access)
				{
					case 0:
						return MethodAttributes.Private;
					case 1:
						return MethodAttributes.Assembly;
					case 2:
						return MethodAttributes.FamORAssem;
					case 3:
						return MethodAttributes.Public;
					default:
						throw new InvalidOperationException();
				}
			}

			private MethodBase GenerateMethod(int index, bool unloadableOverrideStub)
			{
				methods[index].AssertLinked();
				Profiler.Enter("JavaTypeImpl.GenerateMethod");
				try
				{
					if (index >= classFile.Methods.Length)
					{
						if (methods[index].IsMirandaMethod)
						{
							// We're a Miranda method
							Debug.Assert(baseMethods[index].DeclaringType.IsInterface);
							string name = GenerateUniqueMethodName(methods[index].Name, baseMethods[index]);
							MethodBuilder mb = typeBuilder.DefineMethod(name, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.CheckAccessOnOverride, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							AttributeHelper.HideFromReflection(mb);
#if STATIC_COMPILER
							if (unloadableOverrideStub || name != methods[index].Name)
							{
								// instead of creating an override stub, we created the Miranda method with the proper signature and
								// decorate it with a NameSigAttribute that contains the real signature
								AttributeHelper.SetNameSig(mb, methods[index].Name, methods[index].Signature);
							}
#endif // STATIC_COMPILER
							// if we changed the name or if the interface method name is remapped, we need to add an explicit methodoverride.
							if (!baseMethods[index].IsDynamicOnly && name != baseMethods[index].RealName)
							{
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethods[index].GetMethod());
							}
							return mb;
						}
						else if (methods[index].IsAccessStub)
						{
							Debug.Assert(!baseMethods[index].HasCallerID);
							MethodAttributes stubattribs = baseMethods[index].IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
							stubattribs |= MethodAttributes.HideBySig;
							if (baseMethods[index].IsStatic)
							{
								stubattribs |= MethodAttributes.Static;
							}
							else
							{
								stubattribs |= MethodAttributes.CheckAccessOnOverride | MethodAttributes.Virtual;
								if (baseMethods[index].IsAbstract && wrapper.IsAbstract)
								{
									stubattribs |= MethodAttributes.Abstract;
								}
								if (baseMethods[index].IsFinal)
								{
									// NOTE final methods still need to be virtual, because a subclass may need this method to
									// implement an interface method
									stubattribs |= MethodAttributes.Final | MethodAttributes.NewSlot;
								}
							}
							MethodBuilder mb = typeBuilder.DefineMethod(methods[index].Name, stubattribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							AttributeHelper.HideFromReflection(mb);
							if (!baseMethods[index].IsAbstract)
							{
								CodeEmitter ilgen = CodeEmitter.Create(mb);
								int argc = methods[index].GetParametersForDefineMethod().Length + (methods[index].IsStatic ? 0 : 1);
								for (int i = 0; i < argc; i++)
								{
									ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
								}
								baseMethods[index].EmitCall(ilgen);
								ilgen.Emit(OpCodes.Ret);
								ilgen.DoEmit();
							}
							else if (!wrapper.IsAbstract)
							{
								CodeEmitter ilgen = CodeEmitter.Create(mb);
								ilgen.EmitThrow("java.lang.AbstractMethodError", wrapper.Name + "." + methods[index].Name + methods[index].Signature);
								ilgen.DoEmit();
							}
							return mb;
						}
						else
						{
							throw new InvalidOperationException();
						}
					}
					ClassFile.Method m = classFile.Methods[index];
					MethodBase method;
					bool setNameSig = methods[index].ReturnType.IsErasedOrBoxedPrimitiveOrRemapped;
					foreach (TypeWrapper tw in methods[index].GetParameters())
					{
						setNameSig |= tw.IsErasedOrBoxedPrimitiveOrRemapped;
					}
					bool setModifiers = false;
					if (methods[index].HasCallerID && (m.Modifiers & Modifiers.VarArgs) != 0)
					{
						// the implicit callerID parameter was added at the end so that means we shouldn't use ParamArrayAttribute,
						// so we need to explicitly record that the method is varargs
						setModifiers = true;
					}
					MethodAttributes attribs = MethodAttributes.HideBySig;
					if (m.IsNative)
					{
						if (wrapper.IsPInvokeMethod(m))
						{
							// this doesn't appear to be necessary, but we use the flag in Finish to know
							// that we shouldn't emit a method body
							attribs |= MethodAttributes.PinvokeImpl;
						}
						else
						{
							setModifiers = true;
						}
					}
					if (methods[index].IsPropertyAccessor)
					{
						attribs |= GetPropertyAccess(methods[index]);
						attribs |= MethodAttributes.SpecialName;
						setModifiers = true;
					}
					else
					{
						if (m.IsPrivate)
						{
							attribs |= MethodAttributes.Private;
						}
						else if (m.IsProtected)
						{
							attribs |= MethodAttributes.FamORAssem;
						}
						else if (m.IsPublic)
						{
							attribs |= MethodAttributes.Public;
						}
						else
						{
							attribs |= MethodAttributes.Assembly;
						}
					}
					if (ReferenceEquals(m.Name, StringConstants.INIT))
					{
						Type[][] modopt = null;
						if (setNameSig)
						{
							// we add optional modifiers to make the signature unique
							TypeWrapper[] parameters = methods[index].GetParameters();
							modopt = new Type[parameters.Length][];
							for (int i = 0; i < parameters.Length; i++)
							{
								if (parameters[i].IsGhostArray)
								{
									TypeWrapper elemTypeWrapper = parameters[i];
									while (elemTypeWrapper.IsArray)
									{
										elemTypeWrapper = elemTypeWrapper.ElementTypeWrapper;
									}
									modopt[i] = new Type[] { elemTypeWrapper.TypeAsTBD };
								}
								else if (parameters[i].IsBoxedPrimitive)
								{
									modopt[i] = new Type[] { Types.Object };
								}
								else if (parameters[i].IsRemapped && parameters[i] is DotNetTypeWrapper)
								{
									modopt[i] = new Type[] { parameters[i].TypeAsSignatureType };
								}
								else if (parameters[i].IsUnloadable)
								{
									modopt[i] = new Type[] { wrapper.classLoader.GetTypeWrapperFactory().DefineUnloadable(parameters[i].Name) };
								}
							}
						}
						// strictfp is the only modifier that a constructor can have
						if (m.IsStrictfp)
						{
							setModifiers = true;
						}
						method = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, methods[index].GetParametersForDefineMethod(), null, modopt);
						((ConstructorBuilder)method).SetImplementationFlags(MethodImplAttributes.NoInlining);
					}
					else if (m.IsClassInitializer)
					{
						method = DefineClassInitializer(typeBuilder);
					}
					else
					{
						if (m.IsAbstract)
						{
							// only if the classfile is abstract, we make the CLR method abstract, otherwise,
							// we have to generate a method that throws an AbstractMethodError (because the JVM
							// allows abstract methods in non-abstract classes)
							if (classFile.IsAbstract)
							{
								if (classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
								{
									setModifiers = true;
								}
								else
								{
									attribs |= MethodAttributes.Abstract;
								}
							}
							else
							{
								setModifiers = true;
							}
						}
						if (m.IsFinal)
						{
							if (!m.IsStatic && !m.IsPrivate)
							{
								attribs |= MethodAttributes.Final;
							}
							else
							{
								setModifiers = true;
							}
						}
						if (m.IsStatic)
						{
							attribs |= MethodAttributes.Static;
							if (m.IsSynchronized)
							{
								setModifiers = true;
							}
						}
						else if (!m.IsPrivate)
						{
							attribs |= MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
						}
						string name = m.Name;
#if STATIC_COMPILER
						if ((m.Modifiers & Modifiers.Bridge) != 0 && (m.IsPublic || m.IsProtected) && wrapper.IsPublic)
						{
							string sigbase = m.Signature.Substring(0, m.Signature.LastIndexOf(')') + 1);
							foreach (MethodWrapper mw in methods)
							{
								if (mw.Name == m.Name && mw.Signature.StartsWith(sigbase) && mw.Signature != m.Signature)
								{
									// To prevent bridge methods with covariant return types from confusing
									// other .NET compilers (like C#), we rename the bridge method.
									name = "<bridge>" + name;
									setNameSig = true;
									break;
								}
							}
						}
#endif
						// if a method is virtual, we need to find the method it overrides (if any), for several reasons:
						// - if we're overriding a method that has a different name (e.g. some of the virtual methods
						//   in System.Object [Equals <-> equals]) we need to add an explicit MethodOverride
						// - if one of the base classes has a similar method that is private (or package) that we aren't
						//   overriding, we need to specify an explicit MethodOverride
						MethodWrapper baseMce = baseMethods[index];
						bool explicitOverride = methods[index].IsExplicitOverride;
						if ((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
						{
							// make sure the base method is already defined
							Debug.Assert(baseMce == null || baseMce.GetMethod() != null);
							if (baseMce == null || baseMce.DeclaringType.IsInterface)
							{
								// we need to set NewSlot here, to prevent accidentally overriding methods
								// (for example, if a Java class has a method "boolean Equals(object)", we don't want that method
								// to override System.Object.Equals)
								attribs |= MethodAttributes.NewSlot;
							}
							else
							{
								// if we have a method overriding a more accessible method (the JVM allows this), we need to make the
								// method more accessible, because otherwise the CLR will complain that we're reducing access
								MethodBase baseMethod = baseMce.GetMethod();
								if ((baseMethod.IsPublic && !m.IsPublic) ||
									((baseMethod.IsFamily || baseMethod.IsFamilyOrAssembly) && !m.IsPublic && !m.IsProtected) ||
									(!m.IsPublic && !m.IsProtected && !baseMce.DeclaringType.IsPackageAccessibleFrom(wrapper)))
								{
									attribs &= ~MethodAttributes.MemberAccessMask;
									attribs |= baseMethod.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
									setModifiers = true;
								}
							}
						}
						MethodBuilder mb = null;
#if STATIC_COMPILER
						mb = wrapper.DefineGhostMethod(name, attribs, methods[index]);
#endif
						if (mb == null)
						{
							bool needFinalize = false;
							bool needDispatch = false;
							MethodInfo baseFinalize = null;
							if (baseMce != null && ReferenceEquals(m.Name, StringConstants.FINALIZE) && ReferenceEquals(m.Signature, StringConstants.SIG_VOID))
							{
								baseFinalize = GetBaseFinalizeMethod(wrapper.BaseTypeWrapper);
								if (baseMce.DeclaringType == CoreClasses.java.lang.Object.Wrapper)
								{
									// This type is the first type in the hierarchy to introduce a finalize method
									// (other than the one in java.lang.Object obviously), so we need to override
									// the real Finalize method and emit a dispatch call to our finalize method.
									needFinalize = true;
									needDispatch = true;
								}
								else if (m.IsFinal)
								{
									// One of our base classes already has a  finalize method, so we already are
									// hooked into the real Finalize, but we need to override it again, to make it
									// final (so that non-Java types cannot override it either).
									needFinalize = true;
									needDispatch = false;
									// If the base class finalize was optimized away, we need a dispatch call after all.
									if (baseFinalize.DeclaringType == Types.Object)
									{
										needDispatch = true;
									}
								}
								else
								{
									// One of our base classes already has a finalize method, but it may have been an empty
									// method so that the hookup to the real Finalize was optimized away, we need to check
									// for that.
									if (baseFinalize.DeclaringType == Types.Object)
									{
										needFinalize = true;
										needDispatch = true;
									}
								}
								if (needFinalize &&
									!m.IsAbstract && !m.IsNative &&
									(!m.IsFinal || classFile.IsFinal) &&
									m.Instructions.Length > 0 &&
									m.Instructions[0].NormalizedOpCode == NormalizedByteCode.__return)
								{
									// we've got an empty finalize method, so we don't need to override the real finalizer
									// (not having a finalizer makes a huge perf difference)
									needFinalize = false;
								}
							}
							if (setNameSig || memberclashtable != null)
							{
								// TODO we really should make sure that the name we generate doesn't already exist in a
								// base class (not in the Java method namespace, but in the CLR method namespace)
								name = GenerateUniqueMethodName(name, methods[index]);
								if (name != m.Name)
								{
									setNameSig = true;
								}
							}
							bool needMethodImpl = baseMce != null && (setNameSig || explicitOverride || baseMce.RealName != name) && !needFinalize;
							if (unloadableOverrideStub || needMethodImpl)
							{
								attribs |= MethodAttributes.NewSlot;
							}
							mb = typeBuilder.DefineMethod(name, attribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							if (unloadableOverrideStub)
							{
								GenerateUnloadableOverrideStub(wrapper, typeBuilder, baseMce, mb, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							}
							else if (needMethodImpl)
							{
								// assert that the method we're overriding is in fact virtual and not final!
								Debug.Assert(baseMce.GetMethod().IsVirtual && !baseMce.GetMethod().IsFinal);
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMce.GetMethod());
							}
							if (!m.IsStatic && !m.IsAbstract && !m.IsPrivate && baseMce != null && !baseMce.DeclaringType.IsPackageAccessibleFrom(wrapper))
							{
								// we may have to explicitly override another package accessible abstract method
								TypeWrapper btw = baseMce.DeclaringType.BaseTypeWrapper;
								while (btw != null)
								{
									MethodWrapper bmw = btw.GetMethodWrapper(m.Name, m.Signature, true);
									if (bmw == null)
									{
										break;
									}
									if (bmw.DeclaringType.IsPackageAccessibleFrom(wrapper) && bmw.IsAbstract && !(bmw.IsPublic || bmw.IsProtected))
									{
										if (bmw != baseMce)
										{
											typeBuilder.DefineMethodOverride(mb, (MethodInfo)bmw.GetMethod());
										}
										break;
									}
									btw = bmw.DeclaringType.BaseTypeWrapper;
								}
							}
							// if we're overriding java.lang.Object.finalize we need to emit a stub to override System.Object.Finalize,
							// or if we're subclassing a non-Java class that has a Finalize method, we need a new Finalize override
							if (needFinalize)
							{
								string finalizeName = baseFinalize.Name;
								MethodWrapper mwClash = wrapper.GetMethodWrapper(finalizeName, StringConstants.SIG_VOID, true);
								if (mwClash != null && mwClash.GetMethod() != baseFinalize)
								{
									finalizeName = "__<Finalize>";
								}
								MethodAttributes attr = MethodAttributes.HideBySig | MethodAttributes.Virtual;
								// make sure we don't reduce accessibility
								attr |= baseFinalize.IsPublic ? MethodAttributes.Public : MethodAttributes.Family;
								if (m.IsFinal)
								{
									attr |= MethodAttributes.Final;
								}
								finalizeMethod = typeBuilder.DefineMethod(finalizeName, attr, CallingConventions.Standard, Types.Void, Type.EmptyTypes);
								if (finalizeName != baseFinalize.Name)
								{
									typeBuilder.DefineMethodOverride(finalizeMethod, baseFinalize);
								}
								AttributeHelper.HideFromJava(finalizeMethod);
								CodeEmitter ilgen = CodeEmitter.Create(finalizeMethod);
								ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.SkipFinalizer);
								CodeEmitterLabel skip = ilgen.DefineLabel();
								ilgen.Emit(OpCodes.Brtrue_S, skip);
								if (needDispatch)
								{
									ilgen.BeginExceptionBlock();
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Callvirt, mb);
									ilgen.Emit(OpCodes.Leave, skip);
									ilgen.BeginCatchBlock(Types.Object);
									ilgen.Emit(OpCodes.Leave, skip);
									ilgen.EndExceptionBlock();
								}
								else
								{
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Call, baseFinalize);
								}
								ilgen.MarkLabel(skip);
								ilgen.Emit(OpCodes.Ret);
								ilgen.DoEmit();
							}
#if STATIC_COMPILER
							if (classFile.Methods[index].AnnotationDefault != null)
							{
								CustomAttributeBuilder cab = new CustomAttributeBuilder(StaticCompiler.GetRuntimeType("IKVM.Attributes.AnnotationDefaultAttribute").GetConstructor(new Type[] { Types.Object }), new object[] { classFile.Methods[index].AnnotationDefault });
								mb.SetCustomAttribute(cab);
							}
#endif // STATIC_COMPILER
						}
						method = mb;
					}
					string[] exceptions = m.ExceptionsAttribute;
					methods[index].SetDeclaredExceptions(exceptions);
#if STATIC_COMPILER
					AttributeHelper.SetThrowsAttribute(method, exceptions);
					if (setModifiers || m.IsInternal || (m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0)
					{
						if (method is ConstructorBuilder)
						{
							AttributeHelper.SetModifiers((ConstructorBuilder)method, m.Modifiers, m.IsInternal);
						}
						else
						{
							AttributeHelper.SetModifiers((MethodBuilder)method, m.Modifiers, m.IsInternal);
						}
					}
					if ((m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0
						&& (m.IsPublic || m.IsProtected)
						&& wrapper.IsPublic
						&& !IsAccessBridge(classFile, m))
					{
						if (method is ConstructorBuilder)
						{
							AttributeHelper.SetEditorBrowsableNever((ConstructorBuilder)method);
						}
						else
						{
							AttributeHelper.SetEditorBrowsableNever((MethodBuilder)method);
						}
						// TODO on WHIDBEY apply CompilerGeneratedAttribute
					}
					if (m.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(method);
					}
					if (setNameSig)
					{
						AttributeHelper.SetNameSig(method, m.Name, m.Signature);
					}
					if (m.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(method, m.GenericSignature);
					}
#else // STATIC_COMPILER
					if (setModifiers)
					{
						// shut up the compiler
					}
#endif // STATIC_COMPILER
					return method;
				}
				finally
				{
					Profiler.Leave("JavaTypeImpl.GenerateMethod");
				}
			}

#if STATIC_COMPILER
			// The classic example of an access bridge is StringBuilder.length(), the JDK 6 compiler
			// generates this to work around a reflection problem (which otherwise wouldn't surface the
			// length() method, because it is defined in the non-public base class AbstractStringBuilder.)
			private static bool IsAccessBridge(ClassFile classFile, ClassFile.Method m)
			{
				// HACK this is a pretty gross hack
				// We look at the method body to figure out if the bridge method calls another method with the exact
				// same name/signature and if that is the case, we assume that it is an access bridge.
				// This code is based on the javac algorithm in addBridgeIfNeeded(...) in com/sun/tools/javac/comp/TransTypes.java.
				if ((m.Modifiers & (Modifiers.Abstract | Modifiers.Native | Modifiers.Public | Modifiers.Bridge)) == (Modifiers.Public | Modifiers.Bridge))
				{
					foreach (ClassFile.Method.Instruction instr in m.Instructions)
					{
						if (instr.NormalizedOpCode == NormalizedByteCode.__invokespecial)
						{
							ClassFile.ConstantPoolItemMI cpi = classFile.SafeGetMethodref(instr.Arg1);
							return cpi != null && cpi.Name == m.Name && cpi.Signature == m.Signature;
						}
					}
				}
				return false;
			}
#endif // STATIC_COMPILER

			internal static void GenerateUnloadableOverrideStub(DynamicTypeWrapper wrapper, TypeBuilder typeBuilder, MethodWrapper baseMethod, MethodInfo target, Type targetRet, Type[] targetArgs)
			{
				Debug.Assert(!baseMethod.HasCallerID);

				Type stubret = baseMethod.ReturnTypeForDefineMethod;
				Type[] stubargs = baseMethod.GetParametersForDefineMethod();
				string name = wrapper.GenerateUniqueMethodName(baseMethod.RealName + "/unloadablestub", baseMethod);
				MethodBuilder overrideStub = typeBuilder.DefineMethod(name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final, stubret, stubargs);
				AttributeHelper.HideFromJava(overrideStub);
				typeBuilder.DefineMethodOverride(overrideStub, (MethodInfo)baseMethod.GetMethod());
				CodeEmitter ilgen = CodeEmitter.Create(overrideStub);
				ilgen.Emit(OpCodes.Ldarg_0);
				for (int i = 0; i < targetArgs.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
					if (targetArgs[i] != stubargs[i])
					{
						ilgen.Emit(OpCodes.Castclass, targetArgs[i]);
					}
				}
				ilgen.Emit(OpCodes.Callvirt, target);
				if (targetRet != stubret)
				{
					ilgen.Emit(OpCodes.Castclass, stubret);
				}
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
			}

			internal override Type Type
			{
				get
				{
					return typeBuilder;
				}
			}

			internal override string GetGenericSignature()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string[] GetEnclosingMethod()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string GetGenericMethodSignature(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string GetGenericFieldSignature(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetDeclaredAnnotations()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object GetMethodDefaultValue(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetMethodAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[][] GetParameterAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetFieldAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override MethodInfo GetFinalizeMethod()
			{
				return finalizeMethod;
			}

#if STATIC_COMPILER
			internal override Annotation Annotation
			{
				get
				{
					return annotationBuilder;
				}
			}

			internal override Type EnumType
			{
				get
				{
					return enumBuilder;
				}
			}
#endif // STATIC_COMPILER
		}

		private sealed class Metadata
		{
			private string[][] genericMetaData;
			private object[][] annotations;

			private Metadata(string[][] genericMetaData, object[][] annotations)
			{
				this.genericMetaData = genericMetaData;
				this.annotations = annotations;
			}

			internal static Metadata Create(ClassFile classFile)
			{
				if (classFile.MajorVersion < 49)
				{
					return null;
				}
				string[][] genericMetaData = null;
				object[][] annotations = null;
				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					if (classFile.Methods[i].GenericSignature != null)
					{
						if (genericMetaData == null)
						{
							genericMetaData = new string[4][];
						}
						if (genericMetaData[0] == null)
						{
							genericMetaData[0] = new string[classFile.Methods.Length];
						}
						genericMetaData[0][i] = classFile.Methods[i].GenericSignature;
					}
					if (classFile.Methods[i].Annotations != null)
					{
						if (annotations == null)
						{
							annotations = new object[5][];
						}
						if (annotations[1] == null)
						{
							annotations[1] = new object[classFile.Methods.Length];
						}
						annotations[1][i] = classFile.Methods[i].Annotations;
					}
					if (classFile.Methods[i].ParameterAnnotations != null)
					{
						if (annotations == null)
						{
							annotations = new object[5][];
						}
						if (annotations[2] == null)
						{
							annotations[2] = new object[classFile.Methods.Length];
						}
						annotations[2][i] = classFile.Methods[i].ParameterAnnotations;
					}
					if (classFile.Methods[i].AnnotationDefault != null)
					{
						if (annotations == null)
						{
							annotations = new object[5][];
						}
						if (annotations[3] == null)
						{
							annotations[3] = new object[classFile.Methods.Length];
						}
						annotations[3][i] = classFile.Methods[i].AnnotationDefault;
					}
				}
				for (int i = 0; i < classFile.Fields.Length; i++)
				{
					if (classFile.Fields[i].GenericSignature != null)
					{
						if (genericMetaData == null)
						{
							genericMetaData = new string[4][];
						}
						if (genericMetaData[1] == null)
						{
							genericMetaData[1] = new string[classFile.Fields.Length];
						}
						genericMetaData[1][i] = classFile.Fields[i].GenericSignature;
					}
					if (classFile.Fields[i].Annotations != null)
					{
						if (annotations == null)
						{
							annotations = new object[5][];
						}
						if (annotations[4] == null)
						{
							annotations[4] = new object[classFile.Fields.Length][];
						}
						annotations[4][i] = classFile.Fields[i].Annotations;
					}
				}
				if (classFile.EnclosingMethod != null)
				{
					if (genericMetaData == null)
					{
						genericMetaData = new string[4][];
					}
					genericMetaData[2] = classFile.EnclosingMethod;
				}
				if (classFile.GenericSignature != null)
				{
					if (genericMetaData == null)
					{
						genericMetaData = new string[4][];
					}
					genericMetaData[3] = new string[] { classFile.GenericSignature };
				}
				if (classFile.Annotations != null)
				{
					if (annotations == null)
					{
						annotations = new object[5][];
					}
					annotations[0] = classFile.Annotations;
				}
				if (genericMetaData != null || annotations != null)
				{
					return new Metadata(genericMetaData, annotations);
				}
				return null;
			}

			internal static string GetGenericSignature(Metadata m)
			{
				if (m != null && m.genericMetaData != null && m.genericMetaData[3] != null)
				{
					return m.genericMetaData[3][0];
				}
				return null;
			}

			internal static string[] GetEnclosingMethod(Metadata m)
			{
				if (m != null && m.genericMetaData != null)
				{
					return m.genericMetaData[2];
				}
				return null;
			}

			internal static string GetGenericMethodSignature(Metadata m, int index)
			{
				if (m != null && m.genericMetaData != null && m.genericMetaData[0] != null)
				{
					return m.genericMetaData[0][index];
				}
				return null;
			}

			internal static string GetGenericFieldSignature(Metadata m, int index)
			{
				if (m != null && m.genericMetaData != null && m.genericMetaData[1] != null)
				{
					return m.genericMetaData[1][index];
				}
				return null;
			}

			internal static object[] GetAnnotations(Metadata m)
			{
				if (m != null && m.annotations != null)
				{
					return m.annotations[0];
				}
				return null;
			}

			internal static object[] GetMethodAnnotations(Metadata m, int index)
			{
				if (m != null && m.annotations != null && m.annotations[1] != null)
				{
					return (object[])m.annotations[1][index];
				}
				return null;
			}

			internal static object[][] GetMethodParameterAnnotations(Metadata m, int index)
			{
				if (m != null && m.annotations != null && m.annotations[2] != null)
				{
					return (object[][])m.annotations[2][index];
				}
				return null;
			}

			internal static object GetMethodDefaultValue(Metadata m, int index)
			{
				if (m != null && m.annotations != null && m.annotations[3] != null)
				{
					return m.annotations[3][index];
				}
				return null;
			}

			// note that unlike GetGenericFieldSignature, the index is simply the field index 
			internal static object[] GetFieldAnnotations(Metadata m, int index)
			{
				if (m != null && m.annotations != null && m.annotations[4] != null)
				{
					return (object[])m.annotations[4][index];
				}
				return null;
			}
		}

		private sealed class FinishedTypeImpl : DynamicImpl
		{
			private Type type;
			private TypeWrapper[] innerclasses;
			private TypeWrapper declaringTypeWrapper;
			private Modifiers reflectiveModifiers;
			private MethodInfo clinitMethod;
			private MethodInfo finalizeMethod;
			private Metadata metadata;
#if STATIC_COMPILER
			private Annotation annotationBuilder;
			private TypeBuilder enumBuilder;
#endif

			internal FinishedTypeImpl(Type type, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers, Metadata metadata, MethodInfo clinitMethod, MethodInfo finalizeMethod
#if STATIC_COMPILER
, Annotation annotationBuilder
				, TypeBuilder enumBuilder
#endif
)
			{
				this.type = type;
				this.innerclasses = innerclasses;
				this.declaringTypeWrapper = declaringTypeWrapper;
				this.reflectiveModifiers = reflectiveModifiers;
				this.clinitMethod = clinitMethod;
				this.finalizeMethod = finalizeMethod;
				this.metadata = metadata;
#if STATIC_COMPILER
				this.annotationBuilder = annotationBuilder;
				this.enumBuilder = enumBuilder;
#endif
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					// TODO compute the innerclasses lazily (and fix JavaTypeImpl to not always compute them)
					return innerclasses;
				}
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					// TODO compute lazily (and fix JavaTypeImpl to not always compute it)
					return declaringTypeWrapper;
				}
			}

			internal override Modifiers ReflectiveModifiers
			{
				get
				{
					return reflectiveModifiers;
				}
			}

			internal override Type Type
			{
				get
				{
					return type;
				}
			}

			internal override void EmitRunClassConstructor(CodeEmitter ilgen)
			{
				if (clinitMethod != null)
				{
					ilgen.Emit(OpCodes.Call, clinitMethod);
				}
			}

			internal override DynamicImpl Finish()
			{
				return this;
			}

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				// we should never be called, because all methods on a finished type are already linked
				Debug.Assert(false);
				return mw.GetMethod();
			}

			internal override FieldInfo LinkField(FieldWrapper fw)
			{
				// we should never be called, because all fields on a finished type are already linked
				Debug.Assert(false);
				return fw.GetField();
			}

			internal override string GetGenericSignature()
			{
				return Metadata.GetGenericSignature(metadata);
			}

			internal override string[] GetEnclosingMethod()
			{
				return Metadata.GetEnclosingMethod(metadata);
			}

			internal override string GetGenericMethodSignature(int index)
			{
				return Metadata.GetGenericMethodSignature(metadata, index);
			}

			internal override string GetGenericFieldSignature(int index)
			{
				return Metadata.GetGenericFieldSignature(metadata, index);
			}

			internal override object[] GetDeclaredAnnotations()
			{
				return Metadata.GetAnnotations(metadata);
			}

			internal override object GetMethodDefaultValue(int index)
			{
				return Metadata.GetMethodDefaultValue(metadata, index);
			}

			internal override object[] GetMethodAnnotations(int index)
			{
				return Metadata.GetMethodAnnotations(metadata, index);
			}

			internal override object[][] GetParameterAnnotations(int index)
			{
				return Metadata.GetMethodParameterAnnotations(metadata, index);
			}

			internal override object[] GetFieldAnnotations(int index)
			{
				return Metadata.GetFieldAnnotations(metadata, index);
			}

			internal override MethodInfo GetFinalizeMethod()
			{
				return finalizeMethod;
			}

#if STATIC_COMPILER
			internal override Annotation Annotation
			{
				get
				{
					return annotationBuilder;
				}
			}

			internal override Type EnumType
			{
				get
				{
					return enumBuilder;
				}
			}
#endif // STATIC_COMPILER
		}

		internal sealed class FinishContext
		{
			private readonly ClassFile classFile;
			private readonly DynamicTypeWrapper wrapper;
			private readonly TypeBuilder typeBuilder;
			private TypeBuilder typeCallerID;
			private MethodInfo callerIDMethod;
			private List<System.Threading.ThreadStart> postFinishProcs;

			internal FinishContext(ClassFile classFile, DynamicTypeWrapper wrapper, TypeBuilder typeBuilder)
			{
				this.classFile = classFile;
				this.wrapper = wrapper;
				this.typeBuilder = typeBuilder;
			}

			internal void EmitCallerID(CodeEmitter ilgen)
			{
				if (callerIDMethod == null)
				{
					CreateGetCallerID();
				}
				ilgen.Emit(OpCodes.Call, callerIDMethod);
			}

			private void CreateGetCallerID()
			{
				TypeWrapper tw = CoreClasses.ikvm.@internal.CallerID.Wrapper;
				FieldBuilder callerIDField = typeBuilder.DefineField("__<callerID>", tw.TypeAsSignatureType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.SpecialName);
				MethodBuilder mb = typeBuilder.DefineMethod("__<GetCallerID>", MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName, tw.TypeAsSignatureType, Type.EmptyTypes);
				callerIDMethod = mb;
				CodeEmitter ilgen = CodeEmitter.Create(mb);
				ilgen.Emit(OpCodes.Ldsfld, callerIDField);
				CodeEmitterLabel done = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue_S, done);
				EmitCallerIDInitialization(ilgen, callerIDField);
				ilgen.MarkLabel(done);
				ilgen.Emit(OpCodes.Ldsfld, callerIDField);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
			}

			internal void RegisterPostFinishProc(System.Threading.ThreadStart proc)
			{
				if (postFinishProcs == null)
				{
					postFinishProcs = new List<System.Threading.ThreadStart>();
				}
				postFinishProcs.Add(proc);
			}

			internal Type FinishImpl()
			{
				MethodWrapper[] methods = wrapper.GetMethods();
				FieldWrapper[] fields = wrapper.GetFields();
#if STATIC_COMPILER
				wrapper.FinishGhost(typeBuilder, methods);
#endif // STATIC_COMPILER
				// if we're not abstract make sure we don't inherit any abstract methods
				if (!wrapper.IsAbstract)
				{
					TypeWrapper parent = wrapper.BaseTypeWrapper;
					// if parent is not abstract, the .NET implementation will never have abstract methods (only
					// stubs that throw AbstractMethodError)
					// NOTE interfaces are supposed to be abstract, but the VM doesn't enforce this, so
					// we have to check for a null parent (interfaces have no parent).
					while (parent != null && parent.IsAbstract)
					{
						foreach (MethodWrapper mw in parent.GetMethods())
						{
							MethodInfo mi = mw.GetMethod() as MethodInfo;
							if (mi != null && mi.IsAbstract && !mi.DeclaringType.IsInterface)
							{
								bool needStub = false;
								bool needRename = false;
								if (mw.IsPublic || mw.IsProtected)
								{
									MethodWrapper fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
									while (fmw != mw && (fmw.IsStatic || fmw.IsPrivate))
									{
										needRename = true;
										fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
									}
									if (fmw == mw && fmw.DeclaringType != wrapper)
									{
										needStub = true;
									}
								}
								else
								{
									MethodWrapper fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
									while (fmw != mw && (fmw.IsStatic || fmw.IsPrivate || !(mw.DeclaringType.IsPackageAccessibleFrom(fmw.DeclaringType) || (mw.IsInternal && mw.DeclaringType.InternalsVisibleTo(fmw.DeclaringType)))))
									{
										needRename = true;
										fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
									}
									if (fmw == mw && fmw.DeclaringType != wrapper)
									{
										needStub = true;
									}
								}
								if (needStub)
								{
									// NOTE in Sun's JRE 1.4.1 this method cannot be overridden by subclasses,
									// but I think this is a bug, so we'll support it anyway.
									string name = mi.Name;
									MethodAttributes attr = mi.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.NewSlot);
									if (needRename)
									{
										name = "__<>" + name + "/" + mi.DeclaringType.FullName;
										attr = MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot;
									}
									MethodBuilder mb = typeBuilder.DefineMethod(name, attr, CallingConventions.Standard, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
									if (needRename)
									{
										typeBuilder.DefineMethodOverride(mb, mi);
									}
									AttributeHelper.HideFromJava(mb);
									CodeEmitter ilgen = CodeEmitter.Create(mb);
									ilgen.EmitThrow("java.lang.AbstractMethodError", mw.DeclaringType.Name + "." + mw.Name + mw.Signature);
									ilgen.DoEmit();
								}
							}
						}
						parent = parent.BaseTypeWrapper;
					}
				}
				Dictionary<MethodKey, MethodInfo> invokespecialstubcache = new Dictionary<MethodKey, MethodInfo>();
				bool basehasclinit = wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasStaticInitializer;
				int clinitIndex = -1;
				bool hasConstructor = false;
				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBase mb = methods[i].GetMethod();
					if (mb == null)
					{
						// method doesn't really exist (e.g. delegate constructor or <clinit> that is optimized away)
						if (m.Name == StringConstants.INIT)
						{
							hasConstructor = true;
						}
					}
					else if (mb is ConstructorBuilder)
					{
						if (m.IsClassInitializer)
						{
							// we handle the <clinit> after we've done the other methods,
							// to make it easier to inject code needed by the other methods
							clinitIndex = i;
							continue;
						}
						else
						{
							hasConstructor = true;
						}
						CodeEmitter ilGenerator = CodeEmitter.Create((ConstructorBuilder)mb);
						CompileConstructorBody(this, ilGenerator, i, invokespecialstubcache);
					}
					else
					{
						if (m.IsAbstract)
						{
							bool stub = false;
							if (!classFile.IsAbstract)
							{
								// NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
								// the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
								stub = true;
							}
							else if (classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
							{
								// We have an abstract package accessible method in our public class. To allow a class in another
								// assembly to subclass this class, we must fake the abstractness of this method.
								stub = true;
							}
							if (stub)
							{
								CodeEmitter ilGenerator = CodeEmitter.Create((MethodBuilder)mb);
								TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
								ilGenerator.EmitThrow("java.lang.AbstractMethodError", classFile.Name + "." + m.Name + m.Signature);
								ilGenerator.DoEmit();
							}
						}
						else if (m.IsNative)
						{
							if ((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
							{
								continue;
							}
							if (wrapper.IsDelegate)
							{
								((MethodBuilder)mb).SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Runtime);
								continue;
							}
							Profiler.Enter("JavaTypeImpl.Finish.Native");
							try
							{
								CodeEmitter ilGenerator = CodeEmitter.Create((MethodBuilder)mb);
								TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
								// do we have a native implementation in map.xml?
								if (wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
								{
									ilGenerator.DoEmit();
									continue;
								}
#endif
								// see if there exists an IKVM.NativeCode class for this type
								Type nativeCodeType = null;
#if STATIC_COMPILER
								nativeCodeType = StaticCompiler.GetType(wrapper.GetClassLoader(), "IKVM.NativeCode." + classFile.Name.Replace('$', '+'));
#endif
								MethodInfo nativeMethod = null;
								TypeWrapper[] args = methods[i].GetParameters();
								if (nativeCodeType != null)
								{
									TypeWrapper[] nargs = args;
									if (!m.IsStatic)
									{
										nargs = new TypeWrapper[args.Length + 1];
										args.CopyTo(nargs, 1);
										nargs[0] = this.wrapper;
									}
									MethodInfo[] nativeCodeTypeMethods = nativeCodeType.GetMethods(BindingFlags.Static | BindingFlags.Public);
									foreach (MethodInfo method in nativeCodeTypeMethods)
									{
										ParameterInfo[] param = method.GetParameters();
										TypeWrapper[] match = new TypeWrapper[param.Length];
										for (int j = 0; j < param.Length; j++)
										{
											match[j] = ClassLoaderWrapper.GetWrapperFromType(param[j].ParameterType);
										}
										if (m.Name == method.Name && IsCompatibleArgList(nargs, match))
										{
											// TODO instead of taking the first matching method, we should find the best one
											nativeMethod = method;
											break;
										}
									}
								}
								if (nativeMethod != null)
								{
									int add = 0;
									if (!m.IsStatic)
									{
										ilGenerator.Emit(OpCodes.Ldarg_0);
										add = 1;
									}
									for (int j = 0; j < args.Length; j++)
									{
										ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
									}
									ilGenerator.Emit(OpCodes.Call, nativeMethod);
									TypeWrapper retTypeWrapper = methods[i].ReturnType;
									if (!retTypeWrapper.TypeAsTBD.Equals(nativeMethod.ReturnType) && !retTypeWrapper.IsGhost)
									{
										ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
									}
									ilGenerator.Emit(OpCodes.Ret);
								}
								else
								{
									if (wrapper.classLoader.NoJNI)
									{
										// since NoJniStubs can only be set when we're statically compiling, it is safe to use the "compiler" trace switch
										Tracer.Warning(Tracer.Compiler, "Native method not implemented: {0}.{1}.{2}", classFile.Name, m.Name, m.Signature);
										ilGenerator.EmitThrow("java.lang.UnsatisfiedLinkError", "Native method not implemented (compiled with -nojni): " + classFile.Name + "." + m.Name + m.Signature);
									}
#if STATIC_COMPILER
									else if (StaticCompiler.runtimeJniAssembly == null)
									{
										Console.Error.WriteLine("Error: Native method not implemented: {0}.{1}{2}", classFile.Name, m.Name, m.Signature);
										Environment.Exit(1);
									}
#endif
									else
									{
										if (JVM.IsSaveDebugImage)
										{
#if !STATIC_COMPILER
											JniProxyBuilder.Generate(this, ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args);
#endif // !STATIC_COMPILER
										}
										else
										{
											JniBuilder.Generate(this, ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args, false);
										}
									}
								}
								ilGenerator.DoEmit();
							}
							finally
							{
								Profiler.Leave("JavaTypeImpl.Finish.Native");
							}
						}
						else
						{
							MethodBuilder mbld = (MethodBuilder)mb;
							CodeEmitter ilGenerator = CodeEmitter.Create(mbld);
							TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
							if (wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
							{
								ilGenerator.DoEmit();
								continue;
							}
#endif // STATIC_COMPILER
							bool nonleaf = false;
							Compiler.Compile(this, wrapper, methods[i], classFile, m, ilGenerator, ref nonleaf, invokespecialstubcache);
							ilGenerator.CheckLabels();
							ilGenerator.DoEmit();
							if (nonleaf)
							{
								mbld.SetImplementationFlags(mbld.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
							}
#if STATIC_COMPILER
							ilGenerator.EmitLineNumberTable(methods[i].GetMethod());
#else // STATIC_COMPILER
							byte[] linenumbers = ilGenerator.GetLineNumberTable();
							if (linenumbers != null)
							{
								if (wrapper.lineNumberTables == null)
								{
									wrapper.lineNumberTables = new byte[methods.Length][];
								}
								wrapper.lineNumberTables[i] = linenumbers;
							}
#endif // STATIC_COMPILER
						}
					}
				}

				if (clinitIndex != -1 || (basehasclinit && !classFile.IsInterface) || classFile.HasInitializedFields)
				{
					ConstructorBuilder cb;
					if (clinitIndex != -1)
					{
						cb = (ConstructorBuilder)methods[clinitIndex].GetMethod();
					}
					else
					{
						cb = JavaTypeImpl.DefineClassInitializer(typeBuilder);
						AttributeHelper.HideFromJava(cb);
					}
					CodeEmitter ilGenerator = CodeEmitter.Create(cb);
					// before we call the base class initializer, we need to set the non-final static ConstantValue fields
					EmitConstantValueInitialization(fields, ilGenerator);
					if (basehasclinit)
					{
						wrapper.BaseTypeWrapper.EmitRunClassConstructor(ilGenerator);
					}
					if (clinitIndex != -1)
					{
						CompileConstructorBody(this, ilGenerator, clinitIndex, invokespecialstubcache);
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ret);
						ilGenerator.DoEmit();
					}
					ilGenerator.CheckLabels();
				}

				// add all interfaces that we implement (including the magic ones) and handle ghost conversions
				ImplementInterfaces(wrapper.Interfaces, new List<TypeWrapper>());

				// NOTE non-final fields aren't allowed in interfaces so we don't have to initialize constant fields
				if (!classFile.IsInterface)
				{
					// if a class has no constructor, we generate one otherwise Ref.Emit will create a default ctor
					// and that has several problems:
					// - base type may not have an accessible default constructor
					// - Ref.Emit uses BaseType.GetConstructors() which may trigger a TypeResolve event
					// - we don't want the synthesized constructor to show up in Java
					if (!hasConstructor)
					{
						ConstructorBuilder cb = typeBuilder.DefineConstructor(MethodAttributes.PrivateScope, CallingConventions.Standard, Type.EmptyTypes);
						CodeEmitter ilgen = CodeEmitter.Create(cb);
						ilgen.Emit(OpCodes.Ldnull);
						ilgen.Emit(OpCodes.Throw);
						ilgen.DoEmit();
					}

					// here we loop thru all the interfaces to explicitly implement any methods that we inherit from
					// base types that may have a different name from the name in the interface
					// (e.g. interface that has an equals() method that should override System.Object.Equals())
					// also deals with interface methods that aren't implemented (generate a stub that throws AbstractMethodError)
					// and with methods that aren't public (generate a stub that throws IllegalAccessError)
					Dictionary<TypeWrapper, TypeWrapper> doneSet = new Dictionary<TypeWrapper, TypeWrapper>();
					TypeWrapper[] interfaces = wrapper.Interfaces;
					for (int i = 0; i < interfaces.Length; i++)
					{
						ImplementInterfaceMethodStubs(doneSet, interfaces[i], false);
					}
					// if any of our base classes has an incomplete interface implementation we need to look through all
					// the base class interfaces to see if we've got an implementation now
					TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
					while (baseTypeWrapper.HasIncompleteInterfaceImplementation)
					{
						for (int i = 0; i < baseTypeWrapper.Interfaces.Length; i++)
						{
							ImplementInterfaceMethodStubs(doneSet, baseTypeWrapper.Interfaces[i], true);
						}
						baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
					}
					if (!wrapper.IsAbstract && wrapper.HasUnsupportedAbstractMethods)
					{
						AddUnsupportedAbstractMethods();
					}
					wrapper.automagicSerializationCtor = Serialization.AddAutomagicSerialization(wrapper);
				}

#if STATIC_COMPILER
				// If we're an interface that has public/protected fields, we create an inner class
				// to expose these fields to C# (which stubbornly refuses to see fields in interfaces).
				TypeBuilder tbFields = null;
				if (classFile.IsInterface && classFile.IsPublic && !wrapper.IsGhost && classFile.Fields.Length > 0)
				{
					CompilerClassLoader ccl = (CompilerClassLoader)wrapper.GetClassLoader();
					string name = "__Fields";
					while (!ccl.ReserveName(classFile.Name + "$" + name))
					{
						name += "_";
					}
					tbFields = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.Abstract);
					AttributeHelper.HideFromJava(tbFields);
					CodeEmitter ilgenClinit = null;
					for (int i = 0; i < classFile.Fields.Length; i++)
					{
						ClassFile.Field f = classFile.Fields[i];
						if (f.ConstantValue != null)
						{
							FieldAttributes attribs = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;
							FieldBuilder fb = tbFields.DefineField(f.Name, fields[i].FieldTypeWrapper.TypeAsSignatureType, attribs);
							fb.SetConstant(f.ConstantValue);
						}
						else
						{
							FieldAttributes attribs = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly;
							FieldBuilder fb = tbFields.DefineField(f.Name, fields[i].FieldTypeWrapper.TypeAsSignatureType, attribs);
							if (ilgenClinit == null)
							{
								ilgenClinit = CodeEmitter.Create(tbFields.DefineTypeInitializer());
							}
							wrapper.GetFieldWrapper(f.Name, f.Signature).EmitGet(ilgenClinit);
							ilgenClinit.Emit(OpCodes.Stsfld, fb);
						}
					}
					if (ilgenClinit != null)
					{
						ilgenClinit.Emit(OpCodes.Ret);
						ilgenClinit.DoEmit();
					}
				}

				// See if there is any additional metadata
				wrapper.EmitMapXmlMetadata(typeBuilder, classFile, fields, methods);

				// if we have public fields that have non-public field types, we need access stubs
				if (wrapper.IsPublic)
				{
					AddType1FieldAccessStubs(wrapper);
					AddType2FieldAccessStubs();
				}
#endif // STATIC_COMPILER

				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBase mb = methods[i].GetMethod();
					if (mb == null)
					{
						continue;
					}
					ParameterBuilder returnParameter = null;
					ParameterBuilder[] parameterBuilders = null;
					string[] parameterNames = null;
					if (wrapper.GetClassLoader().EmitDebugInfo
#if STATIC_COMPILER
 || (classFile.IsPublic && (m.IsPublic || m.IsProtected))
#endif
)
					{
						parameterNames = new string[methods[i].GetParameters().Length];
						GetParameterNamesFromLVT(m, parameterNames);
						GetParameterNamesFromSig(m.Signature, parameterNames);
#if STATIC_COMPILER
						((AotTypeWrapper)wrapper).GetParameterNamesFromXml(m.Name, m.Signature, parameterNames);
#endif
						parameterBuilders = GetParameterBuilders(mb, parameterNames.Length, parameterNames);
					}
#if STATIC_COMPILER
					if ((m.Modifiers & Modifiers.VarArgs) != 0 && !methods[i].HasCallerID)
					{
						if (parameterBuilders == null)
						{
							parameterBuilders = GetParameterBuilders(mb, methods[i].GetParameters().Length, null);
						}
						if (parameterBuilders.Length > 0)
						{
							AttributeHelper.SetParamArrayAttribute(parameterBuilders[parameterBuilders.Length - 1]);
						}
					}
					((AotTypeWrapper)wrapper).AddXmlMapParameterAttributes(mb, classFile.Name, m.Name, m.Signature, ref parameterBuilders);
#endif
					ConstructorBuilder cb = mb as ConstructorBuilder;
					MethodBuilder mBuilder = mb as MethodBuilder;
					if (m.Annotations != null)
					{
						foreach (object[] def in m.Annotations)
						{
							Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
							if (annotation != null)
							{
								if (cb != null)
								{
									annotation.Apply(wrapper.GetClassLoader(), cb, def);
								}
								if (mBuilder != null)
								{
									annotation.Apply(wrapper.GetClassLoader(), mBuilder, def);
									annotation.ApplyReturnValue(wrapper.GetClassLoader(), mBuilder, ref returnParameter, def);
								}
							}
						}
					}
					if (m.ParameterAnnotations != null)
					{
						if (parameterBuilders == null)
						{
							parameterBuilders = GetParameterBuilders(mb, methods[i].GetParameters().Length, null);
						}
						object[][] defs = m.ParameterAnnotations;
						for (int j = 0; j < defs.Length; j++)
						{
							foreach (object[] def in defs[j])
							{
								Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
								if (annotation != null)
								{
									annotation.Apply(wrapper.GetClassLoader(), parameterBuilders[j], def);
								}
							}
						}
					}
#if STATIC_COMPILER
					if (methods[i].HasCallerID)
					{
						AttributeHelper.SetEditorBrowsableNever((MethodBuilder)mb);
						EmitCallerIDStub(methods[i], parameterNames);
					}
#endif // STATIC_COMPILER
				}

				for (int i = 0; i < classFile.Fields.Length; i++)
				{
					if (classFile.Fields[i].Annotations != null)
					{
						foreach (object[] def in classFile.Fields[i].Annotations)
						{
							Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
							if (annotation != null)
							{
								GetterFieldWrapper getter = fields[i] as GetterFieldWrapper;
								if (getter != null)
								{
									annotation.Apply(wrapper.GetClassLoader(), (MethodBuilder)getter.GetGetter(), def);
								}
								else
								{
									DynamicPropertyFieldWrapper prop = fields[i] as DynamicPropertyFieldWrapper;
									if (prop != null)
									{
										annotation.Apply(wrapper.GetClassLoader(), prop.GetPropertyBuilder(), def);
									}
									else
									{
										annotation.Apply(wrapper.GetClassLoader(), (FieldBuilder)fields[i].GetField(), def);
									}
								}
							}
						}
					}
				}

				if (classFile.Annotations != null)
				{
					foreach (object[] def in classFile.Annotations)
					{
						Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
						if (annotation != null)
						{
							annotation.Apply(wrapper.GetClassLoader(), typeBuilder, def);
						}
					}
				}

				Type type;
				Profiler.Enter("TypeBuilder.CreateType");
				try
				{
					type = typeBuilder.CreateType();
					if (typeCallerID != null)
					{
						typeCallerID.CreateType();
					}
					if (postFinishProcs != null)
					{
						foreach (System.Threading.ThreadStart proc in postFinishProcs)
						{
							proc();
						}
					}
#if STATIC_COMPILER
					if (tbFields != null)
					{
						tbFields.CreateType();
					}
					if (classFile.IsInterface && !classFile.IsPublic)
					{
						((DynamicClassLoader)wrapper.classLoader.GetTypeWrapperFactory()).DefineProxyHelper(type);
					}
#endif
				}
				finally
				{
					Profiler.Leave("TypeBuilder.CreateType");
				}
#if !STATIC_COMPILER
				// When we're statically compiling we don't need to set the wrapper here, because we've already done so for the typeBuilder earlier.
				wrapper.GetClassLoader().SetWrapperForType(type, wrapper);
#endif
#if STATIC_COMPILER
				wrapper.FinishGhostStep2();
#endif
				BakedTypeCleanupHack.Process(wrapper);
				return type;
			}

#if STATIC_COMPILER
			private void AddType1FieldAccessStubs(TypeWrapper tw)
			{
				do
				{
					if (!tw.IsPublic)
					{
						foreach (FieldWrapper fw in tw.GetFields())
						{
							if ((fw.IsPublic || fw.IsProtected)
								&& wrapper.GetFieldWrapper(fw.Name, fw.Signature) == fw)
							{
								GenerateAccessStub(fw, true);
							}
						}
					}
					foreach (TypeWrapper iface in tw.Interfaces)
					{
						AddType1FieldAccessStubs(iface);
					}
					tw = tw.BaseTypeWrapper;
				} while (tw != null && !tw.IsPublic);
			}

			private void AddType2FieldAccessStubs()
			{
				foreach (FieldWrapper fw in wrapper.GetFields())
				{
					if ((fw.IsPublic || fw.IsProtected) && !fw.FieldTypeWrapper.IsPublic)
					{
						GenerateAccessStub(fw, false);
					}
				}
			}

			private void GenerateAccessStub(FieldWrapper fw, bool type1)
			{
				if (fw is ConstantFieldWrapper)
				{
					// constants cannot have a type 2 access stub, because constant types are always public
					Debug.Assert(type1);

					FieldAttributes attribs = fw.IsPublic ? FieldAttributes.Public : FieldAttributes.FamORAssem;
					attribs |= FieldAttributes.Static | FieldAttributes.Literal;
					FieldBuilder fb = typeBuilder.DefineField(fw.Name, fw.FieldTypeWrapper.TypeAsSignatureType, attribs);
					AttributeHelper.HideFromReflection(fb);
					fb.SetConstant(((ConstantFieldWrapper)fw).GetConstantValue());
				}
				else
				{
					string name = fw.Name;
					// If there is a potential for property name clashes (e.g. we have multiple fields with the same name in this class,
					// or the current class hides a field in the base class) we will mangle the name as a precaution. We could use a more
					// complicated scheme which would result in less mangling, but it is exceedingly unlikely to encounter a class with
					// these field name clashes, because Java cannot handle them either.
					foreach (FieldWrapper field in wrapper.GetFields())
					{
						if (field != fw && !field.IsPrivate && field.Name == name)
						{
							name = "<>" + fw.Name + fw.Signature;
							break;
						}
					}
					Type propType = fw.FieldTypeWrapper.GetPublicBaseTypeWrapper().TypeAsSignatureType;
					PropertyBuilder pb = typeBuilder.DefineProperty(name, PropertyAttributes.None, propType, Type.EmptyTypes);
					if (type1)
					{
						AttributeHelper.HideFromReflection(pb);
					}
					else
					{
						AttributeHelper.SetNameSig(pb, fw.Name, fw.Signature);
						AttributeHelper.SetModifiers(pb, fw.Modifiers, fw.IsInternal);
					}
					MethodAttributes attribs = fw.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
					attribs |= MethodAttributes.HideBySig;
					if (fw.IsStatic)
					{
						attribs |= MethodAttributes.Static;
					}
					MethodBuilder getter = typeBuilder.DefineMethod(wrapper.GenerateUniqueMethodName("get_" + fw.Name, propType, Type.EmptyTypes), attribs, propType, Type.EmptyTypes);
					AttributeHelper.HideFromJava(getter);
					pb.SetGetMethod(getter);
					CodeEmitter ilgen = CodeEmitter.Create(getter);
					if (!fw.IsStatic)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
					}
					fw.EmitGet(ilgen);
					ilgen.Emit(OpCodes.Ret);
					ilgen.DoEmit();
					if (!fw.IsFinal)
					{
						MethodBuilder setter = typeBuilder.DefineMethod(wrapper.GenerateUniqueMethodName("set_" + fw.Name, Types.Void, new Type[] { propType }), attribs, null, new Type[] { propType });
						AttributeHelper.HideFromJava(setter);
						pb.SetSetMethod(setter);
						ilgen = CodeEmitter.Create(setter);
						ilgen.Emit(OpCodes.Ldarg_0);
						if (!fw.IsStatic)
						{
							ilgen.Emit(OpCodes.Ldarg_1);
						}
						if (propType != fw.FieldTypeWrapper.TypeAsSignatureType)
						{
							ilgen.Emit(OpCodes.Castclass, fw.FieldTypeWrapper.TypeAsSignatureType);
						}
						fw.EmitSet(ilgen);
						ilgen.Emit(OpCodes.Ret);
						ilgen.DoEmit();
					}
				}
			}
#endif // STATIC_COMPILER

			private void ImplementInterfaceMethodStubs(Dictionary<TypeWrapper, TypeWrapper> doneSet, TypeWrapper interfaceTypeWrapper, bool baseClassInterface)
			{
				Debug.Assert(interfaceTypeWrapper.IsInterface);

				// make sure we don't do the same method twice
				if (doneSet.ContainsKey(interfaceTypeWrapper))
				{
					return;
				}
				doneSet.Add(interfaceTypeWrapper, interfaceTypeWrapper);
				foreach (MethodWrapper method in interfaceTypeWrapper.GetMethods())
				{
					if (!method.IsStatic && !method.IsDynamicOnly)
					{
						ImplementInterfaceMethodStubImpl(method, baseClassInterface);
					}
				}
				TypeWrapper[] interfaces = interfaceTypeWrapper.Interfaces;
				for (int i = 0; i < interfaces.Length; i++)
				{
					ImplementInterfaceMethodStubs(doneSet, interfaces[i], baseClassInterface);
				}
			}

			private void ImplementInterfaceMethodStubImpl(MethodWrapper ifmethod, bool baseClassInterface)
			{
				// we're mangling the name to prevent subclasses from accidentally overriding this method and to
				// prevent clashes with overloaded method stubs that are erased to the same signature (e.g. unloadable types and ghost arrays)
				// HACK the signature and name are the wrong way around to work around a C++/CLI bug (apparantely it looks looks at the last n
				// characters of the method name, or something bizarre like that)
				// https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=234167
				string mangledName = ifmethod.DeclaringType.Name + "/" + ifmethod.Signature + ifmethod.Name;
				MethodWrapper mce = null;
				TypeWrapper lookup = wrapper;
				while (lookup != null)
				{
					mce = lookup.GetMethodWrapper(ifmethod.Name, ifmethod.Signature, true);
					if (mce == null || !mce.IsStatic)
					{
						break;
					}
					lookup = mce.DeclaringType.BaseTypeWrapper;
				}
				if (mce != null)
				{
					Debug.Assert(!mce.HasCallerID);
					if (mce.DeclaringType != wrapper)
					{
						// check the loader constraints
						bool error = false;
						if (mce.ReturnType != ifmethod.ReturnType)
						{
							// TODO handle unloadable
							error = true;
						}
						TypeWrapper[] mceparams = mce.GetParameters();
						TypeWrapper[] ifparams = ifmethod.GetParameters();
						for (int i = 0; i < mceparams.Length; i++)
						{
							if (mceparams[i] != ifparams[i])
							{
								// TODO handle unloadable
								error = true;
								break;
							}
						}
						if (error)
						{
							MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
							AttributeHelper.HideFromJava(mb);
							CodeEmitter ilgen = CodeEmitter.Create(mb);
							ilgen.EmitThrow("java.lang.LinkageError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
							ilgen.DoEmit();
							typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
							return;
						}
					}
					if (mce.IsMirandaMethod && mce.DeclaringType == wrapper)
					{
						// Miranda methods already have a methodimpl (if needed) to implement the correct interface method
					}
					else if (!mce.IsPublic && !mce.IsInternal)
					{
						// NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
						// it makes sense, so I hope the spec is wrong
						// UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
						// methods. Sigh! So I have to use private methods and mangle the name
						MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilgen = CodeEmitter.Create(mb);
						ilgen.EmitThrow("java.lang.IllegalAccessError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
						ilgen.DoEmit();
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						wrapper.HasIncompleteInterfaceImplementation = true;
					}
					else if (mce.GetMethod() == null || mce.RealName != ifmethod.RealName || mce.IsInternal)
					{
						MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilGenerator = CodeEmitter.Create(mb);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						int argc = mce.GetParameters().Length;
						for (int n = 0; n < argc; n++)
						{
							ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
						}
						mce.EmitCallvirt(ilGenerator);
						ilGenerator.Emit(OpCodes.Ret);
						ilGenerator.DoEmit();
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
					}
					else if (!ReflectUtil.IsSameAssembly(mce.DeclaringType.TypeAsTBD, typeBuilder))
					{
						// NOTE methods inherited from base classes in a different assembly do *not* automatically implement
						// interface methods, so we have to generate a stub here that doesn't do anything but call the base
						// implementation
						MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilGenerator = CodeEmitter.Create(mb);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						int argc = mce.GetParameters().Length;
						for (int n = 0; n < argc; n++)
						{
							ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
						}
						mce.EmitCallvirt(ilGenerator);
						ilGenerator.Emit(OpCodes.Ret);
						ilGenerator.DoEmit();
					}
 					else if (CheckRequireOverrideStub(mce, ifmethod))
 					{
 						JavaTypeImpl.GenerateUnloadableOverrideStub(wrapper, typeBuilder, ifmethod, (MethodInfo)mce.GetMethod(), mce.ReturnTypeForDefineMethod, mce.GetParametersForDefineMethod());
 					}
 					else if (baseClassInterface && mce.DeclaringType == wrapper)
 					{
 						typeBuilder.DefineMethodOverride((MethodInfo)mce.GetMethod(), (MethodInfo)ifmethod.GetMethod());
 					}
				}
				else
				{
					if (!wrapper.IsAbstract || (!baseClassInterface && wrapper.GetMethodWrapper(ifmethod.Name, ifmethod.Signature, false) != null))
					{
						// the type doesn't implement the interface method and isn't abstract either. The JVM allows this, but the CLR doesn't,
						// so we have to create a stub method that throws an AbstractMethodError
						MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilgen = CodeEmitter.Create(mb);
						ilgen.EmitThrow("java.lang.AbstractMethodError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
						ilgen.DoEmit();
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						wrapper.HasIncompleteInterfaceImplementation = true;
					}
				}
			}

			private static class BakedTypeCleanupHack
			{
#if NET_4_0 || STATIC_COMPILER
				internal static void Process(DynamicTypeWrapper wrapper) { }
#else
				private static readonly FieldInfo m_methodBuilder = typeof(ConstructorBuilder).GetField("m_methodBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
				private static readonly FieldInfo[] methodBuilderFields = GetFieldList(typeof(MethodBuilder), new string[]
					{
						"m_ilGenerator",
						"m_ubBody",
						"m_RVAFixups",
						"m_mdMethodFixups",
						"m_localSignature",
						"m_localSymInfo",
						"m_exceptions",
						"m_parameterTypes",
						"m_retParam",
						"m_returnType",
						"m_signature"
					});
				private static readonly FieldInfo[] fieldBuilderFields = GetFieldList(typeof(FieldBuilder), new string[]
					{
						"m_data",
						"m_fieldType",
					});

				private static bool IsSupportedVersion
				{
					get
					{
						return Environment.Version.Major == 2 && Environment.Version.Minor == 0 && Environment.Version.Build == 50727 && Environment.Version.Revision == 4016;
					}
				}

				[SecurityCritical]
				[SecurityTreatAsSafe]
				private static FieldInfo[] GetFieldList(Type type, string[] list)
				{
					if (JVM.SafeGetEnvironmentVariable("IKVM_DISABLE_TYPEBUILDER_HACK") != null || !IsSupportedVersion)
					{
						return null;
					}
					if (!SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.Assertion)) ||
						!SecurityManager.IsGranted(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess)))
					{
						return null;
					}
					FieldInfo[] fields = new FieldInfo[list.Length];
					for (int i = 0; i < list.Length; i++)
					{
						fields[i] = type.GetField(list[i], BindingFlags.Instance | BindingFlags.NonPublic);
						if (fields[i] == null)
						{
							return null;
						}
					}
					return fields;
				}

				[SecurityCritical]
				[SecurityTreatAsSafe]
				internal static void Process(DynamicTypeWrapper wrapper)
				{
					if (m_methodBuilder != null && methodBuilderFields != null && fieldBuilderFields != null)
					{
						foreach (MethodWrapper mw in wrapper.GetMethods())
						{
							MethodBuilder mb = mw.GetMethod() as MethodBuilder;
							if (mb == null)
							{
								ConstructorBuilder cb = mw.GetMethod() as ConstructorBuilder;
								if (cb != null)
								{
									new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
									mb = (MethodBuilder)m_methodBuilder.GetValue(cb);
									CodeAccessPermission.RevertAssert();
								}
							}
							if (mb != null)
							{
								new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
								foreach (FieldInfo fi in methodBuilderFields)
								{
									fi.SetValue(mb, null);
								}
								CodeAccessPermission.RevertAssert();
							}
						}
						foreach (FieldWrapper fw in wrapper.GetFields())
						{
							FieldBuilder fb = fw.GetField() as FieldBuilder;
							if (fb != null)
							{
								new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
								foreach (FieldInfo fi in fieldBuilderFields)
								{
									fi.SetValue(fb, null);
								}
								CodeAccessPermission.RevertAssert();
							}
						}
					}
				}
#endif // NET_4_0
			}

#if !STATIC_COMPILER
			internal static class JniProxyBuilder
			{
				private static ModuleBuilder mod;
				private static int count;

				static JniProxyBuilder()
				{
					AssemblyName name = new AssemblyName();
					name.Name = "jniproxy";
					AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(name, JVM.IsSaveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
					DynamicClassLoader.RegisterForSaveDebug(ab);
					mod = ab.DefineDynamicModule("jniproxy.dll", "jniproxy.dll");
					CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(JavaModuleAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
					mod.SetCustomAttribute(cab);
				}

				internal static void Generate(DynamicTypeWrapper.FinishContext context, CodeEmitter ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args)
				{
					TypeBuilder tb = mod.DefineType("__<jni>" + (count++), TypeAttributes.Public | TypeAttributes.Class);
					int instance = m.IsStatic ? 0 : 1;
					Type[] argTypes = new Type[args.Length + instance + 1];
					if (instance != 0)
					{
						argTypes[0] = typeof(object);
					}
					for (int i = 0; i < args.Length; i++)
					{
						// NOTE we take a shortcut here by assuming that all "special" types (i.e. ghost or value types)
						// are public and so we can get away with replacing all other types with object.
						argTypes[i + instance] = (args[i].IsPrimitive || args[i].IsGhost || args[i].IsNonPrimitiveValueType) ? args[i].TypeAsSignatureType : typeof(object);
					}
					argTypes[argTypes.Length - 1] = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.CallerID").TypeAsSignatureType;
					Type retType = (mw.ReturnType.IsPrimitive || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType) ? mw.ReturnType.TypeAsSignatureType : typeof(object);
					MethodBuilder mb = tb.DefineMethod("method", MethodAttributes.Public | MethodAttributes.Static, retType, argTypes);
					AttributeHelper.HideFromJava(mb);
					CodeEmitter ilgen = CodeEmitter.Create(mb);
					JniBuilder.Generate(context, ilgen, wrapper, mw, tb, classFile, m, args, true);
					ilgen.DoEmit();
					tb.CreateType();
					for (int i = 0; i < argTypes.Length - 1; i++)
					{
						ilGenerator.Emit(OpCodes.Ldarg, (short)i);
					}
					context.EmitCallerID(ilGenerator);
					ilGenerator.Emit(OpCodes.Call, mb);
					if (!mw.ReturnType.IsPrimitive && !mw.ReturnType.IsGhost && !mw.ReturnType.IsNonPrimitiveValueType)
					{
						ilGenerator.Emit(OpCodes.Castclass, mw.ReturnType.TypeAsSignatureType);
					}
					ilGenerator.Emit(OpCodes.Ret);
				}
			}
#endif // !STATIC_COMPILER

			private static class JniBuilder
			{
#if STATIC_COMPILER
				private static readonly Type localRefStructType = StaticCompiler.GetRuntimeType("IKVM.Runtime.JNI+Frame");
#elif FIRST_PASS
				private static readonly Type localRefStructType = null;
#else
				private static readonly Type localRefStructType = JVM.LoadType(typeof(IKVM.Runtime.JNI.Frame));
#endif
				private static readonly MethodInfo jniFuncPtrMethod = localRefStructType.GetMethod("GetFuncPtr");
				private static readonly MethodInfo enterLocalRefStruct = localRefStructType.GetMethod("Enter");
				private static readonly MethodInfo leaveLocalRefStruct = localRefStructType.GetMethod("Leave");
				private static readonly MethodInfo makeLocalRef = localRefStructType.GetMethod("MakeLocalRef");
				private static readonly MethodInfo unwrapLocalRef = localRefStructType.GetMethod("UnwrapLocalRef");
				private static readonly MethodInfo writeLine = JVM.Import(typeof(Console)).GetMethod("WriteLine", new Type[] { Types.Object });
				private static readonly MethodInfo monitorEnter = JVM.Import(typeof(System.Threading.Monitor)).GetMethod("Enter", new Type[] { Types.Object });
				private static readonly MethodInfo monitorExit = JVM.Import(typeof(System.Threading.Monitor)).GetMethod("Exit", new Type[] { Types.Object });

				internal static void Generate(DynamicTypeWrapper.FinishContext context, CodeEmitter ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args, bool thruProxy)
				{
					CodeEmitterLocal syncObject = null;
					if (m.IsSynchronized && m.IsStatic)
					{
						wrapper.EmitClassLiteral(ilGenerator);
						ilGenerator.Emit(OpCodes.Dup);
						syncObject = ilGenerator.DeclareLocal(Types.Object);
						ilGenerator.Emit(OpCodes.Stloc, syncObject);
						ilGenerator.Emit(OpCodes.Call, monitorEnter);
						ilGenerator.BeginExceptionBlock();
					}
					string sig = m.Signature.Replace('.', '/');
					// TODO use/unify JNI.METHOD_PTR_FIELD_PREFIX
					FieldBuilder methodPtr = typeBuilder.DefineField("__<jniptr>" + m.Name + sig, Types.IntPtr, FieldAttributes.Static | FieldAttributes.PrivateScope);
					CodeEmitterLocal localRefStruct = ilGenerator.DeclareLocal(localRefStructType);
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					ilGenerator.Emit(OpCodes.Initobj, localRefStructType);
					ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
					CodeEmitterLabel oklabel = ilGenerator.DefineLabel();
					ilGenerator.Emit(OpCodes.Brtrue, oklabel);
					if (thruProxy)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(args.Length + (mw.IsStatic ? 0 : 1)));
					}
					else
					{
						context.EmitCallerID(ilGenerator);
					}
					ilGenerator.Emit(OpCodes.Ldstr, classFile.Name.Replace('.', '/'));
					ilGenerator.Emit(OpCodes.Ldstr, m.Name);
					ilGenerator.Emit(OpCodes.Ldstr, sig);
					ilGenerator.Emit(OpCodes.Call, jniFuncPtrMethod);
					ilGenerator.Emit(OpCodes.Stsfld, methodPtr);
					ilGenerator.MarkLabel(oklabel);
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					if (thruProxy)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(args.Length + (mw.IsStatic ? 0 : 1)));
					}
					else
					{
						context.EmitCallerID(ilGenerator);
					}
					ilGenerator.Emit(OpCodes.Call, enterLocalRefStruct);
					CodeEmitterLocal jnienv = ilGenerator.DeclareLocal(Types.IntPtr);
					ilGenerator.Emit(OpCodes.Stloc, jnienv);
					ilGenerator.BeginExceptionBlock();
					TypeWrapper retTypeWrapper = mw.ReturnType;
					if (!retTypeWrapper.IsUnloadable && !retTypeWrapper.IsPrimitive)
					{
						// this one is for use after we return from "calli"
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					}
					ilGenerator.Emit(OpCodes.Ldloc, jnienv);
					Type[] modargs = new Type[args.Length + 2];
					modargs[0] = Types.IntPtr;
					modargs[1] = Types.IntPtr;
					for (int i = 0; i < args.Length; i++)
					{
						modargs[i + 2] = args[i].TypeAsSignatureType;
					}
					int add = 0;
					if (!m.IsStatic)
					{
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						ilGenerator.Emit(OpCodes.Call, makeLocalRef);
						add = 1;
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
						wrapper.EmitClassLiteral(ilGenerator);
						ilGenerator.Emit(OpCodes.Call, makeLocalRef);
					}
					for (int j = 0; j < args.Length; j++)
					{
						if (args[j].IsUnloadable || !args[j].IsPrimitive)
						{
							ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							if (!args[j].IsUnloadable && args[j].IsNonPrimitiveValueType)
							{
								ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
								args[j].EmitBox(ilGenerator);
							}
							else if (!args[j].IsUnloadable && args[j].IsGhost)
							{
								ilGenerator.Emit(OpCodes.Ldarga_S, (byte)(j + add));
								ilGenerator.Emit(OpCodes.Ldfld, args[j].GhostRefField);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
							}
							ilGenerator.Emit(OpCodes.Call, makeLocalRef);
							modargs[j + 2] = Types.IntPtr;
						}
						else
						{
							ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
						}
					}
					ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
					Type realRetType;
					if (retTypeWrapper == PrimitiveTypeWrapper.BOOLEAN)
					{
						realRetType = Types.Byte;
					}
					else if (retTypeWrapper.IsPrimitive)
					{
						realRetType = retTypeWrapper.TypeAsSignatureType;
					}
					else
					{
						realRetType = Types.IntPtr;
					}
					ilGenerator.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, realRetType, modargs);
					CodeEmitterLocal retValue = null;
					if (retTypeWrapper != PrimitiveTypeWrapper.VOID)
					{
						if (!retTypeWrapper.IsUnloadable && !retTypeWrapper.IsPrimitive)
						{
							ilGenerator.Emit(OpCodes.Call, unwrapLocalRef);
							if (retTypeWrapper.IsNonPrimitiveValueType)
							{
								retTypeWrapper.EmitUnbox(ilGenerator);
							}
							else if (retTypeWrapper.IsGhost)
							{
								CodeEmitterLocal ghost = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
								CodeEmitterLocal obj = ilGenerator.DeclareLocal(Types.Object);
								ilGenerator.Emit(OpCodes.Stloc, obj);
								ilGenerator.Emit(OpCodes.Ldloca, ghost);
								ilGenerator.Emit(OpCodes.Ldloc, obj);
								ilGenerator.Emit(OpCodes.Stfld, retTypeWrapper.GhostRefField);
								ilGenerator.Emit(OpCodes.Ldloc, ghost);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
							}
						}
						retValue = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
						ilGenerator.Emit(OpCodes.Stloc, retValue);
					}
					CodeEmitterLabel retLabel = ilGenerator.DefineLabel();
					ilGenerator.Emit(OpCodes.Leave, retLabel);
					ilGenerator.BeginCatchBlock(Types.Object);
					ilGenerator.Emit(OpCodes.Ldstr, "*** exception in native code ***");
					ilGenerator.Emit(OpCodes.Call, writeLine);
					ilGenerator.Emit(OpCodes.Call, writeLine);
					ilGenerator.Emit(OpCodes.Rethrow);
					ilGenerator.BeginFinallyBlock();
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					ilGenerator.Emit(OpCodes.Call, leaveLocalRefStruct);
					ilGenerator.Emit(OpCodes.Endfinally);
					ilGenerator.EndExceptionBlock();
					if (m.IsSynchronized && m.IsStatic)
					{
						ilGenerator.BeginFinallyBlock();
						ilGenerator.Emit(OpCodes.Ldloc, syncObject);
						ilGenerator.Emit(OpCodes.Call, monitorExit);
						ilGenerator.Emit(OpCodes.Endfinally);
						ilGenerator.EndExceptionBlock();
					}
					ilGenerator.MarkLabel(retLabel);
					if (retTypeWrapper != PrimitiveTypeWrapper.VOID)
					{
						ilGenerator.Emit(OpCodes.Ldloc, retValue);
					}
					ilGenerator.Emit(OpCodes.Ret);
				}
			}

			private static class TraceHelper
			{
#if STATIC_COMPILER
				private readonly static MethodInfo methodIsTracedMethod = JVM.LoadType(typeof(Tracer)).GetMethod("IsTracedMethod");
#endif
				private readonly static MethodInfo methodMethodInfo = JVM.LoadType(typeof(Tracer)).GetMethod("MethodInfo");

				internal static void EmitMethodTrace(CodeEmitter ilgen, string tracemessage)
				{
					if (Tracer.IsTracedMethod(tracemessage))
					{
						CodeEmitterLabel label = ilgen.DefineLabel();
#if STATIC_COMPILER
						// TODO this should be a boolean field test instead of a call to Tracer.IsTracedMessage
						ilgen.Emit(OpCodes.Ldstr, tracemessage);
						ilgen.Emit(OpCodes.Call, methodIsTracedMethod);
						ilgen.Emit(OpCodes.Brfalse_S, label);
#endif
						ilgen.Emit(OpCodes.Ldstr, tracemessage);
						ilgen.Emit(OpCodes.Call, methodMethodInfo);
						ilgen.MarkLabel(label);
					}
				}
			}

#if STATIC_COMPILER
			private void EmitCallerIDStub(MethodWrapper mw, string[] parameterNames)
			{
				Type[] p = mw.GetParametersForDefineMethod();
				Type[] parameterTypes = new Type[p.Length - 1];
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					parameterTypes[i] = p[i];
				}
				MethodAttributes attribs = MethodAttributes.HideBySig;
				int argcount = parameterTypes.Length;
				if (mw.IsStatic)
				{
					attribs |= MethodAttributes.Static;
				}
				else
				{
					argcount++;
				}
				if (mw.IsPublic)
				{
					attribs |= MethodAttributes.Public;
				}
				else if (mw.IsProtected)
				{
					attribs |= MethodAttributes.FamORAssem;
				}
				else if (mw.IsPrivate)
				{
					attribs |= MethodAttributes.Private;
				}
				else
				{
					attribs |= MethodAttributes.Assembly;
				}
				MethodBuilder mb = typeBuilder.DefineMethod(mw.Name, attribs, mw.ReturnTypeForDefineMethod, parameterTypes);
				AttributeHelper.HideFromJava(mb);
				mb.SetImplementationFlags(MethodImplAttributes.NoInlining);
				CodeEmitter ilgen = CodeEmitter.Create(mb);
				for (int i = 0; i < argcount; i++)
				{
					if (parameterNames != null && (mw.IsStatic || i > 0))
					{
						ParameterBuilder pb = mb.DefineParameter(mw.IsStatic ? i + 1 : i, ParameterAttributes.None, parameterNames[mw.IsStatic ? i : i - 1]);
						if (i == argcount - 1 && (mw.Modifiers & Modifiers.VarArgs) != 0)
						{
							AttributeHelper.SetParamArrayAttribute(pb);
						}
					}
					ilgen.Emit(OpCodes.Ldarg, (short)i);
				}
				ilgen.Emit(OpCodes.Ldc_I4_1);
				ilgen.Emit(OpCodes.Ldc_I4_0);
				ilgen.Emit(OpCodes.Newobj, JVM.Import(typeof(StackFrame)).GetConstructor(new Type[] { Types.Int32, Types.Boolean }));
				MethodWrapper callerID = CoreClasses.ikvm.@internal.CallerID.Wrapper.GetMethodWrapper("create", "(Lcli.System.Diagnostics.StackFrame;)Likvm.internal.CallerID;", false);
				callerID.Link();
				callerID.EmitCall(ilgen);
				if (mw.IsStatic)
				{
					mw.EmitCall(ilgen);
				}
				else
				{
					mw.EmitCallvirt(ilgen);
				}
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
			}
#endif // STATIC_COMPILER

			private static bool CheckRequireOverrideStub(MethodWrapper mw1, MethodWrapper mw2)
			{
				// TODO this is too late to generate LinkageErrors so we need to figure this out earlier
				if (mw1.ReturnType != mw2.ReturnType && !(mw1.ReturnType.IsUnloadable && mw2.ReturnType.IsUnloadable))
				{
					return true;
				}
				TypeWrapper[] args1 = mw1.GetParameters();
				TypeWrapper[] args2 = mw2.GetParameters();
				for (int i = 0; i < args1.Length; i++)
				{
					if (args1[i] != args2[i] && !(args1[i].IsUnloadable && args2[i].IsUnloadable))
					{
						return true;
					}
				}
				return false;
			}

			private void ImplementInterfaces(TypeWrapper[] interfaces, List<TypeWrapper> interfaceList)
			{
				foreach (TypeWrapper iface in interfaces)
				{
					if (!interfaceList.Contains(iface))
					{
						interfaceList.Add(iface);
						// NOTE we're using TypeAsBaseType for the interfaces!
						Type ifaceType = iface.TypeAsBaseType;
						if (!iface.IsPublic && !ReflectUtil.IsSameAssembly(ifaceType, typeBuilder))
						{
							ifaceType = ReflectUtil.GetAssembly(ifaceType).GetType(DynamicClassLoader.GetProxyHelperName(ifaceType));
						}
						typeBuilder.AddInterfaceImplementation(ifaceType);
#if STATIC_COMPILER
						if (!wrapper.IsInterface)
						{
							// look for "magic" interfaces that imply a .NET interface
							if (iface.GetClassLoader() == CoreClasses.java.lang.Object.Wrapper.GetClassLoader())
							{
								if (iface.Name == "java.lang.Iterable"
									&& !wrapper.ImplementsInterface(ClassLoaderWrapper.GetWrapperFromType(JVM.Import(typeof(System.Collections.IEnumerable)))))
								{
									TypeWrapper enumeratorType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("ikvm.lang.IterableEnumerator");
									if (enumeratorType != null)
									{
										typeBuilder.AddInterfaceImplementation(JVM.Import(typeof(System.Collections.IEnumerable)));
										// FXBUG we're using the same method name as the C# compiler here because both the .NET and Mono implementations of Xml serialization depend on this method name
										MethodBuilder mb = typeBuilder.DefineMethod("System.Collections.IEnumerable.GetEnumerator", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName, JVM.Import(typeof(System.Collections.IEnumerator)), Type.EmptyTypes);
										AttributeHelper.HideFromJava(mb);
										typeBuilder.DefineMethodOverride(mb, JVM.Import(typeof(System.Collections.IEnumerable)).GetMethod("GetEnumerator"));
										CodeEmitter ilgen = CodeEmitter.Create(mb);
										ilgen.Emit(OpCodes.Ldarg_0);
										MethodWrapper mw = enumeratorType.GetMethodWrapper("<init>", "(Ljava.lang.Iterable;)V", false);
										mw.Link();
										mw.EmitNewobj(ilgen);
										ilgen.Emit(OpCodes.Ret);
										ilgen.DoEmit();
									}
								}
								else if (iface.Name == "java.io.Closeable"
									&& !wrapper.ImplementsInterface(ClassLoaderWrapper.GetWrapperFromType(JVM.Import(typeof(IDisposable)))))
								{
									typeBuilder.AddInterfaceImplementation(JVM.Import(typeof(IDisposable)));
									MethodBuilder mb = typeBuilder.DefineMethod("__<>Dispose", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName, Types.Void, Type.EmptyTypes);
									typeBuilder.DefineMethodOverride(mb, JVM.Import(typeof(IDisposable)).GetMethod("Dispose"));
									CodeEmitter ilgen = CodeEmitter.Create(mb);
									ilgen.Emit(OpCodes.Ldarg_0);
									MethodWrapper mw = iface.GetMethodWrapper("close", "()V", false);
									mw.Link();
									mw.EmitCallvirt(ilgen);
									ilgen.Emit(OpCodes.Ret);
									ilgen.DoEmit();
								}
							}
							// if we implement a ghost interface, add an implicit conversion to the ghost reference value type
							if (iface.IsGhost && wrapper.IsPublic)
							{
								MethodBuilder mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, iface.TypeAsSignatureType, new Type[] { wrapper.TypeAsSignatureType });
								AttributeHelper.HideFromJava(mb);
								CodeEmitter ilgen = CodeEmitter.Create(mb);
								CodeEmitterLocal local = ilgen.DeclareLocal(iface.TypeAsSignatureType);
								ilgen.Emit(OpCodes.Ldloca, local);
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Stfld, iface.GhostRefField);
								ilgen.Emit(OpCodes.Ldloca, local);
								ilgen.Emit(OpCodes.Ldobj, iface.TypeAsSignatureType);
								ilgen.Emit(OpCodes.Ret);
								ilgen.DoEmit();
							}
						}
#endif // STATIC_COMPILER
						// NOTE we're recursively "implementing" all interfaces that we inherit from the interfaces we implement.
						// The C# compiler also does this and the Compact Framework requires it.
						ImplementInterfaces(iface.Interfaces, interfaceList);
					}
				}
			}

			private void AddUnsupportedAbstractMethods()
			{
				foreach (MethodBase mb in wrapper.BaseTypeWrapper.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (DotNetTypeWrapper.IsUnsupportedAbstractMethod(mb))
					{
						GenerateUnsupportedAbstractMethodStub(mb);
					}
				}
				Dictionary<MethodBase, MethodBase> h = new Dictionary<MethodBase, MethodBase>();
				TypeWrapper tw = wrapper;
				while (tw != null)
				{
					foreach (TypeWrapper iface in tw.Interfaces)
					{
						foreach (MethodBase mb in iface.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
						{
							if (!h.ContainsKey(mb))
							{
								h.Add(mb, mb);
								if (DotNetTypeWrapper.IsUnsupportedAbstractMethod(mb))
								{
									GenerateUnsupportedAbstractMethodStub(mb);
								}
							}
						}
					}
					tw = tw.BaseTypeWrapper;
				}
			}

			private void GenerateUnsupportedAbstractMethodStub(MethodBase mb)
			{
				ParameterInfo[] parameters = mb.GetParameters();
				Type[] parameterTypes = new Type[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					parameterTypes[i] = parameters[i].ParameterType;
				}
				MethodAttributes attr = MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Private;
				MethodBuilder m = typeBuilder.DefineMethod("__<unsupported>" + mb.DeclaringType.FullName + "/" + mb.Name, attr, ((MethodInfo)mb).ReturnType, parameterTypes);
				CodeEmitter ilgen = CodeEmitter.Create(m);
				ilgen.EmitThrow("java.lang.AbstractMethodError", "Method " + mb.DeclaringType.FullName + "." + mb.Name + " is unsupported by IKVM.");
				ilgen.DoEmit();
				typeBuilder.DefineMethodOverride(m, (MethodInfo)mb);
			}

			private void CompileConstructorBody(FinishContext context, CodeEmitter ilGenerator, int methodIndex, Dictionary<MethodKey, MethodInfo> invokespecialstubcache)
			{
				MethodWrapper[] methods = wrapper.GetMethods();
				ClassFile.Method m = classFile.Methods[methodIndex];
				TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
				// do we have a native implementation in map.xml?
				if (wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
				{
					ilGenerator.DoEmit();
					return;
				}
#endif
				bool nonLeaf = false;
				Compiler.Compile(context, wrapper, methods[methodIndex], classFile, m, ilGenerator, ref nonLeaf, invokespecialstubcache);
				ilGenerator.DoEmit();
#if STATIC_COMPILER
				ilGenerator.EmitLineNumberTable(methods[methodIndex].GetMethod());
#else // STATIC_COMPILER
				byte[] linenumbers = ilGenerator.GetLineNumberTable();
				if (linenumbers != null)
				{
					if (wrapper.lineNumberTables == null)
					{
						wrapper.lineNumberTables = new byte[methods.Length][];
					}
					wrapper.lineNumberTables[methodIndex] = linenumbers;
				}
#endif // STATIC_COMPILER
			}

			private static bool IsCompatibleArgList(TypeWrapper[] caller, TypeWrapper[] callee)
			{
				if (caller.Length == callee.Length)
				{
					for (int i = 0; i < caller.Length; i++)
					{
						if (!caller[i].IsAssignableTo(callee[i]))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			private void EmitCallerIDInitialization(CodeEmitter ilGenerator, FieldInfo callerIDField)
			{
				{
					TypeWrapper tw = CoreClasses.ikvm.@internal.CallerID.Wrapper;
					// we need to prohibit this optimization at runtime, because proxy classes may be injected into the boot class loader,
					// but they don't actually have access to core library internals
#if STATIC_COMPILER
					if (tw.GetClassLoader() == wrapper.GetClassLoader())
					{
						MethodWrapper create = tw.GetMethodWrapper("create", "(Lcli.System.RuntimeTypeHandle;)Likvm.internal.CallerID;", false);
						ilGenerator.Emit(OpCodes.Ldtoken, this.typeBuilder);
						create.Link();
						create.EmitCall(ilGenerator);
					}
					else
#endif
					{
						typeCallerID = typeBuilder.DefineNestedType("__<CallerID>", TypeAttributes.Sealed | TypeAttributes.NestedPrivate, tw.TypeAsBaseType);
						ConstructorBuilder cb = typeCallerID.DefineConstructor(MethodAttributes.Assembly, CallingConventions.Standard, null);
						CodeEmitter ctorIlgen = CodeEmitter.Create(cb);
						ctorIlgen.Emit(OpCodes.Ldarg_0);
						MethodWrapper mw = tw.GetMethodWrapper("<init>", "()V", false);
						mw.Link();
						mw.EmitCall(ctorIlgen);
						ctorIlgen.Emit(OpCodes.Ret);
						ctorIlgen.DoEmit();
						ilGenerator.Emit(OpCodes.Newobj, cb);
					}
					ilGenerator.Emit(OpCodes.Stsfld, callerIDField);
				}
			}

			private void EmitConstantValueInitialization(FieldWrapper[] fields, CodeEmitter ilGenerator)
			{
				ClassFile.Field[] flds = classFile.Fields;
				for (int i = 0; i < flds.Length; i++)
				{
					ClassFile.Field f = flds[i];
					if (f.IsStatic && !f.IsFinal)
					{
						object constant = f.ConstantValue;
						if (constant != null)
						{
							if (constant is int)
							{
								ilGenerator.Emit_Ldc_I4((int)constant);
							}
							else if (constant is bool)
							{
								ilGenerator.Emit_Ldc_I4((bool)constant ? 1 : 0);
							}
							else if (constant is byte)
							{
								ilGenerator.Emit_Ldc_I4((byte)constant);
							}
							else if (constant is char)
							{
								ilGenerator.Emit_Ldc_I4((char)constant);
							}
							else if (constant is short)
							{
								ilGenerator.Emit_Ldc_I4((short)constant);
							}
							else if (constant is long)
							{
								ilGenerator.Emit(OpCodes.Ldc_I8, (long)constant);
							}
							else if (constant is double)
							{
								ilGenerator.Emit(OpCodes.Ldc_R8, (double)constant);
							}
							else if (constant is float)
							{
								ilGenerator.Emit(OpCodes.Ldc_R4, (float)constant);
							}
							else if (constant is string)
							{
								ilGenerator.Emit(OpCodes.Ldstr, (string)constant);
							}
							else
							{
								throw new InvalidOperationException();
							}
							fields[i].EmitSet(ilGenerator);
						}
					}
				}
			}
		}

		protected static void GetParameterNamesFromLVT(ClassFile.Method m, string[] parameterNames)
		{
			ClassFile.Method.LocalVariableTableEntry[] localVars = m.LocalVariableTableAttribute;
			if (localVars != null)
			{
				for (int i = m.IsStatic ? 0 : 1, pos = 0; i < m.ArgMap.Length; i++)
				{
					// skip double & long fillers
					if (m.ArgMap[i] != -1)
					{
						if (parameterNames[pos] == null)
						{
							for (int j = 0; j < localVars.Length; j++)
							{
								if (localVars[j].index == i)
								{
									parameterNames[pos] = localVars[j].name;
									break;
								}
							}
						}
						pos++;
					}
				}
			}
		}

		protected static void GetParameterNamesFromSig(string sig, string[] parameterNames)
		{
			List<string> names = new List<string>();
			for (int i = 1; sig[i] != ')'; i++)
			{
				if (sig[i] == 'L')
				{
					i++;
					int end = sig.IndexOf(';', i);
					names.Add(GetParameterName(sig.Substring(i, end - i)));
					i = end;
				}
				else if (sig[i] == '[')
				{
					while (sig[++i] == '[') ;
					if (sig[i] == 'L')
					{
						i++;
						int end = sig.IndexOf(';', i);
						names.Add(GetParameterName(sig.Substring(i, end - i)) + "arr");
						i = end;
					}
					else
					{
						switch (sig[i])
						{
							case 'B':
							case 'Z':
								names.Add("barr");
								break;
							case 'C':
								names.Add("charr");
								break;
							case 'S':
								names.Add("sarr");
								break;
							case 'I':
								names.Add("iarr");
								break;
							case 'J':
								names.Add("larr");
								break;
							case 'F':
								names.Add("farr");
								break;
							case 'D':
								names.Add("darr");
								break;
						}
					}
				}
				else
				{
					switch (sig[i])
					{
						case 'B':
						case 'Z':
							names.Add("b");
							break;
						case 'C':
							names.Add("ch");
							break;
						case 'S':
							names.Add("s");
							break;
						case 'I':
							names.Add("i");
							break;
						case 'J':
							names.Add("l");
							break;
						case 'F':
							names.Add("f");
							break;
						case 'D':
							names.Add("d");
							break;
					}
				}
			}
			for (int i = 0; i < parameterNames.Length; i++)
			{
				if (parameterNames[i] == null)
				{
					parameterNames[i] = (string)names[i];
				}
			}
		}

		protected static ParameterBuilder[] GetParameterBuilders(MethodBase mb, int parameterCount, string[] parameterNames)
		{
			ParameterBuilder[] parameterBuilders = new ParameterBuilder[parameterCount];
			Dictionary<string, int> clashes = null;
			for (int i = 0; i < parameterBuilders.Length; i++)
			{
				string name = null;
				if (parameterNames != null)
				{
					name = parameterNames[i];
					if (Array.IndexOf(parameterNames, name, i + 1) >= 0 || (clashes != null && clashes.ContainsKey(name)))
					{
						if (clashes == null)
						{
							clashes = new Dictionary<string, int>();
						}
						int clash = 1;
						if (clashes.ContainsKey(name))
						{
							clash = clashes[name] + 1;
						}
						clashes[name] = clash;
						name += clash;
					}
				}
				MethodBuilder mBuilder = mb as MethodBuilder;
				if (mBuilder != null)
				{
					parameterBuilders[i] = mBuilder.DefineParameter(i + 1, ParameterAttributes.None, name);
				}
				else
				{
					parameterBuilders[i] = ((ConstructorBuilder)mb).DefineParameter(i + 1, ParameterAttributes.None, name);
				}
			}
			return parameterBuilders;
		}

		private static string GetParameterName(string type)
		{
			if (type == "java.lang.String")
			{
				return "str";
			}
			else if (type == "java.lang.Object")
			{
				return "obj";
			}
			else
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				for (int i = type.LastIndexOf('.') + 1; i < type.Length; i++)
				{
					if (char.IsUpper(type, i))
					{
						sb.Append(char.ToLower(type[i]));
					}
				}
				return sb.ToString();
			}
		}

#if STATIC_COMPILER
		protected abstract void AddMapXmlFields(ref FieldWrapper[] fields);
		protected abstract bool EmitMapXmlMethodBody(CodeEmitter ilgen, ClassFile f, ClassFile.Method m);
		protected abstract void EmitMapXmlMetadata(TypeBuilder typeBuilder, ClassFile classFile, FieldWrapper[] fields, MethodWrapper[] methods);
		protected abstract MethodBuilder DefineGhostMethod(string name, MethodAttributes attribs, MethodWrapper mw);
		protected abstract void FinishGhost(TypeBuilder typeBuilder, MethodWrapper[] methods);
		protected abstract void FinishGhostStep2();
		protected abstract TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs);
#endif // STATIC_COMPILER

		protected virtual bool IsPInvokeMethod(ClassFile.Method m)
		{
#if CLASSGC
			// TODO PInvoke is not supported in RunAndCollect assemblies,
			if (JVM.classUnloading)
			{
				return false;
			}
#endif
			if (m.Annotations != null)
			{
				foreach (object[] annot in m.Annotations)
				{
					if ("Lcli/System/Runtime/InteropServices/DllImportAttribute$Annotation;".Equals(annot[1]))
					{
						return true;
					}
				}
			}
			return false;
		}

		internal override MethodBase LinkMethod(MethodWrapper mw)
		{
			mw.AssertLinked();
			return impl.LinkMethod(mw);
		}

		internal override FieldInfo LinkField(FieldWrapper fw)
		{
			fw.AssertLinked();
			return impl.LinkField(fw);
		}

		internal override void EmitRunClassConstructor(CodeEmitter ilgen)
		{
			impl.EmitRunClassConstructor(ilgen);
		}

		internal override string GetGenericSignature()
		{
			return impl.GetGenericSignature();
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					return impl.GetGenericMethodSignature(i);
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			FieldWrapper[] fields = GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i] == fw)
				{
					return impl.GetGenericFieldSignature(i);
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

#if !STATIC_COMPILER
		internal override string[] GetEnclosingMethod()
		{
			return impl.GetEnclosingMethod();
		}

		internal override string GetSourceFileName()
		{
			return sourceFileName;
		}

		private int GetMethodBaseToken(MethodBase mb)
		{
			ConstructorInfo ci = mb as ConstructorInfo;
			if (ci != null)
			{
				return classLoader.GetTypeWrapperFactory().ModuleBuilder.GetConstructorToken(ci).Token;
			}
			else
			{
				return classLoader.GetTypeWrapperFactory().ModuleBuilder.GetMethodToken((MethodInfo)mb).Token;
			}
		}

		internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
		{
			if (lineNumberTables != null)
			{
				int token = GetMethodBaseToken(mb);
				MethodWrapper[] methods = GetMethods();
				for (int i = 0; i < methods.Length; i++)
				{
					if (GetMethodBaseToken(methods[i].GetMethod()) == token)
					{
						if (lineNumberTables[i] != null)
						{
							return new LineNumberTableAttribute(lineNumberTables[i]).GetLineNumber(ilOffset);
						}
						break;
					}
				}
			}
			return -1;
		}

		internal override object[] GetDeclaredAnnotations()
		{
			object[] annotations = impl.GetDeclaredAnnotations();
			if (annotations != null)
			{
				object[] objs = new object[annotations.Length];
				for (int i = 0; i < annotations.Length; i++)
				{
					objs[i] = JVM.NewAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[i]);
				}
				return objs;
			}
			return null;
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					object[] annotations = impl.GetMethodAnnotations(i);
					if (annotations != null)
					{
						object[] objs = new object[annotations.Length];
						for (int j = 0; j < annotations.Length; j++)
						{
							objs[j] = JVM.NewAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j]);
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					object[][] annotations = impl.GetParameterAnnotations(i);
					if (annotations != null)
					{
						object[][] objs = new object[annotations.Length][];
						for (int j = 0; j < annotations.Length; j++)
						{
							objs[j] = new object[annotations[j].Length];
							for (int k = 0; k < annotations[j].Length; k++)
							{
								objs[j][k] = JVM.NewAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j][k]);
							}
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object[] GetFieldAnnotations(FieldWrapper fw)
		{
			FieldWrapper[] fields = GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i] == fw)
				{
					object[] annotations = impl.GetFieldAnnotations(i);
					if (annotations != null)
					{
						object[] objs = new object[annotations.Length];
						for (int j = 0; j < annotations.Length; j++)
						{
							objs[j] = JVM.NewAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j]);
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object GetAnnotationDefault(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					object defVal = impl.GetMethodDefaultValue(i);
					if (defVal != null)
					{
						return JVM.NewAnnotationElementValue(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), mw.ReturnType.ClassObject, defVal);
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}
#endif

		protected virtual Type GetBaseTypeForDefineType()
		{
			return BaseTypeWrapper.TypeAsBaseType;
		}

#if STATIC_COMPILER
		internal virtual MethodWrapper[] GetReplacedMethodsFor(MethodWrapper mw)
		{
			return null;
		}
#endif // STATIC_COMPILER

		internal override ConstructorInfo GetSerializationConstructor()
		{
			return automagicSerializationCtor;
		}
	}
}
