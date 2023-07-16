using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Type
{

    /// <summary>
    /// Represents a Java type introduced through Java byte code.
    /// </summary>
    /// <remarks>
    /// This replaces the existing DynamicTypeWrapper.
    /// </remarks>
    internal class JavaByteCodeType : JavaClassType
    {

        readonly ClassReader reader;
        readonly object[] constantPoolPatches;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="constantPoolPatches"></param>
        /// <param name="context"></param>
        public JavaByteCodeType(JavaTypeContext context, ClassReader reader, object[] constantPoolPatches) :
            base(context)
        {
            this.reader = reader;
            this.constantPoolPatches = constantPoolPatches;
        }

        /// <summary>
        /// Gets the name of this Java type.
        /// </summary>
        public override JavaClassName Name => throw new System.NotImplementedException();

    }

}
