using IKVM.Runtime;

namespace wsgen
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.@internal.ws.WsGen), args);
        }

    }

}
