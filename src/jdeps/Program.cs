using IKVM.Runtime;

using java.lang;

namespace jdeps
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, ((Class)typeof(com.sun.tools.jdeps.Main)).getName());
        }

    }

}
