package sun.security.provider;

import java.io.IOException;

/**
 * Seed generator for IKVM making use of native .NET features.
 *
 */
class NativeSeedGenerator extends SeedGenerator {

    /**
     * Create a new native seed generator instances.
     *
     * @exception IOException if native seeds are not available
     * on this platform.
     */
    NativeSeedGenerator(String seedFile) throws IOException {
        super();
    }

    /**
     * Native method to do the actual work.
     */
    private static native boolean nativeGenerateSeed(byte[] result);

    @Override
    void getSeedBytes(byte[] result) {
        if (nativeGenerateSeed(result) == false) {
            throw new InternalError("Unexpected failure generating seed");
        }
    }

}
