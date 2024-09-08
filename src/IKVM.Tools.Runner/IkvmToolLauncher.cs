using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Runner.Diagnostics;

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
        /// <param name="event"></param>
        /// <returns></returns>
        protected ValueTask LogEventAsync(in IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken)
        {
            return listener.ReceiveAsync(@event, cancellationToken);
        }

        /// <summary>
        /// Logs an anonymous event if a listener is provided.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected ValueTask LogEventAsync(IkvmToolDiagnosticEventLevel level, string message, string[] args, CancellationToken cancellationToken)
        {
            return LogEventAsync(new IkvmToolDiagnosticEvent(-1, level, message, args), cancellationToken);
        }

        /// <summary>
        /// Parses the line and logs it.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected ValueTask ParseAndLogEventAsync(string line, CancellationToken cancellationToken)
        {
            return LogEventAsync(ParseEvent(line), cancellationToken);
        }

        /// <summary>
        /// Parse a line of JSON tool output into a diagnostic event.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected IkvmToolDiagnosticEvent ParseEvent(string line)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(line);
                var reader = new Utf8JsonReader(buffer);
                if (reader.Read() == false)
                    throw new JsonException();

                return IkvmToolDiagnosticEvent.ReadJson(ref reader);
            }
            catch (JsonException)
            {
                return new IkvmToolDiagnosticEvent(0, IkvmToolDiagnosticEventLevel.Fatal, "Unable to parse tool output: {0}", [line]);
            }
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(toolPath, platform == OSPlatform.Windows ? $"{toolName}.exe" : toolName);
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe()
        {
            return GetToolExe(GetOSPlatform(), RuntimeInformation.OSArchitecture);
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
