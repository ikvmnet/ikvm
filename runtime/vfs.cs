/*
  Copyright (C) 2007-2009 Jeroen Frijters

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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace IKVM.Internal
{
	static class VirtualFileSystem
	{
		internal static readonly string RootPath = JVM.IsUnix ? "/.virtual-ikvm-home/" : @"C:\.virtual-ikvm-home\";

		internal static bool IsVirtualFS(string path)
		{
			return (path.Length == RootPath.Length - 1 && String.CompareOrdinal(path, 0, RootPath, 0, RootPath.Length - 1) == 0)
				|| String.CompareOrdinal(path, 0, RootPath, 0, RootPath.Length) == 0;
		}

#if !FIRST_PASS
		private static VfsDirectory root;

		private abstract class VfsEntry
		{
		}

		private abstract class VfsFile : VfsEntry
		{
			internal abstract long Size { get; }
			internal abstract System.IO.Stream Open();
		}

		private class VfsDirectory : VfsEntry
		{
			private readonly Dictionary<string, VfsEntry> entries = new Dictionary<string,VfsEntry>();

			internal VfsDirectory AddDirectory(string name)
			{
				VfsDirectory dir = new VfsDirectory();
				Add(name, dir);
				return dir;
			}

			internal void Add(string name, VfsEntry entry)
			{
				lock (entries)
				{
					entries.Add(name, entry);
				}
			}

			internal VfsEntry GetEntry(string name)
			{
				VfsEntry entry;
				lock (entries)
				{
					entries.TryGetValue(name, out entry);
				}
				return entry;
			}

			internal string[] List()
			{
				lock (entries)
				{
					string[] list = new string[entries.Keys.Count];
					entries.Keys.CopyTo(list, 0);
					return list;
				}
			}
		}

		private sealed class VfsDummyFile : VfsFile
		{
			internal override long Size
			{
				get { return 0; }
			}

			internal override System.IO.Stream Open()
			{
				return System.IO.Stream.Null;
			}
		}

		private sealed class VfsZipEntry : VfsFile
		{
			private java.util.zip.ZipFile zipFile;
			private java.util.zip.ZipEntry entry;

			internal VfsZipEntry(java.util.zip.ZipFile zipFile, java.util.zip.ZipEntry entry)
			{
				this.zipFile = zipFile;
				this.entry = entry;
			}

			internal override long Size
			{
				get { return entry.getSize(); }
			}

			internal override System.IO.Stream Open()
			{
				return new ZipEntryStream(zipFile, entry);
			}
		}

		private sealed class VfsCacertsEntry : VfsFile
		{
			private byte[] buf;

			internal override long Size
			{
				get
				{
					Populate();
					return buf.Length;
				}
			}

			internal override System.IO.Stream Open()
			{
				Populate();
				return new System.IO.MemoryStream(buf, false);
			}

			private void Populate()
			{
				if (buf == null)
				{
					global::java.security.KeyStore jstore = global::java.security.KeyStore.getInstance("jks");
					jstore.load(null);
					global::java.security.cert.CertificateFactory cf = global::java.security.cert.CertificateFactory.getInstance("X509");

					X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
					store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
					foreach (X509Certificate2 cert in store.Certificates)
					{
						if (!cert.HasPrivateKey)
						{
							jstore.setCertificateEntry(cert.Subject, cf.generateCertificate(new global::java.io.ByteArrayInputStream(cert.RawData)));
						}
					}
					store.Close();
					global::java.io.ByteArrayOutputStream baos = new global::java.io.ByteArrayOutputStream();
					jstore.store(baos, new char[0]);
					buf = baos.toByteArray();
				}
			}
		}

		private sealed class VfsVfsZipEntry : VfsFile
		{
			internal override long Size
			{
				get
				{
					using (System.IO.Stream stream = Open())
					{
						return stream.Length;
					}
				}
			}

			internal override System.IO.Stream Open()
			{
				//return new System.IO.FileStream("c:\\ikvm\\openjdk\\vfs.zip", System.IO.FileMode.Open);
				return Assembly.GetExecutingAssembly().GetManifestResourceStream("vfs.zip");
			}
		}

		private abstract class VfsExecutable : VfsFile
		{
			internal override long Size { get { return 0; } }

			internal override System.IO.Stream Open()
			{
				return System.IO.Stream.Null;
			}

			internal abstract string GetPath();
		}

		private sealed class VfsJavaExe : VfsExecutable
		{
			private string path;

			internal override string GetPath()
			{
				if (path == null)
				{
					path = new System.Uri(Assembly.GetEntryAssembly().CodeBase + "/../ikvm.exe").LocalPath;
				}
				return path;
			}
		}

		private static void Initialize()
		{
			VfsDirectory root = new VfsDirectory();
			root.AddDirectory("lib").AddDirectory("security").Add("cacerts", new VfsCacertsEntry());
			VfsDirectory bin = new VfsDirectory();
			root.Add("bin", bin);
			AddDummyLibrary(bin, "zip");
			AddDummyLibrary(bin, "awt");
			AddDummyLibrary(bin, "rmi");
			AddDummyLibrary(bin, "w2k_lsa_auth");
			AddDummyLibrary(bin, "jaas_nt");
			AddDummyLibrary(bin, "jaas_unix");
			AddDummyLibrary(bin, "unpack");
			AddDummyLibrary(bin, "net");
			bin.Add("java", new VfsJavaExe());
			bin.Add("javaw", new VfsJavaExe());
			bin.Add("java.exe", new VfsJavaExe());
			bin.Add("javaw.exe", new VfsJavaExe());

			// this is a weird loop back, the vfs.zip resource is loaded from vfs,
			// because that's the easiest way to construct a ZipFile from a Stream.
			java.util.zip.ZipFile zf = new java.util.zip.ZipFile(RootPath + "vfs.zip");
			java.util.Enumeration e = zf.entries();
			char sep = java.io.File.separatorChar;
			while (e.hasMoreElements())
			{
				AddZipEntry(zf, root, (java.util.zip.ZipEntry)e.nextElement());
			}

			Interlocked.CompareExchange(ref VirtualFileSystem.root, root, null);
		}

		private static void AddDummyLibrary(VfsDirectory dir, string name)
		{
			dir.Add(java.lang.System.mapLibraryName(name), new VfsDummyFile());
		}

		private static void AddZipEntry(java.util.zip.ZipFile zf, VfsDirectory root, java.util.zip.ZipEntry entry)
		{
			if (entry.isDirectory())
			{
				return;
			}
			string[] path = entry.getName().Split('/');
			VfsDirectory dir = root;
			for (int i = 0; i < path.Length - 1; i++)
			{
				VfsDirectory existing = dir.GetEntry(path[i]) as VfsDirectory;
				if (existing == null)
				{
					existing = dir.AddDirectory(path[i]);
				}
				dir = existing;
			}
			dir.Add(path[path.Length - 1], new VfsZipEntry(zf, entry));
		}

		private static VfsEntry GetVfsEntry(string name)
		{
			if (root == null)
			{
				if (name == RootPath + "vfs.zip")
				{
					return new VfsVfsZipEntry();
				}
				Initialize();
			}
			if (name.Length <= RootPath.Length)
			{
				return root;
			}
			string[] path = name.Substring(RootPath.Length).Split(java.io.File.separatorChar);
			VfsDirectory dir = root;
			for (int i = 0; i < path.Length - 1; i++)
			{
				dir = dir.GetEntry(path[i]) as VfsDirectory;
				if (dir == null)
				{
					return null;
				}
			}
			return dir.GetEntry(path[path.Length - 1]);
		}

		private sealed class ZipEntryStream : System.IO.Stream
		{
			private java.util.zip.ZipFile zipFile;
			private java.util.zip.ZipEntry entry;
			private java.io.InputStream inp;
			private long position;

			internal ZipEntryStream(java.util.zip.ZipFile zipFile, java.util.zip.ZipEntry entry)
			{
				this.zipFile = zipFile;
				this.entry = entry;
				inp = zipFile.getInputStream(entry);
			}

			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanWrite
			{
				get { return false; }
			}

			public override bool CanSeek
			{
				get { return true; }
			}

			public override long Length
			{
				get { return entry.getSize(); }
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				// For compatibility with real file i/o, we try to read the requested number
				// of bytes, instead of returning earlier if the underlying InputStream does so.
				int totalRead = 0;
				while (count > 0)
				{
					int read = inp.read(buffer, offset, count);
					if (read <= 0)
					{
						break;
					}
					offset += read;
					count -= read;
					totalRead += read;
					position += read;
				}
				return totalRead;
			}

			public override long Position
			{
				get
				{
					return position;
				}
				set
				{
					if (value < position)
					{
						if (value < 0)
						{
							throw new System.IO.IOException("Negative seek offset");
						}
						position = 0;
						inp.close();
						inp = zipFile.getInputStream(entry);
					}
					long skip = value - position;
					while (skip > 0)
					{
						long skipped = inp.skip(skip);
						if (skipped == 0)
						{
							if (position != entry.getSize())
							{
								throw new System.IO.IOException("skip failed");
							}
							// we're actually at EOF in the InputStream, but we set the virtual position beyond EOF
							position += skip;
							break;
						}
						position += skipped;
						skip -= skipped;
					}
				}
			}

			public override void Flush()
			{
			}

			public override long Seek(long offset, System.IO.SeekOrigin origin)
			{
				switch (origin)
				{
					case System.IO.SeekOrigin.Begin:
						Position = offset;
						break;
					case System.IO.SeekOrigin.Current:
						Position += offset;
						break;
					case System.IO.SeekOrigin.End:
						Position = entry.getSize() + offset;
						break;
				}
				return position;
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			public override void Close()
			{
				base.Close();
				inp.close();
			}
		}
#endif

		internal static System.IO.Stream Open(string name, System.IO.FileMode fileMode, System.IO.FileAccess fileAccess)
		{
#if FIRST_PASS
			return null;
#else
			if (fileMode != System.IO.FileMode.Open || fileAccess != System.IO.FileAccess.Read)
			{
				throw new System.IO.IOException("vfs is read-only");
			}
			VfsFile entry = GetVfsEntry(name) as VfsFile;
			if (entry == null)
			{
				throw new System.IO.FileNotFoundException("File not found");
			}
			return entry.Open();
#endif
		}

		internal static long GetLength(string path)
		{
#if FIRST_PASS
			return 0;
#else
			VfsFile entry = GetVfsEntry(path) as VfsFile;
			return entry == null ? 0 : entry.Size;
#endif
		}

		internal static bool CheckAccess(string path, int access)
		{
#if FIRST_PASS
			return false;
#else
			return access == IKVM.NativeCode.java.io.Win32FileSystem.ACCESS_READ && GetVfsEntry(path) != null;
#endif
		}

		internal static int GetBooleanAttributes(string path)
		{
#if FIRST_PASS
			return 0;
#else
			VfsEntry entry = GetVfsEntry(path);
			if (entry == null)
			{
				return 0;
			}
			const int BA_EXISTS = 0x01;
			const int BA_REGULAR = 0x02;
			const int BA_DIRECTORY = 0x04;
			return entry is VfsDirectory ? BA_EXISTS | BA_DIRECTORY : BA_EXISTS | BA_REGULAR;
#endif
		}

		internal static string MapExecutable(string path)
		{
#if FIRST_PASS
			return null;
#else
			VfsExecutable entry = GetVfsEntry(path) as VfsExecutable;
			if (entry == null)
			{
				return path;
			}
			return entry.GetPath();
#endif
		}

		internal static string[] List(string path)
		{
#if FIRST_PASS
			return null;
#else
			VfsDirectory dir = GetVfsEntry(path) as VfsDirectory;
			return dir == null ? null : dir.List();
#endif
		}
	}
}
