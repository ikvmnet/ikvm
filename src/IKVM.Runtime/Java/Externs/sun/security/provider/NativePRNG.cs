using System;
using System.Security.Cryptography;

namespace IKVM.Java.Externs.sun.security.provider
{

    /// <summary>
    /// Implements the native backend for 'NativePRNG'. This implementation provides seeds using <see cref="RandomNumberGenerator"/>, and then progressive random values using <see cref="Random"/>.
    /// </summary>
    static class NativePRNG
    {

        static RandomNumberGenerator rng = RandomNumberGenerator.Create();
        static Random rnd = new Random();

        /// <summary>
        /// Implements the native method 'engineSetSeed'. This method initializes the pseudo random generator with the given seed value.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="seed"></param>
        public static void engineSetSeed(object self, byte[] seed)
        {
            int s = 0;
            for (int i = 0; i < seed.Length; i++)
                s ^= seed[i];

            rnd = new Random(s);
        }

        /// <summary>
        /// Implements the native method 'engineNextBytes'. This method gets a number of pseudo random bytes.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        public static void engineNextBytes(object self, byte[] bytes)
        {
            rnd.NextBytes(bytes);
        }

        /// <summary>
        /// Implements the native method 'engineGenerateSeed'. This method generates a number of true random bytes.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="numBytes"></param>
        /// <returns></returns>
        public static byte[] engineGenerateSeed(object self, int numBytes)
        {
            var b = new byte[numBytes];
            rng.GetBytes(b);
            return b;
        }

    }

}