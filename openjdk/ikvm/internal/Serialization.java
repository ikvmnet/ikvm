/*
  Copyright (C) 2009-2010 Jeroen Frijters

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
package ikvm.internal;

import cli.System.Runtime.Serialization.SerializationException;
import cli.System.Runtime.Serialization.SerializationInfo;
import cli.System.Security.Permissions.SecurityAction;
import cli.System.Security.Permissions.SecurityPermissionAttribute;
import com.sun.xml.internal.ws.developer.ServerSideException;
import java.io.InteropObjectInputStream;
import java.io.InteropObjectOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public final class Serialization
{
    private Serialization() { }
    
    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.Demand, SerializationFormatter = true)
    public static void writeObject(Object obj, SerializationInfo info)
    {
        InteropObjectOutputStream.writeObject(obj, info);
    }
    
    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.Demand, SerializationFormatter = true)
    public static void readObject(Object obj, SerializationInfo info)
    {
        InteropObjectInputStream.readObject(obj, info);
    }

    public static Object writeReplace(cli.System.Exception x)
    {
        ServerSideException sse = new ServerSideException(x.getClass().getName(), x.get_Message());
        sse.initCause(x.get_InnerException());
        sse.setStackTrace(x.getStackTrace());
        return sse;
    }
}
