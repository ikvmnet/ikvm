package w;

import java.net.URL;

public class Hello {

    public static void hello() {
        URL url1 = Hello.class.getResource("Hello.class");
        if (url1 == null) throw new RuntimeException();
        URL url2 = Hello.class.getResource("/w/Hello.class");
        if (url2 == null) throw new RuntimeException();
        System.out.println("Hello!");
    }

}