/*
  Copyright (C) 2010 Jeroen Frijters

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
namespace IKVM.Reflection.Emit
{

    public sealed class EnumBuilder : TypeInfo
    {

        readonly TypeBuilder typeBuilder;
        readonly FieldBuilder fieldBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="fieldBuilder"></param>
        internal EnumBuilder(TypeBuilder typeBuilder, FieldBuilder fieldBuilder) :
            base(typeBuilder)
        {
            this.typeBuilder = typeBuilder;
            this.fieldBuilder = fieldBuilder;
        }

        internal override TypeName TypeName
        {
            get { return typeBuilder.TypeName; }
        }

        public override string Name
        {
            get { return typeBuilder.Name; }
        }

        public override string FullName
        {
            get { return typeBuilder.FullName; }
        }

        public override Type BaseType
        {
            get { return typeBuilder.BaseType; }
        }

        public override TypeAttributes Attributes
        {
            get { return typeBuilder.Attributes; }
        }

        public override Module Module
        {
            get { return typeBuilder.Module; }
        }

        public FieldBuilder DefineLiteral(string literalName, object literalValue)
        {
            FieldBuilder fb = typeBuilder.DefineField(literalName, typeBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
            fb.SetConstant(literalValue);
            return fb;
        }

        public Type CreateType()
        {
            return typeBuilder.CreateType();
        }

        public TypeInfo CreateTypeInfo()
        {
            return typeBuilder.CreateTypeInfo();
        }

        public TypeToken TypeToken
        {
            get { return typeBuilder.TypeToken; }
        }

        public FieldBuilder UnderlyingField
        {
            get { return fieldBuilder; }
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            typeBuilder.SetCustomAttribute(con, binaryAttribute);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            typeBuilder.SetCustomAttribute(customBuilder);
        }

        public override Type GetEnumUnderlyingType()
        {
            return fieldBuilder.FieldType;
        }

        protected override bool IsValueTypeImpl
        {
            get { return true; }
        }

        internal override bool IsBaked
        {
            get { return typeBuilder.IsBaked; }
        }

    }

}
