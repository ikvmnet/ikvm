package ikvm.tests.java.java.lang.reflect;

import java.lang.reflect.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class StringTests {
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canInvokeToStringOnString() throws Throwable {
        String test = "test";
        test.getClass().getMethod("toString").invoke(test);
    }

}
