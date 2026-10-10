/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a parameter, or the return parameter, of a method defined in a module that was read from a file. The
    /// parameter type comes from the method signature; the name, attributes and default value come from the parameter's
    /// row in the Param table, if it has one.
    /// </summary>
    sealed class ParameterInfoImpl : ParameterInfo
    {

        readonly MethodDefImpl _method;
        readonly int _position;
        readonly ParameterHandle _handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method">The method that declares the parameter.</param>
        /// <param name="position">The zero-based position of the parameter, or -1 for the return parameter.</param>
        /// <param name="handle">The parameter row, or a nil handle for a parameter without one.</param>
        internal ParameterInfoImpl(MethodDefImpl method, int position, ParameterHandle handle)
        {
            _method = method;
            _position = position;
            _handle = handle;
        }

        /// <summary>
        /// Gets the module that defines the declaring method.
        /// </summary>
        ModuleReader ModuleReader => (ModuleReader)_method.Module;

        /// <inheritdoc />
        public override string Name => _handle.IsNil ? null : ModuleReader.GetString(ModuleReader.Metadata.GetParameter(_handle).Name);

        /// <inheritdoc />
        public override Type ParameterType =>
            _position == -1 ? _method.MethodSignature.GetReturnType(_method) : _method.MethodSignature.GetParameterType(_method, _position);

        /// <inheritdoc />
        public override ParameterAttributes Attributes =>
            _handle.IsNil ? ParameterAttributes.None : (ParameterAttributes)ModuleReader.Metadata.GetParameter(_handle).Attributes;

        /// <inheritdoc />
        public override int Position => _position;

        /// <summary>
        /// Gets the default value of the parameter. This is the metadata constant if the parameter has one, otherwise a
        /// <see cref="decimal"/> or <see cref="System.DateTime"/> custom constant, otherwise
        /// <see cref="Missing.Value"/> for an optional parameter, and <see langword="null"/> when there is no default
        /// value.
        /// </summary>
        public override object RawDefaultValue
        {
            get
            {
                if ((Attributes & ParameterAttributes.HasDefault) != 0)
                    return ModuleReader.GetConstantValue(ModuleReader.Metadata.GetParameter(_handle).GetDefaultValue());

                if (TryGetCustomConstant(out var value))
                    return value;

                if ((Attributes & ParameterAttributes.Optional) != 0)
                    return Missing.Value;

                return null;
            }
        }

        /// <inheritdoc />
        public override CustomModifiers __GetCustomModifiers() =>
            _position == -1 ? _method.MethodSignature.GetReturnTypeCustomModifiers(_method) : _method.MethodSignature.GetParameterCustomModifiers(_method, _position);

        /// <inheritdoc />
        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) =>
            FieldMarshal.ReadFieldMarshal(Module, MetadataToken, out fieldMarshal);

        /// <summary>
        /// Gets the member in which the parameter is implemented. The method is resolved through its module rather than
        /// returned directly, so that a constructor is returned as its <see cref="ConstructorInfo"/> wrapper.
        /// </summary>
        public override MemberInfo Member => _method.Module.ResolveMethod(_method.MetadataToken);

        /// <summary>
        /// Gets a value that identifies this parameter in metadata. Like .NET, a parameter without a row in the Param
        /// table has token 0x08000000.
        /// </summary>
        public override int MetadataToken => _handle.IsNil ? 0x08000000 : MetadataTokens.GetToken(_handle);

        /// <inheritdoc />
        public override Module Module => _method.Module;

    }

}
