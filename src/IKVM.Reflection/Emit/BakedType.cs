/*
  Copyright (C) 2008-2011 Jeroen Frijters

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

    sealed class BakedType : TypeInfo
    {

        internal BakedType(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {

        }

        public override string AssemblyQualifiedName
        {
            get { return underlyingType.AssemblyQualifiedName; }
        }

        public override Type BaseType
        {
            get { return underlyingType.BaseType; }
        }

        internal override TypeName TypeName
        {
            get { return underlyingType.TypeName; }
        }

        public override string Name
        {
            // we need to escape here, because TypeBuilder.Name does not escape
            get { return TypeNameParser.Escape(underlyingType.__Name); }
        }

        public override string FullName
        {
            get { return GetFullName(); }
        }

        public override TypeAttributes Attributes
        {
            get { return underlyingType.Attributes; }
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            return underlyingType.__GetDeclaredInterfaces();
        }

        public override MethodBase[] __GetDeclaredMethods()
        {
            return underlyingType.__GetDeclaredMethods();
        }

        public override __MethodImplMap __GetMethodImplMap()
        {
            return underlyingType.__GetMethodImplMap();
        }

        public override FieldInfo[] __GetDeclaredFields()
        {
            return underlyingType.__GetDeclaredFields();
        }

        public override EventInfo[] __GetDeclaredEvents()
        {
            return underlyingType.__GetDeclaredEvents();
        }

        public override PropertyInfo[] __GetDeclaredProperties()
        {
            return underlyingType.__GetDeclaredProperties();
        }

        public override Type[] __GetDeclaredTypes()
        {
            return underlyingType.__GetDeclaredTypes();
        }

        public override Type DeclaringType
        {
            get { return underlyingType.DeclaringType; }
        }

        public override bool __GetLayout(out int packingSize, out int typeSize)
        {
            return underlyingType.__GetLayout(out packingSize, out typeSize);
        }

        public override Type[] GetGenericArguments()
        {
            return underlyingType.GetGenericArguments();
        }

        internal override Type GetGenericTypeArgument(int index)
        {
            return underlyingType.GetGenericTypeArgument(index);
        }

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers()
        {
            return underlyingType.__GetGenericArgumentsCustomModifiers();
        }

        public override bool IsGenericType
        {
            get { return underlyingType.IsGenericType; }
        }

        public override bool IsGenericTypeDefinition
        {
            get { return underlyingType.IsGenericTypeDefinition; }
        }

        public override bool ContainsGenericParameters
        {
            get { return underlyingType.ContainsGenericParameters; }
        }

        public override int MetadataToken
        {
            get { return underlyingType.MetadataToken; }
        }

        public override Module Module
        {
            get { return underlyingType.Module; }
        }

        internal override int GetModuleBuilderToken()
        {
            return underlyingType.GetModuleBuilderToken();
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

        protected override bool IsValueTypeImpl
        {
            get { return underlyingType.IsValueType; }
        }
    }
}
