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
using System.Collections.Generic;

namespace IKVM.Reflection.Reader
{
    abstract class TypeParameterType : TypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sigElementType"></param>
        protected TypeParameterType(byte sigElementType) :
            base(sigElementType)
        {

        }

        public sealed override string AssemblyQualifiedName
        {
            get { return null; }
        }

        protected sealed override bool IsValueTypeImpl
        {
            get { return (this.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0; }
        }

        public sealed override Type BaseType
        {
            get
            {
                foreach (var type in GetGenericParameterConstraints())
                    if (!type.IsInterface && !type.IsGenericParameter)
                        return type;

                return this.IsValueType ? this.Module.Universe.System_ValueType : this.Module.Universe.System_Object;
            }
        }

        public override Type[] __GetDeclaredInterfaces()
        {
            var list = new List<Type>();
            foreach (var type in GetGenericParameterConstraints())
                if (type.IsInterface)
                    list.Add(type);

            return list.ToArray();
        }

        public sealed override TypeAttributes Attributes
        {
            get { return TypeAttributes.Public; }
        }

        public sealed override string FullName
        {
            get { return null; }
        }

        public sealed override string ToString()
        {
            return this.Name;
        }

        protected sealed override bool ContainsMissingTypeImpl
        {
            get { return ContainsMissingType(GetGenericParameterConstraints()); }
        }
    }

}
