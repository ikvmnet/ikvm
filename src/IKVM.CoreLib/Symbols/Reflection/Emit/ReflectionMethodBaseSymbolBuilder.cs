using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    abstract class ReflectionMethodBaseSymbolBuilder : ReflectionMemberSymbolBuilder, IReflectionMethodBaseSymbolBuilder
    {

        ReflectionParameterTable _parameterTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        public ReflectionMethodBaseSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder? resolvingType) :
            base(context, resolvingModule, resolvingType)
        {
            _parameterTable = new ReflectionParameterTable(context, resolvingModule, this);
        }

        /// <inheritdoc />
        public abstract MethodBase UnderlyingMethodBase { get; }

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingMethodBase;

        #region IReflectionMethodBaseSymbol

        /// <inheritdoc />
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _parameterTable.GetOrCreateParameterSymbol(parameter);
        }

        #endregion

        #region IMethodBaseSymbolBuilder

        /// <inheritdoc />
        public abstract void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes);

        /// <inheritdoc />
        public abstract IParameterSymbolBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string? strParamName);

        /// <inheritdoc />
        public abstract IILGenerator GetILGenerator();

        /// <inheritdoc />
        public abstract IILGenerator GetILGenerator(int streamSize);

        /// <inheritdoc />
        public abstract void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute);

        /// <inheritdoc />
        public abstract void SetCustomAttribute(ICustomAttributeBuilder customBuilder);

        /// <inheritdoc />
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _parameterTable.GetOrCreateParameterSymbol(parameter);
        }

        #endregion

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
