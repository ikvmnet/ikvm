using System.Diagnostics;

using java.lang;

namespace IKVM.ConsoleApp
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            Thread.sleep(5000);
            var s = new java.io.File("/mnt/c/dev/ikvm").getCanonicalPath();
        }

    }

}
