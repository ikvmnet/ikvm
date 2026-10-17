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
    /// Type-model representation of a methodtype constant.
    /// </summary>
    internal sealed class ConstantPoolItemMethodType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItem<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        readonly Utf8ConstantHandle _signature;

        string? _descriptor;
        TLinkingType[]? _argTypeWrappers;
        TLinkingType? _retTypeWrapper;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemMethodType(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, MethodTypeConstantData data) :
            base(classFile)
        {
            _signature = data.Descriptor;
        }

        /// <inheritdoc />
        public override ConstantType ConstantType => ConstantType.MethodType;

        /// <inheritdoc />
        public override void Resolve()
        {
            var descriptor = ClassFile.GetConstantPoolUtf8String(_signature);
            if (descriptor == null || !ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidMethodDescriptor(descriptor))
                throw new ClassFormatException("Invalid MethodType signature");

            _descriptor = string.Intern(descriptor.Replace('/', '.'));
        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisType, LoadMode mode)
        {
            lock (this)
                if (_argTypeWrappers != null)
                    return;

            var args = thisType.GetArgTypeListFromSignature(_descriptor, mode);
            var ret = thisType.GetReturnTypeFromSignature(_descriptor, mode);

            lock (this)
            {
                if (_argTypeWrappers == null)
                {
                    _argTypeWrappers = args;
                    _retTypeWrapper = ret;
                }
            }
        }

        public string Signature => _descriptor;

        public TLinkingType[] GetArgTypes() => _argTypeWrappers;

        public TLinkingType GetRetType() => _retTypeWrapper;

    }

}
