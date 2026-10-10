/*
  Copyright (C) 2009-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
namespace IKVM.Reflection
{

    /// <summary>
    /// Holds the custom modifiers of a method signature's return type and parameters in a single array. The array is
    /// <see langword="null"/> when no position has any custom modifiers, so signatures without modifiers cost nothing.
    /// </summary>
    readonly struct PackedCustomModifiers
    {

        /// <summary>
        /// The custom modifiers by signature position: element 0 is the return type and the rest are the parameters.
        /// <see langword="null"/> when there are no custom modifiers at all.
        /// </summary>
        readonly CustomModifiers[] _customModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="customModifiers">The custom modifiers by signature position, or <see langword="null"/> if there are none.</param>
        PackedCustomModifiers(CustomModifiers[] customModifiers)
        {
            _customModifiers = customModifiers;
        }

        /// <inheritdoc />
        public override int GetHashCode() => Util.GetHashCode(_customModifiers);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = obj as PackedCustomModifiers?;
            return other != null && Equals(other.Value);
        }

        /// <summary>
        /// Determines whether this instance holds the same custom modifiers as another.
        /// </summary>
        /// <param name="other">The instance to compare with.</param>
        /// <returns><see langword="true"/> if the custom modifiers are equal at every position; otherwise, <see langword="false"/>.</returns>
        internal bool Equals(PackedCustomModifiers other) => Util.ArrayEquals(_customModifiers, other._customModifiers);

        /// <summary>
        /// Gets the custom modifiers of the return type.
        /// </summary>
        /// <returns>The custom modifiers of the return type, which are empty if there are none.</returns>
        internal CustomModifiers GetReturnTypeCustomModifiers()
        {
            if (_customModifiers == null)
                return new CustomModifiers();

            return _customModifiers[0];
        }

        /// <summary>
        /// Gets the custom modifiers of a parameter.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>The custom modifiers of the parameter, which are empty if there are none.</returns>
        internal CustomModifiers GetParameterCustomModifiers(int index)
        {
            if (_customModifiers == null)
                return new CustomModifiers();

            return _customModifiers[index + 1];
        }

        /// <summary>
        /// Substitutes generic type and method arguments into the custom modifiers.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>A new instance with every custom modifier bound.</returns>
        internal PackedCustomModifiers Bind(IGenericBinder binder)
        {
            if (_customModifiers == null)
                return new PackedCustomModifiers();

            var expanded = new CustomModifiers[_customModifiers.Length];
            for (var i = 0; i < _customModifiers.Length; i++)
                expanded[i] = _customModifiers[i].Bind(binder);

            return new PackedCustomModifiers(expanded);
        }

        /// <summary>
        /// Gets a value indicating whether any of the custom modifiers refers to a type that could not be resolved.
        /// </summary>
        internal bool ContainsMissingType
        {
            get
            {
                if (_customModifiers != null)
                    for (var i = 0; i < _customModifiers.Length; i++)
                        if (_customModifiers[i].ContainsMissingType)
                            return true;

                return false;
            }
        }

        /// <summary>
        /// Creates an instance from caller-supplied required and optional modifier types. The incoming arrays are copied
        /// where necessary, so the caller may reuse them.
        /// </summary>
        /// <param name="returnOptional">The optional custom modifiers of the return type, or <see langword="null"/>.</param>
        /// <param name="returnRequired">The required custom modifiers of the return type, or <see langword="null"/>.</param>
        /// <param name="parameterOptional">The optional custom modifiers of each parameter, or <see langword="null"/>.</param>
        /// <param name="parameterRequired">The required custom modifiers of each parameter, or <see langword="null"/>.</param>
        /// <param name="parameterCount">The number of parameters in the signature.</param>
        /// <returns>The packed custom modifiers.</returns>
        internal static PackedCustomModifiers CreateFromExternal(Type[] returnOptional, Type[] returnRequired, Type[][] parameterOptional, Type[][] parameterRequired, int parameterCount)
        {
            CustomModifiers[] modifiers = null;

            Pack(ref modifiers, 0, CustomModifiers.FromReqOpt(returnRequired, returnOptional), parameterCount + 1);
            for (var i = 0; i < parameterCount; i++)
                Pack(ref modifiers, i + 1, CustomModifiers.FromReqOpt(Util.NullSafeElementAt(parameterRequired, i), Util.NullSafeElementAt(parameterOptional, i)), parameterCount + 1);

            return new PackedCustomModifiers(modifiers);
        }

        /// <summary>
        /// Creates an instance from caller-supplied <see cref="CustomModifiers"/> values.
        /// </summary>
        /// <param name="returnTypeCustomModifiers">The custom modifiers of the return type.</param>
        /// <param name="parameterTypeCustomModifiers">The custom modifiers of each parameter, or <see langword="null"/>.</param>
        /// <param name="parameterCount">The number of parameters in the signature.</param>
        /// <returns>The packed custom modifiers.</returns>
        internal static PackedCustomModifiers CreateFromExternal(CustomModifiers returnTypeCustomModifiers, CustomModifiers[] parameterTypeCustomModifiers, int parameterCount)
        {
            CustomModifiers[] customModifiers = null;

            Pack(ref customModifiers, 0, returnTypeCustomModifiers, parameterCount + 1);
            if (parameterTypeCustomModifiers != null)
                for (var i = 0; i < parameterCount; i++)
                    Pack(ref customModifiers, i + 1, parameterTypeCustomModifiers[i], parameterCount + 1);

            return new PackedCustomModifiers(customModifiers);
        }

        /// <summary>
        /// Wraps an existing array of custom modifiers without copying it.
        /// </summary>
        /// <param name="modifiers">The custom modifiers by signature position, with element 0 for the return type, or <see langword="null"/>.</param>
        /// <returns>An instance that uses <paramref name="modifiers"/> directly.</returns>
        internal static PackedCustomModifiers Wrap(CustomModifiers[] modifiers) => new PackedCustomModifiers(modifiers);

        /// <summary>
        /// Stores custom modifiers at a position of the array, allocating the array on the first non-empty value so that
        /// it stays <see langword="null"/> when every position is empty.
        /// </summary>
        /// <param name="array">The array to store into; allocated if <see langword="null"/> and <paramref name="mods"/> is not empty.</param>
        /// <param name="index">The signature position to store at.</param>
        /// <param name="mods">The custom modifiers to store.</param>
        /// <param name="count">The length of the array to allocate.</param>
        internal static void Pack(ref CustomModifiers[] array, int index, CustomModifiers mods, int count)
        {
            if (!mods.IsEmpty)
            {
                array ??= new CustomModifiers[count];
                array[index] = mods;
            }
        }

    }

}
