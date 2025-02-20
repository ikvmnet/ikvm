using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionMethodBaseSymbol : ReflectionMemberSymbol, IMethodBaseSymbol
    {

        readonly MethodBase _underlyingMethodBase;

        ParameterInfo[]? _parametersSource;
        ReflectionParameterSymbol?[]? _parameters;
        ReflectionParameterSymbol? _returnParameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMethodBase"></param>
        public ReflectionMethodBaseSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? type, MethodBase underlyingMethodBase) :
            base(context, module, type, underlyingMethodBase)
        {
            _underlyingMethodBase = underlyingMethodBase ?? throw new ArgumentNullException(nameof(underlyingMethodBase));
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal ReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            Debug.Assert(parameter.Member == _underlyingMethodBase);

            if (_parametersSource == null)
                Interlocked.CompareExchange(ref _parametersSource, _underlyingMethodBase.GetParameters().OrderBy(i => i.Position).ToArray(), null);
            if (_parameters == null)
                Interlocked.CompareExchange(ref _parameters, new ReflectionParameterSymbol?[_parametersSource.Length], null);

            // index of current record
            var idx = parameter.Position;
            Debug.Assert(idx >= -1);
            Debug.Assert(idx < _parametersSource.Length);

            // check that our list is long enough to contain the entire table
            if (idx >= 0 && _parameters.Length < idx)
                throw new IndexOutOfRangeException();

            // if not yet created, create, allow multiple instances, but only one is eventually inserted
            ref var rec = ref _returnParameter;
            if (idx >= 0)
                rec = ref _parameters[idx];
            if (rec == null)
                Interlocked.CompareExchange(ref rec, new ReflectionParameterSymbol(Context, this, parameter), null);

            // this should never happen
            if (rec is not ReflectionParameterSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        /// <summary>
        /// Gets the underlying <see cref="MethodBase"/> wrapped by this symbol.
        /// </summary>
        internal MethodBase UnderlyingMethodBase => _underlyingMethodBase;

        /// <inheritdoc />
        public MethodAttributes Attributes => _underlyingMethodBase.Attributes;

        /// <inheritdoc />
        public CallingConventions CallingConvention => _underlyingMethodBase.CallingConvention;

        /// <inheritdoc />
        public bool ContainsGenericParameters => _underlyingMethodBase.ContainsGenericParameters;

        /// <inheritdoc />
        public bool IsAbstract => _underlyingMethodBase.IsAbstract;

        /// <inheritdoc />
        public bool IsAssembly => _underlyingMethodBase.IsAssembly;

        /// <inheritdoc />
        public bool IsConstructor => _underlyingMethodBase.IsConstructor;

        /// <inheritdoc />
        public bool IsFamily => _underlyingMethodBase.IsFamily;

        /// <inheritdoc />
        public bool IsFamilyAndAssembly => _underlyingMethodBase.IsFamilyAndAssembly;

        /// <inheritdoc />
        public bool IsFamilyOrAssembly => _underlyingMethodBase.IsFamilyOrAssembly;

        /// <inheritdoc />
        public bool IsFinal => _underlyingMethodBase.IsFinal;

        /// <inheritdoc />
        public bool IsGenericMethod => _underlyingMethodBase.IsGenericMethod;

        /// <inheritdoc />
        public bool IsGenericMethodDefinition => _underlyingMethodBase.IsGenericMethodDefinition;

        /// <inheritdoc />
        public bool IsHideBySig => _underlyingMethodBase.IsHideBySig;

        /// <inheritdoc />
        public bool IsPrivate => _underlyingMethodBase.IsPrivate;

        /// <inheritdoc />
        public bool IsPublic => _underlyingMethodBase.IsPublic;

        /// <inheritdoc />
        public bool IsStatic => _underlyingMethodBase.IsStatic;

        /// <inheritdoc />
        public bool IsVirtual => _underlyingMethodBase.IsVirtual;

        /// <inheritdoc />
        public bool IsSpecialName => _underlyingMethodBase.IsSpecialName;

        /// <inheritdoc />
        public MethodImplAttributes MethodImplementationFlags => _underlyingMethodBase.MethodImplementationFlags;

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(_underlyingMethodBase.GetGenericArguments());
        }

        /// <inheritdoc />
        public MethodImplAttributes GetMethodImplementationFlags()
        {
            return _underlyingMethodBase.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(_underlyingMethodBase.GetParameters());
        }

    }

}