namespace IKVM.Java.Externs.java.util.concurrent.atomic
{

    /// <summary>
    /// Implements the native backend for 'AtomicLong'.
    /// </summary>
    static class AtomicLong
    {

        /// <summary>
        /// Implements the native method for 'VMSupportsCS8'. We return <c>true</c> since the implementation uses
        /// Unsafe methods which we have implemented ontop of Interlocked for long and double values.
        /// </summary>
        /// <returns></returns>
        public static bool VMSupportsCS8()
        {
            return true;
        }

    }

}
