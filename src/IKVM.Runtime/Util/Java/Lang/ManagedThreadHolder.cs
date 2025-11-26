using System;
using System.Threading;

namespace IKVM.Runtime.Util.Java.Lang
{

    /// <summary>
    /// Holds a managed thread.
    /// </summary>
    public sealed class ManagedThreadHolder : ThreadHolder
    {

        /// <summary>
        /// Starts a new managed thread and returns the holder.
        /// </summary>
        /// <param name="threadStart"></param>
        /// <param name="stackSize"></param>
        /// <param name="name"></param>
        /// <param name="isBackground"></param>
        /// <param name="priority"></param>
        /// <param name="apartmentState"></param>
        /// <returns></returns>
        public static ThreadHolder Start(ThreadStart threadStart, int stackSize, string name, bool isBackground, ThreadPriority priority, ApartmentState apartmentState)
        {
            var thread = new Thread(threadStart, stackSize);
            thread.Name = name;
            thread.IsBackground = isBackground;
            thread.Priority = priority;
            thread.SetApartmentState(apartmentState);
            thread.Start();
            return new ManagedThreadHolder(thread);
        }

        readonly Thread _thread;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedThreadHolder(Thread thread)
        {
            _thread = thread ?? throw new ArgumentNullException(nameof(thread));
        }

        /// <inheritdoc />
        public override Thread GetThread() => _thread;

    }

}
