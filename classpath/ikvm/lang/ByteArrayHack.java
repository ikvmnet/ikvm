package ikvm.lang;

public final class ByteArrayHack
{
    private ByteArrayHack() {}

    public static native cli.System.Byte[] cast(byte[] b);
    public static native byte[] cast(cli.System.Byte[] b);
}
