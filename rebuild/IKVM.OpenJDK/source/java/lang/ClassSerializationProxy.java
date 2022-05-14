/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
package java.lang;

import cli.System.Runtime.Serialization.IObjectReference;
import cli.System.Runtime.Serialization.SerializationException;
import cli.System.Runtime.Serialization.StreamingContext;

@cli.System.SerializableAttribute.Annotation
final class ClassSerializationProxy implements IObjectReference
{
    private cli.System.Type type;
    private String sig;

    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public Object GetRealObject(StreamingContext context)
    {
        if (sig != null)
        {
            if (sig.length() == 1)
            {
                switch (sig.charAt(0))
                {
                    case 'B':
                        return Byte.TYPE;
                    case 'C':
                        return Character.TYPE;
                    case 'D':
                        return Double.TYPE;
                    case 'F':
                        return Float.TYPE;
                    case 'I':
                        return Integer.TYPE;
                    case 'J':
                        return Long.TYPE;
                    case 'S':
                        return Short.TYPE;
                    case 'Z':
                        return Boolean.TYPE;
                    case 'V':
                        return Void.TYPE;
                }
            }
            String className;
            if (sig.charAt(0) == 'L')
            {
                className = sig.substring(1, sig.length() - 1);
            }
            else
            {
                className = sig;
            }
            try
            {
                return Class.forName(className, false, Thread.currentThread().getContextClassLoader());
            }
            catch (ClassNotFoundException x)
            {
                ikvm.runtime.Util.throwException(new SerializationException(x.getMessage(), x));
            }
        }
        return ikvm.runtime.Util.getClassFromTypeHandle(type.get_TypeHandle());
    }
}
