using System;

namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.RandomAccessFile' type.
    /// </summary>
    internal sealed class RandomAccessFileAccessor : Accessor<object>
    {

        Type ikvmInternalCallerID;

        FieldAccessor<object, object> fd;
        MethodAccessor<Func<object>> ___getCallerID_;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public RandomAccessFileAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.RandomAccessFile")
        {

        }

        Type IkvmInternalCallerID => Resolve(ref ikvmInternalCallerID, "ikvm.internal.CallerID");

        /// <summary>
        /// Gets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object GetFd(object self) => GetField(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Sets the value of the 'fd' field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd)).SetValue(self, value);

        /// <summary>
        /// Invokes the '__<GetCallerID>' method.
        /// </summary>
        /// <returns></returns>
        public object InvokeGetCallerID() => GetMethod(ref ___getCallerID_, "__<GetCallerID>", IkvmInternalCallerID).Invoker();

    }

#endif

}
