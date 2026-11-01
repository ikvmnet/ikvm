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

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a string constant.
    /// </summary>
    sealed class ConstantPoolItemString<TType, TMember, TField, TMethod, TTypeSystem> : ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
    {

        readonly Utf8ConstantHandle _handle;
        string _value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemString(ClassFile<TType, TMember, TField, TMethod, TTypeSystem> classFile, StringConstantData data) :
            base(classFile)
        {
            _handle = data.Value;
        }

        /// <inheritdoc />
        public override ConstantType ConstantType => ConstantType.String;

        /// <inheritdoc />
        public override void Resolve()
        {
            _value = ClassFile.GetConstantPoolUtf8String(_handle);
        }

        /// <summary>
        /// Gets the string value of the constant.
        /// </summary>
        public string Value => _value;

    }

}
