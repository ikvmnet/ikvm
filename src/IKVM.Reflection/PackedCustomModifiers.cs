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

    readonly struct PackedCustomModifiers
    {

        // element 0 is the return type, the rest are the parameters
        readonly CustomModifiers[] customModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="customModifiers"></param>
        private PackedCustomModifiers(CustomModifiers[] customModifiers)
        {
            this.customModifiers = customModifiers;
        }

        public override int GetHashCode()
        {
            return Util.GetHashCode(customModifiers);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PackedCustomModifiers?;
            return other != null && Equals(other.Value);
        }

        internal bool Equals(PackedCustomModifiers other)
        {
            return Util.ArrayEquals(customModifiers, other.customModifiers);
        }

        internal CustomModifiers GetReturnTypeCustomModifiers()
        {
            if (customModifiers == null)
                return new CustomModifiers();

            return customModifiers[0];
        }

        internal CustomModifiers GetParameterCustomModifiers(int index)
        {
            if (customModifiers == null)
                return new CustomModifiers();

            return customModifiers[index + 1];
        }

        internal PackedCustomModifiers Bind(IGenericBinder binder)
        {
            if (customModifiers == null)
                return new PackedCustomModifiers();

            var expanded = new CustomModifiers[customModifiers.Length];
            for (int i = 0; i < customModifiers.Length; i++)
                expanded[i] = customModifiers[i].Bind(binder);

            return new PackedCustomModifiers(expanded);
        }

        internal bool ContainsMissingType
        {
            get
            {
                if (customModifiers != null)
                    for (int i = 0; i < customModifiers.Length; i++)
                        if (customModifiers[i].ContainsMissingType)
                            return true;

                return false;
            }
        }

        // this method make a copy of the incoming arrays (where necessary) and returns a normalized modifiers array
        internal static PackedCustomModifiers CreateFromExternal(Type[] returnOptional, Type[] returnRequired, Type[][] parameterOptional, Type[][] parameterRequired, int parameterCount)
        {
            CustomModifiers[] modifiers = null;

            Pack(ref modifiers, 0, CustomModifiers.FromReqOpt(returnRequired, returnOptional), parameterCount + 1);
            for (int i = 0; i < parameterCount; i++)
                Pack(ref modifiers, i + 1, CustomModifiers.FromReqOpt(Util.NullSafeElementAt(parameterRequired, i), Util.NullSafeElementAt(parameterOptional, i)), parameterCount + 1);

            return new PackedCustomModifiers(modifiers);
        }

        internal static PackedCustomModifiers CreateFromExternal(CustomModifiers returnTypeCustomModifiers, CustomModifiers[] parameterTypeCustomModifiers, int parameterCount)
        {
            CustomModifiers[] customModifiers = null;

            Pack(ref customModifiers, 0, returnTypeCustomModifiers, parameterCount + 1);
            if (parameterTypeCustomModifiers != null)
                for (int i = 0; i < parameterCount; i++)
                    Pack(ref customModifiers, i + 1, parameterTypeCustomModifiers[i], parameterCount + 1);

            return new PackedCustomModifiers(customModifiers);
        }

        internal static PackedCustomModifiers Wrap(CustomModifiers[] modifiers)
        {
            return new PackedCustomModifiers(modifiers);
        }

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
