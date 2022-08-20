using System.Threading.Tasks;

using IKVM.Tool.Exporter;

namespace ikvmstub
{

    public static class Program
    {

        public static Task<int> Main(string[] args)
        {
            return IkvmExporterTool.Main(args);
        }

    }

}
