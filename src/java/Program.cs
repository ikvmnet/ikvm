using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Tools.Java
{

    public static class Program
    {

        [HideFromJava]
        public static int Main(string[] args) => Launcher.Run(null, false, args, "", null);

    }

}
