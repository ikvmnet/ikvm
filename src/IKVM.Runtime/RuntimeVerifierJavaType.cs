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
#endif

namespace IKVM.Runtime
{

    class RuntimeVerifierJavaTypeFactory
    {

        readonly RuntimeContext context;

        internal readonly RuntimeJavaType Null;
        internal readonly RuntimeJavaType Unloadable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RuntimeVerifierJavaTypeFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            Null = new RuntimeVerifierJavaType(context, "null");
            Unloadable = new RuntimeUnloadableJavaType(context, "<verifier>");
        }

    }

    /// <summary>
    /// The type of the null reference, where a value of it has to be described by a <see cref="RuntimeJavaType"/>.
    /// The verifier itself describes values by <c>VerificationType</c>.
    /// </summary>
    sealed class RuntimeVerifierJavaType : RuntimeJavaType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        public RuntimeVerifierJavaType(RuntimeContext context, string name) :
            base(context, TypeFlags.None, RuntimeJavaType.VerifierTypeModifiersHack, name)
        {

        }

        public override string ToString()
        {
            return GetType().Name + "[" + Name + "]";
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
