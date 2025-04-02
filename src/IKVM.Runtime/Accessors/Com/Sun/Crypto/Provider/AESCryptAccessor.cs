namespace IKVM.Runtime.Accessors.Com.Sun.Crypto.Provider;

#if !(FIRST_PASS || EXPORTER || IMPORTER)

internal sealed class AESCryptAccessor(AccessorTypeResolver resolver)
    : Accessor<com.sun.crypto.provider.AESCrypt>(resolver, "com.sun.crypto.provider.AESCrypt")
{
    private FieldAccessor<com.sun.crypto.provider.AESCrypt, int[]> k;

    public int[] K(com.sun.crypto.provider.AESCrypt self) => GetField(ref k, "K").GetValue(self);
}

#endif