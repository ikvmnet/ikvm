using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.io
{

    [TestClass]
    public class SerializableTests
    {

        [TestMethod]
        public void CanSerializeNonPublicInterfaceDynamic()
        {
            var source = """

import java.io.*;
import java.lang.reflect.InvocationHandler;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.lang.reflect.Proxy;

public class NonPublicInterface {

    static class Handler implements InvocationHandler, Serializable {
        public Object invoke(Object obj, Method meth, Object[] args) {
            return null;
        }
    }

    public static final String nonPublicInterfaceName = "java.util.zip.ZipConstants";

    public void main() throws Exception {
        Class<?> nonPublic = Class.forName(nonPublicInterfaceName);
        if (Modifier.isPublic(nonPublic.getModifiers())) {
            throw new Error("Interface " + nonPublicInterfaceName + " is public and need to be changed!");
        }

        ByteArrayOutputStream bout = new ByteArrayOutputStream();
        ObjectOutputStream oout = new ObjectOutputStream(bout);
        oout.writeObject(Proxy.newProxyInstance(nonPublic.getClassLoader(), new Class<?>[]{ nonPublic }, new Handler()));
        oout.close();
        ObjectInputStream oin = new ObjectInputStream(new ByteArrayInputStream(bout.toByteArray()));
        oin.readObject();
    }
}

""";
            var unit = new InMemoryCodeUnit("NonPublicInterface", source);
            var compiler = new InMemoryCompiler(new[] { unit });
            compiler.Compile();

            // create an instance of the test class
            var clazz = compiler.GetClass("NonPublicInterface");
            var ctor = clazz.getConstructor();
            var test = (dynamic)ctor.newInstance(System.Array.Empty<object>());
            test.main();
        }

    }

}
