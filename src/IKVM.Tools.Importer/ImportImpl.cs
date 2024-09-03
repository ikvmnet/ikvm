using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    class ImportImpl
    {

        public async Task<int> ExecuteAsync(ImportOptions options, CancellationToken cancellationToken)
        {
            return await Task.Run(() => IkvmImporterInternal.Execute(options));
        }

    }

}
