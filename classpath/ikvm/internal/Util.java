package ikvm.internal;

import ikvm.lang.Internal;

@Internal
public final class Util
{
    private Util() {}
	
    public static boolean rangeCheck(int arrayLength, int offset, int length)
    {
        return offset >= 0
	    && offset <= arrayLength
            && length >= 0
            && length <= arrayLength - offset;
    }
}
