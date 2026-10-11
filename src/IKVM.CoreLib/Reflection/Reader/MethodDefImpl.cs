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

    sealed class MethodDefImpl : MethodInfo
    {

        readonly ModuleReader module;
        readonly MethodDefinitionHandle handle;
        readonly MethodDefinition definition;
        readonly TypeDefImpl declaringType;
        MethodSignature lazyMethodSignature;
        ParameterInfo returnParameter;
        ParameterInfo[] parameters;
        Type[] typeArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        internal MethodDefImpl(ModuleReader module, TypeDefImpl declaringType, MethodDefinitionHandle handle)
        {
            this.module = module;
            this.handle = handle;
            this.definition = module.Metadata.GetMethodDefinition(handle);
            this.declaringType = declaringType;
        }

        public override CallingConventions CallingConvention => MethodSignature.CallingConvention;

        public override MethodAttributes Attributes => (MethodAttributes)definition.Attributes;

        public override MethodImplAttributes GetMethodImplementationFlags() => (MethodImplAttributes)definition.ImplAttributes;

        public override ParameterInfo[] GetParameters()
        {
            PopulateParameters();
            return (ParameterInfo[])parameters.Clone();
        }

        void PopulateParameters()
        {
            if (parameters == null)
            {
                var array = new ParameterInfo[MethodSignature.GetParameterCount()];

                // sequence 0 is the return parameter; parameters without a row get one without metadata
                foreach (var h in definition.GetParameters())
                {
                    var position = module.Metadata.GetParameter(h).SequenceNumber - 1;
                    if (position == -1)
                        returnParameter = new ParameterInfoImpl(this, position, h);
                    else if (position < array.Length)
                        array[position] = new ParameterInfoImpl(this, position, h);
                }

                for (int i = 0; i < array.Length; i++)
                    array[i] ??= new ParameterInfoImpl(this, i, default);

                returnParameter ??= new ParameterInfoImpl(this, -1, default);
                parameters = array;
            }
        }

        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = new Type[MethodSignature.GetParameterCount()];
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = MethodSignature.GetParameterType(i);

            return parameterTypes;
        }

        internal override int ParameterCount => MethodSignature.GetParameterCount();

        public override ParameterInfo ReturnParameter
        {
            get
            {
                PopulateParameters();
                return returnParameter;
            }
        }

        public override Type ReturnType => MethodSignature.GetReturnType(this);

        public override Type DeclaringType => declaringType.IsModulePseudoType ? null : declaringType;

        public override string Name => module.GetString(definition.Name);

        public override int MetadataToken => MetadataTokens.GetToken(handle);

        public override bool IsGenericMethodDefinition => definition.GetGenericParameters().Count > 0;

        public override bool IsGenericMethod => IsGenericMethodDefinition;

        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(typeArgs);
        }

        void PopulateGenericArguments()
        {
            if (typeArgs == null)
            {
                var handles = definition.GetGenericParameters();
                var args = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
                for (int i = 0; i < args.Length; i++)
                    args[i] = new GenericTypeParameter(module, handles[i], Signature.ELEMENT_TYPE_MVAR);

                typeArgs = args;
            }
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            PopulateGenericArguments();
            return typeArgs[index];
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return IsGenericMethodDefinition ? (MethodInfo)this : throw new InvalidOperationException();
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => new GenericMethodInstance(declaringType, this, typeArguments);

        public override Module Module => module;

        internal override MethodSignature MethodSignature => lazyMethodSignature ??= MethodSignature.ReadSig(module, module.GetBlobReader(definition.Signature), this);

        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(declaringType, Name, MethodSignature);

        internal override int GetCurrentToken() => MetadataToken;

        internal override bool IsBaked => true;

    }

}
