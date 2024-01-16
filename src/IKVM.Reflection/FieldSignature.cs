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

        readonly Type fieldType;
        readonly CustomModifiers mods;

        internal static FieldSignature Create(Type fieldType, CustomModifiers customModifiers)
        {
            return new FieldSignature(fieldType, customModifiers);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="mods"></param>
        private FieldSignature(Type fieldType, CustomModifiers mods)
        {
            this.fieldType = fieldType;
            this.mods = mods;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FieldSignature;
            return other != null
                && other.fieldType.Equals(fieldType)
                && other.mods.Equals(mods);
        }

        public override int GetHashCode()
        {
            return fieldType.GetHashCode() ^ mods.GetHashCode();
        }

        internal Type FieldType
        {
            get { return fieldType; }
        }

        internal CustomModifiers GetCustomModifiers()
        {
            return mods;
        }

        internal FieldSignature ExpandTypeParameters(Type declaringType)
        {
            return new FieldSignature(
                fieldType.BindTypeParameters(declaringType),
                mods.Bind(declaringType));
        }

        internal static FieldSignature ReadSig(ModuleReader module, ByteReader br, IGenericContext context)
        {
            if (br.ReadByte() != FIELD)
                throw new BadImageFormatException();

            var mods = CustomModifiers.Read(module, br, context);
            var fieldType = ReadType(module, br, context);
            return new FieldSignature(fieldType, mods);
        }

        internal override void Write(ModuleBuilder module, ByteBuffer bb)
        {
            bb.Write(FIELD);
            WriteCustomModifiers(module, bb, mods);
            WriteType(module, bb, fieldType);
        }

    }

}
