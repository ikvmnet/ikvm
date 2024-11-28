using System.IO;
using System.Threading;

using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection
{

    public class IkvmReflectionSymbolTestInit : SymbolTestInit<IkvmReflectionSymbolContext>
    {

        Universe? _universe;
        IkvmReflectionSymbolContext? _symbols;

        /// <summary>
        /// Gets the universe of types.
        /// </summary>
        public Universe Universe
        {
            get
            {
                if (_universe == null)
                {
                    var universe = new Universe(typeof(object).Assembly.GetName().Name);
                    universe.AssemblyResolve += Universe_AssemblyResolve;
                    Interlocked.CompareExchange(ref _universe, universe, null);
                }

                return _universe;
            }
        }

        /// <summary>
        /// Gets the symbol context.
        /// </summary>
        public override IkvmReflectionSymbolContext Symbols
        {
            get
            {
                if (_symbols == null)
                {
                    var coreAssembly = Universe.LoadFile(typeof(object).Assembly.GetAssemblyLocation());
                    var thisAssembly = Universe.LoadFile(typeof(IkvmReflectionModuleSymbolTests).Assembly.GetAssemblyLocation());
                    var symbols = new IkvmReflectionSymbolContext(Universe!, new IkvmReflectionSymbolOptions(true));
                    Interlocked.CompareExchange(ref _symbols, symbols, null);
                }

                return _symbols;
            }
        }

        /// <summary>
        /// Attempt to load assembly from system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly? Universe_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var asm = System.Reflection.Assembly.Load(args.Name);
                if (asm != null && File.Exists(asm.Location))
                    return _universe!.LoadFile(asm.Location);
            }
            catch
            {

            }

            return null;
        }

    }

}
