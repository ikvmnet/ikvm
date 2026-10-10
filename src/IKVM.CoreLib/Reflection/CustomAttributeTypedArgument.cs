/*
  Copyright (C) 2009 Jeroen Frijters

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
using System;

namespace IKVM.Reflection
{

    readonly struct CustomAttributeTypedArgument
    {

        readonly Type _type;
        readonly object _value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        internal CustomAttributeTypedArgument(Type type, object value)
        {
            _type = type;
            _value = value;
        }

        public override bool Equals(object obj) => this == obj as CustomAttributeTypedArgument?;

        public override int GetHashCode() => _type.GetHashCode() ^ 77 * (_value == null ? 0 : _value.GetHashCode());

        public Type ArgumentType => _type;

        public object Value => _value;

        public static bool operator ==(CustomAttributeTypedArgument arg1, CustomAttributeTypedArgument arg2) =>
            arg1._type.Equals(arg2._type) && (arg1._value == arg2._value || (arg1._value != null && arg1._value.Equals(arg2._value)));

        public static bool operator !=(CustomAttributeTypedArgument arg1, CustomAttributeTypedArgument arg2) => !(arg1 == arg2);

    }

}
