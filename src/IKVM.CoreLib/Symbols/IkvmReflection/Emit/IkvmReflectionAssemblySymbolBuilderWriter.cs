using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionAssemblySymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly AssemblySymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        AssemblyBuilder? _assemblyBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionAssemblySymbolBuilderWriter(IkvmReflectionSymbolContext context, AssemblySymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date assembly.
        /// </summary>
        public Assembly? Assembly => _assemblyBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                Define();

            if (state >= IkvmReflectionSymbolState.Emitted && _state < IkvmReflectionSymbolState.Emitted)
                Emit();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var b = _context.Universe.DefineDynamicAssembly(IkvmReflectionUtil.ToAssemblyName(_builder.Identity), AssemblyBuilderAccess.Save, _context.CreateCustomAttributeBuilders(_builder.GetDeclaredCustomAttributes()));
            _assemblyBuilder = b;

            _state = IkvmReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != IkvmReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = IkvmReflectionSymbolState.Emitted;

            // declare modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, IkvmReflectionSymbolState.Defined);

        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Emitted)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // finish modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, IkvmReflectionSymbolState.Emitted);

            // set state
            _state = IkvmReflectionSymbolState.Finished;

            // complete modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, IkvmReflectionSymbolState.Finished);
        }

    }

}
