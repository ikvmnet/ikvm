using System;

namespace IKVM.ConsoleApp
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            var url = ((java.lang.Class)typeof(java.lang.Object)).getResource("Object.class");
            using var stm = url.openStream();
            var buf = new byte[1024];
            stm.read(buf);
        }

    }

}
