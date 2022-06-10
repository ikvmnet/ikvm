namespace IKVM.Tool
{

    public static class Program
    {

        public static void Main(string[] args)
        {
#if NETCOREAPP3_1_OR_GREATER
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
        }

    }

}
