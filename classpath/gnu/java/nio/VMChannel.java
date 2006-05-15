/*
  Copyright (C) 2006 Jeroen Frijters

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
package gnu.java.nio;

import gnu.java.net.PlainSocketImpl;
import gnu.java.nio.PipeImpl.SinkChannelImpl;
import gnu.java.nio.PipeImpl.SourceChannelImpl;
import gnu.java.nio.channels.FileChannelImpl;
import java.io.IOException;
import java.nio.ByteBuffer;

public class VMChannel
{
    public static VMChannel getVMChannel(SourceChannelImpl source)
    {
        throw new Error("Not implemented");
    }
  
    public static VMChannel getVMChannel(SinkChannelImpl sink)
    {
        throw new Error("Not implemented");
    }

    public void setBlocking(boolean blocking)
    {
        throw new Error("Not implemented");
    }
  
    public int read(ByteBuffer dst) throws IOException
    {
        throw new Error("Not implemented");
    }
  
    public long readScattering(ByteBuffer[] dsts, int offset, int length) throws IOException
    {
        throw new Error("Not implemented");
    }
  
    public int write(ByteBuffer src) throws IOException
    {
        throw new Error("Not implemented");
    }
  
    public long writeGathering(ByteBuffer[] srcs, int offset, int length) throws IOException
    {
        throw new Error("Not implemented");
    }
}
