using System.Security.Cryptography;

namespace IKVM.Java.Externs.sun.security.provider
{

    /// <summary>
    /// Implements the native backend for 'NativeSeedGenerator'. This implementation uses the .NET RandomNumberGenerator.
    /// </summary>
    static class NativeSeedGenerator
    {

        static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Implements the native method 'getSeedBytes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static void getSeedBytes(object self, byte[] result)
        {
            rng.GetBytes(result);
        }

    }

}
