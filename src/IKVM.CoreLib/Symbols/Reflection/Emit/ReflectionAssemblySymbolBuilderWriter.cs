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
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(ReflectionSymbolState state)
        {
            if (state >= ReflectionSymbolState.Declared && _state < ReflectionSymbolState.Declared)
                Declare();

            if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                Finish();

            if (state >= ReflectionSymbolState.Completed && _state < ReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Declare()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Declared;

            // define assembly
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_builder.Identity.ToAssemblyName(), AssemblyBuilderAccess.Run, _context.CreateCustomAttributeBuilders(_builder.GetDeclaredCustomAttributes()));
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Declared)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // lock builder and set state
            _builder.Freeze();
            _state = ReflectionSymbolState.Finished;

            // declare modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Declared);

        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void Complete()
        {
            if (_state != ReflectionSymbolState.Finished)
                throw new InvalidOperationException();
            if (_assemblyBuilder is null)
                throw new InvalidOperationException();

            // finish modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Finished);

            // set state
            _state = ReflectionSymbolState.Completed;

            // complete modules
            foreach (var module in _builder.GetModules())
                _context.ResolveModule(module, ReflectionSymbolState.Completed);
        }

    }

}
