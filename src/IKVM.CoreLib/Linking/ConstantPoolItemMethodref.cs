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

using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a methodref constant.
    /// </summary>
    internal sealed class ConstantPoolItemMethodref<TTypeSystem, TType, TMember, TField, TMethod> : ConstantPoolItemMI<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="data"></param>
        public ConstantPoolItemMethodref(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, MethodrefConstantData data) :
            base(classFile, data.Class, data.NameAndType)
        {

        }

        /// <inheritdoc />
        public override void Link(TType thisJavaType, LoadMode mode)
        {
            base.Link(thisJavaType, mode);

            var ts = ClassFile.TypeSystem;

            var javaType = GetClassType();
            if (javaType != null && ts.IsUnloadable(javaType) == false)
            {
                _method = ts.GetMethod(javaType, Name, Signature, !ReferenceEquals(Name, StringConstants.INIT));
                if (_method != null)
                    ts.EnsureLinked(_method, mode);

                if (Name != StringConstants.INIT &&
                    ts.IsInterface(thisJavaType) == false &&
                    (ts.AllowNonVirtualCalls == false || (ts.GetAccessFlags(thisJavaType) & ClassFileAccessFlags.Super) == ClassFileAccessFlags.Super) &&
                    thisJavaType != javaType &&
                    ts.IsSubTypeOf(thisJavaType, javaType))
                {
                    _invokespecialMethod = ts.GetMethod(ts.GetBaseType(thisJavaType)!, Name, Signature, true);
                    if (_invokespecialMethod != null)
                        ts.EnsureLinked(_invokespecialMethod, mode);
                }
            }
        }

    }

}
