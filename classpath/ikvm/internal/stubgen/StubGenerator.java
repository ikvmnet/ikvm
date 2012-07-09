/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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

package ikvm.internal.stubgen;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.ObjectStreamClass;
import java.io.OutputStream;
import java.lang.annotation.Annotation;
import java.lang.reflect.Array;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Hashtable;

public final class StubGenerator implements PrivilegedAction<byte[]>
{
    private Class c;

    private StubGenerator(Class c)
    {
	this.c = c;
    }

    @ikvm.lang.Internal
    public static byte[] generateStub(Class c)
    {
	return AccessController.doPrivileged(new StubGenerator(c));
    }

    public byte[] run()
    {
        boolean includeNonPublicInterfaces = !"true".equalsIgnoreCase(System.getProperty("ikvm.stubgen.skipNonPublicInterfaces"));
        Class outer = c.getDeclaringClass();
        String name = c.getName().replace('.', '/');
        String superClass = null;
        if(c.getSuperclass() != null)
        {
            superClass = c.getSuperclass().getName().replace('.', '/');
        }
        if(c.isInterface())
        {
            superClass = "java/lang/Object";
        }
        ClassFileWriter f = new ClassFileWriter(getRealModifiers(c), name, superClass, 0, 49);
        String genericSignature = BuildGenericSignature(c);
        if(genericSignature != null)
        {
            f.AddStringAttribute("Signature", genericSignature);
        }
        f.AddStringAttribute("IKVM.NET.Assembly", getAssemblyName(c));
        if(isClassDeprecated(c))
        {
            f.AddAttribute(new DeprecatedAttribute(f));
        }
        InnerClassesAttribute innerClassesAttribute = null;
        if(outer != null)
        {
            innerClassesAttribute = new InnerClassesAttribute(f);
            String innername = name;
            // TODO instead of mangling the name, maybe we chould use the new Class APIs (e.g. getSimpleName())
            int idx = name.lastIndexOf('$');
            if(idx >= 0)
            {
                innername = innername.substring(idx + 1);
            }
            innerClassesAttribute.Add(name, outer.getName().replace('.', '/'), innername, getModifiers(c));
        }
        Class[] interfaces = c.getInterfaces();
        for(int i = 0; i < interfaces.length; i++)
        {
            if(includeNonPublicInterfaces || Modifier.isPublic(interfaces[i].getModifiers()))
            {
                f.AddInterface(interfaces[i].getName().replace('.', '/'));
            }
        }
        Class[] innerClasses = c.getDeclaredClasses();
        for(int i = 0; i < innerClasses.length; i++)
        {
	    // TODO add private export support (for bcel)
            int mods = getModifiers(innerClasses[i]);
            if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
            {
                if(innerClassesAttribute == null)
                {
                    innerClassesAttribute = new InnerClassesAttribute(f);
                }
                String namePart = innerClasses[i].getName();
                // TODO name mangling
                namePart = namePart.substring(namePart.lastIndexOf('$') + 1);
                innerClassesAttribute.Add(innerClasses[i].getName().replace('.', '/'), name, namePart, mods);
            }
        }
        java.lang.reflect.Constructor[] constructors = c.getDeclaredConstructors();
        for(int i = 0; i < constructors.length; i++)
        {
            int mods = constructors[i].getModifiers();
            if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
            {
                if(constructors[i].isSynthetic())
                {
                    mods |= Modifiers.Synthetic;
                }
                if(constructors[i].isVarArgs())
                {
                    mods |= Modifiers.VarArgs;
                }
                Class[] args = constructors[i].getParameterTypes();
                FieldOrMethod m = f.AddMethod(mods, "<init>", MakeSig(args, java.lang.Void.TYPE));
                CodeAttribute code = new CodeAttribute(f);
                code.SetMaxLocals(args.length * 2 + 1);
                code.SetMaxStack(3);
                short index1 = f.AddClass("java/lang/UnsatisfiedLinkError");
                short index2 = f.AddString("ikvmstub generated stubs can only be used on IKVM.NET");
                short index3 = f.AddMethodRef("java/lang/UnsatisfiedLinkError", "<init>", "(Ljava/lang/String;)V");
                code.SetByteCode(new byte[] 
                    {
                        (byte)187, (byte)(index1 >> 8), (byte)index1,	// new java/lang/UnsatisfiedLinkError
                        (byte)89,                                       // dup
                        (byte)19,  (byte)(index2 >> 8), (byte)index2,	// ldc_w "..."
                        (byte)183, (byte)(index3 >> 8), (byte)index3,   // invokespecial java/lang/UnsatisfiedLinkError/init()V
                        (byte)191                                       // athrow
                    });
                m.AddAttribute(code);
                AddExceptions(f, m, constructors[i].getExceptionTypes());
                if(isMethodDeprecated(constructors[i]))
                {
                    m.AddAttribute(new DeprecatedAttribute(f));
                }
                String signature = BuildGenericSignature(constructors[i].getTypeParameters(),
                    constructors[i].getGenericParameterTypes(), Void.TYPE, constructors[i].getGenericExceptionTypes());
                if (signature != null)
                {
                    m.AddAttribute(f.MakeStringAttribute("Signature", signature));
                }
                Annotation[] annotations = constructors[i].getDeclaredAnnotations();
                if(annotations.length > 0)
                {
                    m.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, annotations));
                }
                Annotation[][] parameterAnnotations = constructors[i].getParameterAnnotations();
                if(hasParameterAnnotations(parameterAnnotations))
                {
                    m.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, parameterAnnotations));
                }
            }
        }
        java.lang.reflect.Method[] methods = c.getDeclaredMethods();
        for(int i = 0; i < methods.length; i++)
        {
            int mods = methods[i].getModifiers();
            if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
            {
                if((mods & Modifiers.Abstract) == 0)
                {
                    mods |= Modifiers.Native;
                }
                if(methods[i].isBridge())
                {
                    mods |= Modifiers.Bridge;
                }
                if(methods[i].isSynthetic())
                {
                    mods |= Modifiers.Synthetic;
                }
                if(methods[i].isVarArgs())
                {
                    mods |= Modifiers.VarArgs;
                }
                Class[] args = methods[i].getParameterTypes();
                Class retType = methods[i].getReturnType();
                FieldOrMethod m = f.AddMethod(mods, methods[i].getName(), MakeSig(args, retType));
                AddExceptions(f, m, methods[i].getExceptionTypes());
                if(isMethodDeprecated(methods[i]))
                {
                    m.AddAttribute(new DeprecatedAttribute(f));
                }
                String signature = BuildGenericSignature(methods[i].getTypeParameters(),
                    methods[i].getGenericParameterTypes(), methods[i].getGenericReturnType(),
                    methods[i].getGenericExceptionTypes());
                if (signature != null)
                {
                    m.AddAttribute(f.MakeStringAttribute("Signature", signature));
                }
                Object defaultValue = methods[i].getDefaultValue();
                if(defaultValue != null)
                {
                    m.AddAttribute(new AnnotationDefaultClassFileAttribute(f, defaultValue));
                }
                Annotation[] annotations = methods[i].getDeclaredAnnotations();
                if(annotations.length > 0)
                {
                    m.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, annotations));
                }
                Annotation[][] parameterAnnotations = methods[i].getParameterAnnotations();
                if(hasParameterAnnotations(parameterAnnotations))
                {
                    m.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, parameterAnnotations));
                }
            }
        }
        java.lang.reflect.Field[] fields = c.getDeclaredFields();
        for(int i = 0; i < fields.length; i++)
        {
            int mods = fields[i].getModifiers();
            if((mods & (Modifiers.Public | Modifiers.Protected)) != 0)
            {
                // NOTE we can't use Field.get() because that will run the static initializer and
                // also won't allow us to see the difference between constants and blank final fields,
                // so we use a "native" method.
                Object constantValue = getFieldConstantValue(fields[i]);
                Class fieldType = fields[i].getType();
                if(fields[i].isEnumConstant())
                {
                    mods |= Modifiers.Enum;
                }
                if(fields[i].isSynthetic())
                {
                    mods |= Modifiers.Synthetic;
                }
                FieldOrMethod fld = f.AddField(mods, fields[i].getName(), ClassToSig(fieldType), constantValue);
                if(isFieldDeprecated(fields[i]))
                {
                    fld.AddAttribute(new DeprecatedAttribute(f));
                }
                if(fields[i].getGenericType() != fieldType)
                {
                    fld.AddAttribute(f.MakeStringAttribute("Signature", ToSigForm(fields[i].getGenericType())));
                }
                Annotation[] annotations = fields[i].getDeclaredAnnotations();
                if(annotations.length > 0)
                {
                    fld.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, annotations));
                }
            }
        }
	if(innerClassesAttribute != null)
        {
            f.AddAttribute(innerClassesAttribute);
        }
        Annotation[] annotations = c.getDeclaredAnnotations();
        if(annotations.length > 0)
        {
            f.AddAttribute(new RuntimeVisibleAnnotationsAttribute(f, annotations));
        }
        try
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            f.Write(baos);
            return baos.toByteArray();
        }
        catch (IOException x)
        {
            throw new Error(x);
        }
    }

    private static boolean hasParameterAnnotations(Annotation[][] parameterAnnotations)
    {
        for(int i = 0; i < parameterAnnotations.length; i++)
        {
            if(parameterAnnotations[i].length > 0)
            {
                return true;
            }
        }
        return false;
    }

    private static int getModifiers(Class c)
    {
        int mods = c.getModifiers();
        if(c.isAnnotation())
        {
            mods |= Modifiers.Annotation;
        }
        if(c.isEnum())
        {
            mods |= Modifiers.Enum;
        }
        if(c.isSynthetic())
        {
            mods |= Modifiers.Synthetic;
        }
        return mods;
    }

    private static native int getRealModifiers(Class c);
    private static native String getAssemblyName(Class c);
    private static native boolean isClassDeprecated(Class c);
    private static native boolean isFieldDeprecated(Object field);
    private static native boolean isMethodDeprecated(Object method);
    private static native Object getFieldConstantValue(Object field);

    private static void AddExceptions(ClassFileWriter f, FieldOrMethod m, Class[] exceptions)
    {
        if (exceptions.length > 0)
        {
            ExceptionsAttribute attrib = new ExceptionsAttribute(f);
            for (int i = 0; i < exceptions.length; i++)
            {
                attrib.Add(exceptions[i].getName().replace('.', '/'));
            }
            m.AddAttribute(attrib);
        }
    }

    private static String MakeSig(Class[] args, Class ret)
    {
        StringBuilder sb = new StringBuilder();
        sb.append('(');
        for(int i = 0; i < args.length; i++)
        {
            sb.append(ClassToSig(args[i]));
        }
        sb.append(')');
        sb.append(ClassToSig(ret));
        return sb.toString();
    }

    private static String ClassToSig(Class c)
    {
        if(c.isPrimitive())
        {
            if(c == Void.TYPE)
            {
                return "V";
            }
            else if(c == Byte.TYPE)
            {
                return "B";
            }
            else if(c == Boolean.TYPE)
            {
                return "Z";
            }
            else if(c == Short.TYPE)
            {
                return "S";
            }
            else if(c == Character.TYPE)
            {
                return "C";
            }
            else if(c == Integer.TYPE)
            {
                return "I";
            }
            else if(c == Long.TYPE)
            {
                return "J";
            }
            else if(c == Float.TYPE)
            {
                return "F";
            }
            else if(c == Double.TYPE)
            {
                return "D";
            }
            else
            {
                throw new Error();
            }
        }
        else if(c.isArray())
        {
            return "[" + ClassToSig(c.getComponentType());
        }
        else
        {
            return "L" + c.getName().replace('.', '/') + ";";
        }
    }

    private static String BuildGenericSignature(Class c)
    {
        boolean isgeneric = false;
        StringBuilder sb = new StringBuilder();
        java.lang.reflect.TypeVariable[] vars = c.getTypeParameters();
        if(vars.length > 0)
        {
            isgeneric = true;
            sb.append('<');
            for (int i = 0; i < vars.length; i++)
            {
                java.lang.reflect.TypeVariable t = vars[i];
                sb.append(t.getName());
                boolean first = true;
                java.lang.reflect.Type[] bounds = t.getBounds();
                for (int j = 0; j < bounds.length; j++)
                {
                    java.lang.reflect.Type bound = bounds[j];
                    if(first)
                    {
                        first = false;
                        if(bound instanceof Class)
                        {
                            // HACK I don't really understand what the proper criterion is to decide this
                            if(((Class)bound).isInterface())
                            {
                                sb.append(':');
                            }
                        }
                    }
                    sb.append(':').append(ToSigForm(bound));
                }
            }
            sb.append('>');
        }
        java.lang.reflect.Type superclass = c.getGenericSuperclass();
        if(superclass == null)
        {
            sb.append("Ljava/lang/Object;");
        }
        else
        {
            isgeneric |= !(superclass instanceof Class);
            sb.append(ToSigForm(superclass));
        }
        java.lang.reflect.Type[] interfaces = c.getGenericInterfaces();
        for (int i = 0; i < interfaces.length; i++)
        {
            java.lang.reflect.Type t = interfaces[i];
            isgeneric |= !(t instanceof Class);
            sb.append(ToSigForm(t));
        }
        if(isgeneric)
        {
            return sb.toString();
        }
        return null;
    }

    private static String BuildGenericSignature(java.lang.reflect.TypeVariable[] typeParameters,
        java.lang.reflect.Type[] parameterTypes, java.lang.reflect.Type returnType,
        java.lang.reflect.Type[] exceptionTypes)
    {
        boolean isgeneric = false;
        StringBuilder sb = new StringBuilder();
        if(typeParameters.length > 0)
        {
            isgeneric = true;
            sb.append('<');
            for (int i = 0; i < typeParameters.length; i++)
            {
                java.lang.reflect.TypeVariable t = typeParameters[i];
                sb.append(t.getName());
                java.lang.reflect.Type[] bounds = t.getBounds();
                for (int j = 0; j < bounds.length; j++)
                {
                    sb.append(':').append(ToSigForm(bounds[j]));
                }
            }
            sb.append('>');
        }
        sb.append('(');
        for (int i = 0; i < parameterTypes.length; i++)
        {
            java.lang.reflect.Type t = parameterTypes[i];
            isgeneric |= !(t instanceof Class);
            sb.append(ToSigForm(t));
        }
        sb.append(')');
        sb.append(ToSigForm(returnType));
        isgeneric |= !(returnType instanceof Class);
        for (int i = 0; i < exceptionTypes.length; i++)
        {
            java.lang.reflect.Type t = exceptionTypes[i];
            isgeneric |= !(t instanceof Class);
            sb.append('^').append(ToSigForm(t));
        }
        if(isgeneric)
        {
            return sb.toString();
        }
        return null;
    }

    private static String ToSigForm(java.lang.reflect.Type t)
    {
        if(t instanceof java.lang.reflect.ParameterizedType)
        {
            java.lang.reflect.ParameterizedType p = (java.lang.reflect.ParameterizedType)t;
            StringBuilder sb = new StringBuilder();
            sb.append('L').append(((Class)p.getRawType()).getName().replace('.', '/'));
            sb.append('<');
            java.lang.reflect.Type[] args = p.getActualTypeArguments();
            for (int i = 0; i < args.length; i++)
            {
                sb.append(ToSigForm(args[i]));
            }
            sb.append(">;");
            return sb.toString();
        }
        else if(t instanceof java.lang.reflect.TypeVariable)
        {
            return "T" + ((java.lang.reflect.TypeVariable)t).getName() + ";";
        }
        else if(t instanceof java.lang.reflect.WildcardType)
        {
            java.lang.reflect.WildcardType w = (java.lang.reflect.WildcardType)t;
            java.lang.reflect.Type[] lower = w.getLowerBounds();
            java.lang.reflect.Type[] upper = w.getUpperBounds();
            if (lower.length == 0 && upper.length == 0)
            {
                return "*";
            }
            if (lower.length == 1)
            {
                return "-" + ToSigForm(lower[0]);
            }
            if (upper.length == 1)
            {
                return "+" + ToSigForm(upper[0]);
            }
            // TODO
            throw new Error("Not Implemented");
        }
        else if(t instanceof java.lang.reflect.GenericArrayType)
        {
            java.lang.reflect.GenericArrayType a = (java.lang.reflect.GenericArrayType)t;
            return "[" + ToSigForm(a.getGenericComponentType());
        }
        else if(t instanceof Class)
        {
            return ClassToSig((Class)t);
        }
        else
        {
            throw new Error("Not Implemented: " + t);
        }
    }
}

class Modifiers
{
    static final short Public		= 0x0001;
    static final short Private		= 0x0002;
    static final short Protected	= 0x0004;
    static final short Static		= 0x0008;
    static final short Final		= 0x0010;
    static final short Super		= 0x0020;
    static final short Synchronized	= 0x0020;
    static final short Volatile		= 0x0040;
    static final short Bridge		= 0x0040;
    static final short Transient	= 0x0080;
    static final short VarArgs		= 0x0080;
    static final short Native		= 0x0100;
    static final short Interface	= 0x0200;
    static final short Abstract		= 0x0400;
    static final short Strictfp		= 0x0800;
    static final short Synthetic	= 0x1000;
    static final short Annotation	= 0x2000;
    static final short Enum		= 0x4000;
}

class Constant
{
    static final int Utf8 = 1;
    static final int Integer = 3;
    static final int Float = 4;
    static final int Long = 5;
    static final int Double = 6;
    static final int Class = 7;
    static final int String = 8;
    static final int Fieldref = 9;
    static final int Methodref = 10;
    static final int InterfaceMethodref = 11;
    static final int NameAndType = 12;
}

abstract class ConstantPoolItem
{
    abstract void Write(DataOutputStream dos) throws IOException;
}

final class ConstantPoolItemClass extends ConstantPoolItem
{
    private short name_index;

    public ConstantPoolItemClass(short name_index)
    {
        this.name_index = name_index;
    }

    public int hashCode()
    {
        return name_index;
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemClass)
        {
            return ((ConstantPoolItemClass)o).name_index == name_index;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Class);
        dos.writeShort(name_index);
    }
}

final class ConstantPoolItemMethodref extends ConstantPoolItem
{
    private short class_index;
    private short name_and_type_index;

    public ConstantPoolItemMethodref(short class_index, short name_and_type_index)
    {
        this.class_index = class_index;
        this.name_and_type_index = name_and_type_index;
    }

    public int hashCode()
    {
        return (class_index & 0xFFFF) | (name_and_type_index << 16);
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemMethodref)
        {
            ConstantPoolItemMethodref m = (ConstantPoolItemMethodref)o;
            return m.class_index == class_index && m.name_and_type_index == name_and_type_index;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Methodref);
        dos.writeShort(class_index);
        dos.writeShort(name_and_type_index);
    }
}

final class ConstantPoolItemNameAndType extends ConstantPoolItem
{
    private short name_index;
    private short descriptor_index;

    public ConstantPoolItemNameAndType(short name_index, short descriptor_index)
    {
        this.name_index = name_index;
        this.descriptor_index = descriptor_index;
    }

    public int hashCode()
    {
        return (name_index & 0xFFFF) | (descriptor_index << 16);
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemNameAndType)
        {
            ConstantPoolItemNameAndType n = (ConstantPoolItemNameAndType)o;
            return n.name_index == name_index && n.descriptor_index == descriptor_index;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.NameAndType);
        dos.writeShort(name_index);
        dos.writeShort(descriptor_index);
    }
}

final class ConstantPoolItemUtf8 extends ConstantPoolItem
{
    private String str;

    public ConstantPoolItemUtf8(String str)
    {
        this.str = str;
    }

    public int hashCode()
    {
        return str.hashCode();
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemUtf8)
        {
            return ((ConstantPoolItemUtf8)o).str.equals(str);
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Utf8);
        dos.writeUTF(str);
    }
}

final class ConstantPoolItemInt extends ConstantPoolItem
{
    private int v;

    public ConstantPoolItemInt(int v)
    {
        this.v = v;
    }

    public int hashCode()
    {
        return v;
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemInt)
        {
            return ((ConstantPoolItemInt)o).v == v;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Integer);
        dos.writeInt(v);
    }
}

final class ConstantPoolItemLong extends ConstantPoolItem
{
    private long v;

    public ConstantPoolItemLong(long v)
    {
        this.v = v;
    }

    public int hashCode()
    {
        return (int)v;
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemLong)
        {
            return ((ConstantPoolItemLong)o).v == v;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Long);
        dos.writeLong(v);
    }
}

final class ConstantPoolItemFloat extends ConstantPoolItem
{
    private float v;

    public ConstantPoolItemFloat(float v)
    {
        this.v = v;
    }

    public int hashCode()
    {
        return Float.floatToIntBits(v);
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemFloat)
        {
            return ((ConstantPoolItemFloat)o).v == v;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Float);
        dos.writeFloat(v);
    }
}

final class ConstantPoolItemDouble extends ConstantPoolItem
{
    private double v;

    public ConstantPoolItemDouble(double v)
    {
        this.v = v;
    }

    public int hashCode()
    {
        long l = Double.doubleToLongBits(v);
        return ((int)l) ^ ((int)(l >> 32));
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemDouble)
        {
            return ((ConstantPoolItemDouble)o).v == v;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.Double);
        dos.writeDouble(v);
    }
}

final class ConstantPoolItemString extends ConstantPoolItem
{
    private short string_index;

    public ConstantPoolItemString(short string_index)
    {
        this.string_index = string_index;
    }

    public int hashCode()
    {
        return string_index;
    }

    public boolean equals(Object o)
    {
        if(o instanceof ConstantPoolItemString)
        {
            return ((ConstantPoolItemString)o).string_index == string_index;
        }
        return false;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeByte(Constant.String);
        dos.writeShort(string_index);
    }
}

class ClassFileAttribute
{
    private short name_index;

    public ClassFileAttribute(short name_index)
    {
        this.name_index = name_index;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        dos.writeShort(name_index);
    }
}

class DeprecatedAttribute extends ClassFileAttribute
{
    DeprecatedAttribute(ClassFileWriter classFile)
    {
        super(classFile.AddUtf8("Deprecated"));
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(0);
    }
}

class ConstantValueAttribute extends ClassFileAttribute
{
    private short constant_index;

    public ConstantValueAttribute(short name_index, short constant_index)
    {
        super(name_index);
        this.constant_index = constant_index;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(2);
        dos.writeShort(constant_index);
    }
}

class StringAttribute extends ClassFileAttribute
{
    private short string_index;

    public StringAttribute(short name_index, short string_index)
    {
        super(name_index);
        this.string_index = string_index;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(2);
        dos.writeShort(string_index);
    }
}

class InnerClassesAttribute extends ClassFileAttribute
{
    private ClassFileWriter classFile;
    private ArrayList classes = new ArrayList();

    public InnerClassesAttribute(ClassFileWriter classFile)
    {
        super(classFile.AddUtf8("InnerClasses"));
        this.classFile = classFile;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(2 + 8 * classes.size());
        dos.writeShort(classes.size());
        for (int i = 0; i < classes.size(); i++)
        {
            Item it = (Item)classes.get(i);
            dos.writeShort(it.inner_class_info_index);
            dos.writeShort(it.outer_class_info_index);
            dos.writeShort(it.inner_name_index);
            dos.writeShort(it.inner_class_access_flags);
        }
    }

    private class Item
    {
        short inner_class_info_index;
        short outer_class_info_index;
        short inner_name_index;
        short inner_class_access_flags;
    }

    public void Add(String inner, String outer, String name, int access)
    {
        Item i = new Item();
        i.inner_class_info_index = classFile.AddClass(inner);
        i.outer_class_info_index = classFile.AddClass(outer);
        if(name != null)
        {
            i.inner_name_index = classFile.AddUtf8(name);
        }
        i.inner_class_access_flags = (short)access;
        classes.add(i);
    }
}

class ExceptionsAttribute extends ClassFileAttribute
{
    private ClassFileWriter classFile;
    private ArrayList classes = new ArrayList();

    ExceptionsAttribute(ClassFileWriter classFile)
    {
        super(classFile.AddUtf8("Exceptions"));
        this.classFile = classFile;
    }

    void Add(String exceptionClass)
    {
        classes.add(classFile.AddClass(exceptionClass));
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(2 + 2 * classes.size());
        dos.writeShort(classes.size());
        for (int i = 0; i < classes.size(); i++)
        {
            short idx = (Short)classes.get(i);
            dos.writeShort(idx);
        }
    }
}

class RuntimeVisibleAnnotationsAttribute extends ClassFileAttribute
{
    private ClassFileWriter classFile;
    private byte[] buf;

    RuntimeVisibleAnnotationsAttribute(ClassFileWriter classFile, Annotation[] annotations)
    {
        super(classFile.AddUtf8("RuntimeVisibleAnnotations"));
        this.classFile = classFile;
        try
        {
            ByteArrayOutputStream mem = new ByteArrayOutputStream();
            DataOutputStream dos = new DataOutputStream(mem);
            dos.writeShort((short)annotations.length);
            for(Annotation ann : annotations)
            {
                WriteAnnotation(classFile, dos, ann);
            }
            buf = mem.toByteArray();
        }
        catch(Exception x)
        {
            throw new Error(x);
        }
    }

    RuntimeVisibleAnnotationsAttribute(ClassFileWriter classFile, Annotation[][] parameterAnnotations)
    {
        super(classFile.AddUtf8("RuntimeVisibleParameterAnnotations"));
        this.classFile = classFile;
        try
        {
            ByteArrayOutputStream mem = new ByteArrayOutputStream();
            DataOutputStream dos = new DataOutputStream(mem);
            dos.writeByte(parameterAnnotations.length);
            for(Annotation[] annotations : parameterAnnotations)
            {
                dos.writeShort((short)annotations.length);
                for(Annotation ann : annotations)
                {
                    WriteAnnotation(classFile, dos, ann);
                }
            }
            buf = mem.toByteArray();
        }
        catch(Exception x)
        {
            throw new Error(x);
        }
    }

    private static boolean deepEquals(Object o1, Object o2)
    {
        if (o1 == o2)
            return true;

        if (o1 == null || o2 == null)
            return false;

        if (o1 instanceof boolean[] && o2 instanceof boolean[])
            return Arrays.equals((boolean[]) o1, (boolean[]) o2);

        if (o1 instanceof byte[] && o2 instanceof byte[])
            return Arrays.equals((byte[]) o1, (byte[]) o2);

        if (o1 instanceof char[] && o2 instanceof char[])
            return Arrays.equals((char[]) o1, (char[]) o2);

        if (o1 instanceof short[] && o2 instanceof short[])
            return Arrays.equals((short[]) o1, (short[]) o2);

        if (o1 instanceof int[] && o2 instanceof int[])
            return Arrays.equals((int[]) o1, (int[]) o2);

        if (o1 instanceof float[] && o2 instanceof float[])
            return Arrays.equals((float[]) o1, (float[]) o2);

        if (o1 instanceof long[] && o2 instanceof long[])
            return Arrays.equals((long[]) o1, (long[]) o2);

        if (o1 instanceof double[] && o2 instanceof double[])
            return Arrays.equals((double[]) o1, (double[]) o2);

        if (o1 instanceof Object[] && o2 instanceof Object[])
            return Arrays.equals((Object[]) o1, (Object[]) o2);

        return o1.equals(o2);
    }

    static void WriteValue(ClassFileWriter classFile, DataOutputStream dos, Object val)
        throws IOException, IllegalAccessException, InvocationTargetException
    {
        if(val instanceof Boolean)
        {
            dos.writeByte('Z');
            dos.writeShort(classFile.AddInt(((Boolean)val).booleanValue() ? 1 : 0));
        }
        else if(val instanceof Byte)
        {
            dos.writeByte('B');
            dos.writeShort(classFile.AddInt(((Byte)val).byteValue()));
        }
        else if(val instanceof Character)
        {
            dos.writeByte('C');
            dos.writeShort(classFile.AddInt(((Character)val).charValue()));
        }
        else if(val instanceof Short)
        {
            dos.writeByte('S');
            dos.writeShort(classFile.AddInt(((Short)val).shortValue()));
        }
        else if(val instanceof Integer)
        {
            dos.writeByte('I');
            dos.writeShort(classFile.AddInt(((Integer)val).intValue()));
        }
        else if(val instanceof Float)
        {
            dos.writeByte('F');
            dos.writeShort(classFile.AddFloat(((Float)val).floatValue()));
        }
        else if(val instanceof Long)
        {
            dos.writeByte('J');
            dos.writeShort(classFile.AddLong(((Long)val).longValue()));
        }
        else if(val instanceof Double)
        {
            dos.writeByte('D');
            dos.writeShort(classFile.AddDouble(((Double)val).doubleValue()));
        }
        else if(val instanceof String)
        {
            dos.writeByte('s');
            dos.writeShort(classFile.AddUtf8((String)val));
        }
        else if(val instanceof Enum)
        {
            Enum enumVal = (Enum)val;
            Class enumType = enumVal.getDeclaringClass();
            dos.writeByte('e');
            dos.writeShort(classFile.AddUtf8("L" + enumType.getName().replace('.', '/') + ";"));
            dos.writeShort(classFile.AddUtf8(enumVal.name()));
        }
        else if(val instanceof Class)
        {
            dos.writeByte('c');
            Class c = (Class)val;
            String sig;
            if(c.isArray())
            {
                sig = c.getName();
            }
            else if(c == Boolean.TYPE)
            {
                sig = "Z";
            }
            else if(c == Byte.TYPE)
            {
                sig = "B";
            }
            else if(c == Character.TYPE)
            {
                sig = "C";
            }
            else if(c == Short.TYPE)
            {
                sig = "S";
            }
            else if(c == Integer.TYPE)
            {
                sig = "I";
            }
            else if(c == Float.TYPE)
            {
                sig = "F";
            }
            else if(c == Long.TYPE)
            {
                sig = "J";
            }
            else if(c == Double.TYPE)
            {
                sig = "D";
            }
            else if(c == Void.TYPE)
            {
                sig = "V";
            }
            else
            {
                sig = "L" + c.getName() + ";";
            }
            dos.writeShort(classFile.AddUtf8(sig.replace('.', '/')));
        }
        else if(val instanceof Annotation)
        {
            dos.writeByte('@');
            WriteAnnotation(classFile, dos, (Annotation)val);
        }
        else if(val.getClass().isArray())
        {
            dos.writeByte('[');
            int len = Array.getLength(val);
            dos.writeShort((short)len);
            for(int i = 0; i < len; i++)
            {
                WriteValue(classFile, dos, Array.get(val, i));
            }
        }
        else
        {
            throw new Error("Not Implemented: " + val.getClass());
        }
    }

    static void WriteAnnotation(ClassFileWriter classFile, DataOutputStream dos, Annotation ann)
        throws IOException, IllegalAccessException, InvocationTargetException
    {
        Class annotationType = ann.annotationType();
        dos.writeShort(classFile.AddUtf8("L" + annotationType.getName().replace('.', '/') + ";"));
        short numvalues = 0;
        Method[] methods = annotationType.getDeclaredMethods();
        for(int i = 0; i < methods.length; i++)
        {
            final Method m = methods[i];
            AccessController.doPrivileged(new PrivilegedAction() {
                public Object run() {
                    m.setAccessible(true);
                    return null;
                }
            });
            // HACK since there's no way to query if the annotation value is the default value
            // (and I don't want to add a private API for that), we simply compare the value
            // with the default and if they match we won't list the value.
            if(deepEquals(m.invoke(ann), m.getDefaultValue()))
            {
                methods[i] = null;
            }
            else
            {
                numvalues++;
            }
        }
        dos.writeShort(numvalues);
        for(Method m : methods)
        {
            if(m != null)
            {
                dos.writeShort(classFile.AddUtf8(m.getName()));
                WriteValue(classFile, dos, m.invoke(ann));
            }
        }
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(buf.length);
        dos.write(buf);
    }
}

class AnnotationDefaultClassFileAttribute extends ClassFileAttribute
{
    private ClassFileWriter classFile;
    private byte[] buf;

    AnnotationDefaultClassFileAttribute(ClassFileWriter classFile, Object val)
    {
        super(classFile.AddUtf8("AnnotationDefault"));
        this.classFile = classFile;
        try
        {
            ByteArrayOutputStream mem = new ByteArrayOutputStream();
            DataOutputStream dos = new DataOutputStream(mem);
            RuntimeVisibleAnnotationsAttribute.WriteValue(classFile, dos, val);
            buf = mem.toByteArray();
        }
        catch (Exception x)
        {
            throw new Error(x);
        }
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(buf.length);
        dos.write(buf);
    }
}

class FieldOrMethod
{
    private short access_flags;
    private short name_index;
    private short descriptor_index;
    private ArrayList attribs = new ArrayList();

    public FieldOrMethod(int access_flags, short name_index, short descriptor_index)
    {
        this.access_flags = (short)access_flags;
        this.name_index = name_index;
        this.descriptor_index = descriptor_index;
    }

    public void AddAttribute(ClassFileAttribute attrib)
    {
        attribs.add(attrib);
    }

    public void Write(DataOutputStream dos) throws IOException
    {
        dos.writeShort(access_flags);
        dos.writeShort(name_index);
        dos.writeShort(descriptor_index);
        dos.writeShort(attribs.size());
        for(int i = 0; i < attribs.size(); i++)
        {
            ((ClassFileAttribute)attribs.get(i)).Write(dos);
        }
    }
}

class CodeAttribute extends ClassFileAttribute
{
    private ClassFileWriter classFile;
    private short max_stack;
    private short max_locals;
    private byte[] code;

    public CodeAttribute(ClassFileWriter classFile)
    {
        super(classFile.AddUtf8("Code"));
        this.classFile = classFile;
    }

    public void SetMaxStack(int v)
    {
        max_stack = (short)v;
    }

    public void SetMaxLocals(int v)
    {
        max_locals = (short)v;
    }

    public void SetByteCode(byte[] v)
    {
        code = v;
    }

    void Write(DataOutputStream dos) throws IOException
    {
        super.Write(dos);
        dos.writeInt(2 + 2 + 4 + code.length + 2 + 2);
        dos.writeShort(max_stack);
        dos.writeShort(max_locals);
        dos.writeInt(code.length);
        dos.write(code);
        dos.writeShort(0); // no exceptions
        dos.writeShort(0); // no attributes
    }
}

class ClassFileWriter
{
    private ArrayList cplist = new ArrayList();
    private Hashtable cphashtable = new Hashtable();
    private ArrayList fields = new ArrayList();
    private ArrayList methods = new ArrayList();
    private ArrayList attribs = new ArrayList();
    private ArrayList interfaces = new ArrayList();
    private int access_flags;
    private short this_class;
    private short super_class;
    private short minorVersion;
    private short majorVersion;

    public ClassFileWriter(int mods, String name, String superClass, int minorVersion, int majorVersion)
    {
        cplist.add(null);
        access_flags = mods;
        this_class = AddClass(name);
        if(superClass != null)
        {
            super_class = AddClass(superClass);
        }
        this.minorVersion = (short)minorVersion;
        this.majorVersion = (short)majorVersion;
    }

    private short Add(ConstantPoolItem cpi)
    {
        Object index = cphashtable.get(cpi);
        if(index == null)
        {
            index = (short)cplist.size();
            cplist.add(cpi);
            if(cpi instanceof ConstantPoolItemDouble || cpi instanceof ConstantPoolItemLong)
            {
                cplist.add(null);
            }
            cphashtable.put(cpi, index);
        }
        return (Short)index;
    }

    public short AddUtf8(String str)
    {
        return Add(new ConstantPoolItemUtf8(str));
    }

    public short AddClass(String classname)
    {
        return Add(new ConstantPoolItemClass(AddUtf8(classname)));
    }

    public short AddMethodRef(String classname, String methodname, String signature)
    {
        return Add(new ConstantPoolItemMethodref(AddClass(classname), AddNameAndType(methodname, signature)));
    }

    public short AddNameAndType(String name, String type)
    {
        return Add(new ConstantPoolItemNameAndType(AddUtf8(name), AddUtf8(type)));
    }

    public short AddInt(int i)
    {
        return Add(new ConstantPoolItemInt(i));
    }

    public short AddLong(long l)
    {
        return Add(new ConstantPoolItemLong(l));
    }

    public short AddFloat(float f)
    {
        return Add(new ConstantPoolItemFloat(f));
    }

    public short AddDouble(double d)
    {
        return Add(new ConstantPoolItemDouble(d));
    }

    public short AddString(String s)
    {
        return Add(new ConstantPoolItemString(AddUtf8(s)));
    }

    public void AddInterface(String name)
    {
        interfaces.add(AddClass(name));
    }

    public FieldOrMethod AddMethod(int access, String name, String signature)
    {
        FieldOrMethod method = new FieldOrMethod(access, AddUtf8(name), AddUtf8(signature));
        methods.add(method);
        return method;
    }

    public FieldOrMethod AddField(int access, String name, String signature, Object constantValue)
    {
        FieldOrMethod field = new FieldOrMethod(access, AddUtf8(name), AddUtf8(signature));
        if(constantValue != null)
        {
            short constantValueIndex;
            if(constantValue instanceof Boolean)
            {
                constantValueIndex = AddInt(((Boolean)constantValue).booleanValue() ? 1 : 0);
            }
            else if(constantValue instanceof Byte)
            {
                constantValueIndex = AddInt(((Byte)constantValue).byteValue());
            }
            else if(constantValue instanceof Short)
            {
                constantValueIndex = AddInt(((Short)constantValue).shortValue());
            }
            else if(constantValue instanceof Character)
            {
                constantValueIndex = AddInt(((Character)constantValue).charValue());
            }
            else if(constantValue instanceof Integer)
            {
                constantValueIndex = AddInt(((Integer)constantValue).intValue());
            }
            else if(constantValue instanceof Long)
            {
                constantValueIndex = AddLong(((Long)constantValue).longValue());
            }
            else if(constantValue instanceof Float)
            {
                constantValueIndex = AddFloat(((Float)constantValue).floatValue());
            }
            else if(constantValue instanceof Double)
            {
                constantValueIndex = AddDouble(((Double)constantValue).doubleValue());
            }
            else if(constantValue instanceof String)
            {
                constantValueIndex = AddString((String)constantValue);
            }
            else
            {
                throw new Error();
            }
            field.AddAttribute(new ConstantValueAttribute(AddUtf8("ConstantValue"), constantValueIndex));
        }
        fields.add(field);
        return field;
    }

    public ClassFileAttribute MakeStringAttribute(String name, String value)
    {
        return new StringAttribute(AddUtf8(name), AddUtf8(value));
    }

    public void AddStringAttribute(String name, String value)
    {
        attribs.add(MakeStringAttribute(name, value));
    }

    public void AddAttribute(ClassFileAttribute attrib)
    {
        attribs.add(attrib);
    }

    public void Write(OutputStream stream) throws IOException
    {
        DataOutputStream dos = new DataOutputStream(stream);
        dos.writeInt(0xCAFEBABE);
        dos.writeShort(minorVersion);
        dos.writeShort(majorVersion);
        dos.writeShort(cplist.size());
        for(int i = 1; i < cplist.size(); i++)
        {
            ConstantPoolItem cpi = (ConstantPoolItem)cplist.get(i);
            if(cpi != null)
            {
                cpi.Write(dos);
            }
        }
        dos.writeShort(access_flags);
        dos.writeShort(this_class);
        dos.writeShort(super_class);
        // interfaces count
        dos.writeShort(interfaces.size());
        for(int i = 0; i < interfaces.size(); i++)
        {
            dos.writeShort((Short)interfaces.get(i));
        }
        // fields count
        dos.writeShort(fields.size());
        for(int i = 0; i < fields.size(); i++)
        {
            ((FieldOrMethod)fields.get(i)).Write(dos);
        }
        // methods count
        dos.writeShort(methods.size());
        for(int i = 0; i < methods.size(); i++)
        {
            ((FieldOrMethod)methods.get(i)).Write(dos);
        }
        // attributes count
        dos.writeShort(attribs.size());
        for(int i = 0; i < attribs.size(); i++)
        {
            ((ClassFileAttribute)attribs.get(i)).Write(dos);
        }
    }
}
