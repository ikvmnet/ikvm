package sun.nio.ch;

import java.lang.annotation.Native;

/**
 * Manipulates a native array of structs corresponding to (fd, events) pairs.
 *
 * typedef struct pollfd {
 *    SOCKET fd;            // 4 bytes
 *    short events;         // 2 bytes
 * } pollfd_t;
 *
 * @author Konstantin Kladko
 * @author Mike McCloskey
 */

class PollArrayWrapper {

    private AllocatedNativeObject pollArray; // The fd array

    long pollArrayAddress; // pollArrayAddress

    @Native private static final short FD_OFFSET     = 0; // fd offset in pollfd
    @Native private static final short EVENT_OFFSET  = 4; // events offset in pollfd

    static short SIZE_POLLFD = 8; // sizeof pollfd struct

    private int size; // Size of the pollArray

    PollArrayWrapper(int newSize) {
        int allocationSize = newSize * SIZE_POLLFD;
        pollArray = new AllocatedNativeObject(allocationSize, true);
        pollArrayAddress = pollArray.address();
        this.size = newSize;
    }

    // Prepare another pollfd struct for use.
    void addEntry(int index, SelectionKeyImpl ski) {
        putDescriptor(index, ski.channel.getFDVal());
    }

    // Writes the pollfd entry from the source wrapper at the source index
    // over the entry in the target wrapper at the target index.
    void replaceEntry(PollArrayWrapper source, int sindex, PollArrayWrapper target, int tindex) {
        target.putDescriptor(tindex, source.getDescriptor(sindex));
        target.putEventOps(tindex, source.getEventOps(sindex));
    }

    // Grows the pollfd array to new size
    void grow(int newSize) {
        PollArrayWrapper temp = new PollArrayWrapper(newSize);
        for (int i = 0; i < size; i++)
            replaceEntry(this, i, temp, i);
        pollArray.free();
        pollArray = temp.pollArray;
        this.size = temp.size;
        pollArrayAddress = pollArray.address();
    }

    void free() {
        pollArray.free();
    }

    // Access methods for fd structures
    void putDescriptor(int i, int fd) {
        pollArray.putInt(SIZE_POLLFD * i + FD_OFFSET, fd);
    }

    void putEventOps(int i, int event) {
        pollArray.putShort(SIZE_POLLFD * i + EVENT_OFFSET, (short)event);
    }

    int getEventOps(int i) {
        return pollArray.getShort(SIZE_POLLFD * i + EVENT_OFFSET);
    }

    int getDescriptor(int i) {
       return pollArray.getInt(SIZE_POLLFD * i + FD_OFFSET);
    }

    // Adds Windows wakeup socket at a given index.
    void addWakeupSocket(int fdVal, int index) {
        putDescriptor(index, fdVal);
        putEventOps(index, Net.POLLIN);
    }

}
