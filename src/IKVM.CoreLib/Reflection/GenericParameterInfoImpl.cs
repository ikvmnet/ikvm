/*
  Copyright (C) 2009, 2010 Jeroen Frijters

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

    /// <summary>
    /// A parameter of a <see cref="GenericMethodInstance"/>: wraps the parameter of the underlying method and binds the
    /// type parameters in its type and custom modifiers to the instance's type arguments.
    /// </summary>
    sealed class GenericParameterInfoImpl : ParameterInfo
    {

        readonly GenericMethodInstance _method;
        readonly ParameterInfo _parameterInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method">The generic method instance the parameter belongs to.</param>
        /// <param name="parameterInfo">The corresponding parameter of the underlying method.</param>
        internal GenericParameterInfoImpl(GenericMethodInstance method, ParameterInfo parameterInfo)
        {
            _method = method;
            _parameterInfo = parameterInfo;
        }

        /// <inheritdoc />
        public override string Name => _parameterInfo.Name;

        /// <summary>
        /// Gets the type of the parameter, with the type parameters bound to the type arguments of the method instance.
        /// </summary>
        public override Type ParameterType => _parameterInfo.ParameterType.BindTypeParameters(_method);

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _parameterInfo.Attributes;

        /// <inheritdoc />
        public override int Position => _parameterInfo.Position;

        /// <inheritdoc />
        public override object RawDefaultValue => _parameterInfo.RawDefaultValue;

        /// <summary>
        /// Gets the custom modifiers of the parameter, with the type parameters bound to the type arguments of the
        /// method instance.
        /// </summary>
        /// <returns>The bound custom modifiers.</returns>
        public override CustomModifiers __GetCustomModifiers() => _parameterInfo.__GetCustomModifiers().Bind(_method);

        /// <inheritdoc />
        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => _parameterInfo.__TryGetFieldMarshal(out fieldMarshal);

        /// <summary>
        /// Gets the generic method instance the parameter belongs to.
        /// </summary>
        public override MemberInfo Member => _method;

        /// <inheritdoc />
        public override int MetadataToken => _parameterInfo.MetadataToken;

        /// <inheritdoc />
        public override Module Module => _method.Module;

    }

}
