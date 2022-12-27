import java.io.*;
import java.lang.reflect.InvocationHandler;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.lang.reflect.Proxy;

public final class TestTest {

    public static final String nonPublicIntrfaceName = "java.util.zip.ZipConstants";

    static class Handler implements InvocationHandler, Serializable {
        public Object invoke(Object obj, Method meth, Object[] args) {
            return null;
        }
    }
    
    public static void main(String[] args) throws Exception {
        while (true) {
            TestTest.Handler h = new TestTest.Handler();

            Class<?> nonPublic = Class.forName(nonPublicIntrfaceName);
            if (Modifier.isPublic(nonPublic.getModifiers())) {
                throw new Error("Interface " + nonPublicIntrfaceName + " is public and need to be changed!");
            }

            ByteArrayOutputStream bout = new ByteArrayOutputStream();
            ObjectOutputStream oout = new ObjectOutputStream(bout);
            oout.writeObject(Proxy.newProxyInstance(nonPublic.getClassLoader(), new Class<?>[]{ nonPublic }, new TestTest.Handler()));
            oout.close();
            ObjectInputStream oin = new ObjectInputStream(new ByteArrayInputStream(bout.toByteArray()));
            oin.readObject();
            break;
        }
    }

}
