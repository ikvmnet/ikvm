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

namespace IKVM.CoreLib.Linking
{

    internal sealed class ConstantPoolItemInterfaceMethodref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemInterfaceMethodref(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, InterfaceMethodrefConstantData data) :
            base(classFile, data.Class, data.NameAndType)
        {

        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisType, LoadMode mode)
        {
            base.Link(thisType, mode);

            var wrapper = GetClassType();
            if (wrapper != null)
            {
                if (!wrapper.IsUnloadable)
                    _method = wrapper.GetInterfaceMethod(Name, Signature);

                // NOTE vmspec 5.4.3.4 clearly states that an interfacemethod may also refer to a method in Object
                _method ??= ClassFile.Context.TypeOfJavaLangObject.GetMethod(Name, Signature, false);

                if (_method != null)
                    _method.Link(mode);
            }
        }

    }

}
