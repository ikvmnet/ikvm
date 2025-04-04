using System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// <see cref="ModuleReference"/> implementation that accepts in-memory entries.
    /// </summary>
    internal class InMemoryModuleReference : ModuleReference
    {

        readonly Func<ModuleReference, ModuleReader>? _open;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="open"></param>
        public InMemoryModuleReference(ModuleDescriptor descriptor, Func<ModuleReference, ModuleReader>? open) :
            base(descriptor, null)
        {
            _open = open;
        }

        public override ModuleReader Open()
        {
            return _open is not null ? _open(this) : throw new NotSupportedException();
        }

    }

}
