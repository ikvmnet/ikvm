/*
  Copyright (C) 2013 Jeroen Frijters

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
package java.util.zip;

import java.io.File;
import java.io.FileInputStream;
import java.io.InputStream;
import java.io.IOException;
import java.util.LinkedHashMap;

final class ClassStubZipEntry extends ZipEntry
{
    private final String zipFilePath;

    ClassStubZipEntry(String zipFilePath, String name)
    {
        super(name);
        this.zipFilePath = zipFilePath;
    }

    private File getFile()
    {
        return new File(new File(zipFilePath).getParentFile().getParentFile(), "classes" + File.separator + name);
    }

    public long getSize()
    {
        if (size == -1)
        {
            size = getFile().length();
        }
        return size;
    }

    public long getCompressedSize()
    {
        if (csize == -1)
        {
            csize = getSize();
        }
        return csize;
    }

    public long getCrc()
    {
        if (crc == -1)
        {
            crc = computeCrc();
        }
        return crc;
    }

    private long computeCrc()
    {
        try (InputStream in = getInputStream())
        {
            CRC32 crc = new CRC32();
            int b;
            while ((b = in.read()) != -1)
            {
                crc.update(b);
            }
            return crc.getValue();
        }
        catch (IOException _)
        {
            return 0;
        }
    }

    final InputStream getInputStream() throws IOException
    {
        return new FileInputStream(getFile());
    }

    static native void expandIkvmClasses(ZipFile zipFile, LinkedHashMap<String, ZipEntry> entries);
}
