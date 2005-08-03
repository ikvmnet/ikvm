package gnu.classpath;

// NOTE this class isn't actually used (only during compilation)
// at runtime, gnu.classpath.Pointer in IKVM.Runtime is used instead
public final class Pointer
{
    public Pointer(cli.System.IntPtr p)
    {
    }

    public native cli.System.IntPtr p();

    public native void MoveMemory(int dst_offset, int src_offset, int count);

    public native byte ReadByte(int index);

    public native void WriteByte(int index, byte b);
}
