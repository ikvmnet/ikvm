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

public class Handler extends URLStreamHandler
{
    private static final String RFC2396_DIGIT = "0123456789";
    private static final String RFC2396_LOWALPHA = "abcdefghijklmnopqrstuvwxyz";
    private static final String RFC2396_UPALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static final String RFC2396_ALPHA = RFC2396_LOWALPHA + RFC2396_UPALPHA;
    private static final String RFC2396_ALPHANUM = RFC2396_DIGIT + RFC2396_ALPHA;
    private static final String RFC2396_MARK = "-_.!~*'()";
    private static final String RFC2396_UNRESERVED = RFC2396_ALPHANUM + RFC2396_MARK;
    private static final String RFC2396_REG_NAME = RFC2396_UNRESERVED + "$,;:@&=+";
    private static final String RFC2396_PCHAR = RFC2396_UNRESERVED + ":@&=+$,";
    private static final String RFC2396_SEGMENT = RFC2396_PCHAR + ";";
    private static final String RFC2396_PATH_SEGMENTS = RFC2396_SEGMENT + "/";

    static InputStream readResourceFromAssembly(String assembly, int port, String resource)
        throws cli.System.IO.FileNotFoundException,
               cli.System.BadImageFormatException,
               cli.System.Security.SecurityException,
               IOException
    {
        if(assembly.equals("gen") && port != -1 && resource.endsWith(".class") && resource.indexOf('.') == resource.length() - 6)
        {
            ClassLoader loader = GetGenericClassLoaderById(port);
            try
            {
                Class c = Class.forName(resource.substring(1, resource.length() - 6).replace('/', '.'), false, loader);
                return new ByteArrayInputStream(GenerateStub(c));
            }
            catch(ClassNotFoundException _)
            {
            }
            catch(LinkageError _)
            {
            }
        }
        return readResourceFromAssembly(LoadAssembly(assembly), resource);
    }

    public static InputStream readResourceFromAssembly(Assembly asm, String resource)
        throws IOException
    {
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            if(false) throw new cli.System.IO.FileNotFoundException();
            if(false) throw new cli.System.IO.IOException();
            return new ikvm.io.InputStreamWrapper(ReadResourceFromAssemblyImpl(asm, resource));
        }
        catch (cli.System.Security.SecurityException x)
        {
            throw (IOException)new IOException().initCause(x);
        }
        catch (cli.System.IO.FileNotFoundException x)
        {
            if(resource.endsWith(".class") && resource.indexOf('.') == resource.length() - 6)
            {
                Class c = LoadClassFromAssembly(asm, resource.substring(1, resource.length() - 6).replace('/', '.'));
                if(c != null)
                {
                    return new ByteArrayInputStream(GenerateStub(c));
                }
            }
            throw (FileNotFoundException)new FileNotFoundException().initCause(x);
        }
        catch(cli.System.IO.IOException x)
        {
            throw (IOException)new IOException().initCause(x);
        }
    }

    private static native byte[] GenerateStub(Class c);
    private static native cli.System.IO.Stream ReadResourceFromAssemblyImpl(Assembly asm, String resource);
    private static native Class LoadClassFromAssembly(Assembly asm, String className);
    private static native Assembly LoadAssembly(String name)
        throws cli.System.IO.FileNotFoundException, cli.System.BadImageFormatException, cli.System.Security.SecurityException;
    private static native ClassLoader GetGenericClassLoaderById(int id);

    protected URLConnection openConnection(URL url) throws IOException
    {
	return new IkvmresURLConnection(url);
    }

    protected void parseURL(URL url, String url_string, int start, int end)
    {
	try
	{
	    // NOTE originally I wanted to use java.net.URI to handling parsing and constructing of these things,
	    // but it turns out that URI uses regex and that depends on resource loading...
	    url_string = url_string.substring(start, end);
	    if(url_string.startsWith("//"))
	    {
	        int slash = url_string.indexOf('/', 2);
	        if(slash == -1)
	        {
		    throw new RuntimeException("ikvmres: URLs must contain path");
	        }
	        String assembly = unquote(url_string.substring(2, slash));
	        String file = unquote(url_string.substring(slash));
	        setURL(url, "ikvmres", assembly, -1, file, null);
            }
            else if(url_string.startsWith("/"))
            {
                setURL(url, "ikvmres", url.getHost(), -1, url_string, null);
            }
            else
            {
                String[] baseparts = ((cli.System.String)(Object)url.getFile()).Split(new char[] { '/' });
                String[] relparts = ((cli.System.String)(Object)url_string).Split(new char[] { '/' });
                String[] target = new String[baseparts.length + relparts.length - 1];
                for(int i = 1; i < baseparts.length; i++)
                {
                    target[i - 1] = baseparts[i];
                }
                int p = baseparts.length - 2;
                for(int i = 0; i < relparts.length; i++)
                {
                    if(relparts[i].equals("."))
                    {
                    }
                    else if(relparts[i].equals(".."))
                    {
                        p = Math.max(0, p - 1);
                    }
                    else
                    {
                        target[p++] = relparts[i];
                    }
                }
                StringBuffer file = new StringBuffer();
                for(int i = 0; i < p; i++)
                {
                    file.append('/').append(target[i]);
                }
                setURL(url, "ikvmres", url.getHost(), -1, file.toString(), null);
            }
	}
	catch(URISyntaxException x)
	{
	    throw new RuntimeException(x.getMessage());
	}
    }

    protected String toExternalForm(URL url)
    {
	// NOTE originally I wanted to use java.net.URI to handle parsing and constructing of these things,
	// but it turns out that URI uses regex and that depends on resource loading...
	return "ikvmres://" + quote(url.getHost(), RFC2396_REG_NAME) + quote(url.getFile(), RFC2396_PATH_SEGMENTS);
    }

    protected InetAddress getHostAddress(URL url)
    {
	return null;
    }

    private static String quote (String str, String legalCharacters)
    {
	StringBuffer sb = new StringBuffer(str.length());
	for (int i = 0; i < str.length(); i++) 
	{
	    char c = str.charAt(i);
	    if (legalCharacters.indexOf(c) == -1) 
	    {
		String hex = "0123456789ABCDEF";
		if (c <= 127) 
		{
		    sb.append('%')
			.append(hex.charAt(c / 16))
			.append(hex.charAt(c % 16));
		} 
		else 
		{
		    try 
		    {
			// this is far from optimal, but it works
			byte[] utf8 = str.substring(i, i + 1).getBytes("utf-8");
			for (int j = 0; j < utf8.length; j++) 
			{
			    sb.append('%')
				.append(hex.charAt((utf8[j] & 0xff) / 16))
				.append(hex.charAt((utf8[j] & 0xff) % 16));
			}
		    } 
		    catch (java.io.UnsupportedEncodingException x) 
		    {
			throw (Error)new InternalError().initCause(x);
		    }
		}
	    } 
	    else 
	    {
		sb.append(c);
	    }
	}
	return sb.toString();
    }

    private static String unquote (String str)
	throws URISyntaxException
    {
	if (str == null)
	    return null;
	byte[] buf = new byte[str.length()];
	int pos = 0;
	for (int i = 0; i < str.length(); i++) 
	{
	    char c = str.charAt(i);
	    if (c > 127)
		throw new URISyntaxException(str, "Invalid character");
	    if (c == '%') 
	    {
		if (i + 2 >= str.length())
		    throw new URISyntaxException(str, "Invalid quoted character");
		String hex = "0123456789ABCDEF";
		int hi = hex.indexOf(str.charAt(++i));
		int lo = hex.indexOf(str.charAt(++i));
		if (lo < 0 || hi < 0)
		    throw new URISyntaxException(str, "Invalid quoted character");
		buf[pos++] = (byte)(hi * 16 + lo);
	    } 
	    else 
	    {
		buf[pos++] = (byte)c;
	    }
	}
	try 
	{
	    return new String(buf, 0, pos, "utf-8");
	} 
	catch (java.io.UnsupportedEncodingException x2) 
	{
	    throw (Error)new InternalError().initCause(x2);
	}
    }
}
