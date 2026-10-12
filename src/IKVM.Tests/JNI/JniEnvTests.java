package ikvm.tests.jni;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.nio.ByteBuffer;

/**
 * Exercises the functions of the JNI function table, one native method per scenario. The native side checks its own
 * results and reports a mismatch by throwing {@link Failure}, so a native method that returns normally has passed.
 */
public class JniEnvTests
{

	static
	{
		System.loadLibrary("ikvm-tests");
	}

	/**
	 * Thrown by the native side when a check fails. The cause is the exception that was pending at the time, if any.
	 */
	public static class Failure extends RuntimeException
	{

		public Failure(String message, Throwable cause)
		{
			super(message, cause);
		}

	}

	public interface TargetInterface
	{

		int CONSTANT = 77;

		int interfaceMethod();

		default int defaultMethod()
		{
			return 10;
		}

	}

	public static abstract class AbstractTarget
	{

	}

	/**
	 * Target of the method calls and field accesses. Every method records the arguments it was called with, so the
	 * caller can check that each one arrived intact.
	 */
	public static class Target implements TargetInterface
	{

		public static String staticArgs;

		public static boolean zStatic;
		public static byte bStatic;
		public static char cStatic;
		public static short sStatic;
		public static int iStatic;
		public static long jStatic;
		public static float fStatic;
		public static double dStatic;
		public static Object lStatic;

		public static final int CONSTANT = 99;

		public String args;
		public boolean constructed;

		public int shadowed = 1;
		private int privateField = 3;
		public final int finalField = 4;
		public int[] intArrayField;
		public String stringField;

		public boolean zField;
		public byte bField;
		public char cField;
		public short sField;
		public int iField;
		public long jField;
		public float fField;
		public double dField;
		public Object lField;

		public static String format(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			return z + "," + b + "," + (int)c + "," + s + "," + i + "," + j + "," + f + "," + d + "," + l;
		}

		public Target()
		{
			constructed = true;
		}

		public Target(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			constructed = true;
			args = format(z, b, c, s, i, j, f, d, l);
		}

		public String getArgs()
		{
			return args;
		}

		public boolean isConstructed()
		{
			return constructed;
		}

		public static String getStaticArgs()
		{
			return staticArgs;
		}

		public boolean booleanMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return true;
		}

		public byte byteMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return (byte)-12;
		}

		public char charMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return 'x';
		}

		public short shortMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return (short)-1234;
		}

		public int intMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return 123456789;
		}

		public long longMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return 1234567890123L;
		}

		public float floatMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return 1.5f;
		}

		public double doubleMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return 2.25d;
		}

		public Object objectMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
			return "base";
		}

		public void voidMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = format(z, b, c, s, i, j, f, d, l);
		}

		public static boolean staticBooleanMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return true;
		}

		public static byte staticByteMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return (byte)-12;
		}

		public static char staticCharMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return 'x';
		}

		public static short staticShortMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return (short)-1234;
		}

		public static int staticIntMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return 123456789;
		}

		public static long staticLongMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return 1234567890123L;
		}

		public static float staticFloatMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return 1.5f;
		}

		public static double staticDoubleMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return 2.25d;
		}

		public static Object staticObjectMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
			return "base";
		}

		public static void staticVoidMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			staticArgs = format(z, b, c, s, i, j, f, d, l);
		}

		public int interfaceMethod()
		{
			return 1;
		}

		public int inheritedMethod()
		{
			return 5;
		}

		private int privateMethod()
		{
			return 7;
		}

		public static int staticSimpleMethod()
		{
			return 8;
		}

		public int[] reverse(int[] a)
		{
			int[] r = new int[a.length];
			for (int i = 0; i < a.length; i++)
				r[i] = a[a.length - 1 - i];

			return r;
		}

		public Object nullMethod()
		{
			return null;
		}

		public void throwsChecked() throws java.io.IOException
		{
			throw new java.io.IOException("checked");
		}

		public static double weigh(int i1, long j1, float f1, double d1, int i2, long j2, float f2, double d2, int i3, long j3, float f3, double d3, int i4, long j4, float f4, double d4)
		{
			return i1 + 2 * j1 + 3 * f1 + 4 * d1 + 5 * i2 + 6 * j2 + 7 * f2 + 8 * d2 + 9 * i3 + 10 * j3 + 11 * f3 + 12 * d3 + 13 * i4 + 14 * j4 + 15 * f4 + 16 * d4;
		}

		public double manyArgs(int i1, long j1, float f1, double d1, int i2, long j2, float f2, double d2, int i3, long j3, float f3, double d3, int i4, long j4, float f4, double d4)
		{
			return weigh(i1, j1, f1, d1, i2, j2, f2, d2, i3, j3, f3, d3, i4, j4, f4, d4);
		}

		public static double staticManyArgs(int i1, long j1, float f1, double d1, int i2, long j2, float f2, double d2, int i3, long j3, float f3, double d3, int i4, long j4, float f4, double d4)
		{
			return weigh(i1, j1, f1, d1, i2, j2, f2, d2, i3, j3, f3, d3, i4, j4, f4, d4);
		}

		public void throwingMethod()
		{
			throw new IllegalStateException("thrown by target");
		}

		public static void staticThrowingMethod()
		{
			throw new IllegalStateException("thrown by target");
		}

	}

	/**
	 * Overrides every method of {@link Target}, so that virtual and nonvirtual calls give different results.
	 */
	public static class SubTarget extends Target
	{

		public int shadowed = 2;

		public boolean booleanMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return false;
		}

		public byte byteMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return (byte)34;
		}

		public char charMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return 'y';
		}

		public short shortMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return (short)4321;
		}

		public int intMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return -987654321;
		}

		public long longMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return -9876543210L;
		}

		public float floatMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return -3.5f;
		}

		public double doubleMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return -4.75d;
		}

		public Object objectMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
			return "sub";
		}

		public void voidMethod(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l)
		{
			args = "sub:" + format(z, b, c, s, i, j, f, d, l);
		}

		public int interfaceMethod()
		{
			return 2;
		}

	}

	/**
	 * Its native method is bound with RegisterNatives.
	 */
	public static class RegisteredNatives
	{

		public static native int registered(int value);

	}

	/**
	 * Its native method is bound with RegisterNatives and then unbound again with UnregisterNatives.
	 */
	public static class UnregisteredNatives
	{

		public static native int unregistered(int value);

	}

	public static class InitTracker
	{

		public static boolean initialized;

	}

	public static class Initialized
	{

		static
		{
			InitTracker.initialized = true;
		}

	}

	public static class FailingInitializer
	{

		static
		{
			explode();
		}

		static void explode()
		{
			throw new IllegalStateException("failing initializer");
		}

	}

	/**
	 * Its native method is exported by the library, and then bound to something else with RegisterNatives.
	 */
	public static class ReRegisteredNatives
	{

		public static native int value();

	}

	/**
	 * Its native methods are bound with a single call to RegisterNatives.
	 */
	public static class MultiRegisteredNatives
	{

		public static native int first(int value);

		public native int second(int value);

	}

	public static boolean holdsLockWhileSynchronized(Object o)
	{
		synchronized (o)
		{
			return Thread.holdsLock(o);
		}
	}

	public String catchFromNative()
	{
		try
		{
			leavesExceptionPending();
			return null;
		}
		catch (IllegalStateException e)
		{
			return e.getMessage();
		}
	}

	public static Target newTarget()
	{
		return new Target();
	}

	public static Target newSubTarget()
	{
		return new SubTarget();
	}

	public static ClassLoader newClassLoader()
	{
		return new ClassLoader(null) { };
	}

	public static Field getField(Class<?> clazz, String name) throws Exception
	{
		return clazz.getField(name);
	}

	public static Method getMethod(Class<?> clazz, String name) throws Exception
	{
		for (Method m : clazz.getMethods())
			if (m.getName().equals(name))
				return m;

		throw new NoSuchMethodException(name);
	}

	public static Constructor<?> getDefaultConstructor(Class<?> clazz) throws Exception
	{
		return clazz.getConstructor();
	}

	public static ByteBuffer allocateHeapBuffer(int capacity)
	{
		return ByteBuffer.allocate(capacity);
	}

	/**
	 * Called by {@link #localRefsSurviveNestedNativeCalls(int)} to nest another native frame.
	 */
	public void nest(int depth)
	{
		localRefsSurviveNestedNativeCalls(depth);
	}

	// version and classes

	public native void getVersion();

	public native Class<?> defineClass(String name, ClassLoader loader, byte[] bytes);

	public native void defineClassWithBadBytes(ClassLoader loader);

	public native void findClass();

	public native void findClassVisibleOnlyToTheCallersLoader();

	public native void findClassNotFound();

	public native void findClassWithDottedName();

	public native void getSuperclass();

	public native void isAssignableFrom();

	public native void getObjectClass();

	public native void isInstanceOf();

	public native void isInstanceOfNull();

	public native void isSameObject();

	// reflection

	public native void fromReflectedMethodAndBack(Method method);

	public native void fromReflectedConstructorAndBack(Constructor<?> ctor);

	public native void fromReflectedFieldAndBack(Field field);

	// exceptions

	public native void throwAndCheck();

	public native void throwNewWithNullMessage();

	public native void exceptionOccurredAndClear();

	public native void exceptionDescribeClears();

	public native void exceptionFromCalledMethodIsPending();

	public native void exceptionFromCalledStaticMethodIsPending();

	public native void leavesExceptionPending();

	// references

	public native void newLocalRefAndDeleteLocalRef();

	public native void manyLocalRefs(int count);

	public native void deletedLocalRefsAreReused(int count);

	public native void ensureLocalCapacity();

	public native void pushAndPopLocalFrame();

	public native void popLocalFrameWithNullResult();

	public native void nestedLocalFrames(int depth);

	public native void manyLocalFrames(int count);

	public native void localRefsSurviveNestedNativeCalls(int depth);

	public native long newGlobalRef(Object o);

	public native Object resolveGlobalRef(long ref);

	public native void deleteGlobalRef(long ref);

	public native void newGlobalRefOfNull();

	public native void manyGlobalRefs(int count);

	public native long newWeakGlobalRef(Object o);

	public native Object resolveWeakGlobalRef(long ref);

	public native boolean isWeakGlobalRefCleared(long ref);

	public native void deleteWeakGlobalRef(long ref);

	public native void newWeakGlobalRefOfNull();

	public native void manyWeakGlobalRefs(int count);

	public native void getObjectRefType();

	public native void getObjectRefTypeOfNull();

	// objects

	public native Object allocObject();

	public native void allocObjectOfAbstractClass();

	public native void allocObjectOfInterface();

	public native Object newObjectWithArgs(int mode);

	public native void newObjectOfAbstractClass();

	public native Object newObjectThatThrows();

	// method lookup

	public native void getMethodID();

	public native void getMethodIDOfInheritedMethod();

	public native void getMethodIDOfInterfaceMethod();

	public native void getMethodIDNotFound();

	public native void getMethodIDOfStaticMethod();

	public native void getStaticMethodID();

	public native void getStaticMethodIDNotFound();

	public native void getStaticMethodIDOfInstanceMethod();

	// method calls, where mode 0 is the variadic function, 1 the va_list function and 2 the jvalue array function

	public native boolean callBooleanMethod(Object target, int mode);

	public native byte callByteMethod(Object target, int mode);

	public native char callCharMethod(Object target, int mode);

	public native short callShortMethod(Object target, int mode);

	public native int callIntMethod(Object target, int mode);

	public native long callLongMethod(Object target, int mode);

	public native float callFloatMethod(Object target, int mode);

	public native double callDoubleMethod(Object target, int mode);

	public native Object callObjectMethod(Object target, int mode);

	public native void callVoidMethod(Object target, int mode);

	public native boolean callNonvirtualBooleanMethod(Object target, int mode);

	public native byte callNonvirtualByteMethod(Object target, int mode);

	public native char callNonvirtualCharMethod(Object target, int mode);

	public native short callNonvirtualShortMethod(Object target, int mode);

	public native int callNonvirtualIntMethod(Object target, int mode);

	public native long callNonvirtualLongMethod(Object target, int mode);

	public native float callNonvirtualFloatMethod(Object target, int mode);

	public native double callNonvirtualDoubleMethod(Object target, int mode);

	public native Object callNonvirtualObjectMethod(Object target, int mode);

	public native void callNonvirtualVoidMethod(Object target, int mode);

	public native boolean callStaticBooleanMethod(int mode);

	public native byte callStaticByteMethod(int mode);

	public native char callStaticCharMethod(int mode);

	public native short callStaticShortMethod(int mode);

	public native int callStaticIntMethod(int mode);

	public native long callStaticLongMethod(int mode);

	public native float callStaticFloatMethod(int mode);

	public native double callStaticDoubleMethod(int mode);

	public native Object callStaticObjectMethod(int mode);

	public native void callStaticVoidMethod(int mode);

	public native int callInterfaceMethod(Object target);

	public native Class<?> callStaticMethodWithCallerID(String className);

	// fields

	public native void getFieldID();

	public native void getFieldIDOfInheritedField();

	public native void getFieldIDNotFound();

	public native void getFieldIDWithWrongSignature();

	public native void getFieldIDOfStaticField();

	public native void getStaticFieldIDOfInstanceField();

	public native boolean getBooleanField(Object o);

	public native byte getByteField(Object o);

	public native char getCharField(Object o);

	public native short getShortField(Object o);

	public native int getIntField(Object o);

	public native long getLongField(Object o);

	public native float getFloatField(Object o);

	public native double getDoubleField(Object o);

	public native Object getObjectField(Object o);

	public native void setBooleanField(Object o, boolean value);

	public native void setByteField(Object o, byte value);

	public native void setCharField(Object o, char value);

	public native void setShortField(Object o, short value);

	public native void setIntField(Object o, int value);

	public native void setLongField(Object o, long value);

	public native void setFloatField(Object o, float value);

	public native void setDoubleField(Object o, double value);

	public native void setObjectField(Object o, Object value);

	public native boolean getStaticBooleanField();

	public native byte getStaticByteField();

	public native char getStaticCharField();

	public native short getStaticShortField();

	public native int getStaticIntField();

	public native long getStaticLongField();

	public native float getStaticFloatField();

	public native double getStaticDoubleField();

	public native Object getStaticObjectField();

	public native void setStaticBooleanField(boolean value);

	public native void setStaticByteField(byte value);

	public native void setStaticCharField(char value);

	public native void setStaticShortField(short value);

	public native void setStaticIntField(int value);

	public native void setStaticLongField(long value);

	public native void setStaticFloatField(float value);

	public native void setStaticDoubleField(double value);

	public native void setStaticObjectField(Object value);

	// strings

	public native String newString(char[] chars);

	public native int getStringLength(String s);

	public native char[] getStringChars(String s);

	public native String newStringUTF(byte[] bytes);

	public native void newStringUTFOfNull();

	public native void newStringUTFWithInvalidBytes();

	public native int getStringUTFLength(String s);

	public native byte[] getStringUTFChars(String s);

	public native char[] getStringRegion(String s, int start, int len);

	public native byte[] getStringUTFRegion(String s, int start, int len);

	public native char[] getStringCritical(String s);

	// arrays

	public native int getArrayLength(Object array);

	public native Object[] newObjectArray(int length, Class<?> clazz, Object init);

	public native Object getObjectArrayElement(Object[] array, int index);

	public native void setObjectArrayElement(Object[] array, int index, Object value);

	public native boolean[] newBooleanArray(int length);

	public native byte[] newByteArray(int length);

	public native char[] newCharArray(int length);

	public native short[] newShortArray(int length);

	public native int[] newIntArray(int length);

	public native long[] newLongArray(int length);

	public native float[] newFloatArray(int length);

	public native double[] newDoubleArray(int length);

	public native void modifyBooleanArrayElements(boolean[] array, int mode);

	public native void modifyByteArrayElements(byte[] array, int mode);

	public native void modifyCharArrayElements(char[] array, int mode);

	public native void modifyShortArrayElements(short[] array, int mode);

	public native void modifyIntArrayElements(int[] array, int mode);

	public native void modifyLongArrayElements(long[] array, int mode);

	public native void modifyFloatArrayElements(float[] array, int mode);

	public native void modifyDoubleArrayElements(double[] array, int mode);

	public native boolean[] getBooleanArrayRegion(boolean[] array, int start, int len);

	public native byte[] getByteArrayRegion(byte[] array, int start, int len);

	public native char[] getCharArrayRegion(char[] array, int start, int len);

	public native short[] getShortArrayRegion(short[] array, int start, int len);

	public native int[] getIntArrayRegion(int[] array, int start, int len);

	public native long[] getLongArrayRegion(long[] array, int start, int len);

	public native float[] getFloatArrayRegion(float[] array, int start, int len);

	public native double[] getDoubleArrayRegion(double[] array, int start, int len);

	public native void setBooleanArrayRegion(boolean[] array, int start, int len, boolean[] values);

	public native void setByteArrayRegion(byte[] array, int start, int len, byte[] values);

	public native void setCharArrayRegion(char[] array, int start, int len, char[] values);

	public native void setShortArrayRegion(short[] array, int start, int len, short[] values);

	public native void setIntArrayRegion(int[] array, int start, int len, int[] values);

	public native void setLongArrayRegion(long[] array, int start, int len, long[] values);

	public native void setFloatArrayRegion(float[] array, int start, int len, float[] values);

	public native void setDoubleArrayRegion(double[] array, int start, int len, double[] values);

	public native void modifyPrimitiveArrayCritical(int[] array, int mode);

	public native void nestedPrimitiveArrayCritical(int[] a, byte[] b);

	// natives

	public native void registerNatives();

	public native void unregisterNatives();

	public native void registerNativesNotFound();

	public native int under_score();

	public native int dollar$sign();

	public native int café();

	public native int overloaded(int value);

	public native int overloaded(String value);

	public native void notImplemented();

	public native String returnsWrongType(Object o);

	public synchronized native boolean synchronizedHoldsLock();

	public static synchronized native boolean staticSynchronizedHoldsLock();

	// monitors

	public native void monitorEnterAndExit(Object o);

	public native void monitorEnterIsReentrant(Object o);

	public native void monitorExitNotOwned(Object o);

	// direct buffers

	public native void newDirectByteBuffer();

	public native void getDirectBufferAddressOfHeapBuffer();

	// virtual machine

	public native void getJavaVM();

	public native void getEnvWithUnsupportedVersion();

	public native void getCreatedJavaVMs();

	public native void createJavaVMWhenOneExists();

	public native void attachCurrentThread(int variant);

	// native method arguments and results

	public native String echoArgs(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l);

	public static native String staticEchoArgs(boolean z, byte b, char c, short s, int i, long j, float f, double d, Object l);

	public native double manyNativeArgs(int i1, long j1, float f1, double d1, int i2, long j2, float f2, double d2, int i3, long j3, float f3, double d3, int i4, long j4, float f4, double d4);

	public native boolean returnBoolean(boolean value);

	public native byte returnByte(byte value);

	public native char returnChar(char value);

	public native short returnShort(short value);

	public native int returnInt(int value);

	public native long returnLong(long value);

	public native float returnFloat(float value);

	public native double returnDouble(double value);

	public native Object returnObject(Object value);

	public native int[] returnArray(int[] value);

	public native Object returnNull();

	public native Object returnGlobalRef(Object value);

	public native int throwAndReturnValue();

	// more calls

	public native void callMethodsOnString(String s);

	public native void callObjectMethodsOnArray(int[] a);

	public native String callThrowableGetMessage(Throwable t);

	public native int callDefaultMethod(Object target);

	public native int callPrivateMethod(Object target);

	public native int callInheritedStaticMethod();

	public native Object constructWithCallNonvirtualVoidMethod();

	public native int[] callWithArrays(Object target, int[] a);

	public native void callReturningNull(Object target);

	public native void callThrowingChecked(Object target);

	public native double callManyArgs(Object target, int mode);

	public native double callStaticManyArgs(int mode);

	// more fields

	public native void accessPrivateField(Object target);

	public native int getStaticConstant();

	public native int getInterfaceConstant();

	public native void accessInheritedStaticField();

	public native void accessArrayAndStringFields(Object target);

	public native void accessShadowedFields(Object subTarget);

	// more classes

	public native void findArrayAndNestedClasses();

	public native void findClassRunsStaticInitializer();

	public native void findClassWithFailingStaticInitializer();

	public native void arraysImplementCloneableAndSerializable();

	// more exceptions

	public native void throwNewWithUnicodeMessage();

	public native void throwNewChecked();

	public native void rethrow(Throwable t);

	public native void exceptionOccurredIsTheThrownObject();

	// more references

	public native void convertBetweenRefKinds();

	public native void deleteLocalRefFromOuterFrame();

	public native void popLocalFrameWithGlobalResult();

	public native void pushLocalFrameWithLargeAndZeroCapacity();

	// more arrays

	public native void emptyArrayElements();

	public native void arraysOfArrays();

	public native void objectArrayCovariance();

	public native void ghostInterfaceArrayStoreCheck();

	public native void primitiveArrayCriticalOfWideTypes(long[] l, double[] d);

	public native void emptyStrings();

	// more natives

	public native void registerMultipleNatives();

	public native void reRegisterNatives();

	// more monitors

	public native void monitorEnterThenSynchronizedJava(Object o);

	// more direct buffers

	public native void directBufferFromJava(ByteBuffer buffer);

	public native void zeroCapacityDirectBuffer();

	// more virtual machine

	public native void javaVMFromOnLoad();

}
