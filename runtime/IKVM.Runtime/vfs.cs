/*
  Copyright (C) 2007-2011 Jeroen Frijters

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

		internal static string GetAssemblyClassesPath(Assembly asm)
		{
#if FIRST_PASS
			return null;
#else
			// we can't use java.io.File.separatorChar here, because we're invoked by the system property setup code
			return RootPath + "assembly" + System.IO.Path.DirectorySeparatorChar + VfsAssembliesDirectory.GetName(asm) + System.IO.Path.DirectorySeparatorChar + "classes" + System.IO.Path.DirectorySeparatorChar;
#endif
		}

		internal static string GetAssemblyResourcesPath(Assembly asm)
		{
#if FIRST_PASS
			return null;
#else
			return RootPath + "assembly" + System.IO.Path.DirectorySeparatorChar + VfsAssembliesDirectory.GetName(asm) + System.IO.Path.DirectorySeparatorChar + "resources" + System.IO.Path.DirectorySeparatorChar;
#endif
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
			protected readonly Dictionary<string, VfsEntry> entries = new Dictionary<string,VfsEntry>();

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

			internal virtual VfsEntry GetEntry(int index, string[] path)
			{
				VfsEntry entry = GetEntry(path[index++]);
				if (index == path.Length)
				{
					return entry;
				}
				else
				{
					VfsDirectory dir = entry as VfsDirectory;
					if (dir == null)
					{
						return null;
					}
					return dir.GetEntry(index, path);
				}
			}

			internal virtual VfsEntry GetEntry(string name)
			{
				VfsEntry entry;
				lock (entries)
				{
					entries.TryGetValue(name, out entry);
				}
				return entry;
			}

			internal virtual string[] List()
			{
				lock (entries)
				{
					string[] list = new string[entries.Keys.Count];
					entries.Keys.CopyTo(list, 0);
					return list;
				}
			}
		}

		private sealed class VfsAssembliesDirectory : VfsDirectory
		{
			internal override VfsEntry GetEntry(string name)
			{
				VfsEntry entry = base.GetEntry(name);
				if (entry == null)
				{
					Guid guid;
					if (TryParseGuid(name, out guid))
					{
						foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
						{
							if (asm.ManifestModule.ModuleVersionId == guid
								&& !ReflectUtil.IsDynamicAssembly(asm))
							{
								return GetOrAddEntry(name, asm);
							}
						}
					}
					string assemblyName = ParseName(name);
					if (assemblyName != null)
					{
						Assembly asm = null;
						try
						{
							asm = Assembly.Load(assemblyName);
						}
						catch
						{
						}
						if (asm != null
							&& !ReflectUtil.IsDynamicAssembly(asm)
							&& name == GetName(asm))
						{
							return GetOrAddEntry(name, asm);
						}
					}
				}
				return entry;
			}

			private VfsEntry GetOrAddEntry(string name, Assembly asm)
			{
				lock (entries)
				{
					VfsEntry entry;
					if (!entries.TryGetValue(name, out entry))
					{
						VfsDirectory dir = new VfsDirectory();
						dir.Add("resources", new VfsAssemblyResourcesDirectory(asm));
						dir.Add("classes", new VfsAssemblyClassesDirectory(asm));
						Add(name, dir);
						entry = dir;
					}
					return entry;
				}
			}

			// HACK we try to figure out if an assembly is loaded in the Load context
			// http://blogs.msdn.com/b/suzcook/archive/2003/05/29/57143.aspx
			private static bool IsLoadContext(Assembly asm)
			{
				if (asm.ReflectionOnly)
				{
					return false;
				}

				if (asm.GlobalAssemblyCache)
				{
					return true;
				}

				if (ReflectUtil.IsDynamicAssembly(asm) || asm.Location == "")
				{
					return false;
				}

				if (System.IO.Path.GetDirectoryName(asm.Location) == System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))
				{
					// this is an optimization for the common case were the assembly was loaded from the app directory
					return true;
				}

				try
				{
					if (Assembly.Load(asm.FullName) == asm)
					{
						return true;
					}
				}
				catch
				{
				}

				return false;
			}

			internal static string GetName(Assembly asm)
			{
				if (!IsLoadContext(asm))
				{
					return asm.ManifestModule.ModuleVersionId.ToString("N");
				}
				AssemblyName name = asm.GetName();
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string simpleName = name.Name;
				for (int i = 0; i < simpleName.Length; i++)
				{
					if (simpleName[i] == '_')
					{
						sb.Append("_!");
					}
					else
					{
						sb.Append(simpleName[i]);
					}
				}
				byte[] publicKeyToken = name.GetPublicKeyToken();
				if (publicKeyToken != null && publicKeyToken.Length != 0)
				{
					sb.Append("__").Append(name.Version).Append("__");
					for (int i = 0; i < publicKeyToken.Length; i++)
					{
						sb.AppendFormat("{0:x2}", publicKeyToken[i]);
					}
				}
				if (name.CultureInfo != null && !string.IsNullOrEmpty(name.CultureInfo.Name))
				{
					sb.Append("__").Append(name.CultureInfo.Name);
				}
				return sb.ToString();
			}

			private static bool TryParseGuid(string name, out Guid guid)
			{
				if (name.Length != 32)
				{
					guid = Guid.Empty;
					return false;
				}
				for (int i = 0; i < 32; i++)
				{
					if ("0123456789abcdefABCDEF".IndexOf(name[i]) == -1)
					{
						guid = Guid.Empty;
						return false;
					}
				}
				guid = new Guid(name);
				return true;
			}

			private static string ParseName(string directoryName)
			{
				try
				{
					string simpleName = null;
					string version = "0.0.0.0";
					string publicKeyToken = "null";
					string culture = "neutral";
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					int part = 0;
					for (int i = 0; i <= directoryName.Length; i++)
					{
						if (i == directoryName.Length || directoryName[i] == '_')
						{
							if (i < directoryName.Length - 1 && directoryName[i + 1] == '!')
							{
								sb.Append('_');
								i++;
							}
							else if (i == directoryName.Length || directoryName[i + 1] == '_')
							{
								switch (part++)
								{
									case 0:
										simpleName = sb.ToString();
										break;
									case 1:
										version = sb.ToString();
										break;
									case 2:
										publicKeyToken = sb.ToString();
										break;
									case 3:
										culture = sb.ToString();
										break;
									case 4:
										return null;
								}
								sb.Length = 0;
								i++;
							}
							else
							{
								int start = i + 1;
								int end = start;
								while ('0' <= directoryName[end] && directoryName[end] <= '9')
								{
									end++;
								}
								int repeatCount;
								if (directoryName[end] != '_' || !Int32.TryParse(directoryName.Substring(start, end - start), out repeatCount))
								{
									return null;
								}
								sb.Append('_', repeatCount);
								i = end;
							}
						}
						else
						{
							sb.Append(directoryName[i]);
						}
					}
					sb.Length = 0;
					sb.Append(simpleName).Append(", Version=").Append(version).Append(", Culture=").Append(culture).Append(", PublicKeyToken=").Append(publicKeyToken);
					return sb.ToString();
				}
				catch
				{
					return null;
				}
			}
		}

		private sealed class VfsAssemblyResourcesDirectory : VfsDirectory
		{
			private readonly Assembly asm;

			internal VfsAssemblyResourcesDirectory(Assembly asm)
			{
				this.asm = asm;
			}

			internal override VfsEntry GetEntry(string name)
			{
				VfsEntry entry = base.GetEntry(name);
				if (entry == null)
				{
					ManifestResourceInfo resource = asm.GetManifestResourceInfo(name);
					if (resource != null)
					{
						lock (entries)
						{
							if (!entries.TryGetValue(name, out entry))
							{
								entry = new VfsAssemblyResource(asm, name);
								entries.Add(name, entry);
							}
						}
					}
				}
				return entry;
			}

			internal override string[] List()
			{
				return asm.GetManifestResourceNames();
			}
		}

		private sealed class VfsAssemblyResource : VfsFile
		{
			private readonly Assembly asm;
			private readonly string name;

			internal VfsAssemblyResource(Assembly asm, string name)
			{
				this.asm = asm;
				this.name = name;
			}

			internal override System.IO.Stream Open()
			{
				return asm.GetManifestResourceStream(name);
			}

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
		}

		private sealed class VfsAssemblyClassesDirectory : VfsDirectory
		{
			private readonly Assembly asm;
			private readonly Dictionary<string, VfsEntry> classes = new Dictionary<string, VfsEntry>();

			internal VfsAssemblyClassesDirectory(Assembly asm)
			{
				this.asm = asm;
			}

			internal override VfsEntry GetEntry(int index, string[] path)
			{
				if (path[path.Length - 1].EndsWith(".class", StringComparison.Ordinal))
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					for (int i = index; i < path.Length - 1; i++)
					{
						sb.Append(path[i]).Append('.');
					}
					sb.Append(path[path.Length - 1], 0, path[path.Length - 1].Length - 6);
					string className = sb.ToString();
					VfsEntry entry;
					lock (classes)
					{
						if (classes.TryGetValue(className, out entry))
						{
							return entry;
						}
					}
					AssemblyClassLoader acl = AssemblyClassLoader.FromAssembly(asm);
					TypeWrapper tw = null;
					try
					{
						tw = acl.LoadClassByDottedNameFast(className);
					}
					catch
					{
					}
					if (tw != null && !tw.IsArray)
					{
						lock (classes)
						{
							if (!classes.TryGetValue(className, out entry))
							{
								entry = new VfsAssemblyClass(tw);
								classes.Add(className, entry);
							}
						}
						return entry;
					}
					return null;
				}
				else
				{
					Populate();
					return base.GetEntry(index, path);
				}
			}

			internal override string[] List()
			{
				Populate();
				return base.List();
			}

			private void Populate()
			{
				bool populate;
				lock (entries)
				{
					populate = entries.Count == 0;
				}
				if (populate)
				{
					Dictionary<string, string> names = new Dictionary<string, string>();
					AssemblyClassLoader acl = AssemblyClassLoader.FromAssembly(this.asm);
					foreach (Assembly asm in acl.GetAllAvailableAssemblies())
					{
						Type[] types;
						try
						{
							types = asm.GetTypes();
						}
						catch (ReflectionTypeLoadException x)
						{
							types = x.Types;
						}
						catch
						{
							types = Type.EmptyTypes;
						}
						foreach (Type type in types)
						{
							if (type != null)
							{
								string name = null;
								try
								{
									bool isJavaType;
									name = acl.GetTypeNameAndType(type, out isJavaType);
#if !FIRST_PASS
									// annotation custom attributes are pseudo proxies and are not loadable by name (and should not exist in the file systems,
									// because proxies are, ostensibly, created on the fly)
									if (isJavaType && type.BaseType == typeof(global::ikvm.@internal.AnnotationAttributeBase) && name.Contains(".$Proxy"))
									{
										name = null;
									}
#endif
								}
								catch
								{
								}
								if (name != null)
								{
									names[name] = name;
								}
							}
						}
					}
					lock (entries)
					{
						if (entries.Count == 0)
						{
							foreach (string name in names.Keys)
							{
								string[] parts = name.Split('.');
								VfsDirectory dir = this;
								for (int i = 0; i < parts.Length - 1; i++)
								{
									dir = dir.GetEntry(parts[i]) as VfsDirectory ?? dir.AddDirectory(parts[i]);
								}
								// we're adding a dummy file, to make the file appear in the directory listing, it will not actually
								// be accessed, because the code above handles that
								dir.Add(parts[parts.Length - 1] + ".class", VfsDummyFile.Instance);
							}
						}
					}
				}
			}
		}

		private sealed class VfsAssemblyClass : VfsFile
		{
			private readonly TypeWrapper tw;
			private volatile byte[] buf;

			internal VfsAssemblyClass(TypeWrapper tw)
			{
				this.tw = tw;
			}

			private void Populate()
			{
#if !FIRST_PASS
				if (buf == null)
				{
					System.IO.MemoryStream mem = new System.IO.MemoryStream();
					bool includeNonPublicInterfaces = !"true".Equals(java.lang.Props.props.getProperty("ikvm.stubgen.skipNonPublicInterfaces"), StringComparison.OrdinalIgnoreCase);
					IKVM.StubGen.StubGenerator.WriteClass(mem, tw, includeNonPublicInterfaces, false, false, true);
					buf = mem.ToArray();
				}
#endif
			}

			internal override System.IO.Stream Open()
			{
				Populate();
				return new System.IO.MemoryStream(buf);
			}

			internal override long Size
			{
				get
				{
					Populate();
					return buf.Length;
				}
			}
		}

		private sealed class VfsDummyFile : VfsFile
		{
			internal static readonly VfsDummyFile Instance = new VfsDummyFile();

			private VfsDummyFile()
			{
			}

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
			private readonly java.util.zip.ZipFile zipFile;
			private readonly java.util.zip.ZipEntry entry;

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
			private volatile byte[] buf;

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
					Dictionary<string, string> aliases = new Dictionary<string, string>();
					foreach (X509Certificate2 cert in store.Certificates)
					{
						if (!cert.HasPrivateKey)
						{
							// the alias must be unique, otherwise we overwrite the previous certificate with that alias
							string alias = cert.Subject;
							int unique = 0;
							while (aliases.ContainsKey(alias))
							{
								alias = cert.Subject + " #" + (++unique);
							}
							aliases.Add(alias, null);
							jstore.setCertificateEntry(alias, cf.generateCertificate(new global::java.io.ByteArrayInputStream(cert.RawData)));
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
			root.Add("assembly", new VfsAssembliesDirectory());
			AddDummyLibrary(bin, "zip");
			AddDummyLibrary(bin, "awt");
			AddDummyLibrary(bin, "rmi");
			AddDummyLibrary(bin, "w2k_lsa_auth");
			AddDummyLibrary(bin, "jaas_nt");
			AddDummyLibrary(bin, "jaas_unix");
			AddDummyLibrary(bin, "net");
			AddDummyLibrary(bin, "splashscreen");
			AddDummyLibrary(bin, "osx");
			AddDummyLibrary(bin, "management");
			bin.Add("java", new VfsJavaExe());
			bin.Add("javaw", new VfsJavaExe());
			bin.Add("java.exe", new VfsJavaExe());
			bin.Add("javaw.exe", new VfsJavaExe());

			// this is a weird loop back, the vfs.zip resource is loaded from vfs,
			// because that's the easiest way to construct a ZipFile from a Stream.
			java.util.zip.ZipFile zf = new java.util.zip.ZipFile(RootPath + "vfs.zip");
			java.util.Enumeration e = zf.entries();
			while (e.hasMoreElements())
			{
				AddZipEntry(zf, root, (java.util.zip.ZipEntry)e.nextElement());
			}

			// make "lib/security/local_policy.jar" point to "lib/security/US_export_policy.jar"
			// to get the unrestricted crypto policy
			VfsDirectory security = (VfsDirectory)((VfsDirectory)root.GetEntry("lib")).GetEntry("security");
			security.Add("local_policy.jar", security.GetEntry("US_export_policy.jar"));

			Interlocked.CompareExchange(ref VirtualFileSystem.root, root, null);
		}

		private static void AddDummyLibrary(VfsDirectory dir, string name)
		{
			dir.Add(java.lang.System.mapLibraryName(name), VfsDummyFile.Instance);
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
			return root.GetEntry(0, path);
		}

		internal sealed class ZipEntryStream : System.IO.Stream
		{
			private readonly java.util.zip.ZipFile zipFile;
			private readonly java.util.zip.ZipEntry entry;
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
			return access == Java_java_io_WinNTFileSystem.ACCESS_READ && GetVfsEntry(path) != null;
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
