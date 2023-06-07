using System;

using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler
{

    /// <summary>
    /// Describes a Java class loaded from byte code.
    /// </summary>
    sealed class ByteCodeJavaClassInfo : JavaClassInfo
    {

        readonly ClassReader reader;

        JavaClassName? name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="reader"></param>
        public ByteCodeJavaClassInfo(ByteCodeJavaClassContext context, ClassReader reader) :
            base(context)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Gets the name of the Java class.
        /// </summary>
        public override JavaClassName Name => name ??= JavaClassName.ParseBinaryName(reader.This.Name.Value.AsMemory());

    }

}
