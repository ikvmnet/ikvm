using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace IKVM.Tests.Util
{

    /// <summary>
    /// Provides a method to rsolve the current .NET SDK.
    /// </summary>
    public static class DotNetSdkResolver
    {

        public static string ResolvePath(string dotnetExePath)
        {
            var output = GetInfo(dotnetExePath ?? "dotnet");
            if (output == null || output.Count == 0)
                return null;

            // parse output for base path
            var basePath = ParseBasePath(output) ?? ParseInstalledSdksPath(output);
            if (string.IsNullOrWhiteSpace(basePath))
                return null;

            return basePath;
        }

        /// <summary>
        /// Invokes the 'dotnet' exe and captures its output.
        /// </summary>
        /// <param name="dotnetExePath"></param>
        /// <returns></returns>
        static IList<string> GetInfo(string dotnetExePath)
        {
            // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
            // running 'dotnet --info'. Otherwise, we may get localized results
            // Also unset some MSBuild variables, see https://github.com/OmniSharp/omnisharp-roslyn/blob/df160f86ce906bc566fe3e04e38a4077bd6023b4/src/OmniSharp.Abstractions/Services/DotNetCliService.cs#L36
            var envv = new Dictionary<string, string>
            {
                ["DOTNET_CLI_UI_LANGUAGE"] = "en-US",
                ["MSBUILD_EXE_PATH"] = null,
                ["COREHOST_TRACE"] = "0",
                ["MSBuildExtensionsPath"] = null,
            };

            // execute dotnet --info, capture output as binary, without stream reader, to prevent deadlocks
            var psi = new ProcessStartInfo(dotnetExePath);
            psi.Arguments = "--info";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            // start process and append output lines to buffer
            using var prc = new Process();
            var buf = new List<string>();
            prc.StartInfo = psi;
            prc.EnableRaisingEvents = true;
            prc.OutputDataReceived += (s, a) => buf.Add(a.Data);
            prc.Start();
            prc.BeginOutputReadLine();
            prc.WaitForExit(2000);
            return buf;
        }

        /// <summary>
        /// Searches the 'dotnet' executable output for the base path.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        static string ParseBasePath(IList<string> lines)
        {
            foreach (var line in lines.Where(x => x != null))
            {
                var colonIndex = line.IndexOf(':');
                if (colonIndex >= 0 && line.Substring(0, colonIndex).Trim().Equals("Base Path", StringComparison.OrdinalIgnoreCase))
                {
                    var basePath = line.Substring(colonIndex + 1).Trim();

                    // Make sure the base path matches the runtime architecture if on Windows
                    // Note that this only works for the default installation locations under "Program Files"
                    if (basePath.Contains(@"\Program Files\") && Environment.Is64BitProcess == false)
                    {
                        var newBasePath = basePath.Replace(@"\Program Files\", @"\Program Files (x86)\");
                        if (Directory.Exists(newBasePath))
                            basePath = newBasePath;
                    }
                    else if (basePath.Contains(@"\Program Files (x86)\") && Environment.Is64BitProcess)
                    {
                        var newBasePath = basePath.Replace(@"\Program Files (x86)\", @"\Program Files\");
                        if (Directory.Exists(newBasePath))
                            basePath = newBasePath;
                    }

                    return basePath;
                }
            }

            return null;
        }

        /// <summary>
        /// Parse the 'dotnet' executable output for the fallback path by considering SDKs.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static string ParseInstalledSdksPath(IList<string> lines)
        {
            var index = lines.IndexOf(".NET SDKs installed:");
            if (index == -1)
            {
                index = lines.IndexOf(".NET Core SDKs installed:");
                if (index == -1)
                    return null;
            }

            index++;

            while (true)
            {
                if (index >= lines.Count - 1)
                    throw new InvalidOperationException("Could not find the .NET SDK.");

                // Not a version number or an empty string?
                var temp = lines[index].Trim();
                if (string.IsNullOrWhiteSpace(temp) || !char.IsDigit(temp[0]))
                {
                    index--;
                    break;
                }

                index++;
            }

            var segments = lines[index]
                .Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray();

            return $"{segments[1]}{Path.DirectorySeparatorChar}{segments[0]}{Path.DirectorySeparatorChar}";
        }

    }

}
