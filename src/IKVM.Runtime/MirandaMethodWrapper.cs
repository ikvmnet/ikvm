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
using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{
    abstract class MirandaMethodWrapper : SmartMethodWrapper
    {

        readonly MethodWrapper ifmethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="ifmethod"></param>
        /// <param name="modifiers"></param>
        MirandaMethodWrapper(TypeWrapper declaringType, MethodWrapper ifmethod, Modifiers modifiers) :
            base(declaringType, ifmethod.Name, ifmethod.Signature, null, null, null, modifiers, MemberFlags.HideFromReflection | MemberFlags.MirandaMethod)
        {
            this.ifmethod = ifmethod;
        }

        sealed class AbstractMirandaMethodWrapper : MirandaMethodWrapper
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="ifmethod"></param>
            internal AbstractMirandaMethodWrapper(TypeWrapper declaringType, MethodWrapper ifmethod) :
                base(declaringType, ifmethod, Modifiers.Public | Modifiers.Abstract)
            {

            }

        }

        sealed class DefaultMirandaMethodWrapper : MirandaMethodWrapper
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="ifmethod"></param>
            internal DefaultMirandaMethodWrapper(TypeWrapper declaringType, MethodWrapper ifmethod) :
                base(declaringType, ifmethod, Modifiers.Public)
            {

            }

        }

        sealed class ErrorMirandaMethodWrapper : MirandaMethodWrapper
        {

            string error;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="ifmethod"></param>
            /// <param name="error"></param>
            internal ErrorMirandaMethodWrapper(TypeWrapper declaringType, MethodWrapper ifmethod, string error) :
                base(declaringType, ifmethod, Modifiers.Public)
            {
                this.error = error;
            }

            protected override MirandaMethodWrapper AddConflictError(MethodWrapper mw)
            {
                error += " " + mw.DeclaringType.Name + "." + mw.Name;
                return this;
            }

            internal override string Error
            {
                get { return error; }
            }

        }

        internal static MirandaMethodWrapper Create(TypeWrapper declaringType, MethodWrapper ifmethod)
        {
            DefaultMirandaMethodWrapper dmmw = ifmethod as DefaultMirandaMethodWrapper;
            if (dmmw != null)
            {
                return new DefaultMirandaMethodWrapper(declaringType, dmmw.BaseMethod);
            }
            return ifmethod.IsAbstract
                ? (MirandaMethodWrapper)new AbstractMirandaMethodWrapper(declaringType, ifmethod)
                : (MirandaMethodWrapper)new DefaultMirandaMethodWrapper(declaringType, ifmethod);
        }

        internal MirandaMethodWrapper Update(MethodWrapper mw)
        {
            if (ifmethod == mw)
            {
                // an interface can be implemented multiple times
                return this;
            }
            else if (ifmethod.DeclaringType.ImplementsInterface(mw.DeclaringType))
            {
                // we can override a base interface without problems
                return this;
            }
            else if (mw.DeclaringType.ImplementsInterface(ifmethod.DeclaringType))
            {
                return Create(DeclaringType, mw);
            }
            else if (!ifmethod.IsAbstract && !mw.IsAbstract)
            {
                return AddConflictError(mw);
            }
            else if (!ifmethod.IsAbstract && mw.IsAbstract)
            {
                return new ErrorMirandaMethodWrapper(DeclaringType, mw, DeclaringType.Name + "." + Name + Signature);
            }
            else
            {
                return this;
            }
        }

        protected virtual MirandaMethodWrapper AddConflictError(MethodWrapper mw)
        {
            return new ErrorMirandaMethodWrapper(DeclaringType, ifmethod, "Conflicting default methods:")
                .AddConflictError(ifmethod)
                .AddConflictError(mw);
        }

        internal bool IsConflictError
        {
            get { return Error != null && Error.StartsWith("Conflicting default methods:"); }
        }

        internal MethodWrapper BaseMethod
        {
            get { return ifmethod; }
        }

        internal virtual string Error
        {
            get { return null; }
        }

#if EMITTERS
        protected override void CallImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

        protected override void CallvirtImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Callvirt, GetMethod());
        }
#endif // EMITTERS
    }

}
