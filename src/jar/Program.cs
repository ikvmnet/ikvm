namespace jar
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            return ikvm.runtime.Launcher.run(typeof(sun.tools.jar.Main), args, "-J", null);
        }

    }

}
