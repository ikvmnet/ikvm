using System;

using java.lang;

namespace IKVM.Java.Extensions.java.lang
{
    public class DelegateRunnable : Runnable
    {

        readonly Action onRun;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="onRun"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateRunnable(Action onRun)
        {
            this.onRun = onRun ?? throw new ArgumentNullException(nameof(onRun));
        }

        public void run()
        {
            onRun();
        }

    }

}
