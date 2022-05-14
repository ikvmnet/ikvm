/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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

package ikvm.io;

public final class InputStreamWrapper extends java.io.InputStream
{
    private cli.System.IO.Stream stream;
    private long markPosition = -1;
    private boolean atEOF;

    public InputStreamWrapper(cli.System.IO.Stream stream)
    {
        this.stream = stream;
    }

    public int read() throws java.io.IOException
    {
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            int i = stream.ReadByte();
	    if (i == -1)
	    {
		atEOF = true;
	    }
	    return i;
        }
        catch (cli.System.IO.IOException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
        catch (cli.System.ObjectDisposedException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
    }

    public int read(byte[] b) throws java.io.IOException
    {
        return read(b, 0, b.length);
    }

    public int read(byte[] b, int off, int len) throws java.io.IOException
    {
        if (b == null)
        {
            throw new NullPointerException();
        }
        if (off < 0 || len < 0 || b.length - off < len)
        {
            throw new IndexOutOfBoundsException();
        }
        if (len == 0)
        {
            return 0;
        }
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
	    int count = stream.Read(b, off, len);
	    if (count == 0)
	    {
		atEOF = true;
		return -1;
	    }
	    return count;
        }
        catch (cli.System.IO.IOException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
        catch (cli.System.ObjectDisposedException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
    }

    public long skip(long n) throws java.io.IOException
    {
        if (n <= 0)
        {
            return 0;
        }
        else if (stream.get_CanSeek())
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                if (false) throw new cli.System.ObjectDisposedException(null);
                long pos = stream.get_Position();
                n = Math.min(n, Math.max(0, stream.get_Length() - pos));
                stream.set_Position(pos + n);
                return n;
            }
            catch (cli.System.IO.IOException x)
            {
                java.io.IOException ex = new java.io.IOException();
                ex.initCause(x);
                throw ex;
            }
            catch (cli.System.ObjectDisposedException x)
            {
                java.io.IOException ex = new java.io.IOException();
                ex.initCause(x);
                throw ex;
            }
        }
        else
        {
            return super.skip(n);
        }
    }

    public int available() throws java.io.IOException
    {
        if (stream.get_CanSeek())
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                if (false) throw new cli.System.ObjectDisposedException(null);
                long val = stream.get_Length() - stream.get_Position();
                return (int)Math.min(Math.max(val, 0), Integer.MAX_VALUE);
            }
            catch (cli.System.IO.IOException x)
            {
                java.io.IOException ex = new java.io.IOException();
                ex.initCause(x);
                throw ex;
            }
            catch (cli.System.ObjectDisposedException x)
            {
                java.io.IOException ex = new java.io.IOException();
                ex.initCause(x);
                throw ex;
            }
        }
        else
        {
	    // It turns out that it's important that available() return non-zero when
	    // data is still available, because BufferedInputStream uses the non-zero
	    // return value as a cue to continue reading.
	    // As suggested by Mark Reinhold, we emulate InflaterInputStream's behavior
	    // and return 0 after we've reached EOF and otherwise 1.
	    return atEOF ? 0 : 1;
        }
    }

    public void close() throws java.io.IOException
    {
        stream.Close();
    }

    public void mark(int readlimit)
    {
        if (stream.get_CanSeek())
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                if (false) throw new cli.System.ObjectDisposedException(null);
                markPosition = stream.get_Position();
            }
            catch (cli.System.IO.IOException x)
            {
            }
            catch (cli.System.ObjectDisposedException x)
            {
            }
        }
    }

    public void reset() throws java.io.IOException
    {
        if (!stream.get_CanSeek())
        {
            throw new java.io.IOException("mark/reset not supported");
        }
        if (markPosition == -1)
        {
            throw new java.io.IOException("no mark available");
        }
        try
        {
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.ObjectDisposedException(null);
            stream.set_Position(markPosition);
        }
        catch (cli.System.IO.IOException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
        catch (cli.System.ObjectDisposedException x)
        {
            java.io.IOException ex = new java.io.IOException();
            ex.initCause(x);
            throw ex;
        }
    }

    public boolean markSupported()
    {
        return stream.get_CanSeek();
    }
}
