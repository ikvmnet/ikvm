using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IKVM.ConsoleApp
{

    public class Program
    {
        public static void Main(string[] args)
        {
            while (!Debugger.IsAttached)
                System.Threading.Thread.Sleep(100);

            new Program().New();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void New()
        {
            java.lang.System.@out.println("hi");
            System.Console.ReadLine();
        }

    }

}
