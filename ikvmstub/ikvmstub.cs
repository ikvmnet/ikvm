/*
  Copyright (C) 2002, 2004, 2005 Jeroen Frijters

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
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Attributes;
using IKVM.Internal;

public class NetExp
{
	private static ZipOutputStream zipFile;
	private static Hashtable privateClasses = new Hashtable();

	public static void Main(string[] args)
	{
		Tracer.EnableTraceForDebug();
		if(args.Length != 1)
		{
			Console.Error.WriteLine("usage: ikvmstub <assemblyNameOrPath>");
			return;
		}
		Assembly assembly = null;
		FileInfo file = null;
		try
		{
			file = new FileInfo(args[0]);
		}
		catch(System.Exception x)
		{
			Console.Error.WriteLine("Error: unable to load \"{0}\"\n  {1}", args[0], x.Message);
			return;
		}
		if(file != null && file.Exists)
		{
			try
			{
				// If the same assembly can be found in the "Load" context, we prefer to use that
				// http://blogs.gotdotnet.com/suzcook/permalink.aspx/d5c5e14a-3612-4af1-a9b7-0a144c8dbf16
				// We use AssemblyName.FullName, because otherwise the assembly will be loaded in the
				// "LoadFrom" context using the path inside the AssemblyName object.
				assembly = Assembly.Load(AssemblyName.GetAssemblyName(args[0]).FullName);
				Console.Error.WriteLine("Warning: Assembly loaded from {0} instead", assembly.Location);
			}
			catch
			{
			}
			if(assembly == null)
			{
				assembly = Assembly.LoadFrom(args[0]);
			}
		}
		else
		{
			assembly = Assembly.LoadWithPartialName(args[0]);
		}
		int rc = 0;
		if(assembly == null)
		{
			Console.Error.WriteLine("Error: Assembly \"{0}\" not found", args[0]);
		}
		else
		{
			zipFile = new ZipOutputStream(new FileStream(assembly.GetName().Name + ".jar", FileMode.Create));
			try
			{
				ProcessAssembly(assembly);
				ProcessPrivateClasses(assembly);
			}
			catch(System.Exception x)
			{
				java.lang.Throwable.instancehelper_printStackTrace(IKVM.Runtime.Util.MapException(x));
				rc = 1;
			}
			zipFile.Close();
		}
		// FXBUG if we run a static initializer that starts a thread, we would never end,
		// so we force an exit here
		Environment.Exit(rc);
	}

	private static void WriteClass(string name, ClassFileWriter c)
	{
		zipFile.PutNextEntry(new ZipEntry(name));
		c.Write(zipFile);
	}

	private static void ProcessAssembly(Assembly assembly)
	{
		foreach(Type t in assembly.GetTypes())
		{
			if(t.IsPublic)
			{
				java.lang.Class c;
				try
				{
					c = java.lang.Class.forName(t.AssemblyQualifiedName, false, null);
				}
				catch(java.lang.ClassNotFoundException)
				{
					// types that IKVM doesn't support don't show up
					continue;
				}
				ProcessClass(assembly.FullName, c, null);
			}
		}
	}

	// TODO private classes should also be done handled for interfaces, fields and method arguments/return type
	private static void ProcessPrivateClasses(Assembly assembly)
	{
		Hashtable done = new Hashtable();
		bool keepGoing;
		do
		{
			Hashtable todo = privateClasses;
			privateClasses = new Hashtable();
			keepGoing = false;
			foreach(java.lang.Class c in todo.Values)
			{
				if(!done.ContainsKey(c.getName()))
				{
					keepGoing = true;
					done.Add(c.getName(), null);
					ProcessClass(assembly.FullName, c, c.getDeclaringClass());
				}
			}
		} while(keepGoing);
	}

	private static void AddToExportList(java.lang.Class c)
	{
		while(c.isArray())
		{
			c = c.getComponentType();
		}
		privateClasses[c.getName()] = c;
	}

	private static bool IsGenericType(java.lang.Class c)
	{
		// HACK huge hack, we look for the backtick
		return c.getName().IndexOf("_0060") > 0;
	}

	private static void ProcessClass(string assemblyName, java.lang.Class c, java.lang.Class outer)
	{
		string name = c.getName().Replace('.', '/');
		string super = null;
		if(c.getSuperclass() != null)
		{
			super = c.getSuperclass().getName().Replace('.', '/');
			// if the base class isn't public, we still need to export it (!)
			if(!java.lang.reflect.Modifier.isPublic(c.getSuperclass().getModifiers()))
			{
				AddToExportList(c.getSuperclass());
			}
		}
		if(c.isInterface())
		{
			super = "java/lang/Object";
		}
		Modifiers classmods = (Modifiers)c.getModifiers();
		if(outer != null)
		{
			// protected inner classes are actually public and private inner classes are actually package
			if((classmods & Modifiers.Protected) != 0)
			{
				classmods |= Modifiers.Public;
			}
			classmods &= ~(Modifiers.Static | Modifiers.Private | Modifiers.Protected);
		}
		ClassFileWriter f = new ClassFileWriter(classmods, name, super);
		f.AddStringAttribute("IKVM.NET.Assembly", assemblyName);
		if(IKVM.Runtime.Util.IsClassDeprecated(c))
		{
			f.AddAttribute(new DeprecatedAttribute(f));
		}
		InnerClassesAttribute innerClassesAttribute = null;
		if(outer != null)
		{
			innerClassesAttribute = new InnerClassesAttribute(f);
			string innername = name;
			int idx = name.LastIndexOf('$');
			if(idx >= 0)
			{
				innername = innername.Substring(idx + 1);
			}
			innerClassesAttribute.Add(name, outer.getName().Replace('.', '/'), innername, (ushort)c.getModifiers());
		}
		java.lang.Class[] interfaces = c.getInterfaces();
		for(int i = 0; i < interfaces.Length; i++)
		{
			if(java.lang.reflect.Modifier.isPublic(interfaces[i].getModifiers()))
			{
				f.AddInterface(interfaces[i].getName().Replace('.', '/'));
				if(IsGenericType(interfaces[i]))
				{
					AddToExportList(interfaces[i]);
				}
			}
		}
		java.lang.Class[] innerClasses = c.getDeclaredClasses();
		for(int i = 0; i < innerClasses.Length; i++)
		{
			Modifiers mods = (Modifiers)innerClasses[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				if(innerClassesAttribute == null)
				{
					innerClassesAttribute = new InnerClassesAttribute(f);
				}
				string namePart = innerClasses[i].getName();
				namePart = namePart.Substring(namePart.LastIndexOf('$') + 1);
				innerClassesAttribute.Add(innerClasses[i].getName().Replace('.', '/'), name, namePart, (ushort)innerClasses[i].getModifiers());
				ProcessClass(assemblyName, innerClasses[i], c);
			}
		}
		java.lang.reflect.Constructor[] constructors = c.getDeclaredConstructors();
		for(int i = 0; i < constructors.Length; i++)
		{
			Modifiers mods = (Modifiers)constructors[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				// TODO what happens if one of the argument types is non-public?
				java.lang.Class[] args = constructors[i].getParameterTypes();
				foreach(java.lang.Class arg in args)
				{
					if(IsGenericType(arg))
					{
						AddToExportList(arg);
					}
				}
				FieldOrMethod m = f.AddMethod(mods, "<init>", MakeSig(args, java.lang.Void.TYPE));
				CodeAttribute code = new CodeAttribute(f);
				code.MaxLocals = (ushort)(args.Length * 2 + 1);
				code.MaxStack = 3;
				ushort index1 = f.AddClass("java/lang/UnsatisfiedLinkError");
				ushort index2 = f.AddString("ikvmstub generated stubs can only be used on IKVM.NET");
				ushort index3 = f.AddMethodRef("java/lang/UnsatisfiedLinkError", "<init>", "(Ljava/lang/String;)V");
				code.ByteCode = new byte[] {
					187, (byte)(index1 >> 8), (byte)index1,	// new java/lang/UnsatisfiedLinkError
					89,										// dup
				    19,	 (byte)(index2 >> 8), (byte)index2,	// ldc_w "..."
					183, (byte)(index3 >> 8), (byte)index3, // invokespecial java/lang/UnsatisfiedLinkError/init()V
					191										// athrow
				};
				m.AddAttribute(code);
				AddExceptions(f, m, constructors[i].getExceptionTypes());
				if(IKVM.Runtime.Util.IsConstructorDeprecated(constructors[i]))
				{
					m.AddAttribute(new DeprecatedAttribute(f));
				}
			}
		}
		java.lang.reflect.Method[] methods = c.getDeclaredMethods();
		for(int i = 0; i < methods.Length; i++)
		{
			Modifiers mods = (Modifiers)methods[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				if((mods & Modifiers.Abstract) == 0)
				{
					mods |= Modifiers.Native;
				}
				// TODO what happens if one of the argument types (or the return type) is non-public?
				java.lang.Class[] args = methods[i].getParameterTypes();
				foreach(java.lang.Class arg in args)
				{
					if(IsGenericType(arg))
					{
						AddToExportList(arg);
					}
				}
				java.lang.Class retType = methods[i].getReturnType();
				if(IsGenericType(retType))
				{
					AddToExportList(retType);
				}
				FieldOrMethod m = f.AddMethod(mods, methods[i].getName(), MakeSig(args, retType));
				AddExceptions(f, m, methods[i].getExceptionTypes());
				if(IKVM.Runtime.Util.IsMethodDeprecated(methods[i]))
				{
					m.AddAttribute(new DeprecatedAttribute(f));
				}
			}
		}
		java.lang.reflect.Field[] fields = c.getDeclaredFields();
		for(int i = 0; i < fields.Length; i++)
		{
			Modifiers mods = (Modifiers)fields[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0 ||
				// Include serialVersionUID field, to make Japitools comparison more acurate
				((mods & (Modifiers.Static | Modifiers.Final)) == (Modifiers.Static | Modifiers.Final) &&
				fields[i].getName() == "serialVersionUID" && fields[i].getType() == java.lang.Long.TYPE))
			{
				// we use the IKVM runtime API to get constant value
				// NOTE we can't use Field.get() because that will run the static initializer and
				// also won't allow us to see the difference between constants and blank final fields.
				object constantValue = IKVM.Runtime.Util.GetFieldConstantValue(fields[i]);
				if(constantValue != null)
				{
					if(constantValue is java.lang.Boolean)
					{
						constantValue = ((java.lang.Boolean)constantValue).booleanValue();
					}
					else if(constantValue is java.lang.Byte)
					{
						constantValue = ((java.lang.Byte)constantValue).byteValue();
					}
					else if(constantValue is java.lang.Short)
					{
						constantValue = ((java.lang.Short)constantValue).shortValue();
					}
					else if(constantValue is java.lang.Character)
					{
						constantValue = ((java.lang.Character)constantValue).charValue();
					}
					else if(constantValue is java.lang.Integer)
					{
						constantValue = ((java.lang.Integer)constantValue).intValue();
					}
					else if(constantValue is java.lang.Long)
					{
						constantValue = ((java.lang.Long)constantValue).longValue();
					}
					else if(constantValue is java.lang.Float)
					{
						constantValue = ((java.lang.Float)constantValue).floatValue();
					}
					else if(constantValue is java.lang.Double)
					{
						constantValue = ((java.lang.Double)constantValue).doubleValue();
					}
					else if(constantValue is string)
					{
						// no conversion needed
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				// TODO what happens if the field type is non-public?
				java.lang.Class fieldType = fields[i].getType();
				if(IsGenericType(fieldType))
				{
					AddToExportList(fieldType);
				}
				FieldOrMethod fld = f.AddField(mods, fields[i].getName(), ClassToSig(fieldType), constantValue);
				if(IKVM.Runtime.Util.IsFieldDeprecated(fields[i]))
				{
					fld.AddAttribute(new DeprecatedAttribute(f));
				}
			}
		}
		if(innerClassesAttribute != null)
		{
			f.AddAttribute(innerClassesAttribute);
		}
		WriteClass(name + ".class", f);
	}

	private static void AddExceptions(ClassFileWriter f, FieldOrMethod m, java.lang.Class[] exceptions)
	{
		if(exceptions.Length > 0)
		{
			ExceptionsAttribute attrib = new ExceptionsAttribute(f);
			foreach(java.lang.Class x in exceptions)
			{
				// TODO what happens if one of the exception types is non-public?
				attrib.Add(x.getName().Replace('.', '/'));
			}
			m.AddAttribute(attrib);
		}
	}

	private static string MakeSig(java.lang.Class[] args, java.lang.Class ret)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('(');
		for(int i = 0; i < args.Length; i++)
		{
			sb.Append(ClassToSig(args[i]));
		}
		sb.Append(')');
		sb.Append(ClassToSig(ret));
		return sb.ToString();
	}

	private static string ClassToSig(java.lang.Class c)
	{
		if(c.isPrimitive())
		{
			if(c == java.lang.Void.TYPE)
			{
				return "V";
			}
			else if(c == java.lang.Byte.TYPE)
			{
				return "B";
			}
			else if(c == java.lang.Boolean.TYPE)
			{
				return "Z";
			}
			else if(c == java.lang.Short.TYPE)
			{
				return "S";
			}
			else if(c == java.lang.Character.TYPE)
			{
				return "C";
			}
			else if(c == java.lang.Integer.TYPE)
			{
				return "I";
			}
			else if(c == java.lang.Long.TYPE)
			{
				return "J";
			}
			else if(c == java.lang.Float.TYPE)
			{
				return "F";
			}
			else if(c == java.lang.Double.TYPE)
			{
				return "D";
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
		else if(c.isArray())
		{
			return "[" + ClassToSig(c.getComponentType());
		}
		else
		{
			return "L" + c.getName().Replace('.', '/') + ";";
		}
	}
}
