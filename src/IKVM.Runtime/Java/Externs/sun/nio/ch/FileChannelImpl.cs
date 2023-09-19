using System;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::sun.nio.ch.FileChannelImpl"/>.
    /// </summary>
    static class FileChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileChannelImplAccessor fileChannelImplAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileChannelImplAccessor FileChannelImplAccessor => JVM.BaseAccessors.Get(ref fileChannelImplAccessor);

#endif

        delegate long __jniDelegate__transferTo0(IntPtr jniEnv, IntPtr self, IntPtr srcFDO, long position, long count, IntPtr dstFDO);
        static __jniDelegate__transferTo0 __jniPtr__transferTo0;

        /// <summary>
        /// Implements the native method for 'transferTo0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="src"></param>
        /// <param name="position"></param>
        /// <param name="count"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static unsafe long transferTo0(object self, global::java.io.FileDescriptor src, long position, long count, global::java.io.FileDescriptor dst)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            __jniPtr__transferTo0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__transferTo0>(JNIFrame.GetFuncPtr((global::ikvm.@internal.CallerID)FileChannelImplAccessor.InvokeGetCallerID(), "sun/nio/ch/FileChannelImpl", nameof(transferTo0), "(Ljava/io/FileDescriptor;JJLjava/io/FileDescriptor;)J"));
            var jniFrm = new JNIFrame();
            var jniEnv = jniFrm.Enter((global::ikvm.@internal.CallerID)FileChannelImplAccessor.InvokeGetCallerID());
            long result;
            try
            {
                var selfRef = jniFrm.MakeLocalRef(self);
                var srcFDORef = jniFrm.MakeLocalRef(src);
                var dstFDORef = jniFrm.MakeLocalRef(dst);
                if ((result = __jniPtr__transferTo0(jniEnv, selfRef, srcFDORef, position, count, dstFDORef)) < 0)
                    return result;
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

            // if destination is a file, advance its current position by the amount read
            if (FileDescriptorAccessor.GetStream(dst) is FileStream f2)
                f2.Seek(result, SeekOrigin.Current);

            return result;
#endif
        }

        /// <summary>
        /// Implements the native method for 'position0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static long position0(object self, global::java.io.FileDescriptor fd, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                if (offset >= 0)
                {
                    if (s.CanSeek == false)
                        return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                    else
                        return s.Seek(offset, SeekOrigin.Begin);
                }

                return s.Position;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return global::sun.nio.ch.IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return global::sun.nio.ch.IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Position failed.", e);
            }
#endif
        }

    }

}