/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using IKVM.Attributes;

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{
        internal sealed class ConstantPoolItemMethodref : ConstantPoolItemMI
		{
			internal ConstantPoolItemMethodref(BigEndianBinaryReader br) : base(br)
			{
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				base.Link(thisType, mode);
				TypeWrapper wrapper = GetClassType();
				if(wrapper != null && !wrapper.IsUnloadable)
				{
					method = wrapper.GetMethodWrapper(Name, Signature, !ReferenceEquals(Name, StringConstants.INIT));
					if(method != null)
					{
						method.Link(mode);
					}
					if(Name != StringConstants.INIT
						&& !thisType.IsInterface
						&& (!JVM.AllowNonVirtualCalls || (thisType.Modifiers & Modifiers.Super) == Modifiers.Super)
						&& thisType != wrapper
						&& thisType.IsSubTypeOf(wrapper))
					{
						invokespecialMethod = thisType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true);
						if(invokespecialMethod != null)
						{
							invokespecialMethod.Link(mode);
						}
					}
				}
			}
		}
	}

}
