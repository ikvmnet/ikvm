namespace IKVM.MSBuild.Tasks
{
    using System;
    using System.IO;
    using System.Resources;
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
        public string Distribution { get; set; }

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
            var cli = Cli.Wrap(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32\wsl.exe"))
                .WithArguments(AddArguments)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(TrySetPath));

            // start at recorded working directory in case of relative path
            //if (string.IsNullOrEmpty(CurrentWorkingDirectory) == false && Directory.Exists(CurrentWorkingDirectory) == true)
            //    cli = cli.WithWorkingDirectory(CurrentWorkingDirectory);

            Log.LogCommandLine(cli.ToString());
            var exe = await cli.ExecuteAsync(cancellationToken);

            return true;
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
            if (Distribution != null)
                builder.Add("-d").Add(Distribution);

            builder.Add("wslpath");
            builder.Add(Path.Replace("\\", "\\\\"), true);
        }

    }

}
