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
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a methodref or interfaceref constant.
    /// </summary>
    internal class ConstantPoolItemMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ConstantPoolItemFMI<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        TLinkingType[]? _argTypeWrappers;
        TLinkingType? _retTypeWrapper;
        protected TLinkingMethod? _method;
        protected TLinkingMethod? _invokespecialMethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="clazz"></param>
        /// <param name="nameAndTypeIndex"></param>
        public ConstantPoolItemMI(ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile, ClassConstantHandle clazz, NameAndTypeConstantHandle nameAndTypeIndex) :
            base(classFile, clazz, nameAndTypeIndex)
        {

        }

        /// <inheritdoc />
        protected override void Validate(string name, string descriptor, int majorVersion)
        {
            if (!ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidMethodDescriptor(descriptor))
                throw new ClassFormatException("Method {0} has invalid signature {1}", name, descriptor);

            if (!ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>.IsValidMethodName(name, new ClassFormatVersion((ushort)majorVersion, 0)))
            {
                if (!ReferenceEquals(name, StringConstants.INIT))
                    throw new ClassFormatException("Invalid method name \"{0}\"", name);

                if (!descriptor.EndsWith("V"))
                    throw new ClassFormatException("Method {0} has invalid signature {1}", name, descriptor);
            }
        }

        /// <inheritdoc />
        public override void Link(TLinkingType thisType, LoadMode mode)
        {
            base.Link(thisType, mode);

            lock (this)
                if (_argTypeWrappers != null)
                    return;

            var args = thisType.GetArgTypeListFromSignature(Signature, mode);
            var ret = thisType.GetReturnTypeFromSignature(Signature, mode);

            lock (this)
            {
                if (_argTypeWrappers == null)
                {
                    _argTypeWrappers = args;
                    _retTypeWrapper = ret;
                }
            }
        }

        public TLinkingType[]? GetArgTypes()
        {
            return _argTypeWrappers;
        }

        public TLinkingType? GetRetType()
        {
            return _retTypeWrapper;
        }

        public TLinkingMethod? GetMethod()
        {
            return _method;
        }

        public TLinkingMethod? GetMethodForInvokespecial()
        {
            return _invokespecialMethod ?? _method;
        }

        /// <inheritdoc />
        public override TLinkingMember? GetMember()
        {
            return _method;
        }

    }

}
