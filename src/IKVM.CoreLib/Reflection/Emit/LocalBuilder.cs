using System;
using System.Reflection.Emit;

namespace IKVM.Reflection.Emit
{

    sealed class LocalBuilder : LocalVariableInfo
    {

        readonly MethodBuilder _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="index"></param>
        /// <param name="pinned"></param>
        internal LocalBuilder(MethodBuilder method, Type localType, int index, bool pinned) :
            base(index, localType, pinned)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the method that contains this local.
        /// </summary>
        internal MethodBuilder Method => _method;

        /// <summary>
        /// Sets the name of this local variable.
        /// </summary>
        /// <param name="name"></param>
        public void SetLocalSymInfo(string name) => SetLocalSymInfo(name, 0, 0);

        /// <summary>
        /// Sets the name and lexical scope of this local variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startOffset"></param>
        /// <param name="endOffset"></param>
        public void SetLocalSymInfo(string name, int startOffset, int endOffset)
        {
            var module = _method.ModuleBuilder;
            if (_method.IsTypeCreated())
                throw new InvalidOperationException("Unable to change after type has been created.");

            // generate field signature representing type
            var signature = SignatureHelper.GetFieldSigHelper(module);
            signature.AddArgument(LocalType);
            var array = signature.GetSignature();

            var currentActiveScopeIndex = _method.GetILGenerator().ScopeTree.GetCurrentActiveScopeIndex();
            if (currentActiveScopeIndex == -1)
            {
                _method.LocalSymInfo ??= new();
                _method.LocalSymInfo.AddLocalSymInfo(name, array, LocalIndex, startOffset, endOffset);
                return;
            }

            _method.GetILGenerator().ScopeTree.AddLocalSymInfoToCurrentScope(name, array, LocalIndex, startOffset, endOffset);
        }

    }

}
