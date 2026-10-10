/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
    /// Helpers that query a <see cref="System.Type"/> from the runtime's own reflection, for code that needs to inspect
    /// a real CLR type alongside IKVM.Reflection types.
    /// </summary>
    static class TypeUtil
    {

        /// <summary>
        /// Gets a value indicating whether the specified runtime type is an enumeration.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is an enumeration; otherwise, <see langword="false"/>.</returns>
        internal static bool IsEnum(System.Type type) => type.IsEnum;

        /// <summary>
        /// Gets the assembly in which the specified runtime type is declared.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns>The assembly that declares <paramref name="type"/>.</returns>
        internal static System.Reflection.Assembly GetAssembly(System.Type type) => type.Assembly;

        /// <summary>
        /// Gets the generic method that declares the specified runtime generic type parameter.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns>The declaring generic method if <paramref name="type"/> is a method type parameter; otherwise, <see langword="null"/>.</returns>
        internal static System.Reflection.MethodBase GetDeclaringMethod(System.Type type) => type.DeclaringMethod;

        /// <summary>
        /// Gets a value indicating whether the specified runtime type is a generic type.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is a generic type; otherwise, <see langword="false"/>.</returns>
        internal static bool IsGenericType(System.Type type) => type.IsGenericType;

        /// <summary>
        /// Gets a value indicating whether the specified runtime type is a generic type definition.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is a generic type definition; otherwise, <see langword="false"/>.</returns>
        internal static bool IsGenericTypeDefinition(System.Type type) => type.IsGenericTypeDefinition;

        /// <summary>
        /// Gets the type arguments of a closed generic runtime type or the type parameters of a generic type definition.
        /// </summary>
        /// <param name="type">The runtime type to inspect.</param>
        /// <returns>The generic type arguments or parameters of <paramref name="type"/>, or an empty array if it is not generic.</returns>
        internal static System.Type[] GetGenericArguments(System.Type type) => type.GetGenericArguments();

    }

}
