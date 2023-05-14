/*
  Copyright (C) 2011-2015 Jeroen Frijters

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
using System.Reflection;

using IKVM.Internal;

namespace IKVM.Java.Externs.java.lang.invoke
{

    static class MethodHandleNatives
    {

        // called from map.xml as a replacement for Class.isInstance() in JlInvoke.MethodHandleImpl.castReference()
        public static bool Class_isInstance(global::java.lang.Class clazz, object obj)
        {
            var tw = TypeWrapper.FromClass(clazz);
            // handle the type system hole that is caused by arrays being both derived from cli.System.Array and directly from java.lang.Object
            return tw.IsInstance(obj) || (tw == CoreClasses.cli.System.Object.Wrapper && obj is Array);
        }

        public static void init(global::java.lang.invoke.MemberName self, object refObj)
        {
            init(self, refObj, false);
        }

        // this overload is called via a map.xml patch to the MemberName(Method, boolean) constructor, because we need wantSpecial
        public static void init(global::java.lang.invoke.MemberName self, object refObj, bool wantSpecial)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            global::java.lang.reflect.Method method;
            global::java.lang.reflect.Constructor constructor;
            global::java.lang.reflect.Field field;
            if ((method = refObj as global::java.lang.reflect.Method) != null)
            {
                InitMethodImpl(self, MethodWrapper.FromExecutable(method), wantSpecial);
            }
            else if ((constructor = refObj as global::java.lang.reflect.Constructor) != null)
            {
                InitMethodImpl(self, MethodWrapper.FromExecutable(constructor), wantSpecial);
            }
            else if ((field = refObj as global::java.lang.reflect.Field) != null)
            {
                FieldWrapper fw = FieldWrapper.FromField(field);
                self._clazz(fw.DeclaringType.ClassObject);
                int flags = (int)fw.Modifiers | global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_FIELD;
                flags |= (fw.IsStatic ? global::java.lang.invoke.MethodHandleNatives.Constants.REF_getStatic : global::java.lang.invoke.MethodHandleNatives.Constants.REF_getField) << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
                self._flags(flags);
            }
            else
            {
                throw new InvalidOperationException();
            }
#endif
        }

        static void InitMethodImpl(global::java.lang.invoke.MemberName self, MethodWrapper mw, bool wantSpecial)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            int flags = (int)mw.Modifiers;
            flags |= mw.IsConstructor ? global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_CONSTRUCTOR : global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_METHOD;
            if (mw.IsStatic)
            {
                flags |= global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeStatic << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
            }
            else if (mw.IsConstructor && !wantSpecial)
            {
                flags |= global::java.lang.invoke.MethodHandleNatives.Constants.REF_newInvokeSpecial << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
            }
            else if (mw.IsPrivate || mw.IsFinal || mw.IsConstructor || wantSpecial)
            {
                flags |= global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeSpecial << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
            }
            else if (mw.DeclaringType.IsInterface)
            {
                flags |= global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeInterface << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
            }
            else
            {
                flags |= global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
            }
            if (mw.HasCallerID || DynamicTypeWrapper.RequiresDynamicReflectionCallerClass(mw.DeclaringType.Name, mw.Name, mw.Signature))
            {
                flags |= global::java.lang.invoke.MemberName.CALLER_SENSITIVE;
            }
            if (mw.IsConstructor && mw.DeclaringType == CoreClasses.java.lang.String.Wrapper)
            {
                global::java.lang.Class[] parameters1 = new global::java.lang.Class[mw.GetParameters().Length];
                for (int i = 0; i < mw.GetParameters().Length; i++)
                {
                    parameters1[i] = mw.GetParameters()[i].ClassObject;
                }
                global::java.lang.invoke.MethodType mt = global::java.lang.invoke.MethodType.methodType(PrimitiveTypeWrapper.VOID.ClassObject, parameters1);
                self._type(mt);
                self._flags(flags);
                self._clazz(mw.DeclaringType.ClassObject);
                self.vmtarget = CreateMemberNameDelegate(mw, null, false, self.getMethodType().changeReturnType(CoreClasses.java.lang.String.Wrapper.ClassObject));
                return;
            }
            self._flags(flags);
            self._clazz(mw.DeclaringType.ClassObject);
            int firstParam = mw.IsStatic ? 0 : 1;
            global::java.lang.Class[] parameters = new global::java.lang.Class[mw.GetParameters().Length + firstParam];
            for (int i = 0; i < mw.GetParameters().Length; i++)
            {
                parameters[i + firstParam] = mw.GetParameters()[i].ClassObject;
            }
            if (!mw.IsStatic)
            {
                parameters[0] = mw.DeclaringType.ClassObject;
            }
            self.vmtarget = CreateMemberNameDelegate(mw, mw.ReturnType.ClassObject, !wantSpecial, global::java.lang.invoke.MethodType.methodType(mw.ReturnType.ClassObject, parameters));
#endif
        }

#if !FIRST_PASS

        static void SetModifiers(global::java.lang.invoke.MemberName self, MemberWrapper mw)
        {
            self._flags(self._flags() | (int)mw.Modifiers);
        }

#endif

        public static void expand(global::java.lang.invoke.MemberName self)
        {
            throw new NotImplementedException();
        }

        public static global::java.lang.invoke.MemberName resolve(global::java.lang.invoke.MemberName self, global::java.lang.Class caller)
        {
#if !FIRST_PASS
            switch (self.getReferenceKind())
            {
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeStatic:
                    if (self.getDeclaringClass() == CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject)
                    {
                        switch (self.getName())
                        {
                            case "linkToVirtual":
                            case "linkToStatic":
                            case "linkToSpecial":
                            case "linkToInterface":
                                // this delegate is never used normally, only by the PrivateInvokeTest white-box JSR-292 tests
                                self.vmtarget = MethodHandleUtil.DynamicMethodBuilder.CreateMethodHandleLinkTo(self);
                                self._flags(self._flags() | global::java.lang.reflect.Modifier.STATIC | global::java.lang.reflect.Modifier.NATIVE | global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_METHOD);
                                return self;
                        }
                    }
                    ResolveMethod(self, caller);
                    break;
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual:
                    if (self.getDeclaringClass() == CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject)
                    {
                        switch (self.getName())
                        {
                            case "invoke":
                            case "invokeExact":
                            case "invokeBasic":
                                self.vmtarget = MethodHandleUtil.DynamicMethodBuilder.CreateMethodHandleInvoke(self);
                                self._flags(self._flags() | global::java.lang.reflect.Modifier.NATIVE | global::java.lang.reflect.Modifier.FINAL | global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_METHOD);
                                return self;
                        }
                    }
                    ResolveMethod(self, caller);
                    break;
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeInterface:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeSpecial:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_newInvokeSpecial:
                    ResolveMethod(self, caller);
                    break;
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getField:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putField:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getStatic:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putStatic:
                    ResolveField(self);
                    break;
                default:
                    throw new InvalidOperationException();
            }
#endif
            return self;
        }

#if !FIRST_PASS
        static void ResolveMethod(global::java.lang.invoke.MemberName self, global::java.lang.Class caller)
        {
            bool invokeSpecial = self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeSpecial;
            bool newInvokeSpecial = self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_newInvokeSpecial;
            bool searchBaseClasses = !newInvokeSpecial;
            MethodWrapper mw = TypeWrapper.FromClass(self.getDeclaringClass()).GetMethodWrapper(self.getName(), self.getSignature().Replace('/', '.'), searchBaseClasses);
            if (mw == null)
            {
                if (self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeInterface)
                {
                    mw = TypeWrapper.FromClass(self.getDeclaringClass()).GetInterfaceMethod(self.getName(), self.getSignature().Replace('/', '.'));
                    if (mw == null)
                    {
                        mw = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(self.getName(), self.getSignature().Replace('/', '.'), false);
                    }
                    if (mw != null && mw.IsConstructor)
                    {
                        throw new global::java.lang.IncompatibleClassChangeError("Found interface " + self.getDeclaringClass().getName() + ", but class was expected");
                    }
                }
                if (mw == null)
                {
                    string msg = String.Format(invokeSpecial ? "{0}: method {1}{2} not found" : "{0}.{1}{2}", self.getDeclaringClass().getName(), self.getName(), self.getSignature());
                    throw new global::java.lang.NoSuchMethodError(msg);
                }
            }
            if (mw.IsStatic != IsReferenceKindStatic(self.getReferenceKind()))
            {
                string msg = String.Format(mw.IsStatic ? "Expecting non-static method {0}.{1}{2}" : "Expected static method {0}.{1}{2}", mw.DeclaringType.Name, self.getName(), self.getSignature());
                throw new global::java.lang.IncompatibleClassChangeError(msg);
            }
            if (self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual && mw.DeclaringType.IsInterface)
            {
                throw new global::java.lang.IncompatibleClassChangeError("Found interface " + mw.DeclaringType.Name + ", but class was expected");
            }
            if (!mw.IsPublic && self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeInterface)
            {
                throw new global::java.lang.IncompatibleClassChangeError("private interface method requires invokespecial, not invokeinterface: method " + self.getDeclaringClass().getName() + "." + self.getName() + self.getSignature());
            }
            if (mw.IsConstructor && mw.DeclaringType == CoreClasses.java.lang.String.Wrapper)
            {
                self.vmtarget = CreateMemberNameDelegate(mw, caller, false, self.getMethodType().changeReturnType(CoreClasses.java.lang.String.Wrapper.ClassObject));
            }
            else if (!mw.IsConstructor || invokeSpecial || newInvokeSpecial)
            {
                global::java.lang.invoke.MethodType methodType = self.getMethodType();
                if (!mw.IsStatic)
                {
                    methodType = methodType.insertParameterTypes(0, mw.DeclaringType.ClassObject);
                    if (newInvokeSpecial)
                    {
                        methodType = methodType.changeReturnType(global::java.lang.Void.TYPE);
                    }
                }
                self.vmtarget = CreateMemberNameDelegate(mw, caller, self.hasReceiverTypeDispatch(), methodType);
            }
            SetModifiers(self, mw);
            self._flags(self._flags() | (mw.IsConstructor ? global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_CONSTRUCTOR : global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_METHOD));
            if (self.getReferenceKind() == global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual && (mw.IsPrivate || mw.IsFinal || mw.IsConstructor))
            {
                int flags = self._flags();
                flags -= global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
                flags += global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeSpecial << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
                self._flags(flags);
            }
            if (mw.HasCallerID || DynamicTypeWrapper.RequiresDynamicReflectionCallerClass(mw.DeclaringType.Name, mw.Name, mw.Signature))
            {
                self._flags(self._flags() | global::java.lang.invoke.MemberName.CALLER_SENSITIVE);
            }
        }

        private static void ResolveField(global::java.lang.invoke.MemberName self)
        {
            FieldWrapper fw = TypeWrapper.FromClass(self.getDeclaringClass()).GetFieldWrapper(self.getName(), self.getSignature().Replace('/', '.'));
            if (fw == null)
            {
                throw new global::java.lang.NoSuchFieldError(self.getName());
            }
            SetModifiers(self, fw);
            self._flags(self._flags() | global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_FIELD);
            if (fw.IsStatic != IsReferenceKindStatic(self.getReferenceKind()))
            {
                int newReferenceKind;
                switch (self.getReferenceKind())
                {
                    case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getField:
                        newReferenceKind = global::java.lang.invoke.MethodHandleNatives.Constants.REF_getStatic;
                        break;
                    case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putField:
                        newReferenceKind = global::java.lang.invoke.MethodHandleNatives.Constants.REF_putStatic;
                        break;
                    case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getStatic:
                        newReferenceKind = global::java.lang.invoke.MethodHandleNatives.Constants.REF_getField;
                        break;
                    case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putStatic:
                        newReferenceKind = global::java.lang.invoke.MethodHandleNatives.Constants.REF_putField;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                int flags = self._flags();
                flags -= self.getReferenceKind() << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
                flags += newReferenceKind << global::java.lang.invoke.MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT;
                self._flags(flags);
            }
        }

        private static bool IsReferenceKindStatic(int referenceKind)
        {
            switch (referenceKind)
            {
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getField:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putField:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeVirtual:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeSpecial:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_newInvokeSpecial:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeInterface:
                    return false;
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_getStatic:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_putStatic:
                case global::java.lang.invoke.MethodHandleNatives.Constants.REF_invokeStatic:
                    return true;
            }
            throw new InvalidOperationException();
        }
#endif

        // TODO consider caching this delegate in MethodWrapper
        static Delegate CreateMemberNameDelegate(MethodWrapper mw, global::java.lang.Class caller, bool doDispatch, global::java.lang.invoke.MethodType type)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (mw.IsDynamicOnly)
            {
                return MethodHandleUtil.DynamicMethodBuilder.CreateDynamicOnly(mw, type);
            }
            // HACK this code is duplicated in compiler.cs
            if (mw.IsFinalizeOrClone)
            {
                TypeWrapper thisType = TypeWrapper.FromClass(caller);
                // HACK we may need to redirect finalize or clone from java.lang.Object/Throwable
                // to a more specific base type.
                if (thisType.IsAssignableTo(CoreClasses.cli.System.Object.Wrapper))
                {
                    mw = CoreClasses.cli.System.Object.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                }
                else if (thisType.IsAssignableTo(CoreClasses.cli.System.Exception.Wrapper))
                {
                    mw = CoreClasses.cli.System.Exception.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                }
                else if (thisType.IsAssignableTo(CoreClasses.java.lang.Throwable.Wrapper))
                {
                    mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                }
            }
            TypeWrapper tw = mw.DeclaringType;
            tw.Finish();
            mw.Link();
            mw.ResolveMethod();
            MethodInfo mi = mw.GetMethod() as MethodInfo;
            if (mi != null
                && !mw.HasCallerID
                && mw.IsStatic
                && MethodHandleUtil.HasOnlyBasicTypes(mw.GetParameters(), mw.ReturnType)
                && type.parameterCount() <= MethodHandleUtil.MaxArity)
            {
                return Delegate.CreateDelegate(MethodHandleUtil.CreateMemberWrapperDelegateType(mw.GetParameters(), mw.ReturnType), mi);
            }
            else
            {
                // slow path where we emit a DynamicMethod
                return MethodHandleUtil.DynamicMethodBuilder.CreateMemberName(mw, type, doDispatch);
            }
#endif
        }

        public static int getMembers(global::java.lang.Class defc, string matchName, string matchSig, int matchFlags, global::java.lang.Class caller, int skip, global::java.lang.invoke.MemberName[] results)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (matchName != null || matchSig != null || matchFlags != global::java.lang.invoke.MethodHandleNatives.Constants.MN_IS_METHOD)
                throw new NotImplementedException();

            var methods = TypeWrapper.FromClass(defc).GetMethods();
            for (int i = skip, len = Math.Min(results.Length, methods.Length - skip); i < len; i++)
                if (!methods[i].IsConstructor && !methods[i].IsClassInitializer)
                    results[i - skip] = new global::java.lang.invoke.MemberName((global::java.lang.reflect.Method)methods[i].ToMethodOrConstructor(true), false);

            return methods.Length - skip;
#endif
        }

        public static long objectFieldOffset(global::java.lang.invoke.MemberName self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fw = TypeWrapper.FromClass(self.getDeclaringClass()).GetFieldWrapper(self.getName(), self.getSignature().Replace('/', '.'));
            return (long)fw.Cookie;
#endif
        }

        public static long staticFieldOffset(global::java.lang.invoke.MemberName self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fw = TypeWrapper.FromClass(self.getDeclaringClass()).GetFieldWrapper(self.getName(), self.getSignature().Replace('/', '.'));
            return (long)fw.Cookie;
#endif
        }

        public static object staticFieldBase(global::java.lang.invoke.MemberName self)
        {
            return null;
        }

#if !FIRST_PASS
        internal static void InitializeCallSite(global::java.lang.invoke.CallSite site)
        {
            Type type = typeof(IKVM.Runtime.IndyCallSite<>).MakeGenericType(MethodHandleUtil.GetDelegateTypeForInvokeExact(site.type()));
            IKVM.Runtime.IIndyCallSite ics = (IKVM.Runtime.IIndyCallSite)Activator.CreateInstance(type, true);
            global::System.Threading.Interlocked.CompareExchange(ref site.ics, ics, null);
        }
#endif

        public static void setCallSiteTargetNormal(global::java.lang.invoke.CallSite site, global::java.lang.invoke.MethodHandle target)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (site.ics == null)
                InitializeCallSite(site);

            lock (site.ics)
            {
                site.target = target;
                site.ics.SetTarget(target);
            }
#endif
        }

        public static void setCallSiteTargetVolatile(global::java.lang.invoke.CallSite site, global::java.lang.invoke.MethodHandle target)
        {
            setCallSiteTargetNormal(site, target);
        }

        public static void registerNatives()
        {

        }

        public static object getMemberVMInfo(global::java.lang.invoke.MemberName self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (self.isField())
            {
                return new object[] { global::java.lang.Long.valueOf(0), self.getDeclaringClass() };
            }
            if (global::java.lang.invoke.MethodHandleNatives.refKindDoesDispatch(self.getReferenceKind()))
            {
                return new object[] { global::java.lang.Long.valueOf(0), self };
            }
            return new object[] { global::java.lang.Long.valueOf(-1), self };
#endif
        }

        public static int getConstant(int which)
        {
            return 0;
        }

        public static int getNamedCon(int which, object[] name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fields = typeof(global::java.lang.invoke.MethodHandleNatives.Constants).GetFields(BindingFlags.Static | BindingFlags.NonPublic);
            if (which >= fields.Length)
            {
                name[0] = null;
                return -1;
            }
            name[0] = fields[which].Name;
            return ((IConvertible)fields[which].GetRawConstantValue()).ToInt32(null);
#endif
        }

    }

}