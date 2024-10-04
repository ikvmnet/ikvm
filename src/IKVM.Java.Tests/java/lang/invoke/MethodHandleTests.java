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

    interface AddItf {
        void apply(List st, int idx, Object v) throws Throwable;
    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeVirtual() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(String.class, String.class, String.class);
        MethodHandle mh = lookup.findVirtual(TestClass.class, "addString", mt);
        String s = (String)mh.invoke(new TestClass(), (Object)"a", (Object)"b");
        if (!s.equals("ab")) {
            throw new Exception(s);
        }
    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeStatic() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(String.class, String.class, String.class);
        MethodHandle mh = lookup.findStatic(TestClass.class, "addStringStatic", mt);
        String s = (String)mh.invoke((Object)"a", (Object)"b");
        if (!s.equals("ab")) {
            throw new Exception(s);
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeExactVirtual() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(String.class, String.class, String.class);
        MethodHandle mh = lookup.findVirtual(TestClass.class, "addString", mt);
        String s = (String)mh.invokeExact(new TestClass(), "a", "b");
        if (!s.equals("ab")) {
            throw new Exception(s);
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeExactStatic() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(String.class, String.class, String.class);
        MethodHandle mh = lookup.findStatic(TestClass.class, "addStringStatic", mt);
        String s = (String)mh.invokeExact("a", "b");
        if (!s.equals("ab")) {
            throw new Exception(s);
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeStaticMethodThatReturnsVoid() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType pmt = MethodType.methodType(void.class, int.class, Object.class);
        MethodHandle pms = lookup.findVirtual(List.class, "add", pmt);
        List<String> list = new ArrayList<>();
        pms.invoke(list, 0, "Hi");
        AddItf pf2 = pms::invoke;
        pf2.apply(list, 1, "there");
        AddItf pf3 = pms::invokeExact;
        pf3.apply(list, 2, "you");

        if (!list.get(0).equals("Hi")) {
            throw new Exception(list.get(0));
        };
        if (!list.get(1).equals("there")) {
            throw new Exception(list.get(1));
        };
        if (!list.get(2).equals("you")) {
            throw new Exception(list.get(2));
        };
    }

}