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

using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    internal sealed class ConstantPoolItemInterfaceMethodref<TTypeSystem, TType, TMember, TField, TMethod> : ConstantPoolItemMI<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemInterfaceMethodref(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, InterfaceMethodrefConstantData data) :
            base(classFile, data.Class, data.NameAndType)
        {

        }

        /// <inheritdoc />
        public override void Link(TType thisType, LoadMode mode)
        {
            base.Link(thisType, mode);

            var wrapper = GetClassType();
            if (wrapper != null)
            {
                ref readonly var ts = ref ClassFile.TypeSystem;

                if (!ts.IsUnloadable(wrapper))
                    _method = ts.GetInterfaceMethod(wrapper, Name, Signature);

                // NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
                _method ??= ts.GetMethod(ts.TypeOfJavaLangObject, Name, Signature, false);

                if (_method != null)
                    ts.LinkMethod(_method, mode);
            }
        }

    }

}
