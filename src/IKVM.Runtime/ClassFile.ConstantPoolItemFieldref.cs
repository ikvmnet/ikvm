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

using IKVM.ByteCode.Reading;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemFieldref : ConstantPoolItemFMI
        {

            FieldWrapper field;
            RuntimeJavaType fieldTypeWrapper;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="reader"></param>
            internal ConstantPoolItemFieldref(FieldrefConstantReader reader) : base(reader.Record.ClassIndex, reader.Record.NameAndTypeIndex)
            {

            }

            protected override void Validate(string name, string descriptor, int majorVersion)
            {
                if (!IsValidFieldSig(descriptor))
                    throw new ClassFormatError("Invalid field signature \"{0}\"", descriptor);
                if (!IsValidFieldName(name, majorVersion))
                    throw new ClassFormatError("Invalid field name \"{0}\"", name);
            }

            internal RuntimeJavaType GetFieldType()
            {
                return fieldTypeWrapper;
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                base.Link(thisType, mode);
                lock (this)
                {
                    if (fieldTypeWrapper != null)
                    {
                        return;
                    }
                }
                FieldWrapper fw = null;
                RuntimeJavaType wrapper = GetClassType();
                if (wrapper == null)
                {
                    return;
                }
                if (!wrapper.IsUnloadable)
                {
                    fw = wrapper.GetFieldWrapper(Name, Signature);
                    if (fw != null)
                    {
                        fw.Link(mode);
                    }
                }
                ClassLoaderWrapper classLoader = thisType.GetClassLoader();
                RuntimeJavaType fld = classLoader.FieldTypeWrapperFromSig(this.Signature, mode);
                lock (this)
                {
                    if (fieldTypeWrapper == null)
                    {
                        fieldTypeWrapper = fld;
                        field = fw;
                    }
                }
            }

            internal FieldWrapper GetField()
            {
                return field;
            }

            internal override RuntimeJavaMember GetMember()
            {
                return field;
            }

        }

    }

}
