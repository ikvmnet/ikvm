/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Security;
using System.Security.Permissions;
using System.Threading;
using IKVM.Internal;

static class DynamicMethodUtils
{
#if NET_4_0
	private static Module dynamicModule;
#endif

	[SecuritySafeCritical]
	internal static DynamicMethod Create(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
	{
		try
		{
#if NET_4_0
			if (dynamicModule == null)
			{
				// we have to create a module that is security critical to hold the dynamic method, if we want to be able to emit unverifiable code
				AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("<DynamicMethodHolder>"), AssemblyBuilderAccess.RunAndCollect);
				Interlocked.CompareExchange(ref dynamicModule, ab.DefineDynamicModule("<DynamicMethodHolder>"), null);
			}
			return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, dynamicModule, true);
#else
			if (!ReflectUtil.CanOwnDynamicMethod(owner))
			{
				// interfaces and arrays aren't allowed as owners of dynamic methods
				return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, owner.Module, true);
			}
			else
			{
				return new DynamicMethod(name, returnType, paramTypes, owner);
			}
#endif
		}
		catch (SecurityException)
		{
			if (nonPublic && !RestrictedMemberAccess)
			{
				// we don't have RestrictedMemberAccess, so we stick the dynamic method in our module and hope for the best
				// (i.e. that we're trying to access something with assembly access in an assembly that lets us)
				return new DynamicMethod(name, returnType, paramTypes, typeof(DynamicMethodUtils).Module);
			}
			// apparently we don't have full trust, so we try again with .NET 2.0 SP1 method
			// and we only request restrictSkipVisibility if it is required
			return new DynamicMethod(name, returnType, paramTypes, nonPublic);
		}
	}

	private static bool RestrictedMemberAccess
	{
		get
		{
			try
			{
				new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
				return true;
			}
			catch (SecurityException)
			{
				return false;
			}
		}
	}
}
