/*
  Copyright (C) 2009 Jeroen Frijters

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
using System.Collections.Generic;
using System.Text;

namespace IKVM.Reflection
{
	public abstract class MemberInfo
	{
		public abstract string Name { get; }
		public abstract Type DeclaringType { get; }
		public abstract MemberTypes MemberType { get; }

		public virtual int MetadataToken
		{
			get { throw new NotSupportedException(); }
		}

		public abstract Module Module
		{
			get;
		}

		public bool IsDefined(Type attributeType, bool inherit)
		{
			return CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;
		}

		internal virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			return this.Module.GetCustomAttributes(this.MetadataToken);
		}

		internal static bool BindingFlagsMatch(bool state, BindingFlags flags, BindingFlags trueFlag, BindingFlags falseFlag)
		{
			return (state && (flags & trueFlag) == trueFlag)
				|| (!state && (flags & falseFlag) == falseFlag);
		}
	}
}
