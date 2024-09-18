using System.Reflection;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    abstract class ReflectionMethodBaseSymbolBuilder : ReflectionMemberSymbolBuilder, IReflectionMethodBaseSymbolBuilder
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public ReflectionMethodBaseSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType) :
            base(context, resolvingModule, resolvingType)
        {

        }

        /// <inheritdoc />
        public abstract MethodBase UnderlyingMethodBase { get; }

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingMethodBase;

        #region IMethodBaseSymbol

        /// <inheritdoc />
        public MethodAttributes Attributes => UnderlyingMethodBase.Attributes;

        /// <inheritdoc />
        public CallingConventions CallingConvention => UnderlyingMethodBase.CallingConvention;

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
        public MethodImplAttributes MethodImplementationFlags => UnderlyingMethodBase.MethodImplementationFlags;

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingMethodBase.GetGenericArguments());
        }

        /// <inheritdoc />
        public MethodImplAttributes GetMethodImplementationFlags()
        {
            return UnderlyingMethodBase.GetMethodImplementationFlags();
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
                if (i is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete();

            foreach (var i in GetParameters())
                if (i is IReflectionParameterSymbolBuilder b)
                    b.OnComplete();

            base.OnComplete();
        }

    }

}
