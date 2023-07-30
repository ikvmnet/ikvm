package ikvm.tests.java.java.lang;

import java.lang.*;
import java.util.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class SystemTests {
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canPrintLnString() throws Throwable {
        System.out.println("tests");
    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canPrintLnObject() throws Throwable {
        System.out.println(new Object());
    }

}
