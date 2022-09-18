using IKVM.Runtime;

namespace javac
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.javac.Main), args);
        }

    }

}
