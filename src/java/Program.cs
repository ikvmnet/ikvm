using IKVM.Runtime;

namespace java
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Execute(args, null, false, "");
        }

    }
}
