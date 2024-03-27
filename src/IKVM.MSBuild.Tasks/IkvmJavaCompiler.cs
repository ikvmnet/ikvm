using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using com.sun.tools.javac.util;

using java.io;
using java.lang;
using java.util;

using javax.tools;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Executes the Java compiler.
    /// </summary>
    public class IkvmJavaCompiler : IkvmAsyncTask
    {

        /// <summary>
        /// Implements <see cref="DiagnosticListener"/>.
        /// </summary>
        class MSBuildDiagnosticListener : DiagnosticListener
        {

            readonly TaskLoggingHelper log;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="log"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public MSBuildDiagnosticListener(TaskLoggingHelper log)
            {
                this.log = log ?? throw new ArgumentNullException(nameof(log));
            }

            public void report(Diagnostic diagnostic)
            {
                var kind = diagnostic.getKind();
                var code = diagnostic.getCode();
                var source = diagnostic.getSource() is DiagnosticSource f ? f.getFile().toUri().getPath() : null;
                var startPosition = (int)diagnostic.getStartPosition();
                var columnNumber = (int)diagnostic.getColumnNumber();
                var endPosition = (int)diagnostic.getEndPosition();
                var message = diagnostic.getMessage(Locale.getDefault()) ?? "unknown";

                if (kind == Diagnostic.Kind.ERROR)
                    log.LogError(null, code, null, source, startPosition, columnNumber, endPosition, -1, message);
                else if (kind == Diagnostic.Kind.WARNING || kind == Diagnostic.Kind.MANDATORY_WARNING)
                    log.LogWarning(null, code, null, source, startPosition, columnNumber, endPosition, -1, message);
                else
                    log.LogMessage(null, code, null, source, startPosition, columnNumber, endPosition, -1, MessageImportance.Normal, message);
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmJavaCompiler()
        {

        }

        /// <summary>
        /// Classpath for compiler.
        /// </summary>
        [Required]
        public ITaskItem[] Classpath { get; set; }

        /// <summary>
        /// Source files to compile.
        /// </summary>
        [Required]
        public ITaskItem[] Sources { get; set; }

        /// <summary>
        /// Path to place the generated class files.
        /// </summary>
        [Required]
        public string Destination { get; set; }

        /// <summary>
        /// Path to place the generated JNI header files.
        /// </summary>
        public string HeaderDestination { get; set; }

        /// <summary>
        /// Whether to enable debug output of the compiler.
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// If <c>true</c> disables warning messages.
        /// </summary>
        public bool NoWarn { get; set; }

        /// <summary>
        /// Whether to enable verbose output of the compiler.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Source version.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Target version.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Encoding.
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// Executes the Java compiler.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (Classpath != null)
                foreach (var i in Classpath)
                    if (i.ItemSpec != null)
                        i.ItemSpec = System.IO.Path.GetFullPath(i.ItemSpec);

            if (Sources != null)
                foreach (var i in Sources)
                    if (i.ItemSpec != null)
                        i.ItemSpec = System.IO.Path.GetFullPath(i.ItemSpec);

            if (Destination != null)
                Destination = System.IO.Path.GetFullPath(Destination);

            if (HeaderDestination != null)
                HeaderDestination = System.IO.Path.GetFullPath(HeaderDestination);

            return base.Execute();
        }

        /// <summary>
        /// Execute the Java compiler in a separate thread.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override System.Threading.Tasks.Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.Run(Compile);
        }

        /// <summary>
        /// Executes the compiler.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="IkvmTaskException"></exception>
        bool Compile()
        {
            var diagnostics = new MSBuildDiagnosticListener(Log);
            var javacArgLog = new List<string>();

            var compiler = ToolProvider.getSystemJavaCompiler();
            if (compiler == null)
                throw new IkvmTaskException("Could not find Java compiler.");

            var fileManager = compiler.getStandardFileManager(diagnostics, null, null);
            if (fileManager == null)
                throw new IkvmTaskException("Could not find standard file manager.");

            if (string.IsNullOrWhiteSpace(Destination))
                throw new IkvmTaskException("Invalid destination.");

            // create destination directory
            var destination = new File(Destination);
            destination.mkdirs();

            // set output path
            fileManager.setLocation(StandardLocation.CLASS_OUTPUT, Collections.singletonList(destination));
            javacArgLog.Add($"-d");
            javacArgLog.Add($"\"{string.Join(System.IO.Path.PathSeparator.ToString(), fileManager.getLocation(StandardLocation.CLASS_OUTPUT).AsEnumerable<File>().Select(i => i.getPath()))}\"");

            if (string.IsNullOrWhiteSpace(HeaderDestination) == false)
            {
                // create destination directory
                var headerDestination = new File(HeaderDestination);
                headerDestination.mkdirs();

                // set header output path
                fileManager.setLocation(StandardLocation.NATIVE_HEADER_OUTPUT, Collections.singletonList(headerDestination));
                javacArgLog.Add($"-h");
                javacArgLog.Add($"\"{string.Join(System.IO.Path.PathSeparator.ToString(), fileManager.getLocation(StandardLocation.NATIVE_HEADER_OUTPUT).AsEnumerable<File>().Select(i => i.getPath()))}\"");
            }

            // get set of source items to compile
            var compilationUnits = fileManager.getJavaFileObjectsFromFiles(Arrays.asList(Sources.Select(i => new File(i.ItemSpec)).Cast<object>().ToArray()));
            foreach (var i in Sources)
                javacArgLog.Add($"\"{System.IO.Path.GetFullPath(i.ItemSpec)}\"");

            // build options set
            var options = new ArrayList();

            if (string.Equals(Debug, "all", StringComparison.OrdinalIgnoreCase))
            {
                options.add("-g");
            }
            else if (!string.IsNullOrWhiteSpace(Debug))
            {
                options.add($"-g:{Debug.ToLowerInvariant()}");
            }

            if (NoWarn)
            {
                if (compiler.isSupportedOption("-nowarn") >= 0)
                    options.add("-nowarn");
                else
                    Log.LogWarning("Unsupported option '-nowarn'. Ignoring.");
            }

            if (Verbose)
            {
                if (compiler.isSupportedOption("-verbose") >= 0)
                    options.add("-verbose");
                else
                    Log.LogWarning("Unsupported option '-verbose'. Ignoring.");
            }

            if (Source != null)
            {
                if (compiler.isSupportedOption("-source") == 1)
                {
                    options.add($"-source");
                    options.add(Source);
                }
                else
                    Log.LogWarning("Unsupported option '-source'. Ignoring.");
            }

            if (Target != null)
            {
                if (compiler.isSupportedOption("-target") == 1)
                {
                    options.add($"-target");
                    options.add(Target);
                }
                else
                    Log.LogWarning("Unsupported option '-target'. Ignoring.");
            }

            javacArgLog.AddRange(options.AsEnumerable<string>());

            // apply classpath
            if (Classpath != null && Classpath.Length > 0)
            {
                var cp = new ArrayList();
                foreach (var i in Classpath)
                    cp.add(new File(i.ItemSpec));

                fileManager.setLocation(StandardLocation.CLASS_PATH, cp);
                javacArgLog.Add("-cp");
                javacArgLog.Add($"\"{string.Join(System.IO.Path.PathSeparator.ToString(), fileManager.getLocation(StandardLocation.CLASS_PATH).AsEnumerable<File>().Select(i => i.getPath()))}\"");
            }

            // emit log about command
            Log.LogMessage(MessageImportance.Low, $"javac {string.Join(" ", javacArgLog)}");

            // initiate compiler and wait for result
            var rsl = compiler.getTask(null, fileManager, diagnostics, options.size() > 0 ? options : null, null, compilationUnits).call();

            // result indicates status
            return rsl?.booleanValue() ?? false;
        }

    }

}
