using System.Threading;
using System.Threading.Tasks;

#if FIRST_PASS == false

using java.util.concurrent;

namespace IKVM.Runtime.Util.Java.Util.Concurrent
{

    static class ExecutorExtensions
    {

        /// <summary>
        /// Gets a <see cref="TaskScheduler"/> implementation that dispatches to the given <see cref="Executor"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static SynchronizationContext ToSynchronizationContext(this Executor self)
        {
            return new ExecutorSynchronizationContext(self);
        }

        /// <summary>
        /// Gets a <see cref="TaskScheduler"/> implementation that dispatches to the given <see cref="Executor"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TaskScheduler ToTaskScheduler(this Executor self)
        {
            return new ExecutorTaskScheduler(self);
        }

    }

}

#endif
