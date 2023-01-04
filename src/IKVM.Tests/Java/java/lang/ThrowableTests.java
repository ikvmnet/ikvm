package ikvm.tests.java.lang;

public class ThrowableTests
{

    public static void CanPrintStackTraceFromDynamic() {
        try {
            throw new Exception();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
