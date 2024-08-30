using System.Threading.Tasks;

using IKVM.Tools.Exporter;

namespace ikvmstub
{

    public static class Program
    {

        public static Task Main(string[] args)
        {
            return ExportTool.InvokeAsync(args);
        }

    }

}
