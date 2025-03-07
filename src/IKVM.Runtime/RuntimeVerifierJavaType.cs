﻿/*
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
#endif

namespace IKVM.Runtime
{

    class RuntimeVerifierJavaTypeFactory
    {

        readonly RuntimeContext context;

        internal readonly RuntimeJavaType Invalid = null;
        internal readonly RuntimeJavaType Null;
        internal readonly RuntimeJavaType UninitializedThis;
        internal readonly RuntimeJavaType Unloadable;
        internal readonly RuntimeJavaType ExtendedFloat;
        internal readonly RuntimeJavaType ExtendedDouble;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RuntimeVerifierJavaTypeFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            Null = new RuntimeVerifierJavaType(context, "null", 0, null, null);
            UninitializedThis = new RuntimeVerifierJavaType(context, "uninitialized-this", 0, null, null);
            Unloadable = new RuntimeUnloadableJavaType(context, "<verifier>");
            ExtendedFloat = new RuntimeVerifierJavaType(context, "<extfloat>", 0, null, null);
            ExtendedDouble = new RuntimeVerifierJavaType(context, "<extdouble>", 0, null, null);
        }

    }

    // this is a container for the special verifier TypeWrappers
    sealed class RuntimeVerifierJavaType : RuntimeJavaType
    {

        // the TypeWrapper constructor interns the name, so we have to pre-intern here to make sure we have the same string object
        // (if it has only been interned previously)
        static readonly string This = string.Intern("this");
        static readonly string New = string.Intern("new");
        static readonly string Fault = string.Intern("<fault>");

        readonly int index;
        readonly RuntimeJavaType underlyingType;
        readonly MethodAnalyzer methodAnalyzer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="underlyingType"></param>
        /// <param name="methodAnalyzer"></param>
        public RuntimeVerifierJavaType(RuntimeContext context, string name, int index, RuntimeJavaType underlyingType, MethodAnalyzer methodAnalyzer) :
            base(context, TypeFlags.None, RuntimeJavaType.VerifierTypeModifiersHack, name)
        {
            this.index = index;
            this.underlyingType = underlyingType;
            this.methodAnalyzer = methodAnalyzer;
        }

#if EXPORTER

        internal class MethodAnalyzer
        {

            readonly RuntimeContext context;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public MethodAnalyzer(RuntimeContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            internal void ClearFaultBlockException(int dummy) { }

            public RuntimeContext Context => context;

        }

#endif

        public override string ToString()
        {
            return GetType().Name + "[" + Name + "," + index + "," + underlyingType + "]";
        }

        internal static RuntimeJavaType MakeNew(RuntimeJavaType type, int bytecodeIndex)
        {
            return new RuntimeVerifierJavaType(type.Context, New, bytecodeIndex, type, null);
        }

        internal static RuntimeJavaType MakeFaultBlockException(MethodAnalyzer ma, int handlerIndex)
        {
            return new RuntimeVerifierJavaType(ma.Context, Fault, handlerIndex, null, ma);
        }

        // NOTE the "this" type is special, it can only exist in local[0] and on the stack
        // as soon as the type on the stack is merged or popped it turns into its underlying type.
        // It exists to capture the verification rules for non-virtual base class method invocation in .NET 2.0,
        // which requires that the invocation is done on a "this" reference that was directly loaded onto the
        // stack (using ldarg_0).
        internal static RuntimeJavaType MakeThis(RuntimeJavaType type)
        {
            return new RuntimeVerifierJavaType(type.Context, This, 0, type, null);
        }

        internal static bool IsNotPresentOnStack(RuntimeJavaType w)
        {
            return IsNew(w) || IsFaultBlockException(w);
        }

        internal static bool IsNew(RuntimeJavaType w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, New);
        }

        internal static bool IsFaultBlockException(RuntimeJavaType w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, Fault);
        }

        internal static bool IsNullOrUnloadable(RuntimeJavaType w)
        {
            return w == w.Context.VerifierJavaTypeFactory.Null || w.IsUnloadable;
        }

        internal static bool IsThis(RuntimeJavaType w)
        {
            return w != null && w.IsVerifierType && ReferenceEquals(w.Name, This);
        }

        internal static void ClearFaultBlockException(RuntimeJavaType w)
        {
            var vtw = (RuntimeVerifierJavaType)w;
            vtw.methodAnalyzer.ClearFaultBlockException(vtw.Index);
        }

        internal int Index
        {
            get
            {
                return index;
            }
        }

        internal RuntimeJavaType UnderlyingType
        {
            get
            {
                return underlyingType;
            }
        }

        internal override RuntimeJavaType BaseTypeWrapper
        {
            get { return null; }
        }

        internal override RuntimeClassLoader ClassLoader => null;

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

        internal override RuntimeJavaType[] Interfaces
        {
            get
            {
                throw new InvalidOperationException("get_Interfaces called on " + this);
            }
        }

        internal override RuntimeJavaType[] InnerClasses
        {
            get
            {
                throw new InvalidOperationException("get_InnerClasses called on " + this);
            }
        }

        internal override RuntimeJavaType DeclaringTypeWrapper
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
