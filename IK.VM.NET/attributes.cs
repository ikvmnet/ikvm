/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
public sealed class ExceptionIsUnsafeForMappingAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class RemappedInterfaceMethodAttribute : Attribute
{
	private string name;
	private string mappedTo;

	public RemappedInterfaceMethodAttribute(string name, string mappedTo)
	{
		this.name = name;
		this.mappedTo = mappedTo;
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public string MappedTo
	{
		get
		{
			return mappedTo;
		}
	}
}

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class RemappedClassAttribute : Attribute
{
	private string name;
	private Type remappedType;

	public RemappedClassAttribute(string name, Type remappedType)
	{
		this.name = name;
		this.remappedType = remappedType;
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public Type RemappedType
	{
		get
		{
			return remappedType;
		}
	}
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class RemappedTypeAttribute : Attribute
{
	private Type type;

	public RemappedTypeAttribute(Type type)
	{
		this.type = type;
	}

	public Type Type
	{
		get
		{
			return type;
		}
	}
}

[AttributeUsage(AttributeTargets.Module)]
public sealed class JavaModuleAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Assembly)]
public sealed class NoPackagePrefixAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Method)]
public sealed class UnloadableTypeAttribute : Attribute
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

[AttributeUsage(AttributeTargets.Struct)]
public sealed class GhostInterfaceAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Class)]
public sealed class StackTraceInfoAttribute : Attribute
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
