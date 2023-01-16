using System;

namespace IKVM.ByteCode
{

    public sealed class InterfaceInfo
    {

        readonly Class declaringClass;
        readonly InterfaceInfoRecord record;

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal InterfaceInfo(Class clazz, InterfaceInfoRecord record)
        {
            this.declaringClass = clazz ?? throw new ArgumentNullException(nameof(clazz));
            this.record = record;
        }

        /// <summary>
        /// Gets the name of the interface.
        /// </summary>
        public string Name => Class.LazyGet(ref name, () => declaringClass.ResolveConstant<ClassConstant>(record.ClassIndex).Name.Value);

    }

}
