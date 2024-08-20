package ikvm.tests.java.java.lang;

import java.lang.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class ByteTests {
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canCastMinMax() throws Throwable {
        if (!String.valueOf(Byte.MAX_VALUE).equals("127"))
            throw new Exception();
        if (!String.valueOf(Byte.MIN_VALUE).equals("-128"))
            throw new Exception();
        if (!String.valueOf((short)Byte.MAX_VALUE).equals("127"))
            throw new Exception();
        if (!String.valueOf((short)Byte.MIN_VALUE).equals("-128"))
            throw new Exception();
        if (!String.valueOf((int)Byte.MAX_VALUE).equals("127"))
            throw new Exception();
        if (!String.valueOf((int)Byte.MIN_VALUE).equals("-128"))
            throw new Exception();
        if (!String.valueOf((long)Byte.MAX_VALUE).equals("127"))
            throw new Exception();
        if (!String.valueOf((long)Byte.MIN_VALUE).equals("-128"))
            throw new Exception();

        if (!Integer.valueOf(Byte.MAX_VALUE).equals(Integer.valueOf(127)))
            throw new Exception();
        if (!Integer.valueOf(Byte.MIN_VALUE).equals(Integer.valueOf(-128)))
            throw new Exception();
    }

}
