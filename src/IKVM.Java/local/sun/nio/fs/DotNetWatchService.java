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
    private final DotNetFileSystem fs;
    private final ConcurrentHashMap<DotNetPath, DotNetWatchKey> keys = new ConcurrentHashMap();

    DotNetWatchService(DotNetFileSystem fs) {
        this.fs = fs;
    }

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
        final DotNetPath dir = (DotNetPath) path;

        final DotNetWatchKey key = keys.computeIfAbsent(dir, c -> new DotNetWatchKey(c));
        register0(fs, key, dir, events, (Object[]) modifiers);
        return key;
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
            close0(this);
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

    static native void close0(Object key);

    static native void register0(Object fs, Object key, Object dir, Object[] events, Object... modifiers);
}