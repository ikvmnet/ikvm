using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionMethodSymbolBuilder : IkvmReflectionMethodBaseSymbolBuilder<IMethodSymbol, IkvmReflectionMethodSymbol>, IMethodSymbolBuilder
    {

        readonly MethodBuilder _builder;
        IkvmReflectionMethodSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModuleBuilder"></param>
        /// <param name="builder"></param>
        public IkvmReflectionMethodSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbolBuilder containingModuleBuilder, MethodBuilder builder) :
            base(context, containingModuleBuilder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        /// <param name="builder"></param>
        public IkvmReflectionMethodSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbolBuilder containingTypeBuilder, MethodBuilder builder) :
            base(context, containingTypeBuilder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the <see cref="MethodBuilder"/> that underlies this instance.
        /// </summary>
        internal MethodBuilder ReflectionBuilder => _builder;

        /// <inheritdoc />
        internal override IkvmReflectionMethodSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateMethodSymbol(_builder);

        /// <inheritdoc />
        public void SetImplementationFlags(System.Reflection.MethodImplAttributes attributes)
        {
            _builder.SetImplementationFlags((MethodImplAttributes)attributes);
        }

        /// <inheritdoc />
        public void SetParameters(params ITypeSymbol[] parameterTypes)
        {
            _builder.SetParameters(parameterTypes.Unpack());
        }

        /// <inheritdoc />
        public void SetReturnType(ITypeSymbol? returnType)
        {
            _builder.SetReturnType(returnType?.Unpack());
        }

        /// <inheritdoc />
        public void SetSignature(ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            _builder.SetSignature(returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack());
        }

        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int position, System.Reflection.ParameterAttributes attributes, string? strParamName)
        {
            return new IkvmReflectionParameterSymbolBuilder<IMethodSymbol, IkvmReflectionMethodSymbol, IkvmReflectionMethodSymbolBuilder>(Context, this, _builder, _builder.DefineParameter(position, (ParameterAttributes)attributes, strParamName));
        }

        public IILGenerator GetILGenerator()
        {
            throw new NotImplementedException();
        }

        public IILGenerator GetILGenerator(int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
        }

    }

}
