using IKVM.Runtime;

namespace jdeps
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.jdeps.Main), args);
        }

    }

}
