using System;
using System.Diagnostics.Tracing;

using IKVM.Runtime;

namespace IKVM.ConsoleApp
{

    public class Program
    {

        public static void Main(string[] args)
        {
            var l = new Listener();
            var o = new java.lang.Object();
            java.lang.System.loadLibrary("hi");
        }

    }

    class Listener : EventListener
    {

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine(eventData);
        }

    }

}
