using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection.Emit.Writers;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Implementation of <see cref="System.Reflection.Emit.DynamicMethod"/>.
    /// </summary>
    internal class ReflectionDynamicMethod : MethodSymbol
    {

        readonly string _name;
        readonly MethodAttributes _attributes;
        readonly CallingConventions _callingConvention;
        readonly ParameterSymbolBuilder _returnParameter;
        readonly ImmutableArray<ParameterSymbolBuilder> _parameters;
        readonly TypeSymbol? _owner;
        readonly ModuleSymbol? _module;
        readonly bool _skipVisibility;
        IKVM.CoreLib.Symbols.Emit.ILGenerator? _ilGenerator;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="owner"></param>
        /// <param name="skipVisibility"></param>
        public ReflectionDynamicMethod(ReflectionSymbolContext context, string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, TypeSymbol owner, bool skipVisibility) :
            base(context, owner.Module, owner)
        {
            _name = name;
            _attributes = attributes;
            _callingConvention = callingConvention;
            _returnParameter = new ParameterSymbolBuilder(Context, this, returnType ?? context.ResolveCoreType("System.Void"), -1, [], []);
            _parameters = ImmutableArray.CreateRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, [], [])));
            _owner = owner;
            _skipVisibility = skipVisibility;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="module"></param>
        /// <param name="skipVisibility"></param>
        public ReflectionDynamicMethod(ReflectionSymbolContext context, string name, MethodAttributes attributes, CallingConventions callingConvention, TypeSymbol? returnType, ImmutableArray<TypeSymbol> parameterTypes, ModuleSymbol module, bool skipVisibility) :
            base(context, module, null)
        {
            _name = name;
            _attributes = attributes;
            _callingConvention = callingConvention;
            _returnParameter = new ParameterSymbolBuilder(Context, this, returnType ?? context.ResolveCoreType("System.Void"), -1, [], []);
            _parameters = ImmutableArray.CreateRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, [], [])));
            _module = module;
            _skipVisibility = skipVisibility;
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _callingConvention;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => MethodImplAttributes.IL | MethodImplAttributes.NoInlining;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _returnParameter;

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnParameter.ParameterType;

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => _returnParameter;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => ImmutableArray<TypeSymbol>.Empty;

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => null;

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters => _parameters.CastArray<ParameterSymbol>();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return ImmutableArray<CustomAttribute>.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the local variables in the method are zero-initialized.
        /// </summary>
        public bool InitLocals { get; set; }

        /// <summary>
        /// Defines a parameter of the dynamic method.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attributes"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public ParameterSymbolBuilder? DefineParameter(int position, ParameterAttributes attributes, string? parameterName)
        {
            _parameters[position - 1]._attributes = attributes;
            _parameters[position - 1]._name = parameterName;
            return _parameters[position - 1];
        }

        /// <summary>
        /// Returns an MSIL generator that can be used to emit a body for the dynamic method.
        /// </summary>
        /// <returns></returns>
        public IKVM.CoreLib.Symbols.Emit.ILGenerator GetILGenerator()
        {
            return _ilGenerator ??= new IKVM.CoreLib.Symbols.Emit.ILGenerator(Context);
        }

        /// <summary>
        /// Completes the <see cref="DynamicMethod"/>.
        /// </summary>
        /// <returns></returns>
        public DynamicMethod Complete()
        {
            var dm = CreateDynamicMethod();

            // if we have code staged, emit it into method
            if (_ilGenerator != null)
                _ilGenerator.Write(new ReflectionILGenerationWriter(Context, dm.GetILGenerator(), true));

            return dm;
        }

        /// <summary>
        /// Completes the <see cref="DynamicMethod"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        DynamicMethod CreateDynamicMethod()
        {
            var parameterTypes_ = new Type[_parameters.Length];
            for (int i = 0; i < _parameters.Length; i++)
                parameterTypes_[i] = Context.ResolveCompleteType(_parameters[i].ParameterType);

            if (_owner != null)
                return new DynamicMethod(_name, _attributes, _callingConvention, Context.ResolveType(_returnParameter.ParameterType), parameterTypes_, Context.ResolveCompleteType(_owner), _skipVisibility);
            else if (_module != null)
                return new DynamicMethod(_name, _attributes, _callingConvention, Context.ResolveType(_returnParameter.ParameterType), parameterTypes_, Context.ResolveModule(_module), _skipVisibility);
            else
                throw new InvalidOperationException();
        }

    }

}
