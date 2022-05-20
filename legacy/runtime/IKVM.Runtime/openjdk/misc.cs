/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using IKVM.Internal;

static class Java_ikvm_runtime_Startup
{
	// this method is called from ikvm.runtime.Startup.exitMainThread() and from JNI's DetachCurrentThread
	public static void jniDetach()
	{
#if !FIRST_PASS
		java.lang.Thread.currentThread().die();
#endif
	}

	public static void addBootClassPathAssembly(Assembly asm)
	{
		ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(AssemblyClassLoader.FromAssembly(asm));
	}
}

static class Java_java_lang_ref_Reference
{
	public static bool noclassgc()
	{
#if CLASSGC
		return !JVM.classUnloading;
#else
		return true;
#endif
	}
}

static class Java_java_util_logging_FileHandler
{
	public static bool isSetUID()
	{
		// TODO
		return false;
	}
}

static class Java_java_util_jar_JarFile
{
	public static string[] getMetaInfEntryNames(object thisJarFile)
	{
#if FIRST_PASS
		return null;
#else
		java.util.zip.ZipFile zf = (java.util.zip.ZipFile)thisJarFile;
		java.util.Enumeration entries = zf.entries();
		List<string> list = null;
		while (entries.hasMoreElements())
		{
			java.util.zip.ZipEntry entry = (java.util.zip.ZipEntry)entries.nextElement();
			if (entry.getName().StartsWith("META-INF/", StringComparison.OrdinalIgnoreCase))
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(entry.getName());
			}
		}
		return list == null ? null : list.ToArray();
#endif
	}
}

static class Java_java_util_zip_ClassStubZipEntry
{
	public static void expandIkvmClasses(object _zipFile, object _entries)
	{
#if !FIRST_PASS
		java.util.zip.ZipFile zipFile = (java.util.zip.ZipFile)_zipFile;
		java.util.LinkedHashMap entries = (java.util.LinkedHashMap)_entries;

		try
		{
			string path = zipFile.getName();
			java.util.zip.ZipEntry entry = (java.util.zip.ZipEntry)entries.get(JVM.JarClassList);
			if (entry != null && VirtualFileSystem.IsVirtualFS(path))
			{
				using (VirtualFileSystem.ZipEntryStream stream = new VirtualFileSystem.ZipEntryStream(zipFile, entry))
				{
					entries.remove(entry.name);
					BinaryReader br = new BinaryReader(stream);
					int count = br.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						java.util.zip.ClassStubZipEntry classEntry = new java.util.zip.ClassStubZipEntry(path, br.ReadString());
						classEntry.setMethod(java.util.zip.ClassStubZipEntry.STORED);
						classEntry.setTime(entry.getTime());
						entries.put(classEntry.name, classEntry);
					}
				}
			}
		}
		catch (java.io.IOException)
		{
		}
		catch (IOException)
		{
		}
#endif
	}
}

static class Java_java_awt_Choice
{
	public static void initIDs()
	{
	}
}

static class Java_sun_awt_image_ByteComponentRaster
{
	public static void initIDs()
	{
	}
}

static class Java_sun_awt_image_BytePackedRaster
{
	public static void initIDs()
	{
	}
}

static class Java_sun_awt_image_IntegerComponentRaster
{
	public static void initIDs()
	{
	}
}

static class Java_sun_awt_image_ShortComponentRaster
{
	public static void initIDs()
	{
	}
}

static class Java_sun_awt_DefaultMouseInfoPeer
{
	public static int fillPointWithCoords(object _this, object point)
	{
		throw new NotImplementedException();
	}

	public static bool isWindowUnderMouse(object _this, object w)
	{
		throw new NotImplementedException();
	}
}

static class Java_sun_awt_FontDescriptor
{
	public static void initIDs()
	{
	}
}

static class Java_sun_invoke_anon_AnonymousClassLoader
{
	public static java.lang.Class loadClassInternal(java.lang.Class hostClass, byte[] classFile, object[] patchArray)
	{
		throw new NotImplementedException();
	}
}

static class Java_sun_invoke_util_VerifyAccess
{
	// called from map.xml as a replacement for Class.getClassLoader() in sun.invoke.util.VerifyAccess.isTypeVisible()
	public static java.lang.ClassLoader Class_getClassLoader(java.lang.Class clazz)
	{
		TypeWrapper tw = TypeWrapper.FromClass(clazz);
		if (ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(tw.Name) == tw)
		{
			// if a class is visible from the bootstrap class loader, we have to return null to allow the visibility check to succeed
			return null;
		}
		return tw.GetClassLoader().GetJavaClassLoader();
	}
}

static class Java_sun_net_PortConfig
{
	public static int getLower0()
	{
		return 49152;
	}

	public static int getUpper0()
	{
		return 65535;
	}
}

static class Java_sun_net_spi_DefaultProxySelector
{
	public static bool init()
	{
		return true;
	}

	public static object getSystemProxy(object thisDefaultProxySelector, string protocol, string host)
	{
		// TODO on Whidbey we might be able to use System.Net.Configuration.DefaultProxySection.Proxy
		return null;
	}
}

static class Java_sun_nio_fs_NetPath
{
	public static string toRealPathImpl(string path)
	{
#if FIRST_PASS
		return null;
#else
		path = java.io.DefaultFileSystem.getFileSystem().canonicalize(path);
		if (VirtualFileSystem.IsVirtualFS(path))
		{
			if (VirtualFileSystem.CheckAccess(path, Java_java_io_WinNTFileSystem.ACCESS_READ))
			{
				return path;
			}
			throw new java.nio.file.NoSuchFileException(path);
		}
		try
		{
			File.GetAttributes(path);
			return path;
		}
		catch (FileNotFoundException)
		{
			throw new java.nio.file.NoSuchFileException(path);
		}
		catch (DirectoryNotFoundException)
		{
			throw new java.nio.file.NoSuchFileException(path);
		}
		catch (UnauthorizedAccessException)
		{
			throw new java.nio.file.AccessDeniedException(path);
		}
		catch (SecurityException)
		{
			throw new java.nio.file.AccessDeniedException(path);
		}
		catch (ArgumentException x)
		{
			throw new java.nio.file.FileSystemException(path, null, x.Message);
		}
		catch (NotSupportedException x)
		{
			throw new java.nio.file.FileSystemException(path, null, x.Message);
		}
		catch (IOException x)
		{
			throw new java.nio.file.FileSystemException(path, null, x.Message);
		}
#endif
	}
}

static class Java_sun_security_provider_NativeSeedGenerator
{
	public static bool nativeGenerateSeed(byte[] result)
	{
		try
		{
			RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
			csp.GetBytes(result);
#if NET_4_0
			csp.Dispose();
#endif
			return true;
		}
		catch (CryptographicException)
		{
			return false;
		}
	}
}

static class Java_com_sun_java_util_jar_pack_NativeUnpack
{
	public static void initIDs()
	{
	}

	public static long start(object thisNativeUnpack, object buf, long offset)
	{
		throw new NotImplementedException();
	}

	public static bool getNextFile(object thisNativeUnpack, object[] parts)
	{
		throw new NotImplementedException();
	}

	public static object getUnusedInput(object thisNativeUnpack)
	{
		throw new NotImplementedException();
	}

	public static long finish(object thisNativeUnpack)
	{
		throw new NotImplementedException();
	}

	public static bool setOption(object thisNativeUnpack, string opt, string value)
	{
		throw new NotImplementedException();
	}

	public static string getOption(object thisNativeUnpack, string opt)
	{
		throw new NotImplementedException();
	}
}

static class Java_com_sun_security_auth_module_NTSystem
{
	public static void getCurrent(object thisObj, bool debug, ref string userName, ref string domain, ref string domainSID, ref string userSID, ref string[] groupIDs, ref string primaryGroupID)
	{
		WindowsIdentity id = WindowsIdentity.GetCurrent();
		string[] name = id.Name.Split('\\');
		userName = name[1];
		domain = name[0];
		domainSID = id.User.AccountDomainSid.Value;
		userSID = id.User.Value;
		string[] groups = new string[id.Groups.Count];
		for (int i = 0; i < groups.Length; i++)
		{
			groups[i] = id.Groups[i].Value;
		}
		groupIDs = groups;
		// HACK it turns out that Groups[0] is the primary group, but AFAIK this is not documented anywhere
		primaryGroupID = groups[0];
	}

	public static long getImpersonationToken0(object thisObj)
	{
		return WindowsIdentity.GetCurrent().Token.ToInt64();
	}
}

static class Java_com_sun_media_sound_JDK13Services
{
	public static string getDefaultProviderClassName(object deviceClass)
	{
		return null;
	}

	public static string getDefaultInstanceName(object deviceClass)
	{
		return null;
	}

	public static object getProviders(object providerClass)
	{
#if FIRST_PASS
		return null;
#else
		return new java.util.ArrayList();
#endif
	}
}

static class Java_java_awt_AWTEvent
{
	public static void initIDs() { }
	public static void nativeSetSource(object thisObj, object peer) { }
}

static class Java_java_awt_Button
{
	public static void initIDs() { }
}

static class Java_java_awt_Checkbox
{
	public static void initIDs() { }
}

static class Java_java_awt_CheckboxMenuItem
{
	public static void initIDs() { }
}

static class Java_java_awt_Color
{
	public static void initIDs() { }
}

static class Java_java_awt_Component
{
	public static void initIDs() { }
}

static class Java_java_awt_Container
{
	public static void initIDs() { }
}

static class Java_java_awt_Cursor
{
	public static void initIDs() { }
	public static void finalizeImpl(Int64 pData) { }
}

static class Java_java_awt_Dialog
{
	public static void initIDs() { }
}

static class Java_java_awt_Dimension
{
	public static void initIDs() { }
}

static class Java_java_awt_Event
{
	public static void initIDs() { }
}

static class Java_java_awt_FileDialog
{
	public static void initIDs() { }
}

static class Java_java_awt_Frame
{
	public static void initIDs() { }
}

static class Java_java_awt_FontMetrics
{
	public static void initIDs() { }
}

static class Java_java_awt_Insets
{
	public static void initIDs() { }
}

static class Java_java_awt_KeyboardFocusManager
{
	public static void initIDs() { }
}

static class Java_java_awt_Label
{
	public static void initIDs() { }
}

static class Java_java_awt_Menu
{
	public static void initIDs() { }
}

static class Java_java_awt_MenuBar
{
	public static void initIDs() { }
}

static class Java_java_awt_MenuComponent
{
	public static void initIDs() { }
}

static class Java_java_awt_MenuItem
{
	public static void initIDs() { }
}

static class Java_java_awt_Rectangle
{
	public static void initIDs() { }
}

static class Java_java_awt_Scrollbar
{
	public static void initIDs() { }
}

static class Java_java_awt_ScrollPane
{
	public static void initIDs() { }
}

static class Java_java_awt_ScrollPaneAdjustable
{
	public static void initIDs() { }
}

static class Java_java_awt_SplashScreen
{
	public static void _update(long splashPtr, int[] data, int x, int y, int width, int height, int scanlineStride) { }
	public static bool _isVisible(long splashPtr) { return false; }
	public static object _getBounds(long splashPtr) { return null; }
	public static long _getInstance() { return 0; }
	public static void _close(long splashPtr) { }
	public static String _getImageFileName(long splashPtr) { return null; }
	public static String _getImageJarName(long splashPtr) { return null; }
	public static bool _setImageData(long splashPtr, byte[] data) { return false; }
	public static float _getScaleFactor(long SplashPtr) { return 1; }
}

static class Java_java_awt_TextArea
{
	public static void initIDs() { }
}

static class Java_java_awt_TextField
{
	public static void initIDs() { }
}

static class Java_java_awt_Toolkit
{
	public static void initIDs() { }
}

static class Java_java_awt_TrayIcon
{
	public static void initIDs() { }
}

static class Java_java_awt_Window
{
	public static void initIDs() { }
}

static class Java_java_awt_event_InputEvent
{
	public static void initIDs() { }
}

static class Java_java_awt_event_MouseEvent
{
	public static void initIDs() { }
}

static class Java_java_awt_event_KeyEvent
{
	public static void initIDs() { }
}

static class Java_java_awt_image_ColorModel
{
	public static void initIDs() { }
}

static class Java_java_awt_image_ComponentSampleModel
{
	public static void initIDs() { }
}

static class Java_java_awt_image_Kernel
{
	public static void initIDs() { }
}

static class Java_java_awt_image_Raster
{
	public static void initIDs() { }
}

static class Java_java_awt_image_SinglePixelPackedSampleModel
{
	public static void initIDs() { }
}

static class Java_java_awt_image_SampleModel
{
	public static void initIDs() { }
}

static class Java_sun_net_ExtendedOptionsImpl
{
	public static void init()
	{
	}

	public static void setFlowOption(java.io.FileDescriptor fd, object f)
	{
#if !FIRST_PASS
		throw new java.lang.UnsupportedOperationException();
#endif
	}

	public static void getFlowOption(java.io.FileDescriptor fd, object f)
	{
#if !FIRST_PASS
		throw new java.lang.UnsupportedOperationException();
#endif
	}

	public static bool flowSupported()
	{
		// We don't support this. Solaris only functionality.
		return false;
	}
}
