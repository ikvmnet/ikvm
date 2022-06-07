/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Reflection;

using IKVM.Internal;

namespace IKVM.Java.Externs.sun.misc
{

    static class VM
	{

		public static void initialize()
		{

		}

		public static global::java.lang.ClassLoader latestUserDefinedLoader()
		{
			// testing shows that it is cheaper the get the full stack trace and then look at a few frames than getting the frames individually
			StackTrace trace = new StackTrace(2, false);
			for (int i = 0; i < trace.FrameCount; i++)
			{
				StackFrame frame = trace.GetFrame(i);
				MethodBase method = frame.GetMethod();
				if (method == null)
				{
					continue;
				}
				Type type = method.DeclaringType;
				if (type != null)
				{
					TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
					if (tw != null)
					{
						ClassLoaderWrapper classLoader = tw.GetClassLoader();
						AssemblyClassLoader acl = classLoader as AssemblyClassLoader;
						if (acl == null || acl.GetAssembly(tw) != typeof(object).Assembly)
						{
							global::java.lang.ClassLoader javaClassLoader = classLoader.GetJavaClassLoader();
							if (javaClassLoader != null)
							{
								return javaClassLoader;
							}
						}
					}
				}
			}

			return null;
		}

	}

}