using IKVM.Runtime;

namespace wsimport
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return Launcher.Launch(typeof(com.sun.tools.@internal.ws.WsImport), args);
        }

    }

}
