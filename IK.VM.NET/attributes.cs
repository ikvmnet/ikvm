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
using System.Reflection.Emit;

[AttributeUsage(AttributeTargets.Class)]
public class OverrideStubTypeAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Assembly)]
public class IKVMAssemblyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class UnloadableTypeAttribute : Attribute
{
	private string name;

	public UnloadableTypeAttribute(string name)
	{
		this.name = name;
	}

	public string Name
	{
		get
		{
			return name;
		}
	}
}

[AttributeUsage(AttributeTargets.All)]
public class ModifiersAttribute : Attribute
{
	private Modifiers modifiers;

	public ModifiersAttribute(Modifiers modifiers)
	{
		this.modifiers = modifiers;
	}

	public Modifiers Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	public static bool IsSynthetic(FieldInfo fi)
	{
		return (GetModifiers(fi) & Modifiers.Synthetic) != 0;
	}

	public static bool IsSynthetic(MethodBase mb)
	{
		return (GetModifiers(mb) & Modifiers.Synthetic) != 0;
	}

	public static Modifiers GetModifiers(MethodBase mb)
	{
		object[] customAttribute = mb.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
		}
		Modifiers modifiers = 0;
		if(mb.IsPublic)
		{
			modifiers |= Modifiers.Public;
		}
		if(mb.IsPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(mb.IsFamily || mb.IsFamilyOrAssembly)
		{
			modifiers |= Modifiers.Protected;
		}
		// NOTE Java doesn't support non-virtual methods, but we set the Final modifier for
		// non-virtual methods to approximate the semantics
		if(mb.IsFinal || (!mb.IsStatic && !mb.IsVirtual))
		{
			modifiers |= Modifiers.Final;
		}
		if(mb.IsAbstract)
		{
			modifiers |= Modifiers.Abstract;
		}
		if(mb.IsStatic)
		{
			modifiers |= Modifiers.Static;
		}
		if((mb.GetMethodImplementationFlags() & MethodImplAttributes.Synchronized) != 0)
		{
			modifiers |= Modifiers.Synchronized;
		}
		if((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
		{
			modifiers |= Modifiers.Native;
		}
		return modifiers;
	}

	public static Modifiers GetModifiers(FieldInfo fi)
	{
		object[] customAttribute = fi.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
		}
		// NOTE privatescope fields are always treated as synthetic (even when they are in a .NET type, because
		// Java wouldn't be able to cope with them anyway, because of potential name clashes)
		if((fi.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
		{
			return Modifiers.Synthetic;
		}
		Modifiers modifiers = 0;
		if(fi.IsPublic)
		{
			modifiers |= Modifiers.Public;
		}
		if(fi.IsPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(fi.IsFamily || fi.IsFamilyOrAssembly)
		{
			modifiers |= Modifiers.Protected;
		}
		if(fi.IsInitOnly || fi.IsLiteral)
		{
			modifiers |= Modifiers.Final;
		}
		if(fi.IsNotSerialized)
		{
			modifiers |= Modifiers.Transient;
		}
		if(fi.IsStatic)
		{
			modifiers |= Modifiers.Static;
		}
		// TODO reflection doesn't support volatile
		return modifiers;
	}

	public static Modifiers GetModifiers(Type type)
	{
		object[] customAttribute = type.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
		}
		// only returns public, protected, private, final, static, abstract and interface (as per
		// the documentation of Class.getModifiers())
		Modifiers modifiers = 0;
		if(type.IsPublic)
		{
			modifiers |= Modifiers.Public;
		}
		if(type.IsNestedPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(type.IsNestedFamily || type.IsNestedFamORAssem)
		{
			modifiers |= Modifiers.Protected;
		}
		if(type.IsSealed)
		{
			modifiers |= Modifiers.Final;
		}
		if(type.DeclaringType != null)
		{
			modifiers |= Modifiers.Static;
		}
		if(type.IsAbstract)
		{
			modifiers |= Modifiers.Abstract;
		}
		if(type.IsInterface)
		{
			modifiers |= Modifiers.Interface;
		}
		return modifiers;
	}

	public static void SetModifiers(MethodBuilder mb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		mb.SetCustomAttribute(customAttributeBuilder);
	}

	public static void SetModifiers(ConstructorBuilder cb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		cb.SetCustomAttribute(customAttributeBuilder);
	}

	public static void SetModifiers(FieldBuilder fb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		fb.SetCustomAttribute(customAttributeBuilder);
	}

	public static void SetModifiers(TypeBuilder tb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		tb.SetCustomAttribute(customAttributeBuilder);
	}
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Class)]
public class StackTraceInfoAttribute : Attribute
{
	private bool hidden;
	private string className;
	private bool truncate;
	private int eatFrames;

	public bool Hidden
	{
		get
		{
			return hidden;
		}
		set
		{
			hidden = value;
		}
	}

	public int EatFrames
	{
		get
		{
			return eatFrames;
		}
		set
		{
			eatFrames = value;
		}
	}

	public string Class
	{
		get
		{
			return className;
		}
		set
		{
			className = value;
		}
	}

	public bool Truncate
	{
		get
		{
			return truncate;
		}
		set
		{
			truncate = value;
		}
	}
}
