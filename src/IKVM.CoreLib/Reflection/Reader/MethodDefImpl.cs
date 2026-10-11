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
using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a method defined by a row of the MethodDef table of a module read from metadata.
    /// </summary>
    sealed class MethodDefImpl : MethodInfo
    {

        readonly ModuleReader _module;
        readonly MethodDefinitionHandle _handle;
        readonly MethodDefinition _definition;
        readonly TypeDefImpl _declaringType;
        MethodSignature _lazyMethodSignature;
        ParameterInfo _returnParameter;
        ParameterInfo[] _parameters;
        Type[] _typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module that contains the method definition.</param>
        /// <param name="declaringType">The type that defines the method; for a global method, the module's &lt;Module&gt; pseudo type.</param>
        /// <param name="handle">The handle of the method definition in the module's metadata.</param>
        internal MethodDefImpl(ModuleReader module, TypeDefImpl declaringType, MethodDefinitionHandle handle)
        {
            _module = module;
            _handle = handle;
            _definition = module.Metadata.GetMethodDefinition(handle);
            _declaringType = declaringType;
        }

        /// <inheritdoc />
        public override CallingConventions CallingConvention => MethodSignature.CallingConvention;

        /// <inheritdoc />
        public override MethodAttributes Attributes => (MethodAttributes)_definition.Attributes;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags() => (MethodImplAttributes)_definition.ImplAttributes;

        /// <inheritdoc />
        public override ParameterInfo[] GetParameters()
        {
            PopulateParameters();
            return (ParameterInfo[])_parameters.Clone();
        }

        /// <summary>
        /// Creates the parameter and return parameter objects on first use, from the Param rows of the method and its
        /// signature.
        /// </summary>
        void PopulateParameters()
        {
            if (_parameters == null)
            {
                var array = new ParameterInfo[MethodSignature.GetParameterCount()];

                // sequence 0 is the return parameter; parameters without a row get one without metadata
                foreach (var h in _definition.GetParameters())
                {
                    var position = _module.Metadata.GetParameter(h).SequenceNumber - 1;
                    if (position == -1)
                        _returnParameter = new ParameterInfoImpl(this, position, h);
                    else if (position < array.Length)
                        array[position] = new ParameterInfoImpl(this, position, h);
                }

                for (var i = 0; i < array.Length; i++)
                    array[i] ??= new ParameterInfoImpl(this, i, default);

                _returnParameter ??= new ParameterInfoImpl(this, -1, default);
                _parameters = array;
            }
        }

        /// <inheritdoc />
        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = new Type[MethodSignature.GetParameterCount()];
            for (var i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = MethodSignature.GetParameterType(i);

            return parameterTypes;
        }

        /// <inheritdoc />
        internal override int ParameterCount => MethodSignature.GetParameterCount();

        /// <inheritdoc />
        public override ParameterInfo ReturnParameter
        {
            get
            {
                PopulateParameters();
                return _returnParameter;
            }
        }

        /// <inheritdoc />
        public override Type ReturnType => MethodSignature.GetReturnType(this);

        /// <summary>
        /// Gets the class that declares this member, or <see langword="null"/> for a global method, which is defined on
        /// the module's &lt;Module&gt; pseudo type.
        /// </summary>
        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        /// <inheritdoc />
        public override string Name => _module.GetString(_definition.Name);

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => _definition.GetGenericParameters().Count > 0;

        /// <summary>
        /// Gets a value indicating whether the method is generic. A method read from a MethodDef row is generic only
        /// when it is a generic method definition.
        /// </summary>
        public override bool IsGenericMethod => IsGenericMethodDefinition;

        /// <inheritdoc />
        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(_typeArgs);
        }

        /// <summary>
        /// Creates the generic method parameter types on first use, from the GenericParam rows of the method.
        /// </summary>
        void PopulateGenericArguments()
        {
            if (_typeArgs == null)
            {
                var handles = _definition.GetGenericParameters();
                var args = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
                for (var i = 0; i < args.Length; i++)
                    args[i] = new GenericTypeParameter(_module, handles[i], Signature.ELEMENT_TYPE_MVAR);

                _typeArgs = args;
            }
        }

        /// <inheritdoc />
        internal override Type GetGenericMethodArgument(int index)
        {
            PopulateGenericArguments();
            return _typeArgs[index];
        }

        /// <summary>
        /// Returns a <see cref="MethodInfo"/> object that represents a generic method definition from which the current
        /// method can be constructed.
        /// </summary>
        /// <returns>The current method, which is a generic method definition.</returns>
        /// <exception cref="InvalidOperationException">The current method is not a generic method.</exception>
        public override MethodInfo GetGenericMethodDefinition() =>
            IsGenericMethodDefinition ? (MethodInfo)this : throw new InvalidOperationException();

        /// <inheritdoc />
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => new GenericMethodInstance(_declaringType, this, typeArguments);

        /// <inheritdoc />
        public override Module Module => _module;

        /// <summary>
        /// Gets the signature of the method, decoded from the method's signature blob on first use.
        /// </summary>
        internal override MethodSignature MethodSignature =>
            _lazyMethodSignature ??= MethodSignature.ReadSig(_module, _module.GetBlobReader(_definition.Signature), this);

        /// <summary>
        /// Imports the method into <paramref name="module"/> as a MemberRef on its declaring type.
        /// </summary>
        /// <param name="module">The module being built that references the method.</param>
        /// <returns>The token of the method reference in <paramref name="module"/>.</returns>
        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(_declaringType, Name, MethodSignature);

        /// <inheritdoc />
        internal override int GetCurrentToken() => MetadataToken;

        /// <summary>
        /// Gets a value indicating whether the method is complete; a method read from metadata always is.
        /// </summary>
        internal override bool IsBaked => true;

    }

}
