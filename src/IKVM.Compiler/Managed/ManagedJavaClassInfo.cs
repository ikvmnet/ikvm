using System;
using IKVM.ByteCode.Syntax;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides a handle to a Java class described by a <see cref="IManagedTypeDefinition"/>.
    /// </summary>
    sealed class ManagedJavaClassInfo : JavaClassInfo
    {

        readonly IManagedTypeDefinition info;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="info"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ManagedJavaClassInfo(ManagedJavaClassContext context, IManagedTypeDefinition info) :
            base(context)
        {
            this.info = info ?? throw new ArgumentNullException(nameof(info));
        }

        /// <summary>
        /// Gets the Java class name that correlates to the specified managed type.
        /// </summary>
        public override JavaClassName Signature => ManagedTypeUtil.GetName(info);

    }

}
