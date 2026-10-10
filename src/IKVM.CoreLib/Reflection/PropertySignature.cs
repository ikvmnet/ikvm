using System.Reflection.Metadata;
/*
  Copyright (C) 2009-2011 Jeroen Frijters

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
using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    sealed class PropertySignature : Signature
    {

        CallingConventions _callingConvention;
        readonly Type _propertyType;
        readonly Type[] _parameterTypes;
        readonly PackedCustomModifiers _customModifiers;

        internal static PropertySignature Create(CallingConventions callingConvention, Type propertyType, Type[] parameterTypes, PackedCustomModifiers customModifiers) =>
            new PropertySignature(callingConvention, propertyType, Util.Copy(parameterTypes), customModifiers);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="callingConvention"></param>
        /// <param name="propertyType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="customModifiers"></param>
        PropertySignature(CallingConventions callingConvention, Type propertyType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            _callingConvention = callingConvention;
            _propertyType = propertyType;
            _parameterTypes = parameterTypes;
            _customModifiers = customModifiers;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertySignature other
                && other._propertyType.Equals(_propertyType)
                && other._customModifiers.Equals(_customModifiers);
        }

        public override int GetHashCode() => _propertyType.GetHashCode() ^ _customModifiers.GetHashCode();

        internal int ParameterCount => _parameterTypes.Length;

        internal bool HasThis
        {
            set
            {
                if (value)
                    _callingConvention |= CallingConventions.HasThis;
                else
                    _callingConvention &= ~CallingConventions.HasThis;
            }
        }

        internal Type PropertyType => _propertyType;

        internal CustomModifiers GetCustomModifiers() => _customModifiers.GetReturnTypeCustomModifiers();

        internal PropertySignature ExpandTypeParameters(Type declaringType) =>
            new PropertySignature(_callingConvention, _propertyType.BindTypeParameters(declaringType), BindTypeParameters(declaringType, _parameterTypes), _customModifiers.Bind(declaringType));

        internal override void Write(ModuleBuilder module, BlobBuilder bb)
        {
            var flags = PROPERTY;
            if ((_callingConvention & CallingConventions.HasThis) != 0)
                flags |= HASTHIS;
            if ((_callingConvention & CallingConventions.ExplicitThis) != 0)
                flags |= EXPLICITTHIS;
            if ((_callingConvention & CallingConventions.VarArgs) != 0)
                flags |= VARARG;

            bb.WriteByte(flags);
            bb.WriteCompressedInteger(_parameterTypes == null ? 0 : _parameterTypes.Length);
            WriteCustomModifiers(module, bb, _customModifiers.GetReturnTypeCustomModifiers());
            WriteType(module, bb, _propertyType);

            if (_parameterTypes != null)
            {
                for (var i = 0; i < _parameterTypes.Length; i++)
                {
                    WriteCustomModifiers(module, bb, _customModifiers.GetParameterCustomModifiers(i));
                    WriteType(module, bb, _parameterTypes[i]);
                }
            }
        }

        internal Type GetParameter(int parameter) => _parameterTypes[parameter];

        internal CustomModifiers GetParameterCustomModifiers(int parameter) => _customModifiers.GetParameterCustomModifiers(parameter);

        internal bool MatchParameterTypes(Type[] types) => Util.ArrayEquals(types, _parameterTypes);

        internal static PropertySignature ReadSig(ModuleReader module, BlobReader br, IGenericContext context)
        {
            var flags = br.ReadByte();
            if ((flags & PROPERTY) == 0)
                throw new BadImageFormatException();

            var callingConvention = CallingConventions.Standard;
            if ((flags & HASTHIS) != 0)
                callingConvention |= CallingConventions.HasThis;
            if ((flags & EXPLICITTHIS) != 0)
                callingConvention |= CallingConventions.ExplicitThis;

            var paramCount = br.ReadCompressedInteger();
            CustomModifiers[] mods = null;
            PackedCustomModifiers.Pack(ref mods, 0, CustomModifiers.Read(module, ref br, context), paramCount + 1);
            var returnType = ReadRetType(module, ref br, context);
            var parameterTypes = new Type[paramCount];
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                PackedCustomModifiers.Pack(ref mods, i + 1, CustomModifiers.Read(module, ref br, context), paramCount + 1);
                parameterTypes[i] = ReadParam(module, ref br, context);
            }

            return new PropertySignature(callingConvention, returnType, parameterTypes, PackedCustomModifiers.Wrap(mods));
        }
    }
}
