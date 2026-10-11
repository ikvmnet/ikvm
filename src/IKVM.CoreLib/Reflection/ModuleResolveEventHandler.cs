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
namespace IKVM.Reflection
{

    /// <summary>
    /// Represents the method that resolves a module of an <see cref="Assembly"/> that cannot be found by other means.
    /// The assembly reader consults these handlers when a module listed in the manifest cannot be loaded.
    /// </summary>
    /// <param name="sender">The assembly that requests the module.</param>
    /// <param name="args">The arguments describing the module to resolve.</param>
    /// <returns>The module that satisfies the request, or <see langword="null"/> if the handler cannot resolve it.</returns>
    delegate Module ModuleResolveEventHandler(object sender, ResolveEventArgs args);

}
