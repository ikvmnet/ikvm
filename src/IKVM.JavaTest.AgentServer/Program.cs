namespace IKVM.JavaTest.AgentServer
{

    public static class Program
    {
         
        public static void Main(string[] args)
        {
            new com.sun.javatest.regtest.agent.AgentServer(args).run();
        }

    }

}
