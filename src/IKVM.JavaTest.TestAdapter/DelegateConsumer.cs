using System;

namespace IKVM.JavaTest
{


    /// <summary>
    /// Implementation of <see cref="global::java.util.function.Consumer"/> that sends output to a delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DelegateConsumer<T> : global::java.util.function.Consumer
    {

        /// <summary>
        /// Joins two consumers into one.
        /// </summary>
        sealed class JoinConsumer : global::java.util.function.Consumer
        {

            readonly global::java.util.function.Consumer arg1;
            readonly global::java.util.function.Consumer arg2;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="arg1"></param>
            /// <param name="arg2"></param>
            internal JoinConsumer(global::java.util.function.Consumer arg1, global::java.util.function.Consumer arg2)
            {
                this.arg1 = arg1 ?? throw new ArgumentNullException(nameof(arg1));
                this.arg2 = arg2 ?? throw new ArgumentNullException(nameof(arg2));
            }

            public void accept(object obj0)
            {
                arg1.accept(obj0);
                arg2.accept(obj0);
            }

            public global::java.util.function.Consumer andThen(global::java.util.function.Consumer other)
            {
                return new JoinConsumer(this, other);
            }

        }

        readonly Action<T> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateConsumer(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void accept(object t)
        {
            action((T)t);
        }

        public global::java.util.function.Consumer andThen(global::java.util.function.Consumer other)
        {
            return new JoinConsumer(this, other);
        }

    }

}
