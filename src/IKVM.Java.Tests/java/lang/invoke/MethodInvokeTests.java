package ikvm.tests.java.java.lang.invoke;

import java.lang.*;
import java.lang.invoke.*;
import java.lang.reflect.*;
import java.util.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class MethodInvokeTests {
    
    public static void shouldWrapNullPointerExceptionInFastMethodInvokeImpl() {
        throw new NullPointerException();
    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void shouldWrapNullPointerExceptionInMethodInvoke() throws Throwable {
        Method method = MethodInvokeTests.class.getDeclaredMethod("shouldWrapNullPointerExceptionInFastMethodInvokeImpl");

        for (int i = 0; i < 30; i++) {
            try {
                method.invoke();
            } catch (InvocationTargetException ite) {
                if (ite.getTargetException() instanceof NullPointerException) {
                    continue;
                } else {
                    throw new RuntimeException("Expected exception not thrown.", ite);
                }
            }

            throw new RuntimeException("Expected exception not thrown.");
        }
    }

}
