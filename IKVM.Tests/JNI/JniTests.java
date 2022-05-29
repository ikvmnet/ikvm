package ikvm.tests.jni;

public class JniTests
{

	static
	{
		System.load("@@IKVM_TESTS_NATIVE@@");
	}

	public static String echo(String value)
	{
		System.out.println("enter echo");
		String ret = echoImpl(value);
		System.out.println("leave echo");
		return ret;
	}

	public static native String echoImpl(String value);

}
