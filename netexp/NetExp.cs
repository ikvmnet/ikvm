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
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;

public class NetExp
{
	private static ZipOutputStream zipFile;

	public static void Main(string[] args)
	{
		Assembly assembly = null;
		if(new FileInfo(args[0]).Exists)
		{
			assembly = Assembly.LoadFrom(args[0]);
		}
		if(assembly == null)
		{
			Console.Error.WriteLine("Error: Assembly \"{0}\" not found", args[0]);
		}
		else
		{
			zipFile = new ZipOutputStream(new FileStream(assembly.GetName().Name + ".jar", FileMode.Create));
			ProcessAssembly(assembly);
			zipFile.Close();
		}
	}

	private static void ProcessAssembly(Assembly assembly)
	{
		object[] attribs = assembly.GetCustomAttributes(typeof(CLSCompliantAttribute), false);
		bool assemblyIsCLSCompliant = true;
		if(attribs.Length != 1)
		{
			assemblyIsCLSCompliant = false;
			Console.Error.WriteLine("Warning: assembly has no (or multiple) CLS compliance attribute");
		}
		else if(!((CLSCompliantAttribute)attribs[0]).IsCompliant)
		{
			assemblyIsCLSCompliant = false;
			Console.Error.WriteLine("Warning: assembly is marked as non-CLS compliant");
		}
		foreach(Type t in assembly.GetTypes())
		{
			bool typeIsCLSCompliant = true;
			if(assemblyIsCLSCompliant)
			{
				attribs = t.GetCustomAttributes(typeof(CLSCompliantAttribute), false);
				if(attribs.Length == 1)
				{
					typeIsCLSCompliant = ((CLSCompliantAttribute)attribs[0]).IsCompliant;
				}
			}
			if(t.IsPublic && typeIsCLSCompliant)
			{
				ProcessType(t);
			}
		}
	}

	private static object UnwrapEnum(object o)
	{
		// is there a way to generically convert a boxed enum to its boxed underlying value?
		Type underlyingType = Enum.GetUnderlyingType(o.GetType());
		if (underlyingType == typeof(long))
		{
			o = (long)o;
		}	
		else if(underlyingType == typeof(int))
		{
			o = (int)o;
		}
		else if(underlyingType == typeof(short))
		{
			o = (short)o;
		}
		else if (underlyingType == typeof(sbyte))
		{
			o = (sbyte)o;
		}
		else if (underlyingType == typeof(ulong))
		{
			o = (ulong)o;
		}		
		else if (underlyingType == typeof(uint))
		{
			o = (uint)o;
		}
		else if (underlyingType == typeof(ushort))
		{
			o = (ushort)o;
		}
		else if (underlyingType == typeof(byte))
		{
			o = (byte)o;
		}
		else
		{
			throw new NotImplementedException(o.GetType().Name);
		}
		return o;
	}

	private static string ClassName(Type t)
	{
		if(t == typeof(object))
		{
			return "java/lang/Object";
		}
		else if(t == typeof(string))
		{
			return "java/lang/String";
		}
		string name = t.FullName;
		int lastDot = name.LastIndexOf('.');
		if(lastDot > 0)
		{
			name = name.Substring(0, lastDot).ToLower() + name.Substring(lastDot);
		}
		return name.Replace('.', '/');
	}

	// returns the mapped type in signature format (e.g. Ljava/lang/String;)
	private static string SigType(Type t)
	{
		if(t.IsByRef)
		{
			return "[" + SigType(t.GetElementType());
		}
		if(t.IsEnum)
		{
			t = Enum.GetUnderlyingType(t);
		}
		if(t == typeof(void))
		{
			return "V";
		}
		else if(t == typeof(byte) || t == typeof(sbyte))
		{
			return "B";
		}
		else if(t == typeof(bool))
		{
			return "Z";
		}
		else if(t == typeof(short) || t == typeof(ushort))
		{
			return "S";
		}
		else if(t == typeof(char))
		{
			return "C";
		}
		else if(t == typeof(int) || t == typeof(uint))
		{
			return "I";
		}
		else if(t == typeof(long) || t == typeof(ulong))
		{
			return "J";
		}
		else if(t == typeof(float))
		{
			return "F";
		}
		else if(t == typeof(double))
		{
			return "D";
		}
		else if(t == typeof(IntPtr))
		{
			// HACK
			return "I";
		}
		else if(t.IsArray)
		{
			StringBuilder sb = new StringBuilder();
			while(t.IsArray)
			{
				sb.Append('[');
				t = t.GetElementType();
			}
			sb.Append(SigType(t));
			return sb.ToString();
		}
		else if(!t.IsPrimitive)
		{
			return "L" + ClassName(t) + ";";
		}
		else
		{
			throw new NotImplementedException(t.FullName);
		}
	}

	private static void ProcessType(Type type)
	{
		if(type == typeof(object))// || type == typeof(string))
		{
			// special case for System.Object & System.String, don't emit those
			return;
		}
		string name = ClassName(type);
		if(type == typeof(string))
		{
			name = "system/String";
		}
		string super;
		if(type.BaseType == null)
		{
			// in .NET interfaces don't have a baseType, but in Java they "extend" java/lang/Object
			super = "java/lang/Object";
		}
		else
		{
			if(type == typeof(Exception))
			{
				super = "java/lang/Throwable";
			}
			else
			{
				super = ClassName(type.BaseType);
			}
		}
		Modifiers mods = Modifiers.Public | Modifiers.Super;
		if(type.IsInterface)
		{
			mods |= Modifiers.Interface;
		}
		if(type.IsSealed)
		{
			mods |= Modifiers.Final;
		}
		if(type.IsAbstract)
		{
			mods |= Modifiers.Abstract;
		}
		ClassFileWriter f = new ClassFileWriter(mods, name, super);
		f.AddStringAttribute("IK.VM.NET.Type", type.AssemblyQualifiedName);
		FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		Hashtable clashtable = new Hashtable();
		for(int i = 0; i < fields.Length; i++)
		{
			if(fields[i].IsPublic || fields[i].IsFamily)
			{
				object[] attribs = fields[i].GetCustomAttributes(typeof(CLSCompliantAttribute), false);
				if(attribs.Length == 1 && !((CLSCompliantAttribute)attribs[0]).IsCompliant)
				{
					// skip non-CLS compliant field
				}
				else
				{
					ProcessField(type, f, fields[i], clashtable);
				}
			}
		}
		clashtable.Clear();
		ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		for(int i = 0; i < constructors.Length; i++)
		{
			if(constructors[i].IsPublic || constructors[i].IsFamily)
			{
				object[] attribs = constructors[i].GetCustomAttributes(typeof(CLSCompliantAttribute), false);
				if(attribs.Length == 1 && !((CLSCompliantAttribute)attribs[0]).IsCompliant)
				{
					// skip non-CLS compliant constructor
				}
				else
				{
					ProcessMethod(type, f, constructors[i], clashtable);
				}
			}
		}
		MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
		for(int i = 0; i < methods.Length; i++)
		{
			if(methods[i].IsPublic || methods[i].IsFamily)
			{
				object[] attribs = methods[i].GetCustomAttributes(typeof(CLSCompliantAttribute), false);
				if(attribs.Length == 1 && !((CLSCompliantAttribute)attribs[0]).IsCompliant)
				{
					// skip non-CLS compliant method
				}
				else
				{
					ProcessMethod(type, f, methods[i], clashtable);
				}
			}
		}
		// for delegates we have to construct the inner interface
		if(type.IsSubclassOf(typeof(MulticastDelegate)))
		{
			InnerClassesAttribute innerclasses = new InnerClassesAttribute(f);
			string outer = ClassName(type);
			innerclasses.Add(outer + "$Method", outer, "Method", 0x609);
			f.AddAttribute(innerclasses);
			// now we construct the inner interface type
			ClassFileWriter iface = new ClassFileWriter(Modifiers.Interface | Modifiers.Public | Modifiers.Abstract, outer + "$Method", "java/lang/Object");
			MethodInfo invoke = type.GetMethod("Invoke");
			StringBuilder sb = new StringBuilder();
			sb.Append('(');
			ParameterInfo[] parameters = invoke.GetParameters();
			for(int i = 0; i < parameters.Length; i++)
			{
				sb.Append(SigType(parameters[i].ParameterType));
			}
			sb.Append(')');
			sb.Append(SigType(invoke.ReturnType));
			// TODO IK.VM.NET.Sig must be set here as well
			iface.AddMethod(Modifiers.Public | Modifiers.Abstract, "Invoke", sb.ToString());
			innerclasses = new InnerClassesAttribute(iface);
			innerclasses.Add(outer + "$Method", outer, "Method", 0x609);
			iface.AddAttribute(innerclasses);
			WriteClass(outer + "$Method.class", iface);
		}
		WriteClass(name + ".class", f);
	}

	private static void WriteClass(string name, ClassFileWriter c)
	{
		zipFile.PutNextEntry(new ZipEntry(name));
		c.Write(zipFile);
	}

	private static void ProcessField(Type type, ClassFileWriter f, FieldInfo fi, Hashtable clashtable)
	{
		Modifiers access;
		if(fi.IsPublic)
		{
			access = Modifiers.Public;
		}
		else
		{
			access = Modifiers.Protected;
		}
		object v = null;
		if(fi.IsLiteral)
		{
			v = fi.GetValue(null);
			if(v is Enum)
			{
				v = UnwrapEnum(v);
			}
			if(v is byte)
			{
				v = (int)(byte)v;
			}
			else if (v is sbyte)
			{
				v = (int)(sbyte)v;
			}
			else if(v is char)
			{
				v = (int)(char)v;
			}
			else if(v is short)
			{
				v = (int)(short)v;
			}
			else if(v is ushort)
			{
				v = (int)(ushort)v;
			}
			else if(v is bool)
			{
				v = ((bool)v) ? 1 : 0;
			}
			else if(v is int || v is uint || v is ulong || v is long || v is float || v is double || v is string)
			{
			}
			else
			{
				throw new NotImplementedException(v.GetType().FullName);
			}
			access |= Modifiers.Static | Modifiers.Final;
		}
		else
		{
			if(fi.IsInitOnly)
			{
				access |= Modifiers.Final;
			}
			if(fi.IsStatic)
			{
				access |= Modifiers.Static;
			}
			if(type.IsEnum)
			{
				// we don't want the value__ field
				return;
			}
		}
		string sig = SigType(fi.FieldType);
		string key = fi.Name + sig;
		if(clashtable.ContainsKey(key))
		{
			// TODO instead of skipping, we should mangle the name
			Console.Error.WriteLine("Skipping field " + type.FullName + "." + fi.Name + " (type " + sig + ") because it clashes");
		}
		else
		{
			clashtable.Add(key, key);
			f.AddField(access, fi.Name, sig, v);
		}
	}

	private static void ProcessMethod(Type type, ClassFileWriter f, MethodBase mb, Hashtable clashtable)
	{
		Modifiers access = 0;
		if(!mb.IsAbstract)
		{
			access = Modifiers.Native;
		}
		if(mb.IsPublic)
		{
			access |= Modifiers.Public;
		}
		else
		{
			access |= Modifiers.Protected;
		}
		if(mb.IsFinal || !mb.IsVirtual)
		{
			access |= Modifiers.Final;
		}
		if(mb.IsStatic)
		{
			access |= Modifiers.Static;
		}
		if(mb.IsAbstract)
		{
			access |= Modifiers.Abstract;
		}
		// special case for delegate constructors!
		if(mb.IsConstructor && type.IsSubclassOf(typeof(MulticastDelegate)))
		{
			access &= ~Modifiers.Final;
			f.AddMethod(access, "<init>", "(L" + ClassName(type) + "$Method;)V");
			return;
		}
		// HACK the native signature is really is very lame way of storing the signature
		// TODO only store it when it doesn't match the Java sig and split it into parts (instead of one giant string)
		StringBuilder nativesig = new StringBuilder();
		StringBuilder sb = new StringBuilder();
		sb.Append('(');
		ParameterInfo[] parameters = mb.GetParameters();
		string sep = "";
		for(int i = 0; i < parameters.Length; i++)
		{
			if(parameters[i].ParameterType.IsPointer)
			{
				// Java doesn't support pointer parameters
				return;
			}
			sb.Append(SigType(parameters[i].ParameterType));
			nativesig.Append(sep).Append(parameters[i].ParameterType.AssemblyQualifiedName);
			sep = "|";
		}
		sb.Append(')');
		if(mb.IsConstructor)
		{
			// HACK constructors may not be final in Java
			access &= ~Modifiers.Final;
			sb.Append('V');
		}
		else
		{
			sb.Append(SigType(((MethodInfo)mb).ReturnType));
		}
		string name = mb.IsConstructor ? "<init>" : mb.Name;
		string sig = sb.ToString();
		string key = name + sig;
		if(clashtable.ContainsKey(key))
		{
			// TODO instead of skipping, we should mangle the name
			Console.Error.WriteLine("Skipping method " + type.FullName + "." + name + sig + " because it clashes");
		}
		else
		{
			clashtable.Add(key, key);
			f.AddMethod(access, name, sig)
				.AddAttribute(new StringAttribute(f.AddUtf8("IK.VM.NET.Sig"), f.AddUtf8(nativesig.ToString())));
		}
	}
}
