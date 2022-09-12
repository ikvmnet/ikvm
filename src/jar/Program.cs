using IKVM.Runtime;

using java.lang;

namespace jar
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, ((Class)typeof(sun.tools.jar.Main)).getName());
        }

    }

}
