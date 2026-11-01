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
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a methodtype constant.
    /// </summary>
    internal sealed class ConstantPoolItemMethodType<TType, TMember, TField, TMethod, TTypeSystem> : ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
    {

        readonly Utf8ConstantHandle _signature;

        string? _descriptor;
        TType[]? _argTypeWrappers;
        TType? _retTypeWrapper;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemMethodType(ClassFile<TType, TMember, TField, TMethod, TTypeSystem> classFile, MethodTypeConstantData data) :
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
            if (descriptor == null || !ClassFile<TType, TMember, TField, TMethod, TTypeSystem>.IsValidMethodDescriptor(descriptor))
                throw new ClassFormatException("Invalid MethodType signature");

            _descriptor = string.Intern(descriptor.Replace('/', '.'));
        }

        /// <inheritdoc />
        public override void Link(TType thisType, LoadMode mode)
        {
            lock (this)
                if (_argTypeWrappers != null)
                    return;

            var args = ClassFile.TypeSystem.GetArgTypeListFromSignature(thisType, _descriptor, mode);
            var ret = ClassFile.TypeSystem.GetReturnTypeFromSignature(thisType, _descriptor, mode);

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

        public TType[] GetArgTypes() => _argTypeWrappers;

        public TType GetRetType() => _retTypeWrapper;

    }

}
