package ikvm.tests.jni;

public class JniTests
{

	static
	{
		System.load("@@IKVM_TESTS_NATIVE@@");
	}

	public static String echo(String value)
	{
		String ret = echoImpl(value);
		return ret;
	}

	public static native String echoImpl(String value);

}
