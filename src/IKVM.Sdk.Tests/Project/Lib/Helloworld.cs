using System;
using System.Collections.Generic;
using System.Text;

namespace IKVM.Sdk.Tests.Project.Lib
{

    public static class Helloworld
    {

        public static void SayHello(string value)
        {
            Console.WriteLine(new sample.HelloworldImpl().sayHello(value));
        }

    }

}
