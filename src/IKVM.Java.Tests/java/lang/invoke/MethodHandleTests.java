package ikvm.tests.java.java.lang.invoke;

import java.lang.*;
import java.lang.invoke.*;
import java.util.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class MethodHandleTests {

    static class TestClass {

        public String addString(String a, String b) {
            return a + b;
        }

        public static String addStringStatic(String a, String b) {
            return a + b;
        }

    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeExactVirtual() throws Throwable {
        MethodHandles.Lookup lookup = getLookup();
        MethodType mt = getMethodType();
        MethodHandle mh = getMethodHandle(lookup, mt);
        String s = invokeIt(mh);
        if (!s.equals("ab")) {
            throw new Exception(s);
        }
    }
    
    @cli.System.Runtime.CompilerServices.MethodImplAttribute.Annotation(value = cli.System.Runtime.CompilerServices.MethodImplOptions.__Enum.NoInlining)
    MethodHandles.Lookup getLookup() throws Throwable
    {
        return MethodHandles.lookup();
    }
    
    @cli.System.Runtime.CompilerServices.MethodImplAttribute.Annotation(value = cli.System.Runtime.CompilerServices.MethodImplOptions.__Enum.NoInlining)
    MethodType getMethodType() throws Throwable
    {
        return MethodType.methodType(String.class, String.class, String.class);
    }
    
    @cli.System.Runtime.CompilerServices.MethodImplAttribute.Annotation(value = cli.System.Runtime.CompilerServices.MethodImplOptions.__Enum.NoInlining)
    MethodHandle getMethodHandle(MethodHandles.Lookup lookup, MethodType mt) throws Throwable
    {
        return lookup.findVirtual(TestClass.class, "addString", mt);
    }
    
    @cli.System.Runtime.CompilerServices.MethodImplAttribute.Annotation(value = cli.System.Runtime.CompilerServices.MethodImplOptions.__Enum.NoInlining)
    String invokeIt(MethodHandle mh) throws Throwable
    {
        return (String)mh.invokeExact(new TestClass(), "a", "b");
    }

}
