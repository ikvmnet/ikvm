using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class ArrayTypeSymbol : HasElementSymbol
    {

        readonly int _rank;
        readonly ImmutableArray<int> _sizes;
        readonly ImmutableArray<int> _lowerBounds;

        string? _nameSuffix;
        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<ConstructorSymbol> _constructors;
        ImmutableArray<MethodSymbol> _methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ArrayTypeSymbol(SymbolContext context, TypeSymbol elementType, int rank, ImmutableArray<int> sizes, ImmutableArray<int> lowerBounds) :
            base(context, elementType)
        {
            _rank = rank;
            _sizes = sizes;
            _lowerBounds = lowerBounds;
        }

        /// <inheritdoc />
        protected override string NameSuffix => _nameSuffix ??= ComputeNameSuffix();

        /// <summary>
        /// Computes the value for <see cref="NameSuffix"/>.
        /// </summary>
        /// <returns></returns>
        string ComputeNameSuffix()
        {
            if (_rank == 1)
                return "[*]";
            else
                return "[" + new string(',', _rank - 1) + "]";
        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public sealed override bool IsArray => true;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveCoreType("System.Array");

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            return _rank;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            return new InterfaceMapping(ImmutableList<MethodSymbol>.Empty, interfaceType, ImmutableList<MethodSymbol>.Empty, this);
        }

        /// <inheritdoc />
        internal override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            if (_constructors == default)
            {
                var int32 = Context.ResolveCoreType("System.Int32");
                var ctor1Args = ImmutableArray.CreateBuilder<TypeSymbol>();
                var ctor2Args = ImmutableArray.CreateBuilder<TypeSymbol>();
                for (int i = 0; i < _rank; i++)
                {
                    ctor1Args.Add(int32);
                    ctor2Args.Add(int32);
                    ctor2Args.Add(int32);
                }

                ImmutableInterlocked.InterlockedInitialize(ref _constructors,
                [
                    new SyntheticConstructorSymbol(Context, Module, this, MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, ctor1Args.ToImmutable()),
                    new SyntheticConstructorSymbol(Context, Module, this, MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, ctor2Args.ToImmutable()),
                ]);
            }

            return _constructors;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods == default)
            {
                var int32 = Context.ResolveCoreType("System.Int32");

                // get and set args start at the same length
                var argBuilder = ImmutableArray.CreateBuilder<TypeSymbol>();
                for (int i = 0; i < _rank; i++)
                    argBuilder.Add(int32);

                var args = argBuilder.ToImmutable();
                var getArgs = args;
                var setArgs = args.Add(GetElementType()!); // set args takes a value

                ImmutableInterlocked.InterlockedInitialize(ref _methods,
                [
                    new SyntheticMethodSymbol(Context, Module, this, "Set", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, null, setArgs),
                    new SyntheticMethodSymbol(Context, Module, this, "Address", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, GetElementType()!.MakeByRefType(), getArgs),
                    new SyntheticMethodSymbol(Context, Module, this, "Get", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, GetElementType(), getArgs),
                ]);
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
