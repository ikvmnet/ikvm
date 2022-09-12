using IKVM.Runtime;

using java.lang;

namespace keytool
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, ((Class)typeof(sun.security.tools.keytool.Main)).getName());
        }

    }

}
