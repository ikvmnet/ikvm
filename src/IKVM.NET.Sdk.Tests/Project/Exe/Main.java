package ikvm.net.sdk.tests.project.exe;

import java.lang.*;
import ikvm.net.sdk.tests.project.lib.Helloworld;

public final class Main
{

    public static void main(String[] args)
    {
        System.out.println(Helloworld.sayHello(args[0]));
    }

}
