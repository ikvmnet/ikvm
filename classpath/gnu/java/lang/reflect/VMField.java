/*
  Copyright (C) 2005 Jeroen Frijters

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
package gnu.java.lang.reflect;

import java.lang.reflect.Field;

public abstract class VMField
{
    // fieldCookie must be package accessible (actually "assembly") to allow map.xml
    // implementation of LibraryVMInterfaceImpl.getWrapperFromField() to access it.
    protected Object fieldCookie;
    protected Class declaringClass;
    protected boolean isPublic;
    protected int modifiers;

    public final boolean needsAccessCheck(boolean accessible)
    {
        return !accessible & !isPublic;
    }

    public final Class getDeclaringClass()
    {
        return declaringClass;
    }

    public final int getModifiers()
    {
        return modifiers;
    }

    public abstract Field newField();
    public abstract void checkAccess(Object o, Class caller) throws IllegalAccessException;
    public abstract String getName();
    public abstract Class getType();

    public abstract Object get(Object obj);
    public abstract boolean getBoolean(Object obj);
    public abstract byte getByte(Object obj);
    public abstract char getChar(Object obj);
    public abstract short getShort(Object obj);
    public abstract int getInt(Object obj);
    public abstract float getFloat(Object obj);
    public abstract long getLong(Object obj);
    public abstract double getDouble(Object obj);
    public abstract void set(Object obj, Object val, boolean accessible) throws IllegalAccessException;
    public abstract void setBoolean(Object obj, boolean val, boolean accessible) throws IllegalAccessException;
    public abstract void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException;
    public abstract void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException;
    public abstract void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException;
    public abstract void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException;
    public abstract void setFloat(Object obj, float val, boolean accessible) throws IllegalAccessException;
    public abstract void setLong(Object obj, long val, boolean accessible) throws IllegalAccessException;
    public abstract void setDouble(Object obj, double val, boolean accessible) throws IllegalAccessException;
}
