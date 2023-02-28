using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

    /// <summary>
    /// Provides runtime access to the 'java.nio.file.attribute.FileTime' type.
    /// </summary>
    internal sealed class FileTimeAccessor : Accessor<object>
    {

        MethodAccessor<Func<long, object>> fromMillis;
        MethodAccessor<Func<object, long>> toMillis;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public FileTimeAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.nio.file.attribute.FileTime"))
        {

        }

        /// <summary>
        /// Invokes the 'fromMillis' method.
        /// </summary>
        public object InvokeFromMillis(long value) => GetMethod(ref fromMillis, nameof(fromMillis), "(J)Ljava.nio.file.attribute.FileTime;").Invoker(value);

        /// <summary>
        /// Invokes the 'toMillis' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public long InvokeToMillis(object self) => GetMethod(ref toMillis, nameof(toMillis), "()J").Invoker(self);

    }

}
