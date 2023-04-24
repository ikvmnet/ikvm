using System;
using System.Threading;
using System.Threading.Tasks;

#if FIRST_PASS == false

using java.lang;
using java.util.concurrent;

namespace IKVM.Runtime.Util.Java.Util.Concurrent
{

    /// <summary>
    /// <see cref="TaskScheduler"/> implementation that executes tasks on a <see cref="Executor"/>.
    /// </summary>
    class ExecutorSynchronizationContext : SynchronizationContext
    {

        /// <summary>
        /// Runnable implementation that encapsulates the execution of a task.
        /// </summary>
        class TaskRunnable : Runnable
        {

            readonly ExecutorSynchronizationContext self;
            readonly SendOrPostCallback post;
            readonly object state;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="self"></param>
            /// <param name="post"></param>
            /// <param name="state"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public TaskRunnable(ExecutorSynchronizationContext self, SendOrPostCallback post, object state)
            {
                this.self = self ?? throw new ArgumentNullException(nameof(self));
                this.post = post ?? throw new ArgumentNullException(nameof(post));
                this.state = state;
            }

            public void run()
            {
                var save = Current;

                try
                {
                    SetSynchronizationContext(self);
                    post(state);
                }
                finally
                {
                    SetSynchronizationContext(save);
                }
            }

        }

        readonly Executor executor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="executor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExecutorSynchronizationContext(Executor executor)
        {
            this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            executor.execute(new TaskRunnable(this, d, state));
        }

    }

}

#endif