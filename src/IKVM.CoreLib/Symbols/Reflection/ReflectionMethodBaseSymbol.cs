using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionMethodBaseSymbol : ReflectionMemberSymbol, IMethodBaseSymbol
    {

        readonly MethodBase _method;

        ParameterInfo[]? _parametersSource;
        ReflectionParameterSymbol?[]? _parameters;
        ReflectionParameterSymbol? _returnParameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        public ReflectionMethodBaseSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? type, MethodBase method) :
            base(context, module, type, method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
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

            Debug.Assert(parameter.Member == _method);

            if (_parametersSource == null)
                Interlocked.CompareExchange(ref _parametersSource, _method.GetParameters().OrderBy(i => i.Position).ToArray(), null);
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
        internal new MethodBase ReflectionObject => _method;

        /// <inheritdoc />
        public MethodAttributes Attributes => _method.Attributes;

        /// <inheritdoc />
        public CallingConventions CallingConvention => _method.CallingConvention;

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
        public MethodImplAttributes MethodImplementationFlags => _method.MethodImplementationFlags;

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(_method.GetGenericArguments());
        }

        /// <inheritdoc />
        public MethodImplAttributes GetMethodImplementationFlags()
        {
            return _method.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(_method.GetParameters());
        }

    }

}