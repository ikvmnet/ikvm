using System;
using System.Threading.Tasks;

#if FIRST_PASS == false

using java.util.concurrent;

#endif

namespace IKVM.Runtime.Util.Java.Util.Concurrent
{

#if FIRST_PASS == false

    /// <summary>
    /// Implements the Java Future interface around a .NET Task.
    /// </summary>
    class TaskFuture<T> : Future
    {

        readonly Task<T> task;
        readonly Func<bool> cancellation;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TaskFuture(Task<T> task, Func<bool> cancellation)
        {
            this.task = task ?? throw new ArgumentNullException(nameof(task));
            this.cancellation = cancellation ?? throw new ArgumentNullException(nameof(cancellation));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TaskFuture(Task<T> task)
        {
            this.task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public bool cancel(bool mayInterruptIfRunning)
        {
            return cancellation != null && mayInterruptIfRunning != false && cancellation();
        }

        public bool isCancelled()
        {
            return task.IsCanceled;
        }

        public bool isDone()
        {
            return task.IsCompleted;
        }

        public object get()
        {
            try
            {
                return task.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException e)
            {
                throw new CancellationException(e.Message);
            }
        }

        public object get(long timeout, TimeUnit unit)
        {
            try
            {
                if (task.Wait(TimeSpan.FromMilliseconds(unit.toMillis(timeout))) == false)
                    throw new global::java.util.concurrent.TimeoutException();

                return get();
            }
            catch (OperationCanceledException e)
            {
                throw new CancellationException(e.Message);
            }
        }

    }

#endif

}
