using System;

using IKVM.ByteCode;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A dependence upon a module.
    /// </summary>
    public readonly struct ModuleRequires : IComparable<ModuleRequires>
    {

        readonly ModuleRequiresFlag _modifiers;
        readonly string _name;
        readonly ModuleVersion _version;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public ModuleRequires(ModuleRequiresFlag modifiers, string name, ModuleVersion version)
        {
            _modifiers = modifiers;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _version = version;
        }

        /// <summary>
        /// Gets the set of modifiers.
        /// </summary>
        public readonly ModuleRequiresFlag Modifiers => _modifiers;

        /// <summary>
        /// Gets the module name.
        /// </summary>
        public readonly string Name => _name;

        /// <summary>
        /// Gets the version of the module if recorded at compile-time.
        /// </summary>
        public readonly ModuleVersion Version => _version;

        /// <inheritdoc />
        public readonly int CompareTo(ModuleRequires other)
        {
            int c = _name.CompareTo(other._name);
            if (c != 0)
                return c;

            // version
            c = _version.CompareTo(other._version);
            if (c != 0)
                return c;

            // modifiers
            c = _modifiers.CompareTo(other._modifiers);
            if (c != 0)
                return c;

            return 0;
        }

        /// <inheritdoc />
        public readonly override bool Equals(object? obj)
        {
            return obj is ModuleRequires other && Equals(other);
        }

        /// <inheritdoc />
        public readonly bool Equals(ModuleRequires other)
        {
            return _modifiers == other._modifiers && _name == other._name && _version == other._version;
        }

        /// <inheritdoc />
        public readonly override int GetHashCode()
        {
            var hc = new HashCode();
            hc.Add(_modifiers);
            hc.Add(_name);
            if (_version.IsValid)
                hc.Add(_version);

            return hc.ToHashCode();
        }

    }

}
