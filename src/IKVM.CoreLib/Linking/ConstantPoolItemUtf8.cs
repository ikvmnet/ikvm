/*
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

using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    // this is only used to copy strings into "constantpool" when we see a RuntimeVisibleTypeAnnotations attribute,
    // because we need a consistent way of exposing constant pool items to the runtime and that case
    sealed class ConstantPoolItemUtf8<TTypeSystem, TType, TMember, TField, TMethod> : ConstantPoolItem<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        readonly string _value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="value"></param>
        public ConstantPoolItemUtf8(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, string value) :
            base(classFile)
        {
            _value = value;
        }

        /// <inheritdoc />
        public override object GetRuntimeValue()
        {
            return _value;
        }

    }

}
