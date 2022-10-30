namespace IKVM.JTReg.TestAdapter.Core
{

    public interface IJTRegLoggerContext
    {

        void SendMessage(JTRegTestMessageLevel level, string message);

    }

}
