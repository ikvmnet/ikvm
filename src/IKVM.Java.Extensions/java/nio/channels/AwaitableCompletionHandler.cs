using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace java.nio.channels
{

    /// <summary>
    /// An awaitable implementation of a <see cref="CompletionHandler"/>.
    /// </summary>
    public class AwaitableCompletionHandler : AwaitableCompletionHandler<object>
    {

        /// <summary>
        /// Gets the task that can be awaited.
        /// </summary>
        public new Task Task => base.Task;

        /// <summary>
        /// Gets an awaiter used to await this completion handler.
        /// </summary>
        public new TaskAwaiter GetAwaiter() => Task.GetAwaiter();

    }

    /// <summary>
    /// An awaitable implementation of a <see cref="CompletionHandler"/>.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class AwaitableCompletionHandler<V> : AwaitableCompletionHandler<V, object>
    {



    }

    /// <summary>
    /// An awaitable implementation of a <see cref="CompletionHandler"/>.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class AwaitableCompletionHandler<V, A> : CompletionHandler
    {

        readonly TaskCompletionSource<V> source = new TaskCompletionSource<V>();
        A attachment;

        /// <summary>
        /// Gets the attachment which is set after the handler completes.
        /// </summary>
        public A Attachment => attachment;

        /// <summary>
        /// Gets the task that can be awaited.
        /// </summary>
        public Task<V> Task => source.Task;

        /// <summary>
        /// Gets an awaiter used to await this completion handler.
        /// </summary>
        public new TaskAwaiter<V> GetAwaiter() => Task.GetAwaiter();

        /// <summary>
        /// Invoked when the handler is signaled completion.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.completed(object result, object attachment)
        {
            this.attachment = (A)attachment;
            source.SetResult((V)result);
        }

        /// <summary>
        /// Invoked when the handler is signaled failure.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.failed(Exception exc, object attachment)
        {
            this.attachment = (A)attachment;
            source.SetException(exc);
        }

    }

}
