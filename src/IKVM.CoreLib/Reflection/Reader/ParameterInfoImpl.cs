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

    sealed class ParameterInfoImpl : ParameterInfo
    {

        readonly MethodDefImpl _method;
        readonly int _position;
        readonly ParameterHandle _handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="position"></param>
        /// <param name="handle">The parameter row, or a nil handle for a parameter without one.</param>
        internal ParameterInfoImpl(MethodDefImpl method, int position, ParameterHandle handle)
        {
            this._method = method;
            this._position = position;
            this._handle = handle;
        }

        ModuleReader ModuleReader => (ModuleReader)_method.Module;

        public override string Name => _handle.IsNil ? null : ModuleReader.GetString(ModuleReader.Metadata.GetParameter(_handle).Name);

        public override Type ParameterType => _position == -1 ? _method.MethodSignature.GetReturnType(_method) : _method.MethodSignature.GetParameterType(_method, _position);

        public override ParameterAttributes Attributes => _handle.IsNil ? ParameterAttributes.None : (ParameterAttributes)ModuleReader.Metadata.GetParameter(_handle).Attributes;

        public override int Position => _position;

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

        public override CustomModifiers __GetCustomModifiers()
        {
            return _position == -1 ? _method.MethodSignature.GetReturnTypeCustomModifiers(_method) : _method.MethodSignature.GetParameterCustomModifiers(_method, _position);
        }

        public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal) => FieldMarshal.ReadFieldMarshal(Module, MetadataToken, out fieldMarshal);

        // return the right ConstructorInfo wrapper
        public override MemberInfo Member => _method.Module.ResolveMethod(_method.MetadataToken);

        // like .NET, a parameter without a row in the Param table has token 0x08000000
        public override int MetadataToken => _handle.IsNil ? 0x08000000 : MetadataTokens.GetToken(_handle);

        public override Module Module => _method.Module;

    }

}
