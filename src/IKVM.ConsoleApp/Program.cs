using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IKVM.ConsoleApp
{

    public static class Program
    {

        [ModuleInitializer]
        internal static void Foo()
        {
            Console.WriteLine("hi");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("main");
        }

    }

}
