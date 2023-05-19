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
using System.Diagnostics;
using System.Runtime.InteropServices;

using IKVM.Attributes;
using IKVM.Runtime;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{

    abstract class MemberWrapper
    {

        readonly TypeWrapper declaringType;
        readonly string name;
        readonly string sig;
        protected readonly Modifiers modifiers;
        HandleWrapper handle;
        MemberFlags flags;

        sealed class HandleWrapper
        {

            internal readonly IntPtr Value;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="obj"></param>
            [System.Security.SecurityCritical]
            internal HandleWrapper(MemberWrapper obj)
            {
                Value = (IntPtr)GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
            }

#if CLASSGC

            /// <summary>
            /// Finalizes the instance.
            /// </summary>
            [System.Security.SecuritySafeCritical]
            ~HandleWrapper()
            {
                if (!Environment.HasShutdownStarted)
                {
                    var h = (GCHandle)Value;
                    if (h.Target == null)
                        h.Free();
                    else
                        GC.ReRegisterForFinalize(this);
                }
            }

#endif

        }

        protected MemberWrapper(TypeWrapper declaringType, string name, string sig, Modifiers modifiers, MemberFlags flags)
        {
            Debug.Assert(declaringType != null);
            this.declaringType = declaringType;
            this.name = String.Intern(name);
            this.sig = String.Intern(sig);
            this.modifiers = modifiers;
            this.flags = flags;
        }

        internal IntPtr Cookie
        {
            get
            {
                lock (this)
                    handle ??= new HandleWrapper(this);

                return handle.Value;
            }
        }

        [System.Security.SecurityCritical]
        internal static MemberWrapper FromCookieImpl(IntPtr cookie)
        {
            return (MemberWrapper)GCHandle.FromIntPtr(cookie).Target;
        }

        internal TypeWrapper DeclaringType => declaringType;

        internal string Name => name;

        internal string Signature => sig;

        internal bool IsAccessibleFrom(TypeWrapper referencedType, TypeWrapper caller, TypeWrapper instance)
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

        private bool IsPublicOrProtectedMemberAccessible(TypeWrapper caller, TypeWrapper instance)
        {
            if (IsPublic || (IsProtected && caller.IsSubTypeOf(DeclaringType) && (IsStatic || instance.IsUnloadable || instance.IsSubTypeOf(caller))))
            {
                return DeclaringType.IsPublic || InPracticeInternalsVisibleTo(caller);
            }
            return false;
        }

        private bool InPracticeInternalsVisibleTo(TypeWrapper caller)
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
#if CLASSGC
            if (DeclaringType.IsDynamic)
            {
                // if we are dynamic, we can just become friends with the caller
                DeclaringType.GetClassLoader().GetTypeWrapperFactory().AddInternalsVisibleTo(caller.TypeAsTBD.Assembly);
                return true;
            }
#endif
            return DeclaringType.InternalsVisibleTo(caller);
        }

        internal bool IsHideFromReflection
        {
            get
            {
                return (flags & MemberFlags.HideFromReflection) != 0;
            }
        }

        internal bool IsExplicitOverride
        {
            get
            {
                return (flags & MemberFlags.ExplicitOverride) != 0;
            }
        }

        internal bool IsMirandaMethod
        {
            get
            {
                return (flags & MemberFlags.MirandaMethod) != 0;
            }
        }

        internal bool IsAccessStub
        {
            get
            {
                return (flags & MemberFlags.AccessStub) != 0;
            }
        }

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

        internal bool IsIntrinsic
        {
            get
            {
                return (flags & MemberFlags.Intrinsic) != 0;
            }
        }

        protected void SetIntrinsicFlag()
        {
            flags |= MemberFlags.Intrinsic;
        }

        protected void SetNonPublicTypeInSignatureFlag()
        {
            flags |= MemberFlags.NonPublicTypeInSignature;
        }

        internal bool HasNonPublicTypeInSignature
        {
            get { return (flags & MemberFlags.NonPublicTypeInSignature) != 0; }
        }

        protected void SetType2FinalField()
        {
            flags |= MemberFlags.Type2FinalField;
        }

        internal bool IsType2FinalField
        {
            get { return (flags & MemberFlags.Type2FinalField) != 0; }
        }

        internal bool HasCallerID
        {
            get
            {
                return (flags & MemberFlags.CallerID) != 0;
            }
        }

        internal bool IsDelegateInvokeWithByRefParameter
        {
            get { return (flags & MemberFlags.DelegateInvokeWithByRefParameter) != 0; }
        }

        internal bool IsNoOp
        {
            get { return (flags & MemberFlags.NoOp) != 0; }
        }

        internal Modifiers Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        internal bool IsStatic
        {
            get
            {
                return (modifiers & Modifiers.Static) != 0;
            }
        }

        internal bool IsInternal
        {
            get
            {
                return (flags & MemberFlags.InternalAccess) != 0;
            }
        }

        internal bool IsPublic
        {
            get
            {
                return (modifiers & Modifiers.Public) != 0;
            }
        }

        internal bool IsPrivate
        {
            get
            {
                return (modifiers & Modifiers.Private) != 0;
            }
        }

        internal bool IsProtected
        {
            get
            {
                return (modifiers & Modifiers.Protected) != 0;
            }
        }

        internal bool IsFinal
        {
            get
            {
                return (modifiers & Modifiers.Final) != 0;
            }
        }
    }

    sealed class PrivateInterfaceMethodWrapper : SmartMethodWrapper
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="method"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        internal PrivateInterfaceMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags) :
            base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
        {

        }

#if EMITTERS

        protected override void CallImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

        protected override void CallvirtImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

#endif

    }

}
