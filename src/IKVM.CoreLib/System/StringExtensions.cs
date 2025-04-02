namespace System
{

    internal static class StringExtensions
    {

#if NETFRAMEWORK

        /// <summary>
        /// Returns a value indicating whether a specified character occurs within this string.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Contains(this string self, char value)
        {
            return self.IndexOf(value) != -1;
        }

#endif

    }

}
