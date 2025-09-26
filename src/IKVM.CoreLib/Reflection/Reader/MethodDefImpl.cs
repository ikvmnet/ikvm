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
using System.Collections.Generic;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class MethodDefImpl : MethodInfo
    {

        readonly ModuleReader module;
        readonly int index;
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
        /// <param name="index"></param>
        internal MethodDefImpl(ModuleReader module, TypeDefImpl declaringType, int index)
        {
            this.module = module;
            this.index = index;
            this.declaringType = declaringType;
        }

        public override MethodBody GetMethodBody()
        {
            return GetMethodBody(this);
        }

        internal MethodBody GetMethodBody(IGenericContext context)
        {
            if ((GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
                return null; // method is not IL

            var rva = module.MethodDefTable.records[index].RVA;
            return rva == 0 ? null : new MethodBody(module, rva, context);
        }

        public override int __MethodRVA
        {
            get { return module.MethodDefTable.records[index].RVA; }
        }

        public override CallingConventions CallingConvention
        {
            get { return MethodSignature.CallingConvention; }
        }

        public override MethodAttributes Attributes
        {
            get { return (MethodAttributes)module.MethodDefTable.records[index].Flags; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return (MethodImplAttributes)module.MethodDefTable.records[index].ImplFlags;
        }

        public override ParameterInfo[] GetParameters()
        {
            PopulateParameters();
            return (ParameterInfo[])parameters.Clone();
        }

        void PopulateParameters()
        {
            if (parameters == null)
            {
                var methodSignature = MethodSignature;
                parameters = new ParameterInfo[methodSignature.GetParameterCount()];

                var parameter = module.MethodDefTable.records[index].ParamList - 1;
                var end = module.MethodDefTable.records.Length > index + 1 ? module.MethodDefTable.records[index + 1].ParamList - 1 : module.ParamTable.records.Length;
                for (; parameter < end; parameter++)
                {
                    var seq = module.ParamTable.records[parameter].Sequence - 1;
                    if (seq == -1)
                        returnParameter = new ParameterInfoImpl(this, seq, parameter);
                    else
                        parameters[seq] = new ParameterInfoImpl(this, seq, parameter);
                }

                for (int i = 0; i < parameters.Length; i++)
                    if (parameters[i] == null)
                        parameters[i] = new ParameterInfoImpl(this, i, -1);

                if (returnParameter == null)
                    returnParameter = new ParameterInfoImpl(this, -1, -1);
            }
        }

        internal override Type[] GetParameterTypes()
        {
            var parameterTypes = new Type[MethodSignature.GetParameterCount()];
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = MethodSignature.GetParameterType(i);

            return parameterTypes;
        }

        internal override int ParameterCount
        {
            get { return MethodSignature.GetParameterCount(); }
        }

        public override ParameterInfo ReturnParameter
        {
            get
            {
                PopulateParameters();
                return returnParameter;
            }
        }

        public override Type ReturnType
        {
            get
            {
                return MethodSignature.GetReturnType(this);
            }
        }

        public override Type DeclaringType
        {
            get { return declaringType.IsModulePseudoType ? null : declaringType; }
        }

        public override string Name
        {
            get { return module.GetString(module.MethodDefTable.records[index].Name); }
        }

        public override int MetadataToken
        {
            get { return (MethodDefTable.Index << 24) + index + 1; }
        }

        public override bool IsGenericMethodDefinition
        {
            get
            {
                PopulateGenericArguments();
                return typeArgs.Length > 0;
            }
        }

        public override bool IsGenericMethod
        {
            get { return IsGenericMethodDefinition; }
        }

        public override Type[] GetGenericArguments()
        {
            PopulateGenericArguments();
            return Util.Copy(typeArgs);
        }

        void PopulateGenericArguments()
        {
            if (typeArgs == null)
            {
                var token = MetadataToken;
                var first = module.GenericParamTable.FindFirstByOwner(token);
                if (first == -1)
                {
                    typeArgs = Type.EmptyTypes;
                }
                else
                {
                    var list = new List<Type>();
                    var len = module.GenericParamTable.records.Length;
                    for (int i = first; i < len && module.GenericParamTable.records[i].Owner == token; i++)
                        list.Add(new GenericTypeParameter(module, i, Signature.ELEMENT_TYPE_MVAR));

                    typeArgs = list.ToArray();
                }
            }
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            PopulateGenericArguments();
            return typeArgs[index];
        }

        internal override int GetGenericMethodArgumentCount()
        {
            PopulateGenericArguments();
            return typeArgs.Length;
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return IsGenericMethodDefinition ? (MethodInfo)this : throw new InvalidOperationException();
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return new GenericMethodInstance(declaringType, this, typeArguments);
        }

        public override Module Module
        {
            get { return module; }
        }

        internal override MethodSignature MethodSignature
        {
            get { return lazyMethodSignature ??= MethodSignature.ReadSig(module, module.GetBlobReader(module.MethodDefTable.records[index].Signature), this); }
        }

        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            return module.ImportMethodOrField(declaringType, this.Name, this.MethodSignature);
        }

        public override MethodInfo[] __GetMethodImpls()
        {
            Type[] typeArgs = null;
            List<MethodInfo> list = null;

            foreach (var i in module.MethodImplTable.Filter(declaringType.MetadataToken))
            {
                if (module.MethodImplTable.records[i].MethodBody == this.MetadataToken)
                {
                    typeArgs ??= declaringType.GetGenericArguments();
                    list ??= new List<MethodInfo>();
                    list.Add((MethodInfo)module.ResolveMethod(module.MethodImplTable.records[i].MethodDeclaration, typeArgs, null));
                }
            }

            return Util.ToArray(list, Array.Empty<MethodInfo>());
        }

        internal override int GetCurrentToken()
        {
            return this.MetadataToken;
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

    }

}
