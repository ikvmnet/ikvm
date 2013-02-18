/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using DynamicOrAotTypeWrapper = IKVM.Internal.AotTypeWrapper;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;
using DynamicOrAotTypeWrapper = IKVM.Internal.DynamicTypeWrapper;
using ProtectionDomain = java.security.ProtectionDomain;
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
		private readonly TypeWrapper baseTypeWrapper;
		private readonly TypeWrapper[] interfaces;
		private readonly string sourceFileName;
#if !STATIC_COMPILER
		private byte[][] lineNumberTables;
#endif
		private MethodBase automagicSerializationCtor;

		private TypeWrapper LoadTypeWrapper(ClassLoaderWrapper classLoader, ProtectionDomain pd, string name)
		{
			TypeWrapper tw = classLoader.LoadClassByDottedNameFast(name);
			if (tw == null)
			{
				throw new NoClassDefFoundError(name);
			}
			CheckMissing(this, tw);
			classLoader.CheckPackageAccess(tw, pd);
			return tw;
		}

		private static void CheckMissing(TypeWrapper prev, TypeWrapper tw)
		{
#if STATIC_COMPILER
			do
			{
				UnloadableTypeWrapper missing = tw as UnloadableTypeWrapper;
				if (missing != null)
				{
					Type mt = ReflectUtil.GetMissingType(missing.MissingType);
					if (mt.Assembly.__IsMissing)
					{
						throw new FatalCompilerErrorException(Message.MissingBaseTypeReference, mt.FullName, mt.Assembly.FullName);
					}
					throw new FatalCompilerErrorException(Message.MissingBaseType, mt.FullName, mt.Assembly.FullName,
						prev.TypeAsBaseType.FullName, prev.TypeAsBaseType.Module.Name);
				}
				foreach (TypeWrapper iface in tw.Interfaces)
				{
					CheckMissing(tw, iface);
				}
				prev = tw;
				tw = tw.BaseTypeWrapper;
			}
			while (tw != null);
#endif
		}

#if STATIC_COMPILER
		internal DynamicTypeWrapper(ClassFile f, CompilerClassLoader classLoader, ProtectionDomain pd)
#else
		internal DynamicTypeWrapper(ClassFile f, ClassLoaderWrapper classLoader, ProtectionDomain pd)
#endif
			: base(f.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, f.Modifiers, f.Name)
		{
			Profiler.Count("DynamicTypeWrapper");
			this.classLoader = classLoader;
			this.sourceFileName = f.SourceFileAttribute;
			this.baseTypeWrapper = f.IsInterface ? null : LoadTypeWrapper(classLoader, pd, f.SuperClass);
			if (BaseTypeWrapper != null)
			{
				if (!BaseTypeWrapper.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superclass " + BaseTypeWrapper.Name);
				}
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
				TypeWrapper iface = LoadTypeWrapper(classLoader, pd, interfaces[i].Name);
				if (!iface.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superinterface " + iface.Name);
				}
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
			TypeWrapper iface = classLoader.LoadClassByDottedNameFast(f.Name + DotNetTypeWrapper.DelegateInterfaceSuffix);
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

		internal sealed override TypeWrapper BaseTypeWrapper
		{
			get { return baseTypeWrapper; }
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

		internal override void Finish()
		{
			// we don't need locking, because Finish is Thread safe
			impl = impl.Finish();
		}

		internal void CreateStep1()
		{
			((JavaTypeImpl)impl).CreateStep1();
		}

		internal void CreateStep2()
		{
			((JavaTypeImpl)impl).CreateStep2();
		}

		private bool IsSerializable
		{
			get
			{
				return this.IsSubTypeOf(CoreClasses.java.io.Serializable.Wrapper);
			}
		}

		private abstract class DynamicImpl
		{
			internal abstract Type Type { get; }
			internal abstract TypeWrapper[] InnerClasses { get; }
			internal abstract TypeWrapper DeclaringTypeWrapper { get; }
			internal abstract Modifiers ReflectiveModifiers { get; }
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
			private readonly DynamicOrAotTypeWrapper wrapper;
			private TypeBuilder typeBuilder;
			private MethodWrapper[] methods;
			private MethodWrapper[][] baseMethods;
			private FieldWrapper[] fields;
			private FinishedTypeImpl finishedType;
			private bool finishInProgress;
			private MethodBuilder clinitMethod;
			private MethodBuilder finalizeMethod;
#if STATIC_COMPILER
			private DynamicTypeWrapper outerClassWrapper;
			private AnnotationBuilder annotationBuilder;
			private TypeBuilder enumBuilder;
			private Dictionary<string, TypeWrapper> nestedTypeNames;	// only keys are used, values are always null
#endif

			internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper)
			{
				Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
				this.classFile = f;
				this.wrapper = (DynamicOrAotTypeWrapper)wrapper;
			}

			internal void CreateStep1()
			{
				// process all methods
				bool hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
				methods = new MethodWrapper[classFile.Methods.Length];
				baseMethods = new MethodWrapper[classFile.Methods.Length][];
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
						methods[i] = new GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
					}
					else if (m.IsConstructor && wrapper.IsDelegate)
					{
						methods[i] = new DelegateConstructorMethodWrapper(wrapper, m);
					}
					else
					{
						if (!classFile.IsInterface && !m.IsStatic && !m.IsPrivate && !m.IsConstructor)
						{
							bool explicitOverride = false;
							baseMethods[i] = FindBaseMethods(m, out explicitOverride);
							if (explicitOverride)
							{
								flags |= MemberFlags.ExplicitOverride;
							}
						}
						methods[i] = new TypicalMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
					}
				}
				if (hasclinit)
				{
					wrapper.SetHasStaticInitializer();
				}
				if (!wrapper.IsInterface || wrapper.IsPublic)
				{
					List<MethodWrapper> methodsArray = new List<MethodWrapper>(methods);
					List<MethodWrapper[]> baseMethodsArray = new List<MethodWrapper[]>(baseMethods);
					AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
					methods = methodsArray.ToArray();
					baseMethods = baseMethodsArray.ToArray();
				}
				if (!wrapper.IsInterface)
				{
					AddDelegateInvokeStubs(wrapper, ref methods);
				}
				wrapper.SetMethods(methods);

				fields = new FieldWrapper[classFile.Fields.Length];
				for (int i = 0; i < fields.Length; i++)
				{
					ClassFile.Field fld = classFile.Fields[i];
					if (fld.IsStaticFinalConstant)
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
				wrapper.AddMapXmlFields(ref fields);
#endif
				wrapper.SetFields(fields);
			}

			internal void CreateStep2()
			{
#if STATIC_COMPILER
				if (typeBuilder != null)
				{
					// in the static compiler we need to create the TypeBuilder from outer to inner
					// and to avoid having to sort the classes this way, we instead call CreateStep2
					// on demand for outer wrappers and this necessitates us to keep track of
					// whether we've already been called
					return;
				}
#endif
				// this method is not allowed to throw exceptions (if it does, the runtime will abort)
				bool hasclinit = wrapper.HasStaticInitializer;
				string mangledTypeName = wrapper.classLoader.GetTypeWrapperFactory().AllocMangledName(wrapper);
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
								outerClassWrapper = wrapper.classLoader.LoadClassByDottedNameFast(outerClassName) as DynamicTypeWrapper;
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
								JavaTypeImpl oimpl = outerClassWrapper.impl as JavaTypeImpl;
								if (oimpl != null && outerClassWrapper.GetClassLoader() == wrapper.GetClassLoader())
								{
									ClassFile outerClassFile = oimpl.classFile;
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
									outerClassWrapper.CreateStep2();
									outer = oimpl.typeBuilder;
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
						// if any "meaningless" bits are set, preserve them
						setModifiers |= (f.Modifiers & (Modifiers)0x99CE) != 0;
						// by default we assume interfaces are abstract, so in the exceptional case we need a ModifiersAttribute
						setModifiers |= (f.Modifiers & Modifiers.Abstract) == 0;
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
						// if any "meaningless" bits are set, preserve them
						setModifiers |= (f.Modifiers & (Modifiers)0x99CE) != 0;
						// by default we assume ACC_SUPER for classes, so in the exceptional case we need a ModifiersAttribute
						setModifiers |= !f.IsSuper;
						if (f.IsEffectivelyFinal)
						{
							setModifiers = true;
							typeAttribs |= TypeAttributes.Sealed;
							Tracer.Info(Tracer.Compiler, "Sealing type {0}", f.Name);
						}
						if (outer != null && !cantNest)
						{
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs);
						}
						else
#endif // STATIC_COMPILER
						{
							typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
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
					if (outerClass.outerClass != 0 && outer == null)
					{
						AttributeHelper.SetNonNestedOuterClass(typeBuilder, classFile.GetConstantPoolClass(outerClass.outerClass));
					}
					if (classFile.InnerClasses != null)
					{
						foreach (ClassFile.InnerClass inner in classFile.InnerClasses)
						{
							string name = classFile.GetConstantPoolClass(inner.innerClass);
							bool exists = false;
							try
							{
								exists = wrapper.GetClassLoader().LoadClassByDottedNameFast(name) != null;
							}
							catch (RetargetableJavaException) { }
							if (!exists)
							{
								AttributeHelper.SetNonNestedInnerClass(typeBuilder, name);
							}
						}
					}
					if (outer == null && mangledTypeName != wrapper.Name)
					{
						// HACK we abuse the InnerClassAttribute to record to real name
						AttributeHelper.SetInnerClass(typeBuilder, wrapper.Name, wrapper.Modifiers);
					}
					if (typeBuilder.FullName != wrapper.Name
						&& wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
					{
						wrapper.classLoader.AddNameMapping(wrapper.Name, typeBuilder.FullName);
					}
					if (f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
					{
						annotationBuilder = new AnnotationBuilder(this, outer);
						wrapper.SetAnnotation(annotationBuilder);
					}
					// For Java 5 Enum types, we generate a nested .NET enum.
					// This is primarily to support annotations that take enum parameters.
					if (f.IsEnum && f.IsPublic)
					{
						CompilerClassLoader ccl = wrapper.classLoader;
						string name = "__Enum";
						while (!ccl.ReserveName(f.Name + "$" + name))
						{
							name += "_";
						}
						enumBuilder = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, Types.Enum);
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
						wrapper.SetEnumType(enumBuilder);
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
#if STATIC_COMPILER
				finally { }
#else
				catch (Exception x)
				{
					JVM.CriticalFailure("Exception during JavaTypeImpl.CreateStep2", x);
				}
#endif
			}

			private sealed class DelegateConstructorMethodWrapper : MethodWrapper
			{
				private MethodBuilder constructor;
				private MethodInfo invoke;

				internal DelegateConstructorMethodWrapper(DynamicTypeWrapper tw, ClassFile.Method m)
					: base(tw, m.Name, m.Signature, null, null, null, m.Modifiers, MemberFlags.None)
				{
				}

				internal void DoLink(TypeBuilder typeBuilder)
				{
					MethodAttributes attribs = MethodAttributes.HideBySig | MethodAttributes.Public;
					constructor = ReflectUtil.DefineConstructor(typeBuilder, attribs, new Type[] { Types.Object, Types.IntPtr });
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

			private MethodWrapper GetMethodWrapperDuringCtor(TypeWrapper lookup, IList<MethodWrapper> methods, string name, string sig)
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

			private void AddMirandaMethods(List<MethodWrapper> methods, List<MethodWrapper[]> baseMethods, TypeWrapper tw)
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
									mw = new TypicalMethodWrapper(wrapper, ifmethod.Name, ifmethod.Signature, null, null, null, Modifiers.Public | Modifiers.Abstract, MemberFlags.HideFromReflection | MemberFlags.MirandaMethod);
									methods.Add(mw);
									baseMethods.Add(new MethodWrapper[] { ifmethod });
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

			private void AddDelegateInvokeStubs(TypeWrapper tw, ref MethodWrapper[] methods)
			{
				foreach (TypeWrapper iface in tw.Interfaces)
				{
					if (iface.IsFakeNestedType
						&& iface.GetMethods().Length == 1
						&& iface.GetMethods()[0].IsDelegateInvokeWithByRefParameter)
					{
						MethodWrapper mw = new DelegateInvokeStubMethodWrapper(wrapper, iface.DeclaringTypeWrapper.TypeAsBaseType, iface.GetMethods()[0].Signature);
						if (GetMethodWrapperDuringCtor(wrapper, methods, mw.Name, mw.Signature) == null)
						{
							Array.Resize(ref methods, methods.Length + 1);
							methods[methods.Length - 1] = mw;
						}
					}
					AddDelegateInvokeStubs(iface, ref methods);
				}
			}

			private sealed class DelegateInvokeStubMethodWrapper : MethodWrapper
			{
				private readonly Type delegateType;

				internal DelegateInvokeStubMethodWrapper(TypeWrapper declaringType, Type delegateType, string sig)
					: base(declaringType, DotNetTypeWrapper.GetDelegateInvokeStubName(delegateType), sig, null, null, null, Modifiers.Public | Modifiers.Final, MemberFlags.HideFromReflection)
				{
					this.delegateType = delegateType;
				}

				internal MethodInfo DoLink(TypeBuilder tb)
				{
					MethodWrapper mw = this.DeclaringType.GetMethodWrapper("Invoke", this.Signature, true);

					MethodInfo invoke = delegateType.GetMethod("Invoke");
					ParameterInfo[] parameters = invoke.GetParameters();
					Type[] parameterTypes = new Type[parameters.Length];
					for (int i = 0; i < parameterTypes.Length; i++)
					{
						parameterTypes[i] = parameters[i].ParameterType;
					}
					MethodBuilder mb = tb.DefineMethod(this.Name, MethodAttributes.Public, invoke.ReturnType, parameterTypes);
					AttributeHelper.HideFromReflection(mb);
					CodeEmitter ilgen = CodeEmitter.Create(mb);
					if (mw == null || mw.IsStatic || !mw.IsPublic)
					{
						ilgen.EmitThrow(mw == null || mw.IsStatic ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", DeclaringType.Name + ".Invoke" + Signature);
						ilgen.DoEmit();
						return mb;
					}
					CodeEmitterLocal[] byrefs = new CodeEmitterLocal[parameters.Length];
					for (int i = 0; i < parameters.Length; i++)
					{
						if (parameters[i].ParameterType.IsByRef)
						{
							Type elemType = parameters[i].ParameterType.GetElementType();
							CodeEmitterLocal local = ilgen.DeclareLocal(ArrayTypeWrapper.MakeArrayType(elemType, 1));
							byrefs[i] = local;
							ilgen.Emit(OpCodes.Ldc_I4_1);
							ilgen.Emit(OpCodes.Newarr, elemType);
							ilgen.Emit(OpCodes.Stloc, local);
							ilgen.Emit(OpCodes.Ldloc, local);
							ilgen.Emit(OpCodes.Ldc_I4_0);
							ilgen.EmitLdarg(i + 1);
							ilgen.Emit(OpCodes.Ldobj, elemType);
							ilgen.Emit(OpCodes.Stelem, elemType);
						}
					}
					ilgen.BeginExceptionBlock();
					ilgen.Emit(OpCodes.Ldarg_0);
					for (int i = 0; i < parameters.Length; i++)
					{
						if (byrefs[i] != null)
						{
							ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
						}
						else
						{
							ilgen.EmitLdarg(i + 1);
						}
					}
					mw.Link();
					mw.EmitCallvirt(ilgen);
					CodeEmitterLocal returnValue = null;
					if (mw.ReturnType != PrimitiveTypeWrapper.VOID)
					{
						returnValue = ilgen.DeclareLocal(mw.ReturnType.TypeAsSignatureType);
						ilgen.Emit(OpCodes.Stloc, returnValue);
					}
					CodeEmitterLabel exit = ilgen.DefineLabel();
					ilgen.EmitLeave(exit);
					ilgen.BeginFinallyBlock();
					for (int i = 0; i < parameters.Length; i++)
					{
						if (byrefs[i] != null)
						{
							Type elemType = byrefs[i].LocalType.GetElementType();
							ilgen.EmitLdarg(i + 1);
							ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
							ilgen.Emit(OpCodes.Ldc_I4_0);
							ilgen.Emit(OpCodes.Ldelem, elemType);
							ilgen.Emit(OpCodes.Stobj, elemType);
						}
					}
					ilgen.Emit(OpCodes.Endfinally);
					ilgen.EndExceptionBlock();
					ilgen.MarkLabel(exit);
					if (returnValue != null)
					{
						ilgen.Emit(OpCodes.Ldloc, returnValue);
					}
					ilgen.Emit(OpCodes.Ret);
					ilgen.DoEmit();
					return mb;
				}
			}

#if STATIC_COMPILER
			private static bool CheckInnerOuterNames(string inner, string outer)
			{
				// do some sanity checks on the inner/outer class names
				return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.StartsWith(outer);
			}

			private string GetInnerClassName(string outer, string inner)
			{
				Debug.Assert(CheckInnerOuterNames(inner, outer));
				if (nestedTypeNames == null)
				{
					nestedTypeNames = new Dictionary<string, TypeWrapper>();
				}
				return DynamicClassLoader.TypeNameMangleImpl(nestedTypeNames, inner.Substring(outer.Length + 1), null);
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

			private static void CheckLoaderConstraints(MethodWrapper mw, MethodWrapper baseMethod)
			{
#if !STATIC_COMPILER
				if (JVM.FinishingForDebugSave)
				{
					// when we're finishing types to save a debug image (in dynamic mode) we don't care about loader constraints anymore
					// (and we can't throw a LinkageError, because that would prevent the debug image from being saved)
					return;
				}
#endif
				if (mw.ReturnType != baseMethod.ReturnType)
				{
					if (mw.ReturnType.IsUnloadable || baseMethod.ReturnType.IsUnloadable)
					{
						// unloadable types can never cause a loader constraint violation
						if (mw.ReturnType.IsUnloadable && baseMethod.ReturnType.IsUnloadable)
						{
							((UnloadableTypeWrapper)mw.ReturnType).SetCustomModifier(((UnloadableTypeWrapper)baseMethod.ReturnType).CustomModifier);
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
						if (here[i].IsUnloadable || there[i].IsUnloadable)
						{
							// unloadable types can never cause a loader constraint violation
							if (here[i].IsUnloadable && there[i].IsUnloadable)
							{
								((UnloadableTypeWrapper)here[i]).SetCustomModifier(((UnloadableTypeWrapper)there[i]).CustomModifier);
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
				// for compatibility with broken Java code that assumes that reflection returns the fields in class declaration
				// order, we emit the fields in class declaration order in the .NET metadata (and then when we retrieve them
				// using .NET reflection, we sort on metadata token.)
				if (fieldIndex > 0)
				{
					if (!fields[fieldIndex - 1].IsLinked)
					{
						for (int i = 0; i < fieldIndex; i++)
						{
							fields[i].Link();
						}
					}
				}
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
					return DefineField(fw.Name, fw.FieldTypeWrapper, fieldAttribs, fw.IsVolatile);
				}
#endif // STATIC_COMPILER
				FieldBuilder field;
				ClassFile.Field fld = classFile.Fields[fieldIndex];
				string realFieldName = fld.Name;
				FieldAttributes attribs = 0;
				MethodAttributes methodAttribs = MethodAttributes.HideBySig;
#if STATIC_COMPILER
				bool setModifiers = fld.IsInternal || (fld.Modifiers & (Modifiers.Synthetic | Modifiers.Enum)) != 0;
#endif
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

				if (fld.IsStatic)
				{
					attribs |= FieldAttributes.Static;
					methodAttribs |= MethodAttributes.Static;
				}
				// NOTE "constant" static finals are converted into literals
				// TODO it would be possible for Java code to change the value of a non-blank static final, but I don't
				// know if we want to support this (since the Java JITs don't really support it either)
				if (fld.IsStaticFinalConstant)
				{
					Profiler.Count("Static Final Constant");
					attribs |= FieldAttributes.Literal;
					field = DefineField(fld.Name, fw.FieldTypeWrapper, attribs, false);
					field.SetConstant(fld.ConstantValue);
				}
				else
				{
#if STATIC_COMPILER
					if (wrapper.IsPublic && wrapper.NeedsType2AccessStub(fw))
					{
						// this field is going to get a type 2 access stub, so we hide the actual field
						attribs &= ~FieldAttributes.FieldAccessMask;
						attribs |= FieldAttributes.Assembly;
						// instead of adding HideFromJava we rename the field to avoid confusing broken compilers
						// see https://sourceforge.net/tracker/?func=detail&atid=525264&aid=3056721&group_id=69637
						// additional note: now that we maintain the ordering of the fields, we need to recognize
						// these fields so that we know where to insert the corresponding accessor property FieldWrapper.
						realFieldName = NamePrefix.Type2AccessStubBackingField + fld.Name;
					}
					else if (fld.IsFinal)
					{
						if (wrapper.IsInterface || wrapper.classLoader.StrictFinalFieldSemantics)
						{
							attribs |= FieldAttributes.InitOnly;
						}
						else
						{
							setModifiers = true;
						}
					}
#else
					if (fld.IsFinal && wrapper.IsInterface)
					{
						attribs |= FieldAttributes.InitOnly;
					}
#endif

					field = DefineField(realFieldName, fw.FieldTypeWrapper, attribs, fld.IsVolatile);
				}
				if (fld.IsTransient)
				{
					CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(JVM.Import(typeof(NonSerializedAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
					field.SetCustomAttribute(transientAttrib);
				}
#if STATIC_COMPILER
				{
					// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
					// the Java modifiers
					if (setModifiers)
					{
						AttributeHelper.SetModifiers(field, fld.Modifiers, fld.IsInternal);
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

			private FieldBuilder DefineField(string name, TypeWrapper tw, FieldAttributes attribs, bool isVolatile)
			{
				Type[] modreq = isVolatile ? new Type[] { Types.IsVolatile } : Type.EmptyTypes;
				return typeBuilder.DefineField(name, tw.TypeAsSignatureType, modreq, wrapper.GetModOpt(tw, false), attribs);
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
					FinishedTypeImpl impl;
					try
					{
						// call FinishCore in the finally to avoid Thread.Abort interrupting the thread
					}
					finally
					{
						impl = FinishCore();
					}
					return impl;
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
#if STATIC_COMPILER
					if (annotationBuilder != null)
					{
						CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(AnnotationAttributeAttribute)).GetConstructor(new Type[] { Types.String }), new object[] { annotationBuilder.AttributeTypeName });
						typeBuilder.SetCustomAttribute(cab);
					}
					if (!wrapper.IsInterface && wrapper.IsMapUnsafeException)
					{
						// mark all exceptions that are unsafe for mapping with a custom attribute,
						// so that at runtime we can quickly assertain if an exception type can be
						// caught without filtering
						AttributeHelper.SetExceptionIsUnsafeForMapping(typeBuilder);
					}
#endif

					FinishContext context = new FinishContext(classFile, wrapper, typeBuilder);
					Type type = context.FinishImpl();
#if STATIC_COMPILER
					if (annotationBuilder != null)
					{
						annotationBuilder.Finish(this);
					}
					if (enumBuilder != null)
					{
						enumBuilder.CreateType();
					}
#endif
					MethodInfo finishedClinitMethod = clinitMethod;
#if !STATIC_COMPILER
					if (finishedClinitMethod != null)
					{
						// In dynamic mode, we may need to emit a call to this method from a DynamicMethod which doesn't support calling unfinished methods,
						// so we must resolve to the real method here.
						finishedClinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					}
#endif
					finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, wrapper.ReflectiveModifiers, Metadata.Create(classFile), finishedClinitMethod, finalizeMethod);
					return finishedType;
				}
#if !STATIC_COMPILER
				catch (Exception x)
				{
					JVM.CriticalFailure("Exception during finishing of: " + wrapper.Name, x);
					return null;
				}
#endif
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
				private TypeBuilder outer;
				private TypeBuilder annotationTypeBuilder;
				private TypeBuilder attributeTypeBuilder;
				private MethodBuilder defineConstructor;

				internal AnnotationBuilder(JavaTypeImpl o, TypeBuilder outer)
				{
					this.impl = o;
					this.outer = outer;
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
					CompilerClassLoader ccl = o.wrapper.classLoader;
					string name = o.classFile.Name;
					while (!ccl.ReserveName(name + "Attribute"))
					{
						name += "_";
					}

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
						attributeTypeBuilder = outer.DefineNestedType(o.GetInnerClassName(o.outerClassWrapper.Name, name + "Attribute"), typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
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
						CustomAttributeBuilder attributeUsageAttribute = null;
						bool hasAttributeUsageAttribute = false;
						foreach (object[] def in o.classFile.Annotations)
						{
							if (def[1].Equals("Ljava/lang/annotation/Target;") && !hasAttributeUsageAttribute)
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
											attributeUsageAttribute = new CustomAttributeBuilder(JVM.Import(typeof(AttributeUsageAttribute)).GetConstructor(new Type[] { JVM.Import(typeof(AttributeTargets)) }), new object[] { targets });
										}
									}
								}
							}
							else
							{
								// apply any .NET custom attributes that are on the annotation to the custom attribute we synthesize
								// (for example, to allow AttributeUsageAttribute to be overridden)
								Annotation annotation = Annotation.Load(o.wrapper.GetClassLoader(), def);
								if (annotation != null && annotation.IsCustomAttribute)
								{
									annotation.Apply(o.wrapper.GetClassLoader(), attributeTypeBuilder, def);
								}
								if (def[1].Equals("Lcli/System/AttributeUsageAttribute$Annotation;"))
								{
									hasAttributeUsageAttribute = true;
								}
							}
						}
						if (attributeUsageAttribute != null && !hasAttributeUsageAttribute)
						{
							attributeTypeBuilder.SetCustomAttribute(attributeUsageAttribute);
						}
					}

					defineConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, new Type[] { JVM.Import(typeof(object[])) });
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
					ilgen.EmitLdarg(argIndex);
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

					MethodBuilder defaultConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, unsupported || requiredArgCount > 0 ? MethodAttributes.Private : MethodAttributes.Public, Type.EmptyTypes);
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
							MethodBuilder reqArgConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, args);
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
								MethodBuilder cb = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, new Type[] { argType });
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
							MethodBuilder mb = o.methods[i].GetDefineMethodHelper().DefineMethod(o.wrapper, attributeTypeBuilder, o.methods[i].Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot);
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
								o.methods[i].ReturnType.EmitCheckcast(ilgen);
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
						tb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						mb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						fb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						pb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						ab.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
				{
					Link();
					if (annotationTypeBuilder != null)
					{
						annotation = QualifyClassNames(loader, annotation);
						pb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor.__AsConstructorInfo(), new object[] { annotation }));
					}
				}

				internal override bool IsCustomAttribute
				{
					get { return false; }
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
					Modifiers mods;
					ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
					if (innerclasses != null)
					{
						for (int i = 0; i < innerclasses.Length; i++)
						{
							if (innerclasses[i].innerClass != 0)
							{
								if (classFile.GetConstantPoolClass(innerclasses[i].innerClass) == wrapper.Name)
								{
									// the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
									// (minus ACC_SUPER)
									mods = innerclasses[i].accessFlags & (Modifiers)0x761F;
									if (classFile.IsInterface)
									{
										mods |= Modifiers.Abstract;
									}
									return mods;
								}
							}
						}
					}
					// the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
					// (minus ACC_SUPER)
					mods = classFile.Modifiers & (Modifiers)0x7611;
					if (classFile.IsInterface)
					{
						mods |= Modifiers.Abstract;
					}
					return mods;
				}
			}

			// this finds all methods that the specified name/sig is going to be overriding
			private MethodWrapper[] FindBaseMethods(ClassFile.Method m, out bool explicitOverride)
			{
				Debug.Assert(!classFile.IsInterface);
				Debug.Assert(m.Name != "<init>");

				// starting with Java 7 the algorithm changed
				return classFile.MajorVersion >= 51
					? FindBaseMethods7(m.Name, m.Signature, m.IsFinal && !m.IsPublic && !m.IsProtected, out explicitOverride)
					: FindBaseMethodsLegacy(m.Name, m.Signature, out explicitOverride);
			}

			private MethodWrapper[] FindBaseMethods7(string name, string sig, bool packageFinal, out bool explicitOverride)
			{
				// NOTE this implements the (completely broken) OpenJDK 7 b147 HotSpot behavior,
				// not the algorithm specified in section 5.4.5 of the JavaSE7 JVM spec
				// see http://weblog.ikvm.net/PermaLink.aspx?guid=bde44d8b-7ba9-4e0e-b3a6-b735627118ff and subsequent posts
				explicitOverride = false;
				MethodWrapper topPublicOrProtectedMethod = null;
				TypeWrapper tw = wrapper.BaseTypeWrapper;
				while (tw != null)
				{
					MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
					if (baseMethod == null)
					{
						break;
					}
					else if (baseMethod.IsAccessStub)
					{
						// ignore
					}
					else if (!baseMethod.IsStatic && (baseMethod.IsPublic || baseMethod.IsProtected))
					{
						topPublicOrProtectedMethod = baseMethod;
					}
					tw = baseMethod.DeclaringType.BaseTypeWrapper;
				}
				tw = wrapper.BaseTypeWrapper; 
				while (tw != null)
				{
					MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
					if (baseMethod == null)
					{
						break;
					}
					else if (baseMethod.IsAccessStub)
					{
						// ignore
					}
					else if (baseMethod.IsPrivate)
					{
						// skip
					}
					else if (baseMethod.IsFinal && (baseMethod.IsPublic || baseMethod.IsProtected || IsAccessibleInternal(baseMethod) || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
					{
						throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overridden in " + wrapper.Name);
					}
					else if (baseMethod.IsStatic)
					{
						// skip
					}
					else if (topPublicOrProtectedMethod == null && !baseMethod.IsPublic && !baseMethod.IsProtected && !IsAccessibleInternal(baseMethod) && !baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
					{
						// this is a package private method that we're not overriding (unless its vtable stream interleaves ours, which is a case we handle below)
						explicitOverride = true;
					}
					else if (topPublicOrProtectedMethod != null && baseMethod.IsFinal && !baseMethod.IsPublic && !baseMethod.IsProtected && !IsAccessibleInternal(baseMethod) && !baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
					{
						// this is package private final method that we would override had it not been final, but which is ignored by HotSpot (instead of throwing a VerifyError)
						explicitOverride = true;
					}
					else if (topPublicOrProtectedMethod == null)
					{
						if (explicitOverride)
						{
							List<MethodWrapper> list = new List<MethodWrapper>();
							list.Add(baseMethod);
							// we might still have to override package methods from another package if the vtable streams are interleaved with ours
							tw = wrapper.BaseTypeWrapper;
							while (tw != null)
							{
								MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
								if (baseMethod2 == null || baseMethod2 == baseMethod)
								{
									break;
								}
								MethodWrapper baseMethod3 = GetPackageBaseMethod(baseMethod.DeclaringType.BaseTypeWrapper, name, sig, baseMethod2.DeclaringType);
								if (baseMethod3 != null)
								{
									if (baseMethod2.IsFinal)
									{
										baseMethod2 = baseMethod3;
									}
									bool found = false;
									foreach (MethodWrapper mw in list)
									{
										if (mw.DeclaringType.IsPackageAccessibleFrom(baseMethod2.DeclaringType))
										{
											// we should only add each package once
											found = true;
											break;
										}
									}
									if (!found)
									{
										list.Add(baseMethod2);
									}
								}
								tw = baseMethod2.DeclaringType.BaseTypeWrapper;
							}
							return list.ToArray();
						}
						else
						{
							return new MethodWrapper[] { baseMethod };
						}
					}
					else
					{
						if (packageFinal)
						{
							// when a package final method overrides a public or protected method, HotSpot does not mark that vtable slot as final,
							// so we need an explicit override to force the MethodAttributes.NewSlot flag, otherwise the CLR won't allow us
							// to override the original method in subsequent derived types
							explicitOverride = true;
						}

						int majorVersion = 0;
						if (!baseMethod.IsPublic && !baseMethod.IsProtected &&
							((TryGetClassFileVersion(baseMethod.DeclaringType, ref majorVersion) && majorVersion < 51)
							// if TryGetClassFileVersion fails, we know that it is safe to call GetMethod() so we look at the actual method attributes here,
							// because access widing ensures that if the method had overridden the top level method it would also be public or protected
							|| (majorVersion == 0 && (LinkAndGetMethod(baseMethod).Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly)))
						{
							// the method we're overriding is not public or protected, but there is a public or protected top level method,
							// this means that baseMethod is part of a class with a major version < 51, so we have to explicitly override the top level method as well
							// (we don't need to look for another package method to override, because by necessity baseMethod is already in our package)
							return new MethodWrapper[] { baseMethod, topPublicOrProtectedMethod };
						}
						else if (!topPublicOrProtectedMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
						{
							// check if there is another method (in the same package) that we should override
							tw = topPublicOrProtectedMethod.DeclaringType.BaseTypeWrapper;
							while (tw != null)
							{
								MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
								if (baseMethod2 == null)
								{
									break;
								}
								if (baseMethod2.IsAccessStub)
								{
									// ignore
								}
								else if (baseMethod2.DeclaringType.IsPackageAccessibleFrom(wrapper) && !baseMethod2.IsPrivate)
								{
									if (baseMethod2.IsFinal)
									{
										throw new VerifyError("final method " + baseMethod2.Name + baseMethod2.Signature + " in " + baseMethod2.DeclaringType.Name + " is overridden in " + wrapper.Name);
									}
									if (!baseMethod2.IsStatic)
									{
										if (baseMethod2.IsPublic || baseMethod2.IsProtected)
										{
											break;
										}
										return new MethodWrapper[] { baseMethod, baseMethod2 };
									}
								}
								tw = baseMethod2.DeclaringType.BaseTypeWrapper;
							}
						}
						return new MethodWrapper[] { baseMethod };
					}
					tw = baseMethod.DeclaringType.BaseTypeWrapper;
				}
				return null;
			}

			private bool IsAccessibleInternal(MethodWrapper mw)
			{
				return mw.IsInternal && mw.DeclaringType.InternalsVisibleTo(wrapper);
			}

			private static MethodBase LinkAndGetMethod(MethodWrapper mw)
			{
				mw.Link();
				return mw.GetMethod();
			}

			private static bool TryGetClassFileVersion(TypeWrapper tw, ref int majorVersion)
			{
				DynamicTypeWrapper dtw = tw as DynamicTypeWrapper;
				if (dtw != null)
				{
					JavaTypeImpl impl = dtw.impl as JavaTypeImpl;
					if (impl != null)
					{
						majorVersion = impl.classFile.MajorVersion;
						return true;
					}
				}
				return false;
			}

			private static MethodWrapper GetPackageBaseMethod(TypeWrapper tw, string name, string sig, TypeWrapper package)
			{
				while (tw != null)
				{
					MethodWrapper mw = tw.GetMethodWrapper(name, sig, true);
					if (mw == null)
					{
						break;
					}
					if (mw.DeclaringType.IsPackageAccessibleFrom(package))
					{
						return mw.IsFinal ? null : mw;
					}
					tw = mw.DeclaringType.BaseTypeWrapper;
				}
				return null;
			}

			private MethodWrapper[] FindBaseMethodsLegacy(string name, string sig, out bool explicitOverride)
			{
				explicitOverride = false;
				TypeWrapper tw = wrapper.BaseTypeWrapper;
				while (tw != null)
				{
					MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
					if (baseMethod == null)
					{
						return null;
					}
					else if (baseMethod.IsAccessStub)
					{
						// ignore
					}
					// here are the complex rules for determining whether this method overrides the method we found
					// RULE 1: final methods may not be overridden
					// (note that we intentionally not check IsStatic here!)
					else if (baseMethod.IsFinal
						&& !baseMethod.IsPrivate
						&& (baseMethod.IsPublic || baseMethod.IsProtected || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
					{
						throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overridden in " + wrapper.Name);
					}
					// RULE 1a: static methods are ignored (other than the RULE 1 check)
					else if (baseMethod.IsStatic)
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
						if (!baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
						{
							// check if there is another method (in the same package) that we should override
							tw = baseMethod.DeclaringType.BaseTypeWrapper;
							while (tw != null)
							{
								MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
								if (baseMethod2 == null)
								{
									break;
								}
								if (baseMethod2.IsAccessStub)
								{
									// ignore
								}
								else if (baseMethod2.DeclaringType.IsPackageAccessibleFrom(wrapper) && !baseMethod2.IsPrivate)
								{
									if (baseMethod2.IsFinal)
									{
										throw new VerifyError("final method " + baseMethod2.Name + baseMethod2.Signature + " in " + baseMethod2.DeclaringType.Name + " is overridden in " + wrapper.Name);
									}
									if (!baseMethod2.IsStatic)
									{
										if (baseMethod2.IsPublic || baseMethod2.IsProtected)
										{
											break;
										}
										return new MethodWrapper[] { baseMethod, baseMethod2 };
									}
								}
								tw = baseMethod2.DeclaringType.BaseTypeWrapper;
							}
						}
						return new MethodWrapper[] { baseMethod };
					}
					// RULE 3: private and static methods are ignored
					else if (!baseMethod.IsPrivate)
					{
						// RULE 4: package methods can only be overridden in the same package
						if (baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)
							|| (baseMethod.IsInternal && baseMethod.DeclaringType.InternalsVisibleTo(wrapper)))
						{
							return new MethodWrapper[] { baseMethod };
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

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				if (mw is DelegateConstructorMethodWrapper)
				{
					((DelegateConstructorMethodWrapper)mw).DoLink(typeBuilder);
					return null;
				}
				if (mw is DelegateInvokeStubMethodWrapper)
				{
					return ((DelegateInvokeStubMethodWrapper)mw).DoLink(typeBuilder);
				}
				Debug.Assert(mw != null);
				int index = GetMethodIndex(mw);
				if (baseMethods[index] != null)
				{
					foreach (MethodWrapper baseMethod in baseMethods[index])
					{
						baseMethod.Link();
						CheckLoaderConstraints(mw, baseMethod);
					}
				}
				Debug.Assert(mw.GetMethod() == null);
				methods[index].AssertLinked();
				Profiler.Enter("JavaTypeImpl.GenerateMethod");
				try
				{
					if (index >= classFile.Methods.Length)
					{
						if (methods[index].IsMirandaMethod)
						{
							// We're a Miranda method
							Debug.Assert(baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface);
							MethodAttributes attr = MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
							if (wrapper.IsAbstract)
							{
								attr |= MethodAttributes.Abstract;
							}
							MethodBuilder mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, methods[index].Name, attr);
							AttributeHelper.HideFromReflection(mb);
							if (CheckRequireOverrideStub(methods[index], baseMethods[index][0]))
							{
								wrapper.GenerateOverrideStub(typeBuilder, baseMethods[index][0], mb, methods[index]);
							}
							// if we changed the name or if the interface method name is remapped, we need to add an explicit methodoverride.
							else if (!baseMethods[index][0].IsDynamicOnly && methods[index].Name != baseMethods[index][0].RealName)
							{
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethods[index][0].GetMethod());
							}
							if (!wrapper.IsAbstract)
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
					MethodBuilder method;
					bool setModifiers = false;
					if (methods[index].HasCallerID && (m.Modifiers & Modifiers.VarArgs) != 0)
					{
						// the implicit callerID parameter was added at the end so that means we shouldn't use ParamArrayAttribute,
						// so we need to explicitly record that the method is varargs
						setModifiers = true;
					}
					if (m.IsConstructor)
					{
						method = GenerateConstructor(methods[index]);
						// strictfp is the only modifier that a constructor can have
						if (m.IsStrictfp)
						{
							setModifiers = true;
						}
					}
					else if (m.IsClassInitializer)
					{
						method = ReflectUtil.DefineTypeInitializer(typeBuilder);
					}
					else
					{
						method = GenerateMethod(index, m, ref setModifiers);
					}
					string[] exceptions = m.ExceptionsAttribute;
					methods[index].SetDeclaredExceptions(exceptions);
#if STATIC_COMPILER
					AttributeHelper.SetThrowsAttribute(method, exceptions);
					if (setModifiers || m.IsInternal || (m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0)
					{
						AttributeHelper.SetModifiers(method, m.Modifiers, m.IsInternal);
					}
					if ((m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0
						&& (m.IsPublic || m.IsProtected)
						&& wrapper.IsPublic
						&& !IsAccessBridge(classFile, m))
					{
						AttributeHelper.SetEditorBrowsableNever(method);
						// TODO on WHIDBEY apply CompilerGeneratedAttribute
					}
					if (m.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(method);
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

			private MethodBuilder GenerateConstructor(MethodWrapper mw)
			{
				MethodBuilder cb = mw.GetDefineMethodHelper().DefineConstructor(wrapper, typeBuilder, GetMethodAccess(mw) | MethodAttributes.HideBySig);
				cb.SetImplementationFlags(MethodImplAttributes.NoInlining);
				return cb;
			}

			private MethodBuilder GenerateMethod(int index, ClassFile.Method m, ref bool setModifiers)
			{
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
					attribs |= GetMethodAccess(methods[index]);
				}
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
							name = NamePrefix.Bridge + name;
							break;
						}
					}
				}
#endif
				if ((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
				{
					if (baseMethods[index] == null || (baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface))
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
						bool hasPublicBaseMethod = false;
						foreach (MethodWrapper baseMethodWrapper in baseMethods[index])
						{
							MethodBase baseMethod = baseMethodWrapper.GetMethod();
							if ((baseMethod.IsPublic && !m.IsPublic) ||
								((baseMethod.IsFamily || baseMethod.IsFamilyOrAssembly) && !m.IsPublic && !m.IsProtected) ||
								(!m.IsPublic && !m.IsProtected && !baseMethodWrapper.DeclaringType.IsPackageAccessibleFrom(wrapper)))
							{
								hasPublicBaseMethod |= baseMethod.IsPublic;
								attribs &= ~MethodAttributes.MemberAccessMask;
								attribs |= hasPublicBaseMethod ? MethodAttributes.Public : MethodAttributes.FamORAssem;
								setModifiers = true;
							}
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
					if (baseMethods[index] != null && ReferenceEquals(m.Name, StringConstants.FINALIZE) && ReferenceEquals(m.Signature, StringConstants.SIG_VOID))
					{
						baseFinalize = GetBaseFinalizeMethod(wrapper.BaseTypeWrapper);
						if (baseMethods[index][0].DeclaringType == CoreClasses.java.lang.Object.Wrapper)
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
					bool newslot = baseMethods[index] != null
						&& (methods[index].IsExplicitOverride || baseMethods[index][0].RealName != name || CheckRequireOverrideStub(methods[index], baseMethods[index][0]))
						&& !needFinalize;
					if (newslot)
					{
						attribs |= MethodAttributes.NewSlot;
					}
					mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, attribs);
					if (baseMethods[index] != null && !needFinalize)
					{
						bool subsequent = false;
						foreach (MethodWrapper baseMethod in baseMethods[index])
						{
							if (CheckRequireOverrideStub(methods[index], baseMethod))
							{
								wrapper.GenerateOverrideStub(typeBuilder, baseMethod, mb, methods[index]);
							}
							else if (subsequent || methods[index].IsExplicitOverride || baseMethod.RealName != name)
							{
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethod.GetMethod());
							}
							// the non-primary base methods always need an explicit method override
							subsequent = true;
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
						ilgen.EmitBrtrue(skip);
						if (needDispatch)
						{
							ilgen.BeginExceptionBlock();
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Callvirt, mb);
							ilgen.EmitLeave(skip);
							ilgen.BeginCatchBlock(Types.Object);
							ilgen.EmitLeave(skip);
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

				if ((methods[index].Modifiers & (Modifiers.Synchronized | Modifiers.Static)) == Modifiers.Synchronized)
				{
					mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);
				}

				return mb;
			}

			private static MethodAttributes GetMethodAccess(MethodWrapper mw)
			{
				switch (mw.Modifiers & Modifiers.AccessMask)
				{
					case Modifiers.Private:
						return MethodAttributes.Private;
					case Modifiers.Protected:
						return MethodAttributes.FamORAssem;
					case Modifiers.Public:
						return MethodAttributes.Public;
					default:
						return MethodAttributes.Assembly;
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
		}

		private sealed class Metadata
		{
			private readonly string[][] genericMetaData;
			private readonly object[][] annotations;

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
			private readonly Type type;
			private readonly TypeWrapper[] innerclasses;
			private readonly TypeWrapper declaringTypeWrapper;
			private readonly Modifiers reflectiveModifiers;
			private readonly MethodInfo clinitMethod;
			private readonly MethodInfo finalizeMethod;
			private readonly Metadata metadata;

			internal FinishedTypeImpl(Type type, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers, Metadata metadata, MethodInfo clinitMethod, MethodInfo finalizeMethod)
			{
				this.type = type;
				this.innerclasses = innerclasses;
				this.declaringTypeWrapper = declaringTypeWrapper;
				this.reflectiveModifiers = reflectiveModifiers;
				this.clinitMethod = clinitMethod;
				this.finalizeMethod = finalizeMethod;
				this.metadata = metadata;
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
		}

		internal sealed class FinishContext
		{
			private readonly ClassFile classFile;
			private readonly DynamicOrAotTypeWrapper wrapper;
			private readonly TypeBuilder typeBuilder;
			private List<TypeBuilder> nestedTypeBuilders;
			private MethodInfo callerIDMethod;
			private List<Item> items;
			private Dictionary<FieldWrapper, MethodBuilder> arfuMap;
			private Dictionary<MethodKey, MethodInfo> invokespecialstubcache;

			private struct Item
			{
				internal int key;
				internal object value;
			}

			internal FinishContext(ClassFile classFile, DynamicOrAotTypeWrapper wrapper, TypeBuilder typeBuilder)
			{
				this.classFile = classFile;
				this.wrapper = wrapper;
				this.typeBuilder = typeBuilder;
			}

			internal T GetValue<T>(int key)
				where T : class, new()
			{
				if (items == null)
				{
					items = new List<Item>();
				}
				for (int i = 0; i < items.Count; i++)
				{
					T value;
					if (items[i].key == key && (value = items[i].value as T) != null)
					{
						return value;
					}
				}
				Item item;
				item.key = key;
				T val = new T();
				item.value = val;
				items.Add(item);
				return val;
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
				ilgen.EmitBrtrue(done);
				EmitCallerIDInitialization(ilgen, callerIDField);
				ilgen.MarkLabel(done);
				ilgen.Emit(OpCodes.Ldsfld, callerIDField);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
			}

			private void RegisterNestedTypeBuilder(TypeBuilder tb)
			{
				if (nestedTypeBuilders == null)
				{
					nestedTypeBuilders = new List<TypeBuilder>();
				}
				nestedTypeBuilders.Add(tb);
			}

			internal Type FinishImpl()
			{
				MethodWrapper[] methods = wrapper.GetMethods();
				FieldWrapper[] fields = wrapper.GetFields();
#if STATIC_COMPILER
				wrapper.FinishGhost(typeBuilder, methods);
#endif // STATIC_COMPILER

				if (!classFile.IsInterface)
				{
					// set the base type (this needs to be done before we emit any methods, because in the static compiler
					// GetBaseTypeForDefineType() has the side effect of inserting the __WorkaroundBaseClass__ when necessary)
					typeBuilder.SetParent(wrapper.GetBaseTypeForDefineType());
				}

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
									MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, attr);
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
				bool basehasclinit = wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasStaticInitializer;
				int clinitIndex = -1;
				bool hasConstructor = false;
				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBuilder mb = (MethodBuilder)methods[i].GetMethod();
					if (mb == null)
					{
						// method doesn't really exist (e.g. delegate constructor or <clinit> that is optimized away)
						if (m.IsConstructor)
						{
							hasConstructor = true;
						}
					}
					else if (m.IsClassInitializer)
					{
						// we handle the <clinit> after we've done the other methods,
						// to make it easier to inject code needed by the other methods
						clinitIndex = i;
						continue;
					}
					else if (m.IsConstructor)
					{
						hasConstructor = true;
						CodeEmitter ilGenerator = CodeEmitter.Create(mb);
						CompileConstructorBody(this, ilGenerator, i);
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
								CodeEmitter ilGenerator = CodeEmitter.Create(mb);
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
								mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Runtime);
								continue;
							}
							Profiler.Enter("JavaTypeImpl.Finish.Native");
							try
							{
								CodeEmitter ilGenerator = CodeEmitter.Create(mb);
								TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
								// do we have an implementation in map.xml?
								if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
								{
									ilGenerator.DoEmit();
									continue;
								}
#endif
								// see if there exists a "managed JNI" class for this type
								Type nativeCodeType = null;
#if STATIC_COMPILER
								nativeCodeType = StaticCompiler.GetType(wrapper.GetClassLoader(), "IKVM.NativeCode." + classFile.Name.Replace('$', '+'));
								if (nativeCodeType == null)
								{
									// simple JNI like class name mangling
									nativeCodeType = StaticCompiler.GetType(wrapper.GetClassLoader(), "Java_" + classFile.Name.Replace('.', '_'));
								}
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
										ilGenerator.EmitLdarg(j + add);
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
										StaticCompiler.errorCount++;
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
							CodeEmitter ilGenerator = CodeEmitter.Create(mb);
							TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
							// do we have an implementation in map.xml?
							if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
							{
								ilGenerator.DoEmit();
								continue;
							}
#endif // STATIC_COMPILER
							bool nonleaf = false;
							Compiler.Compile(this, wrapper, methods[i], classFile, m, ilGenerator, ref nonleaf);
							ilGenerator.CheckLabels();
							ilGenerator.DoEmit();
							if (nonleaf)
							{
								mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
							}
#if STATIC_COMPILER
							ilGenerator.EmitLineNumberTable((MethodBuilder)methods[i].GetMethod());
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
					MethodBuilder cb;
					if (clinitIndex != -1)
					{
						cb = (MethodBuilder)methods[clinitIndex].GetMethod();
					}
					else
					{
						cb = ReflectUtil.DefineTypeInitializer(typeBuilder);
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
						CompileConstructorBody(this, ilGenerator, clinitIndex);
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
						CodeEmitter ilgen = CodeEmitter.Create(ReflectUtil.DefineConstructor(typeBuilder, MethodAttributes.PrivateScope, Type.EmptyTypes));
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
					if (!wrapper.GetClassLoader().NoAutomagicSerialization)
					{
						wrapper.automagicSerializationCtor = Serialization.AddAutomagicSerialization(wrapper, typeBuilder);
					}
				}

#if STATIC_COMPILER
				// If we're an interface that has public/protected fields, we create an inner class
				// to expose these fields to C# (which stubbornly refuses to see fields in interfaces).
				TypeBuilder tbFields = null;
				if (classFile.IsInterface && classFile.IsPublic && !wrapper.IsGhost && classFile.Fields.Length > 0)
				{
					CompilerClassLoader ccl = wrapper.classLoader;
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
							FieldBuilder fb = tbFields.DefineField(f.Name, ToPublicSignatureType(fields[i].FieldTypeWrapper), attribs);
							if (ilgenClinit == null)
							{
								ilgenClinit = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tbFields));
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

				// if we inherit public members from non-public base classes or have public members with non-public types in their signature, we need access stubs
				if (wrapper.IsPublic)
				{
					AddAccessStubs();
				}
#endif // STATIC_COMPILER

				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBuilder mb = (MethodBuilder)methods[i].GetMethod();
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
						wrapper.GetParameterNamesFromXml(m.Name, m.Signature, parameterNames);
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
					wrapper.AddXmlMapParameterAttributes(mb, classFile.Name, m.Name, m.Signature, ref parameterBuilders);
#endif
					if (m.Annotations != null)
					{
						foreach (object[] def in m.Annotations)
						{
							Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
							if (annotation != null)
							{
								annotation.Apply(wrapper.GetClassLoader(), mb, def);
								annotation.ApplyReturnValue(wrapper.GetClassLoader(), mb, ref returnParameter, def);
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
						AttributeHelper.SetEditorBrowsableNever(mb);
						EmitCallerIDStub(methods[i], parameterNames);
					}
					if (m.DllExportName != null && wrapper.classLoader.TryEnableUnmanagedExports())
					{
						mb.__AddUnmanagedExport(m.DllExportName, m.DllExportOrdinal);
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
					if (nestedTypeBuilders != null)
					{
						ClassLoaderWrapper.LoadClassCritical("ikvm.internal.IntrinsicAtomicReferenceFieldUpdater").Finish();
						ClassLoaderWrapper.LoadClassCritical("ikvm.internal.IntrinsicThreadLocal").Finish();
						foreach (TypeBuilder tb in nestedTypeBuilders)
						{
							tb.CreateType();
						}
					}
#if STATIC_COMPILER
					if (tbFields != null)
					{
						tbFields.CreateType();
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
			private void AddAccessStubs()
			{
				/*
				 * There are two types of access stubs:
				 * 
				 * Type 1   These are required when a public class extends a non-public class.
				 *			In that case we need access stubs for all public and protected members
				 *			of the non-public base classes.
				 *
				 * Type 2	When a public class exposes a member that contains a non-public type in
				 *			its signature, we need an access stub for that member (where we convert
				 *			the non-public type in the signature to the first public base type).
				 *			Additionally, when a public or protected final field is compiled
				 *			without -strictfinalfieldsemantics, a the field will be wrapper with a
				 *			read-only property.
				 *
				 * Note that type 1 access stubs may also need the type 2 signature type widening
				 * if the signature contains non-public types.
				 * 
				 * Type 1 access stubs are always required, because the JVM allow access to these
				 * members via the derived class while the CLR doesn't. Historically, we've exposed
				 * these access stubs in such a way that they are also consumable from other .NET
				 * languages (when feasible), so we'll continue to do that for back compat.
				 * 
				 * Type 2 access stubs are only required by the CLR when running on CLR v4 and the
				 * caller assembly is security transparent code (level 2). We also want the access
				 * stubs to allow other .NET languages (e.g. C#) to consume broken APIs that
				 * (accidentally) expose these members.
				 */
				AddType2FieldAccessStubs();
				AddType1FieldAccessStubs(wrapper);
				if (!wrapper.IsInterface)
				{
					int id = 0;
					AddType2MethodAccessStubs(ref id);
					AddType1MethodAccessStubs(ref id);
				}
			}

			private void AddType1FieldAccessStubs(TypeWrapper tw)
			{
				do
				{
					if (!tw.IsPublic)
					{
						foreach (FieldWrapper fw in tw.GetFields())
						{
							if ((fw.IsPublic || (fw.IsProtected && !wrapper.IsFinal))
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
					if (wrapper.NeedsType2AccessStub(fw))
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
					// we attach the AccessStub custom modifier because the C# compiler prefers fields without custom modifiers
					// so if this class defines a field with the same name, that will be preferred over this one by the C# compiler
					FieldBuilder fb = typeBuilder.DefineField(fw.Name, fw.FieldTypeWrapper.TypeAsSignatureType, null, new Type[] { JVM.LoadType(typeof(IKVM.Attributes.AccessStub)) }, attribs);
					AttributeHelper.HideFromReflection(fb);
					fb.SetConstant(((ConstantFieldWrapper)fw).GetConstantValue());
				}
				else
				{
					Type propType = ToPublicSignatureType(fw.FieldTypeWrapper);
					Type[] modopt = wrapper.GetModOpt(fw.FieldTypeWrapper, true);
					PropertyBuilder pb = typeBuilder.DefineProperty(fw.Name, PropertyAttributes.None, propType, null, modopt, Type.EmptyTypes, null, null);
					if (type1)
					{
						AttributeHelper.HideFromReflection(pb);
					}
					else
					{
						AttributeHelper.SetModifiers(pb, fw.Modifiers, fw.IsInternal);
					}
					MethodAttributes attribs = fw.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
					attribs |= MethodAttributes.HideBySig | MethodAttributes.SpecialName;
					if (fw.IsStatic)
					{
						attribs |= MethodAttributes.Static;
					}
					// we append the IKVM.Attributes.AccessStub type to the modopt array for use in the property accessor method signature
					// to make sure they never conflict with any user defined methhods
					Type[] modopt2 = modopt;
					Array.Resize(ref modopt2, modopt2.Length + 1);
					modopt2[modopt2.Length - 1] = JVM.LoadType(typeof(IKVM.Attributes.AccessStub));
					MethodBuilder getter = typeBuilder.DefineMethod("get_" + fw.Name, attribs, CallingConventions.Standard, propType, null, modopt2, Type.EmptyTypes, null, null);
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
					if (!fw.IsFinal || (!fw.IsStatic && !type1))
					{
						if (fw.IsFinal)
						{
							// we need to generate a (private) setter for final fields for reflection and serialization
							attribs &= ~MethodAttributes.MemberAccessMask;
							attribs |= MethodAttributes.Private;
						}
						MethodBuilder setter = typeBuilder.DefineMethod("set_" + fw.Name, attribs, CallingConventions.Standard, null, null, null, new Type[] { propType }, null, new Type[][] { modopt2 });
						AttributeHelper.HideFromJava(setter);
						pb.SetSetMethod(setter);
						ilgen = CodeEmitter.Create(setter);
						ilgen.Emit(OpCodes.Ldarg_0);
						if (!fw.IsStatic)
						{
							ilgen.Emit(OpCodes.Ldarg_1);
						}
						// we don't do a DynamicCast if fw.FieldTypeWrapper is unloadable, because for normal unloadable fields we don't enfore the type either
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

			private void AddType1MethodAccessStubs(ref int id)
			{
				for (TypeWrapper tw = wrapper.BaseTypeWrapper; tw != null && !tw.IsPublic; tw = tw.BaseTypeWrapper)
				{
					foreach (MethodWrapper mw in tw.GetMethods())
					{
						if ((mw.IsPublic || (mw.IsProtected && !wrapper.IsFinal))
							&& (!mw.IsAbstract || wrapper.IsAbstract)
							&& mw.Name != StringConstants.INIT
							&& wrapper.GetMethodWrapper(mw.Name, mw.Signature, true) == mw
							&& ParametersAreAccessible(mw))
						{
							GenerateAccessStub(id, mw, true, true);
							if (!mw.IsStatic && !mw.IsFinal && !mw.IsAbstract && !wrapper.IsFinal)
							{
								GenerateAccessStub(id, mw, false, true);
							}
							id++;
						}
					}
				}
			}

			private void AddType2MethodAccessStubs(ref int id)
			{
				foreach (MethodWrapper mw in wrapper.GetMethods())
				{
					if (mw.HasNonPublicTypeInSignature
						&& (mw.IsPublic || (mw.IsProtected && !wrapper.IsFinal))
						&& ParametersAreAccessible(mw))
					{
						GenerateAccessStub(id, mw, true, false);
						if (!mw.IsStatic && !mw.IsFinal && !mw.IsAbstract && mw.Name != StringConstants.INIT && !wrapper.IsFinal)
						{
							GenerateAccessStub(id, mw, false, false);
						}
						id++;
					}
				}
			}

			private void GenerateAccessStub(int id, MethodWrapper mw, bool virt, bool type1)
			{
				Debug.Assert(!mw.HasCallerID);
				MethodAttributes stubattribs = mw.IsPublic && virt ? MethodAttributes.Public : MethodAttributes.FamORAssem;
				stubattribs |= MethodAttributes.HideBySig;
				if (mw.IsStatic)
				{
					stubattribs |= MethodAttributes.Static;
				}
				TypeWrapper[] parameters = mw.GetParameters();
				Type[] realParameterTypes = new Type[parameters.Length];
				Type[] parameterTypes = new Type[parameters.Length];
				Type[][] modopt = new Type[parameters.Length][];
				for (int i = 0; i < parameters.Length; i++)
				{
					realParameterTypes[i] = parameters[i].TypeAsSignatureType;
					parameterTypes[i] = ToPublicSignatureType(parameters[i]);
					modopt[i] = wrapper.GetModOpt(parameters[i], true);
				}
				Type returnType = ToPublicSignatureType(mw.ReturnType);
				Type[] modoptReturnType = wrapper.GetModOpt(mw.ReturnType, true);
				Array.Resize(ref modoptReturnType, modoptReturnType.Length + 1);
				modoptReturnType[modoptReturnType.Length - 1] = JVM.LoadType(typeof(IKVM.Attributes.AccessStub));
				string name;
				if (mw.Name == StringConstants.INIT)
				{
					name = ConstructorInfo.ConstructorName;
					stubattribs |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
				}
				else
				{
					name = virt
						? (mw.Modifiers & Modifiers.Bridge) == 0 ? mw.Name : NamePrefix.Bridge + mw.Name
						: NamePrefix.NonVirtual + id;
				}
				MethodBuilder mb = typeBuilder.DefineMethod(name, stubattribs, CallingConventions.Standard, returnType, null, modoptReturnType, parameterTypes, null, modopt);
				if (virt && type1)
				{
					AttributeHelper.HideFromReflection(mb);
					AttributeHelper.SetNameSig(mb, NamePrefix.AccessStub + id + "|" + mw.Name, mw.Signature);
				}
				else
				{
					AttributeHelper.HideFromJava(mb);
					if (!type1)
					{
						AttributeHelper.SetNameSig(mb, mw.Name, mw.Signature);
					}
				}
				CodeEmitter ilgen = CodeEmitter.Create(mb);
				int argpos = 0;
				if (!mw.IsStatic)
				{
					ilgen.EmitLdarg(argpos++);
				}
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					ilgen.EmitLdarg(argpos++);
					// we don't need to do a DynamicCast if for unloadables, because the method itself will already do that
					if (parameterTypes[i] != realParameterTypes[i])
					{
						ilgen.Emit(OpCodes.Castclass, realParameterTypes[i]);
					}
				}
				if (mw.IsStatic || !virt || mw.Name == StringConstants.INIT)
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

			private static Type ToPublicSignatureType(TypeWrapper tw)
			{
				return (tw.IsPublic ? tw : tw.GetPublicBaseTypeWrapper()).TypeAsSignatureType;
			}

			private bool ParametersAreAccessible(MethodWrapper mw)
			{
				foreach (TypeWrapper tw in mw.GetParameters())
				{
					if (!tw.IsAccessibleFrom(wrapper))
					{
						return false;
					}
				}
				return true;
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
							MethodBuilder mb = DefineInterfaceStubMethod(mangledName, ifmethod);
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
						MethodBuilder mb = DefineInterfaceStubMethod(NamePrefix.Incomplete + mangledName, ifmethod);
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilgen = CodeEmitter.Create(mb);
						ilgen.EmitThrow("java.lang.IllegalAccessError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
						ilgen.DoEmit();
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						wrapper.SetHasIncompleteInterfaceImplementation();
					}
					else if (mce.GetMethod() == null || mce.RealName != ifmethod.RealName || mce.IsInternal)
					{
						MethodBuilder mb = DefineInterfaceStubMethod(mangledName, ifmethod);
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilGenerator = CodeEmitter.Create(mb);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						int argc = mce.GetParameters().Length;
						for (int n = 0; n < argc; n++)
						{
							ilGenerator.EmitLdarg(n + 1);
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
						MethodBuilder mb = DefineInterfaceStubMethod(mangledName, ifmethod);
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilGenerator = CodeEmitter.Create(mb);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						int argc = mce.GetParameters().Length;
						for (int n = 0; n < argc; n++)
						{
							ilGenerator.EmitLdarg(n + 1);
						}
						mce.EmitCallvirt(ilGenerator);
						ilGenerator.Emit(OpCodes.Ret);
						ilGenerator.DoEmit();
					}
					else if (CheckRequireOverrideStub(mce, ifmethod))
					{
						wrapper.GenerateOverrideStub(typeBuilder, ifmethod, (MethodInfo)mce.GetMethod(), mce);
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
						MethodBuilder mb = DefineInterfaceStubMethod(NamePrefix.Incomplete + mangledName, ifmethod);
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilgen = CodeEmitter.Create(mb);
						ilgen.EmitThrow("java.lang.AbstractMethodError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
						ilgen.DoEmit();
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						wrapper.SetHasIncompleteInterfaceImplementation();
					}
				}
			}

			private MethodBuilder DefineInterfaceStubMethod(string name, MethodWrapper mw)
			{
				return mw.GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final);
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
				private readonly static ModuleBuilder mod;
				private static int count;

				static JniProxyBuilder()
				{
					mod = DynamicClassLoader.CreateJniProxyModuleBuilder();
					CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(JavaModuleAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
					mod.SetCustomAttribute(cab);
				}

				internal static void Generate(DynamicTypeWrapper.FinishContext context, CodeEmitter ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args)
				{
					TypeBuilder tb = mod.DefineType("__<jni>" + System.Threading.Interlocked.Increment(ref count), TypeAttributes.Public | TypeAttributes.Class);
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
					argTypes[argTypes.Length - 1] = CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
					Type retType = (mw.ReturnType.IsPrimitive || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType) ? mw.ReturnType.TypeAsSignatureType : typeof(object);
					MethodBuilder mb = tb.DefineMethod("method", MethodAttributes.Public | MethodAttributes.Static, retType, argTypes);
					AttributeHelper.HideFromJava(mb);
					CodeEmitter ilgen = CodeEmitter.Create(mb);
					JniBuilder.Generate(context, ilgen, wrapper, mw, tb, classFile, m, args, true);
					ilgen.DoEmit();
					tb.CreateType();
					for (int i = 0; i < argTypes.Length - 1; i++)
					{
						ilGenerator.EmitLdarg(i);
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
					ilGenerator.EmitBrtrue(oklabel);
					if (thruProxy)
					{
						ilGenerator.EmitLdarg(args.Length + (mw.IsStatic ? 0 : 1));
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
						ilGenerator.EmitLdarg(args.Length + (mw.IsStatic ? 0 : 1));
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
								ilGenerator.EmitLdarg(j + add);
								args[j].EmitBox(ilGenerator);
							}
							else if (!args[j].IsUnloadable && args[j].IsGhost)
							{
								ilGenerator.EmitLdarga(j + add);
								ilGenerator.Emit(OpCodes.Ldfld, args[j].GhostRefField);
							}
							else
							{
								ilGenerator.EmitLdarg(j + add);
							}
							ilGenerator.Emit(OpCodes.Call, makeLocalRef);
							modargs[j + 2] = Types.IntPtr;
						}
						else
						{
							ilGenerator.EmitLdarg(j + add);
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
					ilGenerator.EmitLeave(retLabel);
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
						ilgen.EmitBrfalse(label);
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
				// we don't need to support custom modifiers, because there aren't any callerid methods that have parameter types that require a custom modifier
				TypeWrapper[] parameters = mw.GetParameters();
				Type[] parameterTypes = new Type[parameters.Length];
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					parameterTypes[i] = parameters[i].TypeAsSignatureType;
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
				MethodBuilder mb = typeBuilder.DefineMethod(mw.Name, attribs, mw.ReturnType.TypeAsSignatureType, parameterTypes);
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
					ilgen.EmitLdarg(i);
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

			private void ImplementInterfaces(TypeWrapper[] interfaces, List<TypeWrapper> interfaceList)
			{
				foreach (TypeWrapper iface in interfaces)
				{
					if (!interfaceList.Contains(iface))
					{
						interfaceList.Add(iface);
						// NOTE we're using TypeAsBaseType for the interfaces!
						typeBuilder.AddInterfaceImplementation(iface.TypeAsBaseType);
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
				if (mb.IsGenericMethodDefinition)
				{
					CopyGenericArguments(mb, m);
				}
				CodeEmitter ilgen = CodeEmitter.Create(m);
				ilgen.EmitThrow("java.lang.AbstractMethodError", "Method " + mb.DeclaringType.FullName + "." + mb.Name + " is unsupported by IKVM.");
				ilgen.DoEmit();
				typeBuilder.DefineMethodOverride(m, (MethodInfo)mb);
			}

			private static void CopyGenericArguments(MethodBase mi, MethodBuilder mb)
			{
				Type[] genericParameters = mi.GetGenericArguments();
				string[] genParamNames = new string[genericParameters.Length];
				for (int i = 0; i < genParamNames.Length; i++)
				{
					genParamNames[i] = genericParameters[i].Name;
				}
				GenericTypeParameterBuilder[] genParamBuilders = mb.DefineGenericParameters(genParamNames);
				for (int i = 0; i < genParamBuilders.Length; i++)
				{
					// NOTE apparently we don't need to set the interface constraints
					// (and if we do, it fails for some reason)
					if (genericParameters[i].BaseType != Types.Object)
					{
						genParamBuilders[i].SetBaseTypeConstraint(genericParameters[i].BaseType);
					}
					genParamBuilders[i].SetGenericParameterAttributes(genericParameters[i].GenericParameterAttributes);
				}
			}

			private void CompileConstructorBody(FinishContext context, CodeEmitter ilGenerator, int methodIndex)
			{
				MethodWrapper[] methods = wrapper.GetMethods();
				ClassFile.Method m = classFile.Methods[methodIndex];
				TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
				// do we have an implementation in map.xml?
				if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
				{
					ilGenerator.DoEmit();
					return;
				}
#endif
				bool nonLeaf = false;
				Compiler.Compile(context, wrapper, methods[methodIndex], classFile, m, ilGenerator, ref nonLeaf);
				ilGenerator.DoEmit();
#if STATIC_COMPILER
				ilGenerator.EmitLineNumberTable((MethodBuilder)methods[methodIndex].GetMethod());
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
				TypeWrapper tw = CoreClasses.ikvm.@internal.CallerID.Wrapper;
				if (tw.InternalsVisibleTo(wrapper))
				{
					MethodWrapper create = tw.GetMethodWrapper("create", "(Lcli.System.RuntimeTypeHandle;)Likvm.internal.CallerID;", false);
					ilGenerator.Emit(OpCodes.Ldtoken, this.typeBuilder);
					create.Link();
					create.EmitCall(ilGenerator);
				}
				else
				{
					RegisterNestedTypeBuilder(EmitCreateCallerID(typeBuilder, ilGenerator));
				}
				ilGenerator.Emit(OpCodes.Stsfld, callerIDField);
			}

			internal static TypeBuilder EmitCreateCallerID(TypeBuilder typeBuilder, CodeEmitter ilGenerator)
			{
				TypeWrapper tw = CoreClasses.ikvm.@internal.CallerID.Wrapper;
				TypeBuilder typeCallerID = typeBuilder.DefineNestedType("__<CallerID>", TypeAttributes.Sealed | TypeAttributes.NestedPrivate, tw.TypeAsBaseType);
				MethodBuilder cb = ReflectUtil.DefineConstructor(typeCallerID, MethodAttributes.Assembly, null);
				CodeEmitter ctorIlgen = CodeEmitter.Create(cb);
				ctorIlgen.Emit(OpCodes.Ldarg_0);
				MethodWrapper mw = tw.GetMethodWrapper("<init>", "()V", false);
				mw.Link();
				mw.EmitCall(ctorIlgen);
				ctorIlgen.Emit(OpCodes.Ret);
				ctorIlgen.DoEmit();
				ilGenerator.Emit(OpCodes.Newobj, cb);
				return typeCallerID;
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
								ilGenerator.EmitLdc_I4((int)constant);
							}
							else if (constant is bool)
							{
								ilGenerator.EmitLdc_I4((bool)constant ? 1 : 0);
							}
							else if (constant is byte)
							{
								ilGenerator.EmitLdc_I4((byte)constant);
							}
							else if (constant is char)
							{
								ilGenerator.EmitLdc_I4((char)constant);
							}
							else if (constant is short)
							{
								ilGenerator.EmitLdc_I4((short)constant);
							}
							else if (constant is long)
							{
								ilGenerator.EmitLdc_I8((long)constant);
							}
							else if (constant is double)
							{
								ilGenerator.EmitLdc_R8((double)constant);
							}
							else if (constant is float)
							{
								ilGenerator.EmitLdc_R4((float)constant);
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

			internal MethodBuilder DefineThreadLocalType()
			{
				TypeWrapper threadLocal = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.IntrinsicThreadLocal");
				int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
				TypeBuilder tb = typeBuilder.DefineNestedType("__<tls>_" + id, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, threadLocal.TypeAsBaseType);
				FieldBuilder fb = tb.DefineField("field", Types.Object, FieldAttributes.Private | FieldAttributes.Static);
				fb.SetCustomAttribute(new CustomAttributeBuilder(JVM.Import(typeof(ThreadStaticAttribute)).GetConstructor(Type.EmptyTypes), new object[0]));
				MethodBuilder mbGet = tb.DefineMethod("get", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, Types.Object, Type.EmptyTypes);
				ILGenerator ilgen = mbGet.GetILGenerator();
				ilgen.Emit(OpCodes.Ldsfld, fb);
				ilgen.Emit(OpCodes.Ret);
				MethodBuilder mbSet = tb.DefineMethod("set", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, null, new Type[] { Types.Object });
				ilgen = mbSet.GetILGenerator();
				ilgen.Emit(OpCodes.Ldarg_1);
				ilgen.Emit(OpCodes.Stsfld, fb);
				ilgen.Emit(OpCodes.Ret);
				MethodBuilder cb = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, Type.EmptyTypes);
				CodeEmitter ctorilgen = CodeEmitter.Create(cb);
				ctorilgen.Emit(OpCodes.Ldarg_0);
				MethodWrapper basector = threadLocal.GetMethodWrapper("<init>", "()V", false);
				basector.Link();
				basector.EmitCall(ctorilgen);
				ctorilgen.Emit(OpCodes.Ret);
				ctorilgen.DoEmit();
				RegisterNestedTypeBuilder(tb);
				return cb;
			}

			internal MethodBuilder GetAtomicReferenceFieldUpdater(FieldWrapper field)
			{
				if (arfuMap == null)
				{
					arfuMap = new Dictionary<FieldWrapper, MethodBuilder>();
				}
				MethodBuilder cb;
				if (!arfuMap.TryGetValue(field, out cb))
				{
					TypeWrapper arfuTypeWrapper = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.IntrinsicAtomicReferenceFieldUpdater");
					TypeBuilder tb = typeBuilder.DefineNestedType("__<ARFU>_" + arfuMap.Count, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, arfuTypeWrapper.TypeAsBaseType);
					AtomicReferenceFieldUpdaterEmitter.EmitImpl(tb, field.GetField());
					cb = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, Type.EmptyTypes);
					arfuMap.Add(field, cb);
					CodeEmitter ctorilgen = CodeEmitter.Create(cb);
					ctorilgen.Emit(OpCodes.Ldarg_0);
					MethodWrapper basector = arfuTypeWrapper.GetMethodWrapper("<init>", "()V", false);
					basector.Link();
					basector.EmitCall(ctorilgen);
					ctorilgen.Emit(OpCodes.Ret);
					ctorilgen.DoEmit();
					RegisterNestedTypeBuilder(tb);
				}
				return cb;
			}

			internal TypeBuilder DefineIndyCallSiteType()
			{
				int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
				TypeBuilder tb = typeBuilder.DefineNestedType("__<>IndyCS" + id, TypeAttributes.NestedPrivate | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			internal TypeBuilder DefineMethodHandleConstantType(int index)
			{
				TypeBuilder tb = typeBuilder.DefineNestedType("__<>MHC" + index, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit); ;
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			internal TypeBuilder DefineMethodTypeConstantType(int index)
			{
				TypeBuilder tb = typeBuilder.DefineNestedType("__<>MTC" + index, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			internal MethodBuilder DefineMethodHandleDispatchStub(Type returnType, Type[] parameterTypes)
			{
				return typeBuilder.DefineMethod("__<>MHC", MethodAttributes.Static | MethodAttributes.PrivateScope, returnType, parameterTypes);
			}

			internal FieldBuilder DefineMethodHandleInvokeCacheField(Type fieldType)
			{
				return typeBuilder.DefineField("__<>invokeCache", fieldType, FieldAttributes.Static | FieldAttributes.PrivateScope);
			}

			internal FieldBuilder DefineDynamicMethodHandleCacheField()
			{
				return typeBuilder.DefineField("__<>dynamicMethodHandleCache", CoreClasses.java.lang.invoke.MethodHandle.Wrapper.TypeAsSignatureType, FieldAttributes.Static | FieldAttributes.PrivateScope);
			}

			internal FieldBuilder DefineDynamicMethodTypeCacheField()
			{
				return typeBuilder.DefineField("__<>dynamicMethodTypeCache", CoreClasses.java.lang.invoke.MethodType.Wrapper.TypeAsSignatureType, FieldAttributes.Static | FieldAttributes.PrivateScope);
			}

			internal MethodBuilder DefineDelegateInvokeErrorStub(Type returnType, Type[] parameterTypes)
			{
				return typeBuilder.DefineMethod("__<>", MethodAttributes.PrivateScope | MethodAttributes.Static, returnType, parameterTypes);
			}

			internal MethodInfo GetInvokeSpecialStub(MethodWrapper method)
			{
				if (invokespecialstubcache == null)
				{
					invokespecialstubcache = new Dictionary<MethodKey, MethodInfo>();
				}
				MethodKey key = new MethodKey(method.DeclaringType.Name, method.Name, method.Signature);
				MethodInfo mi;
				if (!invokespecialstubcache.TryGetValue(key, out mi))
				{
					DefineMethodHelper dmh = method.GetDefineMethodHelper();
					MethodBuilder stub = dmh.DefineMethod(wrapper, typeBuilder, "__<>", MethodAttributes.PrivateScope);
					CodeEmitter ilgen = CodeEmitter.Create(stub);
					ilgen.Emit(OpCodes.Ldarg_0);
					for (int i = 1; i <= dmh.ParameterCount; i++)
					{
						ilgen.EmitLdarg(i);
					}
					method.EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ret);
					ilgen.DoEmit();
					invokespecialstubcache[key] = stub;
					mi = stub;
				}
				return mi;
			}
		}

		private static bool CheckRequireOverrideStub(MethodWrapper mw1, MethodWrapper mw2)
		{
			// TODO this is too late to generate LinkageErrors so we need to figure this out earlier
			if (!TypesMatchForOverride(mw1.ReturnType, mw2.ReturnType))
			{
				return true;
			}
			TypeWrapper[] args1 = mw1.GetParameters();
			TypeWrapper[] args2 = mw2.GetParameters();
			for (int i = 0; i < args1.Length; i++)
			{
				if (!TypesMatchForOverride(args1[i], args2[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool TypesMatchForOverride(TypeWrapper tw1, TypeWrapper tw2)
		{
			if (tw1 == tw2)
			{
				return true;
			}
			else if (tw1.IsUnloadable && tw2.IsUnloadable)
			{
				return ((UnloadableTypeWrapper)tw1).CustomModifier == ((UnloadableTypeWrapper)tw2).CustomModifier;
			}
			else
			{
				return false;
			}
		}

		private void GenerateOverrideStub(TypeBuilder typeBuilder, MethodWrapper baseMethod, MethodInfo target, MethodWrapper targetMethod)
		{
			Debug.Assert(!baseMethod.HasCallerID);

			MethodBuilder overrideStub = baseMethod.GetDefineMethodHelper().DefineMethod(this, typeBuilder, "__<overridestub>" + baseMethod.Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final);
			typeBuilder.DefineMethodOverride(overrideStub, (MethodInfo)baseMethod.GetMethod());

			Type stubret = baseMethod.ReturnTypeForDefineMethod;
			Type[] stubargs = baseMethod.GetParametersForDefineMethod();
			Type targetRet = targetMethod.ReturnTypeForDefineMethod;
			Type[] targetArgs = targetMethod.GetParametersForDefineMethod();
			CodeEmitter ilgen = CodeEmitter.Create(overrideStub);
			ilgen.Emit(OpCodes.Ldarg_0);
			for (int i = 0; i < targetArgs.Length; i++)
			{
				ilgen.EmitLdarg(i + 1);
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
					parameterNames[i] = names[i];
				}
			}
		}

		protected static ParameterBuilder[] GetParameterBuilders(MethodBuilder mb, int parameterCount, string[] parameterNames)
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
				parameterBuilders[i] = mb.DefineParameter(i + 1, ParameterAttributes.None, name);
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
		protected abstract bool EmitMapXmlMethodPrologueAndOrBody(CodeEmitter ilgen, ClassFile f, ClassFile.Method m);
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

		internal override MethodBase GetSerializationConstructor()
		{
			return automagicSerializationCtor;
		}

		private Type[] GetModOpt(TypeWrapper tw, bool mustBePublic)
		{
			return GetModOpt(GetClassLoader().GetTypeWrapperFactory(), tw, mustBePublic);
		}

		internal static Type[] GetModOpt(TypeWrapperFactory context, TypeWrapper tw, bool mustBePublic)
		{
			Type[] modopt = Type.EmptyTypes;
			if (tw.IsUnloadable)
			{
				if (((UnloadableTypeWrapper)tw).MissingType == null)
				{
					modopt = new Type[] { ((UnloadableTypeWrapper)tw).GetCustomModifier(context) };
				}
			}
			else
			{
				TypeWrapper tw1 = tw.IsArray ? tw.GetUltimateElementTypeWrapper() : tw;
				if (tw1.IsErasedOrBoxedPrimitiveOrRemapped || tw.IsGhostArray || (mustBePublic && !tw1.IsPublic))
				{
					// FXBUG Ref.Emit refuses arrays in custom modifiers, so we add an array type for each dimension
					modopt = new Type[tw.ArrayRank + 1];
					modopt[0] = GetModOptHelper(tw1);
					for (int i = 1; i < modopt.Length; i++)
					{
						modopt[i] = Types.Array;
					}
				}
			}
			return modopt;
		}

		private static Type GetModOptHelper(TypeWrapper tw)
		{
			Debug.Assert(!tw.IsUnloadable);
			if (tw.IsArray)
			{
				return ArrayTypeWrapper.MakeArrayType(GetModOptHelper(tw.GetUltimateElementTypeWrapper()), tw.ArrayRank);
			}
			else if (tw.IsGhost)
			{
				return tw.TypeAsTBD;
			}
			else
			{
				return tw.TypeAsBaseType;
			}
		}

#if STATIC_COMPILER
		private bool NeedsType2AccessStub(FieldWrapper fw)
		{
			Debug.Assert(this.IsPublic && fw.DeclaringType == this);
			return fw.IsType2FinalField
				|| (fw.HasNonPublicTypeInSignature
					&& (fw.IsPublic || (fw.IsProtected && !this.IsFinal))
					&& (fw.FieldTypeWrapper.IsUnloadable || fw.FieldTypeWrapper.IsAccessibleFrom(this) || fw.FieldTypeWrapper.InternalsVisibleTo(this)));
		}
#endif
	}

	sealed class DefineMethodHelper
	{
		private readonly MethodWrapper mw;

		internal DefineMethodHelper(MethodWrapper mw)
		{
			this.mw = mw;
		}

		internal int ParameterCount
		{
			get { return mw.GetParameters().Length + (mw.HasCallerID ? 1 : 0); }
		}

		internal MethodBuilder DefineMethod(DynamicTypeWrapper context, TypeBuilder tb, string name, MethodAttributes attribs)
		{
			return DefineMethod(context.GetClassLoader().GetTypeWrapperFactory(), tb, name, attribs);
		}

		internal MethodBuilder DefineMethod(TypeWrapperFactory context, TypeBuilder tb, string name, MethodAttributes attribs)
		{
			// we add optional modifiers to make the signature unique
			TypeWrapper[] parameters = mw.GetParameters();
			Type[] parameterTypes = new Type[parameters.Length + (mw.HasCallerID ? 1 : 0)];
			Type[][] modopt = new Type[parameterTypes.Length][];
			for (int i = 0; i < parameters.Length; i++)
			{
				parameterTypes[i] = parameters[i].TypeAsSignatureType;
				modopt[i] = DynamicTypeWrapper.GetModOpt(context, parameters[i], false);
			}
			if (mw.HasCallerID)
			{
				parameterTypes[parameterTypes.Length - 1] = CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
			}
			Type[] modoptReturnType = DynamicTypeWrapper.GetModOpt(context, mw.ReturnType, false);
			return tb.DefineMethod(name, attribs, CallingConventions.Standard, mw.ReturnType.TypeAsSignatureType, null, modoptReturnType, parameterTypes, null, modopt);
		}

		internal MethodBuilder DefineConstructor(DynamicTypeWrapper context, TypeBuilder tb, MethodAttributes attribs)
		{
			return DefineConstructor(context.GetClassLoader().GetTypeWrapperFactory(), tb, attribs);
		}

		internal MethodBuilder DefineConstructor(TypeWrapperFactory context, TypeBuilder tb, MethodAttributes attribs)
		{
			return DefineMethod(context, tb, ConstructorInfo.ConstructorName, attribs | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
		}
	}
}
