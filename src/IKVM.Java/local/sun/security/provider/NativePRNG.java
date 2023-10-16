package sun.security.provider;

import java.security.SecureRandomSpi;

public final class NativePRNG extends SecureRandomSpi {

    public static final class NonBlocking {
        static boolean isAvailable() {
            return false;
        }
    }

    public static final class Blocking {
        static boolean isAvailable() {
            return false;
        }
    }

    static boolean isAvailable() {
        return true;
    }

    public NativePRNG() {
        super();
    }

    @Override
    protected native void engineSetSeed(byte[] seed);

    @Override
    protected native void engineNextBytes(byte[] bytes);

    @Override
    protected native byte[] engineGenerateSeed(int numBytes);

}
