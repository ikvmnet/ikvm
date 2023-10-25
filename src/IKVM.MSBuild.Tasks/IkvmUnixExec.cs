namespace IKVM.MSBuild.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using CliWrap;
    using CliWrap.Builders;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Task to execute a Unix shell script.
    /// </summary>
    public class IkvmUnixExec : IkvmAsyncTask
    {

        /// <summary>
        /// Script to be run on a Unix shell.
        /// </summary>
        [Required]
        public string Command { get; set; }

        /// <summary>
        /// Optionally, additional arguments to add to the command. If empty, the Command will be treated as a single command line.
        /// </summary>
        public ITaskItem[] Arguments { get; set; }

        /// <summary>
        /// If on Windows, allow WSL to be used. Paths and commands need to be previously converted to Unix paths.
        /// </summary>
        public bool UseWsl { get; set; } = true;

        /// <summary>
        /// Optionally the distribution to use.
        /// </summary>
        public string WslDistribution { get; set; }

        /// <summary>
        /// Path to the working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Gets the set of environmental variables to apply. Each ItemSpec represents a name, with the Value metadata representing a value.
        /// </summary>
        public ITaskItem[] EnvironmentVariables { get; set; }

        /// <summary>
        /// Whether or not to capture the standard output of the command.
        /// </summary>
        public bool RedirectStandardOutput { get; set; } = false;

        /// <summary>
        /// Whether or not to log standard output of the command.
        /// </summary>
        public bool LogStandardOutput { get; set; } = true;

        /// <summary>
        /// Log level at which to emit standard output messages.
        /// </summary>
        public string StandardOutputLogLevel { get; set; }

        /// <summary>
        /// Whether or not to capture the standard error of the command.
        /// </summary>
        public bool RedirectStandardError { get; set; } = false;

        /// <summary>
        /// Whether or not to log standard error of the command.
        /// </summary>
        public bool LogStandardError { get; set; } = true;

        /// <summary>
        /// Log level at which to emit standard error messages.
        /// </summary>
        public string StandardErrorLogLevel { get; set; }

        /// <summary>
        /// Lines to provide as standard input to the command.
        /// </summary>
        public List<ITaskItem> StandardInput { get; set; }

        /// <summary>
        /// Resulting standard output lines.
        /// </summary>
        [Output]
        public List<ITaskItem> StandardOutput { get; set; }

        /// <summary>
        /// Resulting standard error lines.
        /// </summary>
        [Output]
        public List<ITaskItem> StandardError { get; set; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmUnixExec()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        public IkvmUnixExec(ResourceManager taskResources) :
            base(taskResources)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        /// <param name="helpKeywordPrefix"></param>
        public IkvmUnixExec(ResourceManager taskResources, string helpKeywordPrefix) :
            base(taskResources, helpKeywordPrefix)
        {

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            // clear output lists
            StandardOutput = new List<ITaskItem>();
            StandardError = new List<ITaskItem>();

            // execute with wsl.exe
            var cli = BuildCommand();

            // add standard input to the command
            if (StandardInput != null)
            {
                // collect standard input lines as a single string
                var stdin = new StringBuilder();
                foreach (var i in StandardInput)
                    stdin.AppendLine(i.ItemSpec);

                // add final string to command
                cli = cli.WithStandardInputPipe(PipeSource.FromString(stdin.ToString()));
            }

            // capture standard output to output list
            var stdout = new List<PipeTarget>();
            if (RedirectStandardOutput)
                stdout.Add(PipeTarget.ToDelegate(s => StandardOutput.Add(new TaskItem(s))));
            if (LogStandardOutput)
                stdout.Add(PipeTarget.ToDelegate(s => LogWithLevel(StandardOutputLogLevel, s)));
            if (stdout.Count == 1)
                cli = cli.WithStandardOutputPipe(stdout[0]);
            if (stdout.Count >= 2)
                cli = cli.WithStandardOutputPipe(PipeTarget.Merge(stdout));

            // capture standard error to output list
            var stderr = new List<PipeTarget>();
            if (RedirectStandardError)
                stderr.Add(PipeTarget.ToDelegate(s => StandardError.Add(new TaskItem(s))));
            if (LogStandardError)
                stderr.Add(PipeTarget.ToDelegate(s => LogWithLevel(StandardErrorLogLevel, s)));
            if (stderr.Count == 1)
                cli = cli.WithStandardErrorPipe(stderr[0]);
            if (stderr.Count >= 2)
                cli = cli.WithStandardErrorPipe(PipeTarget.Merge(stderr));

            // log the final command line
            Log.LogMessage(MessageImportance.High, cli.ToString());

            // log the environment variables being passed
            if (cli.EnvironmentVariables.Count > 0)
            {
                var s = new StringBuilder();
                foreach (var env in cli.EnvironmentVariables)
                    s.AppendLine($"ENV {env.Key}={env.Value}");

                Log.LogMessage(MessageImportance.High, s.ToString());
            }

            // execute executable
            var exe = await cli.ExecuteAsync(cancellationToken);
            if (exe.ExitCode != 0)
            {
                Log.LogError("Non-zero exit code running Unix command: {0}", exe.ExitCode);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs the specified message at the given level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        void LogWithLevel(string level, string message)
        {
            switch (level)
            {
                case "Warning":
                    Log.LogWarning(message);
                    break;
                case "Error":
                    Log.LogError(message);
                    break;
                default:
                    Log.LogMessage(message);
                    break;
            }
        }

        /// <summary>
        /// Builds a new command.
        /// </summary>
        /// <returns></returns>
        Command BuildCommand()
        {
            var cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && UseWsl ? Cli.Wrap(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32\wsl.exe")).WithArguments(BuildWslArguments) : Cli.Wrap(Command).WithArguments(BuildArguments);
            cmd = cmd.WithEnvironmentVariables(BuildEnvironmentVariables).WithValidation(CommandResultValidation.None);
            cmd = cmd.WithWorkingDirectory(CurrentWorkingDirectory);
            return cmd;
        }

        /// <summary>
        /// Builds the arguments to execute.
        /// </summary>
        /// <param name="builder"></param>
        void BuildWslArguments(ArgumentsBuilder builder)
        {
            if (WslDistribution != null)
                builder.Add("-d").Add(WslDistribution);

            builder.Add("--");
            builder.Add(Command);
            BuildArguments(builder);
        }

        /// <summary>
        /// Builds the arguments to execute.
        /// </summary>
        /// <param name="builder"></param>
        void BuildArguments(ArgumentsBuilder builder)
        {
            if (Arguments != null)
                foreach (var i in Arguments)
                    builder.Add(i.ItemSpec);
        }

        /// <summary>
        /// Builds the environment variables.
        /// </summary>
        /// <param name="builder"></param>
        void BuildEnvironmentVariables(EnvironmentVariablesBuilder builder)
        {
            if (EnvironmentVariables == null)
                return;

            foreach (var env in EnvironmentVariables)
                if (env.ItemSpec != null)
                    builder.Set(env.ItemSpec, env.GetMetadata("Value") ?? "");

            // WSL requires a WSLENV variable to determine which values pass to the process
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && UseWsl)
            {
                // record each variable
                var wslenv = new List<string>();
                foreach (var env in EnvironmentVariables)
                    if (env.ItemSpec != null)
                        wslenv.Add(env.ItemSpec);

                builder.Set("WSLENV", string.Join(":", wslenv));
            }
        }

    }

}
