using ikvm.runtime;

using IKVM.Attributes;

using java.util;

namespace IKVM.Tools.Java
{

    public static class Program
    {

        [HideFromJava]
        public static int Main(string[] args) => Launcher.run(null, args, "", new Properties());

    }

}
