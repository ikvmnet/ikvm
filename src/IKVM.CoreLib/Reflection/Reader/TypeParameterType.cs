/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Base class for generic type and method parameters read from a module. Derives the base type, interfaces and
    /// value type status from the parameter's constraints.
    /// </summary>
    abstract class TypeParameterType : TypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sigElementType">The signature element type, ELEMENT_TYPE_VAR or ELEMENT_TYPE_MVAR.</param>
        protected TypeParameterType(byte sigElementType) :
            base(sigElementType)
        {

        }

        /// <summary>
        /// Gets the assembly-qualified name of the type, which is always <see langword="null"/> for a generic parameter.
        /// </summary>
        public sealed override string AssemblyQualifiedName => null;

        /// <summary>
        /// Gets whether the parameter is a value type, which is the case when it has the
        /// <see cref="GenericParameterAttributes.NotNullableValueTypeConstraint"/> constraint.
        /// </summary>
        protected sealed override bool IsValueTypeImpl =>
            (GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;

        /// <summary>
        /// Gets the base type of the parameter: its first class constraint, or <see cref="System.ValueType"/> or
        /// <see cref="object"/> when it has none.
        /// </summary>
        public sealed override Type BaseType
        {
            get
            {
                foreach (var type in GetGenericParameterConstraints())
                    if (!type.IsInterface && !type.IsGenericParameter)
                        return type;

                return IsValueType ? Module.Universe.System_ValueType : Module.Universe.System_Object;
            }
        }

        /// <summary>
        /// Gets the interface constraints of the parameter.
        /// </summary>
        /// <returns>The constraints that are interfaces.</returns>
        public override Type[] __GetDeclaredInterfaces()
        {
            var list = new List<Type>();
            foreach (var type in GetGenericParameterConstraints())
                if (type.IsInterface)
                    list.Add(type);

            return list.ToArray();
        }

        /// <summary>
        /// Gets the attributes of the parameter, which are always <see cref="TypeAttributes.Public"/>.
        /// </summary>
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <summary>
        /// Gets the fully qualified name of the type, which is always <see langword="null"/> for a generic parameter.
        /// </summary>
        public sealed override string FullName => null;

        /// <summary>
        /// Returns the name of the parameter.
        /// </summary>
        /// <returns>The name of the parameter.</returns>
        public sealed override string ToString() => Name;

        /// <summary>
        /// Gets whether any of the parameter's constraints refers to a missing type.
        /// </summary>
        protected sealed override bool ContainsMissingTypeImpl => ContainsMissingType(GetGenericParameterConstraints());

    }

}
