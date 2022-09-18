using IKVM.Runtime;

namespace jarsigner
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(sun.security.tools.jarsigner.Main), args);
        }

    }

}
