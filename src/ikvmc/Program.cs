using System.Threading.Tasks;

using IKVM.Tools.Importer;

namespace IKVM.Tools.Importer
{

    public static class Program
    {

        public static Task<int> Main(string[] args)
        {
            return IkvmImporterTool.Main(args);
        }

    }

}
