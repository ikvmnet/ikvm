/*
  Copyright (C) 2006 Jeroen Frijters

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

package sun.reflect;

import gnu.classpath.VMStackWalker;
import java.lang.reflect.Modifier;

public final class Reflection
{
    public static boolean verifyMemberAccess(Class caller, Class declarer, Object ignored, int modifiers)
    {
        // NOTE we don't support @ikvm.lang.Internal access here
        return caller == declarer
            || (modifiers & Modifier.PUBLIC) != 0
            || ((modifiers & Modifier.PROTECTED) != 0 && declarer.isAssignableFrom(caller))
            || ((modifiers & Modifier.PRIVATE) == 0 && caller.getPackage() == declarer.getPackage());
    }

    public static Class getCallerClass(int skip)
    {
        return VMStackWalker.getClassContext()[skip - 1];
    }
}
