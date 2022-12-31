package ikvm.tests.jni;

public class JniTests
{

	static
	{
		System.load("@@IKVM_TESTS_NATIVE@@");
	}

	public native void getVersionTest();

	public native void defineClassTest();

	public native void findClassTest();

	public native void getSuperclassTest();

	public native void throwTest();

	public native void throwNewTest();

	public native void newObjectTest();

	public native void newObjectVTest();

	public native void newObjectATest();

	public native void newObjectTestWithArg();

	public native void newObjectVTestWithArg();

	public native void newObjectATestWithArg();

	public native Object getNullObjectField(Class clazz, Object o);

	public native Object getObjectField(Class clazz, Object o);

	public native Object getStringField(Class clazz, Object o);

	public native boolean getBooleanField(Class clazz, Object o);

	public native byte getByteField(Class clazz, Object o);

	public native char getCharField(Class clazz, Object o);

	public native short getShortField(Class clazz, Object o);

	public native int getIntField(Class clazz, Object o);

	public native long getLongField(Class clazz, Object o);

	public native float getFloatField(Class clazz, Object o);

	public native double getDoubleField(Class clazz, Object o);

	public native Object getStaticNullObjectField(Class clazz);

	public native Object getStaticObjectField(Class clazz);

	public native Object getStaticStringField(Class clazz);

	public native boolean getStaticBooleanField(Class clazz);

	public native byte getStaticByteField(Class clazz);

	public native char getStaticCharField(Class clazz);

	public native short getStaticShortField(Class clazz);

	public native int getStaticIntField(Class clazz);

	public native long getStaticLongField(Class clazz);

	public native float getStaticFloatField(Class clazz);

	public native double getStaticDoubleField(Class clazz);

	public native Object newWeakGlobalRef(Object o);

}
