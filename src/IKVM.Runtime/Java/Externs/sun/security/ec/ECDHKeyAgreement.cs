using System;

namespace IKVM.Java.Externs.sun.security.ec
{

    /// <summary>
    /// Implements the backing native methods for 'ECDHKeyAgreement'.
    /// </summary>
    static class ECDHKeyAgreement
    {

        /// <summary>
        /// Implements the native method for 'deriveKey'.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="w"></param>
        /// <param name="encodedParams"></param>
        /// <returns></returns>
        public static byte[] deriveKey(byte[] s, byte[] w, byte[] encodedParams)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return IKVM.Java.Externs.Impl.sun.security.ec.ECDHKeyAgreement.deriveKey(s, w, encodedParams);
#endif
        }

    }

}
