using System;
using System.Threading;
using System.Threading.Tasks;

#if FIRST_PASS == false

using java.util.concurrent;

#endif

namespace IKVM.Runtime.Util.Java.Util.Concurrent
{

    static class TaskExtensions
    {

#if FIRST_PASS == false

        public static Future ToFuture<T>(this Task<T> task, Func<bool> cancellation)
        {
            return new TaskFuture<T>(task, cancellation);
        }

        public static Future ToFuture<T>(this Task<T> task, CancellationTokenSource cancellationTokenSource)
        {
            return new TaskFuture<T>(task, () => { cancellationTokenSource.Cancel(); return true; });
        }

        public static Future ToFuture<T>(this Task<T> task)
        {
            return new TaskFuture<T>(task);
        }

#endif

    }

}
