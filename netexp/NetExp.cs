/*
  Copyright (C) 2002 Jeroen Frijters

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
using ICSharpCode.SharpZipLib.Zip;
using java.lang;
using java.lang.reflect;

public class NetExp
{
	private static ZipOutputStream zipFile;

	public static void Main(string[] args)
	{
		Assembly assembly = null;
		FileInfo file = new FileInfo(args[0]);
		if(file.Exists)
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
		if(assembly == null)
		{
			Console.Error.WriteLine("Error: Assembly \"{0}\" not found", args[0]);
		}
		else
		{
			zipFile = new ZipOutputStream(new FileStream(assembly.GetName().Name + ".jar", FileMode.Create));
			// HACK if we're doing the "classpath" assembly, also include the remapped types
			// java.lang.Object and java.lang.Throwable are automatic, because of the $OverrideStub
			if(assembly.GetType("java.lang.Object$OverrideStub") != null)
			{
				ProcessClass(assembly.FullName, Class.forName("java.lang.String"), null);
				ProcessClass(assembly.FullName, Class.forName("java.lang.Comparable"), null);
			}
			ProcessAssembly(assembly);
			zipFile.Close();
		}
		// HACK if we run on the "classpath" assembly, the awt thread gets started,
		// so we force an exit here
		Environment.Exit(0);
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
				ProcessClass(assembly.FullName, Class.forName(t.AssemblyQualifiedName, false, null), null);
			}
		}
	}

	private static void ProcessClass(string assemblyName, Class c, Class outer)
	{
		string name = c.getName().Replace('.', '/');
		//Console.WriteLine(name);
		string super = null;
		if(c.getSuperclass() != null)
		{
			super = c.getSuperclass().getName().Replace('.', '/');
		}
		if(c.isInterface())
		{
			super = "java/lang/Object";
		}
		ClassFileWriter f = new ClassFileWriter((Modifiers)c.getModifiers(), name, super);
		f.AddStringAttribute("IKVM.NET.Assembly", assemblyName);
		InnerClassesAttribute innerClassesAttribute = null;
		if(outer != null)
		{
			innerClassesAttribute = new InnerClassesAttribute(f);
			innerClassesAttribute.Add(name, outer.getName().Replace('.', '/'), null, (ushort)Modifiers.Public);
		}
		Class[] innerClasses = c.getDeclaredClasses();
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
		Constructor[] constructors = c.getDeclaredConstructors();
		for(int i = 0; i < constructors.Length; i++)
		{
			Modifiers mods = (Modifiers)constructors[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				f.AddMethod(mods | Modifiers.Native, "<init>", MakeSig(constructors[i].getParameterTypes(), java.lang.Void.TYPE));
			}
		}
		Method[] methods = c.getDeclaredMethods();
		for(int i = 0; i < methods.Length; i++)
		{
			Modifiers mods = (Modifiers)methods[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				if((mods & Modifiers.Abstract) == 0)
				{
					mods |= Modifiers.Native;
				}
				f.AddMethod(mods, methods[i].getName(), MakeSig(methods[i].getParameterTypes(), methods[i].getReturnType()));
			}
		}
		Field[] fields = c.getDeclaredFields();
		for(int i = 0; i < fields.Length; i++)
		{
			Modifiers mods = (Modifiers)fields[i].getModifiers();
			if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
			{
				object constantValue = null;
				// HACK we only look for constants on potential constant fields, to trigger less static initializers
				if((mods & (Modifiers.Final | Modifiers.Static)) == (Modifiers.Final | Modifiers.Static) &&
					(fields[i].getType().isPrimitive() || fields[i].getType() == Class.forName("java.lang.String")))
				{
					// HACK we use a non-standard API to get constant value
					// NOTE we can't use Field.get() because that will run the static initializer and
					// also won't allow us to see the difference between constants and blank final fields.
					constantValue = NativeCode.java.lang.reflect.Field.getConstant(fields[i]);
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
				}
				f.AddField(mods, fields[i].getName(), ClassToSig(fields[i].getType()), constantValue);
			}
		}
		if(innerClassesAttribute != null)
		{
			f.AddAttribute(innerClassesAttribute);
		}
		WriteClass(name + ".class", f);
	}

	private static string MakeSig(Class[] args, Class ret)
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

	private static string ClassToSig(Class c)
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
