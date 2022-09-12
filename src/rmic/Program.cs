using IKVM.Runtime;

using java.lang;

namespace rmic
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, ((Class)typeof(sun.rmi.rmic.Main)).getName());
        }

    }

}
