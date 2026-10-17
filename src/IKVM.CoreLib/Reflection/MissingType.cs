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
using System;

namespace IKVM.Reflection
{

    /// <summary>
    /// Stands in for a type that is referenced but could not be found, when the universe is configured to tolerate
    /// missing members. Only the name, declaring type and module are known; members are synthesized on demand as
    /// missing members, and querying anything that requires the definition throws <see cref="MissingMemberException"/>.
    /// </summary>
    sealed class MissingType : Type
    {

        readonly Module _module;
        readonly Type _declaringType;
        readonly string _ns;
        readonly string _name;
        Type[] _typeArgs;
        int _token;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module the missing type is expected to be defined in.</param>
        /// <param name="declaringType">The declaring type if the missing type is nested; otherwise, <see langword="null"/>.</param>
        /// <param name="ns">The namespace of the missing type, or <see langword="null"/> if it has none.</param>
        /// <param name="name">The unescaped name of the missing type.</param>
        internal MissingType(Module module, Type declaringType, string ns, string name)
        {
            _module = module;
            _declaringType = declaringType;
            _ns = ns;
            _name = name;
            MarkKnownType(ns, name);
        }

        /// <summary>
        /// Creates a missing method or constructor with the specified name and signature, since the members of a
        /// missing type cannot be looked up.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="signature">The signature of the method.</param>
        /// <returns>A <see cref="MissingMethod"/>, wrapped in a <see cref="ConstructorInfoImpl"/> if <paramref name="name"/> is <c>.ctor</c>.</returns>
        internal override MethodBase FindMethod(string name, MethodSignature signature)
        {
            var method = new MissingMethod(this, name, signature);
            if (name == ".ctor")
                return new ConstructorInfoImpl(method);

            return method;
        }

        /// <summary>
        /// Creates a missing field with the specified name and signature, since the members of a missing type cannot be
        /// looked up.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="signature">The signature of the field.</param>
        /// <returns>A new <see cref="MissingField"/>.</returns>
        internal override FieldInfo FindField(string name, FieldSignature signature) => new MissingField(this, name, signature);

        /// <summary>
        /// Finds a nested type by name. A missing type has no known nested types.
        /// </summary>
        /// <param name="name">The name of the nested type.</param>
        /// <returns><see langword="null"/>.</returns>
        internal override Type FindNestedType(TypeName name) => null;

        /// <summary>
        /// Finds a nested type by name, ignoring case. A missing type has no known nested types.
        /// </summary>
        /// <param name="lowerCaseName">The lower-cased name of the nested type.</param>
        /// <returns><see langword="null"/>.</returns>
        internal override Type FindNestedTypeIgnoreCase(TypeName lowerCaseName) => null;

        /// <summary>
        /// Gets a value indicating that this type is missing, which is always <see langword="true"/>.
        /// </summary>
        public override bool __IsMissing => true;

        /// <inheritdoc />
        public override Type DeclaringType => _declaringType;

        /// <inheritdoc />
        internal override TypeName TypeName => new TypeName(_ns, _name);

        /// <inheritdoc />
        public override string Name => TypeNameParser.Escape(_name);

        /// <inheritdoc />
        public override string FullName => GetFullName();

        /// <inheritdoc />
        public override Module Module => _module;

        /// <summary>
        /// Gets the token recorded by <see cref="SetMetadataTokenForMissing(int)"/>, or 0 if none was recorded.
        /// </summary>
        public override int MetadataToken => _token;

        /// <summary>
        /// Gets whether the type is a value type, as recorded from the signatures that referenced it.
        /// </summary>
        /// <exception cref="MissingMemberException">Signatures have marked the type as neither or as both a value type and a reference type.</exception>
        protected override bool IsValueTypeImpl
        {
            get
            {
                return (typeFlags & (TypeFlags.ValueType | TypeFlags.NotValueType)) switch
                {
                    TypeFlags.ValueType => true,
                    TypeFlags.NotValueType => false,
                    _ => throw new MissingMemberException(this),
                };
            }
        }

        /// <summary>
        /// Throws, because the base type of a missing type is unknown.
        /// </summary>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override Type BaseType => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the attributes of a missing type are unknown.
        /// </summary>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override TypeAttributes Attributes => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the nested types of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override Type[] __GetDeclaredTypes() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the interfaces of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override Type[] __GetDeclaredInterfaces() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the methods of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override MethodBase[] __GetDeclaredMethods() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the method implementations of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override __MethodImplMap __GetMethodImplMap() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the fields of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override FieldInfo[] __GetDeclaredFields() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the events of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override EventInfo[] __GetDeclaredEvents() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the properties of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override PropertyInfo[] __GetDeclaredProperties() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the custom modifiers of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override CustomModifiers __GetCustomModifiers() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the generic parameters of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override Type[] GetGenericArguments() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because the generic parameters of a missing type are unknown.
        /// </summary>
        /// <returns>This method does not return.</returns>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override CustomModifiers[] __GetGenericArgumentsCustomModifiers() => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because whether a missing type is generic is unknown.
        /// </summary>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override bool IsGenericType => throw new MissingMemberException(this);

        /// <summary>
        /// Throws, because whether a missing type is a generic type definition is unknown.
        /// </summary>
        /// <exception cref="MissingMemberException">Always.</exception>
        public override bool IsGenericTypeDefinition => throw new MissingMemberException(this);

        /// <summary>
        /// Gets a missing type parameter at the specified position, creating and caching it on first use. This lets
        /// signatures that refer to type parameters of a missing generic type be read.
        /// </summary>
        /// <param name="index">The zero-based position of the type parameter.</param>
        /// <returns>The <see cref="MissingTypeParameter"/> at <paramref name="index"/>.</returns>
        internal override Type GetGenericTypeArgument(int index)
        {
            if (_typeArgs == null)
                _typeArgs = new Type[index + 1];
            else if (_typeArgs.Length <= index)
                Array.Resize(ref _typeArgs, index + 1);

            return _typeArgs[index] ?? (_typeArgs[index] = new MissingTypeParameter(this, index));
        }

        /// <summary>
        /// Binds the type parameters of this type. A missing type is never treated as a generic type definition, so it
        /// is returned unchanged.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>This instance.</returns>
        internal override Type BindTypeParameters(IGenericBinder binder) => this;

        /// <summary>
        /// Records the token of the exported type entry (or other reference) that resolved to this missing type.
        /// </summary>
        /// <param name="token">The token to report from <see cref="MetadataToken"/>, such as the type definition id hint of an exported type.</param>
        /// <returns>This instance.</returns>
        internal override Type SetMetadataTokenForMissing(int token)
        {
            _token = token;
            return this;
        }

        /// <summary>
        /// Throws, because whether a missing type is complete is unknown.
        /// </summary>
        /// <exception cref="MissingMemberException">Always.</exception>
        internal override bool IsBaked => throw new MissingMemberException(this);

    }

}
