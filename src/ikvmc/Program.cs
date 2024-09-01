using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Importer;

namespace ikvmc
{

    public static class Program
    {

        public static Task<int> Main(string[] args)
        {
            return ImportTool.InvokeAsync(args, CancellationToken.None);
        }

    }

}
