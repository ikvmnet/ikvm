using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Exporter;

namespace ikvmexp
{

    public static class Program
    {

        public static Task<int> Main(string[] args)
        {
            return IkvmExporterTool.Main(args, CancellationToken.None);
        }

    }

}
