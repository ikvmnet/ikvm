using IKVM.Runtime;

namespace javadoc
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.javadoc.Main), args);
        }

    }

}
