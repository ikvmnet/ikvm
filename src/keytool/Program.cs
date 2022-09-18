using IKVM.Runtime;

namespace keytool
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(sun.security.tools.keytool.Main), args);
        }

    }

}
