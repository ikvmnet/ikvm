using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SZArrayTypeSymbol : HasElementSymbol
    {

        ImmutableArray<TypeSymbol> _interfaces;
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
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
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
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException(); // TODO, need to map methods
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return ImmutableArray<CustomAttribute>.Empty;
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var elementType = GetElementType() ?? throw new InvalidOperationException();
            return elementType.Specialize(context).MakeArrayType();
        }

    }

}
