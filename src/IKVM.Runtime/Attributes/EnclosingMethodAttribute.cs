/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

using IKVM.Runtime;

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class EnclosingMethodAttribute : Attribute
	{

		private string className;
		private string methodName;
		private string methodSig;

		public EnclosingMethodAttribute(string className, string methodName, string methodSig)
		{
			this.className = UnicodeUtil.UnescapeInvalidSurrogates(className);
			this.methodName = UnicodeUtil.UnescapeInvalidSurrogates(methodName);
			this.methodSig = UnicodeUtil.UnescapeInvalidSurrogates(methodSig);
		}

		internal EnclosingMethodAttribute SetClassName(Type type)
		{
			if (className == null)
				className = IKVM.Runtime.RuntimeClassLoaderFactory.GetJavaTypeFromType(type.DeclaringType).Name;

			return this;
		}

        public string ClassName => className;

        public string MethodName => methodName;

        public string MethodSignature => methodSig;

    }

}
