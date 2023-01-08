namespace IKVM.Java.Externs.sun.security.ec
{

#if NET47_OR_GREATER || NETCOREAPP
#else

    struct ECPoint
    {

        public byte[] X;
        public byte[] Y;

    }

#endif

}
