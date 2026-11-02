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
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a methodref or interfaceref constant.
    /// </summary>
    internal class ConstantPoolItemMI<TTypeSystem, TType, TMember, TField, TMethod> : ConstantPoolItemFMI<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        TType[]? _argTypeWrappers;
        TType? _retTypeWrapper;
        protected TMethod? _method;
        protected TMethod? _invokespecialMethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="clazz"></param>
        /// <param name="nameAndTypeIndex"></param>
        public ConstantPoolItemMI(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, ClassConstantHandle clazz, NameAndTypeConstantHandle nameAndTypeIndex) :
            base(classFile, clazz, nameAndTypeIndex)
        {

        }

        /// <inheritdoc />
        protected override void Validate(string name, string descriptor, int majorVersion)
        {
            if (!ClassFile<TTypeSystem, TType, TMember, TField, TMethod>.IsValidMethodDescriptor(descriptor))
                throw new ClassFormatException("Method {0} has invalid signature {1}", name, descriptor);

            if (!ClassFile<TTypeSystem, TType, TMember, TField, TMethod>.IsValidMethodName(name, new ClassFormatVersion((ushort)majorVersion, 0)))
            {
                if (!ReferenceEquals(name, StringConstants.INIT))
                    throw new ClassFormatException("Invalid method name \"{0}\"", name);

                if (!descriptor.EndsWith("V"))
                    throw new ClassFormatException("Method {0} has invalid signature {1}", name, descriptor);
            }
        }

        /// <inheritdoc />
        public override void Link(TType thisType, LoadMode mode)
        {
            base.Link(thisType, mode);

            lock (this)
                if (_argTypeWrappers != null)
                    return;

            var args = ClassFile.TypeSystem.GetArgTypeListFromSignature(thisType, Signature, mode);
            var ret = ClassFile.TypeSystem.GetReturnTypeFromSignature(thisType, Signature, mode);

            lock (this)
            {
                if (_argTypeWrappers == null)
                {
                    _argTypeWrappers = args;
                    _retTypeWrapper = ret;
                }
            }
        }

        public TType[]? GetArgTypes()
        {
            return _argTypeWrappers;
        }

        public TType? GetRetType()
        {
            return _retTypeWrapper;
        }

        public TMethod? GetMethod()
        {
            return _method;
        }

        public TMethod? GetMethodForInvokespecial()
        {
            return _invokespecialMethod ?? _method;
        }

        /// <inheritdoc />
        public override TMember? GetMember()
        {
            return _method;
        }

    }

}
