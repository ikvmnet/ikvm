using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if FIRST_PASS == false

using java.lang;
using java.util.concurrent;

namespace IKVM.Runtime.Util.Java.Util.Concurrent
{

    /// <summary>
    /// <see cref="TaskScheduler"/> implementation that executes tasks on a <see cref="Executor"/>.
    /// </summary>
    class ExecutorTaskScheduler : TaskScheduler
    {

        /// <summary>
        /// Runnable implementation that encapsulates the execution of a task.
        /// </summary>
        class TaskRunnable : Runnable
        {

            readonly ExecutorTaskScheduler scheduler;
            readonly Task task;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="scheduler"></param>
            /// <param name="task"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public TaskRunnable(ExecutorTaskScheduler scheduler, Task task)
            {
                this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
                this.task = task ?? throw new ArgumentNullException(nameof(task));
            }

            public void run()
            {
                scheduler.TryExecuteTask(task);
            }

        }

        readonly Executor executor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="executor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExecutorTaskScheduler(Executor executor)
        {
            this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        protected override void QueueTask(Task task)
        {
            executor.execute(new TaskRunnable(this, task));
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Array.Empty<Task>();
        }

    }

}

#endif