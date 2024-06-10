using java.text;

namespace IKVM.ConsoleApp
{

    public class Program
    {
        public static void Main(string[] args)
        {
            java.text.Normalizer.normalize("hi", Normalizer.Form.NFC);
            System.Console.ReadLine();
        }

    }

}
