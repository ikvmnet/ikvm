using System;

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECKeyPairGenerator'.
    /// </summary>
    static class ECKeyPairGenerator
    {

        /// <summary>
        /// Implements the native method for 'generateECKeyPair'.
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="encodedParams"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static object[] generateECKeyPair(int keySize, byte[] encodedParams, byte[] seed)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return IKVM.Java.Externs.Impl.sun.security.ec.ECKeyPairGenerator.generateECKeyPair(keySize, encodedParams, seed);
#endif
        }

    }

}
