package ikvm.tests.java.sun.misc;

import java.lang.reflect.Field;

import sun.misc.Unsafe;

/**
 * Exercises the Unsafe object array operations from compiled Java code, so that the call sites are subject to
 * the Unsafe intrinsics. The arrays are passed through parameters of the static type Object[] (or another
 * supertype) so that the static type differs from the actual runtime type of the array.
 */
public final class UnsafeArrayTests {

    public interface TestInterface {

    }

    public static class TestClass implements TestInterface {

    }

    public static class TestSubClass extends TestClass {

    }

    static final Unsafe U;
    static final long BASE;
    static final long SCALE;

    static {
        try {
            Field f = Unsafe.class.getDeclaredField("theUnsafe");
            f.setAccessible(true);
            U = (Unsafe) f.get(null);
            BASE = U.arrayBaseOffset(Object[].class);
            SCALE = U.arrayIndexScale(Object[].class);
        } catch (Exception e) {
            throw new Error(e);
        }
    }

    static long offset(int i) {
        return BASE + i * SCALE;
    }

    static void check(boolean b, String message) {
        if (!b) {
            throw new AssertionError(message);
        }
    }

    static Object getObject(Object[] a, int i) {
        return U.getObject(a, offset(i));
    }

    static Object getObjectVolatile(Object[] a, int i) {
        return U.getObjectVolatile(a, offset(i));
    }

    static boolean compareAndSwapObject(Object[] a, int i, Object e, Object x) {
        return U.compareAndSwapObject(a, offset(i), e, x);
    }

    static Object getAndSetObject(Object[] a, int i, Object x) {
        return U.getAndSetObject(a, offset(i), x);
    }

    /**
     * Runs each operation against an Object[] typed reference to the given array, which must have at least two
     * elements, using the given values, which must be distinct instances valid for the array.
     */
    static void testObjectArrayReference(Object[] a, Object v1, Object v2, Object v3) {
        a[0] = v1;
        a[1] = v2;

        check(getObject(a, 0) == v1, "getObject [0]");
        check(getObject(a, 1) == v2, "getObject [1]");
        check(getObjectVolatile(a, 0) == v1, "getObjectVolatile [0]");
        check(getObjectVolatile(a, 1) == v2, "getObjectVolatile [1]");

        check(compareAndSwapObject(a, 0, v2, v3) == false, "compareAndSwapObject should fail");
        check(a[0] == v1, "compareAndSwapObject failure should not modify");
        check(compareAndSwapObject(a, 0, v1, v3), "compareAndSwapObject should succeed");
        check(a[0] == v3, "compareAndSwapObject success should modify");
        check(a[1] == v2, "compareAndSwapObject should not modify other elements");

        check(getAndSetObject(a, 1, v1) == v2, "getAndSetObject should return previous value");
        check(a[1] == v1, "getAndSetObject should modify");
        check(getAndSetObject(a, 1, null) == v1, "getAndSetObject should return previous value");
        check(a[1] == null, "getAndSetObject should set null");
        check(getObject(a, 1) == null, "getObject should read null");
    }

    public static void testObjectArray() {
        testObjectArrayReference(new Object[2], new Object(), new Object(), new Object());
    }

    public static void testStringArray() {
        testObjectArrayReference(new String[2], new String("a"), new String("b"), new String("c"));
    }

    public static void testIntegerArray() {
        testObjectArrayReference(new Integer[2], new Integer(1000), new Integer(2000), new Integer(3000));
    }

    public static void testClassArray() {
        testObjectArrayReference(new TestClass[2], new TestClass(), new TestSubClass(), new TestClass());
    }

    public static void testInterfaceArray() {
        testObjectArrayReference(new TestInterface[2], new TestClass(), new TestSubClass(), new TestClass());
    }

    public static void testSubClassArrayAsClassArray() {
        TestClass[] a = new TestSubClass[2];
        TestClass v1 = new TestSubClass();
        TestClass v2 = new TestSubClass();
        TestClass v3 = new TestSubClass();
        a[0] = v1;
        a[1] = v2;

        check(U.getObject(a, offset(0)) == v1, "getObject");
        check(U.getObjectVolatile(a, offset(1)) == v2, "getObjectVolatile");
        check(U.compareAndSwapObject(a, offset(0), v2, v3) == false, "compareAndSwapObject should fail");
        check(U.compareAndSwapObject(a, offset(0), v1, v3), "compareAndSwapObject should succeed");
        check(a[0] == v3, "compareAndSwapObject success should modify");
        check(U.getAndSetObject(a, offset(1), v1) == v2, "getAndSetObject should return previous value");
        check(a[1] == v1, "getAndSetObject should modify");
    }

    public static void testExactStringArray() {
        String[] a = new String[2];
        String v1 = new String("a");
        String v2 = new String("b");
        String v3 = new String("c");
        a[0] = v1;
        a[1] = v2;

        check(U.getObject(a, offset(0)) == v1, "getObject");
        check(U.getObjectVolatile(a, offset(1)) == v2, "getObjectVolatile");
        check(U.compareAndSwapObject(a, offset(0), v2, v3) == false, "compareAndSwapObject should fail");
        check(U.compareAndSwapObject(a, offset(0), v1, v3), "compareAndSwapObject should succeed");
        check(a[0] == v3, "compareAndSwapObject success should modify");
        check(U.getAndSetObject(a, offset(1), v1) == v2, "getAndSetObject should return previous value");
        check(a[1] == v1, "getAndSetObject should modify");
    }

    public static void testObjectArrayOfArrays() {
        testObjectArrayReference(new Object[2][], new Object[1], new String[1], new Integer[1]);
        testObjectArrayReference(new String[2][], new String[1], new String[1], new String[1]);
    }

}
