package ikvm.tests.java.java.lang.invoke;

import java.lang.*;
import java.lang.invoke.*;
import java.lang.reflect.*;
import java.util.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class MethodInvokeTests {
    
    public static void switchWithNoCases(String s) {
        switch (s) { }
    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void switchWithNullShouldThrowNullPointerExceptionInInvocationTargetException() throws Throwable {
        Method method = MethodInvokeTests.class.getDeclaredMethod("switchWithNoCases", String.class);

        try {
            method.invoke(null, (String)null);
        } catch (InvocationTargetException ite) {
            if (ite.getTargetException() instanceof NullPointerException) {
                return;
            } else {
                throw new RuntimeException("Expected exception not thrown.", ite);
            }
        }

        throw new RuntimeException("Expected exception not thrown.");
    }

}
