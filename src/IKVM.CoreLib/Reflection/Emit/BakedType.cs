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

    /// <summary>
    /// Represents a completed <see cref="TypeBuilder"/>, as returned by <see cref="TypeBuilder.CreateType"/>. Members are
    /// forwarded to the underlying type builder, but the type reports itself as baked.
    /// </summary>
    sealed class BakedType : TypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder">The completed type builder that this type represents.</param>
        internal BakedType(TypeBuilder typeBuilder) :
            base(typeBuilder)
        {
        }

        /// <inheritdoc />
        public override string AssemblyQualifiedName => underlyingType.AssemblyQualifiedName;

        /// <inheritdoc />
        public override Type BaseType => underlyingType.BaseType;

        /// <inheritdoc />
        internal override TypeName TypeName => underlyingType.TypeName;

        /// <summary>
        /// Gets the name of the type, escaped for use in a type name string.
        /// </summary>
        public override string Name
            // we need to escape here, because TypeBuilder.Name does not escape
            => TypeNameParser.Escape(underlyingType.__Name);

        /// <inheritdoc />
        public override string FullName => GetFullName();

        /// <inheritdoc />
        public override TypeAttributes Attributes => underlyingType.Attributes;

        /// <inheritdoc />
        public override Type[] __GetDeclaredInterfaces() => underlyingType.__GetDeclaredInterfaces();

        /// <inheritdoc />
        public override MethodBase[] __GetDeclaredMethods() => underlyingType.__GetDeclaredMethods();

        /// <inheritdoc />
        public override __MethodImplMap __GetMethodImplMap() => underlyingType.__GetMethodImplMap();

        /// <inheritdoc />
        public override FieldInfo[] __GetDeclaredFields() => underlyingType.__GetDeclaredFields();

        /// <inheritdoc />
        public override EventInfo[] __GetDeclaredEvents() => underlyingType.__GetDeclaredEvents();

        /// <inheritdoc />
        public override PropertyInfo[] __GetDeclaredProperties() => underlyingType.__GetDeclaredProperties();

        /// <inheritdoc />
        public override Type[] __GetDeclaredTypes() => underlyingType.__GetDeclaredTypes();

        /// <inheritdoc />
        public override Type DeclaringType => underlyingType.DeclaringType;

        /// <inheritdoc />
        public override Type[] GetGenericArguments() => underlyingType.GetGenericArguments();

        /// <inheritdoc />
        internal override Type GetGenericTypeArgument(int index) => underlyingType.GetGenericTypeArgument(index);

        /// <inheritdoc />
        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() => underlyingType.__GetGenericArgumentsCustomModifiers();

        /// <inheritdoc />
        public override bool IsGenericType => underlyingType.IsGenericType;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => underlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public override bool ContainsGenericParameters => underlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public override int MetadataToken => underlyingType.MetadataToken;

        /// <inheritdoc />
        public override Module Module => underlyingType.Module;

        /// <inheritdoc />
        internal override int GetModuleBuilderToken() => underlyingType.GetModuleBuilderToken();

        /// <summary>
        /// Gets a value indicating whether the type is complete, which is always <see langword="true"/>.
        /// </summary>
        internal override bool IsBaked => true;

        /// <inheritdoc />
        protected override bool IsValueTypeImpl => underlyingType.IsValueType;

    }

}
