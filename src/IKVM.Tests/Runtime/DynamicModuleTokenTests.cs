using System;
using System.Reflection;
using System.Reflection.Emit;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Pins down the dynamic module behaviour that <c>RuntimeJavaMethod.ResolveMethod</c> and
    /// <c>RuntimeJavaField.ResolveField</c> depend on.
    ///
    /// Both replace the builder left over from class emission with the member of the reified type by asking the module
    /// to resolve the token the builder reports. That is only correct if the token a builder hands out once its
    /// declaring type has been baked is the token of the emitted member, and if resolving it yields the runtime member
    /// rather than the builder that produced it. Neither is promised by the documentation, and an emitter that remapped
    /// tokens while baking would leave both methods dispatching to the wrong member instead of failing, so the
    /// assumption is asserted here rather than left implicit.
    /// </summary>
    [TestClass]
    public class DynamicModuleTokenTests
    {

        ModuleBuilder module;

        [TestInitialize]
        public void Setup()
        {
            var name = new AssemblyName("IKVM.Tests.DynamicModuleTokenTests." + Guid.NewGuid().ToString("N"));
#if NETFRAMEWORK
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
#else
            var assembly = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
#endif
            module = assembly.DefineDynamicModule(name.Name);
        }

        /// <summary>
        /// Gets the metadata token of the specified method the same way the runtime does.
        /// </summary>
        static int TokenOf(MethodBuilder builder)
        {
#if NETFRAMEWORK
            return builder.GetToken().Token;
#else
            return builder.MetadataToken;
#endif
        }

        /// <summary>
        /// Gets the metadata token of the specified field the same way the runtime does.
        /// </summary>
        static int TokenOf(FieldBuilder builder)
        {
#if NETFRAMEWORK
            return builder.GetToken().Token;
#else
            return builder.MetadataToken;
#endif
        }

        static MethodBuilder DefineMethodReturningInt(TypeBuilder type, string name, MethodAttributes attributes, int value)
        {
            var method = type.DefineMethod(name, attributes, typeof(int), Type.EmptyTypes);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldc_I4, value);
            il.Emit(OpCodes.Ret);
            return method;
        }

        [TestMethod]
        public void CanResolveMethodBuilderTokenToReifiedMethod()
        {
            var builder = module.DefineType("MethodHolder", TypeAttributes.Public);
            var instance = DefineMethodReturningInt(builder, "Instance", MethodAttributes.Public, 1);
            var shared = DefineMethodReturningInt(builder, "Shared", MethodAttributes.Public | MethodAttributes.Static, 2);
            var hidden = DefineMethodReturningInt(builder, "Hidden", MethodAttributes.Private, 3);
            var type = builder.CreateType();

            foreach (var method in new[] { instance, shared, hidden })
            {
                var resolved = module.ResolveMethod(TokenOf(method));
                resolved.Should().NotBeNull();
                resolved.Should().NotBeAssignableTo<MethodBuilder>();
                resolved.Name.Should().Be(method.Name);
                resolved.DeclaringType.Should().Be(type);
                resolved.MetadataToken.Should().Be(TokenOf(method));
            }
        }

        [TestMethod]
        public void CanResolveFieldBuilderTokenToReifiedField()
        {
            var builder = module.DefineType("FieldHolder", TypeAttributes.Public);
            var instance = builder.DefineField("Instance", typeof(int), FieldAttributes.Public);
            var shared = builder.DefineField("Shared", typeof(string), FieldAttributes.Public | FieldAttributes.Static);
            var hidden = builder.DefineField("Hidden", typeof(long), FieldAttributes.Private);
            var type = builder.CreateType();

            foreach (var field in new[] { instance, shared, hidden })
            {
                var resolved = module.ResolveField(TokenOf(field));
                resolved.Should().NotBeNull();
                resolved.Should().NotBeAssignableTo<FieldBuilder>();
                resolved.Name.Should().Be(field.Name);
                resolved.DeclaringType.Should().Be(type);
                resolved.MetadataToken.Should().Be(TokenOf(field));
            }
        }

        [TestMethod]
        public void CanResolveConstructorDefinedAsMethodBuilder()
        {
            var builder = module.DefineType("ConstructorHolder", TypeAttributes.Public);

            // constructors are emitted through DefineMethod rather than DefineConstructor, so the leftover builder is a
            // MethodBuilder whose token has to resolve to a ConstructorInfo
            var ctor = builder.DefineMethod(ConstructorInfo.ConstructorName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, null, Type.EmptyTypes);
            var il = ctor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ret);

            var value = DefineMethodReturningInt(builder, "Value", MethodAttributes.Public, 42);
            var type = builder.CreateType();

            var resolvedCtor = module.ResolveMethod(TokenOf(ctor));
            resolvedCtor.Should().BeAssignableTo<ConstructorInfo>();
            resolvedCtor.DeclaringType.Should().Be(type);

            // resolving is only worth anything if what comes back can actually be invoked, which a builder cannot
            var instance = ((ConstructorInfo)resolvedCtor).Invoke(null);
            ((MethodInfo)module.ResolveMethod(TokenOf(value))).Invoke(instance, null).Should().Be(42);
        }

        [TestMethod]
        public void CanResolveMembersThatShareANameAndParameterTypes()
        {
            var builder = module.DefineType("CollisionHolder", TypeAttributes.Public);

            // a covariant bridge method and the method it bridges to differ only in return type, and a class file may
            // declare two fields under one name with different descriptors; a signature scan cannot tell either apart
            var getObject = builder.DefineMethod("Get", MethodAttributes.Public, typeof(object), new[] { typeof(int) });
            var il = getObject.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);

            var getString = builder.DefineMethod("Get", MethodAttributes.Public, typeof(string), new[] { typeof(int) });
            il = getString.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);

            var intValue = builder.DefineField("value", typeof(int), FieldAttributes.Public);
            var stringValue = builder.DefineField("value", typeof(string), FieldAttributes.Public);
            builder.CreateType();

            ((MethodInfo)module.ResolveMethod(TokenOf(getObject))).ReturnType.Should().Be(typeof(object));
            ((MethodInfo)module.ResolveMethod(TokenOf(getString))).ReturnType.Should().Be(typeof(string));
            module.ResolveField(TokenOf(intValue)).FieldType.Should().Be(typeof(int));
            module.ResolveField(TokenOf(stringValue)).FieldType.Should().Be(typeof(string));
        }

    }

}
