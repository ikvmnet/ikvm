using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionModuleSymbolBuilder : IkvmReflectionSymbolBuilder<IModuleSymbol, IkvmReflectionModuleSymbol>, IModuleSymbolBuilder
    {

        readonly ModuleBuilder _builder;
        IkvmReflectionModuleSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionModuleSymbolBuilder(IkvmReflectionSymbolContext context, ModuleBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        internal override IkvmReflectionModuleSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateModuleSymbol(_builder);

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typesize)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packsize)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packsize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packingSize, int typesize)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packingSize, typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return new IkvmReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), interfaces?.Unpack()));
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

        /// <inheritdoc />
        public void Complete()
        {
            _builder.CreateGlobalFunctions();

            foreach (var type in _builder.GetTypes())
                if (type is TypeBuilder typeBuilder)
                    if (typeBuilder.IsCreated() == false)
                        if (typeBuilder.CreateType() is { } t)
                            ReflectionSymbol.ResolveTypeSymbol(t).Complete(t);
        }

    }

}
