/*
  Copyright (C) 2010-2012 Jeroen Frijters

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
using System.Globalization;

namespace IKVM.Reflection
{

    /// <summary>
    /// Selects a member from a list of candidates, and performs type conversion from actual argument type to formal
    /// argument type.
    /// </summary>
    abstract class Binder
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected Binder()
        {
        }

        /// <summary>
        /// Selects a method from the given set of methods, based on the argument type.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values.</param>
        /// <param name="match">The set of methods that are candidates for matching.</param>
        /// <param name="types">The parameter types used to locate a matching method.</param>
        /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
        /// <returns>The matching method, if found; otherwise, <see langword="null"/>.</returns>
        public abstract MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Selects a property from the given set of properties, based on the specified criteria.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values.</param>
        /// <param name="match">The set of properties that are candidates for matching.</param>
        /// <param name="returnType">The return value the matching property must have.</param>
        /// <param name="indexes">The index types of the property being searched for.</param>
        /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
        /// <returns>The matching property.</returns>
        public abstract PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers);

    }

}
