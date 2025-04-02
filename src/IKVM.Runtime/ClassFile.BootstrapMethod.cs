﻿/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

using IKVM.ByteCode;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal struct BootstrapMethod
        {

            readonly MethodHandleConstantHandle _method;
            readonly ConstantHandle[] _args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="args"></param>
            internal BootstrapMethod(MethodHandleConstantHandle method, ConstantHandle[] args)
            {
                _method = method;
                _args = args;
            }

            /// <summary>
            /// Gets the handle to the bootstrap method.
            /// </summary>
            public readonly MethodHandleConstantHandle BootstrapMethodIndex => _method;

            /// <summary>
            /// Gets the argument count.
            /// </summary>
            public readonly int ArgumentCount => _args.Length;

            /// <summary>
            /// Gets the argument at the specified index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public readonly ConstantHandle GetArgument(int index)
            {
                return _args[index];
            }

        }

    }

}
