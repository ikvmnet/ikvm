using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SZArrayTypeSymbol : HasElementSymbol
    {

        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<ConstructorSymbol> _constructors;
        ImmutableArray<MethodSymbol> _methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public SZArrayTypeSymbol(SymbolContext context, TypeSymbol elementType) :
            base(context, elementType)
        {

        }

        /// <inheritdoc />
        protected sealed override string NameSuffix => "[]";

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Serializable;

        /// <inheritdoc />
        public sealed override bool IsArray => true;

        /// <inheritdoc />
        public sealed override bool IsSZArray => true;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveCoreType("System.Array");

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            return 1;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces == default)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, [
                    Context.ResolveCoreType("System.Collections.Generic.IEnumerable`1").MakeGenericType([GetElementType()!]),
                    Context.ResolveCoreType("System.Collections.Generic.ICollection`1").MakeGenericType([GetElementType()!]),
                    Context.ResolveCoreType("System.Collections.Generic.IList`1").MakeGenericType([GetElementType()!]),
                ]);

            return _interfaces;
        }

        /// <inheritdoc />
        public override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            if (_constructors == default)
            {
                var int32 = Context.ResolveCoreType("System.Int32");
                var param = ImmutableArray<TypeSymbol>.Empty;

                var b = ImmutableArray.CreateBuilder<ConstructorSymbol>();
                for (TypeSymbol? type = this; type != null && type.IsSZArray; type = type.GetElementType())
                    b.Add(new SyntheticConstructorSymbol(Context, Module, this, MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, param = param.Add(int32)));

                ImmutableInterlocked.InterlockedInitialize(ref _constructors, b.ToImmutable());
            }

            return _constructors;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods == default)
            {
                var int32 = Context.ResolveCoreType("System.Int32");
                var args = ImmutableArray.Create(int32);

                ImmutableInterlocked.InterlockedInitialize(ref _methods,
                [
                    new SyntheticMethodSymbol(Context, Module, this, "Set", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, null, [int32, GetElementType()!]),
                    new SyntheticMethodSymbol(Context, Module, this, "Address", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, GetElementType()!.MakeByRefType(), args),
                    new SyntheticMethodSymbol(Context, Module, this, "Get", MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, GetElementType(), args),
                ]);
            }

            return _methods;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
