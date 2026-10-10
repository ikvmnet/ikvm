using System.Reflection.Metadata;
/*
  Copyright (C) 2009 Jeroen Frijters

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
using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    sealed class FieldSignature : Signature
    {

        readonly Type _fieldType;
        readonly CustomModifiers _mods;

        internal static FieldSignature Create(Type fieldType, CustomModifiers customModifiers) => new FieldSignature(fieldType, customModifiers);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="mods"></param>
        FieldSignature(Type fieldType, CustomModifiers mods)
        {
            _fieldType = fieldType;
            _mods = mods;
        }

        public override bool Equals(object obj)
        {
            return obj is FieldSignature other
                && other._fieldType.Equals(_fieldType)
                && other._mods.Equals(_mods);
        }

        public override int GetHashCode() => _fieldType.GetHashCode() ^ _mods.GetHashCode();

        internal Type FieldType => _fieldType;

        internal CustomModifiers GetCustomModifiers() => _mods;

        internal FieldSignature ExpandTypeParameters(Type declaringType)
        {
            return new FieldSignature(
                _fieldType.BindTypeParameters(declaringType),
                _mods.Bind(declaringType));
        }

        internal static FieldSignature ReadSig(ModuleReader module, BlobReader br, IGenericContext context)
        {
            if (br.ReadByte() != FIELD)
                throw new BadImageFormatException();

            var mods = CustomModifiers.Read(module, ref br, context);
            var fieldType = ReadType(module, ref br, context);
            return new FieldSignature(fieldType, mods);
        }

        internal override void Write(ModuleBuilder module, BlobBuilder bb)
        {
            bb.WriteByte(FIELD);
            WriteCustomModifiers(module, bb, _mods);
            WriteType(module, bb, _fieldType);
        }

    }

}
