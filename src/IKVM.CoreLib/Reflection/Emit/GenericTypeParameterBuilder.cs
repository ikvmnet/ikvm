/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    internal sealed class GenericTypeParameterBuilder : TypeInfo
    {

        readonly string name;
        readonly TypeBuilder type;
        readonly MethodBuilder method;
        readonly int pseudoIndex;
        readonly int position;
        readonly List<int> constraints = new List<int>();
        int row;
        int typeToken;
        Type baseType;
        GenericParameterAttributes attr;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="position"></param>
        internal GenericTypeParameterBuilder(string name, TypeBuilder type, int position) :
            this(name, type, null, position, Signature.ELEMENT_TYPE_VAR)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="method"></param>
        /// <param name="position"></param>
        internal GenericTypeParameterBuilder(string name, MethodBuilder method, int position) :
            this(name, null, method, position, Signature.ELEMENT_TYPE_MVAR)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="position"></param>
        /// <param name="sigElementType"></param>
        GenericTypeParameterBuilder(string name, TypeBuilder type, MethodBuilder method, int position, byte sigElementType) :
            base(sigElementType)
        {
            this.name = name;
            this.type = type;
            this.method = method;
            this.position = position;
            this.pseudoIndex = ModuleBuilder.AllocGenericParameterIndex();
        }

        public override string AssemblyQualifiedName
        {
            get { return null; }
        }

        protected override bool IsValueTypeImpl
        {
            get { return (this.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0; }
        }

        public override Type BaseType
        {
            get { return baseType; }
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            throw new NotImplementedException();
        }

        public override TypeAttributes Attributes
        {
            get { return TypeAttributes.Public; }
        }

        public override string Namespace
        {
            get { return DeclaringType.Namespace; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override string FullName
        {
            get { return null; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private ModuleBuilder ModuleBuilder
        {
            get { return type != null ? type.ModuleBuilder : method.ModuleBuilder; }
        }

        public override Module Module
        {
            get { return ModuleBuilder; }
        }

        public override int GenericParameterPosition
        {
            get { return position; }
        }

        public override Type DeclaringType
        {
            get { return type; }
        }

        public override MethodBase DeclaringMethod
        {
            get { return method; }
        }

        public override Type[] GetGenericParameterConstraints()
        {
            throw new NotImplementedException();
        }

        public override GenericParameterAttributes GenericParameterAttributes
        {
            get
            {
                CheckBaked();
                return attr;
            }
        }

        internal override void CheckBaked()
        {
            if (type != null)
            {
                type.CheckBaked();
            }
            else
            {
                method.CheckBaked();
            }
        }

        void AddConstraint(Type type)
        {
            constraints.Add(ModuleBuilder.GetTypeTokenForMemberRef(type));
        }

        public void SetBaseTypeConstraint(Type? baseTypeConstraint)
        {
            this.baseType = baseTypeConstraint;
            AddConstraint(baseTypeConstraint);
        }

        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
        {
            this.attr = genericParameterAttributes;
        }

        public override int MetadataToken
        {
            get
            {
                CheckBaked();
                return (GenericParamTable.Index << 24) | pseudoIndex;
            }
        }

        /// <summary>
        /// Gets the token of the type or method that owns the parameter, which for a method can be a pseudo token.
        /// </summary>
        internal int OwnerToken => type != null ? type.MetadataToken : method.MetadataToken;

        internal int Position => position;

        internal GenericParameterAttributes GenericParameterAttributesValue => attr;

        /// <summary>
        /// Gets the tokens of the constraints, in the order they were added.
        /// </summary>
        internal List<int> Constraints => constraints;

        /// <summary>
        /// Records the row the parameter was written to.
        /// </summary>
        /// <param name="row"></param>
        internal void SetRow(int row) => this.row = row;

        internal override int GetModuleBuilderToken()
        {
            if (typeToken == 0)
            {
                var spec = new BlobBuilder(5);
                Signature.WriteTypeSpec(ModuleBuilder, spec, this);
                typeToken = ModuleBuilder.AddTypeSpec(ModuleBuilder.GetOrAddBlob(spec));
            }
            return typeToken;
        }

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            if (type != null)
            {
                return binder.BindTypeParameter(this);
            }
            else
            {
                return binder.BindMethodParameter(this);
            }
        }

        internal override int GetCurrentToken()
        {
            return (GenericParamTable.Index << 24) | (ModuleBuilder.IsSaved ? row : pseudoIndex);
        }

        internal override bool IsBaked
        {
            get { return ((MemberInfo)type ?? method).IsBaked; }
        }

    }

}
