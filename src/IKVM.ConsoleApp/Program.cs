using System;

namespace IKVM.ConsoleApp
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Foo();
        }

        public static void Foo()
        {
            new Bar();
            Environment.Exit(0);
        }

        class Bar
        {

            public Bar()
            {
                for (int i = 0; i < 19383; i++)
                    System.Console.WriteLine(java.net.InetAddress.getLocalHost().getHostName());
            }

            ~Bar()
            {

            }

        }

    }

}
