/*
  Copyright (C) 2008-2015 Jeroen Frijters

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

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Represents a runtime-provided method on an array type, such as Get, Set or Address, which has no definition in
    /// metadata and is referenced through a MemberRef on the array type. Modeled on the method returned by
    /// System.Reflection.Emit's ModuleBuilder.GetArrayMethod; <see cref="BuiltinArrayMethod"/> derives from it.
    /// </summary>
    class ArrayMethod : MethodInfo
    {

        readonly Module _module;
        readonly Type _arrayClass;
        readonly string _methodName;
        readonly CallingConventions _callingConvention;
        readonly Type _returnType;

        /// <summary>
        /// The types of the method's parameters.
        /// </summary>
        protected readonly Type[] parameterTypes;

        MethodSignature _methodSignature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module the method was requested from.</param>
        /// <param name="arrayClass">The array type that declares the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method, or <see langword="null"/> for <see cref="void"/>.</param>
        /// <param name="parameterTypes">The types of the method's parameters. The array is copied.</param>
        internal ArrayMethod(Module module, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            _module = module;
            _arrayClass = arrayClass;
            _methodName = methodName;
            _callingConvention = callingConvention;
            _returnType = returnType ?? module.Universe.System_Void;
            this.parameterTypes = Util.Copy(parameterTypes);
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override MethodImplAttributes GetMethodImplementationFlags() => throw new NotSupportedException();

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override ParameterInfo[] GetParameters() => throw new NotSupportedException();

        /// <inheritdoc />
        internal override Type[] GetParameterTypes() => parameterTypes;

        /// <summary>
        /// Imports the method into the specified module as a MemberRef on the array type.
        /// </summary>
        /// <param name="module">The module to import the method into.</param>
        /// <returns>The MemberRef token.</returns>
        internal override int ImportTo(ModuleBuilder module) => module.ImportMethodOrField(_arrayClass, _methodName, MethodSignature);

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override MethodAttributes Attributes => throw new NotSupportedException();

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _callingConvention;

        /// <summary>
        /// Gets the array type that declares the method.
        /// </summary>
        public override Type DeclaringType => _arrayClass;

        /// <summary>
        /// Gets the signature of the method, built on first use from the return and parameter types.
        /// </summary>
        internal override MethodSignature MethodSignature
        {
            get
            {
                _methodSignature ??= MethodSignature.MakeFromBuilder(_returnType, parameterTypes, new PackedCustomModifiers(), _callingConvention, 0);
                return _methodSignature;
            }
        }

        /// <summary>
        /// Gets the module passed to the constructor.
        /// </summary>
        // FXBUG like .NET's ModuleBuilder.GetArrayMethod, we return the module the method was requested from, not the
        // module associated with the array type
        public override Module Module => _module;

        /// <inheritdoc />
        public override string Name => _methodName;

        /// <inheritdoc />
        internal override int ParameterCount => parameterTypes.Length;

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        // FXBUG like .NET, we throw NotImplementedException
        public override ParameterInfo ReturnParameter => throw new NotImplementedException();

        /// <inheritdoc />
        public override Type ReturnType => _returnType;

        /// <inheritdoc />
        internal override int GetCurrentToken() => MetadataToken;

        /// <summary>
        /// Gets whether the method is complete, which is the case when its array type is.
        /// </summary>
        internal override bool IsBaked => _arrayClass.IsBaked;

    }

}
