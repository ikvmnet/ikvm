using java.util;
using ikvm.runtime;

namespace java
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.run(null, args, "-J", new Properties());
        }

    }
}
