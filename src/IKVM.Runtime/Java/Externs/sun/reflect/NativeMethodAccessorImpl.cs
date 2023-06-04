using System;
using System.Reflection;

using IKVM.Attributes;
using IKVM.Internal;
using IKVM.Runtime;

namespace IKVM.Java.Externs.sun.reflect
{

    /// <summary>
    /// Implements the native methods for 'NativeMethodAccessorImpl'.
    /// </summary>
    static class NativeMethodAccessorImpl
    {

#if FIRST_PASS == false

        static object ConvertPrimitive(TypeWrapper tw, object value)
        {
            if (tw == PrimitiveTypeWrapper.BOOLEAN)
            {
                if (value is global::java.lang.Boolean boolean)
                    return boolean.booleanValue();
            }
            else if (tw == PrimitiveTypeWrapper.BYTE)
            {
                if (value is global::java.lang.Byte @byte)
                    return @byte.byteValue();
            }
            else if (tw == PrimitiveTypeWrapper.CHAR)
            {
                if (value is global::java.lang.Character character)
                    return character.charValue();
            }
            else if (tw == PrimitiveTypeWrapper.SHORT)
            {
                if (value is global::java.lang.Short || value is global::java.lang.Byte)
                    return ((global::java.lang.Number)value).shortValue();
            }
            else if (tw == PrimitiveTypeWrapper.INT)
            {
                if (value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                    return ((global::java.lang.Number)value).intValue();
                else if (value is global::java.lang.Character)
                    return (int)((global::java.lang.Character)value).charValue();
            }
            else if (tw == PrimitiveTypeWrapper.LONG)
            {
                if (value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                    return ((global::java.lang.Number)value).longValue();
                else if (value is global::java.lang.Character)
                    return (long)((global::java.lang.Character)value).charValue();
            }
            else if (tw == PrimitiveTypeWrapper.FLOAT)
            {
                if (value is global::java.lang.Float || value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                    return ((global::java.lang.Number)value).floatValue();
                else if (value is global::java.lang.Character)
                    return (float)((global::java.lang.Character)value).charValue();
            }
            else if (tw == PrimitiveTypeWrapper.DOUBLE)
            {
                if (value is global::java.lang.Double || value is global::java.lang.Float || value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                    return ((global::java.lang.Number)value).doubleValue();
                else if (value is global::java.lang.Character)
                    return (double)((global::java.lang.Character)value).charValue();
            }

            throw new global::java.lang.IllegalArgumentException();
        }

        static object[] ConvertArgs(ClassLoaderWrapper loader, TypeWrapper[] argumentTypes, object[] args)
        {
            var nargs = new object[args == null ? 0 : args.Length];
            if (nargs.Length != argumentTypes.Length)
                throw new global::java.lang.IllegalArgumentException("wrong number of arguments");

            for (int i = 0; i < nargs.Length; i++)
            {
                if (argumentTypes[i].IsPrimitive)
                {
                    nargs[i] = ConvertPrimitive(argumentTypes[i], args[i]);
                }
                else
                {
                    if (args[i] != null && !argumentTypes[i].EnsureLoadable(loader).IsInstance(args[i]))
                        throw new global::java.lang.IllegalArgumentException();

                    nargs[i] = argumentTypes[i].GhostWrap(args[i]);
                }
            }

            return nargs;
        }

#endif

        [HideFromJava]
        public static object newInstance0(object m, object obj, object[] args)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var mw = MethodWrapper.FromExecutable((global::java.lang.reflect.Executable)m);
                if (mw == null)
                    throw new InternalException();

                if (!mw.IsStatic && !mw.DeclaringType.IsInstance(obj))
                {
                    if (obj == null)
                        throw new global::java.lang.NullPointerException();

                    throw new global::java.lang.IllegalArgumentException("object is not an instance of declaring class");
                }

                args = ConvertArgs(mw.DeclaringType.GetClassLoader(), mw.GetParameters(), args);

                // if the method is an interface method, we must explicitly run <clinit>,
                // because .NET reflection doesn't
                if (mw.DeclaringType.IsInterface)
                    mw.DeclaringType.RunClassInit();

                if (mw.HasCallerID)
                    throw new NotImplementedException();
                    //args = ArrayUtil.Concat(args, callerID);

                var retval = mw.Invoke(obj, args);

                if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
                    retval = JVM.Box(retval);
                else
                    retval = mw.ReturnType.GhostUnwrap(retval);

                return retval;
            }
            catch (global::java.lang.Throwable)
            {
                throw;
            }
            catch (TargetInvocationException x)
            {
                throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(x.InnerException));
            }
            catch (Exception x)
            {
                throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(x));
            }
#endif
        }

    }

}
