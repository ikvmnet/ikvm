/*
  Copyright (C) 2011-2012 Jeroen Frijters

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
namespace IKVM.Reflection
{
    sealed class MissingField : FieldInfo
    {

        readonly Type declaringType;
        readonly string name;
        readonly FieldSignature signature;
        FieldInfo forwarder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        internal MissingField(Type declaringType, string name, FieldSignature signature)
        {
            this.declaringType = declaringType;
            this.name = name;
            this.signature = signature;
        }

        FieldInfo Forwarder => TryGetForwarder() ?? throw new MissingMemberException(this);

        FieldInfo TryGetForwarder()
        {
            if (forwarder == null && !declaringType.__IsMissing)
                forwarder = declaringType.FindField(name, signature);

            return forwarder;
        }

        public override bool __IsMissing
        {
            get { return TryGetForwarder() == null; }
        }

        public override FieldAttributes Attributes
        {
            get { return Forwarder.Attributes; }
        }

        public override void __GetDataFromRVA(byte[] data, int offset, int length)
        {
            Forwarder.__GetDataFromRVA(data, offset, length);
        }

        public override int __FieldRVA
        {
            get { return Forwarder.__FieldRVA; }
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            return Forwarder.__TryGetFieldOffset(out offset);
        }

        public override object GetRawConstantValue()
        {
            return Forwarder.GetRawConstantValue();
        }

        internal override FieldSignature FieldSignature
        {
            get { return signature; }
        }

        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            var field = TryGetForwarder();
            if (field != null)
                return field.ImportTo(module);

            return module.ImportMethodOrField(declaringType, this.Name, this.FieldSignature);
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { return declaringType.IsModulePseudoType ? null : declaringType; }
        }

        public override Module Module
        {
            get { return declaringType.Module; }
        }

        internal override FieldInfo BindTypeParameters(Type type)
        {
            var forwarder = TryGetForwarder();
            if (forwarder != null)
                return forwarder.BindTypeParameters(type);

            return new GenericFieldInstance(type, this);
        }

        public override int MetadataToken
        {
            get { return Forwarder.MetadataToken; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as MissingField;
            return other != null
                && other.declaringType == declaringType
                && other.name == name
                && other.signature.Equals(signature);
        }

        public override int GetHashCode()
        {
            return declaringType.GetHashCode() ^ name.GetHashCode() ^ signature.GetHashCode();
        }

        public override string ToString()
        {
            return FieldType.Name + " " + Name;
        }

        internal override int GetCurrentToken()
        {
            return Forwarder.GetCurrentToken();
        }

        internal override bool IsBaked
        {
            get { return Forwarder.IsBaked; }
        }

    }

}
