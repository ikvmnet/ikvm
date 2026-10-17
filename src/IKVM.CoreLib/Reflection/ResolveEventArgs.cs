/*
  Copyright (C) 2009-2013 Jeroen Frijters

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

    /// <summary>
    /// Provides data for loader resolution events, such as the assembly resolve event.
    /// </summary>
    sealed class ResolveEventArgs : EventArgs
    {

        readonly string _name;
        readonly Assembly _requestingAssembly;

        /// <summary>
        /// Initializes a new instance, specifying the name of the item to resolve and the assembly whose dependency is
        /// being resolved.
        /// </summary>
        /// <param name="name">The name of an item to resolve.</param>
        /// <param name="requestingAssembly">The assembly whose dependency is being resolved.</param>
        public ResolveEventArgs(string name, Assembly requestingAssembly)
        {
            _name = name;
            _requestingAssembly = requestingAssembly;
        }

        /// <summary>
        /// Gets the name of the item to resolve.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the assembly whose dependency is being resolved.
        /// </summary>
        public Assembly RequestingAssembly => _requestingAssembly;

    }

}
