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
    /// Supplies the generic type and method arguments used to resolve generic parameters while decoding signatures.
    /// </summary>
    interface IGenericContext
    {

        /// <summary>
        /// Gets the generic type argument at the specified position.
        /// </summary>
        /// <param name="index">The zero-based position of the generic type parameter.</param>
        /// <returns>The type that substitutes for the generic type parameter at <paramref name="index"/>.</returns>
        Type GetGenericTypeArgument(int index);

        /// <summary>
        /// Gets the generic method argument at the specified position.
        /// </summary>
        /// <param name="index">The zero-based position of the generic method parameter.</param>
        /// <returns>The type that substitutes for the generic method parameter at <paramref name="index"/>.</returns>
        Type GetGenericMethodArgument(int index);

    }

}
