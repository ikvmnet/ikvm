/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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

package gnu.java.net.protocol.ikvmres;

import cli.System.Reflection.Assembly;
import java.net.*;
import java.io.*;

class IkvmresURLConnection extends URLConnection
{

    private InputStream inputStream;

    IkvmresURLConnection(URL url)
    {
        super(url);
        doOutput = false;
    }

    public void connect() throws IOException
    {
        if(!connected)
        {
            String assembly = url.getHost();
            String resource = url.getFile();
            if(assembly == null || resource == null || !resource.startsWith("/"))
            {
                throw new MalformedURLException(url.toString());
            }

            try
            {
                inputStream = Handler.readResourceFromAssembly(assembly, url.getPort(), resource);
                connected = true;
            }
            catch(cli.System.IO.FileNotFoundException x)
            {
                throw (IOException)new FileNotFoundException(assembly).initCause(x);
            }
            catch(cli.System.BadImageFormatException x1)
            {
                throw (IOException)new IOException().initCause(x1);
            }
            catch(cli.System.Security.SecurityException x2)
            {
                throw (IOException)new IOException().initCause(x2);
            }
        }
    }

    public InputStream getInputStream() throws IOException
    {
        if(!connected)
        {
            connect();
        }
        return inputStream;
    }

    public OutputStream getOutputStream() throws IOException
    {
        throw new IOException("resource URLs are read only");
    }

    public long getLastModified()
    {
        return -1;
    }

    public int getContentLength()
    {
        return -1;
    }

}
