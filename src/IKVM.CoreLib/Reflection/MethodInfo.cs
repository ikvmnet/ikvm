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
using System.Text;

namespace IKVM.Reflection
{

    /// <summary>
    /// Discovers the attributes of a method and provides access to method metadata.
    /// </summary>
    abstract class MethodInfo : MethodBase, IGenericContext, IGenericBinder
    {

        /// <summary>
        /// Initializes a new instance. The constructor is internal to prevent external subclasses.
        /// </summary>
        internal MethodInfo()
        {
        }

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is a method.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Method;

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        public abstract Type ReturnType { get; }

        /// <summary>
        /// Gets a <see cref="ParameterInfo"/> object that contains information about the return type of the method,
        /// such as whether the return type has custom modifiers.
        /// </summary>
        public abstract ParameterInfo ReturnParameter { get; }

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method
        /// definition, and returns a <see cref="MethodInfo"/> object representing the resulting constructed method.
        /// </summary>
        /// <param name="typeArguments">
        /// An array of types to be substituted for the type parameters of the current generic method definition.
        /// </param>
        /// <returns>
        /// A <see cref="MethodInfo"/> object that represents the constructed method formed by substituting the elements
        /// of <paramref name="typeArguments"/> for the type parameters of the current generic method definition.
        /// </returns>
        /// <exception cref="NotSupportedException">This method is not supported by the current object.</exception>
        public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments) => throw new NotSupportedException(GetType().FullName);

        /// <summary>
        /// Returns a <see cref="MethodInfo"/> object that represents a generic method definition from which the current
        /// method can be constructed.
        /// </summary>
        /// <returns>A <see cref="MethodInfo"/> object representing a generic method definition.</returns>
        /// <exception cref="NotSupportedException">This method is not supported by the current object.</exception>
        public virtual MethodInfo GetGenericMethodDefinition() => throw new NotSupportedException(GetType().FullName);

        /// <summary>
        /// Returns a string that represents the method, in the form <c>ReturnType Name[TypeArguments](ParameterTypes)</c>.
        /// </summary>
        /// <returns>A string that represents the current method.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(ReturnType.Name).Append(' ').Append(Name);

            string sep;
            if (IsGenericMethod)
            {
                sb.Append('[');
                sep = "";

                foreach (var arg in GetGenericArguments())
                {
                    sb.Append(sep).Append(arg);
                    sep = ", ";
                }

                sb.Append(']');
            }

            sb.Append('(');
            sep = "";

            foreach (var arg in GetParameters())
            {
                sb.Append(sep).Append(arg.ParameterType);
                sep = ", ";
            }

            sb.Append(')');
            return sb.ToString();
        }

        /// <summary>
        /// Gets whether the method always gets a new slot in the vtable.
        /// </summary>
        internal bool IsNewSlot => (Attributes & MethodAttributes.NewSlot) != 0;

        /// <summary>
        /// Returns the <see cref="MethodInfo"/> object for the method on the direct or indirect base class in which the
        /// method represented by this instance was first declared.
        /// </summary>
        /// <returns>A <see cref="MethodInfo"/> object for the first implementation of this method.</returns>
        public MethodInfo GetBaseDefinition()
        {
            var match = this;
            if (match.IsVirtual)
            {
                for (var type = DeclaringType.BaseType; type != null && !match.IsNewSlot; type = type.BaseType)
                {
                    var method = type.FindMethod(Name, MethodSignature) as MethodInfo;
                    if (method != null && method.IsVirtual)
                        match = method;
                }
            }

            return match;
        }

        /// <summary>
        /// Gets the P/Invoke mapping (ImplMap) of the method, if it has one.
        /// </summary>
        /// <param name="mappingFlags">When this method returns, the mapping flags.</param>
        /// <param name="importName">When this method returns, the name of the imported entry point.</param>
        /// <param name="importScope">When this method returns, the name of the module the entry point is imported from.</param>
        /// <returns><see langword="true"/> if the method has an ImplMap entry; otherwise, <see langword="false"/>.</returns>
        public bool __TryGetImplMap(out ImplMapFlags mappingFlags, out string importName, out string importScope) =>
            Module.__TryGetImplMap(GetCurrentToken(), out mappingFlags, out importName, out importScope);

        /// <summary>
        /// Returns a <see cref="ConstructorInfo"/> that wraps this method, to treat a method named <c>.ctor</c> or
        /// <c>.cctor</c> as a constructor.
        /// </summary>
        /// <returns>The constructor that wraps this method.</returns>
        public ConstructorInfo __AsConstructorInfo() => new ConstructorInfoImpl(this);

        /// <inheritdoc />
        Type IGenericContext.GetGenericTypeArgument(int index) => DeclaringType.GetGenericTypeArgument(index);

        /// <inheritdoc />
        Type IGenericContext.GetGenericMethodArgument(int index) => GetGenericMethodArgument(index);

        /// <summary>
        /// Gets the type argument, or the type parameter of a generic method definition, at the given position.
        /// </summary>
        /// <param name="index">The position of the generic method parameter.</param>
        /// <returns>The type argument or type parameter.</returns>
        /// <exception cref="InvalidOperationException">The method is not generic.</exception>
        internal virtual Type GetGenericMethodArgument(int index) => throw new InvalidOperationException();

        /// <inheritdoc />
        Type IGenericBinder.BindTypeParameter(Type type) => DeclaringType.GetGenericTypeArgument(type.GenericParameterPosition);

        /// <inheritdoc />
        Type IGenericBinder.BindMethodParameter(Type type) => GetGenericMethodArgument(type.GenericParameterPosition);

        /// <inheritdoc />
        internal override MethodBase BindTypeParameters(Type type) => new GenericMethodInstance(DeclaringType.BindTypeParameters(type), this, null);

        /// <inheritdoc />
        internal sealed override MemberInfo SetReflectedType(Type type) => new MethodInfoWithReflectedType(type, this);

        /// <summary>
        /// Gets the pseudo custom attributes of the method: <c>DllImportAttribute</c> for a P/Invoke method and
        /// <c>PreserveSigAttribute</c> for a method with <see cref="MethodImplAttributes.PreserveSig"/>.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter on, or <see langword="null"/> for all.</param>
        /// <returns>The pseudo custom attributes.</returns>
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType)
        {
            var module = Module;
            var list = new List<CustomAttributeData>();

            if ((Attributes & MethodAttributes.PinvokeImpl) != 0 && (attributeType == null || attributeType.IsAssignableFrom(module.Universe.System_Runtime_InteropServices_DllImportAttribute)))
                if (__TryGetImplMap(out var flags, out var importName, out var importScope))
                    if (CustomAttributeData.CreateDllImportPseudoCustomAttribute(module, flags, importName, importScope, GetMethodImplementationFlags()) is { } pseudo)
                        list.Add(pseudo);

            if ((GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) != 0 && (attributeType == null || attributeType.IsAssignableFrom(module.Universe.System_Runtime_InteropServices_PreserveSigAttribute)))
                if (CustomAttributeData.CreatePreserveSigPseudoCustomAttribute(module) is { } pseudo)
                    list.Add(pseudo);

            return list;
        }

    }

}
