using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionMethodBaseSymbol : ReflectionMemberSymbol, IReflectionMethodBaseSymbol
    {

        ReflectionParameterTable _parameterTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public ReflectionMethodBaseSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType) :
            base(context, resolvingModule, resolvingType)
        {
            _parameterTable = new ReflectionParameterTable(context, resolvingModule, this);
        }

        #region IReflectionMethodBaseSymbol

        /// <inheritdoc />
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _parameterTable.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _parameterTable.GetOrCreateParameterSymbol(parameter);
        }

        #endregion

        #region IMethodBaseSymbol

        /// <inheritdoc />
        public abstract MethodBase UnderlyingMethodBase { get; }

        /// <inheritdoc />
        public abstract MethodBase UnderlyingRuntimeMethodBase { get; }

        /// <inheritdoc />
        public override sealed MemberInfo UnderlyingMember => UnderlyingMethodBase;

        /// <inheritdoc />
        public override sealed MemberInfo UnderlyingRuntimeMember => UnderlyingRuntimeMethodBase;

        /// <inheritdoc />
        public virtual System.Reflection.MethodAttributes Attributes => (System.Reflection.MethodAttributes)UnderlyingMethodBase.Attributes;

        /// <inheritdoc />
        public virtual System.Reflection.CallingConventions CallingConvention => (System.Reflection.CallingConventions)UnderlyingMethodBase.CallingConvention;

        /// <inheritdoc />
        public virtual bool ContainsGenericParameters => UnderlyingMethodBase.ContainsGenericParameters;

        /// <inheritdoc />
        public virtual bool IsAbstract => UnderlyingMethodBase.IsAbstract;

        /// <inheritdoc />
        public virtual bool IsAssembly => UnderlyingMethodBase.IsAssembly;

        /// <inheritdoc />
        public virtual bool IsConstructor => UnderlyingMethodBase.IsConstructor;

        /// <inheritdoc />
        public virtual bool IsFamily => UnderlyingMethodBase.IsFamily;

        /// <inheritdoc />
        public virtual bool IsFamilyAndAssembly => UnderlyingMethodBase.IsFamilyAndAssembly;

        /// <inheritdoc />
        public virtual bool IsFamilyOrAssembly => UnderlyingMethodBase.IsFamilyOrAssembly;

        /// <inheritdoc />
        public virtual bool IsFinal => UnderlyingMethodBase.IsFinal;

        /// <inheritdoc />
        public virtual bool IsGenericMethod => UnderlyingMethodBase.IsGenericMethod;

        /// <inheritdoc />
        public virtual bool IsGenericMethodDefinition => UnderlyingMethodBase.IsGenericMethodDefinition;

        /// <inheritdoc />
        public virtual bool IsHideBySig => UnderlyingMethodBase.IsHideBySig;

        /// <inheritdoc />
        public virtual bool IsPrivate => UnderlyingMethodBase.IsPrivate;

        /// <inheritdoc />
        public virtual bool IsPublic => UnderlyingMethodBase.IsPublic;

        /// <inheritdoc />
        public virtual bool IsStatic => UnderlyingMethodBase.IsStatic;

        /// <inheritdoc />
        public virtual bool IsVirtual => UnderlyingMethodBase.IsVirtual;

        /// <inheritdoc />
        public virtual bool IsSpecialName => UnderlyingMethodBase.IsSpecialName;

        /// <inheritdoc />
        public virtual System.Reflection.MethodImplAttributes MethodImplementationFlags => (System.Reflection.MethodImplAttributes)UnderlyingMethodBase.MethodImplementationFlags;

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingMethodBase.GetGenericArguments());
        }

        /// <inheritdoc />
        public virtual System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            return (System.Reflection.MethodImplAttributes)UnderlyingMethodBase.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public virtual IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(UnderlyingMethodBase.GetParameters());
        }

        #endregion

    }

}
