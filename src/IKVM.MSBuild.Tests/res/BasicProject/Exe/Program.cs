using System;

using IKVM.MSBuild.Tests.Project.Lib;

namespace IKVM.MSBuild.Tests.Project.Exe
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine(Helloworld.SayHello(args[0]));
        }

    }

}
