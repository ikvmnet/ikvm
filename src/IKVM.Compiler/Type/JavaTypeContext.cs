using System;

using IKVM.ByteCode.Reading;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Represents a context that holds multiple <see cref="JavaType"/> instances. The associated <see cref="IJavaTypeResolver"/> is invoked to link dependencies.
    /// </summary>
    internal class JavaTypeContext
    {

        readonly IJavaTypeResolver resolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public JavaTypeContext(IJavaTypeResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Defines a new <see cref="JavaClassType"/> within this context. Linkage resolution is attempted through the configured <see cref="IJavaTypeResolver"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constantPoolPatches"></param>
        /// <returns></returns>
        public JavaClassType DefineClass(ClassReader reader, object[] constantPoolPatches)
        {
            return new JavaByteCodeType(this, reader, constantPoolPatches);
        }

    }

}
