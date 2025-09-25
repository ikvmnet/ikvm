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

        readonly TaskCompletionSource<V> _source = new TaskCompletionSource<V>();
        A? _attachment;

        /// <summary>
        /// Gets the attachment which is set after the handler completes.
        /// </summary>
        public A? Attachment => _attachment;

        /// <summary>
        /// Gets the task that can be awaited.
        /// </summary>
        public Task<V> Task => _source.Task;

        /// <summary>
        /// Gets an awaiter used to await this completion handler.
        /// </summary>
        public TaskAwaiter<V> GetAwaiter() => Task.GetAwaiter();

        /// <summary>
        /// Invoked when the handler is signaled completion.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.completed(object result, object attachment)
        {
            _attachment = (A)attachment;
            _source.SetResult((V)result);
        }

        /// <summary>
        /// Invoked when the handler is signaled failure.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.failed(Exception exc, object attachment)
        {
            _attachment = (A)attachment;
            _source.SetException(exc);
        }

    }

}
