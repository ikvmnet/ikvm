package ikvm.tests.java.java.lang;

import java.lang.*;
import java.util.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class ThreadTests {
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canPrintStackTrace() throws Throwable {
        for (StackTraceElement ste : Thread.currentThread().getStackTrace()) {
            System.out.println(ste + "\n");
        }
    }

}
