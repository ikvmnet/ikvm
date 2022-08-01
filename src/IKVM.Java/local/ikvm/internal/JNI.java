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

@ikvm.lang.Internal
public final class JNI
{
    public static final Object NULL = null;
    public static final boolean TRUE = true;
    public static final boolean FALSE = false;
    public static final boolean JNI_TRUE = true;
    public static final boolean JNI_FALSE = false;

    public static final String JNU_JAVAIOPKG = "java.io.";
    public static final String JNU_JAVANETPKG = "java.net.";

    public static long JVM_CurrentTimeMillis(JNIEnv env, int ignored)
    {
        return System.currentTimeMillis();
    }

    public static void JNU_ThrowNullPointerException(JNIEnv env, String message)
    {
        env.Throw(new NullPointerException(message));
    }

    public static void JNU_ThrowByName(JNIEnv env, String exceptionClass, String message)
    {
        if (exceptionClass.equals("java.net.SocketException"))
        {
            env.Throw(new java.net.SocketException(message));
        }
        else if (exceptionClass.equals("java.net.SocketTimeoutException"))
        {
            env.Throw(new java.net.SocketTimeoutException(message));
        }
        else if (exceptionClass.equals("java.net.PortUnreachableException"))
        {
            env.Throw(new java.net.PortUnreachableException(message));
        }
        else if (exceptionClass.equals("java.io.InterruptedIOException"))
        {
            env.Throw(new java.io.InterruptedIOException(message));
        }
        else
        {
            try
            {
                env.ThrowNew(Class.forName(exceptionClass), message);
            }
            catch (ClassNotFoundException x)
            {
                env.Throw(x);
            }
        }
    }

    public static final class JNIEnv
    {
        private Throwable pendingException;

        public void Throw(Throwable t)
        {
            pendingException = t;
        }

        public void ThrowNew(Class c, String msg)
        {
            try
            {
                pendingException = (Throwable)c.getConstructor(String.class).newInstance(msg);
            }
            catch (Exception x)
            {
                pendingException = x;
            }
        }

        public Throwable ExceptionOccurred()
        {
            return pendingException;
        }

        public void ThrowPendingException()
        {
            if (pendingException != null)
            {
                sun.misc.Unsafe.getUnsafe().throwException(pendingException);
            }
        }
    }
}
