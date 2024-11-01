using System.Threading;

using IKVM.JTReg.TestAdapter.Core;

namespace IKVM.ConsoleApp
{

    public class Program
    {

        class MyDiscoverycontext : IJTRegDiscoveryContext
        {

            public JTRegTestOptions Options => new JTRegTestOptions()
            {

            };

            public void SendMessage(JTRegTestMessageLevel level, string message)
            {

            }

            public void SendTestCase(JTRegTestCase testCase)
            {

            }

        }

        public static void Main(string[] args)
        {
            JTRegTestManager.Instance.DiscoverTests(@"D:\ikvm\src\IKVM.JTReg.TestAdapter.Tests\bin\Debug\net478\IKVM.JTReg.TestAdapter.Tests.dll", new MyDiscoverycontext(), CancellationToken.None);
        }

    }

}
