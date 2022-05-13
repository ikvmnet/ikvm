/*
  Copyright (C) 2007 Jeroen Frijters

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
package sun.nio.ch;

import java.io.File;
import java.io.FileDescriptor;
import java.io.IOException;

public class FileKey
{
    private String path;

    public static FileKey create(FileDescriptor fd)
    {
        FileKey fk = new FileKey();
        fk.path = ((cli.System.IO.FileStream)fd.getStream()).get_Name();
        try
        {
            fk.path = new File(fk.path).getCanonicalPath();
        }
        catch (IOException x)
        {
        }
        return fk;
    }

    public int hashCode()
    {
        return path.hashCode();
    }

    public boolean equals(Object obj)
    {
        return obj == this || (obj instanceof FileKey && ((FileKey)obj).path.equals(path));
    }
}
