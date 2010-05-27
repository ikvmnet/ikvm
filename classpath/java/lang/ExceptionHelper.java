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
import cli.System.Runtime.Serialization.OnSerializingAttribute;
import cli.System.Runtime.Serialization.SerializationInfo;
import cli.System.Runtime.Serialization.StreamingContext;

final class ExceptionHelper
{
    private static final boolean cleanStackTrace = SafeGetEnvironmentVariable("IKVM_DISABLE_STACKTRACE_CLEANING") == null;
    private static final cli.System.Type System_Reflection_MethodBase = ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Reflection.MethodBase.class);
    private static final cli.System.Type System_Exception = ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Exception.class);

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
    static native cli.System.Type getTypeFromObject(Object o);

    private static native String SafeGetEnvironmentVariable(String name);
}
