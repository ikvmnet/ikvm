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
#pragma warning disable 628 // don't complain about protected members in sealed type
	sealed class DynamicTypeWrapper : TypeWrapper
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

		private TypeWrapper LoadTypeWrapper(ClassLoaderWrapper classLoader, ProtectionDomain pd, ClassFile.ConstantPoolItemClass clazz)
		{
			// check for patched constant pool items
			TypeWrapper tw = clazz.GetClassType();
			if (tw == null || tw == VerifierTypeWrapper.Null)
			{
				tw = classLoader.LoadClassByDottedNameFast(clazz.Name);
			}
			if (tw == null)
			{
				throw new NoClassDefFoundError(clazz.Name);
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
		internal DynamicTypeWrapper(TypeWrapper host, ClassFile f, CompilerClassLoader classLoader, ProtectionDomain pd)
#else
		internal DynamicTypeWrapper(TypeWrapper host, ClassFile f, ClassLoaderWrapper classLoader, ProtectionDomain pd)
#endif
			: base(f.IsInternal ? TypeFlags.InternalAccess : host != null ? TypeFlags.Anonymous : TypeFlags.None, f.Modifiers, f.Name)
		{
			Profiler.Count("DynamicTypeWrapper");
			this.classLoader = classLoader;
			this.sourceFileName = f.SourceFileAttribute;
			if (f.IsInterface)
			{
				// interfaces can't "override" final methods in object
				foreach (ClassFile.Method method in f.Methods)
				{
					MethodWrapper mw;
					if (method.IsVirtual
						&& (mw = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(method.Name, method.Signature, false)) != null
						&& mw.IsVirtual
						&& mw.IsFinal)
					{
						throw new VerifyError("class " + f.Name + " overrides final method " + method.Name + "." + method.Signature);
					}
				}
			}
			else
			{
				this.baseTypeWrapper = LoadTypeWrapper(classLoader, pd, f.SuperClass);
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
				TypeWrapper iface = LoadTypeWrapper(classLoader, pd, interfaces[i]);
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

			impl = new JavaTypeImpl(host, f, this);
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
			internal abstract MethodParametersEntry[] GetMethodParameters(int index);
			internal abstract object[] GetFieldAnnotations(int index);
			internal abstract MethodInfo GetFinalizeMethod();
			internal abstract object[] GetConstantPool();
			internal abstract byte[] GetRawTypeAnnotations();
			internal abstract byte[] GetMethodRawTypeAnnotations(int index);
			internal abstract byte[] GetFieldRawTypeAnnotations(int index);
			internal abstract TypeWrapper Host { get; }
		}

		private sealed class JavaTypeImpl : DynamicImpl
		{
			private readonly TypeWrapper host;
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
			private int recursionCount;
#if STATIC_COMPILER
			private DynamicTypeWrapper enclosingClassWrapper;
			private AnnotationBuilder annotationBuilder;
			private TypeBuilder enumBuilder;
			private TypeBuilder privateInterfaceMethods;
			private Dictionary<string, TypeWrapper> nestedTypeNames;	// only keys are used, values are always null
#endif

			internal JavaTypeImpl(TypeWrapper host, ClassFile f, DynamicTypeWrapper wrapper)
			{
				Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
				this.host = host;
				this.classFile = f;
				this.wrapper = (DynamicOrAotTypeWrapper)wrapper;
			}

			internal void CreateStep1()
			{
				// process all methods (needs to be done first, because property fields depend on being able to lookup the accessor methods)
				bool hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
				methods = new MethodWrapper[classFile.Methods.Length];
				baseMethods = new MethodWrapper[classFile.Methods.Length][];
				for (int i = 0; i < methods.Length; i++)
				{
					MemberFlags flags = MemberFlags.None;
					ClassFile.Method m = classFile.Methods[i];
					if (m.IsClassInitializer)
					{
#if STATIC_COMPILER
						bool noop;
						if (IsSideEffectFreeStaticInitializerOrNoop(m, out noop))
						{
							if (noop)
							{
								flags |= MemberFlags.NoOp;
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
					if (m.IsInternal)
					{
						flags |= MemberFlags.InternalAccess;
					}
#if STATIC_COMPILER
					if (m.IsCallerSensitive && SupportsCallerID(m))
					{
						flags |= MemberFlags.CallerID;
					}
#endif
					if (wrapper.IsGhost && m.IsVirtual)
					{
						// note that a GhostMethodWrapper can also represent a default interface method
						methods[i] = new GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
					}
					else if (m.IsConstructor && wrapper.IsDelegate)
					{
						methods[i] = new DelegateConstructorMethodWrapper(wrapper, m);
					}
					else if (classFile.IsInterface && !m.IsStatic && !m.IsPublic)
					{
						// we can't use callvirt to call interface private instance methods (because we have to compile them as static methods,
						// since the CLR doesn't support interface instance methods), so need a special MethodWrapper
						methods[i] = new PrivateInterfaceMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
					}
					else if (classFile.IsInterface && m.IsVirtual && !m.IsAbstract)
					{
						// note that a GhostMethodWrapper can also represent a default interface method
						methods[i] = new DefaultInterfaceMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
					}
					else
					{
						if (!classFile.IsInterface && m.IsVirtual)
						{
							bool explicitOverride;
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
						fieldType = ClassLoaderWrapper.GetBootstrapClassLoader().FieldTypeWrapperFromSig(fld.Signature, LoadMode.LoadOrThrow);
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

#if STATIC_COMPILER
			private bool SupportsCallerID(ClassFile.Method method)
			{
				if ((classFile.Name == "sun.reflect.Reflection" && method.Name == "getCallerClass")
					|| (classFile.Name == "java.lang.SecurityManager" && method.Name == "checkMemberAccess"))
				{
					// ignore CallerSensitive on methods that don't need CallerID parameter
					return false;
				}
				else if (method.IsStatic)
				{
					return true;
				}
				else if ((classFile.IsFinal || classFile.Name == "java.lang.Runtime" || classFile.Name == "java.io.ObjectStreamClass")
					&& wrapper.BaseTypeWrapper.GetMethodWrapper(method.Name, method.Signature, true) == null
					&& !HasInterfaceMethod(wrapper, method.Name, method.Signature))
				{
					// We only support CallerID instance methods on final or effectively final types,
					// because we don't support interface stubs with CallerID.
					// We also don't support a CallerID method overriding a method or implementing an interface.
					return true;
				}
				else if (RequiresDynamicReflectionCallerClass(classFile.Name, method.Name, method.Signature))
				{
					// We don't support CallerID for virtual methods that can be overridden or implement an interface,
					// so these methods will do a dynamic stack walk if when Reflection.getCallerClass() is used.
					return false;
				}
				else
				{
					// If we end up here, we either have to add support or add them to the white-list in the above clause
					// to allow them to fall back to dynamic stack walking.
					StaticCompiler.IssueMessage(Message.CallerSensitiveOnUnsupportedMethod, classFile.Name, method.Name, method.Signature);
					return false;
				}
			}

			private static bool HasInterfaceMethod(TypeWrapper tw, string name, string signature)
			{
				for (; tw != null; tw = tw.BaseTypeWrapper)
				{
					foreach (TypeWrapper iface in tw.Interfaces)
					{
						if (iface.GetMethodWrapper(name, signature, false) != null)
						{
							return true;
						}
						if (HasInterfaceMethod(iface, name, signature))
						{
							return true;
						}
					}
				}
				return false;
			}
#endif

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
					TypeBuilder enclosing = null;
					string enclosingClassName = null;
					// we only compile inner classes as nested types in the static compiler, because it has a higher cost
					// and doesn't buy us anything in dynamic mode (and if fact, due to an FXBUG it would make handling
					// the TypeResolve event very hard)
					ClassFile.InnerClass outerClass = getOuterClass();
					if (outerClass.outerClass != 0)
					{
						enclosingClassName = classFile.GetConstantPoolClass(outerClass.outerClass);
					}
					else if (f.EnclosingMethod != null)
					{
						enclosingClassName = f.EnclosingMethod[0];
					}
					if (enclosingClassName != null)
					{
						if (!CheckInnerOuterNames(f.Name, enclosingClassName))
						{
							Tracer.Warning(Tracer.Compiler, "Incorrect {0} attribute on {1}", outerClass.outerClass != 0 ? "InnerClasses" : "EnclosingMethod", f.Name);
						}
						else
						{
							try
							{
								enclosingClassWrapper = wrapper.classLoader.LoadClassByDottedNameFast(enclosingClassName) as DynamicTypeWrapper;
							}
							catch (RetargetableJavaException x)
							{
								Tracer.Warning(Tracer.Compiler, "Unable to load outer class {0} for inner class {1} ({2}: {3})", enclosingClassName, f.Name, x.GetType().Name, x.Message);
							}
							if (enclosingClassWrapper != null)
							{
								// make sure the relationship is reciprocal (otherwise we run the risk of
								// baking the outer type before the inner type) and that the inner and outer
								// class live in the same class loader (when doing a multi target compilation,
								// it is possible to split the two classes across assemblies)
								JavaTypeImpl oimpl = enclosingClassWrapper.impl as JavaTypeImpl;
								if (oimpl != null && enclosingClassWrapper.GetClassLoader() == wrapper.GetClassLoader())
								{
									ClassFile outerClassFile = oimpl.classFile;
									ClassFile.InnerClass[] outerInnerClasses = outerClassFile.InnerClasses;
									if (outerInnerClasses == null)
									{
										enclosingClassWrapper = null;
									}
									else
									{
										bool ok = false;
										for (int i = 0; i < outerInnerClasses.Length; i++)
										{
											if (((outerInnerClasses[i].outerClass != 0 && outerClassFile.GetConstantPoolClass(outerInnerClasses[i].outerClass) == outerClassFile.Name)
													|| (outerInnerClasses[i].outerClass == 0 && outerClass.outerClass == 0))
												&& outerInnerClasses[i].innerClass != 0
												&& outerClassFile.GetConstantPoolClass(outerInnerClasses[i].innerClass) == f.Name)
											{
												ok = true;
												break;
											}
										}
										if (!ok)
										{
											enclosingClassWrapper = null;
										}
									}
								}
								else
								{
									enclosingClassWrapper = null;
								}
								if (enclosingClassWrapper != null)
								{
									enclosingClassWrapper.CreateStep2();
									enclosing = oimpl.typeBuilder;
									if (outerClass.outerClass == 0)
									{
										// we need to record that we're not an inner classes, but an enclosed class
										typeAttribs |= TypeAttributes.SpecialName;
									}
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
						if (enclosing != null)
						{
							if (enclosingClassWrapper.IsPublic)
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
					else if (enclosing != null)
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
						if (enclosing != null && !cantNest)
						{
							if (wrapper.IsGhost)
							{
								// TODO this is low priority, since the current Java class library doesn't define any ghost interfaces
								// as inner classes
								throw new NotImplementedException();
							}
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = enclosing.DefineNestedType(AllocNestedTypeName(enclosingClassWrapper.Name, f.Name), typeAttribs);
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
						if (enclosing != null && !cantNest)
						{
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = enclosing.DefineNestedType(AllocNestedTypeName(enclosingClassWrapper.Name, f.Name), typeAttribs);
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
					if (outerClass.outerClass != 0)
					{
						if (enclosing != null && cantNest)
						{
							AttributeHelper.SetNonNestedInnerClass(enclosing, f.Name);
						}
						if (enclosing == null || cantNest)
						{
							AttributeHelper.SetNonNestedOuterClass(typeBuilder, enclosingClassName);
						}
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
					if (typeBuilder.FullName != wrapper.Name
						&& wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
					{
						wrapper.classLoader.AddNameMapping(wrapper.Name, typeBuilder.FullName);
					}
					if (f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
					{
						annotationBuilder = new AnnotationBuilder(this, enclosing);
						wrapper.SetAnnotation(annotationBuilder);
					}
					// For Java 5 Enum types, we generate a nested .NET enum.
					// This is primarily to support annotations that take enum parameters.
					if (f.IsEnum && f.IsPublic)
					{
						AddCliEnum();
					}
					AddInnerClassAttribute(enclosing != null, outerClass.innerClass != 0, mangledTypeName, outerClass.accessFlags);
					if (classFile.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(classFile.Annotations))
					{
						AttributeHelper.SetDeprecatedAttribute(typeBuilder);
					}
					if (classFile.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(typeBuilder, classFile.GenericSignature);
					}
					if (classFile.EnclosingMethod != null)
					{
						if (outerClass.outerClass == 0 && enclosing != null && !cantNest)
						{
							// we don't need to record the enclosing type, if we're compiling the current type as a nested type because of the EnclosingMethod attribute
							AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, null, classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
						}
						else
						{
							AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, classFile.EnclosingMethod[0], classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
						}
					}
					if (classFile.RuntimeVisibleTypeAnnotations != null)
					{
						AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(typeBuilder, classFile.RuntimeVisibleTypeAnnotations);
					}
					if (wrapper.classLoader.EmitStackTraceInfo)
					{
						if (f.SourceFileAttribute != null)
						{
							if ((enclosingClassWrapper == null && f.SourceFileAttribute == typeBuilder.Name + ".java")
								|| (enclosingClassWrapper != null && f.SourceFileAttribute == enclosingClassWrapper.sourceFileName))
							{
								// we don't need to record the name because it matches our heuristic
							}
							else
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
						AddClinitTrigger();
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

#if STATIC_COMPILER
			private void AddInnerClassAttribute(bool isNestedType, bool isInnerClass, string mangledTypeName, Modifiers innerClassFlags)
			{
				string name = classFile.Name;

				if (isNestedType)
				{
					if (name == enclosingClassWrapper.Name + "$" + typeBuilder.Name)
					{
						name = null;
					}
				}
				else if (name == mangledTypeName)
				{
					name = null;
				}

				if ((isInnerClass && CompiledTypeWrapper.PredictReflectiveModifiers(wrapper) != innerClassFlags) || name != null)
				{
					// HACK we abuse the InnerClassAttribute to record to real name for non-inner classes as well
					AttributeHelper.SetInnerClass(typeBuilder, name, isInnerClass ? innerClassFlags : wrapper.Modifiers);
				}
			}

			private void AddCliEnum()
			{
				CompilerClassLoader ccl = wrapper.classLoader;
				string name = "__Enum";
				while (!ccl.ReserveName(classFile.Name + "$" + name))
				{
					name += "_";
				}
				enumBuilder = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, Types.Enum);
				AttributeHelper.HideFromJava(enumBuilder);
				enumBuilder.DefineField("value__", Types.Int32, FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
				for (int i = 0; i < classFile.Fields.Length; i++)
				{
					if (classFile.Fields[i].IsEnum)
					{
						FieldBuilder fieldBuilder = enumBuilder.DefineField(classFile.Fields[i].Name, enumBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
						fieldBuilder.SetConstant(i);
					}
				}
				wrapper.SetEnumType(enumBuilder);
			}
#endif

			private void AddClinitTrigger()
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
					NormalizedByteCode bc;
					while ((bc = m.Instructions[i].NormalizedOpCode) == NormalizedByteCode.__goto)
					{
						int target = m.Instructions[i].TargetIndex;
						if (target <= i)
						{
							// backward branch means we can't do anything
							noop = false;
							return false;
						}
						// we must skip the unused instructions because the "remove assertions" optimization
						// uses a goto to remove the (now unused) code
						i = target;
					}
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
						ClassFile.Field field = classFile.GetField(fld.Name, fld.Signature);
						if (field == null)
						{
							noop = false;
							return false;
						}
						if (bc == NormalizedByteCode.__putstatic)
						{
							if (field.IsProperty && field.PropertySetter != null)
							{
								noop = false;
								return false;
							}
						}
						else if (field.IsProperty && field.PropertyGetter != null)
						{
							noop = false;
							return false;
						}
					}
					else if (ByteCodeMetaData.CanThrowException(bc))
					{
						noop = false;
						return false;
					}
					else if (bc == NormalizedByteCode.__aconst_null
						|| (bc == NormalizedByteCode.__iconst && m.Instructions[i].Arg1 == 0)
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
					new MethodAnalyzer(null, wrapper, null, classFile, m, wrapper.classLoader);
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
						// skip <clinit> and non-virtual interface methods introduced in Java 8
						if (ifmethod.IsVirtual)
						{
							TypeWrapper lookup = wrapper;
							while (lookup != null)
							{
								MethodWrapper mw = GetMethodWrapperDuringCtor(lookup, methods, ifmethod.Name, ifmethod.Signature);
								if (mw == null || (mw.IsMirandaMethod && mw.DeclaringType != wrapper))
								{
									mw = MirandaMethodWrapper.Create(wrapper, ifmethod);
									methods.Add(mw);
									baseMethods.Add(new MethodWrapper[] { ifmethod });
									break;
								}
								if (mw.IsMirandaMethod && mw.DeclaringType == wrapper)
								{
									methods[methods.IndexOf(mw)] = ((MirandaMethodWrapper)mw).Update(ifmethod);
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
							methods = ArrayUtil.Concat(methods, mw);
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
				return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.StartsWith(outer, StringComparison.Ordinal);
			}

			private string AllocNestedTypeName(string outer, string inner)
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
#else
						// when we're finishing types to save a debug image (in dynamic mode) we don't care about loader constraints anymore
						// (and we can't throw a LinkageError, because that would prevent the debug image from being saved)
						if (!JVM.FinishingForDebugSave)
						{
							throw new LinkageError("Loader constraints violated");
						}
#endif
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
#else
							// when we're finishing types to save a debug image (in dynamic mode) we don't care about loader constraints anymore
							// (and we can't throw a LinkageError, because that would prevent the debug image from being saved)
							if (!JVM.FinishingForDebugSave)
							{
								throw new LinkageError("Loader constraints violated");
							}
#endif
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
				if (wrapper.GetClassLoader().RemoveUnusedFields
					&& fw.IsPrivate
					&& fw.IsStatic
					&& fw.IsFinal
					&& !fw.IsSerialVersionUID
					&& classFile.Fields[fieldIndex].Annotations == null
					&& !classFile.IsReferenced(classFile.Fields[fieldIndex]))
				{
					// unused, so we skip it
					Tracer.Info(Tracer.Compiler, "Unused field {0}::{1}", wrapper.Name, fw.Name);
					return null;
				}
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
				FieldAttributes attribs = 0;
				string realFieldName = UnicodeUtil.EscapeInvalidSurrogates(fld.Name);
				if (!ReferenceEquals(realFieldName, fld.Name))
				{
					attribs |= FieldAttributes.SpecialName;
				}
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
					field = DefineField(realFieldName, fw.FieldTypeWrapper, attribs, false);
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
						realFieldName = NamePrefix.Type2AccessStubBackingField + realFieldName;
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
					if (fld.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(fld.Annotations))
					{
						AttributeHelper.SetDeprecatedAttribute(field);
					}
					if (fld.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(field, fld.GenericSignature);
					}
					if (fld.RuntimeVisibleTypeAnnotations != null)
					{
						AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(field, fld.RuntimeVisibleTypeAnnotations);
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
				TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
				if (baseTypeWrapper != null)
				{
					baseTypeWrapper.Finish();
					baseTypeWrapper.LinkAll();
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
				foreach (TypeWrapper iface in wrapper.interfaces)
				{
					iface.Finish();
					iface.LinkAll();
				}
				// make sure all classes are loaded, before we start finishing the type. During finishing, we
				// may not run any Java code, because that might result in a request to finish the type that we
				// are in the process of finishing, and this would be a problem.
				// Prevent infinity recursion for broken class loaders by keeping a recursion count and falling
				// back to late binding if we recurse more than twice.
				LoadMode mode = System.Threading.Interlocked.Increment(ref recursionCount) > 2 || (JVM.DisableEagerClassLoading && wrapper.Name != "sun.reflect.misc.Trampoline")
					? LoadMode.ReturnUnloadable
					: LoadMode.Link;
				try
				{
					classFile.Link(wrapper, mode);
					for (int i = 0; i < fields.Length; i++)
					{
						fields[i].Link(mode);
					}
					for (int i = 0; i < methods.Length; i++)
					{
						methods[i].Link(mode);
					}
				}
				finally
				{
					System.Threading.Interlocked.Decrement(ref recursionCount);
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
						CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(AnnotationAttributeAttribute)).GetConstructor(new Type[] { Types.String }),
							new object[] { UnicodeUtil.EscapeInvalidSurrogates(annotationBuilder.AttributeTypeName) });
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

					FinishContext context = new FinishContext(host, classFile, wrapper, typeBuilder);
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
					if (privateInterfaceMethods != null)
					{
						privateInterfaceMethods.CreateType();
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
					finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, wrapper.ReflectiveModifiers, Metadata.Create(classFile), finishedClinitMethod, finalizeMethod, host);
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
					string name = UnicodeUtil.EscapeInvalidSurrogates(o.classFile.Name);
					while (!ccl.ReserveName(name + "Attribute"))
					{
						name += "_";
					}

					TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Sealed;
					if (o.enclosingClassWrapper != null)
					{
						if (o.wrapper.IsPublic)
						{
							typeAttributes |= TypeAttributes.NestedPublic;
						}
						else
						{
							typeAttributes |= TypeAttributes.NestedAssembly;
						}
						attributeTypeBuilder = outer.DefineNestedType(o.AllocNestedTypeName(o.enclosingClassWrapper.Name, name + "Attribute"), typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
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
								Annotation annotation = Annotation.Load(o.wrapper, def);
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
						else if (tw.EnumType != null)	// is it a Java enum?
						{
							argType = tw.EnumType;
						}
						else if (IsDotNetEnum(tw))
						{
							argType = tw.DeclaringTypeWrapper.TypeAsSignatureType;
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

				private static bool IsDotNetEnum(TypeWrapper tw)
				{
					return tw.IsFakeNestedType && (tw.Modifiers & Modifiers.Enum) != 0;
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
					else if (tw.EnumType != null)	// is it a Java enum?
					{
						ilgen.Emit(OpCodes.Box, tw.EnumType);
					}
					else if (IsDotNetEnum(tw))
					{
						ilgen.Emit(OpCodes.Box, tw.DeclaringTypeWrapper.TypeAsSignatureType);
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
						// skip <clinit> and non-virtual interface methods introduced in Java 8
						if (o.methods[i].IsVirtual)
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

				private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
				{
					Link();
					ConstructorInfo ctor = defineConstructor != null
						? defineConstructor.__AsConstructorInfo()
						: StaticCompiler.GetRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute").GetConstructor(new Type[] { Types.Object.MakeArrayType() });
					return new CustomAttributeBuilder(ctor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
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
				// UPDATE as of JDK 7u65 and JDK 8u11, the algorithm changed again to handle package private methods differently
				// this code has not been updated to reflect these changes (we're still at JDK 8 GA level)
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
				Debug.Assert(mw != null);
				if (mw is DelegateConstructorMethodWrapper)
				{
					((DelegateConstructorMethodWrapper)mw).DoLink(typeBuilder);
					return null;
				}
				if (mw is DelegateInvokeStubMethodWrapper)
				{
					return ((DelegateInvokeStubMethodWrapper)mw).DoLink(typeBuilder);
				}
				if (mw.IsClassInitializer && mw.IsNoOp && (!wrapper.IsSerializable || HasSerialVersionUID))
				{
					// we don't need to emit the <clinit>, because it is empty and we're not serializable or have an explicit serialVersionUID
					// (because we cannot affect serialVersionUID computation (which is the only way the presence of a <clinit> can surface)
					// we cannot do this optimization if the class is serializable but doesn't have a serialVersionUID)
					return null;
				}
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
							// We're a Miranda method or we're an inherited default interface method
							Debug.Assert(baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface);
							MirandaMethodWrapper mmw = (MirandaMethodWrapper)methods[index];
							MethodAttributes attr = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
							MethodWrapper baseMiranda = null;
							bool baseMirandaOverrideStub = false;
							if (wrapper.BaseTypeWrapper == null || (baseMiranda = wrapper.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true)) == null || !baseMiranda.IsMirandaMethod)
							{
								// we're not overriding a miranda method in a base class, so can we set the newslot flag
								attr |= MethodAttributes.NewSlot;
							}
							else
							{
								baseMiranda.Link();
								if (CheckRequireOverrideStub(methods[index], baseMiranda))
								{
									baseMirandaOverrideStub = true;
									attr |= MethodAttributes.NewSlot;
								}
							}
							if (wrapper.IsInterface || (wrapper.IsAbstract && mmw.BaseMethod.IsAbstract && mmw.Error == null))
							{
								attr |= MethodAttributes.Abstract;
							}
							MethodBuilder mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, methods[index].Name, attr);
							AttributeHelper.HideFromReflection(mb);
							if (baseMirandaOverrideStub)
							{
								wrapper.GenerateOverrideStub(typeBuilder, baseMiranda, mb, methods[index]);
							}
							if ((!wrapper.IsAbstract && mmw.BaseMethod.IsAbstract) || (!wrapper.IsInterface && mmw.Error != null))
							{
								string message = mmw.Error ?? (wrapper.Name + "." + methods[index].Name + methods[index].Signature);
								CodeEmitter ilgen = CodeEmitter.Create(mb);
								ilgen.EmitThrow(mmw.IsConflictError ? "java.lang.IncompatibleClassChangeError" : "java.lang.AbstractMethodError", message);
								ilgen.DoEmit();
								wrapper.EmitLevel4Warning(mmw.IsConflictError ? HardError.IncompatibleClassChangeError : HardError.AbstractMethodError, message);
							}
#if STATIC_COMPILER
							if (wrapper.IsInterface && !mmw.IsAbstract)
							{
								// even though we're not visible to reflection., we need to record the fact that we have a default implementation
								AttributeHelper.SetModifiers(mb, mmw.Modifiers, false);
							}
#endif
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
						method = ReflectUtil.DefineTypeInitializer(typeBuilder, wrapper.classLoader);
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
					if (m.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(m.Annotations))
					{
						AttributeHelper.SetDeprecatedAttribute(method);
					}
					if (m.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(method, m.GenericSignature);
					}
					if (wrapper.GetClassLoader().NoParameterReflection)
					{
						// ignore MethodParameters (except to extract parameter names)
					}
					else if (m.MalformedMethodParameters)
					{
						AttributeHelper.SetMethodParametersAttribute(method, null);
					}
					else if (m.MethodParameters != null)
					{
						Modifiers[] modifiers = new Modifiers[m.MethodParameters.Length];
						for (int i = 0; i < modifiers.Length; i++)
						{
							modifiers[i] = (Modifiers)m.MethodParameters[i].flags;
						}
						AttributeHelper.SetMethodParametersAttribute(method, modifiers);
					}
					if (m.RuntimeVisibleTypeAnnotations != null)
					{
						AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(method, m.RuntimeVisibleTypeAnnotations);
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

			private bool HasSerialVersionUID
			{
				get
				{
					foreach (FieldWrapper field in fields)
					{
						if (field.IsSerialVersionUID)
						{
							return true;
						}
					}
					return false;
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
				if (m.IsAbstract || (!m.IsStatic && m.IsPublic && classFile.IsInterface))
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
							if (!m.IsAbstract)
							{
								setModifiers = true;
							}
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
					if (m.IsVirtual)
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
				string name = UnicodeUtil.EscapeInvalidSurrogates(m.Name);
				if (!ReferenceEquals(name, m.Name))
				{
					// mark as specialname to remind us to unescape the name
					attribs |= MethodAttributes.SpecialName;
				}
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
				mb = wrapper.DefineGhostMethod(typeBuilder, name, attribs, methods[index]);
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
					if (classFile.IsInterface && !m.IsPublic && !wrapper.IsGhost)
					{
						TypeBuilder tb = typeBuilder;
#if STATIC_COMPILER
						if (wrapper.IsPublic && wrapper.classLoader.WorkaroundInterfacePrivateMethods)
						{
							// FXBUG csc.exe doesn't like non-public methods in interfaces, so we put them in a nested type
							if (privateInterfaceMethods == null)
							{
								privateInterfaceMethods = typeBuilder.DefineNestedType(NestedTypeName.PrivateInterfaceMethods,
									TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
							}
							tb = privateInterfaceMethods;
							attribs &= ~MethodAttributes.MemberAccessMask;
							attribs |= MethodAttributes.Assembly;
						}
#endif
						if (m.IsStatic)
						{
							mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, tb, name, attribs);
						}
						else
						{
							// the CLR doesn't allow (non-virtual) instance methods in interfaces,
							// so we need to turn it into a static method
							mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
								tb, NamePrefix.PrivateInterfaceInstanceMethod + name, attribs | MethodAttributes.Static | MethodAttributes.SpecialName,
								typeBuilder, false);
#if STATIC_COMPILER
							AttributeHelper.SetNameSig(mb, m.Name, m.Signature);
#endif
						}
						setModifiers = true;
					}
					else
					{
						mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, attribs);
					}
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
						CustomAttributeBuilder cab = new CustomAttributeBuilder(StaticCompiler.GetRuntimeType("IKVM.Attributes.AnnotationDefaultAttribute").GetConstructor(new Type[] { Types.Object }),
							new object[] { AnnotationDefaultAttribute.Escape(classFile.Methods[index].AnnotationDefault) });
						mb.SetCustomAttribute(cab);
					}
#endif // STATIC_COMPILER
				}

				if ((methods[index].Modifiers & (Modifiers.Synchronized | Modifiers.Static)) == Modifiers.Synchronized)
				{
					mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);
				}

				if (classFile.Methods[index].IsForceInline)
				{
					const MethodImplAttributes AggressiveInlining = (MethodImplAttributes)256;
					mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | AggressiveInlining);
				}

				if (classFile.Methods[index].IsLambdaFormCompiled || classFile.Methods[index].IsLambdaFormHidden)
				{
					HideFromJavaFlags flags = HideFromJavaFlags.None;
					if (classFile.Methods[index].IsLambdaFormCompiled)
					{
						flags |= HideFromJavaFlags.StackWalk;
					}
					if (classFile.Methods[index].IsLambdaFormHidden)
					{
						flags |= HideFromJavaFlags.StackTrace;
					}
					AttributeHelper.HideFromJava(mb, flags);
				}

				if (classFile.IsInterface && methods[index].IsVirtual && !methods[index].IsAbstract)
				{
					if (wrapper.IsGhost)
					{
						DefaultInterfaceMethodWrapper.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
							typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.SpecialName,
							null, false));
					}
					else
					{
						DefaultInterfaceMethodWrapper.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
							typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName,
							typeBuilder, false));
					}
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

			internal override MethodParametersEntry[] GetMethodParameters(int index)
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

			internal override object[] GetConstantPool()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override byte[] GetRawTypeAnnotations()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override byte[] GetMethodRawTypeAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override byte[] GetFieldRawTypeAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override TypeWrapper Host
			{
				get { return host; }
			}
		}

		private sealed class Metadata
		{
			private readonly string[][] genericMetaData;
			private readonly object[][] annotations;
			private readonly MethodParametersEntry[][] methodParameters;
			private readonly byte[][][] runtimeVisibleTypeAnnotations;
			private readonly object[] constantPool;

			private Metadata(string[][] genericMetaData, object[][] annotations, MethodParametersEntry[][] methodParameters,
				byte[][][] runtimeVisibleTypeAnnotations, object[] constantPool)
			{
				this.genericMetaData = genericMetaData;
				this.annotations = annotations;
				this.methodParameters = methodParameters;
				this.runtimeVisibleTypeAnnotations = runtimeVisibleTypeAnnotations;
				this.constantPool = constantPool;
			}

			internal static Metadata Create(ClassFile classFile)
			{
				if (classFile.MajorVersion < 49)
				{
					return null;
				}
				string[][] genericMetaData = null;
				object[][] annotations = null;
				MethodParametersEntry[][] methodParameters = null;
				byte[][][] runtimeVisibleTypeAnnotations = null;
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
					if (classFile.Methods[i].MethodParameters != null)
					{
						if (methodParameters == null)
						{
							methodParameters = new MethodParametersEntry[classFile.Methods.Length][];
						}
						methodParameters[i] = classFile.Methods[i].MethodParameters;
					}
					if (classFile.Methods[i].RuntimeVisibleTypeAnnotations != null)
					{
						if (runtimeVisibleTypeAnnotations == null)
						{
							runtimeVisibleTypeAnnotations = new byte[3][][];
						}
						if (runtimeVisibleTypeAnnotations[1] == null)
						{
							runtimeVisibleTypeAnnotations[1] = new byte[classFile.Methods.Length][];
						}
						runtimeVisibleTypeAnnotations[1][i] = classFile.Methods[i].RuntimeVisibleTypeAnnotations;
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
					if (classFile.Fields[i].RuntimeVisibleTypeAnnotations != null)
					{
						if (runtimeVisibleTypeAnnotations == null)
						{
							runtimeVisibleTypeAnnotations = new byte[3][][];
						}
						if (runtimeVisibleTypeAnnotations[2] == null)
						{
							runtimeVisibleTypeAnnotations[2] = new byte[classFile.Fields.Length][];
						}
						runtimeVisibleTypeAnnotations[2][i] = classFile.Fields[i].RuntimeVisibleTypeAnnotations;
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
				if (classFile.RuntimeVisibleTypeAnnotations != null)
				{
					if (runtimeVisibleTypeAnnotations == null)
					{
						runtimeVisibleTypeAnnotations = new byte[3][][];
					}
					runtimeVisibleTypeAnnotations[0] = new byte[1][] { classFile.RuntimeVisibleTypeAnnotations };
				}
				if (genericMetaData != null || annotations != null || methodParameters != null || runtimeVisibleTypeAnnotations != null)
				{
					object[] constantPool = runtimeVisibleTypeAnnotations == null ? null : classFile.GetConstantPool();
					return new Metadata(genericMetaData, annotations, methodParameters, runtimeVisibleTypeAnnotations, constantPool);
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

			internal static MethodParametersEntry[] GetMethodParameters(Metadata m, int index)
			{
				if (m != null && m.methodParameters != null)
				{
					return m.methodParameters[index];
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

			internal static object[] GetConstantPool(Metadata m)
			{
				return m.constantPool;
			}

			internal static byte[] GetRawTypeAnnotations(Metadata m)
			{
				if (m != null && m.runtimeVisibleTypeAnnotations != null && m.runtimeVisibleTypeAnnotations[0] != null)
				{
					return m.runtimeVisibleTypeAnnotations[0][0];
				}
				return null;
			}

			internal static byte[] GetMethodRawTypeAnnotations(Metadata m, int index)
			{
				if (m != null && m.runtimeVisibleTypeAnnotations != null && m.runtimeVisibleTypeAnnotations[1] != null)
				{
					return m.runtimeVisibleTypeAnnotations[1][index];
				}
				return null;
			}

			internal static byte[] GetFieldRawTypeAnnotations(Metadata m, int index)
			{
				if (m != null && m.runtimeVisibleTypeAnnotations != null && m.runtimeVisibleTypeAnnotations[2] != null)
				{
					return m.runtimeVisibleTypeAnnotations[2][index];
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
			private readonly TypeWrapper host;

			internal FinishedTypeImpl(Type type, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers, Metadata metadata, MethodInfo clinitMethod, MethodInfo finalizeMethod, TypeWrapper host)
			{
				this.type = type;
				this.innerclasses = innerclasses;
				this.declaringTypeWrapper = declaringTypeWrapper;
				this.reflectiveModifiers = reflectiveModifiers;
				this.clinitMethod = clinitMethod;
				this.finalizeMethod = finalizeMethod;
				this.metadata = metadata;
				this.host = host;
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

			internal override MethodParametersEntry[] GetMethodParameters(int index)
			{
				return Metadata.GetMethodParameters(metadata, index);
			}

			internal override object[] GetFieldAnnotations(int index)
			{
				return Metadata.GetFieldAnnotations(metadata, index);
			}

			internal override MethodInfo GetFinalizeMethod()
			{
				return finalizeMethod;
			}

			internal override object[] GetConstantPool()
			{
				return Metadata.GetConstantPool(metadata);
			}

			internal override byte[] GetRawTypeAnnotations()
			{
				return Metadata.GetRawTypeAnnotations(metadata);
			}

			internal override byte[] GetMethodRawTypeAnnotations(int index)
			{
				return Metadata.GetMethodRawTypeAnnotations(metadata, index);
			}

			internal override byte[] GetFieldRawTypeAnnotations(int index)
			{
				return Metadata.GetFieldRawTypeAnnotations(metadata, index);
			}

			internal override TypeWrapper Host
			{
				get { return host; }
			}
		}

		internal sealed class FinishContext
		{
			private readonly TypeWrapper host;
			private readonly ClassFile classFile;
			private readonly DynamicOrAotTypeWrapper wrapper;
			private readonly TypeBuilder typeBuilder;
			private List<TypeBuilder> nestedTypeBuilders;
			private MethodInfo callerIDMethod;
			private List<Item> items;
			private Dictionary<FieldWrapper, MethodBuilder> arfuMap;
			private Dictionary<MethodKey, MethodInfo> invokespecialstubcache;
			private Dictionary<string, MethodInfo> dynamicClassLiteral;
#if STATIC_COMPILER
			private TypeBuilder interfaceHelperMethodsTypeBuilder;
#else
			private List<object> liveObjects;
#endif

			private struct Item
			{
				internal int key;
				internal object value;
			}

			internal FinishContext(TypeWrapper host, ClassFile classFile, DynamicOrAotTypeWrapper wrapper, TypeBuilder typeBuilder)
			{
				this.host = host;
				this.classFile = classFile;
				this.wrapper = wrapper;
				this.typeBuilder = typeBuilder;
			}

			internal DynamicTypeWrapper TypeWrapper
			{
				get { return wrapper; }
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

			internal void EmitDynamicClassLiteral(CodeEmitter ilgen, TypeWrapper tw, bool dynamicCallerID)
			{
				Debug.Assert(tw.IsUnloadable);
				if (dynamicClassLiteral == null)
				{
					dynamicClassLiteral = new Dictionary<string, MethodInfo>();
				}
				string cacheKey = tw.Name;
				if (dynamicCallerID)
				{
					cacheKey += ";dynamic";
				}
				MethodInfo method;
				if (!dynamicClassLiteral.TryGetValue(cacheKey, out method))
				{
					FieldBuilder fb = typeBuilder.DefineField("__<>class", CoreClasses.java.lang.Class.Wrapper.TypeAsSignatureType, FieldAttributes.PrivateScope | FieldAttributes.Static);
					MethodBuilder mb = DefineHelperMethod("__<>class", CoreClasses.java.lang.Class.Wrapper.TypeAsSignatureType, Type.EmptyTypes);
					CodeEmitter ilgen2 = CodeEmitter.Create(mb);
					ilgen2.Emit(OpCodes.Ldsfld, fb);
					CodeEmitterLabel label = ilgen2.DefineLabel();
					ilgen2.EmitBrtrue(label);
					ilgen2.Emit(OpCodes.Ldstr, tw.Name);
					EmitCallerID(ilgen2, dynamicCallerID);
					ilgen2.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicClassLiteral);
					ilgen2.Emit(OpCodes.Stsfld, fb);
					ilgen2.MarkLabel(label);
					ilgen2.Emit(OpCodes.Ldsfld, fb);
					ilgen2.Emit(OpCodes.Ret);
					ilgen2.DoEmit();
					method = mb;
					dynamicClassLiteral.Add(cacheKey, method);
				}
				ilgen.Emit(OpCodes.Call, method);
			}

			internal void EmitHostCallerID(CodeEmitter ilgen)
			{
#if STATIC_COMPILER || FIRST_PASS
				throw new InvalidOperationException();
#else
				EmitLiveObjectLoad(ilgen, DynamicCallerIDProvider.CreateCallerID(host));
				CoreClasses.ikvm.@internal.CallerID.Wrapper.EmitCheckcast(ilgen);
#endif
			}

			internal void EmitCallerID(CodeEmitter ilgen, bool dynamic)
			{
#if !FIRST_PASS && !STATIC_COMPILER
				if (dynamic)
				{
					EmitLiveObjectLoad(ilgen, DynamicCallerIDProvider.Instance);
					ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCallerID);
					return;
				}
#endif
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
				MethodBuilder mb = DefineHelperMethod("__<GetCallerID>", tw.TypeAsSignatureType, Type.EmptyTypes);
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
									wrapper.EmitLevel4Warning(HardError.AbstractMethodError, mw.DeclaringType.Name + "." + mw.Name + mw.Signature);
								}
							}
						}
						parent = parent.BaseTypeWrapper;
					}
				}
#if STATIC_COMPILER
				TypeBuilder tbDefaultMethods = null;
#endif
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
#if STATIC_COMPILER
						if (methods[i].GetParameters().Length > MethodHandleUtil.MaxArity && methods[i].RequiresNonVirtualDispatcher && wrapper.GetClassLoader().EmitNoRefEmitHelpers)
						{
							wrapper.GetClassLoader().GetTypeWrapperFactory().DefineDelegate(methods[i].GetParameters().Length, methods[i].ReturnType == PrimitiveTypeWrapper.VOID);
						}
#endif
						if (m.IsAbstract)
						{
							bool stub = false;
							if (!classFile.IsAbstract)
							{
								// NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
								// the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
								stub = true;
								wrapper.EmitLevel4Warning(HardError.AbstractMethodError, classFile.Name + "." + m.Name + m.Signature);
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
								if (m.InterlockedCompareAndSetField != null && EmitInterlockedCompareAndSet(methods[i], m.InterlockedCompareAndSetField, ilGenerator))
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
									nativeCodeType = StaticCompiler.GetType(wrapper.GetClassLoader(), "Java_" + classFile.Name.Replace('.', '_').Replace("$", "_00024"));
								}
#endif
								MethodInfo nativeMethod = null;
								TypeWrapper[] args = methods[i].GetParameters();
								TypeWrapper[] nargs = args;
								if (nativeCodeType != null)
								{
									if (!m.IsStatic)
									{
										nargs = ArrayUtil.Concat(wrapper, args);
									}
									if (methods[i].HasCallerID)
									{
										nargs = ArrayUtil.Concat(nargs, CoreClasses.ikvm.@internal.CallerID.Wrapper);
									}
									MethodInfo[] nativeCodeTypeMethods = nativeCodeType.GetMethods(BindingFlags.Static | BindingFlags.Public);
									foreach (MethodInfo method in nativeCodeTypeMethods)
									{
										ParameterInfo[] param = method.GetParameters();
										int paramLength = param.Length;
										while (paramLength != 0 && (param[paramLength - 1].IsIn || param[paramLength - 1].ParameterType.IsByRef))
										{
											paramLength--;
										}
										TypeWrapper[] match = new TypeWrapper[paramLength];
										for (int j = 0; j < paramLength; j++)
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
#if STATIC_COMPILER
									for (int j = 0; j < nargs.Length; j++)
									{
										ilGenerator.EmitLdarg(j);
									}
									ParameterInfo[] param = nativeMethod.GetParameters();
									for (int j = nargs.Length; j < param.Length; j++)
									{
										Type paramType = param[j].ParameterType;
										TypeWrapper fieldTypeWrapper = ClassLoaderWrapper.GetWrapperFromType(paramType.IsByRef ? paramType.GetElementType() : paramType);
										FieldWrapper field = wrapper.GetFieldWrapper(param[j].Name, fieldTypeWrapper.SigName);
										if (field == null)
										{
											Console.Error.WriteLine("Error: Native method field binding not found: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
											StaticCompiler.errorCount++;
											continue;
										}
										if (m.IsStatic && !field.IsStatic)
										{
											Console.Error.WriteLine("Error: Native method field binding cannot access instance field from static method: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
											StaticCompiler.errorCount++;
											continue;
										}
										if (!field.IsAccessibleFrom(wrapper, wrapper, wrapper))
										{
											Console.Error.WriteLine("Error: Native method field binding not accessible: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
											StaticCompiler.errorCount++;
											continue;
										}
										if (paramType.IsByRef && field.IsFinal)
										{
											Console.Error.WriteLine("Error: Native method field binding cannot use ByRef for final field: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
											StaticCompiler.errorCount++;
											continue;
										}
										field.Link();
										if (paramType.IsByRef && field.GetField() == null)
										{
											Console.Error.WriteLine("Error: Native method field binding cannot use ByRef on field without backing field: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
											StaticCompiler.errorCount++;
											continue;
										}
										if (!field.IsStatic)
										{
											ilGenerator.EmitLdarg(0);
										}
										if (paramType.IsByRef)
										{
											ilGenerator.Emit(field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field.GetField());
										}
										else
										{
											field.EmitGet(ilGenerator);
										}
									}
									ilGenerator.Emit(OpCodes.Call, nativeMethod);
									TypeWrapper retTypeWrapper = methods[i].ReturnType;
									if (!retTypeWrapper.TypeAsTBD.Equals(nativeMethod.ReturnType) && !retTypeWrapper.IsGhost)
									{
										ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
									}
									ilGenerator.Emit(OpCodes.Ret);
#endif
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
							if (m.IsVirtual && classFile.IsInterface)
							{
								mb = (MethodBuilder)DefaultInterfaceMethodWrapper.GetImpl(methods[i]);
#if STATIC_COMPILER
								CreateDefaultMethodInterop(ref tbDefaultMethods, mb, methods[i]);
#endif
							}
							CodeEmitter ilGenerator = CodeEmitter.Create(mb);
							if (!m.IsStatic && !m.IsPublic && classFile.IsInterface)
							{
								// Java 8 non-virtual interface method that we compile as a static method,
								// we need to make sure the passed in this reference isn't null
								ilGenerator.EmitLdarg(0);
								if (wrapper.IsGhost)
								{
									ilGenerator.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
								}
								ilGenerator.EmitNullCheck();
							}
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
							Compiler.Compile(this, host, wrapper, methods[i], classFile, m, ilGenerator, ref nonleaf);
							ilGenerator.CheckLabels();
							ilGenerator.DoEmit();
							if (nonleaf && !m.IsForceInline)
							{
								mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
							}
#if STATIC_COMPILER
							ilGenerator.EmitLineNumberTable(mb);
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

				AddInheritedDefaultInterfaceMethods(methods);

				if (clinitIndex != -1 || (basehasclinit && !classFile.IsInterface) || classFile.HasInitializedFields)
				{
					MethodBuilder cb;
					if (clinitIndex != -1)
					{
						cb = (MethodBuilder)methods[clinitIndex].GetMethod();
					}
					else
					{
						cb = ReflectUtil.DefineTypeInitializer(typeBuilder, wrapper.classLoader);
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
				AddInterfaceFieldsInterop(fields);

				// If we're a Java 8 interface with static methods, we create an inner class
				// to expose these methods to C#.
				AddInterfaceMethodsInterop(methods);

				// See if there is any additional metadata
				wrapper.EmitMapXmlMetadata(typeBuilder, classFile, fields, methods);

				// if we inherit public members from non-public base classes or have public members with non-public types in their signature, we need access stubs
				if (wrapper.IsPublic)
				{
					AddAccessStubs();
				}

				AddConstantPoolAttributeIfNecessary(classFile, typeBuilder);
#endif // STATIC_COMPILER

				for (int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBuilder mb = (MethodBuilder)methods[i].GetMethod();
					if (mb == null)
					{
						continue;
					}
					if (m.Annotations != null)
					{
						ParameterBuilder returnParameter = null;
						foreach (object[] def in m.Annotations)
						{
							Annotation annotation = Annotation.Load(wrapper, def);
							if (annotation != null)
							{
								annotation.Apply(wrapper.GetClassLoader(), mb, def);
								annotation.ApplyReturnValue(wrapper.GetClassLoader(), mb, ref returnParameter, def);
							}
						}
					}
					string[] parameterNames;
					AddMethodParameterInfo(m, methods[i], mb, out parameterNames);
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
							Annotation annotation = Annotation.Load(wrapper, def);
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
						Annotation annotation = Annotation.Load(wrapper, def);
						if (annotation != null)
						{
							annotation.Apply(wrapper.GetClassLoader(), typeBuilder, def);
						}
					}
				}

#if STATIC_COMPILER
				AddImplementsAttribute();
#endif

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
#if !STATIC_COMPILER
					if (liveObjects != null)
					{
						typeof(IKVM.Runtime.LiveObjectHolder<>).MakeGenericType(type).GetField("values", BindingFlags.Static | BindingFlags.Public).SetValue(null, liveObjects.ToArray());
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
			private static void AddConstantPoolAttributeIfNecessary(ClassFile classFile, TypeBuilder typeBuilder)
			{
				object[] constantPool = null;
				bool[] inUse = null;
				MarkConstantPoolUsage(classFile, classFile.RuntimeVisibleTypeAnnotations, ref constantPool, ref inUse);
				foreach (ClassFile.Method method in classFile.Methods)
				{
					MarkConstantPoolUsage(classFile, method.RuntimeVisibleTypeAnnotations, ref constantPool, ref inUse);
				}
				foreach (ClassFile.Field field in classFile.Fields)
				{
					MarkConstantPoolUsage(classFile, field.RuntimeVisibleTypeAnnotations, ref constantPool, ref inUse);
				}
				if (constantPool != null)
				{
					// to save space, we clear out the items that aren't used by the RuntimeVisibleTypeAnnotations and
					// use an RLE for the empty slots
					AttributeHelper.SetConstantPoolAttribute(typeBuilder, ConstantPoolAttribute.Compress(constantPool, inUse));
				}
			}

			private static void MarkConstantPoolUsage(ClassFile classFile, byte[] runtimeVisibleTypeAnnotations, ref object[] constantPool, ref bool[] inUse)
			{
				if (runtimeVisibleTypeAnnotations != null)
				{
					if (constantPool == null)
					{
						constantPool = classFile.GetConstantPool();
						inUse = new bool[constantPool.Length];
					}
					try
					{
						BigEndianBinaryReader br = new BigEndianBinaryReader(runtimeVisibleTypeAnnotations, 0, runtimeVisibleTypeAnnotations.Length);
						ushort num_annotations = br.ReadUInt16();
						for (int i = 0; i < num_annotations; i++)
						{
							MarkConstantPoolUsageForTypeAnnotation(br, inUse);
						}
						return;
					}
					catch (ClassFormatError)
					{
					}
					catch (IndexOutOfRangeException)
					{
					}
					// if we fail to parse the annotations (e.g. due to a malformed attribute), we simply keep all the constant pool entries
					for (int i = 0; i < inUse.Length; i++)
					{
						inUse[i] = true;
					}
				}
			}

			private static void MarkConstantPoolUsageForTypeAnnotation(BigEndianBinaryReader br, bool[] inUse)
			{
				switch (br.ReadByte())		// target_type
				{
					case 0x00:
					case 0x01:
						br.ReadByte();		// type_parameter_index
						break;
					case 0x10:
						br.ReadUInt16();	// supertype_index
						break;
					case 0x11:
					case 0x12:
						br.ReadByte();		// type_parameter_index
						br.ReadByte();		// bound_index
						break;
					case 0x13:
					case 0x14:
					case 0x15:
						// empty_target
						break;
					case 0x16:
						br.ReadByte();		// formal_parameter_index
						break;
					case 0x17:
						br.ReadUInt16();	// throws_type_index
						break;
					default:
						throw new ClassFormatError("");
				}
				byte path_length = br.ReadByte();
				for (int i = 0; i < path_length; i++)
				{
					br.ReadByte();			// type_path_kind
					br.ReadByte();			// type_argument_index
				}
				MarkConstantPoolUsageForAnnotation(br, inUse);
			}

			private static void MarkConstantPoolUsageForAnnotation(BigEndianBinaryReader br, bool[] inUse)
			{
				ushort type_index = br.ReadUInt16();
				inUse[type_index] = true;
				ushort num_components = br.ReadUInt16();
				for (int i = 0; i < num_components; i++)
				{
					ushort component_name_index = br.ReadUInt16();
					inUse[component_name_index] = true;
					MarkConstantPoolUsageForAnnotationComponentValue(br, inUse);
				}
			}

			private static void MarkConstantPoolUsageForAnnotationComponentValue(BigEndianBinaryReader br, bool[] inUse)
			{
				switch ((char)br.ReadByte())	// tag
				{
					case 'B':
					case 'C':
					case 'D':
					case 'F':
					case 'I':
					case 'J':
					case 'S':
					case 'Z':
					case 's':
					case 'c':
						inUse[br.ReadUInt16()] = true;
						break;
					case 'e':
						inUse[br.ReadUInt16()] = true;
						inUse[br.ReadUInt16()] = true;
						break;
					case '@':
						MarkConstantPoolUsageForAnnotation(br, inUse);
						break;
					case '[':
						ushort num_values = br.ReadUInt16();
						for (int i = 0; i < num_values; i++)
						{
							MarkConstantPoolUsageForAnnotationComponentValue(br, inUse);
						}
						break;
					default:
						throw new ClassFormatError("");
				}
			}

			private bool EmitInterlockedCompareAndSet(MethodWrapper method, string fieldName, CodeEmitter ilGenerator)
			{
				if (method.ReturnType != PrimitiveTypeWrapper.BOOLEAN)
				{
					return false;
				}
				TypeWrapper[] parameters = method.GetParameters();
				TypeWrapper target;
				int firstValueIndex;
				if (method.IsStatic)
				{
					if (parameters.Length != 3)
					{
						return false;
					}
					target = parameters[0];
					firstValueIndex = 1;
				}
				else
				{
					if (parameters.Length != 2)
					{
						return false;
					}
					target = method.DeclaringType;
					firstValueIndex = 0;
				}
				if (target.IsUnloadable || target.IsPrimitive || target.IsNonPrimitiveValueType || target.IsGhost)
				{
					return false;
				}
				TypeWrapper fieldType = parameters[firstValueIndex];
				if (fieldType != parameters[firstValueIndex + 1])
				{
					return false;
				}
				if (fieldType.IsUnloadable || fieldType.IsNonPrimitiveValueType || fieldType.IsGhost)
				{
					return false;
				}
				if (fieldType.IsPrimitive && fieldType != PrimitiveTypeWrapper.LONG && fieldType != PrimitiveTypeWrapper.INT)
				{
					return false;
				}
				FieldWrapper casField = null;
				foreach (FieldWrapper fw in target.GetFields())
				{
					if (fw.Name == fieldName)
					{
						if (casField != null)
						{
							return false;
						}
						casField = fw;
					}
				}
				if (casField == null)
				{
					return false;
				}
				if (casField.IsStatic)
				{
					return false;
				}
				if (casField.FieldTypeWrapper != fieldType)
				{
					return false;
				}
				if (casField.IsPropertyAccessor)
				{
					return false;
				}
				if (casField.DeclaringType.TypeAsBaseType == typeBuilder.DeclaringType)
				{
					// allow access to fields in outer class
				}
				else if (!casField.IsAccessibleFrom(casField.DeclaringType, wrapper, casField.DeclaringType))
				{
					return false;
				}
				casField.Link();
				FieldInfo fi = casField.GetField();
				if (fi == null)
				{
					return false;
				}
				ilGenerator.EmitLdarg(0);
				ilGenerator.Emit(OpCodes.Ldflda, fi);
				ilGenerator.EmitLdarg(2);
				ilGenerator.EmitLdarg(1);
				if (fieldType == PrimitiveTypeWrapper.LONG)
				{
					ilGenerator.Emit(OpCodes.Call, InterlockedMethods.CompareExchangeInt64);
				}
				else if (fieldType == PrimitiveTypeWrapper.INT)
				{
					ilGenerator.Emit(OpCodes.Call, InterlockedMethods.CompareExchangeInt32);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(casField.FieldTypeWrapper.TypeAsSignatureType));
				}
				ilGenerator.EmitLdarg(1);
				ilGenerator.Emit(OpCodes.Ceq);
				ilGenerator.Emit(OpCodes.Ret);
				return true;
			}
#endif

			private void AddMethodParameterInfo(ClassFile.Method m, MethodWrapper mw, MethodBuilder mb, out string[] parameterNames)
			{
				parameterNames = null;
				ParameterBuilder[] parameterBuilders = null;
				if (wrapper.GetClassLoader().EmitDebugInfo
#if STATIC_COMPILER
					|| (classFile.IsPublic && (m.IsPublic || m.IsProtected))
					|| (m.MethodParameters != null && !wrapper.GetClassLoader().NoParameterReflection)
#endif
					)
				{
					parameterNames = new string[mw.GetParameters().Length];
					GetParameterNamesFromMP(m, parameterNames);
#if STATIC_COMPILER
					if (m.MethodParameters == null)
#endif
					{
						GetParameterNamesFromLVT(m, parameterNames);
						GetParameterNamesFromSig(m.Signature, parameterNames);
					}
#if STATIC_COMPILER
					wrapper.GetParameterNamesFromXml(m.Name, m.Signature, parameterNames);
#endif
					parameterBuilders = GetParameterBuilders(mb, parameterNames.Length, parameterNames);
				}
#if STATIC_COMPILER
				if ((m.Modifiers & Modifiers.VarArgs) != 0 && !mw.HasCallerID)
				{
					if (parameterBuilders == null)
					{
						parameterBuilders = GetParameterBuilders(mb, mw.GetParameters().Length, null);
					}
					if (parameterBuilders.Length > 0)
					{
						AttributeHelper.SetParamArrayAttribute(parameterBuilders[parameterBuilders.Length - 1]);
					}
				}
				wrapper.AddXmlMapParameterAttributes(mb, classFile.Name, m.Name, m.Signature, ref parameterBuilders);
#endif
				if (m.ParameterAnnotations != null)
				{
					if (parameterBuilders == null)
					{
						parameterBuilders = GetParameterBuilders(mb, mw.GetParameters().Length, null);
					}
					object[][] defs = m.ParameterAnnotations;
					for (int j = 0; j < defs.Length; j++)
					{
						foreach (object[] def in defs[j])
						{
							Annotation annotation = Annotation.Load(wrapper, def);
							if (annotation != null)
							{
								annotation.Apply(wrapper.GetClassLoader(), parameterBuilders[j], def);
							}
						}
					}
				}
			}

#if STATIC_COMPILER
			private void AddImplementsAttribute()
			{
				TypeWrapper[] interfaces = wrapper.Interfaces;
				if (wrapper.BaseTypeWrapper == CoreClasses.java.lang.Object.Wrapper)
				{
					// We special case classes extending java.lang.Object to optimize the metadata encoding
					// for anonymous classes that implement an interface.
					Type[] actualInterfaces = typeBuilder.GetInterfaces();
					if (actualInterfaces.Length == 0)
					{
						return;
					}
					else if (actualInterfaces.Length == 1
						&& interfaces.Length == 1
						&& !interfaces[0].IsRemapped
						&& interfaces[0].TypeAsBaseType == actualInterfaces[0])
					{
						// We extend java.lang.Object and implement only a single (non-remapped) interface,
						// in this case we can omit the ImplementAttribute since the runtime will be able
						// to reliable reproduce the "list" of implemented interfaces.
						return;
					}
				}
				else if (interfaces.Length == 0)
				{
					return;
				}
				AttributeHelper.SetImplementsAttribute(typeBuilder, interfaces);
			}

			private TypeBuilder DefineNestedInteropType(string name)
			{
				CompilerClassLoader ccl = wrapper.classLoader;
				while (!ccl.ReserveName(classFile.Name + "$" + name))
				{
					name += "_";
				}
				TypeBuilder tb = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.Abstract);
				RegisterNestedTypeBuilder(tb);
				AttributeHelper.HideFromJava(tb);
				return tb;
			}
	
			private void AddInterfaceFieldsInterop(FieldWrapper[] fields)
			{
				if (classFile.IsInterface && classFile.IsPublic && !wrapper.IsGhost && classFile.Fields.Length > 0 && wrapper.classLoader.WorkaroundInterfaceFields)
				{
					TypeBuilder tbFields = DefineNestedInteropType(NestedTypeName.Fields);
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
							FieldBuilder fb = tbFields.DefineField(f.Name, fields[i].FieldTypeWrapper.TypeAsPublicSignatureType, attribs);
							if (ilgenClinit == null)
							{
								ilgenClinit = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tbFields, wrapper.classLoader));
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
			}

			private void AddInterfaceMethodsInterop(MethodWrapper[] methods)
			{
				if (classFile.IsInterface && classFile.IsPublic && classFile.MajorVersion >= 52 && !wrapper.IsGhost && methods.Length > 0 && wrapper.classLoader.WorkaroundInterfaceStaticMethods)
				{
					TypeBuilder tbMethods = null;
					foreach (MethodWrapper mw in methods)
					{
						if (mw.IsStatic && mw.IsPublic && mw.Name != StringConstants.CLINIT && ParametersAreAccessible(mw))
						{
							if (tbMethods == null)
							{
								tbMethods = DefineNestedInteropType(NestedTypeName.Methods);
							}
							MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(), tbMethods, mw.Name, MethodAttributes.Public | MethodAttributes.Static, null, true);
							CodeEmitter ilgen = CodeEmitter.Create(mb);
							TypeWrapper[] parameters = mw.GetParameters();
							for (int i = 0; i < parameters.Length; i++)
							{
								ilgen.EmitLdarg(i);
								if (!parameters[i].IsUnloadable && !parameters[i].IsPublic)
								{
									parameters[i].EmitCheckcast(ilgen);
								}
							}
							mw.EmitCall(ilgen);
							ilgen.Emit(OpCodes.Ret);
							ilgen.DoEmit();
						}
					}
				}
			}

			private void CreateDefaultMethodInterop(ref TypeBuilder tbDefaultMethods, MethodBuilder defaultMethod, MethodWrapper mw)
			{
				if (!ParametersAreAccessible(mw))
				{
					return;
				}
				if (tbDefaultMethods == null)
				{
					tbDefaultMethods = DefineNestedInteropType(NestedTypeName.DefaultMethods);
				}
				MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(), tbDefaultMethods, mw.Name, MethodAttributes.Public | MethodAttributes.Static, wrapper.TypeAsSignatureType, true);
				CodeEmitter ilgen = CodeEmitter.Create(mb);
				if (wrapper.IsGhost)
				{
					ilgen.EmitLdarga(0);
					ilgen.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
					ilgen.EmitNullCheck();
					ilgen.EmitLdarga(0);
				}
				else
				{
					ilgen.EmitLdarg(0);
					ilgen.Emit(OpCodes.Dup);
					ilgen.EmitNullCheck();
				}
				TypeWrapper[] parameters = mw.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					ilgen.EmitLdarg(i + 1);
					if (!parameters[i].IsUnloadable && !parameters[i].IsPublic)
					{
						parameters[i].EmitCheckcast(ilgen);
					}
				}
				ilgen.Emit(OpCodes.Call, defaultMethod);
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
			}
#endif

			private void AddInheritedDefaultInterfaceMethods(MethodWrapper[] methods)
			{
				// look at the miranda methods to see if we inherit any default interface methods
				for (int i = classFile.Methods.Length; i < methods.Length; i++)
				{
					if (methods[i].IsMirandaMethod)
					{
						MirandaMethodWrapper mmw = (MirandaMethodWrapper)methods[i];
						if (mmw.Error == null && !mmw.BaseMethod.IsAbstract)
						{
							// we inherited a default interface method, so we need to forward the miranda method to the default method
							MethodBuilder mb = (MethodBuilder)mmw.GetMethod();
							if (classFile.IsInterface)
							{
								// if we're an interface with a default miranda method, we need to create a new default method that forwards to the original
								mb = methods[i].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
									typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, typeBuilder, false);
							}
							EmitCallDefaultInterfaceMethod(mb, mmw.BaseMethod);
						}
					}
				}
			}

			internal static void EmitCallDefaultInterfaceMethod(MethodBuilder mb, MethodWrapper defaultMethod)
			{
				CodeEmitter ilgen = CodeEmitter.Create(mb);
				if (defaultMethod.DeclaringType.IsGhost)
				{
					CodeEmitterLocal local = ilgen.DeclareLocal(defaultMethod.DeclaringType.TypeAsSignatureType);
					ilgen.Emit(OpCodes.Ldloca, local);
					ilgen.EmitLdarg(0);
					ilgen.Emit(OpCodes.Stfld, defaultMethod.DeclaringType.GhostRefField);
					ilgen.Emit(OpCodes.Ldloca, local);
				}
				else
				{
					ilgen.EmitLdarg(0);
				}
				for (int j = 0, count = defaultMethod.GetParameters().Length; j < count; j++)
				{
					ilgen.EmitLdarg(j + 1);
				}
				ilgen.Emit(OpCodes.Call, DefaultInterfaceMethodWrapper.GetImpl(defaultMethod));
				ilgen.Emit(OpCodes.Ret);
				ilgen.DoEmit();
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
					Type propType = fw.FieldTypeWrapper.TypeAsPublicSignatureType;
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
					Type[] modopt2 = ArrayUtil.Concat(modopt, JVM.LoadType(typeof(IKVM.Attributes.AccessStub)));
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
					parameterTypes[i] = parameters[i].TypeAsPublicSignatureType;
					modopt[i] = wrapper.GetModOpt(parameters[i], true);
				}
				Type returnType = mw.ReturnType.TypeAsPublicSignatureType;
				Type[] modoptReturnType = ArrayUtil.Concat(wrapper.GetModOpt(mw.ReturnType, true), JVM.LoadType(typeof(IKVM.Attributes.AccessStub)));
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
					if (!method.IsStatic && method.IsPublic && !method.IsDynamicOnly)
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
						if (mce.ReturnType != ifmethod.ReturnType && !mce.ReturnType.IsUnloadable && !ifmethod.ReturnType.IsUnloadable)
						{
							error = true;
						}
						TypeWrapper[] mceparams = mce.GetParameters();
						TypeWrapper[] ifparams = ifmethod.GetParameters();
						for (int i = 0; i < mceparams.Length; i++)
						{
							if (mceparams[i] != ifparams[i] && !mceparams[i].IsUnloadable && !ifparams[i].IsUnloadable)
							{
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
					if (!mce.IsPublic && !mce.IsInternal)
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
					else if (mce.GetMethod() == null || mce.RealName != ifmethod.RealName || mce.IsInternal || !ReflectUtil.IsSameAssembly(mce.DeclaringType.TypeAsTBD, typeBuilder) || CheckRequireOverrideStub(mce, ifmethod))
					{
						// NOTE methods inherited from base classes in a different assembly do *not* automatically implement
						// interface methods, so we have to generate a stub here that doesn't do anything but call the base
						// implementation
						wrapper.GenerateOverrideStub(typeBuilder, ifmethod, null, mce);
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
						wrapper.EmitLevel4Warning(HardError.AbstractMethodError, wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
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
						argTypes[i + instance] = !args[i].IsUnloadable && (args[i].IsPrimitive || args[i].IsGhost || args[i].IsNonPrimitiveValueType) ? args[i].TypeAsSignatureType : typeof(object);
					}
					argTypes[argTypes.Length - 1] = CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
					Type retType = !mw.ReturnType.IsUnloadable && (mw.ReturnType.IsPrimitive || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType) ? mw.ReturnType.TypeAsSignatureType : typeof(object);
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
					context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
					ilGenerator.Emit(OpCodes.Call, mb);
					if (!mw.ReturnType.IsUnloadable && !mw.ReturnType.IsPrimitive && !mw.ReturnType.IsGhost && !mw.ReturnType.IsNonPrimitiveValueType)
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
						context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
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
						context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
					}
					ilGenerator.Emit(OpCodes.Call, enterLocalRefStruct);
					CodeEmitterLocal jnienv = ilGenerator.DeclareLocal(Types.IntPtr);
					ilGenerator.Emit(OpCodes.Stloc, jnienv);
					ilGenerator.BeginExceptionBlock();
					TypeWrapper retTypeWrapper = mw.ReturnType;
					if (retTypeWrapper.IsUnloadable || !retTypeWrapper.IsPrimitive)
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
						if (retTypeWrapper.IsUnloadable)
						{
							ilGenerator.Emit(OpCodes.Call, unwrapLocalRef);
						}
						else if (!retTypeWrapper.IsPrimitive)
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
						if (!iface.IsAccessibleFrom(wrapper))
						{
							continue;
						}
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
				Compiler.Compile(context, host, wrapper, methods[methodIndex], classFile, m, ilGenerator, ref nonLeaf);
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
						if (caller[i].IsUnloadable || callee[i].IsUnloadable)
						{
							return false;
						}
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
				TypeBuilder typeCallerID = typeBuilder.DefineNestedType(NestedTypeName.CallerID, TypeAttributes.Sealed | TypeAttributes.NestedPrivate, tw.TypeAsBaseType);
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
				TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.ThreadLocal + id, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, threadLocal.TypeAsBaseType);
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
					TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.AtomicReferenceFieldUpdater + arfuMap.Count, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, arfuTypeWrapper.TypeAsBaseType);
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
				TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.IndyCallSite + id, TypeAttributes.NestedPrivate | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			internal TypeBuilder DefineMethodHandleConstantType(int index)
			{
				TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.MethodHandleConstant + index, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit); ;
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			internal TypeBuilder DefineMethodTypeConstantType(int index)
			{
				TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.MethodTypeConstant + index, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			// this is used to define intrinsified anonymous classes (in the Unsafe.defineAnonymousClass() sense)
			internal TypeBuilder DefineAnonymousClass()
			{
				int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
				TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.IntrinsifiedAnonymousClass + id, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.SpecialName | TypeAttributes.BeforeFieldInit);
				RegisterNestedTypeBuilder(tb);
				return tb;
			}

			private MethodBuilder DefineHelperMethod(string name, Type returnType, Type[] parameterTypes)
			{
#if STATIC_COMPILER
				// FXBUG csc.exe doesn't like non-public methods in interfaces, so for public interfaces we move
				// the helper methods into a nested type.
				if (wrapper.IsPublic && wrapper.IsInterface && wrapper.classLoader.WorkaroundInterfacePrivateMethods)
				{
					if (interfaceHelperMethodsTypeBuilder == null)
					{
						interfaceHelperMethodsTypeBuilder = typeBuilder.DefineNestedType(NestedTypeName.InterfaceHelperMethods, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
						RegisterNestedTypeBuilder(interfaceHelperMethodsTypeBuilder);
					}
					return interfaceHelperMethodsTypeBuilder.DefineMethod(name, MethodAttributes.PrivateScope | MethodAttributes.Static, returnType, parameterTypes);
				}
#endif
				return typeBuilder.DefineMethod(name, MethodAttributes.PrivateScope | MethodAttributes.Static, returnType, parameterTypes);
			}

			internal MethodBuilder DefineMethodHandleDispatchStub(Type returnType, Type[] parameterTypes)
			{
				return DefineHelperMethod("__<>MHC", returnType, parameterTypes);
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
				return DefineHelperMethod("__<>", returnType, parameterTypes);
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

#if !STATIC_COMPILER
			internal void EmitLiveObjectLoad(CodeEmitter ilgen, object value)
			{
				if (liveObjects == null)
				{
					liveObjects = new List<object>();
				}
				FieldInfo fi = TypeBuilder.GetField(typeof(IKVM.Runtime.LiveObjectHolder<>).MakeGenericType(typeBuilder), typeof(IKVM.Runtime.LiveObjectHolder<>).GetField("values", BindingFlags.Static | BindingFlags.Public));
				ilgen.Emit(OpCodes.Ldsfld, fi);
				ilgen.EmitLdc_I4(liveObjects.Count);
				ilgen.Emit(OpCodes.Ldelem_Ref);
				liveObjects.Add(value);
			}
#endif
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

			MethodBuilder overrideStub = baseMethod.GetDefineMethodHelper().DefineMethod(this, typeBuilder, "__<overridestub>" + baseMethod.DeclaringType.Name + "::" + baseMethod.Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final);
			typeBuilder.DefineMethodOverride(overrideStub, (MethodInfo)baseMethod.GetMethod());

			TypeWrapper[] stubargs = baseMethod.GetParameters();
			TypeWrapper[] targetArgs = targetMethod.GetParameters();
			CodeEmitter ilgen = CodeEmitter.Create(overrideStub);
			ilgen.Emit(OpCodes.Ldarg_0);
			for (int i = 0; i < targetArgs.Length; i++)
			{
				ilgen.EmitLdarg(i + 1);
				ConvertStubArg(stubargs[i], targetArgs[i], ilgen);
			}
			if (target != null)
			{
				ilgen.Emit(OpCodes.Callvirt, target);
			}
			else
			{
				targetMethod.EmitCallvirt(ilgen);
			}
			ConvertStubArg(targetMethod.ReturnType, baseMethod.ReturnType, ilgen);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
		}

		private static void ConvertStubArg(TypeWrapper src, TypeWrapper dst, CodeEmitter ilgen)
		{
			if (src != dst)
			{
				if (dst.IsUnloadable)
				{
					if (!src.IsUnloadable && (src.IsGhost || src.IsNonPrimitiveValueType))
					{
						src.EmitConvSignatureTypeToStackType(ilgen);
					}
				}
				else if (dst.IsGhost || dst.IsNonPrimitiveValueType)
				{
					dst.EmitConvStackTypeToSignatureType(ilgen, null);
				}
				else
				{
					dst.EmitCheckcast(ilgen);
				}
			}
		}

		private static void GetParameterNamesFromMP(ClassFile.Method m, string[] parameterNames)
		{
			MethodParametersEntry[] methodParameters = m.MethodParameters;
			if (methodParameters != null)
			{
				for (int i = 0, count = Math.Min(parameterNames.Length, methodParameters.Length); i < count; i++)
				{
					if (parameterNames[i] == null)
					{
						parameterNames[i] = methodParameters[i].name;
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
				if (parameterNames != null && parameterNames[i] != null)
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
		protected abstract MethodBuilder DefineGhostMethod(TypeBuilder typeBuilder, string name, MethodAttributes attribs, MethodWrapper mw);
		protected abstract void FinishGhost(TypeBuilder typeBuilder, MethodWrapper[] methods);
		protected abstract void FinishGhostStep2();
		protected abstract TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs);
#endif // STATIC_COMPILER

		private bool IsPInvokeMethod(ClassFile.Method m)
		{
#if STATIC_COMPILER
			Dictionary<string, IKVM.Internal.MapXml.Class> mapxml = classLoader.GetMapXmlClasses();
			if (mapxml != null)
			{
				IKVM.Internal.MapXml.Class clazz;
				if (mapxml.TryGetValue(this.Name, out clazz) && clazz.Methods != null)
				{
					foreach (IKVM.Internal.MapXml.Method method in clazz.Methods)
					{
						if (method.Name == m.Name && method.Sig == m.Signature)
						{
							if (method.Attributes != null)
							{
								foreach (IKVM.Internal.MapXml.Attribute attr in method.Attributes)
								{
									if (StaticCompiler.GetType(classLoader, attr.Type) == JVM.Import(typeof(System.Runtime.InteropServices.DllImportAttribute)))
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
#elif CLASSGC
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

		internal override MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					return impl.GetMethodParameters(i);
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
			MethodBuilder mbld = mb as MethodBuilder;
			if (mbld != null)
			{
				return mbld.GetToken().Token;
			}
			return mb.MetadataToken;
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

		private object[] DecodeAnnotations(object[] definitions)
		{
			if (definitions == null)
			{
				return null;
			}
			java.lang.ClassLoader loader = GetClassLoader().GetJavaClassLoader();
			List<object> annotations = new List<object>();
			for (int i = 0; i < definitions.Length; i++)
			{
				object obj = JVM.NewAnnotation(loader, definitions[i]);
				if (obj != null)
				{
					annotations.Add(obj);
				}
			}
			return annotations.ToArray();
		}

		internal override object[] GetDeclaredAnnotations()
		{
			return DecodeAnnotations(impl.GetDeclaredAnnotations());
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i] == mw)
				{
					return DecodeAnnotations(impl.GetMethodAnnotations(i));
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
							objs[j] = DecodeAnnotations(annotations[j]);
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
					return DecodeAnnotations(impl.GetFieldAnnotations(i));
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
		private Type GetBaseTypeForDefineType()
		{
			return BaseTypeWrapper.TypeAsBaseType;
		}
#endif

#if STATIC_COMPILER
		protected virtual Type GetBaseTypeForDefineType()
		{
			return BaseTypeWrapper.TypeAsBaseType;
		}

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

		internal static bool RequiresDynamicReflectionCallerClass(string classFile, string method, string signature)
		{
			return (classFile == "java.lang.ClassLoader" && method == "getParent" && signature == "()Ljava.lang.ClassLoader;")
				|| (classFile == "java.lang.Thread" && method == "getContextClassLoader" && signature == "()Ljava.lang.ClassLoader;")
				|| (classFile == "java.io.ObjectStreamField" && method == "getType" && signature == "()Ljava.lang.Class;")
				|| (classFile == "javax.sql.rowset.serial.SerialJavaObject" && method == "getFields" && signature == "()[Ljava.lang.reflect.Field;")
				;
		}


		internal override object[] GetConstantPool()
		{
			return impl.GetConstantPool();
		}

		internal override byte[] GetRawTypeAnnotations()
		{
			return impl.GetRawTypeAnnotations();
		}

		internal override byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
		{
			return impl.GetMethodRawTypeAnnotations(Array.IndexOf(GetMethods(), mw));
		}

		internal override byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
		{
			return impl.GetFieldRawTypeAnnotations(Array.IndexOf(GetFields(), fw));
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal override TypeWrapper Host
		{
			get { return impl.Host; }
		}
#endif

		[Conditional("STATIC_COMPILER")]
		internal void EmitLevel4Warning(HardError error, string message)
		{
#if STATIC_COMPILER
			if (GetClassLoader().WarningLevelHigh)
			{
				switch (error)
				{
					case HardError.AbstractMethodError:
						GetClassLoader().IssueMessage(Message.EmittedAbstractMethodError, this.Name, message);
						break;
					case HardError.IncompatibleClassChangeError:
						GetClassLoader().IssueMessage(Message.EmittedIncompatibleClassChangeError, this.Name, message);
						break;
					default:
						throw new InvalidOperationException();
				}
			}
#endif
		}
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
			return DefineMethod(context.GetClassLoader().GetTypeWrapperFactory(), tb, name, attribs, null, false);
		}

		internal MethodBuilder DefineMethod(TypeWrapperFactory context, TypeBuilder tb, string name, MethodAttributes attribs)
		{
			return DefineMethod(context, tb, name, attribs, null, false);
		}

		internal MethodBuilder DefineMethod(TypeWrapperFactory context, TypeBuilder tb, string name, MethodAttributes attribs, Type firstParameter, bool mustBePublic)
		{
			// we add optional modifiers to make the signature unique
			int firstParam = firstParameter == null ? 0 : 1;
			TypeWrapper[] parameters = mw.GetParameters();
			Type[] parameterTypes = new Type[parameters.Length + (mw.HasCallerID ? 1 : 0) + firstParam];
			Type[][] modopt = new Type[parameterTypes.Length][];
			if (firstParameter != null)
			{
				parameterTypes[0] = firstParameter;
				modopt[0] = Type.EmptyTypes;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				parameterTypes[i + firstParam] = mustBePublic
					? parameters[i].TypeAsPublicSignatureType
					: parameters[i].TypeAsSignatureType;
				modopt[i + firstParam] = DynamicTypeWrapper.GetModOpt(context, parameters[i], mustBePublic);
			}
			if (mw.HasCallerID)
			{
				parameterTypes[parameterTypes.Length - 1] = CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
			}
			Type returnType = mustBePublic
				? mw.ReturnType.TypeAsPublicSignatureType
				: mw.ReturnType.TypeAsSignatureType;
			Type[] modoptReturnType = DynamicTypeWrapper.GetModOpt(context, mw.ReturnType, mustBePublic);
			return tb.DefineMethod(name, attribs, CallingConventions.Standard, returnType, null, modoptReturnType, parameterTypes, null, modopt);
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

#if !STATIC_COMPILER
	sealed class DynamicCallerIDProvider
	{
		// this object acts as a capability that is passed to trusted code to allow the DynamicCallerID()
		// method to be public without giving untrusted code the ability to forge a CallerID token
		internal static readonly DynamicCallerIDProvider Instance = new DynamicCallerIDProvider();

		private DynamicCallerIDProvider() { }

		internal ikvm.@internal.CallerID GetCallerID()
		{
			for (int i = 0; ; )
			{
				MethodBase method = new StackFrame(i++, false).GetMethod();
				if (method == null)
				{
#if !FIRST_PASS
					return ikvm.@internal.CallerID.create(null, null);
#endif
				}
				if (Java_sun_reflect_Reflection.IsHideFromStackWalk(method))
				{
					continue;
				}
				TypeWrapper caller = ClassLoaderWrapper.GetWrapperFromType(method.DeclaringType);
				return CreateCallerID(caller.Host ?? caller);
			}
		}

		internal static ikvm.@internal.CallerID CreateCallerID(TypeWrapper tw)
		{
#if FIRST_PASS
			return null;
#else
			return ikvm.@internal.CallerID.create(tw.ClassObject, tw.GetClassLoader().GetJavaClassLoader());
#endif
		}
	}
#endif
}
