using IKVM.Runtime.JNI;

namespace IKVM.ConsoleApp
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            var f = new JNIFrame();
            f.Enter(null);
        }

    }

}