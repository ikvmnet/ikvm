/*
  Copyright (C) 2009-2015 Jeroen Frijters

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

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents one of the methods the runtime provides on an array type, such as <c>Get</c>, <c>Set</c>,
    /// <c>Address</c> and the constructors, which have no definition in metadata.
    /// </summary>
    sealed class BuiltinArrayMethod : ArrayMethod
    {

        /// <summary>
        /// Describes a parameter or the return value of a <see cref="BuiltinArrayMethod"/>.
        /// </summary>
        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MethodInfo _method;
            readonly Type _type;
            readonly int _pos;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method">The method that declares the parameter.</param>
            /// <param name="type">The type of the parameter.</param>
            /// <param name="pos">The zero-based position of the parameter, or -1 for the return value.</param>
            internal ParameterInfoImpl(MethodInfo method, Type type, int pos)
            {
                _method = method;
                _type = type;
                _pos = pos;
            }

            /// <inheritdoc />
            public override Type ParameterType => _type;

            /// <summary>
            /// Gets the name of the parameter. Built-in array method parameters are unnamed, so this is always
            /// <see langword="null"/>.
            /// </summary>
            public override string Name => null;

            /// <summary>
            /// Gets the attributes for this parameter, which are always <see cref="ParameterAttributes.None"/>.
            /// </summary>
            public override ParameterAttributes Attributes => ParameterAttributes.None;

            /// <inheritdoc />
            public override int Position => _pos;

            /// <summary>
            /// Gets the default value of the parameter, which is always <see langword="null"/>.
            /// </summary>
            public override object RawDefaultValue => null;

            /// <summary>
            /// Gets the custom modifiers of the parameter, which are always empty.
            /// </summary>
            /// <returns>An empty <see cref="CustomModifiers"/>.</returns>
            public override CustomModifiers __GetCustomModifiers() => new CustomModifiers();

            /// <summary>
            /// Gets the marshaling information of the parameter. Built-in array method parameters have none.
            /// </summary>
            /// <param name="fieldMarshal">Always set to the default value.</param>
            /// <returns>Always <see langword="false"/>.</returns>
            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                fieldMarshal = new FieldMarshal();
                return false;
            }

            /// <summary>
            /// Gets the member in which the parameter is implemented, as a <see cref="ConstructorInfo"/> when the
            /// method is a constructor.
            /// </summary>
            public override MemberInfo Member => _method.IsConstructor ? (MethodBase)new ConstructorInfoImpl(_method) : _method;

            /// <summary>
            /// Gets a nil <c>Param</c> metadata token, because the parameter has no metadata row.
            /// </summary>
            public override int MetadataToken => 0x08000000;

            /// <inheritdoc />
            public override Module Module => _method.Module;

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module the array method is referenced from.</param>
        /// <param name="arrayClass">The array type that declares the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="parameterTypes">The parameter types of the method.</param>
        internal BuiltinArrayMethod(Module module, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) :
            base(module, arrayClass, methodName, callingConvention, returnType, parameterTypes)
        {
        }

        /// <summary>
        /// Gets the attributes of the method: public, and additionally <see cref="MethodAttributes.RTSpecialName"/> for
        /// the constructor.
        /// </summary>
        public override MethodAttributes Attributes =>
            Name == ".ctor" ? MethodAttributes.RTSpecialName | MethodAttributes.Public : MethodAttributes.Public;

        /// <summary>
        /// Returns the method implementation flags, which are always <see cref="MethodImplAttributes.IL"/>.
        /// </summary>
        /// <returns><see cref="MethodImplAttributes.IL"/>.</returns>
        public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.IL;

        /// <summary>
        /// Gets a nil <c>MethodDef</c> metadata token, because the method has no metadata row.
        /// </summary>
        public override int MetadataToken => 0x06000000;

        /// <inheritdoc />
        public override ParameterInfo[] GetParameters()
        {
            var parameterInfos = new ParameterInfo[parameterTypes.Length];
            for (var i = 0; i < parameterInfos.Length; i++)
                parameterInfos[i] = new ParameterInfoImpl(this, parameterTypes[i], i);

            return parameterInfos;
        }

        /// <inheritdoc />
        public override ParameterInfo ReturnParameter => new ParameterInfoImpl(this, ReturnType, -1);

    }

}
