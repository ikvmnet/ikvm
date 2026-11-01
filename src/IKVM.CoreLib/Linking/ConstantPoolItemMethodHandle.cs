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
    /// Type-model representation of a methodhandle constant.
    /// </summary>
    internal sealed class ConstantPoolItemMethodHandle<TTypeSystem, TType, TMember, TField, TMethod> : ConstantPoolItem<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        readonly MethodHandleConstantData _data;
        ConstantPoolItemFMI<TTypeSystem, TType, TMember, TField, TMethod>? _cpi;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        internal ConstantPoolItemMethodHandle(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, MethodHandleConstantData data) :
            base(classFile)
        {
            this._data = data;
        }

        /// <inheritdoc />
        public override ConstantType ConstantType => ConstantType.MethodHandle;

        /// <inheritdoc />
        public override void Resolve()
        {
            switch (_data.Kind)
            {
                case MethodHandleKind.GetField:
                case MethodHandleKind.GetStatic:
                case MethodHandleKind.PutField:
                case MethodHandleKind.PutStatic:
                    _cpi = ClassFile.GetConstantPoolItem(_data.Reference) as ConstantPoolItemFieldref<TTypeSystem, TType, TMember, TField, TMethod>;
                    break;
                case MethodHandleKind.InvokeSpecial:
                case MethodHandleKind.InvokeVirtual:
                case MethodHandleKind.InvokeStatic:
                case MethodHandleKind.NewInvokeSpecial:
                    _cpi = ClassFile.GetConstantPoolItem(_data.Reference) as ConstantPoolItemMethodref<TTypeSystem, TType, TMember, TField, TMethod>;
                    if (_cpi == null && ClassFile.MajorVersion >= 52 && (_data.Kind is MethodHandleKind.InvokeStatic or MethodHandleKind.InvokeSpecial))
                        goto case MethodHandleKind.InvokeInterface;
                    break;
                case MethodHandleKind.InvokeInterface:
                    _cpi = ClassFile.GetConstantPoolItem(_data.Reference) as ConstantPoolItemInterfaceMethodref<TTypeSystem, TType, TMember, TField, TMethod>;
                    break;
            }

            if (_cpi == null)
                throw new ClassFormatException("Invalid constant pool item MethodHandle");

            if (ReferenceEquals(_cpi.Name, StringConstants.INIT) && _data.Kind != MethodHandleKind.NewInvokeSpecial)
                throw new ClassFormatException("Bad method name");
        }

        /// <inheritdoc />
        public override void MarkLinkRequired()
        {
            _cpi.MarkLinkRequired();
        }

        /// <inheritdoc />
        public override void Link(TType thisType, LoadMode mode)
        {
            _cpi.Link(thisType, mode);
        }

        public string? Class => _cpi.Class;

        public string? Name => _cpi.Name;

        public string? Signature => _cpi.Signature;

        public ConstantPoolItemFMI<TTypeSystem, TType, TMember, TField, TMethod> MemberConstantPoolItem => _cpi;

        public MethodHandleKind Kind => _data.Kind;

        public TMember? Member => _cpi.GetMember();

        public TType GetClassType()
        {
            return _cpi.GetClassType();
        }

    }

}
