extern alias helloworld21;

using System;
using System.Collections.Generic;
using System.Text;

namespace IKVM.Sdk.Tests.Project.Lib
{

    public static class Helloworld
    {

        public static string SayHello(string value)
        {
            return new sample.HelloworldImpl().sayHello(value);
        }

        public static string SayHello21(string value)
        {
            return new helloworld21::sample.HelloworldImpl().sayHello(value);
        }

    }

}
