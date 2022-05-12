/*
  Copyright (C) 2004-2015 Jeroen Frijters

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

namespace IKVM.Internal
{
	internal static class CoreClasses
	{
		internal static class cli
		{
			internal static class System
			{
				internal static class Object
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Object() { }
					internal static readonly TypeWrapper Wrapper = DotNetTypeWrapper.GetWrapperFromDotNetType(Types.Object);
				}

				internal static class Exception
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Exception() { }
					internal static readonly TypeWrapper Wrapper = DotNetTypeWrapper.GetWrapperFromDotNetType(Types.Exception);
				}
			}
		}

		internal static class ikvm
		{
			internal static class @internal
			{
				internal static class CallerID
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static CallerID() { }
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.CallerID");
				}
			}
		}

		internal static class java
		{
			internal static class io
			{
				internal static class Serializable
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Serializable() { }
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.io.Serializable");
				}
			}

			internal static class lang
			{
				internal static class Object
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Object() {}
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Object");
				}

				internal static class String
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static String() {}
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.String");
				}

				internal static class Class
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Class() {}
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Class");
				}

				internal static class Cloneable
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Cloneable() {}
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Cloneable");
				}

				internal static class Throwable
				{
					// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
					// (we don't want the classes to be loaded prematurely, because they might not be available then)
					static Throwable() {}
					internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Throwable");
				}

				internal static class invoke
				{
					internal static class MethodHandle
					{
						// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
						// (we don't want the classes to be loaded prematurely, because they might not be available then)
						static MethodHandle() { }
						internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.invoke.MethodHandle");
					}

					internal static class MethodType
					{
						// NOTE we have a dummy static initializer, to make sure we don't get the beforeFieldInit attribute
						// (we don't want the classes to be loaded prematurely, because they might not be available then)
						static MethodType() { }
						internal static readonly TypeWrapper Wrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.invoke.MethodType");
					}
				}
			}
		}
	}
}
