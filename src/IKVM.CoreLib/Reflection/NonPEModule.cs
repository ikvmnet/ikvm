/*
  Copyright (C) 2009-2013 Jeroen Frijters

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

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// Base class for modules that are not backed by a PE image and so have no metadata tables of their own, such as
    /// the in-memory manifest module of an assembly builder or the module of a missing assembly. Operations that need
    /// metadata throw the exceptions supplied by the overridable factory methods.
    /// </summary>
    abstract class NonPEModule : Module
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe">The universe the module belongs to.</param>
        protected NonPEModule(Universe universe) :
            base(universe)
        {
        }

        /// <summary>
        /// Creates the exception thrown by operations that are invalid on a module without metadata.
        /// </summary>
        /// <returns>A new <see cref="System.InvalidOperationException"/> by default.</returns>
        protected virtual Exception InvalidOperationException() => new InvalidOperationException();

        /// <summary>
        /// Creates the exception thrown by operations that this kind of module does not support.
        /// </summary>
        /// <returns>A new <see cref="System.NotSupportedException"/> by default.</returns>
        protected virtual Exception NotSupportedException() => new NotSupportedException();

        /// <summary>
        /// Creates the exception thrown when a metadata token is resolved, since a module without metadata has no valid
        /// tokens.
        /// </summary>
        /// <returns>A new <see cref="System.ArgumentOutOfRangeException"/> by default.</returns>
        protected virtual Exception ArgumentOutOfRangeException() => new ArgumentOutOfRangeException();

        /// <summary>
        /// Throws, because a module without metadata has no <c>&lt;Module&gt;</c> type.
        /// </summary>
        /// <returns>This method does not return.</returns>
        internal sealed override Type GetModuleType() => throw InvalidOperationException();

        /// <summary>
        /// Throws, because a module without metadata has no blob heap.
        /// </summary>
        /// <param name="handle">The blob handle.</param>
        /// <returns>This method does not return.</returns>
        internal sealed override BlobReader GetBlobReader(BlobHandle handle) => throw InvalidOperationException();

        /// <summary>
        /// Throws, because a module without metadata has no exported type table.
        /// </summary>
        /// <returns>This method does not return.</returns>
        public override Type[] __GetExportedTypes() => throw NotSupportedException();

        /// <summary>
        /// Throws, because a module without metadata has no valid type tokens.
        /// </summary>
        /// <param name="metadataToken">The metadata token to resolve.</param>
        /// <param name="context">The generic context used to resolve generic parameters.</param>
        /// <returns>This method does not return.</returns>
        internal sealed override Type ResolveType(int metadataToken, IGenericContext context) => throw ArgumentOutOfRangeException();

        /// <summary>
        /// Throws, because a module without metadata has no valid method tokens.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">The generic type arguments of the type where the token is in scope.</param>
        /// <param name="genericMethodArguments">The generic type arguments of the method where the token is in scope.</param>
        /// <returns>This method does not return.</returns>
        public sealed override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments) =>
            throw ArgumentOutOfRangeException();

        /// <summary>
        /// Throws, because a module without metadata has no valid field tokens.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <param name="genericTypeArguments">The generic type arguments of the type where the token is in scope.</param>
        /// <param name="genericMethodArguments">The generic type arguments of the method where the token is in scope.</param>
        /// <returns>This method does not return.</returns>
        public sealed override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments) =>
            throw ArgumentOutOfRangeException();

        /// <summary>
        /// Throws, because a module without metadata has no valid member tokens.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <param name="genericTypeArguments">The generic type arguments of the type where the token is in scope.</param>
        /// <param name="genericMethodArguments">The generic type arguments of the method where the token is in scope.</param>
        /// <returns>This method does not return.</returns>
        public sealed override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments) =>
            throw ArgumentOutOfRangeException();

        /// <summary>
        /// Throws, because a module without metadata has no user string heap.
        /// </summary>
        /// <param name="metadataToken">A metadata token for a string in the string heap of the module.</param>
        /// <returns>This method does not return.</returns>
        public sealed override string ResolveString(int metadataToken) => throw ArgumentOutOfRangeException();

    }

}
