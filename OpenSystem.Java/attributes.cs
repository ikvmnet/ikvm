/*
  Copyright (C) 2003 Jeroen Frijters

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

namespace OpenSystem.Java
{
	// Whenever the VM or compiler generates a helper class/method/field, it should be marked
	// with this custom attribute, so that it can be hidden from Java reflection.
	// NOTE when this attribute is applied to a class, it means that instances of this class
	// will appear to be instances of the base class.
	[AttributeUsage(AttributeTargets.All)]
	public class HideFromReflectionAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly)]
	public class JavaAssemblyAttribute : Attribute
	{
	}

	[Flags]
	public enum Modifiers : ushort
	{
		Public			= 0x0001,
		Private			= 0x0002,
		Protected		= 0x0004,
		Static			= 0x0008,
		Final			= 0x0010,
		Super			= 0x0020,
		Synchronized	= 0x0020,
		Volatile		= 0x0040,
		Transient		= 0x0080,
		Native			= 0x0100,
		Interface		= 0x0200,
		Abstract		= 0x0400,
		Strictfp		= 0x0800
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
	}

	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true)]
	public class ThrowsAttribute : Attribute
	{
		private string className;

		public ThrowsAttribute(string className)
		{
			this.className = className;
		}

		// dotted Java class name (e.g. java.lang.Throwable)
		public string ClassName
		{
			get
			{
				return className;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class InnerClassAttribute : Attribute
	{
		private string innerClassName;
		private string outerClassName;
		private string name;
		private Modifiers modifiers;

		public InnerClassAttribute(string innerClassName, string outerClassName, string name, Modifiers modifiers)
		{
			this.innerClassName = innerClassName;
			this.outerClassName = outerClassName;
			this.name = name;
			this.modifiers = modifiers;
		}

		public string InnerClassName
		{
			get
			{
				return innerClassName;
			}
		}

		public string OuterClassName
		{
			get
			{
				return OuterClassName;
			}
		}

		// NOTE returns null for anonymous inner classes
		public string Name
		{
			get
			{
				return name;
			}
		}

		public Modifiers Modifiers
		{
			get
			{
				return modifiers;
			}
		}
	}
}
