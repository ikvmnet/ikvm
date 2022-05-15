/*
  Copyright (C) 2008-2015 Jeroen Frijters

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

import cli.System.Type;
import cli.System.Diagnostics.StackFrame;
import cli.System.Reflection.Assembly;

public abstract class CallerID
{
    private Class clazz;
    private ClassLoader classLoader;

    private CallerID(Class clazz, ClassLoader classLoader)
    {
        this.clazz = clazz;
        this.classLoader = classLoader;
    }

    protected CallerID() { }

    @ikvm.lang.Internal
    public final Class getCallerClass()
    {
	if (clazz == null)
	{
	    clazz = GetClass();
	}
        return clazz;
    }

    @ikvm.lang.Internal
    public final ClassLoader getCallerClassLoader()
    {
	ClassLoader cl = classLoader;
	if (cl == null)
	{
	    cl = classLoader = GetClassLoader();
	    if (cl == null)
	    {
		cl = classLoader = ClassLoader.DUMMY;
	    }
	}
	return cl == ClassLoader.DUMMY ? null : cl;
    }

    @ikvm.lang.Internal
    public static CallerID create(cli.System.Diagnostics.StackFrame frame)
    {
	return create(frame.GetMethod());
    }

    @ikvm.lang.Internal
    public static CallerID create(final cli.System.Reflection.MethodBase method)
    {
	return new CallerID() {
	    Class GetClass() {
		if (method == null) {
		    // this happens if a native thread attaches and calls back into Java
		    return null;
		}
		Type type = method.get_DeclaringType();
		if (type == null) {
		    // TODO we probably should return a class corresponding to <Module>
		    throw new InternalError();
		}
		return ikvm.runtime.Util.getClassFromTypeHandle(type.get_TypeHandle());
	    }
	    ClassLoader GetClassLoader() {
		if (method == null) {
		    // this happens if a native thread attaches and calls back into Java
		    return null;
		}
		Assembly asm = method.get_Module().get_Assembly();
		return GetAssemblyClassLoader(asm);
	    }
	};
    }

    // this is a shortcut for use inside the core class library, it removes the need to create a nested type for every caller
    @ikvm.lang.Internal
    public static CallerID create(final cli.System.RuntimeTypeHandle typeHandle)
    {
	return new CallerID() {
	    Class GetClass() {
		return ikvm.runtime.Util.getClassFromTypeHandle(typeHandle);
	    }
	    ClassLoader GetClassLoader() {
	        // since this optimization is only available inside the core class library, we know that the class loader is null
		return null;
	    }
	};
    }

    // used by the runtime for EmitHostCallerID and DynamicCallerID
    static CallerID create(Class clazz, ClassLoader classLoader)
    {
        return new CallerID(clazz, classLoader) {
	    Class GetClass() {
		return null;
	    }
	    ClassLoader GetClassLoader() {
		return null;
	    }
        };
    }

    @ikvm.lang.Internal
    public static CallerID getCallerID()
    {
	// this is a compiler intrinsic, so there is no meaningful implementation here
	return null;
    }

    native Class GetClass();
    native ClassLoader GetClassLoader();
    static native ClassLoader GetAssemblyClassLoader(Assembly asm);
}
