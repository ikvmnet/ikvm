using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a method of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericMethodSymbol : MethodSymbol
    {

        internal readonly MethodSymbol _definition;
        internal readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _typeParameters;
        TypeSymbol? _returnType;
        ConstructedGenericParameterSymbol? _returnParameter;
        ImmutableArray<ParameterSymbol> _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericMethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType, MethodSymbol definition, GenericContext genericContext) :
            base(context, module, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override bool IsGenericMethod => true;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => false;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => GetReturnParameter();

        /// <summary>
        /// Computes the value for <see cref="ReturnParameter"/>.
        /// </summary>
        /// <returns></returns>
        ConstructedGenericParameterSymbol GetReturnParameter()
        {
            if (_returnParameter == null)
                Interlocked.CompareExchange(ref _returnParameter, new ConstructedGenericParameterSymbol(Context, this, _definition.ReturnParameter, _genericContext), null);

            return _returnParameter;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnType ??= _definition.ReturnType.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => _definition.ReturnTypeCustomAttributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => _genericContext.GenericMethodArguments != null && _genericContext.GenericMethodArguments.Value.Any(i => i.IsMissing || i.ContainsMissing);

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override MethodSymbol GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            if (_typeParameters == default)
                ImmutableInterlocked.InterlockedInitialize(ref _typeParameters, _definition.GetGenericArguments().Select(i => i.Specialize(_genericContext)).ToImmutableArray());

            return _typeParameters;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol GetGenericMethodDefinition()
        {
            // value only supported for generic method not method on generic type
            if (_genericContext.GenericMethodArguments == null)
                throw new InvalidOperationException();

            return _definition;
        }

        /// <inheritdoc />
        public sealed override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _definition.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> GetParameters()
        {
            if (_parameters == default)
            {
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>();
                foreach (var i in _definition.GetParameters())
                    b.Add(new ConstructedGenericParameterSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.ToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
