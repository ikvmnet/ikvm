package ikvm.tests.java.java.util.concurrent;

import java.lang.*;
import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class ForkJoinPoolTests {

    class RecursiveActionTestTask extends RecursiveAction {

        private int range;
        private AtomicInteger count;
        
        public RecursiveActionTestTask(int range, AtomicInteger count) {
            this.range = range;
            this.count = count;
        }
        
        protected void compute() {
            if (this.range > 1) {
                RecursiveActionTestTask t1 = new RecursiveActionTestTask(this.range / 2, count);
                t1.fork();
                RecursiveActionTestTask t2 = new RecursiveActionTestTask(this.range / 2, count);
                t2.fork();

                t1.join();
                t2.join();
            } else {
                count.incrementAndGet();
            }
        }

    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void canExecuteRecursiveAction() throws Exception {
        ForkJoinPool p = ForkJoinPool.commonPool();
        AtomicInteger c = new AtomicInteger();
        RecursiveActionTestTask t = new RecursiveActionTestTask(1024 * 1024, c);
        p.invoke(t);
        if (c.get() != 1024 * 1024) {
            throw new Exception();
        }
    }

}
