/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Reflection.Emit;
using System.Diagnostics;
using OpenSystem.Java;

class MemberWrapper
{
	private System.Runtime.InteropServices.GCHandle handle;
	private TypeWrapper declaringType;
	private Modifiers modifiers;
	private bool hideFromReflection;

	protected MemberWrapper(TypeWrapper declaringType, Modifiers modifiers, bool hideFromReflection)
	{
		Debug.Assert(declaringType != null);
		this.declaringType = declaringType;
		this.modifiers = modifiers;
		this.hideFromReflection = hideFromReflection;
	}

	~MemberWrapper()
	{
		if(handle.IsAllocated)
		{
			handle.Free();
		}
	}

	internal IntPtr Cookie
	{
		get
		{
			lock(this)
			{
				if(!handle.IsAllocated)
				{
					handle = System.Runtime.InteropServices.GCHandle.Alloc(this, System.Runtime.InteropServices.GCHandleType.Weak);
				}
			}
			return (IntPtr)handle;
		}
	}

	internal static MemberWrapper FromCookieImpl(IntPtr cookie)
	{
		return (MemberWrapper)((System.Runtime.InteropServices.GCHandle)cookie).Target;
	}

	internal TypeWrapper DeclaringType
	{
		get
		{
			return declaringType;
		}
	}

	internal bool IsHideFromReflection
	{
		get
		{
			return hideFromReflection;
		}
	}

	internal Modifiers Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	internal bool IsStatic
	{
		get
		{
			return (modifiers & Modifiers.Static) != 0;
		}
	}

	internal bool IsPublic
	{
		get
		{
			return (modifiers & Modifiers.Public) != 0;
		}
	}

	internal bool IsPrivate
	{
		get
		{
			return (modifiers & Modifiers.Private) != 0;
		}
	}

	internal bool IsProtected
	{
		get
		{
			return (modifiers & Modifiers.Protected) != 0;
		}
	}

	internal bool IsFinal
	{
		get
		{
			return (modifiers & Modifiers.Final) != 0;
		}
	}
}

class MethodWrapper : MemberWrapper
{
	private MethodDescriptor md;
	private MethodBase originalMethod;
	private MethodBase redirMethod;
	private string[] declaredExceptions;
	internal CodeEmitter EmitCall;
	internal CodeEmitter EmitCallvirt;
	internal CodeEmitter EmitNewobj;

	private class GhostUnwrapper : CodeEmitter
	{
		private TypeWrapper type;

		internal GhostUnwrapper(TypeWrapper type)
		{
			this.type = type;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			LocalBuilder local = ilgen.DeclareLocal(type.TypeAsParameterType);
			ilgen.Emit(OpCodes.Stloc, local);
			ilgen.Emit(OpCodes.Ldloca, local);
			ilgen.Emit(OpCodes.Ldfld, type.GhostRefField);
		}
	}

	// TODO creation of MethodWrappers should be cleaned up (and every instance should support Invoke())
	internal static MethodWrapper Create(TypeWrapper declaringType, MethodDescriptor md, MethodBase originalMethod, MethodBase method, Modifiers modifiers, bool hideFromReflection)
	{
		Debug.Assert(method != null);

		MethodWrapper wrapper = new MethodWrapper(declaringType, md, originalMethod, method, modifiers, hideFromReflection);
		if(declaringType.IsGhost)
		{
			wrapper.EmitCallvirt = new GhostCallEmitter(declaringType, md, originalMethod);
		}
		else
		{
			CreateEmitters(originalMethod, method, ref wrapper.EmitCall, ref wrapper.EmitCallvirt, ref wrapper.EmitNewobj);
		}
		TypeWrapper retType = md.RetTypeWrapper;
		if(!retType.IsUnloadable)
		{
			if(retType.IsNonPrimitiveValueType)
			{
				wrapper.EmitCall += CodeEmitter.CreateEmitBoxCall(retType);
				wrapper.EmitCallvirt += CodeEmitter.CreateEmitBoxCall(retType);
			}
			else if(retType.IsGhost)
			{
				wrapper.EmitCall += new GhostUnwrapper(retType);
				wrapper.EmitCallvirt += new GhostUnwrapper(retType);
			}
		}
		if(declaringType.IsNonPrimitiveValueType)
		{
			if(method is ConstructorInfo)
			{
				// HACK after constructing a new object, we don't want the custom boxing rule to run
				// (because that would turn "new IntPtr" into a null reference)
				wrapper.EmitNewobj += CodeEmitter.Create(OpCodes.Box, declaringType.TypeAsTBD);
			}
			else
			{
				// callvirt isn't allowed on a value type
				wrapper.EmitCallvirt = wrapper.EmitCall;
			}
		}
		return wrapper;
	}

	internal class GhostCallEmitter : CodeEmitter
	{
		private MethodDescriptor md;
		private TypeWrapper type;
		private MethodInfo method;

		internal GhostCallEmitter(TypeWrapper type, MethodDescriptor md, MethodBase __method)
		{
			this.type = type;
			this.md = md;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			if(method == null)
			{
				method = type.TypeAsParameterType.GetMethod(md.Name, md.ArgTypesForDefineMethod);
			}
			ilgen.Emit(OpCodes.Call, method);
		}
	}

	internal static void CreateEmitters(MethodBase originalMethod, MethodBase method, ref CodeEmitter call, ref CodeEmitter callvirt, ref CodeEmitter newobj)
	{
		if(method is ConstructorInfo)
		{
			call = CodeEmitter.Create(OpCodes.Call, (ConstructorInfo)method);
			callvirt = null;
			newobj = CodeEmitter.Create(OpCodes.Newobj, (ConstructorInfo)method);
		}
		else
		{
			call = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
			if(originalMethod != null && originalMethod != method)
			{
				// if we're calling a virtual method that is redirected, that overrides an already
				// existing method, we have to call it virtually, instead of redirecting
				callvirt = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)originalMethod);
			}
			else if(method.IsStatic)
			{
				// because of redirection, it can be legal to call a static method with invokevirtual
				callvirt = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
			}
			else
			{
				callvirt = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)method);
			}
			newobj = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
		}
	}

	internal MethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodBase originalMethod, MethodBase method, Modifiers modifiers, bool hideFromReflection)
		: base(declaringType, modifiers, hideFromReflection)
	{
		Profiler.Count("MethodWrapper");
		if(method != originalMethod)
		{
			redirMethod = method;
			Debug.Assert(!(method is MethodBuilder));
		}
		this.md = md;
		// NOTE originalMethod may be null
		this.originalMethod = originalMethod;
	}

	internal void SetDeclaredExceptions(string[] exceptions)
	{
		if(exceptions == null)
		{
			exceptions = new string[0];
		}
		this.declaredExceptions = (string[])exceptions.Clone();
	}

	internal void SetDeclaredExceptions(MapXml.Throws[] throws)
	{
		if(throws != null)
		{
			declaredExceptions = new string[throws.Length];
			for(int i = 0; i < throws.Length; i++)
			{
				declaredExceptions[i] = throws[i].Class;
			}
		}
	}

	internal static MethodWrapper FromCookie(IntPtr cookie)
	{
		return (MethodWrapper)FromCookieImpl(cookie);
	}

	internal MethodDescriptor Descriptor
	{
		get
		{
			return md;
		}
	}

	internal string Name
	{
		get
		{
			return md.Name;
		}
	}

	internal bool HasUnloadableArgsOrRet
	{
		get
		{
			if(ReturnType.IsUnloadable)
			{
				return true;
			}
			foreach(TypeWrapper tw in GetParameters())
			{
				if(tw.IsUnloadable)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal TypeWrapper ReturnType
	{
		get
		{
			return md.RetTypeWrapper;
		}
	}

	internal TypeWrapper[] GetParameters()
	{
		return md.ArgTypeWrappers;
	}

	internal string[] GetExceptions()
	{
		// remapped types and dynamically compiled types have declaredExceptions set
		if(declaredExceptions != null)
		{
			return (string[])declaredExceptions.Clone();
		}
		// NOTE if originalMethod is a MethodBuilder, GetCustomAttributes doesn't work
		if(originalMethod != null && !(originalMethod is MethodBuilder))
		{
			object[] attributes = originalMethod.GetCustomAttributes(typeof(ThrowsAttribute), false);
			string[] exceptions = new string[attributes.Length];
			for(int i = 0; i < exceptions.Length; i++)
			{
				exceptions[i] = ((ThrowsAttribute)attributes[i]).ClassName;
			}
			return exceptions;
		}
		return new string[0];
	}

	// we expose the underlying MethodBase object,
	// for Java types, this is the method that contains the compiled Java bytecode
	// for remapped types, this is the method that underlies the remapped method
	internal MethodBase GetMethod()
	{
		return originalMethod;
	}

	internal string RealName
	{
		get
		{
			return originalMethod.Name;
		}
	}

	// this returns the Java method's attributes in .NET terms (e.g. used to create stubs for this method)
	internal MethodAttributes GetMethodAttributes()
	{
		MethodAttributes attribs = 0;
		if(IsStatic)
		{
			attribs |= MethodAttributes.Static;
		}
		if(IsPublic)
		{
			attribs |= MethodAttributes.Public;
		}
		else if(IsPrivate)
		{
			attribs |= MethodAttributes.Private;
		}
		else if(IsProtected)
		{
			attribs |= MethodAttributes.FamORAssem;
		}
		else
		{
			attribs |= MethodAttributes.Family;
		}
		// constructors aren't virtual
		if(!IsStatic && !IsPrivate && md.Name != "<init>")
		{
			attribs |= MethodAttributes.Virtual;
		}
		if(IsFinal)
		{
			attribs |= MethodAttributes.Final;
		}
		if(IsAbstract)
		{
			attribs |= MethodAttributes.Abstract;
		}
		return attribs;
	}

	internal bool IsAbstract
	{
		get
		{
			return (Modifiers & Modifiers.Abstract) != 0;
		}
	}

	internal virtual object Invoke(object obj, object[] args, bool nonVirtual)
	{
		// TODO if any of the parameters is a ghost, convert the passed in reference to a ghost value type
		// TODO instead of looking up the method using reflection, we should use the method object passed into the
		// constructor
		if(IsStatic)
		{
			MethodInfo method = this.originalMethod != null && !(this.originalMethod is MethodBuilder) ? (MethodInfo)this.originalMethod : DeclaringType.TypeAsTBD.GetMethod(md.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, md.ArgTypesDontUse, null);
			try
			{
				return method.Invoke(null, args);
			}
			catch(ArgumentException x1)
			{
				throw JavaException.IllegalArgumentException(x1.Message);
			}
			catch(TargetInvocationException x)
			{
				throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
			}
		}
		else
		{
			// calling <init> without an instance means that we're constructing a new instance
			// NOTE this means that we cannot detect a NullPointerException when calling <init>
			if(md.Name == "<init>")
			{
				ConstructorInfo constructor = this.originalMethod != null && !(this.originalMethod is ConstructorBuilder) ? (ConstructorInfo)this.originalMethod : DeclaringType.TypeAsTBD.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Standard, md.ArgTypesDontUse, null);
				try
				{
					if(obj != null)
					{
						return constructor.Invoke(obj, args);
					}
					else
					{
						return constructor.Invoke(args);
					}
				}
				catch(ArgumentException x1)
				{
					throw JavaException.IllegalArgumentException(x1.Message);
				}
				catch(TargetInvocationException x)
				{
					throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
				}
			}
			if(nonVirtual)
			{
				throw new NotImplementedException("non-virtual reflective method invocation not implemented");
			}
			MethodInfo method = (MethodInfo)this.originalMethod;
			if(redirMethod != null)
			{
				method = (MethodInfo)redirMethod;
				if(method.IsStatic)
				{
					// we've been redirected to a static method, so we have to copy the this into the args
					object[] oldargs = args;
					args = new object[args.Length + 1];
					args[0] = obj;
					oldargs.CopyTo(args, 1);
					obj = null;
				}
			}
			else
			{
				if(method is MethodBuilder || method == null)
				{
					method = DeclaringType.TypeAsTBD.GetMethod(md.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, md.ArgTypesDontUse, null);
				}
				if(method == null)
				{
					throw new NotImplementedException("method not found: " + this.DeclaringType.Name + "." + md.Name + md.Signature);
				}
			}
			try
			{
				return method.Invoke(obj, args);
			}
			catch(ArgumentException x1)
			{
				throw JavaException.IllegalArgumentException(x1.Message);
			}
			catch(TargetInvocationException x)
			{
				throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
			}
		}
	}
}

// This class tests if reflection on a constant field triggers the class constructor to run
// (it shouldn't run, but on .NET 1.0 & 1.1 it does)
class ReflectionOnConstant
{
	private static bool isBroken;
	private static System.Collections.Hashtable warnOnce;

	static ReflectionOnConstant()
	{
		typeof(Helper).GetField("foo", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
	}

	internal static bool IsBroken
	{
		get
		{
			return isBroken;
		}
	}

	internal static void IssueWarning(FieldInfo field)
	{
		// FXBUG .NET (1.0 & 1.1)
		// FieldInfo.GetValue() on a literal causes the type initializer to run and
		// we don't want that.
		// TODO may need to find a workaround, for now we just spit out a warning
		if(ReflectionOnConstant.IsBroken && field.DeclaringType.TypeInitializer != null)
		{
			if(Tracer.FxBug.TraceWarning)
			{
				if(warnOnce == null)
				{
					warnOnce = new System.Collections.Hashtable();
				}
				if(!warnOnce.ContainsKey(field.DeclaringType.FullName))
				{
					warnOnce.Add(field.DeclaringType.FullName, null);
					Tracer.Warning(Tracer.FxBug, "Running type initializer for {0} due to CLR bug", field.DeclaringType.FullName);
				}
			}
		}
	}

	private class Helper
	{
		internal const int foo = 1;

		static Helper()
		{
			isBroken = true;
		}
	}
}

class FieldWrapper : MemberWrapper
{
	private string name;
	private string sig;
	internal readonly CodeEmitter EmitGet;
	internal readonly CodeEmitter EmitSet;
	private FieldInfo field;
	private TypeWrapper fieldType;

	private FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, CodeEmitter emitGet, CodeEmitter emitSet)
		: base(declaringType, modifiers, false)
	{
		Debug.Assert(fieldType != null);
		Debug.Assert(name != null);
		Debug.Assert(sig != null);
		Debug.Assert(emitGet != null);
		Debug.Assert(emitSet != null);
		this.name = name;
		this.sig = sig;
		this.fieldType = fieldType;
		this.field = field;
		this.EmitGet = emitGet;
		this.EmitSet = emitSet;
	}

	// HACK used (indirectly thru NativeCode.java.lang.Field.getConstant) by netexp to find out if the
	// field is a constant (and if it is, to get its value)
	internal object GetConstant()
	{
		// NOTE only pritimives and string can be literals in Java (because the other "primitives" (like uint),
		// are treated as NonPrimitiveValueTypes)
		if(field != null && (fieldType.IsPrimitive || fieldType == CoreClasses.java.lang.String.Wrapper) && field.IsLiteral)
		{
			ReflectionOnConstant.IssueWarning(field);
			object val = field.GetValue(null);
			if(val != null && !(val is string))
			{
				return NativeCode.java.lang.reflect.JavaWrapper.Box(val);
			}
			return val;
		}
		return null;
	}

	internal static FieldWrapper FromCookie(IntPtr cookie)
	{
		return (FieldWrapper)FromCookieImpl(cookie);
	}

	internal string Name
	{
		get
		{
			return name;
		}
	}

	internal TypeWrapper FieldTypeWrapper
	{
		get
		{
			return fieldType;
		}
	}

	internal bool IsVolatile
	{
		get
		{
			return (Modifiers & Modifiers.Volatile) != 0;
		}
	}

	private class VolatileLongDoubleGetter : CodeEmitter
	{
		private static MethodInfo volatileReadDouble = typeof(System.Threading.Thread).GetMethod("VolatileRead", new Type[] { Type.GetType("System.Double&") });
		private static MethodInfo volatileReadLong = typeof(System.Threading.Thread).GetMethod("VolatileRead", new Type[] { Type.GetType("System.Int64&") });
		private FieldInfo fi;

		internal VolatileLongDoubleGetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(fi.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, fi);
			if(fi.FieldType == typeof(double))
			{
				ilgen.Emit(OpCodes.Call, volatileReadDouble);
			}
			else
			{
				Debug.Assert(fi.FieldType == typeof(long));
				ilgen.Emit(OpCodes.Call, volatileReadLong);
			}
		}
	}

	private class VolatileLongDoubleSetter : CodeEmitter
	{
		private static MethodInfo volatileWriteDouble = typeof(System.Threading.Thread).GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Double&"), typeof(double) });
		private static MethodInfo volatileWriteLong = typeof(System.Threading.Thread).GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Int64&"), typeof(long) });
		private FieldInfo fi;

		internal VolatileLongDoubleSetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			LocalBuilder temp = ilgen.DeclareLocal(fi.FieldType);
			ilgen.Emit(OpCodes.Stloc, temp);
			ilgen.Emit(fi.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, fi);
			ilgen.Emit(OpCodes.Ldloc, temp);
			if(fi.FieldType == typeof(double))
			{
				ilgen.Emit(OpCodes.Call, volatileWriteDouble);
			}
			else
			{
				Debug.Assert(fi.FieldType == typeof(long));
				ilgen.Emit(OpCodes.Call, volatileWriteLong);
			}
		}
	}

	private class ValueTypeFieldSetter : CodeEmitter
	{
		private Type declaringType;
		private Type fieldType;

		internal ValueTypeFieldSetter(Type declaringType, Type fieldType)
		{
			this.declaringType = declaringType;
			this.fieldType = fieldType;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			LocalBuilder local = ilgen.DeclareLocal(fieldType);
			ilgen.Emit(OpCodes.Stloc, local);
			ilgen.Emit(OpCodes.Unbox, declaringType);
			ilgen.Emit(OpCodes.Ldloc, local);
		}
	}

	private class GhostFieldSetter : CodeEmitter
	{
		private OpCode ldflda;
		private FieldInfo field;
		private TypeWrapper type;

		internal GhostFieldSetter(FieldInfo field, TypeWrapper type, OpCode ldflda)
		{
			this.field = field;
			this.type = type;
			this.ldflda = ldflda;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			LocalBuilder local = ilgen.DeclareLocal(type.TypeAsLocalOrStackType);
			ilgen.Emit(OpCodes.Stloc, local);
			ilgen.Emit(ldflda, field);
			ilgen.Emit(OpCodes.Ldloc, local);
			ilgen.Emit(OpCodes.Stfld, type.GhostRefField);
		}
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo fi, CodeEmitter getter, CodeEmitter setter)
	{
		return new FieldWrapper(declaringType, fieldType, name, sig, modifiers, fi, getter, setter);
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo fi, CodeEmitter getter, CodeEmitter setter, object constant)
	{
		if(constant != null)
		{
			return new ConstantFieldWrapper(declaringType, fieldType, name, sig, modifiers, fi, getter, setter, constant);
		}
		else
		{
			return new FieldWrapper(declaringType, fieldType, name, sig, modifiers, fi, getter, setter);
		}
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string sig, Modifiers modifiers)
	{
		CodeEmitter emitGet = null;
		CodeEmitter emitSet = null;
		if(declaringType.IsNonPrimitiveValueType)
		{
			// NOTE all that ValueTypeFieldSetter does, is unbox the boxed value type that contains the field that we are setting
			emitSet = new ValueTypeFieldSetter(declaringType.TypeAsTBD, fieldType.TypeAsTBD);
			emitGet = CodeEmitter.Create(OpCodes.Unbox, declaringType.TypeAsTBD);
		}
		if(fieldType.IsUnloadable)
		{
			// TODO we might need to emit code to check the type dynamically
			// TODO the fact that the type is unloadable now, doesn't mean it will be unloadable when a method
			// that accesses this field is compiled, that means that that method (may) need to emit a cast
			if((modifiers & Modifiers.Static) != 0)
			{
				emitGet += CodeEmitter.Create(OpCodes.Ldsfld, fi);
				emitSet += CodeEmitter.Create(OpCodes.Stsfld, fi);
			}
			else
			{
				emitGet += CodeEmitter.Create(OpCodes.Ldfld, fi);
				emitSet += CodeEmitter.Create(OpCodes.Stfld, fi);
			}
			return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, fi, emitGet, emitSet);
		}
		if(fieldType.IsGhost)
		{
			if((modifiers & Modifiers.Static) != 0)
			{
				emitGet += CodeEmitter.Create(OpCodes.Ldsflda, fi) + CodeEmitter.Create(OpCodes.Ldfld, fieldType.GhostRefField);
				emitSet += new GhostFieldSetter(fi, fieldType, OpCodes.Ldsflda);
			}
			else
			{
				emitGet += CodeEmitter.Create(OpCodes.Ldflda, fi) + CodeEmitter.Create(OpCodes.Ldfld, fieldType.GhostRefField);
				emitSet += new GhostFieldSetter(fi, fieldType, OpCodes.Ldflda);
			}
			return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, fi, emitGet, emitSet);
		}
		if(fieldType.IsNonPrimitiveValueType)
		{
			emitSet += CodeEmitter.CreateEmitUnboxCall(fieldType);
		}
		if((modifiers & Modifiers.Volatile) != 0)
		{
			// long & double field accesses must be made atomic
			if(fi.FieldType == typeof(long) || fi.FieldType == typeof(double))
			{
				// TODO shouldn't we use += here (for volatile fields inside of value types)?
				emitGet = new VolatileLongDoubleGetter(fi);
				emitSet = new VolatileLongDoubleSetter(fi);
				return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, fi, emitGet, emitSet);
			}
			emitGet += CodeEmitter.Volatile;
			emitSet += CodeEmitter.Volatile;
		}
		if((modifiers & Modifiers.Static) != 0)
		{
			emitGet += CodeEmitter.Create(OpCodes.Ldsfld, fi);
			emitSet += CodeEmitter.Create(OpCodes.Stsfld, fi);
		}
		else
		{
			emitGet += CodeEmitter.Create(OpCodes.Ldfld, fi);
			emitSet += CodeEmitter.Create(OpCodes.Stfld, fi);
		}
		if(fieldType.IsNonPrimitiveValueType)
		{
			emitGet += CodeEmitter.CreateEmitBoxCall(fieldType);
		}
		return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, fi, emitGet, emitSet);
	}

	private void LookupField()
	{
		BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic;
		if(IsStatic)
		{
			bindings |= BindingFlags.Static;
		}
		else
		{
			bindings |= BindingFlags.Instance;
		}
		field = DeclaringType.TypeAsTBD.GetField(name, bindings);
		Debug.Assert(field != null);
	}

	internal void SetValue(object obj, object val)
	{
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null || field is FieldBuilder)
		{
			LookupField();
		}
		if(fieldType.IsGhost)
		{
			object temp = field.GetValue(obj);
			fieldType.GhostRefField.SetValue(temp, val);
			val = temp;
		}
		try
		{
			field.SetValue(obj, val);
		}
		catch(FieldAccessException x)
		{
			throw JavaException.IllegalAccessException(x.Message);
		}
	}

	internal virtual object GetValue(object obj)
	{
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null || field is FieldBuilder)
		{
			LookupField();
		}
		if(field.IsLiteral)
		{
			// on a non-broken CLR GetValue on a literal will not trigger type initialization, but on Java it should
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(field.DeclaringType.TypeHandle);
		}
		object val = field.GetValue(obj);
		if(fieldType.IsGhost)
		{
			val = fieldType.GhostRefField.GetValue(val);
		}
		return val;
	}

	// NOTE this type is only used for remapped fields, dynamically compiled classes are always finished before we
	// allow reflection (so we can look at the underlying field in that case)
	private class ConstantFieldWrapper : FieldWrapper
	{
		private object constant;

		internal ConstantFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, CodeEmitter emitGet, CodeEmitter emitSet, object constant)
			: base(declaringType, fieldType, name, sig, modifiers, field, emitGet, emitSet)
		{
			this.constant = constant;
		}

		internal override object GetValue(object obj)
		{
			return constant;
		}
	}
}
