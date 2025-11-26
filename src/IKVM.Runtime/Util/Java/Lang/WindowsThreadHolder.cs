using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Runtime.Util.Java.Lang
{

    /// <summary>
    /// Implementation of <see cref="ThreadHolder"/> that holds a reference to a native Win32 thread.
    /// </summary>
    public sealed class WindowsThreadHolder : ThreadHolder
    {

        /// <summary>
        /// Starts a new Windows thread and returns the holder.
        /// </summary>
        /// <param name="threadStart"></param>
        /// <param name="stackSize"></param>
        /// <param name="name"></param>
        /// <param name="isBackground"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static ThreadHolder Start(ThreadStart threadStart, int stackSize, string name, bool isBackground, ThreadPriority priority)
        {
            return new WindowsThreadHolder(threadStart, stackSize, name, isBackground, priority);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        unsafe static extern uint CreateThread(uint* lpThreadAttributes, uint dwStackSize, ThreadStart lpStartAddress, uint* lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        unsafe static extern uint CloseHandle(nuint hObject);

        readonly ThreadStart _entry;
        readonly TaskCompletionSource<Thread> _tcs = new TaskCompletionSource<Thread>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        unsafe WindowsThreadHolder(ThreadStart threadStart, int stackSize, string name, bool isBackground, ThreadPriority priority)
        {
            // we hold the delegate in _entry so it does not get GCd
            _entry = () =>
            {
                var t = Thread.CurrentThread;
                t.Name = name;
                t.IsBackground = isBackground;
                t.Priority = priority;
                _tcs.SetResult(t);
                threadStart();
            };

            var i = 0u;
            var h = CreateThread(null, (uint)stackSize, _entry, null, 0, out _);
            if (h == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            // handle is not needed
            if (CloseHandle(h) != 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public override Thread GetThread()
        {
            return _tcs.Task.GetAwaiter().GetResult();
        }

    }

}
