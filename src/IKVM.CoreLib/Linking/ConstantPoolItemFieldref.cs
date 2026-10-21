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
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a fieldRef constant.
    /// </summary>
    internal sealed class ConstantPoolItemFieldref<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItemFMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        TLinkingField? _field;
        TLinkingType? _fieldJavaType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemFieldref(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, FieldrefConstantData data) :
            base(classFile, data.Class, data.NameAndType)
        {

        }

        protected override void Validate(string name, string descriptor, int majorVersion)
        {
            if (!ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidFieldDescriptor(descriptor))
                throw new ClassFormatException("Invalid field signature \"{0}\"", descriptor);
            if (!ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidFieldName(name, new ClassFormatVersion((ushort)majorVersion, 0)))
                throw new ClassFormatException("Invalid field name \"{0}\"", name);
        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisType, LoadMode mode)
        {
            base.Link(thisType, mode);

            lock (this)
                if (_fieldJavaType != null)
                    return;

            var wrapper = GetClassType();
            if (wrapper == null)
                return;

            TLinkingField? fw = null;
            if (wrapper.IsUnloadable == false)
            {
                fw = wrapper.GetField(Name, Signature);
                if (fw != null)
                    fw.Link(mode);
            }

            var fld = thisType.GetFieldTypeFromSignature(Signature, mode);

            lock (this)
            {
                if (_fieldJavaType == null)
                {
                    _fieldJavaType = fld;
                    _field = fw;
                }
            }
        }

        /// <summary>
        /// Gets the type of the linked field.
        /// </summary>
        /// <returns></returns>
        public TLinkingType? GetFieldType()
        {
            return _fieldJavaType;
        }

        /// <summary>
        /// Gets the linked field.
        /// </summary>
        /// <returns></returns>
        public TLinkingField? GetField()
        {
            return _field;
        }

        /// <inheritdoc />
        public override TLinkingMember? GetMember()
        {
            return _field;
        }

    }

}
