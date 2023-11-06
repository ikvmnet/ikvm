namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Resources;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Build.Framework;

    /// <summary>
    /// IKVM specific task type that handles async operations.
    /// </summary>
    public abstract class IkvmAsyncTask : Microsoft.Build.Utilities.Task, ICancelableTask
    {

        readonly CancellationTokenSource cts;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        protected IkvmAsyncTask() :
            base()
        {
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        protected IkvmAsyncTask(ResourceManager taskResources) :
            base(taskResources)
        {
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="taskResources"></param>
        /// <param name="helpKeywordPrefix"></param>
        protected IkvmAsyncTask(ResourceManager taskResources, string helpKeywordPrefix) :
            base(taskResources, helpKeywordPrefix)
        {
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (cts.IsCancellationRequested)
                return false;

            // wait for result, and ensure we reacquire in case of return value or exception
            Task<bool> run;

            try
            {
                // kick off the launcher with the configured options
                run = ExecuteAsync(cts.Token);
                if (run.IsCompleted)
                    return run.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                return false;
            }

            try
            {
                if (run.Wait(TimeSpan.FromSeconds(5)))
                    if (run.IsCompleted)
                        return run.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                return false;
            }

            // yield and wait for the task to complete
            BuildEngine3.Yield();

            try
            {
                return run.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            finally
            {
                BuildEngine3.Reacquire();
            }
        }

        /// <summary>
        /// Signals the task to cancel.
        /// </summary>
        public void Cancel()
        {
            cts.Cancel();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> ExecuteAsync(CancellationToken cancellationToken);

    }

}
