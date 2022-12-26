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

using IKVM.Internal;

static class DynamicMethodUtils
{

    /// <summary>
    /// Creates a new <see cref="DynamicMethod"/> owned by the appropriate metadata element.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="owner"></param>
    /// <param name="nonPublic"></param>
    /// <param name="returnType"></param>
    /// <param name="paramTypes"></param>
    /// <returns></returns>
    [SecuritySafeCritical]
    internal static DynamicMethod Create(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
    {
        if (ReflectUtil.CanOwnDynamicMethod(owner))
            return new DynamicMethod(name, returnType, paramTypes, owner);
        else
            return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, owner.Module, true);
    }

}