namespace IKVM.MSBuild.Tasks
{
    using System;
    using System.IO;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    using CliWrap;
    using CliWrap.Builders;

    using Microsoft.Build.Framework;

    /// <summary>
    /// Task to convert a Windows path to a WSL path.
    /// </summary>
    public class IkvmWslPath : IkvmAsyncTask
    {

        /// <summary>
        /// Path to convert.
        /// </summary>
        [Required]
        [Output]
        public string Path { get; set; }

        /// <summary>
        /// Optional name of the distro to use.
        /// </summary>
        public string DistributionName { get; set; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmWslPath()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        public IkvmWslPath(ResourceManager taskResources) :
            base(taskResources)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        /// <param name="helpKeywordPrefix"></param>
        public IkvmWslPath(ResourceManager taskResources, string helpKeywordPrefix) :
            base(taskResources, helpKeywordPrefix)
        {

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            var cmd = Cli.Wrap(GetWslExePath())
                .WithArguments(AddArguments)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(TrySetPath));

            // start at recorded working directory in case of relative path
            if (string.IsNullOrEmpty(CurrentWorkingDirectory) == false && Directory.Exists(CurrentWorkingDirectory) == true)
                cmd = cmd.WithWorkingDirectory(CurrentWorkingDirectory);

            Log.LogCommandLine(cmd.ToString());
            var exe = await cmd.ExecuteAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Gets the path to wsl.exe based on the current environment.
        /// </summary>
        /// <returns></returns>
        string GetWslExePath()
        {
            return Environment.ExpandEnvironmentVariables(RuntimeInformation.ProcessArchitecture == Architecture.X86 ? @"%SystemRoot%\Sysnative\wsl.exe" : @"%SystemRoot%\System32\wsl.exe");
        }

        /// <summary>
        /// Attempts to set the path.
        /// </summary>
        /// <param name="arg"></param>
        void TrySetPath(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg) == false)
                Path = arg;
        }

        /// <summary>
        /// Adds the arguments for 'wslpath'.
        /// </summary>
        /// <param name="builder"></param>
        void AddArguments(ArgumentsBuilder builder)
        {
            if (string.IsNullOrEmpty(DistributionName) == false)
                builder.Add("-d").Add(DistributionName);

            builder.Add("-e");
            builder.Add("wslpath");
            builder.Add(Path.Replace("\\", "\\\\"), true);
        }

    }

}
