package p;

import java.net.URL;

public class Main {

    public static void main(String[] args) {
        URL url1 = Main.class.getResource("Main.class");
        if (url1 == null) throw new RuntimeException();
        URL url2 = Main.class.getResource("/p/Main.class");
        if (url2 == null) throw new RuntimeException();

        q.Hello.hello();
        w.Hello.hello();
    }

}