using System;

namespace java.nio.channels
{

    /// <summary>
    /// An implementation of a <see cref="CompletionHandler"/> that accepts a delegate.
    /// </summary>
    public class DelegateCompletionHandler : DelegateCompletionHandler<object>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onCompleted"></param>
        /// <param name="onFailed"></param>
        public DelegateCompletionHandler(Action<object> onCompleted, Action<Exception> onFailed) :
            base(onCompleted, onFailed)
        {

        }

    }

    /// <summary>
    /// An implementation of a <see cref="CompletionHandler"/> that accepts a delegate.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class DelegateCompletionHandler<V> : DelegateCompletionHandler<V, object>
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onCompleted"></param>
        /// <param name="onFailed"></param>
        public DelegateCompletionHandler(Action<V> onCompleted, Action<Exception> onFailed) :
            base((r, a) => onCompleted(r), (e, a) => onFailed(e))
        {

        }

    }

    /// <summary>
    /// An implementation of a <see cref="CompletionHandler"/> that accepts a delegate.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public class DelegateCompletionHandler<V, A> : CompletionHandler
    {

        readonly Action<V, A> onCompleted;
        readonly Action<Exception, A> onFailed;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onCompleted"></param>
        /// <param name="onFailed"></param>
        public DelegateCompletionHandler(Action<V, A> onCompleted, Action<Exception, A> onFailed)
        {
            this.onCompleted = onCompleted;
            this.onFailed = onFailed;
        }

        /// <summary>
        /// Invoked when the handler is signaled completion.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.completed(object result, object attachment)
        {
            onCompleted((V)result, (A)attachment);
        }

        /// <summary>
        /// Invoked when the handler is signaled failure.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="attachment"></param>
        void CompletionHandler.failed(Exception exc, object attachment)
        {
            onFailed(exc, (A)attachment);
        }

    }

}
