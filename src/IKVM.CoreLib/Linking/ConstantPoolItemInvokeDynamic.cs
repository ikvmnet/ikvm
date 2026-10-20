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
    /// Type-model representation of a invokedynamic constant.
    /// </summary>
    internal sealed class ConstantPoolItemInvokeDynamic<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItem<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        readonly ushort _bootstrapMethodAttributeIndex;
        readonly NameAndTypeConstantHandle _nameAndTypeHandle;

        string? _name;
        string? _descriptor;
        TLinkingType[]? _argTypes;
        TLinkingType? _returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemInvokeDynamic(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, InvokeDynamicConstantData data) :
            base(classFile)
        {
            _bootstrapMethodAttributeIndex = data.BootstrapMethodAttributeIndex;
            _nameAndTypeHandle = data.NameAndType;
        }

        /// <inheritdoc />
        public override void Resolve()
        {
            var nameAndType = (ConstantPoolItemNameAndType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>)ClassFile.GetConstantPoolItem(_nameAndTypeHandle);
            if (nameAndType == null)
                throw new ClassFormatException("Bad index in constant pool");

            _name = string.Intern(ClassFile.GetConstantPoolUtf8String(nameAndType._nameHandle));
            _descriptor = string.Intern(ClassFile.GetConstantPoolUtf8String(nameAndType._descriptorHandle).Replace('/', '.'));
        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisType, LoadMode mode)
        {
            lock (this)
                if (_argTypes != null)
                    return;

            var args = thisType.GetArgTypeListFromSignature(_descriptor, mode);
            var ret = thisType.GetReturnTypeFromSignature(_descriptor, mode);

            lock (this)
            {
                if (_argTypes == null)
                {
                    _argTypes = args;
                    _returnType = ret;
                }
            }
        }

        public TLinkingType[] GetArgTypes()
        {
            return _argTypes;
        }

        public TLinkingType GetRetType()
        {
            return _returnType;
        }

        public string Name => _name;

        public string Signature => _descriptor;

        public ushort BootstrapMethod => _bootstrapMethodAttributeIndex;

    }

}
