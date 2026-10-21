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

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides instances of <see cref="MethodAnalyzer"/>.
    /// </summary>
    class MethodAnalyzerFactory
    {

        readonly RuntimeContext _context;

        public readonly RuntimeJavaType ByteArrayType;
        public readonly RuntimeJavaType BooleanArrayType;
        public readonly RuntimeJavaType ShortArrayType;
        public readonly RuntimeJavaType CharArrayType;
        public readonly RuntimeJavaType IntArrayType;
        public readonly RuntimeJavaType FloatArrayType;
        public readonly RuntimeJavaType DoubleArrayType;
        public readonly RuntimeJavaType LongArrayType;
        public readonly RuntimeJavaType JavaLangThreadDeathType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MethodAnalyzerFactory(RuntimeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ByteArrayType = context.PrimitiveJavaTypeFactory.BYTE.MakeArrayType(1);
            BooleanArrayType = context.PrimitiveJavaTypeFactory.BOOLEAN.MakeArrayType(1);
            ShortArrayType = context.PrimitiveJavaTypeFactory.SHORT.MakeArrayType(1);
            CharArrayType = context.PrimitiveJavaTypeFactory.CHAR.MakeArrayType(1);
            IntArrayType = context.PrimitiveJavaTypeFactory.INT.MakeArrayType(1);
            FloatArrayType = context.PrimitiveJavaTypeFactory.FLOAT.MakeArrayType(1);
            DoubleArrayType = context.PrimitiveJavaTypeFactory.DOUBLE.MakeArrayType(1);
            LongArrayType = context.PrimitiveJavaTypeFactory.LONG.MakeArrayType(1);
            JavaLangThreadDeathType = context.ClassLoaderFactory.LoadClassCritical("java.lang.ThreadDeath");
        }

        /// <summary>
        /// Creates a new <see cref="MethodAnalyzer"/>.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="wrapper"></param>
        /// <param name="mw"></param>
        /// <param name="classFile"></param>
        /// <param name="method"></param>
        /// <param name="classLoader"></param>
        /// <returns></returns>
        public MethodAnalyzer Create(RuntimeJavaType host, RuntimeJavaType wrapper, RuntimeJavaMethod mw, ClassFile classFile, Method method, RuntimeClassLoader classLoader)
        {
            return new MethodAnalyzer(_context, host, wrapper, mw, classFile, method, classLoader);
        }

    }

}
