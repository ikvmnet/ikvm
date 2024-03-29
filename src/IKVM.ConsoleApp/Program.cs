using System.Diagnostics;
using System.Threading;

namespace IKVM.ConsoleApp
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            while (Debugger.IsAttached == false)
                Thread.Sleep(1000);
        }

    }

}
