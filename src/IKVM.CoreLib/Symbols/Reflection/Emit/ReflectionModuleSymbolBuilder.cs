using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionModuleSymbolBuilder : ReflectionSymbolBuilder<IModuleSymbol, ReflectionModuleSymbol>, IModuleSymbolBuilder
    {

        readonly ModuleBuilder _builder;
        ReflectionModuleSymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionModuleSymbolBuilder(ReflectionSymbolContext context, ModuleBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        internal override ReflectionModuleSymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateModuleSymbol(_builder);

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, int typesize)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr, parent?.Unpack(), typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packsize)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr, parent?.Unpack(), packsize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packingSize, int typesize)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr, parent?.Unpack(), packingSize, typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return new ReflectionTypeSymbolBuilder(Context, this, _builder.DefineType(name, attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).ReflectionBuilder);
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
