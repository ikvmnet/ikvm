using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace IKVM.Tools.Runner
{

    public abstract class IkvmToolLauncher
    {

        readonly string toolName;
        readonly string toolPath;
        readonly IIkvmToolDiagnosticEventListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolName"></param>
        /// <param name="toolPath"></param>
        /// <param name="listener"></param>
        public IkvmToolLauncher(string toolName, string toolPath, IIkvmToolDiagnosticEventListener listener)
        {
            this.toolName = toolName ?? throw new ArgumentNullException(nameof(toolName));
            this.toolPath = toolPath ?? throw new ArgumentNullException(nameof(toolPath));
            this.listener = listener;

            if (Directory.Exists(toolPath) == false)
                throw new DirectoryNotFoundException($"Could not locate tool path {toolPath}.");
        }

        /// <summary>
        /// Logs an event if a listener is provided.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected Task LogEvent(IkvmToolDiagnosticEventLevel level, string message, params object[] args)
        {
            return listener?.ReceiveAsync(new IkvmToolDiagnosticEvent(level, message, args));
        }

        /// <summary>
        /// Gets the path to the tool directory for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolDir(IkvmToolFramework framework, OSPlatform platform, Architecture architecture)
        {
            // determine the TFM of the tool to be executed
            var tfm = framework switch
            {
                IkvmToolFramework.NetFramework => "net461",
                IkvmToolFramework.NetCore => "netcoreapp3.1",
                _ => throw new NotImplementedException(),
            };

            // determine the RID of the tool to be executed
            var rid = architecture switch
            {
                Architecture.X86 => framework switch
                {
                    IkvmToolFramework.NetFramework => "any",
                    IkvmToolFramework.NetCore when platform == OSPlatform.Windows => "win7-x86",
                    IkvmToolFramework.NetCore when platform == OSPlatform.Linux => "linux-x86",
                    _ => throw new NotImplementedException(),
                },
                Architecture.X64 => framework switch
                {
                    IkvmToolFramework.NetFramework => "any",
                    IkvmToolFramework.NetCore when platform == OSPlatform.Windows => "win7-x64",
                    IkvmToolFramework.NetCore when platform == OSPlatform.Linux => "linux-x64",
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };

            // we use a different path and args set based on which version we're running
            return Path.Combine(toolPath, tfm, rid);
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(IkvmToolFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), platform == OSPlatform.Windows ? $"{toolName}.exe" : toolName);
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(IkvmToolFramework framework)
        {
            return GetToolExe(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the path to the reference assemblies for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetReferenceAssemblyDirectory(IkvmToolFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), "refs");
        }

        /// <summary>
        /// Gets the path to the reference assemblies for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetReferenceAssemblyDirectory(IkvmToolFramework framework)
        {
            return GetReferenceAssemblyDirectory(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the current OS platform.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        OSPlatform GetOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX;

            throw new PlatformNotSupportedException();
        }

    }

}
