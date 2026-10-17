/*
  Copyright (C) 2010-2013 Jeroen Frijters
  Copyright (C) 2011 Marek Safar

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
    /// Holds the components of an assembly display name as parsed by <see cref="Fusion.ParseAssemblyName"/>.
    /// </summary>
    struct ParsedAssemblyName
    {

        /// <summary>
        /// The simple name of the assembly.
        /// </summary>
        internal string Name;

        /// <summary>
        /// The version string, or <see langword="null"/> if the display name does not specify one.
        /// </summary>
        internal string Version;

        /// <summary>
        /// The culture name, or <see langword="null"/> if the display name does not specify one.
        /// </summary>
        internal string Culture;

        /// <summary>
        /// The public key token as a hexadecimal string, or <see langword="null"/> if the display name specifies
        /// neither a public key nor a public key token. When the display name specifies a full public key, this holds
        /// the token computed from it.
        /// </summary>
        internal string PublicKeyToken;

        /// <summary>
        /// Whether the assembly is retargetable, or <see langword="null"/> if the display name does not say.
        /// </summary>
        internal bool? Retargetable;

        /// <summary>
        /// The processor architecture specified by the display name.
        /// </summary>
        internal ProcessorArchitecture ProcessorArchitecture;

        /// <summary>
        /// Whether the display name specifies a full public key rather than only a public key token.
        /// </summary>
        internal bool HasPublicKey;

        /// <summary>
        /// Whether the display name specifies the Windows Runtime content type.
        /// </summary>
        internal bool WindowsRuntime;

    }

}
