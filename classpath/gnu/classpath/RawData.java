package gnu.classpath;

// NOTE this class isn't actually used (only during compilation)
// at runtime, gnu.classpath.RawData in IKVM.Runtime is used instead
public final class RawData
{
    public RawData(cli.System.IntPtr p)
    {
    }

    public native cli.System.IntPtr p();

    public native void MoveMemory(int dst_offset, int src_offset, int count);

    public native byte ReadByte(int index);

    public native void WriteByte(int index, byte b);
}
