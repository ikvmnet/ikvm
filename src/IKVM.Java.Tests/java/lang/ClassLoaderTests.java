package ikvm.tests.java.java.lang;

import java.net.*;

@cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.Annotation()
public class ClassLoaderTests {

    static class Foo extends ClassLoader {
        @Override
        protected void finalize() {
            System.out.println("");
        }
    }

    static class Bar extends Foo {
        @Override
        protected void finalize() {
            super.finalize();
            System.out.println("");
        }
    }

    static class ShouldPreventUninitializedParentClass {
        static ClassLoader loader;
        
        public static void run() throws Exception {
            try {
                new ClassLoader(null) {
                    @Override
                    protected void finalize() {
                        loader = this;
                    }
                };
            } catch (SecurityException exc) {
                
            }

            System.gc();
            System.runFinalization();

            if (loader != null) {
                try {
                    URLClassLoader child = URLClassLoader.newInstance(new URL[0], loader);
                    throw new RuntimeException("child class loader created");
                } catch (SecurityException se) {
                    // test passed
                }
            } else {
                // test passed
            }
        }
    }

    @cli.Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute.Annotation()
    public void shouldPreventUninitializedParent() throws Exception {
        ShouldPreventUninitializedParentClass.run();
    }

}
