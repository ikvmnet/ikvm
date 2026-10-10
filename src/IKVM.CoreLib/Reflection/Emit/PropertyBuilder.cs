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

    internal sealed class PropertyBuilder : PropertyInfo
    {

        readonly TypeBuilder _typeBuilder;
        readonly string _name;
        PropertyAttributes _attributes;
        PropertySignature _sig;
        MethodBuilder _getter;
        MethodBuilder _setter;
        readonly List<Accessor> _accessors = new List<Accessor>();
        int _lazyPseudoToken;
        bool _patchCallingConvention;

        struct Accessor
        {

            internal short Semantics;
            internal MethodBuilder Method;

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="sig"></param>
        /// <param name="patchCallingConvention"></param>
        internal PropertyBuilder(TypeBuilder typeBuilder, string name, PropertyAttributes attributes, PropertySignature sig, bool patchCallingConvention)
        {
            this._typeBuilder = typeBuilder;
            this._name = name;
            this._attributes = attributes;
            this._sig = sig;
            this._patchCallingConvention = patchCallingConvention;
        }

        internal override PropertySignature PropertySignature
        {
            get { return _sig; }
        }

        public void SetGetMethod(MethodBuilder mdBuilder)
        {
            _getter = mdBuilder;
            Accessor acc;
            acc.Semantics = (short)System.Reflection.MethodSemanticsAttributes.Getter;
            acc.Method = mdBuilder;
            _accessors.Add(acc);
        }

        public void SetSetMethod(MethodBuilder mdBuilder)
        {
            _setter = mdBuilder;
            Accessor acc;
            acc.Semantics = (short)System.Reflection.MethodSemanticsAttributes.Setter;
            acc.Method = mdBuilder;
            _accessors.Add(acc);
        }

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

        public override object GetRawConstantValue()
        {
            if (_lazyPseudoToken != 0)
                return _typeBuilder.ModuleBuilder.GetConstant(_lazyPseudoToken);

            throw new InvalidOperationException();
        }

        public override PropertyAttributes Attributes
        {
            get { return _attributes; }
        }

        public override bool CanRead
        {
            get { return _getter != null; }
        }

        public override bool CanWrite
        {
            get { return _setter != null; }
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return nonPublic || (_getter != null && _getter.IsPublic) ? _getter : null;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return nonPublic || (_setter != null && _setter.IsPublic) ? _setter : null;
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var acc in _accessors)
                AddAccessor(list, nonPublic, acc.Method);

            return list.ToArray();
        }

        static void AddAccessor(List<MethodInfo> list, bool nonPublic, MethodInfo method)
        {
            if (method != null && (nonPublic || method.IsPublic))
                list.Add(method);
        }

        public override Type DeclaringType
        {
            get { return _typeBuilder; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override Module Module
        {
            get { return _typeBuilder.Module; }
        }

        internal void Bake()
        {
            if (_patchCallingConvention)
                _sig.HasThis = !this.IsStatic;

            var token = _typeBuilder.ModuleBuilder.AddProperty(_attributes, _name, _sig);

            if (_lazyPseudoToken == 0)
                _lazyPseudoToken = token;
            else
                _typeBuilder.ModuleBuilder.RegisterTokenFixup(_lazyPseudoToken, token);

            foreach (var acc in _accessors)
                _typeBuilder.ModuleBuilder.AddMethodSemantics(acc.Semantics, acc.Method.MetadataToken, token);
        }

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

        internal override bool IsBaked
        {
            get { return _typeBuilder.IsBaked; }
        }

        internal override int GetCurrentToken()
        {
            if (_typeBuilder.ModuleBuilder.IsSaved && ModuleBuilder.IsPseudoToken(_lazyPseudoToken))
                return _typeBuilder.ModuleBuilder.ResolvePseudoToken(_lazyPseudoToken);
            else
                return _lazyPseudoToken;
        }

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
