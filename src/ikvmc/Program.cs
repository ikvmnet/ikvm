using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    public static class Program
    {

        public static Task<int> Main(string[] args)
        {
            return Task.FromResult(IkvmImporterInternal.Execute(args));
        }

    }

}
