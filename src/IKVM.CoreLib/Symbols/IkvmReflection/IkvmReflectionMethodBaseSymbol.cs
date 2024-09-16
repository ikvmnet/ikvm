using System;

using MethodBase = IKVM.Reflection.MethodBase;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    abstract class IkvmReflectionMethodBaseSymbol : IkvmReflectionMemberSymbol, IMethodBaseSymbol
    {

        MethodBase _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="method"></param>
        public IkvmReflectionMethodBaseSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, MethodBase method) :
            base(context, module, method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        public IkvmReflectionMethodBaseSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, MethodBase method) :
            this(context, type.ContainingModule, method)
        {

        }

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/> wrapped by this symbol.
        /// </summary>
        internal new MethodBase ReflectionObject => _method;

        /// <inheritdoc />
        public System.Reflection.MethodAttributes Attributes => (System.Reflection.MethodAttributes)_method.Attributes;

        /// <inheritdoc />
        public System.Reflection.CallingConventions CallingConvention => (System.Reflection.CallingConventions)_method.CallingConvention;

        /// <inheritdoc />
        public bool ContainsGenericParameters => _method.ContainsGenericParameters;

        /// <inheritdoc />
        public bool IsAbstract => _method.IsAbstract;

        /// <inheritdoc />
        public bool IsAssembly => _method.IsAssembly;

        /// <inheritdoc />
        public bool IsConstructor => _method.IsConstructor;

        /// <inheritdoc />
        public bool IsFamily => _method.IsFamily;

        /// <inheritdoc />
        public bool IsFamilyAndAssembly => _method.IsFamilyAndAssembly;

        /// <inheritdoc />
        public bool IsFamilyOrAssembly => _method.IsFamilyOrAssembly;

        /// <inheritdoc />
        public bool IsFinal => _method.IsFinal;

        /// <inheritdoc />
        public bool IsGenericMethod => _method.IsGenericMethod;

        /// <inheritdoc />
        public bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

        /// <inheritdoc />
        public bool IsHideBySig => _method.IsHideBySig;

        /// <inheritdoc />
        public bool IsPrivate => _method.IsPrivate;

        /// <inheritdoc />
        public bool IsPublic => _method.IsPublic;

        /// <inheritdoc />
        public bool IsStatic => _method.IsStatic;

        /// <inheritdoc />
        public bool IsVirtual => _method.IsVirtual;

        /// <inheritdoc />
        public bool IsSpecialName => _method.IsSpecialName;

        /// <inheritdoc />
        public System.Reflection.MethodImplAttributes MethodImplementationFlags => (System.Reflection.MethodImplAttributes)_method.MethodImplementationFlags;

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(_method.GetGenericArguments());
        }

        /// <inheritdoc />
        public System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            return (System.Reflection.MethodImplAttributes)_method.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(_method.GetParameters());
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="method"></param>
        internal void Complete(MethodBase method)
        {
            ResolveMethodBaseSymbol(_method = method);
            base.Complete(_method);

            foreach (var i in _method.GetParameters())
                ResolveParameterSymbol(i).Complete(i);

        }

    }

}