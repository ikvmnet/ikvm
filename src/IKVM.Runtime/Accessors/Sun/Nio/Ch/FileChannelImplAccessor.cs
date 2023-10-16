using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.ch.FileChannelImpl' type.
    /// </summary>
    internal sealed class FileChannelImplAccessor : Accessor<object>
    {

        Type ikvmInternalCallerID;

        FieldAccessor<object, object> fd;
        MethodAccessor<Func<object>> ___getCallerID_;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileChannelImplAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.ch.FileChannelImpl")
        {

        }

        Type IkvmInternalCallerID => Resolve(ref ikvmInternalCallerID, "ikvm.internal.CallerID");

        /// <summary>
        /// Gets the value for the 'fd' field.
        /// </summary>
        public object GetFd(object self) => GetField(ref fd, nameof(fd)).GetValue(self);

        /// <summary>
        /// Sets the value for the 'fd' field.
        /// </summary>
        public void SetFd(object self, object value) => GetField(ref fd, nameof(fd)).SetValue(self, value);

        /// <summary>
        /// Invokes the '__<GetCallerID>' method.
        /// </summary>
        /// <returns></returns>
        public object InvokeGetCallerID() => GetMethod(ref ___getCallerID_, "__<GetCallerID>", IkvmInternalCallerID).Invoker();

    }

#endif

}