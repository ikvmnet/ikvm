/*
  Copyright (C) 2010 Jeroen Frijters

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
package ikvm.internal;

// This is the base class for intrinsified ThreadLocals, it's main purpose
// is to avoid having to add a remove() method to every generated class,
// but it also gives us some maneuvering room should a future JDK version
// change ThreadLocal.
// Note that because this class is abstract, getClass() on an intrinsified
// ThreadLocal instance will return java.lang.ThreadLocal instead of this class.
// We don't use HideFromJava for this, because that would make the life of
// the runtime/ikvmc more difficult (because it needs a TypeWrapper for this class).

public abstract class IntrinsicThreadLocal extends ThreadLocal
{
    public final void remove()
    {
        set(null);
    }
}
