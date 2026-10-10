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

    /// <summary>
    /// Represents the signature of a property: its calling convention, property type, index parameter types and their custom modifiers.
    /// </summary>
    sealed class PropertySignature : Signature
    {

        CallingConventions _callingConvention;
        readonly Type _propertyType;
        readonly Type[] _parameterTypes;
        readonly PackedCustomModifiers _customModifiers;

        /// <summary>
        /// Creates a property signature from the specified components.
        /// </summary>
        /// <param name="callingConvention">The calling convention of the property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="parameterTypes">The types of the index parameters, or <see langword="null"/>; the array is copied.</param>
        /// <param name="customModifiers">The custom modifiers of the property type and parameters.</param>
        /// <returns>The property signature.</returns>
        internal static PropertySignature Create(CallingConventions callingConvention, Type propertyType, Type[] parameterTypes, PackedCustomModifiers customModifiers) =>
            new PropertySignature(callingConvention, propertyType, Util.Copy(parameterTypes), customModifiers);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="callingConvention">The calling convention of the property.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="parameterTypes">The types of the index parameters.</param>
        /// <param name="customModifiers">The custom modifiers of the property type and parameters.</param>
        PropertySignature(CallingConventions callingConvention, Type propertyType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            _callingConvention = callingConvention;
            _propertyType = propertyType;
            _parameterTypes = parameterTypes;
            _customModifiers = customModifiers;
        }

        /// <summary>
        /// Determines whether the specified object is a property signature with the same property type and custom modifiers.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an equal property signature; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is PropertySignature other
                && other._propertyType.Equals(_propertyType)
                && Util.ArrayEquals(other._parameterTypes, _parameterTypes)
                && other._customModifiers.Equals(_customModifiers);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _propertyType.GetHashCode() ^ Util.GetHashCode(_parameterTypes) ^ _customModifiers.GetHashCode();

        /// <summary>
        /// Gets the number of index parameters.
        /// </summary>
        internal int ParameterCount => _parameterTypes.Length;

        /// <summary>
        /// Sets whether the property is an instance property, by setting or clearing <see cref="CallingConventions.HasThis"/>.
        /// </summary>
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

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        internal Type PropertyType => _propertyType;

        /// <summary>
        /// Gets the custom modifiers of the property type.
        /// </summary>
        /// <returns>The custom modifiers of the property type.</returns>
        internal CustomModifiers GetCustomModifiers() => _customModifiers.GetReturnTypeCustomModifiers();

        /// <summary>
        /// Creates a copy of this signature with the generic type parameters replaced by the type arguments of <paramref name="declaringType"/>.
        /// </summary>
        /// <param name="declaringType">The constructed generic type that supplies the type arguments.</param>
        /// <returns>The bound property signature.</returns>
        internal PropertySignature ExpandTypeParameters(Type declaringType) =>
            new PropertySignature(_callingConvention, _propertyType.BindTypeParameters(declaringType), BindTypeParameters(declaringType, _parameterTypes), _customModifiers.Bind(declaringType));

        /// <inheritdoc />
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

        /// <summary>
        /// Gets the type of the specified index parameter.
        /// </summary>
        /// <param name="parameter">The zero-based position of the parameter.</param>
        /// <returns>The type of the parameter.</returns>
        internal Type GetParameter(int parameter) => _parameterTypes[parameter];

        /// <summary>
        /// Gets the custom modifiers of the specified index parameter.
        /// </summary>
        /// <param name="parameter">The zero-based position of the parameter.</param>
        /// <returns>The custom modifiers of the parameter.</returns>
        internal CustomModifiers GetParameterCustomModifiers(int parameter) => _customModifiers.GetParameterCustomModifiers(parameter);

        /// <summary>
        /// Determines whether the index parameter types of this signature equal <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The parameter types to compare with.</param>
        /// <returns><see langword="true"/> if the parameter types are equal; otherwise, <see langword="false"/>.</returns>
        internal bool MatchParameterTypes(Type[] types) => Util.ArrayEquals(types, _parameterTypes);

        /// <summary>
        /// Reads a property signature from a metadata blob.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve generic parameters.</param>
        /// <returns>The property signature.</returns>
        /// <exception cref="BadImageFormatException">The blob is not a property signature.</exception>
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
