package sun.nio.ch;

import java.io.IOException;
import java.nio.channels.spi.AbstractSelector;
import java.nio.channels.spi.SelectorProvider;

public class DefaultSelectorProvider {

    private DefaultSelectorProvider() {
        
    }

    public static SelectorProvider create() {
        return new SelectorProviderImpl() {
            public AbstractSelector openSelector() throws IOException {
                return new DotNetSelectorImpl(this);
            }
        };
    }

}
