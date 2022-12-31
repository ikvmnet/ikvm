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

using IKVM.Internal;

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AnnotationDefaultAttribute : Attribute
    {
        public const byte TAG_ENUM = (byte)'e';
        public const byte TAG_CLASS = (byte)'c';
        public const byte TAG_ANNOTATION = (byte)'@';
        public const byte TAG_ARRAY = (byte)'[';
        public const byte TAG_ERROR = (byte)'?';
        private object defaultValue;

        // element_value encoding:
        // primitives:
        //   boxed values
        // string:
        //   string
        // enum:
        //   new object[] { (byte)'e', "<EnumType>", "<enumvalue>" }
        // class:
        //   new object[] { (byte)'c', "<Type>" }
        // annotation:
        //   new object[] { (byte)'@', "<AnnotationType>", ("name", (element_value))* }
        // array:
        //   new object[] { (byte)'[', (element_value)* }
        // error:
        //   new object[] { (byte)'?', "<exceptionClass>", "<exceptionMessage>" }
        public AnnotationDefaultAttribute(object defaultValue)
        {
            this.defaultValue = Unescape(defaultValue);
        }

        public object Value
        {
            get
            {
                return defaultValue;
            }
        }

        internal static object Escape(object obj)
        {
            return EscapeOrUnescape(obj, true);
        }

        internal static object Unescape(object obj)
        {
            return EscapeOrUnescape(obj, false);
        }

        private static object EscapeOrUnescape(object obj, bool escape)
        {
            string str = obj as string;
            if (str != null)
            {
                return escape ? UnicodeUtil.EscapeInvalidSurrogates(str) : UnicodeUtil.UnescapeInvalidSurrogates(str);
            }
            object[] arr = obj as object[];
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = EscapeOrUnescape(arr[i], escape);
                }
            }
            return obj;
        }

    }

}
