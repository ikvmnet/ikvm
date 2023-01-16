using System;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InterfaceReader
    {

        readonly ClassReader declaringClass;
        readonly InterfaceInfoRecord record;

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal InterfaceReader(ClassReader declaringClass, InterfaceInfoRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the name of the interface.
        /// </summary>
        public string Name => ClassReader.LazyGet(ref name, () => declaringClass.ResolveConstant<ClassConstantReader>(record.ClassIndex).Name.Value);

    }

}
