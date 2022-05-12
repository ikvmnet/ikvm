/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using IKVM.Internal;

static class Java_java_lang_Class
{
	public static java.lang.Class forName0(string name, bool initialize, java.lang.ClassLoader loader, java.lang.Class caller)
	{
#if FIRST_PASS
		return null;
#else
		//Console.WriteLine("forName: " + name + ", loader = " + loader);
		TypeWrapper tw = null;
		if (name.IndexOf(',') > 0)
		{
			// we essentially require full trust before allowing arbitrary types to be loaded,
			// hence we do the "createClassLoader" permission check
			java.lang.SecurityManager sm = java.lang.System.getSecurityManager();
			if (sm != null)
				sm.checkPermission(new java.lang.RuntimePermission("createClassLoader"));
			Type type = Type.GetType(name);
			if (type != null)
			{
				tw = ClassLoaderWrapper.GetWrapperFromType(type);
			}
			if (tw == null)
			{
				java.lang.Throwable.suppressFillInStackTrace = true;
				throw new java.lang.ClassNotFoundException(name);
			}
		}
		else
		{
			try
			{
				ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(loader);
				tw = classLoaderWrapper.LoadClassByDottedName(name);
			}
			catch (ClassNotFoundException x)
			{
				java.lang.Throwable.suppressFillInStackTrace = true;
				throw new java.lang.ClassNotFoundException(x.Message);
			}
			catch (ClassLoadingException x)
			{
				throw x.InnerException;
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
		}
		java.security.ProtectionDomain pd;
		if (loader != null && caller != null && (pd = getProtectionDomain0(caller)) != null)
		{
			loader.checkPackageAccess(tw.ClassObject, pd);
		}
		if (initialize && !tw.IsArray)
		{
			try
			{
				tw.Finish();
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			tw.RunClassInit();
		}
		return tw.ClassObject;
#endif
	}

	public static byte[] getRawTypeAnnotations(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).GetRawTypeAnnotations();
	}

#if !FIRST_PASS
	private sealed class ConstantPoolImpl : sun.reflect.ConstantPool
	{
		private readonly object[] constantPool;

		internal ConstantPoolImpl(object[] constantPool)
		{
			this.constantPool = constantPool;
		}

		public override string getUTF8At(int index)
		{
			return (string)constantPool[index];
		}

		public override int getIntAt(int index)
		{
			return (int)constantPool[index];
		}

		public override long getLongAt(int index)
		{
			return (long)constantPool[index];
		}

		public override float getFloatAt(int index)
		{
			return (float)constantPool[index];
		}

		public override double getDoubleAt(int index)
		{
			return (double)constantPool[index];
		}
	}
#endif

    public static object getConstantPool(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		return new ConstantPoolImpl(TypeWrapper.FromClass(thisClass).GetConstantPool());
#endif
	}

	public static bool isInstance(java.lang.Class thisClass, object obj)
	{
		return TypeWrapper.FromClass(thisClass).IsInstance(obj);
	}

	public static bool isAssignableFrom(java.lang.Class thisClass, java.lang.Class otherClass)
	{
#if !FIRST_PASS
		if (otherClass == null)
		{
			throw new java.lang.NullPointerException();
		}
#endif
		return TypeWrapper.FromClass(otherClass).IsAssignableTo(TypeWrapper.FromClass(thisClass));
	}

	public static bool isInterface(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).IsInterface;
	}

	public static bool isArray(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).IsArray;
	}

	public static bool isPrimitive(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).IsPrimitive;
	}

	public static string getName0(java.lang.Class thisClass)
	{
		TypeWrapper tw = TypeWrapper.FromClass(thisClass);
		if (tw.IsPrimitive)
		{
			if (tw == PrimitiveTypeWrapper.BYTE)
			{
				return "byte";
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				return "char";
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
			{
				return "double";
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				return "float";
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				return "int";
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				return "long";
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				return "short";
			}
			else if (tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				return "boolean";
			}
			else if (tw == PrimitiveTypeWrapper.VOID)
			{
				return "void";
			}
		}
		if (tw.IsUnsafeAnonymous)
		{
#if !FIRST_PASS
			// for OpenJDK compatibility and debugging convenience we modify the class name to
			// include the identity hashcode of the class object
			return tw.Name + "/" + java.lang.System.identityHashCode(thisClass);
#endif
		}
		return tw.Name;
	}

	public static string getSigName(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).SigName;
	}

	public static java.lang.ClassLoader getClassLoader0(java.lang.Class thisClass)
	{
		return TypeWrapper.FromClass(thisClass).GetClassLoader().GetJavaClassLoader();
	}

	public static java.lang.Class getSuperclass(java.lang.Class thisClass)
	{
		TypeWrapper super = TypeWrapper.FromClass(thisClass).BaseTypeWrapper;
		return super != null ? super.ClassObject : null;
	}

	public static java.lang.Class[] getInterfaces0(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper[] ifaces = TypeWrapper.FromClass(thisClass).Interfaces;
		java.lang.Class[] interfaces = new java.lang.Class[ifaces.Length];
		for (int i = 0; i < ifaces.Length; i++)
		{
			interfaces[i] = ifaces[i].ClassObject;
		}
		return interfaces;
#endif
	}

	public static java.lang.Class getComponentType(java.lang.Class thisClass)
	{
		TypeWrapper tw = TypeWrapper.FromClass(thisClass);
		return tw.IsArray ? tw.ElementTypeWrapper.ClassObject : null;
	}

	public static int getModifiers(java.lang.Class thisClass)
	{
		// the 0x7FFF mask comes from JVM_ACC_WRITTEN_FLAGS in hotspot\src\share\vm\utilities\accessFlags.hpp
		// masking out ACC_SUPER comes from instanceKlass::compute_modifier_flags() in hotspot\src\share\vm\oops\instanceKlass.cpp
		const int mask = 0x7FFF & (int)~IKVM.Attributes.Modifiers.Super;
		return (int)TypeWrapper.FromClass(thisClass).ReflectiveModifiers & mask;
	}

	public static object[] getSigners(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		return thisClass.signers;
#endif
	}

	public static void setSigners(java.lang.Class thisClass, object[] signers)
	{
#if !FIRST_PASS
		thisClass.signers = signers;
#endif
	}

	public static object[] getEnclosingMethod0(java.lang.Class thisClass)
	{
		try
		{
			TypeWrapper tw = TypeWrapper.FromClass(thisClass);
			tw.Finish();
			string[] enc = tw.GetEnclosingMethod();
			if (enc == null)
			{
				return null;
			}
			return new object[] { tw.GetClassLoader().LoadClassByDottedName(enc[0]).ClassObject, enc[1], enc[2] == null ? null : enc[2].Replace('.', '/') };
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
	}

	public static java.lang.Class getDeclaringClass0(java.lang.Class thisClass)
	{
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
			wrapper.Finish();
			TypeWrapper decl = wrapper.DeclaringTypeWrapper;
			if (decl == null)
			{
				return null;
			}
			decl = decl.EnsureLoadable(wrapper.GetClassLoader());
			if (!decl.IsAccessibleFrom(wrapper))
			{
				throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", decl.Name, wrapper.Name));
			}
			decl.Finish();
			TypeWrapper[] declInner = decl.InnerClasses;
			for (int i = 0; i < declInner.Length; i++)
			{
				if (declInner[i].Name == wrapper.Name && declInner[i].EnsureLoadable(decl.GetClassLoader()) == wrapper)
				{
					return decl.ClassObject;
				}
			}
			throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", decl.Name, wrapper.Name));
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
	}

	public static java.security.ProtectionDomain getProtectionDomain0(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
		if (wrapper.IsArray)
		{
			return null;
		}
		java.security.ProtectionDomain pd = wrapper.ClassObject.pd;
		if (pd == null)
		{
			// The protection domain for statically compiled code is created lazily (not at java.lang.Class creation time),
			// to work around boot strap issues.
			AssemblyClassLoader acl = wrapper.GetClassLoader() as AssemblyClassLoader;
			if (acl != null)
			{
				pd = acl.GetProtectionDomain();
			}
			else if (wrapper is AnonymousTypeWrapper)
			{
				// dynamically compiled intrinsified lamdba anonymous types end up here and should get their
				// protection domain from the host class
				pd = ClassLoaderWrapper.GetWrapperFromType(wrapper.TypeAsTBD.DeclaringType).ClassObject.pd;
			}
		}
		return pd;
#endif
	}

	public static java.lang.Class getPrimitiveClass(string name)
	{
		// note that this method isn't used anymore (because it is an intrinsic (during core class library compilation))
		// it still remains for compat because it might be invoked through reflection by evil code
		switch (name)
		{
			case "byte":
				return PrimitiveTypeWrapper.BYTE.ClassObject;
			case "char":
				return PrimitiveTypeWrapper.CHAR.ClassObject;
			case "double":
				return PrimitiveTypeWrapper.DOUBLE.ClassObject;
			case "float":
				return PrimitiveTypeWrapper.FLOAT.ClassObject;
			case "int":
				return PrimitiveTypeWrapper.INT.ClassObject;
			case "long":
				return PrimitiveTypeWrapper.LONG.ClassObject;
			case "short":
				return PrimitiveTypeWrapper.SHORT.ClassObject;
			case "boolean":
				return PrimitiveTypeWrapper.BOOLEAN.ClassObject;
			case "void":
				return PrimitiveTypeWrapper.VOID.ClassObject;
			default:
				throw new ArgumentException(name);
		}
	}

	public static string getGenericSignature0(java.lang.Class thisClass)
	{
		TypeWrapper tw = TypeWrapper.FromClass(thisClass);
		tw.Finish();
		return tw.GetGenericSignature();
	}

	internal static object AnnotationsToMap(ClassLoaderWrapper loader, object[] objAnn)
	{
#if FIRST_PASS
		return null;
#else
		java.util.LinkedHashMap map = new java.util.LinkedHashMap();
		if (objAnn != null)
		{
			foreach (object obj in objAnn)
			{
				java.lang.annotation.Annotation a = obj as java.lang.annotation.Annotation;
				if (a != null)
				{
					map.put(a.annotationType(), FreezeOrWrapAttribute(a));
				}
				else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
				{
					a = (java.lang.annotation.Annotation)JVM.NewAnnotation(loader.GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition);
					if (a != null)
					{
						map.put(a.annotationType(), a);
					}
				}
			}
		}
		return map;
#endif
	}

#if !FIRST_PASS
	internal static java.lang.annotation.Annotation FreezeOrWrapAttribute(java.lang.annotation.Annotation ann)
	{
		ikvm.@internal.AnnotationAttributeBase attr = ann as ikvm.@internal.AnnotationAttributeBase;
		if (attr != null)
		{
#if DONT_WRAP_ANNOTATION_ATTRIBUTES
			attr.freeze();
#else
			// freeze to make sure the defaults are set
			attr.freeze();
			ann = sun.reflect.annotation.AnnotationParser.annotationForMap(attr.annotationType(), attr.getValues());
#endif
		}
		return ann;
	}
#endif

	public static object getDeclaredAnnotationsImpl(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
		try
		{
			wrapper.Finish();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		return AnnotationsToMap(wrapper.GetClassLoader(), wrapper.GetDeclaredAnnotations());
#endif
	}

	public static object getDeclaredFields0(java.lang.Class thisClass, bool publicOnly)
	{
#if FIRST_PASS
		return null;
#else
		Profiler.Enter("Class.getDeclaredFields0");
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
			// we need to finish the type otherwise all fields will not be in the field map yet
			wrapper.Finish();
			FieldWrapper[] fields = wrapper.GetFields();
			List<java.lang.reflect.Field> list = new List<java.lang.reflect.Field>();
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i].IsHideFromReflection)
				{
					// skip
				}
				else if (publicOnly && !fields[i].IsPublic)
				{
					// caller is only asking for public field, so we don't return this non-public field
				}
				else
				{
					list.Add((java.lang.reflect.Field)fields[i].ToField(false, i));
				}
			}
			return list.ToArray();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		finally
		{
			Profiler.Leave("Class.getDeclaredFields0");
		}
#endif
	}

	public static object getDeclaredMethods0(java.lang.Class thisClass, bool publicOnly)
	{
#if FIRST_PASS
		return null;
#else
		Profiler.Enter("Class.getDeclaredMethods0");
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
			wrapper.Finish();
			if (wrapper.HasVerifyError)
			{
				// TODO we should get the message from somewhere
				throw new VerifyError();
			}
			if (wrapper.HasClassFormatError)
			{
				// TODO we should get the message from somewhere
				throw new ClassFormatError(wrapper.Name);
			}
			MethodWrapper[] methods = wrapper.GetMethods();
			List<java.lang.reflect.Method> list = new List<java.lang.reflect.Method>(methods.Length);
			for (int i = 0; i < methods.Length; i++)
			{
				// we don't want to expose "hideFromReflection" methods (one reason is that it would
				// mess up the serialVersionUID computation)
				if (!methods[i].IsHideFromReflection
					&& !methods[i].IsConstructor
					&& !methods[i].IsClassInitializer
					&& (!publicOnly || methods[i].IsPublic))
				{
					list.Add((java.lang.reflect.Method)methods[i].ToMethodOrConstructor(false));
				}
			}
			return list.ToArray();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		finally
		{
			Profiler.Leave("Class.getDeclaredMethods0");
		}
#endif
	}

	public static object getDeclaredConstructors0(java.lang.Class thisClass, bool publicOnly)
	{
#if FIRST_PASS
		return null;
#else
		Profiler.Enter("Class.getDeclaredConstructors0");
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
			wrapper.Finish();
			if (wrapper.HasVerifyError)
			{
				// TODO we should get the message from somewhere
				throw new VerifyError();
			}
			if (wrapper.HasClassFormatError)
			{
				// TODO we should get the message from somewhere
				throw new ClassFormatError(wrapper.Name);
			}
			MethodWrapper[] methods = wrapper.GetMethods();
			List<java.lang.reflect.Constructor> list = new List<java.lang.reflect.Constructor>();
			for (int i = 0; i < methods.Length; i++)
			{
				// we don't want to expose "hideFromReflection" methods (one reason is that it would
				// mess up the serialVersionUID computation)
				if (!methods[i].IsHideFromReflection
					&& methods[i].IsConstructor
					&& (!publicOnly || methods[i].IsPublic))
				{
					list.Add((java.lang.reflect.Constructor)methods[i].ToMethodOrConstructor(false));
				}
			}
			return list.ToArray();
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		finally
		{
			Profiler.Leave("Class.getDeclaredConstructors0");
		}
#endif
	}

	public static java.lang.Class[] getDeclaredClasses0(java.lang.Class thisClass)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
			// NOTE to get at the InnerClasses we need to finish the type
			wrapper.Finish();
			TypeWrapper[] wrappers = wrapper.InnerClasses;
			java.lang.Class[] innerclasses = new java.lang.Class[wrappers.Length];
			for (int i = 0; i < innerclasses.Length; i++)
			{
				TypeWrapper tw = wrappers[i].EnsureLoadable(wrapper.GetClassLoader());
				if (!tw.IsAccessibleFrom(wrapper))
				{
					throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", tw.Name, wrapper.Name));
				}
				tw.Finish();
				innerclasses[i] = tw.ClassObject;
			}
			return innerclasses;
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
#endif
	}

	public static bool desiredAssertionStatus0(java.lang.Class clazz)
	{
		return IKVM.Runtime.Assertions.IsEnabled(TypeWrapper.FromClass(clazz));
	}
}

static class Java_java_lang_ClassLoader
{
	public static java.net.URL getBootstrapResource(string name)
	{
		foreach (java.net.URL url in ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name))
		{
			return url;
		}
		return null;
	}

	public static java.util.Enumeration getBootstrapResources(string name)
	{
#if FIRST_PASS
		return null;
#else
		return new ikvm.runtime.EnumerationWrapper(ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name));
#endif
	}

	public static java.lang.Class defineClass0(java.lang.ClassLoader thisClassLoader, string name, byte[] b, int off, int len, java.security.ProtectionDomain pd)
	{
		return defineClass1(thisClassLoader, name, b, off, len, pd, null);
	}

	public static java.lang.Class defineClass1(java.lang.ClassLoader thisClassLoader, string name, byte[] b, int off, int len, java.security.ProtectionDomain pd, string source)
	{
		// it appears the source argument is only used for trace messages in HotSpot. We'll just ignore it for now.
		Profiler.Enter("ClassLoader.defineClass");
		try
		{
			try
			{
				ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
				ClassFile classFile = new ClassFile(b, off, len, name, classLoaderWrapper.ClassFileParseOptions, null);
				if (name != null && classFile.Name != name)
				{
#if !FIRST_PASS
					throw new java.lang.NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
#endif
				}
				TypeWrapper type = classLoaderWrapper.DefineClass(classFile, pd);
				return type.ClassObject;
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
		}
		finally
		{
			Profiler.Leave("ClassLoader.defineClass");
		}
	}

	public static java.lang.Class defineClass2(java.lang.ClassLoader thisClassLoader, string name, java.nio.ByteBuffer bb, int off, int len, java.security.ProtectionDomain pd, string source)
	{
#if FIRST_PASS
		return null;
#else
		byte[] buf = new byte[bb.remaining()];
		bb.get(buf);
		return defineClass1(thisClassLoader, name, buf, 0, buf.Length, pd, source);
#endif
	}

	public static void resolveClass0(java.lang.ClassLoader thisClassLoader, java.lang.Class clazz)
	{
		// no-op
	}

	public static java.lang.Class findBootstrapClass(java.lang.ClassLoader thisClassLoader, string name)
	{
#if FIRST_PASS
		return null;
#else
		TypeWrapper tw;
		try
		{
			tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		return tw != null ? tw.ClassObject : null;
#endif
	}

	public static java.lang.Class findLoadedClass0(java.lang.ClassLoader thisClassLoader, string name)
	{
		if (name == null)
		{
			return null;
		}
		ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
		TypeWrapper tw = loader.FindLoadedClass(name);
		return tw != null ? tw.ClassObject : null;
	}

	public static object retrieveDirectives()
	{
		return IKVM.Runtime.Assertions.RetrieveDirectives();
	}
}

static class Java_java_lang_ClassLoader_00024NativeLibrary
{
	public static void load(object thisNativeLibrary, string name, bool isBuiltin)
	{
#if !FIRST_PASS
		if (VirtualFileSystem.IsVirtualFS(name))
		{
			// we fake success for native libraries loaded from VFS
			((java.lang.ClassLoader.NativeLibrary)thisNativeLibrary).loaded = true;
		}
		else
		{
			doLoad(thisNativeLibrary, name);
		}
#endif
	}

#if !FIRST_PASS
	// we don't want to inline this method, because that would needlessly cause IKVM.Runtime.JNI.dll to be loaded when loading a fake native library from VFS
	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	private static void doLoad(object thisNativeLibrary, string name)
	{
		java.lang.ClassLoader.NativeLibrary lib = (java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
		lib.handle = IKVM.Runtime.JniHelper.LoadLibrary(name, TypeWrapper.FromClass(java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
		lib.loaded = true;
	}
#endif

	public static long find(object thisNativeLibrary, string name)
	{
		// TODO
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static void unload(object thisNativeLibrary, string name, bool isBuiltin)
	{
#if !FIRST_PASS
		java.lang.ClassLoader.NativeLibrary lib = (java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
		long handle = Interlocked.Exchange(ref lib.handle, 0);
		if (handle != 0)
		{
			IKVM.Runtime.JniHelper.UnloadLibrary(handle, TypeWrapper.FromClass(java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
		}
#endif
	}

	public static string findBuiltinLib(string name)
	{
		return null;
	}
}

static class Java_java_lang_Compiler
{
	public static void initialize()
	{
	}

	public static void registerNatives()
	{
	}

	public static bool compileClass(object clazz)
	{
		return false;
	}

	public static bool compileClasses(string str)
	{
		return false;
	}

	public static object command(object any)
	{
		return null;
	}

	public static void enable()
	{
	}

	public static void disable()
	{
	}
}

static class Java_java_lang_Double
{
	public static long doubleToRawLongBits(double value)
	{
		IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
		return IKVM.Runtime.DoubleConverter.ToLong(value, ref converter);
	}

	public static double longBitsToDouble(long bits)
	{
		IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
		return IKVM.Runtime.DoubleConverter.ToDouble(bits, ref converter);
	}
}

static class Java_java_lang_Float
{
	public static int floatToRawIntBits(float value)
	{
		IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
		return IKVM.Runtime.FloatConverter.ToInt(value, ref converter);
	}

	public static float intBitsToFloat(int bits)
	{
		IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
		return IKVM.Runtime.FloatConverter.ToFloat(bits, ref converter);
	}
}

static class Java_java_lang_Package
{
	private static Dictionary<string, string> systemPackages;

	private static void LazyInitSystemPackages()
	{
		if (systemPackages == null)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			string path = VirtualFileSystem.GetAssemblyResourcesPath(JVM.CoreAssembly) + "resources.jar";
			foreach (KeyValuePair<string, string[]> pkgs in ClassLoaderWrapper.GetBootstrapClassLoader().GetPackageInfo())
			{
				foreach (string pkg in pkgs.Value)
				{
					dict[pkg.Replace('.', '/') + "/"] = path;
				}
			}
			Interlocked.CompareExchange(ref systemPackages, dict, null);
		}
	}

	public static string getSystemPackage0(string name)
	{
		LazyInitSystemPackages();
		string path;
		systemPackages.TryGetValue(name, out path);
		return path;
	}

	public static string[] getSystemPackages0()
	{
		LazyInitSystemPackages();
		string[] pkgs = new string[systemPackages.Count];
		systemPackages.Keys.CopyTo(pkgs, 0);
		return pkgs;
	}
}

static class Java_java_lang_ProcessEnvironment
{
	public static string environmentBlock()
	{
		StringBuilder sb = new StringBuilder();
		foreach (System.Collections.DictionaryEntry de in Environment.GetEnvironmentVariables())
		{
			sb.Append(de.Key).Append('=').Append(de.Value).Append('\u0000');
		}
		if (sb.Length == 0)
		{
			sb.Append('\u0000');
		}
		sb.Append('\u0000');
		return sb.ToString();
	}
}

static class Java_java_lang_Runtime
{
	public static int availableProcessors(object thisRuntime)
	{
		return Environment.ProcessorCount;
	}

	public static long freeMemory(object thisRuntime)
	{
		// TODO figure out if there is anything meaningful we can return here
		return 10 * 1024 * 1024;
	}

	public static long totalMemory(object thisRuntime)
	{
		// NOTE this really is a bogus number, but we have to return something
		return GC.GetTotalMemory(false) + freeMemory(thisRuntime);
	}

	public static long maxMemory(object thisRuntime)
	{
		// spec says: If there is no inherent limit then the value Long.MAX_VALUE will be returned.
		return Int64.MaxValue;
	}

	public static void gc(object thisRuntime)
	{
		GC.Collect();
	}

	public static void traceInstructions(object thisRuntime, bool on)
	{
	}

	public static void traceMethodCalls(object thisRuntime, bool on)
	{
	}

	public static void runFinalization0()
	{
		GC.WaitForPendingFinalizers();
	}
}

static class Java_java_lang_SecurityManager
{
	// this field is set by code in the JNI assembly itself,
	// to prevent having to load the JNI assembly when it isn't used.
	internal static volatile Assembly jniAssembly;

	public static java.lang.Class[] getClassContext(object thisSecurityManager)
	{
#if FIRST_PASS
		return null;
#else
		List<java.lang.Class> stack = new List<java.lang.Class>();
		StackTrace trace = new StackTrace();
		for (int i = 0; i < trace.FrameCount; i++)
		{
			StackFrame frame = trace.GetFrame(i);
			MethodBase method = frame.GetMethod();
			if (Java_sun_reflect_Reflection.IsHideFromStackWalk(method))
			{
				continue;
			}
			Type type = method.DeclaringType;
			if (type == typeof(java.lang.SecurityManager))
			{
				continue;
			}
			stack.Add(ClassLoaderWrapper.GetWrapperFromType(type).ClassObject);
		}
		return stack.ToArray();
#endif
	}

	public static object currentClassLoader0(object thisSecurityManager)
	{
		java.lang.Class currentClass = currentLoadedClass0(thisSecurityManager);
		if (currentClass != null)
		{
			return TypeWrapper.FromClass(currentClass).GetClassLoader().GetJavaClassLoader();
		}
		return null;
	}

	public static int classDepth(object thisSecurityManager, string name)
	{
		throw new NotImplementedException();
	}

	public static int classLoaderDepth0(object thisSecurityManager)
	{
		throw new NotImplementedException();
	}

	public static java.lang.Class currentLoadedClass0(object thisSecurityManager)
	{
		throw new NotImplementedException();
	}
}

static class Java_java_lang_StrictMath
{
	public static double sin(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.sin(d);
#endif
	}

	public static double cos(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.cos(d);
#endif
	}

	public static double tan(double d)
	{
		return fdlibm.tan(d);
	}

	public static double asin(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.asin(d);
#endif
	}

	public static double acos(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.acos(d);
#endif
	}

	public static double atan(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.atan(d);
#endif
	}

	public static double exp(double d)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.exp(d);
#endif
	}

	public static double log(double d)
	{
		// FPU behavior is correct
		return Math.Log(d);
	}

	public static double log10(double d)
	{
		// FPU behavior is correct
		return Math.Log10(d);
	}

	public static double sqrt(double d)
	{
		// FPU behavior is correct
		return Math.Sqrt(d);
	}

	public static double cbrt(double d)
	{
		return fdlibm.cbrt(d);
	}

	public static double IEEEremainder(double f1, double f2)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.IEEEremainder(f1, f2);
#endif
	}

	public static double atan2(double y, double x)
	{
#if FIRST_PASS
		return 0;
#else
		return ikvm.@internal.JMath.atan2(y, x);
#endif
	}

	public static double pow(double x, double y)
	{
		return fdlibm.__ieee754_pow(x, y);
	}

	public static double sinh(double d)
	{
		return Math.Sinh(d);
	}

	public static double cosh(double d)
	{
		return Math.Cosh(d);
	}

	public static double tanh(double d)
	{
		return Math.Tanh(d);
	}

	public static double hypot(double a, double b)
	{
		return fdlibm.__ieee754_hypot(a, b);
	}

	public static double expm1(double d)
	{
		return fdlibm.expm1(d);
	}

	public static double log1p(double d)
	{
		return fdlibm.log1p(d);
	}
}

static class Java_java_lang_System
{
	public static void registerNatives()
	{
	}

	public static void setIn0(object @in)
	{
#if !FIRST_PASS
		java.lang.StdIO.@in = (java.io.InputStream)@in;
#endif
	}

	public static void setOut0(object @out)
	{
#if !FIRST_PASS
		java.lang.StdIO.@out = (java.io.PrintStream)@out;
#endif
	}

	public static void setErr0(object err)
	{
#if !FIRST_PASS
		java.lang.StdIO.err = (java.io.PrintStream)err;
#endif
	}

	public static object initProperties(object props)
	{
#if FIRST_PASS
		return null;
#else
		java.lang.VMSystemProperties.initProperties((java.util.Properties)props);
		return props;
#endif
	}

	public static string mapLibraryName(string libname)
	{
#if FIRST_PASS
		return null;
#else
		if (libname == null)
		{
			throw new java.lang.NullPointerException();
		}
		if (ikvm.@internal.Util.WINDOWS)
		{
			return libname + ".dll";
		}
		else if (ikvm.@internal.Util.MACOSX)
		{
			return "lib" + libname + ".jnilib";
		}
		else
		{
			return "lib" + libname + ".so";
		}
#endif
	}

	public static void arraycopy(object src, int srcPos, object dest, int destPos, int length)
	{
		IKVM.Runtime.ByteCodeHelper.arraycopy(src, srcPos, dest, destPos, length);
	}
}

static class Java_java_lang_Thread
{
	private static readonly object mainThreadGroup;

#if !FIRST_PASS
	static Java_java_lang_Thread()
	{
		mainThreadGroup = new java.lang.ThreadGroup(java.lang.ThreadGroup.createRootGroup(), "main");
	}
#endif

	public static object getMainThreadGroup()
	{
		return mainThreadGroup;
	}

	// this is called from JniInterface.cs
	internal static void WaitUntilLastJniThread()
	{
#if !FIRST_PASS
		int count = java.lang.Thread.currentThread().isDaemon() ? 0 : 1;
		while (Interlocked.CompareExchange(ref java.lang.Thread.nonDaemonCount[0], 0, 0) > count)
		{
			Thread.Sleep(1);
		}
#endif
	}

	// this is called from JniInterface.cs
	internal static void AttachThreadFromJni(object threadGroup)
	{
#if !FIRST_PASS
		if (threadGroup == null)
		{
			threadGroup = mainThreadGroup;
		}
		if (java.lang.Thread.current == null)
		{
			new java.lang.Thread((java.lang.ThreadGroup)threadGroup);
		}
#endif
	}

	public static java.lang.StackTraceElement[] getStackTrace(StackTrace stack)
	{
#if FIRST_PASS
		return null;
#else
		List<java.lang.StackTraceElement> stackTrace = new List<java.lang.StackTraceElement>();
		ExceptionHelper.ExceptionInfoHelper.Append(stackTrace, stack, 0, true);
		return stackTrace.ToArray();
#endif
	}

	public static object getThreads()
	{
#if FIRST_PASS
		return null;
#else
		return java.security.AccessController.doPrivileged(ikvm.runtime.Delegates.toPrivilegedAction(delegate
		{
			java.lang.ThreadGroup root = (java.lang.ThreadGroup)mainThreadGroup;
			for (; ; )
			{
				java.lang.Thread[] threads = new java.lang.Thread[root.activeCount()];
				if (root.enumerate(threads) == threads.Length)
				{
					return threads;
				}
			}
		}));
#endif
	}
}

static class Java_java_lang_ProcessImpl
{
	public static string mapVfsExecutable(string path)
	{
		string unquoted = path;
		if (unquoted.Length > 2 && unquoted[0] == '"' && unquoted[unquoted.Length - 1] == '"')
		{
			unquoted = unquoted.Substring(1, unquoted.Length - 2);
		}
		if (VirtualFileSystem.IsVirtualFS(unquoted))
		{
			return VirtualFileSystem.MapExecutable(unquoted);
		}
		return path;
	}

	public static int parseCommandString(string cmdstr)
	{
		int pos = cmdstr.IndexOf(' ');
		if (pos == -1)
		{
			return cmdstr.Length;
		}
		if (cmdstr[0] == '"')
		{
			int close = cmdstr.IndexOf('"', 1);
			return close == -1 ? cmdstr.Length : close + 1;
		}
		if (Environment.OSVersion.Platform != PlatformID.Win32NT)
		{
			return pos;
		}
		IList<string> path = null;
		for (; ; )
		{
			string str = cmdstr.Substring(0, pos);
			if (IsPathRooted(str))
			{
				if (Exists(str))
				{
					return pos;
				}
			}
			else
			{
				if (path == null)
				{
					path = GetSearchPath();
				}
				foreach (string p in path)
				{
					if (Exists(Path.Combine(p, str)))
					{
						return pos;
					}
				}
			}
			if (pos == cmdstr.Length)
			{
				return cmdstr.IndexOf(' ');
			}
			pos = cmdstr.IndexOf(' ', pos + 1);
			if (pos == -1)
			{
				pos = cmdstr.Length;
			}
		}
	}

	private static List<string> GetSearchPath()
	{
		List<string> list = new List<string>();
		list.Add(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
		list.Add(Environment.CurrentDirectory);
		list.Add(Environment.SystemDirectory);
		string windir = Path.GetDirectoryName(Environment.SystemDirectory);
		list.Add(Path.Combine(windir, "System"));
		list.Add(windir);
		string path = Environment.GetEnvironmentVariable("PATH");
		if (path != null)
		{
			foreach (string p in path.Split(Path.PathSeparator))
			{
				list.Add(p);
			}
		}
		return list;
	}

	private static bool IsPathRooted(string path)
	{
		try
		{
			return Path.IsPathRooted(path);
		}
		catch (ArgumentException)
		{
			return false;
		}
	}

	private static bool Exists(string file)
	{
		try
		{
			if (File.Exists(file))
			{
				return true;
			}
			else if (Directory.Exists(file))
			{
				return false;
			}
			else if (file.IndexOf('.') == -1 && File.Exists(file + ".exe"))
			{
				return true;
			}
			else if (mapVfsExecutable(file) != file)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		catch
		{
			return false;
		}
	}
}
