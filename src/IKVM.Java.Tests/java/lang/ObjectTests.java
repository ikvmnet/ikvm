package ikvm.tests.java.java.lang;

import java.lang.*;
import java.util.*;
import java.util.concurrent.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class ObjectTests {

    static class CanRunFinalizerObject {

        final CountDownLatch fin;

        public CanRunFinalizerObject(CountDownLatch fin) {
            this.fin = fin;
        }

        protected void finalize() {
            System.out.println("finalize");
            fin.countDown();
        }

    }
    
    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canRunFinalizer() throws Throwable {
        System.gc();

        final CountDownLatch fin = new CountDownLatch(1);
        canRunFinalizerCreateObject(fin);

        System.gc();

        try {
            fin.await();
        } catch (InterruptedException ie) {
            throw new Error(ie);
        }
    }
    
    @cli.System.Runtime.CompilerServices.MethodImplAttribute.Annotation(value = cli.System.Runtime.CompilerServices.MethodImplOptions.__Enum.NoInlining)
    void canRunFinalizerCreateObject(CountDownLatch fin) {
        new CanRunFinalizerObject(fin);
    }

}
