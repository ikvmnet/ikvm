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

    /// <summary>
    /// Creates or associates parameter information.
    /// </summary>
    sealed class ParameterBuilder
    {

        readonly MethodBuilder _method;
        short _flags;
        readonly short _sequence;
        readonly StringHandle _nameIndex;
        readonly string _name;

        /// <summary>
        /// The pseudo token allocated for this parameter on first use, or zero if none has been allocated yet. It is
        /// replaced by the real <c>Param</c> token once metadata is written.
        /// </summary>
        int _lazyPseudoToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method">The method that declares the parameter.</param>
        /// <param name="sequence">The position of the parameter in the signature; 0 for the return value.</param>
        /// <param name="attribs">The attributes of the parameter.</param>
        /// <param name="name">The name of the parameter, or <see langword="null"/> if it is unnamed.</param>
        internal ParameterBuilder(MethodBuilder method, int sequence, ParameterAttributes attribs, string name)
        {
            _method = method;
            _flags = (short)attribs;
            _sequence = (short)sequence;
            _nameIndex = name == null ? default : method.ModuleBuilder.GetOrAddString(name);
            _name = name;
        }

        /// <summary>
        /// Gets the pseudo token that stands for this parameter until its real metadata token is known, allocating it on
        /// first access. Custom attributes and marshaling information applied to the parameter are recorded against it.
        /// </summary>
        internal int PseudoToken
        {
            get
            {
                if (_lazyPseudoToken == 0)
                    _lazyPseudoToken = _method.ModuleBuilder.AllocPseudoToken();

                return _lazyPseudoToken;
            }
        }

        /// <summary>
        /// Retrieves the name of this parameter.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Retrieves the signature position for this parameter. Parameters are numbered from 1; 0 denotes the return
        /// value.
        /// </summary>
        public int Position => _sequence;

        /// <summary>
        /// Retrieves the attributes for this parameter.
        /// </summary>
        public int Attributes => _flags;

        /// <summary>
        /// Set a custom attribute using a custom attribute builder. The pseudo-custom attributes <c>InAttribute</c>,
        /// <c>OutAttribute</c>, <c>OptionalAttribute</c> and <c>MarshalAsAttribute</c> are translated into parameter
        /// flags and marshaling information rather than emitted as custom attributes.
        /// </summary>
        /// <param name="customAttributeBuilder">An instance of a helper class to define the custom attribute.</param>
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

        /// <summary>
        /// Adds the <c>Param</c> row for this parameter to the module's metadata.
        /// </summary>
        internal void WriteMetadata()
        {
            _method.ModuleBuilder.Metadata.AddParameter(
                (System.Reflection.ParameterAttributes)_flags,
                _nameIndex,
                _sequence);
        }

        /// <summary>
        /// Registers the real metadata token of this parameter as the replacement for its pseudo token, if one was
        /// allocated.
        /// </summary>
        /// <param name="parameterToken">The <c>Param</c> metadata token assigned to this parameter.</param>
        internal void FixupToken(int parameterToken)
        {
            if (_lazyPseudoToken != 0)
                _method.ModuleBuilder.RegisterTokenFixup(_lazyPseudoToken, parameterToken);
        }

    }

}
