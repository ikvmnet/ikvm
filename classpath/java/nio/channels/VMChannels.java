/*
  Copyright (C) 2005 Jeroen Frijters

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
package java.nio.channels;

import java.io.InputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.OutputStream;
import gnu.java.nio.ChannelInputStream;
import gnu.java.nio.ChannelOutputStream;
import gnu.java.nio.channels.FileChannelImpl;

final class VMChannels
{
  // These methods are implemented in map.xml, because they rely on a package private
  // constructor of FileInputStream and FileOutputStream.
  private static native FileInputStream newInputStream(FileChannelImpl ch);
  private static native FileOutputStream newOutputStream(FileChannelImpl ch);

  static InputStream newInputStream(ReadableByteChannel ch)
  {
    if (ch instanceof FileChannelImpl)
      return newInputStream((FileChannelImpl)ch);
    else
      return new ChannelInputStream(ch);
  }

  static OutputStream newOutputStream(WritableByteChannel ch)
  {
    if (ch instanceof FileChannelImpl)
      return newOutputStream((FileChannelImpl)ch);
    else
      return new ChannelOutputStream(ch);
  }
}
