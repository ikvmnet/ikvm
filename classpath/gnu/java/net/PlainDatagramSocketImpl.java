package gnu.java.net;

public class PlainDatagramSocketImpl extends java.net.PlainDatagramSocketImpl
{
   public int getNativeFD() { throw new NoSuchMethodError("Not supported"); }
}
