using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using MethodBase = IKVM.Reflection.MethodBase;
using ParameterInfo = IKVM.Reflection.ParameterInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    abstract class IkvmReflectionMethodBaseSymbol : IkvmReflectionMemberSymbol, IMethodBaseSymbol
    {

        readonly MethodBase _underlyingMethodBase;

        ParameterInfo[]? _parametersSource;
        IkvmReflectionParameterSymbol?[]? _parameters;
        IkvmReflectionParameterSymbol? _returnParameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingMethodBase"></param>
        public IkvmReflectionMethodBaseSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? type, MethodBase underlyingMethodBase) :
            base(context, module, type, underlyingMethodBase)
        {
            _underlyingMethodBase = underlyingMethodBase ?? throw new ArgumentNullException(nameof(underlyingMethodBase));
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            Debug.Assert(parameter.Member == _underlyingMethodBase);

            if (_parametersSource == null)
                Interlocked.CompareExchange(ref _parametersSource, _underlyingMethodBase.GetParameters().OrderBy(i => i.Position).ToArray(), null);
            if (_parameters == null)
                Interlocked.CompareExchange(ref _parameters, new IkvmReflectionParameterSymbol?[_parametersSource.Length], null);

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
                Interlocked.CompareExchange(ref rec, new IkvmReflectionParameterSymbol(Context, this, parameter), null);

            // this should never happen
            if (rec is not IkvmReflectionParameterSymbol sym)
                throw new InvalidOperationException();

            return sym;
        }

        public global::System.Reflection.MethodAttributes Attributes => (global::System.Reflection.MethodAttributes)_underlyingMethodBase.Attributes;

        public global::System.Reflection.CallingConventions CallingConvention => (global::System.Reflection.CallingConventions)_underlyingMethodBase.CallingConvention;

        public bool ContainsGenericParameters => _underlyingMethodBase.ContainsGenericParameters;

        public bool IsAbstract => _underlyingMethodBase.IsAbstract;

        public bool IsAssembly => _underlyingMethodBase.IsAssembly;

        public bool IsConstructor => _underlyingMethodBase.IsConstructor;

        public bool IsFamily => _underlyingMethodBase.IsFamily;

        public bool IsFamilyAndAssembly => _underlyingMethodBase.IsFamilyAndAssembly;

        public bool IsFamilyOrAssembly => _underlyingMethodBase.IsFamilyOrAssembly;

        public bool IsFinal => _underlyingMethodBase.IsFinal;

        public bool IsGenericMethod => _underlyingMethodBase.IsGenericMethod;

        public bool IsGenericMethodDefinition => _underlyingMethodBase.IsGenericMethodDefinition;

        public bool IsHideBySig => _underlyingMethodBase.IsHideBySig;

        public bool IsPrivate => _underlyingMethodBase.IsPrivate;

        public bool IsPublic => _underlyingMethodBase.IsPublic;

        public bool IsSpecialName => _underlyingMethodBase.IsSpecialName;

        public bool IsStatic => _underlyingMethodBase.IsStatic;

        public bool IsVirtual => _underlyingMethodBase.IsVirtual;

        public global::System.Reflection.MethodImplAttributes MethodImplementationFlags => (global::System.Reflection.MethodImplAttributes)_underlyingMethodBase.MethodImplementationFlags;

        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(_underlyingMethodBase.GetGenericArguments());
        }

        public global::System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            return (global::System.Reflection.MethodImplAttributes)_underlyingMethodBase.GetMethodImplementationFlags();
        }

        public IParameterSymbol[] GetParameters()
        {
            return ResolveParameterSymbols(_underlyingMethodBase.GetParameters());
        }

    }

}