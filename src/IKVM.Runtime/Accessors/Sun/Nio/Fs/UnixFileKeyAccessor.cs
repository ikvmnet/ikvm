using System;

namespace IKVM.Runtime.Accessors.Sun.Nio.Fs
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal sealed class UnixFileKeyAccessor : Accessor<object>
    {

        MethodAccessor<Func<long, long, object>> init;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public UnixFileKeyAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.fs.UnixFileKey")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="st_dev"></param>
        /// <param name="st_ino"></param>
        /// <returns></returns>
        public object Init(long st_dev, long st_ino) => GetConstructor(ref init, typeof(long), typeof(long)).Invoker(st_dev, st_ino);

    }

#endif

}