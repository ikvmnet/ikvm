using IKVM.Runtime;

namespace policytool
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(sun.security.tools.policytool.PolicyTool), args);
        }

    }

}
