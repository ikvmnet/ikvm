/*
  Copyright (C) 2009 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net 

*/

package sun.awt;

import java.awt.Image;
import java.io.InputStream;
import sun.awt.datatransfer.DataTransferer;
import sun.awt.datatransfer.ToolkitThreadBlockedHandler;

public class IkvmDataTransferer extends DataTransferer
{
    private static final IkvmDataTransferer instance = new IkvmDataTransferer();

    public static IkvmDataTransferer getInstanceImpl()
    {
        return instance;
    }

    public ToolkitThreadBlockedHandler getToolkitThreadBlockedHandler()
    {
	throw new Error("Not implemented");
    }

    protected byte[] imageToPlatformBytes(Image image, long format)
    {
	throw new Error("Not implemented");
    }

    protected Image platformImageBytesOrStreamToImage(InputStream str, byte[] bytes, long format)
    {
	throw new Error("Not implemented");
    }

    protected String[] dragQueryFile(byte[] bytes)
    {
	throw new Error("Not implemented");
    }

    protected String getNativeForFormat(long format)
    {
	throw new Error("Not implemented");
    }

    protected Long getFormatForNativeAsLong(String str)
    {
        // TODO
	return 1L;
    }

    public boolean isImageFormat(long format)
    {
	throw new Error("Not implemented");
    }

    public boolean isFileFormat(long format)
    {
	throw new Error("Not implemented");
    }

    public boolean isLocaleDependentTextFormat(long format)
    {
	throw new Error("Not implemented");
    }

    public String getDefaultUnicodeEncoding()
    {
	throw new Error("Not implemented");
    }
}
