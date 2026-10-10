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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and creates generic type parameters for dynamically defined generic types and methods.
    /// </summary>
    sealed class GenericTypeParameterBuilder : TypeInfo
    {

        readonly string _name;
        readonly TypeBuilder _type;
        readonly MethodBuilder _method;

        /// <summary>
        /// The index used to form the parameter's token before the module is saved and real GenericParam rows exist.
        /// </summary>
        readonly int _pseudoIndex;

        readonly int _position;
        readonly List<int> _constraints = [];
        int _row;
        int _typeToken;
        Type _baseType;
        GenericParameterAttributes _attr;

        /// <summary>
        /// Initializes a new instance for a generic parameter of a type.
        /// </summary>
        /// <param name="name">The name of the generic parameter.</param>
        /// <param name="type">The type that declares the generic parameter.</param>
        /// <param name="position">The position of the parameter in the type's generic parameter list.</param>
        internal GenericTypeParameterBuilder(string name, TypeBuilder type, int position) :
            this(name, type, null, position, Signature.ELEMENT_TYPE_VAR)
        {
        }

        /// <summary>
        /// Initializes a new instance for a generic parameter of a method.
        /// </summary>
        /// <param name="name">The name of the generic parameter.</param>
        /// <param name="method">The method that declares the generic parameter.</param>
        /// <param name="position">The position of the parameter in the method's generic parameter list.</param>
        internal GenericTypeParameterBuilder(string name, MethodBuilder method, int position) :
            this(name, null, method, position, Signature.ELEMENT_TYPE_MVAR)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name">The name of the generic parameter.</param>
        /// <param name="type">The type that declares the generic parameter, or <see langword="null"/> for a method parameter.</param>
        /// <param name="method">The method that declares the generic parameter, or <see langword="null"/> for a type parameter.</param>
        /// <param name="position">The position of the parameter in the owner's generic parameter list.</param>
        /// <param name="sigElementType">The signature element type: <c>ELEMENT_TYPE_VAR</c> for a type parameter or <c>ELEMENT_TYPE_MVAR</c> for a method parameter.</param>
        GenericTypeParameterBuilder(string name, TypeBuilder type, MethodBuilder method, int position, byte sigElementType) :
            base(sigElementType)
        {
            _name = name;
            _type = type;
            _method = method;
            _position = position;
            _pseudoIndex = ModuleBuilder.AllocGenericParameterIndex();
        }

        /// <summary>
        /// Gets the assembly-qualified name of the current type, which is always <see langword="null"/> for a generic
        /// type parameter.
        /// </summary>
        public override string AssemblyQualifiedName => null;

        /// <summary>
        /// Gets a value indicating whether the parameter has the non-nullable value type constraint.
        /// </summary>
        protected override bool IsValueTypeImpl => (GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;

        /// <summary>
        /// Gets the base type constraint of the current generic type parameter.
        /// </summary>
        public override Type BaseType => _baseType;

        /// <summary>
        /// Not supported for a generic type parameter builder.
        /// </summary>
        /// <returns>This method always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override Type[] __GetDeclaredInterfaces() => throw new NotImplementedException();

        /// <summary>
        /// Gets the attributes of the type, which are always <see cref="TypeAttributes.Public"/> for a generic type
        /// parameter.
        /// </summary>
        public override TypeAttributes Attributes => TypeAttributes.Public;

        /// <summary>
        /// Gets the namespace of the generic type definition that declares the parameter.
        /// </summary>
        public override string Namespace => DeclaringType.Namespace;

        /// <summary>
        /// Gets the name of the current generic type parameter.
        /// </summary>
        public override string Name => _name;

        /// <summary>
        /// Gets the fully qualified name of the type, which is always <see langword="null"/> for a generic type
        /// parameter.
        /// </summary>
        public override string FullName => null;

        /// <summary>
        /// Returns a string representation of the current generic type parameter.
        /// </summary>
        /// <returns>The name of the generic type parameter.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Gets the module builder of the type or method that declares the parameter.
        /// </summary>
        ModuleBuilder ModuleBuilder => _type != null ? _type.ModuleBuilder : _method.ModuleBuilder;

        /// <summary>
        /// Gets the dynamic module that contains the generic type parameter.
        /// </summary>
        public override Module Module => ModuleBuilder;

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that
        /// declared the parameter.
        /// </summary>
        public override int GenericParameterPosition => _position;

        /// <summary>
        /// Gets the generic type definition that the current generic type parameter belongs to, or
        /// <see langword="null"/> for a parameter of a generic method.
        /// </summary>
        public override Type DeclaringType => _type;

        /// <summary>
        /// Gets the generic method that declared the current generic type parameter, or <see langword="null"/> for a
        /// parameter of a generic type.
        /// </summary>
        public override MethodBase DeclaringMethod => _method;

        /// <summary>
        /// Not supported for a generic type parameter builder.
        /// </summary>
        /// <returns>This method always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override Type[] GetGenericParameterConstraints() => throw new NotImplementedException();

        /// <summary>
        /// Gets a combination of flags that describe the covariance and special constraints of the current generic
        /// type parameter.
        /// </summary>
        public override GenericParameterAttributes GenericParameterAttributes
        {
            get
            {
                CheckBaked();
                return _attr;
            }
        }

        /// <summary>
        /// Throws if the type or method that declares the parameter has not been completed.
        /// </summary>
        internal override void CheckBaked()
        {
            if (_type != null)
            {
                _type.CheckBaked();
            }
            else
            {
                _method.CheckBaked();
            }
        }

        /// <summary>
        /// Records a constraint as a token usable from the owning module.
        /// </summary>
        /// <param name="type">The constraint type.</param>
        void AddConstraint(Type type) => _constraints.Add(ModuleBuilder.GetTypeTokenForMemberRef(type));

        /// <summary>
        /// Sets the base type that a type must inherit in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="baseTypeConstraint">The <see cref="Type"/> that must be inherited by any type that is to be substituted for the type parameter.</param>
        public void SetBaseTypeConstraint(Type? baseTypeConstraint)
        {
            _baseType = baseTypeConstraint;
            AddConstraint(baseTypeConstraint);
        }

        /// <summary>
        /// Sets the variance characteristics and special constraints of the generic parameter, such as the
        /// parameterless constructor constraint.
        /// </summary>
        /// <param name="genericParameterAttributes">A bitwise combination of enumeration values that represent the variance characteristics and special constraints of the generic type parameter.</param>
        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes) => _attr = genericParameterAttributes;

        /// <summary>
        /// Gets a value that identifies the generic parameter. Until the module is saved this is a pseudo token formed
        /// from the parameter's allocation index rather than its final GenericParam row.
        /// </summary>
        public override int MetadataToken
        {
            get
            {
                CheckBaked();
                return ((int)TableIndex.GenericParam << 24) | _pseudoIndex;
            }
        }

        /// <summary>
        /// Gets the token of the type or method that owns the parameter, which for a method can be a pseudo token.
        /// </summary>
        internal int OwnerToken => _type != null ? _type.MetadataToken : _method.MetadataToken;

        /// <summary>
        /// Gets the position of the parameter in its owner's generic parameter list, without requiring the owner to be
        /// complete.
        /// </summary>
        internal int Position => _position;

        /// <summary>
        /// Gets the attributes set by <see cref="SetGenericParameterAttributes"/>, without requiring the owner to be
        /// complete.
        /// </summary>
        internal GenericParameterAttributes GenericParameterAttributesValue => _attr;

        /// <summary>
        /// Gets the tokens of the constraints, in the order they were added.
        /// </summary>
        internal List<int> Constraints => _constraints;

        /// <summary>
        /// Records the row the parameter was written to.
        /// </summary>
        /// <param name="row">The row number of the parameter in the GenericParam table.</param>
        internal void SetRow(int row) => _row = row;

        /// <summary>
        /// Gets the token of a TypeSpec in the owning module that refers to this parameter, adding the TypeSpec on first
        /// use.
        /// </summary>
        /// <returns>The TypeSpec token.</returns>
        internal override int GetModuleBuilderToken()
        {
            if (_typeToken == 0)
            {
                var spec = new BlobBuilder(5);
                Signature.WriteTypeSpec(ModuleBuilder, spec, this);
                _typeToken = ModuleBuilder.AddTypeSpec(ModuleBuilder.GetOrAddBlob(spec));
            }
            return _typeToken;
        }

        /// <summary>
        /// Substitutes the parameter with the matching type or method argument supplied by <paramref name="binder"/>.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The bound type argument.</returns>
        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            if (_type != null)
            {
                return binder.BindTypeParameter(this);
            }
            else
            {
                return binder.BindMethodParameter(this);
            }
        }

        /// <summary>
        /// Gets the token of the parameter: its GenericParam row once the module has been saved, otherwise its pseudo
        /// token.
        /// </summary>
        /// <returns>The current token of the parameter.</returns>
        internal override int GetCurrentToken() => ((int)TableIndex.GenericParam << 24) | (ModuleBuilder.IsSaved ? _row : _pseudoIndex);

        /// <summary>
        /// Gets a value indicating whether the type or method that declares the parameter has been completed.
        /// </summary>
        internal override bool IsBaked => ((MemberInfo)_type ?? _method).IsBaked;

    }

}
