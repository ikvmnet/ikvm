package sun.nio.fs;

import java.io.Closeable;
import java.io.IOException;
import java.lang.IllegalArgumentException;
import java.nio.file.ClosedWatchServiceException;
import java.nio.file.Path;
import java.nio.file.ProviderMismatchException;
import java.nio.file.WatchEvent;
import java.nio.file.WatchKey;
import java.util.concurrent.ConcurrentHashMap;

final class DotNetWatchService extends AbstractWatchService {
    private final ConcurrentHashMap<DotNetPath, DotNetWatchKey> keys = new ConcurrentHashMap();

    @Override
    void implClose()
            throws IOException {
        for (DotNetWatchKey key : keys.values()) {
            key.close();
        }

        keys.clear();
    }

    @Override
    WatchKey register(Path path, WatchEvent.Kind<?>[] events, WatchEvent.Modifier... modifiers)
            throws IOException {
        if (!isOpen()) {
            throw new ClosedWatchServiceException();
        }
        if (!(path instanceof DotNetPath)) {
            path.getClass();
            throw new ProviderMismatchException();
        }
        final DotNetPath dir = (DotNetPath)path;

        final DotNetWatchKey key = keys.computeIfAbsent(dir, c -> new DotNetWatchKey(c));
        register0(key, dir, events, modifiers);
        return key;
    }

    void remove(DotNetPath dir) {
        final DotNetWatchKey key = keys.remove(dir);
        close0(key, dir);
    }

    final class DotNetWatchKey extends AbstractWatchKey implements Closeable {
        private final DotNetPath dir;
        private Object state;

        DotNetWatchKey(DotNetPath dir) {
            super(dir, DotNetWatchService.this);
            this.dir = dir;
        }

        @Override
        public void cancel() {
            synchronized (DotNetWatchService.this.closeLock()) {
                DotNetWatchService.this.keys.remove(dir, this);
                close();
            }
        }

        @Override
        public synchronized void close() {
            close0(this, dir);
        }

        @Override
        public boolean isValid() {
            return state != null;
        }

        void error() {
            cancel();
            signal();
        }
    }

    static native void close0(Object self, Object dir);

    static native void register0(Object self, Object dir, Object[] events, Object... modifiers);
}