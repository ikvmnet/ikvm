using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.io
{

    static class WinNTFileSystem
    {

#if FIRST_PASS == false

        static WinNTFileSystemAccessor winNTFileSystemAccessor;

        static WinNTFileSystemAccessor WinNTFileSystemAccessor => JVM.Internal.BaseAccessors.Get(ref winNTFileSystemAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate IntPtr __jniDelegate__canonicalize0(IntPtr jniEnv, IntPtr self, IntPtr path);
        static __jniDelegate__canonicalize0 __jniPtr__canonicalize0;
        delegate IntPtr __jniDelegate__canonicalizeWithPrefix0(IntPtr jniEnv, IntPtr self, IntPtr canonicalPrefix, IntPtr pathWithCanonicalPrefix);
        static __jniDelegate__canonicalizeWithPrefix0 __jniPtr__canonicalizeWithPrefix0;
        delegate byte __jniDelegate__delete0(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__delete0 __jniPtr__delete0;
        delegate byte __jniDelegate__rename0(IntPtr jniEnv, IntPtr self, IntPtr f1, IntPtr f2);
        static __jniDelegate__rename0 __jniPtr__rename0;
        delegate int __jniDelegate__getBooleanAttributes(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__getBooleanAttributes __jniPtr__getBooleanAttributes;
        delegate byte __jniDelegate__checkAccess(IntPtr jniEnv, IntPtr self, IntPtr f, int access);
        static __jniDelegate__checkAccess __jniPtr__checkAccess;
        delegate long __jniDelegate__getLastModifiedTime(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__getLastModifiedTime __jniPtr__getLastModifiedTime;
        delegate long __jniDelegate__getLength(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__getLength __jniPtr__getLength;
        delegate byte __jniDelegate__setPermission(IntPtr jniEnv, IntPtr self, IntPtr f, int access, byte enable, byte owneronly);
        static __jniDelegate__setPermission __jniPtr__setPermission;
        delegate byte __jniDelegate__createFileExclusively(IntPtr jniEnv, IntPtr self, IntPtr path);
        static __jniDelegate__createFileExclusively __jniPtr__createFileExclusively;
        delegate IntPtr __jniDelegate__list(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__list __jniPtr__list;
        delegate byte __jniDelegate__createDirectory(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__createDirectory __jniPtr__createDirectory;
        delegate byte __jniDelegate__setLastModifiedTime(IntPtr jniEnv, IntPtr self, IntPtr f, long time);
        static __jniDelegate__setLastModifiedTime __jniPtr__setLastModifiedTime;
        delegate byte __jniDelegate__setReadOnly(IntPtr jniEnv, IntPtr self, IntPtr f);
        static __jniDelegate__setReadOnly __jniPtr__setReadOnly;

#endif

        /// <summary>
        /// Attempts to canonicalize the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string CanonicalizePath(string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                // begin by processing parent element
                var parent = Path.GetDirectoryName(path);

                // root paths with drive letter should have driver letter upper cased
                if (parent == null)
                    return path.Length > 1 && path[1] == ':' ? $"{char.ToUpper(path[0])}:{Path.DirectorySeparatorChar}" : path;
                else
                    parent = CanonicalizePath(parent);

                // trailing slash would result in a last path element of empty string
                var name = Path.GetFileName(path);
                if (name == "" || name == ".")
                    return parent;
                if (name == "..")
                    return Path.GetDirectoryName(parent);

                try
                {
                    if (JVM.Vfs.IsPath(path) == false)
                    {
                        // consult the file system for an actual node with the appropriate name
                        var all = Directory.EnumerateFileSystemEntries(parent, name);
                        if (all.FirstOrDefault() is string one)
                            name = Path.GetFileName(one);
                    }
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (IOException)
                {

                }

                return Path.Combine(parent, name);
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (SecurityException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return path;
#endif
        }

        /// <summary>
        /// Implements the native method 'canonicalize0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string canonicalize0(object self, string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(path))
            {
                return CanonicalizePath(Path.IsPathRooted(path) == false ? Path.GetFullPath(path) : path);
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__canonicalize0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__canonicalize0>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(canonicalize0), "(Ljava/lang/String;)Ljava/lang/String;"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return (string)jniFrm.UnwrapLocalRef(__jniPtr__canonicalize0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(path)));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'canonicalizeWithPrefix0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="canonicalPrefix"></param>
        /// <param name="pathWithCanonicalPrefix"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string canonicalizeWithPrefix0(object self, string canonicalPrefix, string pathWithCanonicalPrefix)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(pathWithCanonicalPrefix))
            {
                return CanonicalizePath(Path.IsPathRooted(pathWithCanonicalPrefix) == false ? Path.GetFullPath(pathWithCanonicalPrefix) : pathWithCanonicalPrefix);
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__canonicalizeWithPrefix0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__canonicalizeWithPrefix0>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(canonicalizeWithPrefix0), "(Ljava/lang/String;Ljava/lang/String;)Ljava/lang/String;"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return (string)jniFrm.UnwrapLocalRef(__jniPtr__canonicalizeWithPrefix0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(canonicalPrefix), jniFrm.MakeLocalRef(pathWithCanonicalPrefix)));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'delete0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool delete0(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                throw new NotImplementedException();
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__delete0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__delete0>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(delete0), "(Ljava/io/File;)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__delete0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f)) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'rename0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool rename0(object self, object f1, object f2)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f1).getPath()) || JVM.Vfs.IsPath(((global::java.io.File)f2).getPath()))
            {
                throw new NotImplementedException();
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__rename0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__rename0>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(rename0), "(Ljava/io/File;Ljava/io/File;)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__rename0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f1), jniFrm.MakeLocalRef(f2)) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getBooleanAttributes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int getBooleanAttributes(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return JVM.Vfs.GetBooleanAttributes(((global::java.io.File)f).getPath());
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__getBooleanAttributes ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__getBooleanAttributes>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(getBooleanAttributes), "(Ljava/io/File;)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__getBooleanAttributes(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'checkAccess'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public static bool checkAccess(object self, object f, int access)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                const int ACCESS_READ = 0x04;
                return JVM.Vfs.GetEntry(((global::java.io.File)f).getPath()) switch
                {
                    VfsFile file => access == ACCESS_READ && file.CanOpen(FileMode.Open, FileAccess.Read),
                    VfsDirectory => true,
                    _ => false,
                };
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__checkAccess ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__checkAccess>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(checkAccess), "(Ljava/io/File;I)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__checkAccess(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f), access) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getLastModifiedTime'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long getLastModifiedTime(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return 0;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__getLastModifiedTime ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__getLastModifiedTime>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(getLastModifiedTime), "(Ljava/io/File;)J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__getLastModifiedTime(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getLength'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long getLength(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return JVM.Vfs.GetEntry(((global::java.io.File)f).getPath()) is VfsFile file ? file.Size : 0;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__getLength ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__getLength>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(getLength), "(Ljava/io/File;)J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__getLength(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'setPermission'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <param name="access"></param>
        /// <param name="enable"></param>
        /// <param name="owneronly"></param>
        /// <returns></returns>
        public static bool setPermission(object self, object f, int access, bool enable, bool owneronly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return false;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__setPermission ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__setPermission>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(setPermission), "(Ljava/io/File;IZZ)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__setPermission(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f), access, enable ? (byte)1 : (byte)0, owneronly ? (byte)1 : (byte)0) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'createFileExclusively'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool createFileExclusively(object self, string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(path))
            {
                return false;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__createFileExclusively ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__createFileExclusively>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(createFileExclusively), "(Ljava/lang/String;)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__createFileExclusively(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(path)) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'list'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string[] list(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                if (JVM.Vfs.GetEntry(((global::java.io.File)f).getPath()) is VfsDirectory vfs)
                    return vfs.List();

                return null;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__list ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__list>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(list), "(Ljava/io/File;)[Ljava/lang/String;"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return (string[])jniFrm.UnwrapLocalRef(__jniPtr__list(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f)));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'createDirectory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool createDirectory(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return false;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__createDirectory ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__createDirectory>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(createDirectory), "(Ljava/io/File;)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__createDirectory(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f)) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'setLastModifiedTime'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool setLastModifiedTime(object self, object f, long time)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return false;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__setLastModifiedTime ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__setLastModifiedTime>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(setLastModifiedTime), "(Ljava/io/File;J)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__setLastModifiedTime(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f), time) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'setReadOnly'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool setReadOnly(object self, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(((global::java.io.File)f).getPath()))
            {
                return false;
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(WinNTFileSystemAccessor.Type.TypeHandle);
                __jniPtr__setReadOnly ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__setReadOnly>(JNIFrame.GetFuncPtr(__callerID, "java/io/WinNTFileSystem", nameof(setLastModifiedTime), "(Ljava/io/File;)Z"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__setReadOnly(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(f)) != 0;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

    }

}