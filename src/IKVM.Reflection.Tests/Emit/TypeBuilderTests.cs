using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    /// <summary>
    /// Tests for <see cref="TypeBuilder"/>.
    /// </summary>
    public class TypeBuilderTests
    {

        /// <summary>
        /// Checks that a type with a field, a property with its accessors and a method is written to the saved
        /// assembly.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineFieldMethodsAndProperty(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var objectType = u.Import(typeof(object));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type");

            var valueField = type.DefineField("value", objectType, FieldAttributes.Private);

            var getValueMethod = type.DefineMethod("get_Value", MethodAttributes.Public, objectType, Type.EmptyTypes);
            var getValueIL = getValueMethod.GetILGenerator();
            getValueIL.Emit(OpCodes.Ldarg_0);
            getValueIL.Emit(OpCodes.Ldfld, valueField);
            getValueIL.Emit(OpCodes.Ret);

            var setValueMethod = type.DefineMethod("set_Value", MethodAttributes.Public, u.Import(typeof(void)), [objectType]);
            var setValueIL = setValueMethod.GetILGenerator();
            setValueIL.Emit(OpCodes.Ldarg_0);
            setValueIL.Emit(OpCodes.Ldarg_1);
            setValueIL.Emit(OpCodes.Stfld, valueField);
            setValueIL.Emit(OpCodes.Ret);

            var valueProperty = type.DefineProperty("Value", PropertyAttributes.None, objectType, Type.EmptyTypes);
            valueProperty.SetGetMethod(getValueMethod);
            valueProperty.SetSetMethod(setValueMethod);

            var execMethod = type.DefineMethod("Exec", MethodAttributes.Public, objectType, [u.Import(typeof(string[]))]);
            execMethod.DefineParameter(1, ParameterAttributes.None, "args");
            var execMethodIL = execMethod.GetILGenerator();
            execMethodIL.Emit(OpCodes.Ldnull);
            execMethodIL.Emit(OpCodes.Ret);

            type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            a.GetName().Name.Should().Be("Test");
            a.GetModule("Test").Should().NotBeNull();
            var t = a.GetType("Type");
            t.Should().NotBeNull();
            t.Should().NotBeStatic();
            t.Should().HaveMethod("Exec", [u.LoadContextType("System.String").MakeArrayType()]).Which.Should().Return(u.LoadContextType("System.Object"));
            t.Should().HaveProperty(u.LoadContextType("System.Object"), "Value");
        }

        /// <summary>
        /// Checks that an interface and a class that implements it with an explicit method override are written
        /// to the saved assembly, with the override's body and declaration the right way round.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineInterfaceImplementation(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var objectType = u.Import(typeof(object));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var ifaceType = module.DefineType("Iface", TypeAttributes.Interface);
            var ifaceMethod = ifaceType.DefineMethod("Method", InterfaceMethod, objectType, Type.EmptyTypes);

            var implType = module.DefineType("Impl", TypeAttributes.Public, null, [ifaceType]);
            var implMethod = implType.DefineMethod("Method", ImplementationMethod, objectType, Type.EmptyTypes);
            implType.DefineMethodOverride(implMethod, ifaceMethod);

            var il = implMethod.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);

            ifaceType.CreateType();
            implType.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            var i = a.GetType("Iface")!;
            i.IsInterface.Should().BeTrue();
            i.Should().HaveMethod("Method", []);
            var t = a.GetType("Impl")!;
            t.IsInterface.Should().BeFalse();
            t.IsClass.Should().BeTrue();
            t.GetInterfaces().Should().Contain(i);
            t.GetMethod("Method").Should().NotBeNull().And.Return(u.LoadContextType("System.Object"));

            ReadMethodImpls(u, "Test.dll").Should().Equal(("Impl", "Impl.Method", "Iface.Method"));
        }

        /// <summary>
        /// Checks that custom attributes set on builders can be read back from the created type, before and after saving.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanReadBackCustomAttributes(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var obsolete = u.Import(typeof(System.ObsoleteAttribute)).GetConstructor([u.Import(typeof(string))]);
            // builds an ObsoleteAttribute that carries the given message
            CustomAttributeBuilder Attribute(string message) => new(obsolete, [message]);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Type", TypeAttributes.Public);
            type.SetCustomAttribute(Attribute("type"));
            var field = type.DefineField("field", u.Import(typeof(int)), FieldAttributes.Public);
            field.SetCustomAttribute(Attribute("field"));
            var method = type.DefineMethod("Method", MethodAttributes.Public, u.Import(typeof(void)), [u.Import(typeof(int))]);
            method.SetCustomAttribute(Attribute("method"));
            method.DefineParameter(1, ParameterAttributes.None, "value").SetCustomAttribute(Attribute("parameter"));
            method.GetILGenerator().Emit(OpCodes.Ret);
            var created = type.CreateType();

            // checks what the created type reports
            void Check()
            {
                static string Message(System.Collections.Generic.IList<CustomAttributeData> data) => (string)data.Single(i => i.AttributeType.Name == "ObsoleteAttribute").ConstructorArguments[0].Value!;
                Message(created.GetCustomAttributesData()).Should().Be("type");
                Message(created.GetField("field")!.GetCustomAttributesData()).Should().Be("field");
                var m = created.GetMethod("Method")!;
                Message(m.GetCustomAttributesData()).Should().Be("method");
                Message(m.GetParameters()[0].GetCustomAttributesData()).Should().Be("parameter");
            }

            Check();
            assembly.Save("Test.dll");
            Check();
        }

        /// <summary>
        /// Checks that the saved assembly verifies and reads back when overrides are defined out of type order. The
        /// InterfaceImpl and MethodImpl tables must be sorted by class, and the overrides are what land in MethodImpl.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineMultipleInterfaceImplementationsOutOfOrder(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var objectType = u.Import(typeof(object));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var iface1Type = module.DefineType("Iface1", TypeAttributes.Interface);
            var iface1Method = iface1Type.DefineMethod("Method1", InterfaceMethod, objectType, Type.EmptyTypes);

            var iface2Type = module.DefineType("Iface2", TypeAttributes.Interface);
            var iface2Method = iface2Type.DefineMethod("Method2", InterfaceMethod, objectType, Type.EmptyTypes);

            var impl1Type = module.DefineType("Impl1", TypeAttributes.Public, null, [iface1Type]);
            var impl1Method = impl1Type.DefineMethod("Method1", ImplementationMethod, objectType, Type.EmptyTypes);
            var il1 = impl1Method.GetILGenerator();
            il1.Emit(OpCodes.Ldnull);
            il1.Emit(OpCodes.Ret);

            var impl2Type = module.DefineType("Impl2", TypeAttributes.Public, null, [iface2Type]);
            var impl2Method = impl2Type.DefineMethod("Method2", ImplementationMethod, objectType, Type.EmptyTypes);
            var il2 = impl2Method.GetILGenerator();
            il2.Emit(OpCodes.Ldnull);
            il2.Emit(OpCodes.Ret);

            impl2Type.DefineMethodOverride(impl2Method, iface2Method);
            impl1Type.DefineMethodOverride(impl1Method, iface1Method);

            iface1Type.CreateType();
            iface2Type.CreateType();
            impl1Type.CreateType();
            impl2Type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            var i1 = a.GetType("Iface1")!;
            var i2 = a.GetType("Iface2")!;
            var t1 = a.GetType("Impl1")!;
            var t2 = a.GetType("Impl2")!;
            i1.IsInterface.Should().BeTrue();
            i2.IsInterface.Should().BeTrue();
            t1.GetInterfaces().Should().ContainSingle().Which.Should().BeSameAs(i1);
            t2.GetInterfaces().Should().ContainSingle().Which.Should().BeSameAs(i2);
            t1.GetMethod("Method1").Should().NotBeNull().And.Return(u.LoadContextType("System.Object"));
            t2.GetMethod("Method2").Should().NotBeNull().And.Return(u.LoadContextType("System.Object"));

            ReadMethodImpls(u, "Test.dll").Should().Equal(("Impl1", "Impl1.Method1", "Iface1.Method1"), ("Impl2", "Impl2.Method2", "Iface2.Method2"));
        }

        /// <summary>
        /// The attributes of an interface method that classes implement.
        /// </summary>
        const MethodAttributes InterfaceMethod = MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig;

        /// <summary>
        /// The attributes of a method that implements an interface method through an explicit override.
        /// </summary>
        const MethodAttributes ImplementationMethod = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot | MethodAttributes.HideBySig;

        /// <summary>
        /// Reads the MethodImpl rows of a saved assembly, in table order.
        /// </summary>
        /// <param name="u">The universe the assembly was saved from.</param>
        /// <param name="fileName">The file name of the assembly in <see cref="TestUniverse.TempPath"/>.</param>
        /// <returns>For each row, the implementing type and the body and declaration as <c>Type.Method</c>.</returns>
        static (string Type, string Body, string Declaration)[] ReadMethodImpls(TestUniverse u, string fileName)
        {
            using var pe = new PEReader(File.OpenRead(Path.Combine(u.TempPath, fileName)));
            var md = pe.GetMetadataReader();

            string Name(EntityHandle handle)
            {
                var method = md.GetMethodDefinition((MethodDefinitionHandle)handle);
                return md.GetString(md.GetTypeDefinition(method.GetDeclaringType()).Name) + "." + md.GetString(method.Name);
            }

            return Enumerable.Range(1, md.GetTableRowCount(TableIndex.MethodImpl))
                .Select(i => md.GetMethodImplementation(MetadataTokens.MethodImplementationHandle(i)))
                .Select(m => (md.GetString(md.GetTypeDefinition(m.Type).Name), Name(m.MethodBody), Name(m.MethodDeclaration)))
                .ToArray();
        }

    }

}
