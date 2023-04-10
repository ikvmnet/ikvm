using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Java.Extensions.java.lang;
using IKVM.Tests.Util;

using java.lang;
using java.util.function;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    //[TestClass]
    //public class ObjectTests
//    {

//        [TestMethod]
//        public void CanOverrideFinalize()
//        {
//            var source = """
//import java.lang.*;
//import java.util.function.Consumer;

//public class ObjectWithFinalize {

//    //public Runnable onFinalize;

//    @Override
//    protected void finalize() {
//        System.out.println("finalize");
//        //onFinalize.run();
//    }

//}
//""";
//            var unit = new InMemoryCodeUnit("ObjectWithFinalize", source);
//            var compiler = new InMemoryCompiler(new[] { unit });
//            compiler.Compile();

//            // check the clazz
//            var clazz = compiler.GetClass("ObjectWithFinalize");
//            var ctor = clazz.getConstructor();
//            var type = global::ikvm.runtime.Util.getRuntimeTypeFromClass(clazz);
//            type.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();

//            // record when finalizer is executed
//            var f = false;
//            f.Should().BeFalse();

//            // run on other method to prevent debug holding instance
//            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
//            static void CreateClassWithoutReference(Action action)
//            {
//                var test = (dynamic)ctor.newInstance(System.Array.Empty<object>());
//                //test.onFinalize = new DelegateRunnable(action);
//            }

//            CreateClassWithoutReference(() => f = true);
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            f.Should().BeTrue();
//        }

//        [TestMethod]
//        public void FinalizerWorks()
//        {
//            var obj = new Example();
//            obj = null;    // Avoid debugger extending its lifetime
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            global::System.Console.WriteLine.ReadLine();
//        }

//        class Base { ~Base() { global::System.Console.WriteLine("Base finalizer called"); } }
//        class Derived : Base { ~Derived() { global::System.Console.WriteLine("Derived finalizer called"); } }
//        class Example : Derived { }

//    }

}
