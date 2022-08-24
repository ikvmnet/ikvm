package IKVM.NET.Sdk.Tests.Project.Exe;

import java.lang.*;
import IKVM.NET.Sdk.Tests.Project.Lib.Helloworld;

public final class Program
{

    public static void main(String[] args)
    {
        System.out.println(Helloworld.sayHello(args[0]));
    }

}
