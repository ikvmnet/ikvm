package sun.security.provider;

import java.io.IOException;

class NativeSeedGenerator extends SeedGenerator {

    NativeSeedGenerator(String seedFile) throws IOException {
        super();
    }

    @Override
    native void getSeedBytes(byte[] result);

}
