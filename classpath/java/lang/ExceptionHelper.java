/*
  Copyright (C) 2003, 2004, 2005, 2006, 2007, 2009 Jeroen Frijters

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
package java.lang;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.ObjectStreamField;
import java.util.ArrayList;
import cli.System.Runtime.Serialization.IObjectReference;
import cli.System.Runtime.Serialization.ISerializable;
import cli.System.Runtime.Serialization.OnSerializingAttribute;
import cli.System.Runtime.Serialization.SerializationInfo;
import cli.System.Runtime.Serialization.StreamingContext;

@ikvm.lang.Internal
public final class ExceptionHelper
{
    private static final Key EXCEPTION_DATA_KEY = new Key();
    private static final ikvm.internal.WeakIdentityMap exceptions = new ikvm.internal.WeakIdentityMap();
    private static final boolean cleanStackTrace = SafeGetEnvironmentVariable("IKVM_DISABLE_STACKTRACE_CLEANING") == null;
    private static final cli.System.Type System_Reflection_MethodBase = ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Reflection.MethodBase.class);
    private static final cli.System.Type System_Exception = ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Exception.class);
    // we use Activator.CreateInstance to prevent the exception from being added to the exceptions map
    private static final Throwable NOT_REMAPPED = (Throwable)cli.System.Activator.CreateInstance(System_Exception);
    private static final java.util.Hashtable failedTypes = new java.util.Hashtable();

    static
    {
        // make sure the exceptions map continues to work during AppDomain finalization
        cli.System.GC.SuppressFinalize(exceptions);
    }
    
    @cli.System.SerializableAttribute.Annotation
    private static final class Key implements ISerializable
    {
        @cli.System.SerializableAttribute.Annotation
        private static final class Helper implements IObjectReference
        {
            @cli.System.Security.SecurityCriticalAttribute.Annotation
            public Object GetRealObject(StreamingContext context)
            {
                return EXCEPTION_DATA_KEY;
            }
        }

        @cli.System.Security.SecurityCriticalAttribute.Annotation
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(ikvm.runtime.Util.getInstanceTypeFromClass(Helper.class));
        }
    }

    @cli.System.SerializableAttribute.Annotation
    private static final class ExceptionInfoHelper
    {
	private transient cli.System.Diagnostics.StackTrace tracePart1;
	private transient cli.System.Diagnostics.StackTrace tracePart2;
	private StackTraceElement[] stackTrace;

        ExceptionInfoHelper(StackTraceElement[] stackTrace)
        {
            this.stackTrace = stackTrace;
        }

        ExceptionInfoHelper(cli.System.Diagnostics.StackTrace tracePart1, cli.System.Diagnostics.StackTrace tracePart2)
        {
            this.tracePart1 = tracePart1;
            this.tracePart2 = tracePart2;
        }

	ExceptionInfoHelper(Throwable x, boolean captureAdditionalStackTrace)
	{
	    tracePart1 = new cli.System.Diagnostics.StackTrace(x, true);
            if(captureAdditionalStackTrace)
            {
	        tracePart2 = new cli.System.Diagnostics.StackTrace(true);
            }
	}

        @OnSerializingAttribute.Annotation
        private void OnSerializing(StreamingContext context)
        {
            // make sure the stack trace is computed before serializing
            get_StackTrace(null);
        }

	private static boolean IsPrivateScope(cli.System.Reflection.MethodBase mb)
	{
	    // HACK shouldn't there be a better way to determine whether a method is privatescope?
	    return !mb.get_IsPrivate() && !mb.get_IsFamily() && !mb.get_IsFamilyAndAssembly() &&
		    !mb.get_IsFamilyOrAssembly() && !mb.get_IsPublic() && !mb.get_IsAssembly();
	}

	private static String getDeclaringTypeNameSafe(cli.System.Reflection.MethodBase mb)
	{
	    cli.System.Type type = mb.get_DeclaringType();
	    return type == null ? "" : type.get_FullName();
	}

	StackTraceElement[] get_StackTrace(Throwable t)
	{
	    synchronized(this)
	    {
		if(stackTrace == null)
		{
		    cli.System.Collections.ArrayList stackTrace = new cli.System.Collections.ArrayList();
                    if(tracePart1 != null)
                    {
		        int skip1 = 0;
		        if(cleanStackTrace && t instanceof NullPointerException && tracePart1.get_FrameCount() > 0)
		        {
			    // HACK if a NullPointerException originated inside an instancehelper method,
			    // we assume that the reference the method was called on was really the one that was null,
			    // so we filter it.
			    if(tracePart1.GetFrame(0).GetMethod().get_Name().startsWith("instancehelper_") &&
			        !GetMethodName(tracePart1.GetFrame(0).GetMethod()).startsWith("instancehelper_"))
			    {
			        skip1 = 1;
			    }
		        }
		        Append(stackTrace, tracePart1, skip1);
                    }
		    if(tracePart2 != null)
		    {
			int skip = 0;
			if(cleanStackTrace)
			{
			    while(tracePart2.get_FrameCount() > skip && 
				getDeclaringTypeNameSafe(tracePart2.GetFrame(skip).GetMethod()).startsWith("java.lang.ExceptionHelper"))
			    {
				skip++;
			    }
                            if(tracePart2.get_FrameCount() > skip)
                            {
                                cli.System.Reflection.MethodBase mb = tracePart2.GetFrame(skip).GetMethod();
				// here we have to check for both fillInStackTrace and .ctor, because on x64 the fillInStackTrace method
				// disappears from the stack trace due to the tail call optimization.
                                if(getDeclaringTypeNameSafe(mb).equals("java.lang.Throwable") &&
                                    (mb.get_Name().endsWith("fillInStackTrace") || mb.get_Name().equals(".ctor")))
                                {
                                    while(tracePart2.get_FrameCount() > skip)
                                    {
                                        mb = tracePart2.GetFrame(skip).GetMethod();
                                        if(!getDeclaringTypeNameSafe(mb).equals("java.lang.Throwable")
                                            || !mb.get_Name().endsWith("fillInStackTrace"))
                                        {
                                            break;
                                        }
                                        skip++;
                                    }
                                    cli.System.Type exceptionType = getTypeFromObject(t);
                                    while(tracePart2.get_FrameCount() > skip)
                                    {
                                        mb = tracePart2.GetFrame(skip).GetMethod();
                                        if(!mb.get_Name().equals(".ctor")
                                            || !mb.get_DeclaringType().IsAssignableFrom(exceptionType))
                                        {
                                            break;
                                        }
                                        skip++;
                                    }
                                }
                            }
                            // skip java.lang.Throwable.__<map>
                            while(tracePart2.get_FrameCount() > skip && IsHideFromJava(tracePart2.GetFrame(skip).GetMethod()))
                            {
                                skip++;
                            }
			    if(tracePart1 != null &&
                                tracePart1.get_FrameCount() > 0 &&
				tracePart2.get_FrameCount() > skip &&
				tracePart1.GetFrame(tracePart1.get_FrameCount() - 1).GetMethod() == tracePart2.GetFrame(skip).GetMethod())
			    {
				skip++;
			    }
			}
			Append(stackTrace, tracePart2, skip);
		    }
		    if(cleanStackTrace && stackTrace.get_Count() > 0)
		    {
			StackTraceElement elem = (StackTraceElement)stackTrace.get_Item(stackTrace.get_Count() - 1);
			if(elem.getClassName().equals("java.lang.reflect.Method"))
			{
			    stackTrace.RemoveAt(stackTrace.get_Count() - 1);
			}
		    }
		    tracePart1 = null;
		    tracePart2 = null;
	            StackTraceElement[] array = new StackTraceElement[stackTrace.get_Count()];
	            stackTrace.CopyTo((cli.System.Array)(Object)array);
	            this.stackTrace = array;
		}
	    }
	    return stackTrace.clone();
	}
	
	static void Append(cli.System.Collections.ArrayList stackTrace, cli.System.Diagnostics.StackTrace st, int skip)
	{
	    for(int i = skip; i < st.get_FrameCount(); i++)
	    {
		cli.System.Diagnostics.StackFrame frame = st.GetFrame(i);
		cli.System.Reflection.MethodBase m = frame.GetMethod();
		// TODO I may need more safety checks like these
		if(m == null || m.get_DeclaringType() == null)
		{
		    continue;
		}
                String methodName = GetMethodName(m);
                String className = getClassNameFromType(m.get_DeclaringType());
                if(cleanStackTrace &&
		    (System_Reflection_MethodBase.IsAssignableFrom(m.get_DeclaringType())
		    || className.startsWith("java.lang.ExceptionHelper")
		    || className.equals("cli.System.RuntimeMethodHandle")
                    || className.equals("java.lang.LibraryVMInterfaceImpl")
                    || (className.equals("java.lang.Throwable") && m.get_Name().equals("instancehelper_fillInStackTrace"))
                    || methodName.startsWith("__<")
		    || IsHideFromJava(m)
		    || IsPrivateScope(m))) // NOTE we assume that privatescope methods are always stubs that we should exclude
		{
		    continue;
		}
		int lineNumber = frame.GetFileLineNumber();
		if(lineNumber == 0)
		{
		    lineNumber = GetLineNumber(frame);
		}
		String fileName = frame.GetFileName();
		if(fileName != null)
		{
		    try
		    {
			fileName = new cli.System.IO.FileInfo(fileName).get_Name();
		    }
		    catch(Throwable x)
		    {
			// Mono returns "<unknown>" for frame.GetFileName() and the FileInfo constructor
			// doesn't like that
			fileName = null;
		    }
		}
		if(fileName == null)
		{
		    fileName = GetFileName(frame);
		}
		stackTrace.Add(new StackTraceElement(className, methodName, fileName, IsNative(m) ? -2 : lineNumber));
	    }
	}
    }

    // NOTE these should all be private, but they're used from the inner class and we don't want the accessor methods
    static native boolean IsHideFromJava(cli.System.Reflection.MethodBase mb);
    static native boolean IsNative(cli.System.Reflection.MethodBase mb);
    static native String GetMethodName(cli.System.Reflection.MethodBase mb);
    static native String getClassNameFromType(cli.System.Type type);
    static native int GetLineNumber(cli.System.Diagnostics.StackFrame frame);
    static native String GetFileName(cli.System.Diagnostics.StackFrame frame);
    static native Throwable MapExceptionImpl(Throwable t);
    static native cli.System.Type getTypeFromObject(Object o);

    private static native String SafeGetEnvironmentVariable(String name);
    
    // native methods implemented in map.xml
    private static native cli.System.Exception getOriginalAndClear(Throwable t);
    private static native void setOriginal(Throwable t, cli.System.Exception org);
    private static native boolean needStackTraceInfo(Throwable t);
    private static native void setStackTraceInfo(Throwable t, cli.System.Diagnostics.StackTrace part1, cli.System.Diagnostics.StackTrace part2);

    static void printStackTrace(Throwable x)
    {
	x.printStackTrace(System.err);
    }

    static void printStackTrace(Throwable x, java.io.PrintStream printStream)
    {
        synchronized (printStream)
        {
	    for (String line : buildStackTrace(x))
	    {
	        printStream.println(line);
	    }
	}
    }

    static void printStackTrace(Throwable x, java.io.PrintWriter printWriter)
    {
        synchronized (printWriter)
        {
	    for (String line : buildStackTrace(x))
	    {
	        printWriter.println(line);
	    }
	}
    }

    private static ArrayList<String> buildStackTrace(Throwable x)
    {
	ArrayList<String> list = new ArrayList<String>();
	list.add(x.toString());
	StackTraceElement[] stack = x.getStackTrace();
	for(int i = 0; i < stack.length; i++)
	{
	    list.add("\tat " + stack[i]);
	}
	Throwable cause = x.getCause();
	while(cause != null)
	{
	    list.add("Caused by: " + cause);

	    // Cause stacktrace
	    StackTraceElement[] parentStack = stack;
	    stack = cause.getStackTrace();
	    boolean equal = false; // Is rest of stack equal to parent frame?
	    for(int i = 0; i < stack.length && !equal; i++)
	    {
		// Check if we already printed the rest of the stack
		// since it was the tail of the parent stack
		int remaining = stack.length - i;
		int element = i;
		int parentElement = parentStack.length - remaining;
		equal = parentElement >= 0 && parentElement < parentStack.length;
		while(equal && element < stack.length)
		{
		    if(stack[element].equals(parentStack[parentElement]))
		    {
			element++;
			parentElement++;
		    }
		    else
		    {
			equal = false;
		    }
		}
		// Print stacktrace element or indicate the rest is equal 
		if(!equal)
		{
		    list.add("\tat " + stack[i]);
		}
		else
		{
		    list.add("\t... " + remaining + " more");
		    break; // from stack printing for loop
		}
	    }
	    cause = cause.getCause();
	}
	return list;
    }
    
    static void checkInitCause(Throwable _this, Throwable _this_cause, Throwable cause)
    {
        if (_this_cause != _this)
        {
            throw new IllegalStateException("Can't overwrite cause");
        }
        if (cause == _this)
        {
            throw new IllegalArgumentException("Self-causation not permitted");
        }
    }

    static void FixateException(cli.System.Exception x)
    {
        exceptions.put(x, NOT_REMAPPED);
    }
    
    static Throwable getCause(Throwable _this, Throwable cause)
    {
        return cause == _this ? null : cause;
    }
    
    static StackTraceElement[] computeStackTrace(Throwable t, cli.System.Diagnostics.StackTrace part1, cli.System.Diagnostics.StackTrace part2)
    {
        ExceptionInfoHelper eih = new ExceptionInfoHelper(part1, part2);
        return eih.get_StackTrace(t);
    }

    static StackTraceElement[] getStackTrace(cli.System.Exception x)
    {
        synchronized (x)
        {
            ExceptionInfoHelper eih = null;
            cli.System.Collections.IDictionary data = x.get_Data();
            if (data != null && !data.get_IsReadOnly())
            {
                synchronized (data)
                {
                    eih = (ExceptionInfoHelper)data.get_Item(EXCEPTION_DATA_KEY);
                }
            }
	    if (eih == null)
	    {
	        return new StackTraceElement[0];
	    }
	    return eih.get_StackTrace(x);
	}
    }
    
    static StackTraceElement[] checkStackTrace(StackTraceElement[] original)
    {
        StackTraceElement[] copy = original.clone();
        for (int i = 0; i < copy.length; i++)
        {
            copy[i].getClass(); // efficient null check
        }
        return copy;
    }

    static void setStackTrace(cli.System.Exception x, StackTraceElement[] stackTrace)
    {
        ExceptionInfoHelper eih = new ExceptionInfoHelper(checkStackTrace(stackTrace));
        cli.System.Collections.IDictionary data = x.get_Data();
        if (data != null && !data.get_IsReadOnly())
        {
            synchronized (data)
            {
                data.set_Item(EXCEPTION_DATA_KEY, eih);
            }
        }
    }

    static String FilterMessage(String message)
    {
	if(message == null)
	{
	    message = "";
	}
	return message;
    }

    static String GetMessageFromCause(Throwable cause)
    {
	if(cause == null)
	{
	    return "";
	}
	return cause.toString();
    }

    static String getLocalizedMessage(Throwable x)
    {
	return x.getMessage();
    }

    static void fillInStackTrace(cli.System.Exception x)
    {
        synchronized (x)
        {
            ExceptionInfoHelper eih = new ExceptionInfoHelper(null, new cli.System.Diagnostics.StackTrace(true));
            cli.System.Collections.IDictionary data = x.get_Data();
            if (data != null && !data.get_IsReadOnly())
            {
                synchronized (data)
                {
                    data.set_Item(EXCEPTION_DATA_KEY, eih);
                }
            }
        }
    }

    static String toString(Throwable x)
    {
	String message = x.getLocalizedMessage();
	if(message == null)
	{
	    return x.getClass().getName();
	}
	return x.getClass().getName() + ": " + message;
    }

    // also used by ikvm.extensions.ExtensionMethods.printStackTrace()
    public static Throwable UnmapException(Throwable t)
    {
        if(!(t instanceof cli.System.Exception))
        {
            cli.System.Exception org = getOriginalAndClear(t);
            if(org != null)
            {
	        exceptions.put(org, t);
                t = org;
            }
        }
        return t;
    }

    // used by ikvm.runtime.Util
    public static Throwable MapExceptionFast(Throwable t, boolean remap)
    {
        return MapException(t, null, remap);
    }

    static Throwable MapException(Throwable t, cli.System.Type handler, boolean remap)
    {
        Throwable org = t;
        boolean nonJavaException = t instanceof cli.System.Exception;
        if (nonJavaException && remap)
        {
            if (t instanceof cli.System.TypeInitializationException)
            {
                return MapTypeInitializeException((cli.System.TypeInitializationException)t, handler);
            }
            Object obj = exceptions.get(t);
            Throwable remapped = (Throwable)obj;
            if (remapped == null)
            {
                remapped = MapExceptionImpl(t);
                if (remapped == t)
                {
                    exceptions.put(t, NOT_REMAPPED);
                }
                else
                {
                    exceptions.put(t, remapped);
                    t = remapped;
                }
            }
            else if (remapped != NOT_REMAPPED)
            {
                t = remapped;
            }
        }

        if (handler == null || isInstanceOfType(t, handler, remap))
        {
            if (t instanceof cli.System.Exception)
            {
                cli.System.Exception x = (cli.System.Exception)t;
                cli.System.Collections.IDictionary data = x.get_Data();
                if (data != null && !data.get_IsReadOnly())
                {
                    synchronized (data)
                    {
                        if (!data.Contains(EXCEPTION_DATA_KEY))
                        {
                            data.Add(EXCEPTION_DATA_KEY, new ExceptionInfoHelper(t, true));
                        }
                    }
                }
            }
            else
            {
                if (needStackTraceInfo(t))
                {
	            cli.System.Diagnostics.StackTrace tracePart1 = new cli.System.Diagnostics.StackTrace(org, true);
                    cli.System.Diagnostics.StackTrace tracePart2 = new cli.System.Diagnostics.StackTrace(true);
                    setStackTraceInfo(t, tracePart1, tracePart2);
                }
            }
            
            if (nonJavaException && !remap)
            {
                exceptions.put(t, NOT_REMAPPED);
            }
            
            if (t != org)
            {
                setOriginal(t, (cli.System.Exception)org);
	        exceptions.remove(org);
            }
            return t;
        }
        return null;
    }

    private static boolean isInstanceOfType(Throwable t, cli.System.Type type, boolean remap)
    {
        if(!remap && type == System_Exception)
        {
            return t instanceof cli.System.Exception;
        }
        return type.IsInstanceOfType(t);
    }

    static Throwable MapTypeInitializeException(cli.System.TypeInitializationException t, cli.System.Type handler)
    {
        boolean wrapped = false;
        Throwable r = MapExceptionFast(t.get_InnerException(), true);
        if(!(r instanceof Error))
        {
            r = new ExceptionInInitializerError(r);
            wrapped = true;
        }
        String type = t.get_TypeName();
        if(failedTypes.containsKey(type))
        {
            r = new NoClassDefFoundError(type).initCause(r);
            wrapped = true;
        }
        if(handler != null && !handler.IsInstanceOfType(r))
        {
            return null;
        }
        failedTypes.put(type, type);
        if(wrapped)
        {
            // transplant the stack trace
            r.setStackTrace(new ExceptionInfoHelper(t, true).get_StackTrace(t));
        }
        return r;
    }

    // helper for use by java.lang.management.VMThreadInfo
    public static StackTraceElement[] getStackTrace(cli.System.Diagnostics.StackTrace st, int maxDepth)
    {
        cli.System.Collections.ArrayList stackTrace = new cli.System.Collections.ArrayList();
        ExceptionInfoHelper.Append(stackTrace, st, 0);
        StackTraceElement[] ste = new StackTraceElement[Math.min(maxDepth, stackTrace.get_Count())];
        stackTrace.CopyTo(0, (cli.System.Array)(Object)ste, 0, ste.length);
        return ste;
    }
}
