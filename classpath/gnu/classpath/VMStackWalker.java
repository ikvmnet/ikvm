/*
  Copyright (C) 2005, 2006 Jeroen Frijters

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
package gnu.classpath;

import cli.System.Diagnostics.StackFrame;
import cli.System.Diagnostics.StackTrace;
import cli.System.Reflection.MethodBase;

public final class VMStackWalker
{
    private static final int SKIP_FRAMES = 2;
    private static final cli.System.Reflection.Assembly mscorlib = cli.System.Type.GetType("System.Object").get_Assembly();
    private static final cli.System.Type methodType = cli.System.Type.GetType("java.lang.reflect.Method");
    private static final cli.System.Type constructorType = cli.System.Type.GetType("java.lang.reflect.Constructor");
    private static final cli.System.Type jniEnvType = getJNIEnvType();

    public static ClassLoader firstNonNullClassLoader()
    {
        StackTrace stack = new StackTrace(1);
        for(int i = 0; i < stack.get_FrameCount(); i++)
        {
            StackFrame frame = stack.GetFrame(i);
            // TODO handle reflection scenarios
            MethodBase method = frame.GetMethod();
            if(!isHideFromJava(method))
            {
                cli.System.Type type = method.get_DeclaringType();
                if(type != null)
                {
                    ClassLoader loader = (ClassLoader)getClassLoaderFromType(type);
                    if(loader != null)
                    {
                        return loader;
                    }
                }
            }
        }
        return null;
    }

    public static Class[] getClassContext()
    {
        StackTrace stack = new StackTrace(1);
        java.util.ArrayList list = new java.util.ArrayList();
        for(int i = 0; i < stack.get_FrameCount(); i++)
        {
            StackFrame frame = stack.GetFrame(i);
            // TODO handle reflection scenarios
            MethodBase method = frame.GetMethod();
            if(!isHideFromJava(method))
            {
                cli.System.Type type = method.get_DeclaringType();
                if(type != null)
                {
                    list.add(getClassFromType(type));
                }
            }
        }
        Class[] classes = new Class[list.size()];
        list.toArray(classes);
        return classes;
    }

    public static Class getCallingClass()
    {
        int skip = SKIP_FRAMES;
        StackFrame frame = new StackFrame(skip++);
        MethodBase method = frame.GetMethod();
        cli.System.Type type = method.get_DeclaringType();
        if(isReflectionCaller(type))
        {
            type = getRealCaller(new StackTrace(skip));
        }
        if(type != null)
        {
            return (Class)getClassFromType(type);
        }
        return null;
    }

    public static ClassLoader getCallingClassLoader()
    {
        int skip = SKIP_FRAMES;
        StackFrame frame = new StackFrame(skip++);
        MethodBase method = frame.GetMethod();
        cli.System.Type type = method.get_DeclaringType();
        if(isReflectionCaller(type))
        {
            type = getRealCaller(new StackTrace(skip));
        }
        if(type != null)
        {
            return (ClassLoader)getClassLoaderFromType(type);
        }
        return null;
    }

    private static boolean isReflectionCaller(cli.System.Type type)
    {
        if(type != null)
        {
            cli.System.Reflection.Assembly asm = type.get_Assembly();
            // if we're being called by mscorlib, that means that reflection was used
            return asm == mscorlib;
        }
        return false;
    }

    private static cli.System.Type getRealCaller(StackTrace stack)
    {
        for(int i = 0; i < stack.get_FrameCount(); i++)
        {
            cli.System.Type type = stack.GetFrame(i).GetMethod().get_DeclaringType();
            if(type == methodType || type == constructorType || type == jniEnvType)
            {
                while(++i < stack.get_FrameCount() && (type == methodType || type == constructorType || type == jniEnvType))
                {
                   type = stack.GetFrame(i).GetMethod().get_DeclaringType();
                }
                if(type == null || i >= stack.get_FrameCount())
                {
                    return null;
                }
                while(isHideFromJava(stack.GetFrame(i).GetMethod()))
                {
                    type = stack.GetFrame(++i).GetMethod().get_DeclaringType();
                }
                // If the reflection method was invoked by reflection, continue going up the stack.
                if(isReflectionCaller(type))
                {
                    continue;
                }
                return type;
            }
        }
        return null;
    }

    private static native Object getClassFromType(cli.System.Type type);
    private static native Object getClassLoaderFromType(cli.System.Type type);
    private static native cli.System.Type getJNIEnvType();
    private static native boolean isHideFromJava(MethodBase mb);
}
