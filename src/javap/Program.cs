using IKVM.Runtime;

namespace javap
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.javap.Main), args);
        }

    }

}
