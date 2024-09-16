using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionAssemblySymbolBuilder : IkvmReflectionSymbolBuilder<IAssemblySymbol, IkvmReflectionAssemblySymbol>, IAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;
        IkvmReflectionAssemblySymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionAssemblySymbolBuilder(IkvmReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        internal sealed override IkvmReflectionAssemblySymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateAssemblySymbol(_builder);

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name)
        {
            return new IkvmReflectionModuleSymbolBuilder(Context, _builder.DefineDynamicModule(name, name + ".dll"));
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName)
        {
            return new IkvmReflectionModuleSymbolBuilder(Context, _builder.DefineDynamicModule(name, fileName));
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName, bool emitSymbolInfo)
        {
            return new IkvmReflectionModuleSymbolBuilder(Context, _builder.DefineDynamicModule(name, fileName, emitSymbolInfo));
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
            foreach (var module in _builder.GetModules())
            {
                if (module is ModuleBuilder moduleBuilder)
                {
                    moduleBuilder.CreateGlobalFunctions();

                    foreach (var type in moduleBuilder.GetTypes())
                        if (type is TypeBuilder typeBuilder)
                            if (typeBuilder.IsCreated() == false)
                                if (typeBuilder.CreateType() is { } t)
                                    ReflectionSymbol.ResolveTypeSymbol(t).Complete(t);
                }
            }
        }

    }

}
