/*
  Copyright (C) 2002 Jeroen Frijters

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
package java.io;

import system.Console;
import system.io.*;

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

	static final int Append = 0;	// FileMode.Append, FileAccess.Write (FileOutputStream(append = true))
	static final int Write = 1;		// FileMode.Create, FileAccess.Write (FileOutputStream(append = false))
	static final int Read = 2;		// FileMode.Open, FileAccess.Read (FileInputStream, RandomAccessFile("r"))
	static final int ReadWrite = 3; // FileMode.OpenOrCreate, FileAccess.ReadWrite (RandomAccessFile("rw"))

	static FileDescriptor open(String name, int mode) throws FileNotFoundException
	{
		try
		{
			int fileMode;
			int fileAccess;
			switch(mode)
			{
				case Append:
					fileMode = FileMode.Append;
					fileAccess = FileAccess.Write;
					break;
				case Write:
					fileMode = FileMode.Create;
					fileAccess = FileAccess.Write;
					break;
				case Read:
					fileMode = FileMode.Open;
					fileAccess = FileAccess.Read;
					break;
				case ReadWrite:
					fileMode = FileMode.OpenOrCreate;
					fileAccess = FileAccess.ReadWrite;
					break;
				default:
					throw new Error("Invalid mode: " + mode);
			}
			if(false) throw new system.io.IOException();
			if(false) throw new system.security.SecurityException();
			if(false) throw new system.UnauthorizedAccessException();
			FileStream fs = system.io.File.Open(demanglePath(name), fileMode, fileAccess, FileShare.ReadWrite);
			return new FileDescriptor(fs);
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
		{
			throw new IOException();
		}
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
		{
			throw new IOException();
		}
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

	synchronized void setLength(long length) throws IOException
	{
		if(stream == null)
		{
			throw new IOException();
		}
		try
		{
			if(false) throw new system.io.IOException();
			stream.SetLength(length);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized void seek(long pos) throws IOException
	{
		if(stream == null)
		{
			throw new IOException();
		}
		try
		{
			if(false) throw new system.io.IOException();
			stream.Seek(pos, SeekOrigin.Begin);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized int read(byte[] buf, int offset, int length) throws IOException
	{
		if(stream == null)
		{
			throw new IOException();
		}
		try
		{
			if(false) throw new system.io.IOException();
			return stream.Read(buf, offset, length);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized void write(byte[] buf, int offset, int length) throws IOException
	{
		if(stream == null)
		{
			throw new IOException();
		}
		try
		{
			if(false) throw new system.io.IOException();
			stream.Write(buf, offset, length);
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}

	synchronized long skip(long n) throws IOException
	{
		if(stream == null)
		{
			throw new IOException();
		}
		try
		{
			if(false) throw new system.io.IOException();
			// TODO this is broken, because non-seekable streams should support skip as well...
			// (and I don't think we should seek past EOF here)
			stream.Seek(n, SeekOrigin.Current);
			return n;
		}
		catch(system.io.IOException x)
		{
			throw new IOException(x.get_Message());
		}
		// TODO map al the other exceptions as well...
	}
}
