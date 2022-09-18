using IKVM.Runtime;

namespace javah
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.javah.Main), args);
        }

    }

}
