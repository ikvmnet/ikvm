using System;
using System.Reflection.Emit;

namespace IKVM.Reflection.Emit
{

    public sealed class LocalBuilder : LocalVariableInfo
    {

        readonly MethodBuilder method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="index"></param>
        /// <param name="pinned"></param>
        internal LocalBuilder(MethodBuilder method, Type localType, int index, bool pinned) :
            base(index, localType, pinned)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="index"></param>
        /// <param name="pinned"></param>
        /// <param name="customModifiers"></param>
        internal LocalBuilder(MethodBuilder method, Type localType, int index, bool pinned, CustomModifiers customModifiers) :
            base(index, localType, pinned, customModifiers)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the method that contains this local.
        /// </summary>
        internal MethodBuilder Method => method;

        /// <summary>
        /// Sets the name of this local variable.
        /// </summary>
        /// <param name="name"></param>
        public void SetLocalSymInfo(string name)
        {
            SetLocalSymInfo(name, 0, 0);
        }

        /// <summary>
        /// Sets the name and lexical scope of this local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startOffset"></param>
        /// <param name="endOffset"></param>
        public void SetLocalSymInfo(string name, int startOffset, int endOffset)
        {
            var module = method.ModuleBuilder;
            if (method.IsTypeCreated())
                throw new InvalidOperationException("Unable to change after type has been created.");

            // generate field signature representing type
            var signature = SignatureHelper.GetFieldSigHelper(module);
            signature.AddArgument(LocalType);
            var array = signature.GetSignature();

            int currentActiveScopeIndex = method.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex();
            if (currentActiveScopeIndex == -1)
            {
                method.m_localSymInfo ??= new();
                method.m_localSymInfo.AddLocalSymInfo(name, array, LocalIndex, startOffset, endOffset);
                return;
            }

            method.GetILGenerator().m_ScopeTree.AddLocalSymInfoToCurrentScope(name, array, LocalIndex, startOffset, endOffset);
        }

    }

}
