using System.Linq;
using System;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    public class TypeBuilderTests
    {

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
            execMethod.DefineParameter(0, ParameterAttributes.None, "args");
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

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineInterfaceImplementation(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var objectType = u.Import(typeof(object));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var ifaceType = module.DefineType("Iface", TypeAttributes.Interface);
            var ifaceMethod = ifaceType.DefineMethod("Method", MethodAttributes.Abstract, objectType, Type.EmptyTypes);

            var implType = module.DefineType("Impl", TypeAttributes.Public, null, [ifaceType]);
            var implMethod = implType.DefineMethod("Method", MethodAttributes.Public, objectType, Type.EmptyTypes);
            implType.DefineMethodOverride(ifaceMethod, implMethod);

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
        }

        /// <summary>
        /// Custom attributes set on builders can be read back from the created type, before and after saving.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanReadBackCustomAttributes(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var obsolete = u.Import(typeof(System.ObsoleteAttribute)).GetConstructor([u.Import(typeof(string))]);
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
        /// The InterfaceImpl table must be sorted by class; defining overrides out of type order checks that.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineMultipleInterfaceImplementationsOutOfOrder(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var objectType = u.Import(typeof(object));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            var iface1Type = module.DefineType("Iface1", TypeAttributes.Interface);
            var iface1Method = iface1Type.DefineMethod("Method1", MethodAttributes.Abstract, objectType, Type.EmptyTypes);

            var iface2Type = module.DefineType("Iface2", TypeAttributes.Interface);
            var iface2Method = iface2Type.DefineMethod("Method2", MethodAttributes.Abstract, objectType, Type.EmptyTypes);

            var impl1Type = module.DefineType("Impl1", TypeAttributes.Public, null, [iface1Type]);
            var impl1Method = impl1Type.DefineMethod("Method1", MethodAttributes.Public, objectType, Type.EmptyTypes);
            var il1 = impl1Method.GetILGenerator();
            il1.Emit(OpCodes.Ldnull);
            il1.Emit(OpCodes.Ret);

            var impl2Type = module.DefineType("Impl2", TypeAttributes.Public, null, [iface2Type]);
            var impl2Method = impl2Type.DefineMethod("Method2", MethodAttributes.Public, objectType, Type.EmptyTypes);
            var il2 = impl2Method.GetILGenerator();
            il2.Emit(OpCodes.Ldnull);
            il2.Emit(OpCodes.Ret);

            impl2Type.DefineMethodOverride(iface2Method, impl2Method);
            impl1Type.DefineMethodOverride(iface1Method, impl1Method);

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
        }

    }

}
