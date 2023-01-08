namespace IKVM.Runtime.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDSASignature'.
    /// </summary>
    static class ECDSASignature
    {

        /// <summary>
        /// Implements the native method for 'signDigest'.
        /// </summary>
        /// <param name="digest"></param>
        /// <param name="s"></param>
        /// <param name="encodedParams"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static byte[] signDigest(byte[] digest, byte[] s, byte[] encodedParams, byte[] seed)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return IKVM.Java.Externs.Impl.sun.security.ec.ECDSASignature.signDigest(digest, s, encodedParams, seed);
#endif
        }

        /// <summary>
        /// Implements the native method for 'verifySignedDigest'.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="digest"></param>
        /// <param name="w"></param>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        public static bool verifySignedDigest(byte[] signature, byte[] digest, byte[] w, byte[] encodedParams)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return IKVM.Java.Externs.Impl.sun.security.ec.ECDSASignature.verifySignedDigest(signature, digest, w, encodedParams);
#endif
        }

    }

}
