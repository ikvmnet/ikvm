/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{
    // this is a container for the special verifier TypeWrappers
    sealed class VerifierTypeWrapper : TypeWrapper
    {

        // the TypeWrapper constructor interns the name, so we have to pre-intern here to make sure we have the same string object
        // (if it has only been interned previously)
        static readonly string This = string.Intern("this");
        static readonly string New = string.Intern("new");
        static readonly string Fault = string.Intern("<fault>");
        internal static readonly TypeWrapper Invalid = null;
        internal static readonly TypeWrapper Null = new VerifierTypeWrapper("null", 0, null, null);
        internal static readonly TypeWrapper UninitializedThis = new VerifierTypeWrapper("uninitialized-this", 0, null, null);
        internal static readonly TypeWrapper Unloadable = new UnloadableTypeWrapper("<verifier>");
        internal static readonly TypeWrapper ExtendedFloat = new VerifierTypeWrapper("<extfloat>", 0, null, null);
        internal static readonly TypeWrapper ExtendedDouble = new VerifierTypeWrapper("<extdouble>", 0, null, null);

        readonly int index;
        readonly TypeWrapper underlyingType;
        readonly MethodAnalyzer methodAnalyzer;

#if EXPORTER

        internal class MethodAnalyzer
        {
            internal void ClearFaultBlockException(int dummy) { }
        }

#endif

        public override string ToString()
        {
            return GetType().Name + "[" + Name + "," + index + "," + underlyingType + "]";
        }

        internal static TypeWrapper MakeNew(TypeWrapper type, int bytecodeIndex)
        {
            return new VerifierTypeWrapper(New, bytecodeIndex, type, null);
        }

        internal static TypeWrapper MakeFaultBlockException(MethodAnalyzer ma, int handlerIndex)
        {
            return new VerifierTypeWrapper(Fault, handlerIndex, null, ma);
        }

        // NOTE the "this" type is special, it can only exist in local[0] and on the stack
        // as soon as the type on the stack is merged or popped it turns into its underlying type.
        // It exists to capture the verification rules for non-virtual base class method invocation in .NET 2.0,
        // which requires that the invocation is done on a "this" reference that was directly loaded onto the
        // stack (using ldarg_0).
        internal static TypeWrapper MakeThis(TypeWrapper type)
        {
            return new VerifierTypeWrapper(This, 0, type, null);
        }

        internal static bool IsNotPresentOnStack(TypeWrapper w)
        {
            return IsNew(w) || IsFaultBlockException(w);
        }

        internal static bool IsNew(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, New);
        }

        internal static bool IsFaultBlockException(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, Fault);
        }

        internal static bool IsNullOrUnloadable(TypeWrapper w)
        {
            return w == Null || w.IsUnloadable;
        }

        internal static bool IsThis(TypeWrapper w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, This);
        }

        internal static void ClearFaultBlockException(TypeWrapper w)
        {
            VerifierTypeWrapper vtw = (VerifierTypeWrapper)w;
            vtw.methodAnalyzer.ClearFaultBlockException(vtw.Index);
        }

        internal int Index
        {
            get
            {
                return index;
            }
        }

        internal TypeWrapper UnderlyingType
        {
            get
            {
                return underlyingType;
            }
        }

        private VerifierTypeWrapper(string name, int index, TypeWrapper underlyingType, MethodAnalyzer methodAnalyzer)
            : base(TypeFlags.None, TypeWrapper.VerifierTypeModifiersHack, name)
        {
            this.index = index;
            this.underlyingType = underlyingType;
            this.methodAnalyzer = methodAnalyzer;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return null; }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return null;
        }

        protected override void LazyPublishMembers()
        {
            throw new InvalidOperationException("LazyPublishMembers called on " + this);
        }

        internal override Type TypeAsTBD
        {
            get
            {
                throw new InvalidOperationException("get_Type called on " + this);
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                throw new InvalidOperationException("get_Interfaces called on " + this);
            }
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                throw new InvalidOperationException("get_InnerClasses called on " + this);
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                throw new InvalidOperationException("get_DeclaringTypeWrapper called on " + this);
            }
        }

        internal override void Finish()
        {
            throw new InvalidOperationException("Finish called on " + this);
        }
    }

}
