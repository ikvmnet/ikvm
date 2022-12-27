﻿package ikvm.tests.jni;

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

}
