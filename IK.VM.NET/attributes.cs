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
