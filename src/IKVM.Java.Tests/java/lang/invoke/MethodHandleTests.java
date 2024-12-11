package ikvm.tests.java.java.lang.invoke;

import java.lang.*;
import java.lang.invoke.*;
import java.util.*;
import java.util.function.*;

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

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeStaticMethodReference() throws Throwable {
        Supplier<String> supplier = System::lineSeparator;
        final String lineSeparator = supplier.get();
        if (!lineSeparator.equals(System.lineSeparator())) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeCallerSensitiveStaticMethodReference() throws Throwable {
        final Function<String, java.util.logging.Logger> function = java.util.logging.Logger::getLogger;
        final java.util.logging.Logger value = function.apply("TEST");
        if (value != java.util.logging.Logger.getLogger("TEST")) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canFindStaticCallerSensitive() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(java.util.logging.Logger.class, String.class);
        MethodHandle mh = lookup.findStatic(java.util.logging.Logger.class, "getLogger", mt);
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeExactStaticCallerSensitive() throws Throwable {
        MethodHandles.Lookup lookup = MethodHandles.lookup();
        MethodType mt = MethodType.methodType(java.util.logging.Logger.class, String.class);
        MethodHandle mh = lookup.findStatic(java.util.logging.Logger.class, "getLogger", mt);
        java.util.logging.Logger s = (java.util.logging.Logger)mh.invokeExact("TEST");
        if (s.getName() != "TEST") {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeMethodReference() throws Throwable {
        Integer intObject = new Integer(1);
        final Supplier<Integer> supplier = intObject::intValue;
        final Integer value = supplier.get();
        if (!value.equals(intObject)) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeCallerSensitiveMethodReference() throws Throwable {
        Class<?> clazz = MethodHandleTests.class;
        final Supplier<ClassLoader> supplier = clazz::getClassLoader;
        final ClassLoader value = supplier.get();
        if (value != clazz.getClassLoader()) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeMethodReferenceCapturedInAnonymousClass() throws Throwable {
        Integer intObject = new Integer(1);

        final Supplier<Integer> supplier = new Supplier() {
            @Override
            public Integer get() {
                final Supplier<Integer> supplier = intObject::intValue;
                final Integer value = supplier.get();
                return value;
            }
        };

        final Integer value = supplier.get();
        if (!value.equals(intObject)) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeCallerSensitiveMethodReferenceCapturedInAnonymousClass() throws Throwable {
        Class<?> clazz = MethodHandleTests.class;

        final Supplier<ClassLoader> supplier = new Supplier() {
            @Override
            public ClassLoader get() {
                final Supplier<ClassLoader> supplier = clazz::getClassLoader;
                final ClassLoader value = supplier.get();
                return value;
            }
        };
        
        final ClassLoader value = supplier.get();
        if (value != clazz.getClassLoader()) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeMethodReferenceCapturedInAnonymousClassField() throws Throwable {
        Integer intObject = new Integer(1);

        final Supplier<Integer> supplier = new Supplier() {
            final Supplier<Integer> supplier = intObject::intValue;

            @Override
            public Integer get() {
                final Integer intValue = supplier.get();
                return intValue;
            }
        };

        final Integer intValue = supplier.get();
        if (!intValue.equals(intObject)) {
            throw new Exception();
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeCallerSensitiveMethodReferenceCapturedInAnonymousClassField() throws Throwable {
        Class<?> clazz = MethodHandleTests.class;

        final Supplier<ClassLoader> supplier = new Supplier() {
            private final Supplier<ClassLoader> supplier = clazz::getClassLoader;

            @Override
            public ClassLoader get() {
                final ClassLoader value = supplier.get();
                return value;
            }
        };

        final ClassLoader value = supplier.get();
        if (value != clazz.getClassLoader()) {
            throw new Exception();
        }
    }

}
