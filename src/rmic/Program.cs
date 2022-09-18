namespace rmic
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return ikvm.runtime.Launcher.run(typeof(sun.rmi.rmic.Main), args, "-J", null);
        }

    }

}
