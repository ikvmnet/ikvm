/*
  Copyright (C) 2009-2011 Jeroen Frijters

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

package ikvm.runtime;

import cli.System.MulticastDelegate;
import java.lang.reflect.InvocationHandler;
import java.security.PrivilegedAction;

public final class Delegates
{
    private Delegates() { }
    
    public static Runnable toRunnable(RunnableDelegate delegate)
    {
        return delegate;
    }
    
    public static PrivilegedAction toPrivilegedAction(PrivilegedActionDelegate delegate)
    {
        return delegate;
    }

    public static InvocationHandler toInvocationHandler(InvocationHandlerDelegate delegate)
    {
        return delegate;
    }
    
    public static final class RunnableDelegate extends MulticastDelegate implements Runnable
    {
        public RunnableDelegate(Method m) { }
        public native void Invoke();
        public interface Method
        {
            void Invoke();
        }
        public void run()
        {
            Invoke();
        }
    }

    public static final class PrivilegedActionDelegate extends MulticastDelegate implements PrivilegedAction
    {
        public PrivilegedActionDelegate(Method m) { }
        public native Object Invoke();
        public interface Method
        {
            Object Invoke();
        }
        public Object run()
        {
            return Invoke();
        }
    }

    public static final class InvocationHandlerDelegate extends MulticastDelegate implements InvocationHandler
    {
        public InvocationHandlerDelegate(Method m) { }
        public native Object Invoke(Object proxy, java.lang.reflect.Method method, Object[] args) throws Throwable;
        public interface Method
        {
            Object Invoke(Object proxy, java.lang.reflect.Method method, Object[] args) throws Throwable;
        }
        public Object invoke(Object proxy, java.lang.reflect.Method method, Object[] args) throws Throwable
        {
            return Invoke(proxy, method, args);
        }
    }
}
