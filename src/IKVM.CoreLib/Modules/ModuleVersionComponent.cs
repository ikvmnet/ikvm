using System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Describes a component of a module version.
    /// </summary>
    public readonly struct ModuleVersionComponent : IComparable<ModuleVersionComponent>
    {

        public static bool operator ==(ModuleVersionComponent x, ModuleVersionComponent y) => x.Equals(y);

        public static bool operator !=(ModuleVersionComponent x, ModuleVersionComponent y) => x.Equals(y) == false;

        public static bool operator <(ModuleVersionComponent x, ModuleVersionComponent y) => Compare(x, y) < 0;

        public static bool operator >(ModuleVersionComponent x, ModuleVersionComponent y) => Compare(x, y) > 0;

        public static bool operator <=(ModuleVersionComponent x, ModuleVersionComponent y) => Compare(x, y) <= 0;

        public static bool operator >=(ModuleVersionComponent x, ModuleVersionComponent y) => Compare(x, y) >= 0;

        public static int Compare(ModuleVersionComponent x, ModuleVersionComponent y) => x.CompareTo(y);

        readonly int _integer;
        readonly string? _string;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ModuleVersionComponent(int value)
        {
            _integer = value;
            _string = null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ModuleVersionComponent(string value)
        {
            _integer = -1;
            _string = value;
        }

        /// <summary>
        /// Gets the kind of module version component.
        /// </summary>
        public readonly bool IsInteger => _string is null;

        /// <summary>
        /// Gets the integer value of the module version component.
        /// </summary>
        public readonly int AsInteger() => _string is null ? _integer : throw new InvalidOperationException();

        /// <summary>
        /// Gets the string value of the module version component.
        /// </summary>
        public readonly string AsString() => _string ?? _integer.ToString();

        /// <inheritdoc />
        public readonly int CompareTo(ModuleVersionComponent other)
        {
            // attempt integer comparison
            if (IsInteger && other.IsInteger)
                return AsInteger().CompareTo(other.AsInteger());

            // else fallback to string comparison
            return AsString().CompareTo(other.AsString());
        }

    }

}
