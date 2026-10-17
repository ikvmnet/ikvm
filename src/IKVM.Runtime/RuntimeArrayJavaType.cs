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
using System.Diagnostics;

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    sealed class RuntimeArrayJavaType : RuntimeJavaType
    {

        volatile RuntimeJavaType[] interfaces;
        readonly RuntimeJavaType ultimateElementTypeWrapper;
        Type arrayType;
        bool finished;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ultimateElementTypeWrapper"></param>
        /// <param name="name"></param>
        internal RuntimeArrayJavaType(RuntimeContext context, RuntimeJavaType ultimateElementTypeWrapper, string name) :
            base(context, ultimateElementTypeWrapper.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.Modifiers & Modifiers.Public), name)
        {
            Debug.Assert(!ultimateElementTypeWrapper.IsArray);
            this.ultimateElementTypeWrapper = ultimateElementTypeWrapper;
        }

        internal override RuntimeJavaType BaseTypeWrapper
        {
            get { return Context.JavaBase.TypeOfJavaLangObject; }
        }

        internal override RuntimeClassLoader ClassLoader => ultimateElementTypeWrapper.ClassLoader;

        internal static MethodInfo GetCloneMethod(RuntimeContext context)
        {
            return context.Types.Array.GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }

        protected override void LazyPublishMembers()
        {
            var mw = new RuntimeSimpleCallJavaMethod(this, "clone", "()Ljava.lang.Object;", GetCloneMethod(Context), Context.JavaBase.TypeOfJavaLangObject, Array.Empty<RuntimeJavaType>(), Modifiers.Public, MemberFlags.HideFromReflection, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt);
            mw.Link();
            SetMethods(new RuntimeJavaMethod[] { mw });
            SetFields(Array.Empty<RuntimeJavaField>());
        }

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                return Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.ReflectiveModifiers & Modifiers.AccessMask);
            }
        }

        internal override string SignatureName
        {
            get
            {
                // for arrays the signature name is the same as the normal name
                return Name;
            }
        }

        internal override RuntimeJavaType[] Interfaces
        {
            get
            {
                if (interfaces == null)
                {
                    RuntimeJavaType[] tw = new RuntimeJavaType[2];
                    tw[0] = Context.JavaBase.TypeOfJavaLangCloneable;
                    tw[1] = Context.JavaBase.TypeOfJavaIoSerializable;
                    interfaces = tw;
                }
                return interfaces;
            }
        }

        internal override Type TypeAsTBD
        {
            get
            {
                while (arrayType == null)
                {
                    bool prevFinished = finished;
                    Type type = MakeArrayType(ultimateElementTypeWrapper.TypeAsArrayType, this.ArrayRank);
                    if (prevFinished)
                    {
                        // We were already finished prior to the call to MakeArrayType, so we can safely
                        // set arrayType to the finished type.
                        // Note that this takes advantage of the fact that once we've been finished,
                        // we can never become unfinished.
                        arrayType = type;
                    }
                    else
                    {
                        lock (this)
                        {
                            // To prevent a race with Finish, we can only set arrayType in this case
                            // (inside the locked region) if we've not already finished. If we have
                            // finished, we need to rerun MakeArrayType on the now finished element type.
                            // Note that there is a benign race left, because it is possible that another
                            // thread finishes right after we've set arrayType and exited the locked
                            // region. This is not problem, because TypeAsTBD is only guaranteed to
                            // return a finished type *after* Finish has been called.
                            if (!finished)
                            {
                                arrayType = type;
                            }
                        }
                    }
                }

                return arrayType;
            }
        }

        internal override void Finish()
        {
            if (!finished)
            {
                ultimateElementTypeWrapper.Finish();
                lock (this)
                {
                    // Now that we've finished the element type, we must clear arrayType,
                    // because it may still refer to a TypeBuilder. Note that we have to
                    // do this atomically with setting "finished", to prevent a race
                    // with TypeAsTBD.
                    finished = true;
                    arrayType = null;
                }
            }
        }

        internal override bool IsFastClassLiteralSafe
        {
            // here we have to deal with the somewhat strange fact that in Java you cannot represent primitive type class literals,
            // but you can represent arrays of primitive types as a class literal
            get { return ultimateElementTypeWrapper.IsFastClassLiteralSafe || ultimateElementTypeWrapper.IsPrimitive; }
        }

        internal override RuntimeJavaType GetUltimateElementTypeWrapper()
        {
            return ultimateElementTypeWrapper;
        }

        internal static Type MakeArrayType(Type type, int dims)
        {
            // NOTE this is not just an optimization, but it is also required to
            // make sure that ReflectionOnly types stay ReflectionOnly types
            // (in particular instantiations of generic types from mscorlib that
            // have ReflectionOnly type parameters).
            for (int i = 0; i < dims; i++)
                type = type.MakeArrayType();

            return type;
        }

    }

}
