using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionAssemblySymbolBuilder : ReflectionSymbolBuilder<IAssemblySymbol, ReflectionAssemblySymbol>, IAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;
        ReflectionAssemblySymbol? _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionAssemblySymbolBuilder(ReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        internal sealed override ReflectionAssemblySymbol ReflectionSymbol => _symbol ??= Context.GetOrCreateAssemblySymbol(_builder);

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name)
        {
            return new ReflectionModuleSymbolBuilder(Context, _builder.DefineDynamicModule(name));
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

    }

}
