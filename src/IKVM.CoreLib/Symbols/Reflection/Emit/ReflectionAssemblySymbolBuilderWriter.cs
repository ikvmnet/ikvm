using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionAssemblySymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly AssemblySymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
        AssemblyBuilder? _assemblyBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionAssemblySymbolBuilderWriter(ReflectionSymbolContext context, AssemblySymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date assembly.
        /// </summary>
        public Assembly? Assembly => _assemblyBuilder;

        /// <summary>
        /// Advances the writer to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Defined;

            // define assembly
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_builder.Identity.ToAssemblyName(), AssemblyBuilderAccess.Run, _context.CreateCustomAttributeBuilders(_builder.GetDeclaredCustomAttributes()));
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != ReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Emitted;

            // define modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Defined);
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // finish modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Emitted);

            // set state
            _state = ReflectionSymbolState.Finished;

            // complete modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Finished);
        }

    }

}
