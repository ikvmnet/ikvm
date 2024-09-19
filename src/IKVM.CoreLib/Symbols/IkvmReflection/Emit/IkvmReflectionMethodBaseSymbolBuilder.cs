using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    abstract class IkvmReflectionMethodBaseSymbolBuilder : IkvmReflectionMemberSymbolBuilder, IIkvmReflectionMethodBaseSymbolBuilder
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public IkvmReflectionMethodBaseSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionTypeSymbol? resolvingType) :
            base(context, resolvingModule, resolvingType)
        {

        }

        /// <inheritdoc />
        public abstract MethodBase UnderlyingMethodBase { get; }

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingMethodBase;

        #region IMethodBaseSymbol

        /// <inheritdoc />
        public System.Reflection.MethodAttributes Attributes => (System.Reflection.MethodAttributes)UnderlyingMethodBase.Attributes;

        /// <inheritdoc />
        public System.Reflection.CallingConventions CallingConvention => (System.Reflection.CallingConventions)UnderlyingMethodBase.CallingConvention;

        /// <inheritdoc />
        public bool ContainsGenericParameters => UnderlyingMethodBase.ContainsGenericParameters;

        /// <inheritdoc />
        public bool IsAbstract => UnderlyingMethodBase.IsAbstract;

        /// <inheritdoc />
        public bool IsAssembly => UnderlyingMethodBase.IsAssembly;

        /// <inheritdoc />
        public bool IsConstructor => UnderlyingMethodBase.IsConstructor;

        /// <inheritdoc />
        public bool IsFamily => UnderlyingMethodBase.IsFamily;

        /// <inheritdoc />
        public bool IsFamilyAndAssembly => UnderlyingMethodBase.IsFamilyAndAssembly;

        /// <inheritdoc />
        public bool IsFamilyOrAssembly => UnderlyingMethodBase.IsFamilyOrAssembly;

        /// <inheritdoc />
        public bool IsFinal => UnderlyingMethodBase.IsFinal;

        /// <inheritdoc />
        public bool IsGenericMethod => UnderlyingMethodBase.IsGenericMethod;

        /// <inheritdoc />
        public bool IsGenericMethodDefinition => UnderlyingMethodBase.IsGenericMethodDefinition;

        /// <inheritdoc />
        public bool IsHideBySig => UnderlyingMethodBase.IsHideBySig;

        /// <inheritdoc />
        public bool IsPrivate => UnderlyingMethodBase.IsPrivate;

        /// <inheritdoc />
        public bool IsPublic => UnderlyingMethodBase.IsPublic;

        /// <inheritdoc />
        public bool IsStatic => UnderlyingMethodBase.IsStatic;

        /// <inheritdoc />
        public bool IsVirtual => UnderlyingMethodBase.IsVirtual;

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingMethodBase.IsSpecialName;

        /// <inheritdoc />
        public System.Reflection.MethodImplAttributes MethodImplementationFlags => (System.Reflection.MethodImplAttributes)UnderlyingMethodBase.MethodImplementationFlags;

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingMethodBase.GetGenericArguments());
        }

        /// <inheritdoc />
        public System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            return (System.Reflection.MethodImplAttributes)UnderlyingMethodBase.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(UnderlyingMethodBase.GetParameters());
        }

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            foreach (var i in GetGenericArguments())
                if (i is IIkvmReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetParameters())
                if (i is IIkvmReflectionParameterSymbolBuilder b)
                    b.OnComplete();

            base.OnComplete();
        }

    }

}
