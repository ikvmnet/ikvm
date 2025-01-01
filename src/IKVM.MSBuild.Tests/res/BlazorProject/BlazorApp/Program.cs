using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp
{

    public static class Program
    {

        public static async Task Main(string[] args)
        {
            await WebAssemblyHostBuilder.CreateDefault(args).Build().RunAsync();
        }

    }

}
