package gnu.java.net;

public class PlainSocketImpl extends java.net.PlainSocketImpl
{
   public int getNativeFD() { throw new NoSuchMethodError("Not supported"); }
}
