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

sealed class MethodWrapper : MemberWrapper
{
	private MethodDescriptor md;
	private MethodBase originalMethod;
	private MethodBase redirMethod;
	private bool isRemappedVirtual;
	private bool isRemappedOverride;
	internal CodeEmitter EmitCall;
	internal CodeEmitter EmitCallvirt;
	internal CodeEmitter EmitNewobj;

	// TODO creation of MethodWrappers should be cleaned up (and every instance should support Invoke())
	internal static MethodWrapper Create(TypeWrapper declaringType, MethodDescriptor md, MethodBase originalMethod, MethodBase method, Modifiers modifiers, bool hideFromReflection)
	{
		if(method == null)
		{
			throw new InvalidOperationException();
		}
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
		if(!retType.IsUnloadable && retType.IsNonPrimitiveValueType)
		{
			wrapper.EmitCall += CodeEmitter.Create(OpCodes.Box, retType.Type);
			wrapper.EmitCallvirt += CodeEmitter.Create(OpCodes.Box, retType.Type);
		}
		if(declaringType.IsNonPrimitiveValueType)
		{
			if(method is ConstructorInfo)
			{
				wrapper.EmitNewobj += CodeEmitter.Create(OpCodes.Box, declaringType.Type);
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
		private TypeWrapper[] implementers;
		private CodeEmitter[] methods;

		internal GhostCallEmitter(TypeWrapper type, MethodDescriptor md, MethodBase method)
		{
			this.md = md;
			implementers = ClassLoaderWrapper.GetGhostImplementers(type);
			methods = new CodeEmitter[implementers.Length + 1];
			implementers[0] = type;
			methods[0] = CodeEmitter.Create(OpCodes.Callvirt, method);
		}

		internal override void Emit(ILGenerator ilgen)
		{
			Type[] args = md.ArgTypes;
			LocalBuilder[] argLocals = new LocalBuilder[args.Length];
			for(int i = args.Length - 1; i >= 0; i--)
			{
				argLocals[i] = ilgen.DeclareLocal(args[i]);
				ilgen.Emit(OpCodes.Stloc, argLocals[i]);
			}
			Label end = ilgen.DefineLabel();
			for(int i = 0; i < implementers.Length; i++)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Isinst, implementers[i].Type);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brfalse_S, label);
				ilgen.Emit(OpCodes.Castclass, implementers[i].Type);
				for(int j = 0; j < args.Length; j++)
				{
					ilgen.Emit(OpCodes.Ldloc, argLocals[j]);
				}
				if(methods[i] == null)
				{
					// NOTE this needs to be done lazily, because otherwise any the first implementer
					// of a ghost interface will trigger GetMethodWrapper calls to itself, before it is
					// constructed.
					methods[i] = implementers[i].GetMethodWrapper(md, true).EmitCallvirt;
				}
				methods[i].Emit(ilgen);
				ilgen.Emit(OpCodes.Br, end);
				ilgen.MarkLabel(label);
			}
			EmitHelper.Throw(ilgen, "java.lang.IncompatibleClassChangeError", "ghost interface not implemented");
			ilgen.MarkLabel(end);
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

	// IsRemappedOverride (only possible for remapped types) indicates whether the method in the
	// remapped type overrides (i.e. replaces) the method from the underlying type
	// Example: System.Object.ToString() is overriden by java.lang.Object.toString(),
	//          because java.lang.Object's toString() does something different from
	//          System.Object's ToString().
	internal bool IsRemappedOverride
	{
		get
		{
			return isRemappedOverride;
		}
		set
		{
			isRemappedOverride = value;
		}
	}

	// IsRemappedVirtual (only possible for remapped types) indicates that the method is virtual,
	// but doesn't really exist in the underlying type (there is no vtable slot to reuse)
	internal bool IsRemappedVirtual
	{
		get
		{
			return isRemappedVirtual;
		}
		set
		{
			isRemappedVirtual = value;
		}
	}

	// we expose the underlying MethodBase object,
	// for Java types, this is the method that contains the compiled Java bytecode
	// for remapped types, this is the method that underlies the remapped method
	internal MethodBase GetMethod()
	{
		return originalMethod;
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

	internal object Invoke(object obj, object[] args, bool nonVirtual)
	{
		// TODO instead of looking up the method using reflection, we should use the method object passed into the
		// constructor
		if(IsStatic)
		{
			MethodInfo method = this.originalMethod != null && !(this.originalMethod is MethodBuilder) ? (MethodInfo)this.originalMethod : DeclaringType.Type.GetMethod(md.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, md.ArgTypes, null);
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
				if(obj == null)
				{
					ConstructorInfo constructor = this.originalMethod != null && !(this.originalMethod is ConstructorBuilder) ? (ConstructorInfo)this.originalMethod : DeclaringType.Type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Standard, md.ArgTypes, null);
					try
					{
						return constructor.Invoke(args);
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
					throw new NotImplementedException("invoking constructor on existing instance");
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
					// if we calling a remapped virtual method, we need to locate the proper helper method
					if(IsRemappedVirtual)
					{
						Type[] argTypes = new Type[md.ArgTypes.Length + 1];
						argTypes[0] = this.DeclaringType.Type;
						md.ArgTypes.CopyTo(argTypes, 1);
						method = ((RemappedTypeWrapper)this.DeclaringType).VirtualsHelperHack.GetMethod(md.Name, BindingFlags.Static | BindingFlags.Public, null, argTypes, null);
					}
				}
				else if(IsRemappedVirtual)
				{
					throw new NotImplementedException("non-static remapped virtual invocation not implement");
				}
			}
			else
			{
				if(method is MethodBuilder || method == null)
				{
					method = DeclaringType.Type.GetMethod(md.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, md.ArgTypes, null);
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

sealed class FieldWrapper : MemberWrapper
{
	private string name;
	private string sig;
	internal readonly CodeEmitter EmitGet;
	internal readonly CodeEmitter EmitSet;
	private FieldInfo field;
	private TypeWrapper fieldType;

	private FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, CodeEmitter emitGet, CodeEmitter emitSet)
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
		this.EmitGet = emitGet;
		this.EmitSet = emitSet;
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

	internal Type FieldType
	{
		get
		{
			return FieldTypeWrapper.Type;
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
		private static MethodInfo getFieldFromHandle = typeof(FieldInfo).GetMethod("GetFieldFromHandle");
		private static MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter");
		private static MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit");
		private FieldInfo fi;

		internal VolatileLongDoubleGetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			if(!fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Dup);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue, label);
				ilgen.ThrowException(typeof(NullReferenceException));
				ilgen.MarkLabel(label);
			}
			// HACK we lock on the FieldInfo object
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorEnter);
			if(fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Ldsfld, fi);
			}
			else
			{
				ilgen.Emit(OpCodes.Ldfld, fi);
			}
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorExit);
		}
	}

	private class VolatileLongDoubleSetter : CodeEmitter
	{
		private static MethodInfo getFieldFromHandle = typeof(FieldInfo).GetMethod("GetFieldFromHandle");
		private static MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter");
		private static MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit");
		private FieldInfo fi;

		internal VolatileLongDoubleSetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			if(!fi.IsStatic)
			{
				LocalBuilder local = ilgen.DeclareLocal(fi.FieldType);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Dup);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue, label);
				ilgen.ThrowException(typeof(NullReferenceException));
				ilgen.MarkLabel(label);
				ilgen.Emit(OpCodes.Ldloc, local);
			}
			// HACK we lock on the FieldInfo object
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorEnter);
			if(fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Stsfld, fi);
			}
			else
			{
				ilgen.Emit(OpCodes.Stfld, fi);
			}
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorExit);
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

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, CodeEmitter getter, CodeEmitter setter)
	{
		return new FieldWrapper(declaringType, fieldType, name, sig, modifiers, getter, setter);
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string sig, Modifiers modifiers)
	{
		CodeEmitter emitGet = null;
		CodeEmitter emitSet = null;
		if(declaringType.IsNonPrimitiveValueType)
		{
			emitSet = new ValueTypeFieldSetter(declaringType.Type, fieldType.Type);
			emitGet = CodeEmitter.Create(OpCodes.Unbox, declaringType.Type);
		}
		if(fieldType.IsUnloadable)
		{
			emitGet += CodeEmitter.NoClassDefFoundError(fieldType.Name);
			emitSet += CodeEmitter.NoClassDefFoundError(fieldType.Name);
			return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, emitGet, emitSet);
		}
		if(fieldType.IsNonPrimitiveValueType)
		{
			emitSet += CodeEmitter.Create(OpCodes.Unbox, fieldType.Type);
			emitSet += CodeEmitter.Create(OpCodes.Ldobj, fieldType.Type);
		}
		if((modifiers & Modifiers.Volatile) != 0)
		{
			// long & double field accesses must be made atomic
			if(fi.FieldType == typeof(long) || fi.FieldType == typeof(double))
			{
				// TODO shouldn't we use += here (for volatile fields inside of value types)?
				emitGet = new VolatileLongDoubleGetter(fi);
				emitSet = new VolatileLongDoubleSetter(fi);
				return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, emitGet, emitSet);
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
			emitGet += CodeEmitter.Create(OpCodes.Box, fieldType.Type);
		}
		return new FieldWrapper(declaringType, fieldType, fi.Name, sig, modifiers, emitGet, emitSet);
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
		field = DeclaringType.Type.GetField(name, bindings);
	}

	internal void SetValue(object obj, object val)
	{
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null)
		{
			LookupField();
		}
		field.SetValue(obj, val);
	}

	internal object GetValue(object obj)
	{
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null)
		{
			LookupField();
		}
		return field.GetValue(obj);
	}
}
