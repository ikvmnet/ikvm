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
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines the properties for a type.
    /// </summary>
    sealed class PropertyBuilder : PropertyInfo
    {

        readonly TypeBuilder _typeBuilder;
        readonly string _name;
        PropertyAttributes _attributes;
        readonly PropertySignature _sig;
        MethodBuilder _getter;
        MethodBuilder _setter;
        readonly List<Accessor> _accessors = [];

        // the token of the property: a pseudo token until the property is baked, then the real token
        int _lazyPseudoToken;

        // whether Bake sets the HASTHIS flag of the signature from whether the accessors are static
        readonly bool _patchCallingConvention;

        /// <summary>
        /// An accessor method of the property and its MethodSemantics role.
        /// </summary>
        struct Accessor
        {

            /// <summary>
            /// The <see cref="System.Reflection.MethodSemanticsAttributes"/> value describing the role of the accessor.
            /// </summary>
            internal short Semantics;

            /// <summary>
            /// The accessor method.
            /// </summary>
            internal MethodBuilder Method;

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder">The type that declares the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="attributes">The attributes of the property.</param>
        /// <param name="sig">The signature of the property.</param>
        /// <param name="patchCallingConvention">
        /// <see langword="true"/> to set the <c>HASTHIS</c> flag of the signature from the accessors when the property is
        /// baked, because the caller did not specify a calling convention.
        /// </param>
        internal PropertyBuilder(TypeBuilder typeBuilder, string name, PropertyAttributes attributes, PropertySignature sig, bool patchCallingConvention)
        {
            _typeBuilder = typeBuilder;
            _name = name;
            _attributes = attributes;
            _sig = sig;
            _patchCallingConvention = patchCallingConvention;
        }

        /// <inheritdoc />
        internal override PropertySignature PropertySignature => _sig;

        /// <summary>
        /// Sets the method that gets the property value.
        /// </summary>
        /// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method that gets the property value.</param>
        public void SetGetMethod(MethodBuilder mdBuilder)
        {
            _getter = mdBuilder;
            Accessor acc;
            acc.Semantics = (short)System.Reflection.MethodSemanticsAttributes.Getter;
            acc.Method = mdBuilder;
            _accessors.Add(acc);
        }

        /// <summary>
        /// Sets the method that sets the property value.
        /// </summary>
        /// <param name="mdBuilder">A <see cref="MethodBuilder"/> object that represents the method that sets the property value.</param>
        public void SetSetMethod(MethodBuilder mdBuilder)
        {
            _setter = mdBuilder;
            Accessor acc;
            acc.Semantics = (short)System.Reflection.MethodSemanticsAttributes.Setter;
            acc.Method = mdBuilder;
            _accessors.Add(acc);
        }

        /// <summary>
        /// Set a custom attribute using a custom attribute builder. <c>SpecialNameAttribute</c> is not emitted as an
        /// attribute but sets <see cref="PropertyAttributes.SpecialName"/>.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class to define the custom attribute.</param>
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (customBuilder.KnownCA == KnownCA.SpecialNameAttribute)
            {
                _attributes |= PropertyAttributes.SpecialName;
            }
            else
            {
                if (_lazyPseudoToken == 0)
                    _lazyPseudoToken = _typeBuilder.ModuleBuilder.AllocPseudoToken();

                _typeBuilder.ModuleBuilder.SetCustomAttribute(_lazyPseudoToken, customBuilder);
            }
        }

        /// <summary>
        /// Returns the constant recorded for the property in the module being built.
        /// </summary>
        /// <returns>The constant value.</returns>
        /// <exception cref="InvalidOperationException">The property has no token yet, so no constant can have been recorded.</exception>
        public override object GetRawConstantValue()
        {
            if (_lazyPseudoToken != 0)
                return _typeBuilder.ModuleBuilder.GetConstant(_lazyPseudoToken);

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _attributes;

        /// <summary>
        /// Gets a value indicating whether a <c>get</c> accessor has been set.
        /// </summary>
        public override bool CanRead => _getter != null;

        /// <summary>
        /// Gets a value indicating whether a <c>set</c> accessor has been set.
        /// </summary>
        public override bool CanWrite => _setter != null;

        /// <inheritdoc />
        public override MethodInfo GetGetMethod(bool nonPublic) => nonPublic || (_getter != null && _getter.IsPublic) ? _getter : null;

        /// <inheritdoc />
        public override MethodInfo GetSetMethod(bool nonPublic) => nonPublic || (_setter != null && _setter.IsPublic) ? _setter : null;

        /// <inheritdoc />
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var acc in _accessors)
                AddAccessor(list, nonPublic, acc.Method);

            return list.ToArray();
        }

        /// <summary>
        /// Adds an accessor to the list if it is not <see langword="null"/> and is public or non-public accessors are
        /// requested.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="nonPublic"><see langword="true"/> to include non-public accessors.</param>
        /// <param name="method">The accessor.</param>
        static void AddAccessor(List<MethodInfo> list, bool nonPublic, MethodInfo method)
        {
            if (method != null && (nonPublic || method.IsPublic))
                list.Add(method);
        }

        /// <inheritdoc />
        public override Type DeclaringType => _typeBuilder;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override Module Module => _typeBuilder.Module;

        /// <summary>
        /// Writes the property and its MethodSemantics rows to the module, and fixes up the pseudo token handed out
        /// before baking, if any, to the real token.
        /// </summary>
        internal void Bake()
        {
            if (_patchCallingConvention)
                _sig.HasThis = !IsStatic;

            var token = _typeBuilder.ModuleBuilder.AddProperty(_attributes, _name, _sig);

            if (_lazyPseudoToken == 0)
                _lazyPseudoToken = token;
            else
                _typeBuilder.ModuleBuilder.RegisterTokenFixup(_lazyPseudoToken, token);

            foreach (var acc in _accessors)
                _typeBuilder.ModuleBuilder.AddMethodSemantics(acc.Semantics, acc.Method.MetadataToken, token);
        }

        /// <inheritdoc />
        internal override bool IsPublic
        {
            get
            {
                foreach (var acc in _accessors)
                    if (acc.Method.IsPublic)
                        return true;

                return false;
            }
        }

        /// <inheritdoc />
        internal override bool IsNonPrivate
        {
            get
            {
                foreach (var acc in _accessors)
                    if ((acc.Method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Gets whether the property is static, which is the case when any of its accessors is static.
        /// </summary>
        internal override bool IsStatic
        {
            get
            {
                foreach (var acc in _accessors)
                    if (acc.Method.IsStatic)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Gets whether the declaring type has been baked.
        /// </summary>
        internal override bool IsBaked => _typeBuilder.IsBaked;

        /// <summary>
        /// Gets the current token of the property: the real token once the module has been saved, otherwise the token
        /// handed out so far, which may be a pseudo token or zero.
        /// </summary>
        /// <returns>The current token.</returns>
        internal override int GetCurrentToken()
        {
            if (_typeBuilder.ModuleBuilder.IsSaved && ModuleBuilder.IsPseudoToken(_lazyPseudoToken))
                return _typeBuilder.ModuleBuilder.ResolvePseudoToken(_lazyPseudoToken);
            else
                return _lazyPseudoToken;
        }

        /// <summary>
        /// Gets a value that identifies the property in metadata. Before the property is baked, this allocates a pseudo
        /// token that is fixed up when the module is saved.
        /// </summary>
        public override int MetadataToken
        {
            get
            {
                if (_lazyPseudoToken == 0)
                    _lazyPseudoToken = _typeBuilder.ModuleBuilder.AllocPseudoToken();

                return GetCurrentToken();
            }
        }

    }

}
