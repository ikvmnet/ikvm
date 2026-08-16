/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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

import cli.System.Delegate;
import cli.System.Type;
import cli.System.RuntimeTypeHandle;
import java.lang.invoke.MethodHandle;
import java.lang.invoke.MethodHandles;
import java.lang.reflect.Constructor;
import java.lang.reflect.Executable;
import java.lang.reflect.Method;
import sun.misc.Unsafe;

public final class Util
{

    private Util()
    {

    }

    public static native Class getClassFromObject(Object o);

    public static native Class getClassFromTypeHandle(RuntimeTypeHandle handle);

    // this is used to create an array of a remapped type (e.g. getClassFromTypeHandle(typeof(object), 1) returns cli.System.Object[])
    public static native Class getClassFromTypeHandle(RuntimeTypeHandle handle, int rank);

    public static native Class getFriendlyClassFromType(Type type);

    public static native Type getInstanceTypeFromClass(Class classObject);

    public static native Type getRuntimeTypeFromClass(Class classObject);

    /**
     * Creates a delegate of the given type that invokes the given method handle. The handle is adapted to the
     * signature of the delegate's Invoke method, which performs any conversions required; a handle that cannot be
     * adapted results in a {@link java.lang.invoke.WrongMethodTypeException}.
     */
    public static native Delegate getDelegateFromMethodHandle(Type delegateType, MethodHandle methodHandle);

    /**
     * Creates a delegate of the given type that invokes the given method or constructor. Access is checked as it is
     * for {@link MethodHandles.Lookup#unreflect}; call {@code setAccessible(true)} on the member first in order to
     * create a delegate for an otherwise inaccessible one.
     */
    public static Delegate getDelegateFromMethod(Type delegateType, Executable executable) throws IllegalAccessException
    {
        if (executable == null)
            throw new NullPointerException("executable");

        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodHandle methodHandle = executable instanceof Constructor
            ? lookup.unreflectConstructor((Constructor)executable)
            : lookup.unreflect((Method)executable);

        return getDelegateFromMethodHandle(delegateType, methodHandle);
    }

    public static native Throwable mapException(Throwable x);

    public static native Throwable unmapException(Throwable x);

    public static void throwException(cli.System.Exception x)
    {
        Unsafe.getUnsafe().throwException(x);
    }

}
