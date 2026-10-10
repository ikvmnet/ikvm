/*
  Copyright (C) 2008 Jeroen Frijters

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
using System.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    sealed class ParameterBuilder
    {

        readonly MethodBuilder _method;
        short _flags;
        readonly short _sequence;
        readonly StringHandle _nameIndex;
        readonly string _name;
        int _lazyPseudoToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="sequence"></param>
        /// <param name="attribs"></param>
        /// <param name="name"></param>
        internal ParameterBuilder(MethodBuilder method, int sequence, ParameterAttributes attribs, string name)
        {
            _method = method;
            _flags = (short)attribs;
            _sequence = (short)sequence;
            _nameIndex = name == null ? default : method.ModuleBuilder.GetOrAddString(name);
            _name = name;
        }

        internal int PseudoToken
        {
            get
            {
                if (_lazyPseudoToken == 0)
                    _lazyPseudoToken = _method.ModuleBuilder.AllocPseudoToken();

                return _lazyPseudoToken;
            }
        }

        public string Name => _name;

        /// <summary>
        /// One-based index of the parameter.
        /// </summary>
        public int Position => _sequence;

        public int Attributes => _flags;

        public void SetCustomAttribute(CustomAttributeBuilder customAttributeBuilder)
        {
            switch (customAttributeBuilder.KnownCA)
            {
                case KnownCA.InAttribute:
                    _flags |= (short)ParameterAttributes.In;
                    break;
                case KnownCA.OutAttribute:
                    _flags |= (short)ParameterAttributes.Out;
                    break;
                case KnownCA.OptionalAttribute:
                    _flags |= (short)ParameterAttributes.Optional;
                    break;
                case KnownCA.MarshalAsAttribute:
                    FieldMarshal.SetMarshalAsAttribute(_method.ModuleBuilder, PseudoToken, customAttributeBuilder);
                    _flags |= (short)ParameterAttributes.HasFieldMarshal;
                    break;
                default:
                    _method.ModuleBuilder.SetCustomAttribute(PseudoToken, customAttributeBuilder);
                    break;
            }
        }

        internal void WriteMetadata()
        {
            _method.ModuleBuilder.Metadata.AddParameter(
                (System.Reflection.ParameterAttributes)_flags,
                _nameIndex,
                _sequence);
        }

        internal void FixupToken(int parameterToken)
        {
            if (_lazyPseudoToken != 0)
                _method.ModuleBuilder.RegisterTokenFixup(_lazyPseudoToken, parameterToken);
        }

    }

}
