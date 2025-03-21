/*
  Copyright (C) 2007-2011 Jeroen Frijters

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

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

using InstructionFlags = IKVM.Runtime.ClassFile.Method.InstructionFlags;

namespace IKVM.Runtime
{

    static class AtomicReferenceFieldUpdaterEmitter
    {

        internal static bool Emit(RuntimeByteCodeJavaType.FinishContext context, RuntimeJavaType wrapper, CodeEmitter ilgen, ClassFile classFile, int i, ClassFile.Method.Instruction[] code, InstructionFlags[] flags)
        {
            if (i >= 3
                && (flags[i - 0] & InstructionFlags.BranchTarget) == 0
                && (flags[i - 1] & InstructionFlags.BranchTarget) == 0
                && (flags[i - 2] & InstructionFlags.BranchTarget) == 0
                && (flags[i - 3] & InstructionFlags.BranchTarget) == 0
                && code[i - 1].NormalizedOpCode == NormalizedByteCode.__ldc_nothrow
                && code[i - 2].NormalizedOpCode == NormalizedByteCode.__ldc
                && code[i - 3].NormalizedOpCode == NormalizedByteCode.__ldc)
            {
                // we now have a structural match, now we need to make sure that the argument values are what we expect
                RuntimeJavaType tclass = classFile.GetConstantPoolClassType(code[i - 3].Arg1);
                RuntimeJavaType vclass = classFile.GetConstantPoolClassType(code[i - 2].Arg1);
                string fieldName = classFile.GetConstantPoolConstantString(code[i - 1].Arg1);
                if (tclass == wrapper && !vclass.IsUnloadable && !vclass.IsPrimitive && !vclass.IsNonPrimitiveValueType)
                {
                    RuntimeJavaField field = wrapper.GetFieldWrapper(fieldName, vclass.SigName);
                    if (field != null && !field.IsStatic && field.IsVolatile && field.DeclaringType == wrapper && field.FieldTypeWrapper == vclass)
                    {
                        // everything matches up, now call the actual emitter
                        ilgen.Emit(OpCodes.Pop);
                        ilgen.Emit(OpCodes.Pop);
                        ilgen.Emit(OpCodes.Pop);
                        ilgen.Emit(OpCodes.Newobj, context.GetAtomicReferenceFieldUpdater(field));
                        return true;
                    }
                }
            }
            return false;
        }

        internal static void EmitImpl(RuntimeContext context, TypeBuilder tb, FieldInfo field)
        {
            EmitCompareAndSet(context, "compareAndSet", tb, field);
            EmitGet(context, tb, field);
            EmitSet(context, "set", tb, field);
        }

        private static void EmitCompareAndSet(RuntimeContext context, string name, TypeBuilder tb, FieldInfo field)
        {
            MethodBuilder compareAndSet = tb.DefineMethod(name, MethodAttributes.Public | MethodAttributes.Virtual, context.Types.Boolean, new Type[] { context.Types.Object, context.Types.Object, context.Types.Object });
            ILGenerator ilgen = compareAndSet.GetILGenerator();
            ilgen.Emit(OpCodes.Ldarg_1);
            ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
            ilgen.Emit(OpCodes.Ldflda, field);
            ilgen.Emit(OpCodes.Ldarg_3);
            ilgen.Emit(OpCodes.Castclass, field.FieldType);
            ilgen.Emit(OpCodes.Ldarg_2);
            ilgen.Emit(OpCodes.Castclass, field.FieldType);
            ilgen.Emit(OpCodes.Call, MakeCompareExchange(context, field.FieldType));
            ilgen.Emit(OpCodes.Ldarg_2);
            ilgen.Emit(OpCodes.Ceq);
            ilgen.Emit(OpCodes.Ret);
        }

        internal static MethodInfo MakeCompareExchange(RuntimeContext context, Type type)
        {
            return context.InterlockedMethods.CompareExchangeOfT.MakeGenericMethod(type);
        }

        static void EmitGet(RuntimeContext context, TypeBuilder tb, FieldInfo field)
        {
            MethodBuilder get = tb.DefineMethod("get", MethodAttributes.Public | MethodAttributes.Virtual, context.Types.Object, new Type[] { context.Types.Object });
            ILGenerator ilgen = get.GetILGenerator();
            ilgen.Emit(OpCodes.Ldarg_1);
            ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
            ilgen.Emit(OpCodes.Volatile);
            ilgen.Emit(OpCodes.Ldfld, field);
            ilgen.Emit(OpCodes.Ret);
        }

        static void EmitSet(RuntimeContext context, string name, TypeBuilder tb, FieldInfo field)
        {
            MethodBuilder set = tb.DefineMethod(name, MethodAttributes.Public | MethodAttributes.Virtual, context.Types.Void, new Type[] { context.Types.Object, context.Types.Object });
            CodeEmitter ilgen = context.CodeEmitterFactory.Create(set);
            ilgen.Emit(OpCodes.Ldarg_1);
            ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
            ilgen.Emit(OpCodes.Ldarg_2);
            ilgen.Emit(OpCodes.Castclass, field.FieldType);
            ilgen.Emit(OpCodes.Volatile);
            ilgen.Emit(OpCodes.Stfld, field);
            ilgen.EmitMemoryBarrier();
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

    }

    class InterlockedMethods
    {

        readonly RuntimeContext context;

        internal readonly MethodInfo AddInt32;
        internal readonly MethodInfo CompareExchangeInt32;
        internal readonly MethodInfo CompareExchangeInt64;
        internal readonly MethodInfo CompareExchangeOfT;
        internal readonly MethodInfo ExchangeOfT;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public InterlockedMethods(RuntimeContext context)
        {
            this.context = context;

            var type = context.Resolver.ResolveCoreType(typeof(System.Threading.Interlocked).FullName).AsReflection();
            AddInt32 = type.GetMethod("Add", [context.Types.Int32.MakeByRefType(), context.Types.Int32]);
            CompareExchangeInt32 = type.GetMethod("CompareExchange", new Type[] { context.Types.Int32.MakeByRefType(), context.Types.Int32, context.Types.Int32 });
            CompareExchangeInt64 = type.GetMethod("CompareExchange", new Type[] { context.Types.Int64.MakeByRefType(), context.Types.Int64, context.Types.Int64 });
            foreach (MethodInfo m in type.GetMethods())
            {
                if (m.IsGenericMethodDefinition)
                {
                    switch (m.Name)
                    {
                        case "CompareExchange":
                            CompareExchangeOfT = m;
                            break;
                        case "Exchange":
                            ExchangeOfT = m;
                            break;
                    }
                }
            }

        }
    }

}
