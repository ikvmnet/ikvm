/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Runtime.InteropServices;

using IKVM.Attributes;
using IKVM.CoreLib.Linking;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

namespace IKVM.Runtime
{

    abstract class RuntimeJavaMember : ILinkingMember<RuntimeJavaType, RuntimeJavaMember, RuntimeJavaField, RuntimeJavaMethod>
    {

        /// <summary>
        /// Holds a <see cref="GCHandle"/> for the member and ensures it is released on finalization.
        /// </summary>
        class HandleManager
        {

            /// <summary>
            /// Gets the <see cref="RuntimeJavaMember"/> associated with the specified cookie.
            /// </summary>
            /// <param name="cookie"></param>
            /// <returns></returns>
            public static RuntimeJavaMember FromCookie(IntPtr cookie)
            {
                return (RuntimeJavaMember)GCHandle.FromIntPtr(cookie).Target;
            }

            GCHandle handle;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="member"></param>
            public HandleManager(RuntimeJavaMember member)
            {
                handle = GCHandle.Alloc(member, GCHandleType.WeakTrackResurrection);
            }

            /// <summary>
            /// Gets the <see cref="GCHandle"/> value.
            /// </summary>
            public IntPtr Cookie => GCHandle.ToIntPtr(handle);

            /// <summary>
            /// Disposes of the handle.
            /// </summary>
            ~HandleManager()
            {
                handle.Free();
            }

        }

        readonly RuntimeJavaType declaringType;
        readonly string name;
        readonly string sig;
        protected readonly Modifiers modifiers;
        MemberFlags flags;

        HandleManager handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected RuntimeJavaMember(RuntimeJavaType declaringType, string name, string sig, Modifiers modifiers, MemberFlags flags)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            this.name = string.Intern(name);
            this.sig = string.Intern(sig);
            this.modifiers = modifiers;
            this.flags = flags;
        }

        /// <summary>
        /// Gets the cookie for this member. A cookie is a unique platform sized pointer that can be resolved to the member info.
        /// </summary>
        internal nint Cookie
        {
            get
            {
                // synchronize the creation of a handle
                lock (this)
                    handle ??= new HandleManager(this);

                return handle.Cookie;
            }
        }

        /// <summary>
        /// Finds the <see cref="RuntimeJavaMember"/> represented by the specied cookie.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        [System.Security.SecurityCritical]
        internal static RuntimeJavaMember FromCookieImpl(nint cookie)
        {
            return (RuntimeJavaMember)HandleManager.FromCookie(cookie);
        }

        internal RuntimeJavaType DeclaringType => declaringType;

        internal string Name => name;

        internal string Signature => sig;

        internal bool IsAccessibleFrom(RuntimeJavaType referencedType, RuntimeJavaType caller, RuntimeJavaType instance)
        {
            if (referencedType.IsAccessibleFrom(caller))
            {
                return (
                    caller == DeclaringType ||
                    IsPublicOrProtectedMemberAccessible(caller, instance) ||
                    (IsInternal && DeclaringType.InternalsVisibleTo(caller)) ||
                    (!IsPrivate && DeclaringType.IsPackageAccessibleFrom(caller)))
                    // The JVM supports accessing members that have non-public types in their signature from another package,
                    // but the CLI doesn't. It would be nice if we worked around that by emitting extra accessors, but for now
                    // we'll simply disallow such access across assemblies (unless the appropriate InternalsVisibleToAttribute exists).
                    && (!(HasNonPublicTypeInSignature || IsType2FinalField) || InPracticeInternalsVisibleTo(caller));
            }

            return false;
        }

        bool IsPublicOrProtectedMemberAccessible(RuntimeJavaType caller, RuntimeJavaType instance)
        {
            if (IsPublic || (IsProtected && caller.IsSubTypeOf(DeclaringType) && (IsStatic || instance.IsUnloadable || instance.IsSubTypeOf(caller))))
                return DeclaringType.IsPublic || InPracticeInternalsVisibleTo(caller);

            return false;
        }

        bool InPracticeInternalsVisibleTo(RuntimeJavaType caller)
        {
#if !IMPORTER
            if (DeclaringType.TypeAsTBD.Assembly.Equals(caller.TypeAsTBD.Assembly))
            {
                // both the caller and the declaring type are in the same assembly
                // so we know that the internals are visible
                // (this handles the case where we're running in dynamic mode)
                return true;
            }
#endif

            return DeclaringType.InternalsVisibleTo(caller);
        }

        internal bool IsHideFromReflection => (flags & MemberFlags.HideFromReflection) != 0;

        internal bool IsExplicitOverride => (flags & MemberFlags.ExplicitOverride) != 0;

        internal bool IsMirandaMethod => (flags & MemberFlags.MirandaMethod) != 0;

        internal bool IsAccessStub => (flags & MemberFlags.AccessStub) != 0;

        internal bool IsPropertyAccessor
        {
            get
            {
                return (flags & MemberFlags.PropertyAccessor) != 0;
            }
            set
            {
                // this is unsynchronized, so it may only be called during the JavaTypeImpl constructor
                if (value)
                {
                    flags |= MemberFlags.PropertyAccessor;
                }
                else
                {
                    flags &= ~MemberFlags.PropertyAccessor;
                }
            }
        }

        internal bool IsIntrinsic => (flags & MemberFlags.Intrinsic) != 0;

        protected void SetIntrinsicFlag()
        {
            flags |= MemberFlags.Intrinsic;
        }

        protected void SetNonPublicTypeInSignatureFlag()
        {
            flags |= MemberFlags.NonPublicTypeInSignature;
        }

        internal bool HasNonPublicTypeInSignature => (flags & MemberFlags.NonPublicTypeInSignature) != 0;

        protected void SetType2FinalField()
        {
            flags |= MemberFlags.Type2FinalField;
        }

        internal bool IsType2FinalField => (flags & MemberFlags.Type2FinalField) != 0;

        internal bool HasCallerID => (flags & MemberFlags.CallerID) != 0;

        internal bool IsDelegateInvokeWithByRefParameter => (flags & MemberFlags.DelegateInvokeWithByRefParameter) != 0;

        internal bool IsNoOp => (flags & MemberFlags.NoOp) != 0;

        internal Modifiers Modifiers => modifiers;

        internal bool IsStatic => (modifiers & Modifiers.Static) != 0;

        internal bool IsInternal => (flags & MemberFlags.InternalAccess) != 0;

        internal bool IsPublic => (modifiers & Modifiers.Public) != 0;

        internal bool IsPrivate => (modifiers & Modifiers.Private) != 0;

        internal bool IsProtected => (modifiers & Modifiers.Protected) != 0;

        internal bool IsFinal => (modifiers & Modifiers.Final) != 0;

        /// <summary>
        /// Gets whether or not this method is marked as a module initializer.
        /// </summary>
        internal bool IsModuleInitializer => IsStatic && IsPrivate == false && (flags & MemberFlags.ModuleInitializer) != 0;

    }

}
