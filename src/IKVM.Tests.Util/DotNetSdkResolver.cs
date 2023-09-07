using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using CliWrap;

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
        static List<string> GetInfo(string dotnetExePath)
        {
            // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
            // running 'dotnet --info'. Otherwise, we may get localized results
            // Also unset some MSBuild variables, see https://github.com/OmniSharp/omnisharp-roslyn/blob/df160f86ce906bc566fe3e04e38a4077bd6023b4/src/OmniSharp.Abstractions/Services/DotNetCliService.cs#L36
            var environmentVariables = new Dictionary<string, string>
            {
                ["DOTNET_CLI_UI_LANGUAGE"] = "en-US",
                ["MSBUILD_EXE_PATH"] = null,
                ["COREHOST_TRACE"] = "0",
                ["MSBuildExtensionsPath"] = null,
            };

            var lines = new List<string>();
            var task = (Cli.Wrap(dotnetExePath)
                .WithArguments("--info")
                .WithEnvironmentVariables(environmentVariables)
                | lines.Add)
                .ExecuteAsync(new CancellationTokenSource(10000).Token);
            task.GetAwaiter().GetResult();

            return lines;
        }

        /// <summary>
        /// Searches the 'dotnet' executable output for the base path.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        static string ParseBasePath(List<string> lines)
        {
            foreach (var line in lines.Where(x => x != null))
            {
                var colonIndex = line.IndexOf(':');
                if (colonIndex >= 0 && line.Substring(0, colonIndex).Trim().Equals("Base Path", StringComparison.OrdinalIgnoreCase))
                {
                    var basePath = line.Substring(colonIndex + 1).Trim();

                    // Make sure the base path matches the runtime architecture if on Windows
                    // Note that this only works for the default installation locations under "Program Files"
                    if (basePath.Contains(@"\Program Files\") && !System.Environment.Is64BitProcess)
                    {
                        var newBasePath = basePath.Replace(@"\Program Files\", @"\Program Files (x86)\");
                        if (Directory.Exists(newBasePath))
                            basePath = newBasePath;
                    }
                    else if (basePath.Contains(@"\Program Files (x86)\") && System.Environment.Is64BitProcess)
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
        static string ParseInstalledSdksPath(List<string> lines)
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
