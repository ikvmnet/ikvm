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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        internal BakedType(TypeBuilder typeBuilder) :
            base(typeBuilder)
        {
        }

        public override string AssemblyQualifiedName => underlyingType.AssemblyQualifiedName;

        public override Type BaseType => underlyingType.BaseType;

        internal override TypeName TypeName => underlyingType.TypeName;

        public override string Name
            // we need to escape here, because TypeBuilder.Name does not escape
            => TypeNameParser.Escape(underlyingType.__Name);

        public override string FullName => GetFullName();

        public override TypeAttributes Attributes => underlyingType.Attributes;

        public override Type[] __GetDeclaredInterfaces() => underlyingType.__GetDeclaredInterfaces();

        public override MethodBase[] __GetDeclaredMethods() => underlyingType.__GetDeclaredMethods();

        public override __MethodImplMap __GetMethodImplMap() => underlyingType.__GetMethodImplMap();

        public override FieldInfo[] __GetDeclaredFields() => underlyingType.__GetDeclaredFields();

        public override EventInfo[] __GetDeclaredEvents() => underlyingType.__GetDeclaredEvents();

        public override PropertyInfo[] __GetDeclaredProperties() => underlyingType.__GetDeclaredProperties();

        public override Type[] __GetDeclaredTypes() => underlyingType.__GetDeclaredTypes();

        public override Type DeclaringType => underlyingType.DeclaringType;

        public override Type[] GetGenericArguments() => underlyingType.GetGenericArguments();

        internal override Type GetGenericTypeArgument(int index) => underlyingType.GetGenericTypeArgument(index);

        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() => underlyingType.__GetGenericArgumentsCustomModifiers();

        public override bool IsGenericType => underlyingType.IsGenericType;

        public override bool IsGenericTypeDefinition => underlyingType.IsGenericTypeDefinition;

        public override bool ContainsGenericParameters => underlyingType.ContainsGenericParameters;

        public override int MetadataToken => underlyingType.MetadataToken;

        public override Module Module => underlyingType.Module;

        internal override int GetModuleBuilderToken() => underlyingType.GetModuleBuilderToken();

        internal override bool IsBaked => true;

        protected override bool IsValueTypeImpl => underlyingType.IsValueType;

    }

}
