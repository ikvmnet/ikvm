using System;
using System.Collections.Generic;
using System.Text;

namespace IKVM.Sdk.Tests.Project
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine(new sample.HelloworldImpl().sayHello("Bob"));
        }

    }

}
