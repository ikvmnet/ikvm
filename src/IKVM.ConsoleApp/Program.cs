using System.Diagnostics;

using java.awt;

namespace IKVM.ConsoleApp
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            while (!Debugger.IsAttached)
                System.Threading.Thread.Sleep(100);

            java.lang.System.@out.println("hi");
            System.Console.ReadLine();
        }

        public class AWTExample1 : Frame
        {

            AWTExample1()
            {
                var t = new TextField("THIS IS TEXT");
                t.setBounds(30, 30, 80, 30);
                add(t);

                var b = new Button("1234");
                b.setLabel("1234");
                b.setBounds(30, 100, 80, 30);
                add(b);

                var b2 = new Button("1234");
                b2.setLabel("1234");
                b2.setBounds(30, 200, 80, 30);
                add(b2);

                setSize(300, 300);
                setTitle("This is our basic AWT example");
                setLayout(null);
                setVisible(true);
            }

            public static void Foo()
            {
                new AWTExample1();
            }

        }

    }

}
