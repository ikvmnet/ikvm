/* FileDescriptor.java -- Opaque file handle class
   Copyright (C) 1998,2003 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.
 
GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */


package java.io;

import system.Console;
import system.io.*;

/**
  * This class represents an opaque file handle as a Java class.  It should
  * be used only to pass to other methods that expect an object of this
  * type.  No system specific information can be obtained from this object.
  *
  * @author Aaron M. Renn (arenn@urbanophile.com)
  * @author Jeroen Frijters (jeroen@sumatra.nl)
  */
public final class FileDescriptor
{
	public static final FileDescriptor in = new FileDescriptor(Console.OpenStandardInput());
	public static final FileDescriptor out = new FileDescriptor(Console.OpenStandardOutput());
	public static final FileDescriptor err = new FileDescriptor(Console.OpenStandardError());
	private Stream stream;

	public FileDescriptor()
	{
	}

	public FileDescriptor(Stream stream)
	{
		this.stream = stream;
	}

	public synchronized void sync() throws SyncFailedException
	{
		if(stream == null)
		{
			throw new SyncFailedException("The handle is invalid");
		}
		try
		{
			if(false) throw new system.io.IOException();
			stream.Flush();
		}
		catch(system.io.IOException x)
		{
			throw new SyncFailedException(x.get_Message());
		}
	}

	public synchronized boolean valid()
	{
		return stream != null;
	}

	synchronized void close() throws IOException
	{
		if(stream != null)
		{
			// HACK don't close stdin because that throws a NotSupportedException (bug in System.IO.__ConsoleStream)
			if(stream != in.stream)
			{
				try
				{
					stream.Close();
					if(false) throw new system.io.IOException();
				}
				catch(system.io.IOException x)
				{
					throw new IOException(x.get_Message());
				}
			}
			stream = null;
		}
	}

	private static String demanglePath(String path)
	{
		// HACK for some reason Java accepts: \c:\foo.txt
		// I don't know what else, but for now lets just support this
		if(path.length() > 3 && (path.charAt(0) == '\\' || path.charAt(0) == '/') && path.charAt(2) == ':')
		{
			path = path.substring(1);
		}
		return path;
	}

	synchronized void open(String path, String mode) throws IOException
	{
		if(stream != null)
			throw new IOException("FileDescriptor already open");
		if ((path == null) || path.equals(""))
			throw new IllegalArgumentException("Path cannot be null");
		try
		{
			int fileMode;
			int fileAccess;
			if(mode.equals("r"))
			{
				fileMode = FileMode.Open;
				fileAccess = FileAccess.Read;
			}
			else if(mode.equals("rw"))
			{
				fileMode = FileMode.OpenOrCreate;
				fileAccess = FileAccess.ReadWrite;
			}
			else if(mode.equals("rws") || mode.equals("rwd"))
			{
				// TODO implement this
				throw new IllegalArgumentException("rws and rwd not implemented");
				//fileMode = FileMode.OpenOrCreate;
				//fileAccess = FileAccess.ReadWrite;
			}
			else if(mode.equals("rwa"))
			{
				// TODO this is a bogus mode
				fileMode = FileMode.Append;
				fileAccess = FileAccess.ReadWrite;
			}
			else if(mode.equals("w"))
			{
				fileMode = FileMode.Create;
				fileAccess = FileAccess.Write;
			}
			else if(mode.equals("a"))
			{
				fileMode = FileMode.Append;
				fileAccess = FileAccess.Write;
			}
			else
			{
				throw new IllegalArgumentException("Invalid mode value: " + mode);
			}
			if(false) throw new system.io.IOException();
			if(false) throw new system.security.SecurityException();
			if(false) throw new system.UnauthorizedAccessException();
			stream = system.io.File.Open(demanglePath(path), fileMode, fileAccess, FileShare.ReadWrite);
		}
		catch(system.security.SecurityException x1)
		{
			throw new SecurityException(x1.get_Message());
		}
		catch(system.io.IOException x2)
		{
			throw new FileNotFoundException(x2.get_Message());
		}
		catch(system.UnauthorizedAccessException x3)
		{
			// this is caused by "name" being a directory instead of a file
			throw new FileNotFoundException(x3.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized long getFilePointer() throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		try
		{
			if(false) throw new system.io.IOException();
			return stream.get_Position();
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized long getLength() throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		try
		{
			if(false) throw new system.io.IOException();
			return stream.get_Length();
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized void setLength(long len) throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		if (len < 0)
			throw new IllegalArgumentException("Length cannot be less than zero " +
				len);

		try
		{
			if(false) throw new system.io.IOException();
			stream.SetLength(len);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	static final int SET = SeekOrigin.Begin;
	static final int CUR = SeekOrigin.Current;
	static final int END = SeekOrigin.End;

	synchronized long seek(long offset, int whence, boolean stopAtEof) throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		if ((whence != SET) && (whence != CUR) && (whence != END))
			throw new IllegalArgumentException("Invalid whence value: " + whence);

		try
		{
			if(false) throw new system.io.IOException();
			long newpos = stream.Seek(offset, whence);
			if(stopAtEof && newpos > stream.get_Length())
			{
				newpos = stream.Seek(0, SeekOrigin.End);
			}
			return newpos;
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized int read() throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		try
		{
			if(false) throw new system.io.IOException();
			return stream.ReadByte();
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized int read(byte[] buf, int offset, int len) throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		if (len == 0)
			return(0);

		if ((offset < 0) || (offset > buf.length))
			throw new IllegalArgumentException("Offset invalid: " + offset);

		if ((len < 0) || (len > (buf.length - offset)))
			throw new IllegalArgumentException("Length invalid: " + len);

		try
		{
			if(false) throw new system.io.IOException();
			int count = stream.Read(buf, offset, len);
			if(count == 0)
			{
				count = -1;
			}
			return count;
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized void write(int b) throws IOException
	{
		if(stream == null)
		{
			throw new IOException("Invalid FileDescriptor");
		}
		try
		{
			if(false) throw new system.io.IOException();
			stream.WriteByte((byte)b);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized void write(byte[] buf, int offset, int len) throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		if (len == 0)
			return;

		if ((offset < 0) || (offset > buf.length))
			throw new IllegalArgumentException("Offset invalid: " + offset);

		if ((len < 0) || (len > (buf.length - offset)))
			throw new IllegalArgumentException("Length invalid: " + len);

		try
		{
			if(false) throw new system.io.IOException();
			stream.Write(buf, offset, len);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized int available() throws IOException
	{
		if(stream == null)
			throw new IOException("Invalid FileDescriptor");

		try
		{
		    if(false) throw new system.io.IOException();
		    if(false) throw new system.NotSupportedException();
		    if(stream.get_CanSeek())
			    return (int)Math.min(Integer.MAX_VALUE, Math.max(0, stream.get_Length() - stream.get_Position()));
		    return 0;
		}
		catch(system.io.IOException x)
		{
		    throw new IOException(x.get_Message());
		}
		catch(system.NotSupportedException x1)
		{
		    // this means we have a broken Stream, because if CanSeek returns true, it must
		    // support Length and Position
		    return 0;
		}
	}
} // class FileDescriptor
