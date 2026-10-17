/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
    /// Supplies the replacements for generic type and method parameters when a type, method or signature is bound to
    /// a generic instantiation.
    /// </summary>
    interface IGenericBinder
    {

        /// <summary>
        /// Returns the replacement for a generic type parameter.
        /// </summary>
        /// <param name="type">The generic type parameter, identified by its <see cref="Type.GenericParameterPosition"/>.</param>
        /// <returns>The type to substitute for the parameter.</returns>
        Type BindTypeParameter(Type type);

        /// <summary>
        /// Returns the replacement for a generic method parameter.
        /// </summary>
        /// <param name="type">The generic method parameter, identified by its <see cref="Type.GenericParameterPosition"/>.</param>
        /// <returns>The type to substitute for the parameter.</returns>
        Type BindMethodParameter(Type type);

    }

}
