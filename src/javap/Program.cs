using IKVM.Runtime;

using java.lang;

namespace javap
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, ((Class)typeof(com.sun.tools.javap.Main)).getName());
        }

    }

}
