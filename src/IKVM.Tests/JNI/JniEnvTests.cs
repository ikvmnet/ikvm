using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.JNI
{

    /// <summary>
    /// Exercises the JNI function table through the native methods of ikvm.tests.jni.JniEnvTests. A native method that
    /// finds something wrong throws JniEnvTests$Failure, which fails the test with the location of the failed check.
    /// </summary>
    /// <remarks>
    /// Tests that are ignored document behavior that is known to be wrong. The reason given is the defect, and the
    /// test should pass once it is fixed.
    /// </remarks>
    [TestClass]
    public class JniEnvTests
    {

        const int JNI_COMMIT = 1;
        const int JNI_ABORT = 2;

        /// <summary>
        /// How Target.format records the arguments the native side passes to every target method.
        /// </summary>
        const string ExpectedArgs = "true,-5,9786,-300,70000,78187493530,0.5,-0.125,arg";

        /// <summary>
        /// A string with a NUL, two and three byte characters and a supplementary character.
        /// </summary>
        const string UnicodeString = "a\0é€\U0001F600";

        /// <summary>
        /// <see cref="UnicodeString"/> in modified UTF-8, where NUL takes two bytes and each surrogate three.
        /// </summary>
        static readonly byte[] UnicodeStringMUTF8 = [0x61, 0xC0, 0x80, 0xC3, 0xA9, 0xE2, 0x82, 0xAC, 0xED, 0xA0, 0xBD, 0xED, 0xB8, 0x80];

        /// <summary>
        /// The arguments the native side passes to Target.manyArgs and Target.staticManyArgs.
        /// </summary>
        static readonly object[] ManyArgs = [1, 2L, 0.5f, 0.25d, -3, 4000000000L, 1.5f, -2.75d, 5, -6L, 2.5f, 8.125d, -7, 9L, -3.25f, 1e10d];

        const string NativeCharMarshaling = "Native method stubs marshal char as a single ANSI byte, so a char outside ASCII passed to or returned from a native method is corrupted.";
        const string GetStaticMethodIDEscapes = "GetStaticMethodID lets NoSuchMethodError escape into native code instead of returning NULL with the exception pending. On Windows it unwinds through the native frames; elsewhere it crashes the process.";
        const string BooleanAndByteArrayRegionBounds = "The boolean and byte array region functions do not check the region before copying: a negative length or a start past the end is accepted, and the set functions write part of the region before throwing.";

        static object test;
        static Type testsType;
        static java.lang.Class targetClass;
        static Type targetType;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var clazz = JniTestClasses.Compiler.GetClass("ikvm.tests.jni.JniEnvTests");
            test = clazz.getConstructor().newInstance([]);
            testsType = test.GetType();
            targetClass = JniTestClasses.Compiler.GetClass("ikvm.tests.jni.JniEnvTests$Target");
            targetType = NewTarget().GetType();
        }

        static object Invoke(string name, params object[] args)
        {
            return InvokeMethod(test, testsType.GetMethod(name) ?? throw new MissingMethodException(testsType.FullName, name), args);
        }

        static object InvokeStatic(string name, params object[] args)
        {
            return InvokeMethod(null, testsType.GetMethod(name) ?? throw new MissingMethodException(testsType.FullName, name), args);
        }

        static object InvokeMethod(object target, MethodInfo method, object[] args)
        {
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        static object NewTarget() => InvokeStatic("newTarget");

        static object NewSubTarget() => InvokeStatic("newSubTarget");

        static object GetField(object target, string name) => targetType.GetField(name).GetValue(target);

        static object GetStaticField(string name) => targetType.GetField(name).GetValue(null);

        static void SetStaticField(string name, object value) => targetType.GetField(name).SetValue(null, value);

        static double Weigh(object[] args) => (double)targetType.GetMethod("weigh").Invoke(null, args);

        static java.lang.Class ClassOf(object o) => ikvm.runtime.Util.getClassFromObject(o);

        static void AssertArray(object actual, Array expected)
        {
            actual.Should().NotBeNull();
            actual.GetType().Should().Be(expected.GetType());
            ((Array)actual).Cast<object>().Should().Equal(expected.Cast<object>());
        }

        /// <summary>
        /// Runs the action on several threads at once, and rethrows the first exception any of them threw.
        /// </summary>
        static void RunConcurrently(int threadCount, Action<int> action)
        {
            var exceptions = new Exception[threadCount];
            var threads = new Thread[threadCount];
            using var start = new ManualResetEventSlim();

            for (int i = 0; i < threadCount; i++)
            {
                var index = i;
                threads[i] = new Thread(() =>
                {
                    start.Wait();

                    try
                    {
                        action(index);
                    }
                    catch (Exception e)
                    {
                        exceptions[index] = e;
                    }
                });
                threads[i].Start();
            }

            start.Set();
            foreach (var thread in threads)
                thread.Join();

            foreach (var e in exceptions)
                if (e != null)
                    ExceptionDispatchInfo.Capture(e).Throw();
        }

        /// <summary>
        /// Encodes a string the way Java's modified UTF-8 does, independently of the code under test.
        /// </summary>
        static byte[] EncodeMUTF8(string s)
        {
            var bytes = new List<byte>(s.Length);
            foreach (var c in s)
            {
                if (c != 0 && c <= 0x7F)
                {
                    bytes.Add((byte)c);
                }
                else if (c <= 0x7FF)
                {
                    bytes.Add((byte)(0xC0 | (c >> 6)));
                    bytes.Add((byte)(0x80 | (c & 0x3F)));
                }
                else
                {
                    bytes.Add((byte)(0xE0 | (c >> 12)));
                    bytes.Add((byte)(0x80 | ((c >> 6) & 0x3F)));
                    bytes.Add((byte)(0x80 | (c & 0x3F)));
                }
            }

            return bytes.ToArray();
        }

        static string AllChars() => new string(Enumerable.Range(0, 0x10000).Select(i => (char)i).ToArray());

        #region Version and classes

        [TestMethod]
        public void GetVersion() => Invoke("getVersion");

        [TestMethod]
        [Ignore("DefineClass passes the name to ClassLoader.defineClass with its slashes, so the class is rejected as having the wrong name.")]
        public void DefineClass()
        {
            var bytes = JniTestClasses.Compiler.GetClassBytes(JniTestClasses.DefinedByJniClassName);
            var loader = InvokeStatic("newClassLoader");

            var defined = (java.lang.Class)Invoke("defineClass", "ikvm/tests/jni/DefinedByJni", loader, bytes);
            defined.Should().NotBeNull();
            defined.getName().Should().Be(JniTestClasses.DefinedByJniClassName);
            defined.getClassLoader().Should().BeSameAs(loader);
            defined.Should().NotBeSameAs(JniTestClasses.Compiler.GetClass(JniTestClasses.DefinedByJniClassName));
        }

        [TestMethod]
        public void DefineClassWithBadBytes() => Invoke("defineClassWithBadBytes", InvokeStatic("newClassLoader"));

        [TestMethod]
        public void FindClass() => Invoke("findClass");

        [TestMethod]
        public void FindClassVisibleOnlyToTheCallersLoader() => Invoke("findClassVisibleOnlyToTheCallersLoader");

        [TestMethod]
        public void FindClassNotFound() => Invoke("findClassNotFound");

        [TestMethod]
        public void FindClassWithDottedName() => Invoke("findClassWithDottedName");

        [TestMethod]
        public void FindArrayAndNestedClasses() => Invoke("findArrayAndNestedClasses");

        [TestMethod]
        public void FindClassRunsStaticInitializer() => Invoke("findClassRunsStaticInitializer");

        [TestMethod]
        public void FindClassWithFailingStaticInitializer() => Invoke("findClassWithFailingStaticInitializer");

        [TestMethod]
        public void GetSuperclass() => Invoke("getSuperclass");

        [TestMethod]
        public void IsAssignableFrom() => Invoke("isAssignableFrom");

        [TestMethod]
        public void ArraysImplementCloneableAndSerializable() => Invoke("arraysImplementCloneableAndSerializable");

        [TestMethod]
        public void GetObjectClass() => Invoke("getObjectClass");

        [TestMethod]
        public void IsInstanceOf() => Invoke("isInstanceOf");

        [TestMethod]
        [Ignore("IsInstanceOf throws NullPointerException for a NULL object, instead of returning JNI_TRUE.")]
        public void IsInstanceOfNull() => Invoke("isInstanceOfNull");

        [TestMethod]
        public void IsSameObject() => Invoke("isSameObject");

        #endregion

        #region Reflection

        [TestMethod]
        public void FromReflectedMethodAndBack() => Invoke("fromReflectedMethodAndBack", InvokeStatic("getMethod", targetClass, "intMethod"));

        [TestMethod]
        public void FromReflectedConstructorAndBack() => Invoke("fromReflectedConstructorAndBack", InvokeStatic("getDefaultConstructor", targetClass));

        [TestMethod]
        public void FromReflectedFieldAndBack() => Invoke("fromReflectedFieldAndBack", InvokeStatic("getField", targetClass, "iField"));

        #endregion

        #region Exceptions

        [TestMethod]
        public void ThrowAndCheck() => Invoke("throwAndCheck");

        [TestMethod]
        public void ThrowNewWithNullMessage() => Invoke("throwNewWithNullMessage");

        [TestMethod]
        public void ThrowNewWithUnicodeMessage()
        {
            FluentActions.Invoking(() => Invoke("throwNewWithUnicodeMessage")).Should().Throw<java.lang.IllegalStateException>().Which.getMessage().Should().Be("café ☃");
        }

        [TestMethod]
        public void ThrowNewChecked()
        {
            FluentActions.Invoking(() => Invoke("throwNewChecked")).Should().Throw<java.io.IOException>().Which.getMessage().Should().Be("checked");
        }

        [TestMethod]
        public void RethrowJavaException()
        {
            var e = new java.lang.IllegalStateException("rethrown");
            FluentActions.Invoking(() => Invoke("rethrow", e)).Should().Throw<java.lang.IllegalStateException>().Which.Should().BeSameAs(e);
        }

        [TestMethod]
        public void RethrowDotNetException()
        {
            var e = new InvalidOperationException("rethrown");
            FluentActions.Invoking(() => Invoke("rethrow", e)).Should().Throw<InvalidOperationException>().Which.Should().BeSameAs(e);
        }

        [TestMethod]
        public void ExceptionOccurredAndClear() => Invoke("exceptionOccurredAndClear");

        [TestMethod]
        public void ExceptionOccurredIsTheThrownObject() => Invoke("exceptionOccurredIsTheThrownObject");

        [TestMethod]
        public void ExceptionDescribeClears() => Invoke("exceptionDescribeClears");

        [TestMethod]
        public void ExceptionFromCalledMethodIsPending() => Invoke("exceptionFromCalledMethodIsPending");

        [TestMethod]
        public void ExceptionFromCalledStaticMethodIsPending() => Invoke("exceptionFromCalledStaticMethodIsPending");

        [TestMethod]
        public void LeavesExceptionPending()
        {
            FluentActions.Invoking(() => Invoke("leavesExceptionPending")).Should().Throw<java.lang.IllegalStateException>().Which.getMessage().Should().Be("pending");
        }

        [TestMethod]
        public void PendingExceptionIsCaughtByJava() => Invoke("catchFromNative").Should().Be("pending");

        [TestMethod]
        public void PendingExceptionWinsOverReturnValue()
        {
            FluentActions.Invoking(() => Invoke("throwAndReturnValue")).Should().Throw<java.lang.IllegalStateException>().Which.getMessage().Should().Be("returned anyway");
        }

        #endregion

        #region References

        [TestMethod]
        public void NewLocalRefAndDeleteLocalRef() => Invoke("newLocalRefAndDeleteLocalRef");

        [TestMethod]
        public void ManyLocalRefs() => Invoke("manyLocalRefs", 5000);

        [TestMethod]
        public void DeletedLocalRefsAreReused() => Invoke("deletedLocalRefsAreReused", 100000);

        [TestMethod]
        public void EnsureLocalCapacity() => Invoke("ensureLocalCapacity");

        [TestMethod]
        public void PushAndPopLocalFrame() => Invoke("pushAndPopLocalFrame");

        [TestMethod]
        public void PopLocalFrameWithNullResult() => Invoke("popLocalFrameWithNullResult");

        [TestMethod]
        public void PopLocalFrameWithGlobalResult() => Invoke("popLocalFrameWithGlobalResult");

        [TestMethod]
        public void PushLocalFrameWithLargeAndZeroCapacity() => Invoke("pushLocalFrameWithLargeAndZeroCapacity");

        [TestMethod]
        public void DeleteLocalRefFromOuterFrame() => Invoke("deleteLocalRefFromOuterFrame");

        [TestMethod]
        public void NestedLocalFrames() => Invoke("nestedLocalFrames", 100);

        [TestMethod]
        public void ManyLocalFrames() => Invoke("manyLocalFrames", 10000);

        [TestMethod]
        public void LocalRefsSurviveNestedNativeCalls() => Invoke("localRefsSurviveNestedNativeCalls", 100);

        [TestMethod]
        public void LocalRefsOnManyThreads()
        {
            RunConcurrently(8, i =>
            {
                for (int j = 0; j < 20; j++)
                {
                    Invoke("manyLocalRefs", 2000);
                    Invoke("localRefsSurviveNestedNativeCalls", 20);
                    Invoke("manyLocalFrames", 100);
                }
            });
        }

        [TestMethod]
        public void ConvertBetweenRefKinds() => Invoke("convertBetweenRefKinds");

        [TestMethod]
        public void GlobalRefOutlivesNativeCall()
        {
            var o = new object();
            var r = (long)Invoke("newGlobalRef", o);
            Invoke("resolveGlobalRef", r).Should().BeSameAs(o);
            Invoke("deleteGlobalRef", r);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static long NewGlobalRefToOnlyReference() => (long)Invoke("newGlobalRef", [new int[] { 1, 2, 3 }]);

        [TestMethod]
        public void GlobalRefKeepsObjectAlive()
        {
            var r = NewGlobalRefToOnlyReference();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            AssertArray(Invoke("resolveGlobalRef", r), new int[] { 1, 2, 3 });
            Invoke("deleteGlobalRef", r);
        }

        [TestMethod]
        public void GlobalRefResolvedOnAnotherThread()
        {
            var o = new object();
            var r = (long)Invoke("newGlobalRef", o);

            object resolved = null;
            RunConcurrently(1, i => resolved = Invoke("resolveGlobalRef", r));
            resolved.Should().BeSameAs(o);

            Invoke("deleteGlobalRef", r);
        }

        [TestMethod]
        public void NewGlobalRefOfNull() => Invoke("newGlobalRefOfNull");

        [TestMethod]
        public void ManyGlobalRefs() => Invoke("manyGlobalRefs", 10000);

        [TestMethod]
        public void GlobalRefsOnManyThreads() => RunConcurrently(8, i => Invoke("manyGlobalRefs", 2000));

        [TestMethod]
        public void WeakGlobalRefOutlivesNativeCall()
        {
            var o = new object();
            var r = (long)Invoke("newWeakGlobalRef", o);
            Invoke("resolveWeakGlobalRef", r).Should().BeSameAs(o);
            ((bool)Invoke("isWeakGlobalRefCleared", r)).Should().BeFalse();
            Invoke("deleteWeakGlobalRef", r);
            GC.KeepAlive(o);
        }

        [TestMethod]
        public void WeakGlobalRefSurvivesCollectionWhileReachable()
        {
            var o = new object();
            var r = (long)Invoke("newWeakGlobalRef", o);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Invoke("resolveWeakGlobalRef", r).Should().BeSameAs(o);
            Invoke("deleteWeakGlobalRef", r);
            GC.KeepAlive(o);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static long NewWeakGlobalRefToGarbage() => (long)Invoke("newWeakGlobalRef", new object());

        [TestMethod]
        public void WeakGlobalRefIsClearedWhenCollected()
        {
            var r = NewWeakGlobalRefToGarbage();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            ((bool)Invoke("isWeakGlobalRefCleared", r)).Should().BeTrue();
            Invoke("resolveWeakGlobalRef", r).Should().BeNull();
            Invoke("deleteWeakGlobalRef", r);
        }

        [TestMethod]
        public void NewWeakGlobalRefOfNull() => Invoke("newWeakGlobalRefOfNull");

        [TestMethod]
        public void ManyWeakGlobalRefs() => Invoke("manyWeakGlobalRefs", 1000);

        [TestMethod]
        public void WeakGlobalRefsOnManyThreads() => RunConcurrently(8, i => Invoke("manyWeakGlobalRefs", 500));

        [TestMethod]
        public void GetObjectRefType() => Invoke("getObjectRefType");

        [TestMethod]
        [Ignore("GetObjectRefType reports a local reference for NULL, instead of JNIInvalidRefType.")]
        public void GetObjectRefTypeOfNull() => Invoke("getObjectRefTypeOfNull");

        #endregion

        #region Objects

        [TestMethod]
        public void AllocObject()
        {
            var o = Invoke("allocObject");
            o.GetType().Should().Be(targetType);

            // the constructor did not run
            GetField(o, "constructed").Should().Be(false);
        }

        [TestMethod]
        public void AllocObjectOfAbstractClass() => Invoke("allocObjectOfAbstractClass");

        [TestMethod]
        public void AllocObjectOfInterface() => Invoke("allocObjectOfInterface");

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void NewObjectWithArgs(int mode)
        {
            var o = Invoke("newObjectWithArgs", mode);
            o.GetType().Should().Be(targetType);
            GetField(o, "constructed").Should().Be(true);
            GetField(o, "args").Should().Be(ExpectedArgs);
        }

        [TestMethod]
        public void ConstructWithCallNonvirtualVoidMethod()
        {
            var o = Invoke("constructWithCallNonvirtualVoidMethod");
            o.GetType().Should().Be(targetType);
            GetField(o, "constructed").Should().Be(true);
            GetField(o, "args").Should().Be(ExpectedArgs);
        }

        [TestMethod]
        public void NewObjectOfAbstractClass() => Invoke("newObjectOfAbstractClass");

        [TestMethod]
        public void NewObjectThatThrows() => Invoke("newObjectThatThrows").Should().BeNull();

        #endregion

        #region Method lookup

        [TestMethod]
        public void GetMethodID() => Invoke("getMethodID");

        [TestMethod]
        public void GetMethodIDOfInheritedMethod() => Invoke("getMethodIDOfInheritedMethod");

        [TestMethod]
        public void GetMethodIDOfInterfaceMethod() => Invoke("getMethodIDOfInterfaceMethod");

        [TestMethod]
        public void GetMethodIDNotFound() => Invoke("getMethodIDNotFound");

        [TestMethod]
        public void GetMethodIDOfStaticMethod() => Invoke("getMethodIDOfStaticMethod");

        [TestMethod]
        public void GetStaticMethodID() => Invoke("getStaticMethodID");

        [TestMethod]
        [Ignore(GetStaticMethodIDEscapes)]
        public void GetStaticMethodIDNotFound() => Invoke("getStaticMethodIDNotFound");

        [TestMethod]
        [Ignore(GetStaticMethodIDEscapes)]
        public void GetStaticMethodIDOfInstanceMethod() => Invoke("getStaticMethodIDOfInstanceMethod");

        #endregion

        #region Method calls

        /// <summary>
        /// What each method of Target and SubTarget returns, by return type.
        /// </summary>
        static readonly Dictionary<string, (object Base, object Sub)> CallResults = new()
        {
            ["Boolean"] = (true, false),
            ["Byte"] = (unchecked((byte)-12), (byte)34),
            ["Char"] = ('x', 'y'),
            ["Short"] = ((short)-1234, (short)4321),
            ["Int"] = (123456789, -987654321),
            ["Long"] = (1234567890123L, -9876543210L),
            ["Float"] = (1.5f, -3.5f),
            ["Double"] = (2.25d, -4.75d),
            ["Object"] = ("base", "sub"),
            ["Void"] = (null, null),
        };

        /// <summary>
        /// Each return type, with each way of passing arguments: 0 is the variadic function, 1 the va_list function
        /// and 2 the jvalue array function.
        /// </summary>
        public static IEnumerable<object[]> CallTypesAndModes()
        {
            foreach (var type in CallResults.Keys)
                for (int mode = 0; mode < 3; mode++)
                    yield return [type, mode];
        }

        [DataTestMethod]
        [DynamicData(nameof(CallTypesAndModes), DynamicDataSourceType.Method)]
        public void CallMethod(string type, int mode)
        {
            var target = NewTarget();
            Invoke("call" + type + "Method", target, mode).Should().Be(CallResults[type].Base);
            GetField(target, "args").Should().Be(ExpectedArgs);

            // the call is virtual
            var subTarget = NewSubTarget();
            Invoke("call" + type + "Method", subTarget, mode).Should().Be(CallResults[type].Sub);
            GetField(subTarget, "args").Should().Be("sub:" + ExpectedArgs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CallTypesAndModes), DynamicDataSourceType.Method)]
        public void CallNonvirtualMethod(string type, int mode)
        {
            // the method of Target runs, even though SubTarget overrides it
            var subTarget = NewSubTarget();
            Invoke("callNonvirtual" + type + "Method", subTarget, mode).Should().Be(CallResults[type].Base);
            GetField(subTarget, "args").Should().Be(ExpectedArgs);
        }

        [DataTestMethod]
        [DynamicData(nameof(CallTypesAndModes), DynamicDataSourceType.Method)]
        public void CallStaticMethod(string type, int mode)
        {
            SetStaticField("staticArgs", null);
            Invoke("callStatic" + type + "Method", mode).Should().Be(CallResults[type].Base);
            GetStaticField("staticArgs").Should().Be(ExpectedArgs);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void CallMethodWithManyArgs(int mode)
        {
            Invoke("callManyArgs", NewTarget(), mode).Should().Be(Weigh(ManyArgs));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void CallStaticMethodWithManyArgs(int mode)
        {
            Invoke("callStaticManyArgs", mode).Should().Be(Weigh(ManyArgs));
        }

        [TestMethod]
        public void CallInterfaceMethod()
        {
            Invoke("callInterfaceMethod", NewTarget()).Should().Be(1);
            Invoke("callInterfaceMethod", NewSubTarget()).Should().Be(2);
        }

        [TestMethod]
        public void CallDefaultMethod() => Invoke("callDefaultMethod", NewSubTarget()).Should().Be(10);

        [TestMethod]
        public void CallPrivateMethod() => Invoke("callPrivateMethod", NewTarget()).Should().Be(7);

        [TestMethod]
        public void CallInheritedStaticMethod() => Invoke("callInheritedStaticMethod").Should().Be(8);

        [TestMethod]
        public void CallWithArrays() => AssertArray(Invoke("callWithArrays", NewTarget(), new int[] { 1, 2, 3 }), new int[] { 3, 2, 1 });

        [TestMethod]
        public void CallReturningNull() => Invoke("callReturningNull", NewTarget());

        [TestMethod]
        public void CallThrowingChecked() => Invoke("callThrowingChecked", NewTarget());

        [TestMethod]
        public void CallMethodsOnString() => Invoke("callMethodsOnString", "hello");

        [TestMethod]
        public void CallObjectMethodsOnArray() => Invoke("callObjectMethodsOnArray", [new int[3]]);

        [TestMethod]
        public void CallThrowableMethodOnJavaException()
        {
            Invoke("callThrowableGetMessage", new java.lang.IllegalStateException("java message")).Should().Be("java message");
        }

        [TestMethod]
        public void CallThrowableMethodOnDotNetException()
        {
            Invoke("callThrowableGetMessage", new InvalidOperationException("dotnet message")).Should().Be("dotnet message");
        }

        [TestMethod]
        public void CallStaticMethodWithCallerID()
        {
            // the class is only visible to the class loader of the native method
            Invoke("callStaticMethodWithCallerID", "ikvm.tests.jni.JniEnvTests$Target").Should().BeSameAs(targetClass);
            Invoke("callStaticMethodWithCallerID", "java.lang.String").Should().BeSameAs(ClassOf(""));
        }

        #endregion

        #region Fields

        [TestMethod]
        public void GetFieldID() => Invoke("getFieldID");

        [TestMethod]
        public void GetFieldIDOfInheritedField() => Invoke("getFieldIDOfInheritedField");

        [TestMethod]
        public void GetFieldIDNotFound() => Invoke("getFieldIDNotFound");

        [TestMethod]
        public void GetFieldIDWithWrongSignature() => Invoke("getFieldIDWithWrongSignature");

        [TestMethod]
        public void GetFieldIDOfStaticField() => Invoke("getFieldIDOfStaticField");

        [TestMethod]
        public void GetStaticFieldIDOfInstanceField() => Invoke("getStaticFieldIDOfInstanceField");

        /// <summary>
        /// Each field type, with the prefix of the fields of that type on Target and a value to store in them.
        /// </summary>
        public static IEnumerable<object[]> FieldTypesAndValues()
        {
            yield return ["Boolean", "z", true];
            yield return ["Byte", "b", (byte)200];
            yield return ["Char", "c", 'Z'];
            yield return ["Short", "s", (short)-31000];
            yield return ["Int", "i", -2000000000];
            yield return ["Long", "j", long.MinValue + 7];
            yield return ["Float", "f", -1.25e-30f];
            yield return ["Double", "d", 1.5e300];
            yield return ["Object", "l", "value"];
        }

        public static IEnumerable<object[]> StaticFieldTypesAndValues() => FieldTypesAndValues().Where(i => (string)i[0] != "Boolean");

        [DataTestMethod]
        [DynamicData(nameof(FieldTypesAndValues), DynamicDataSourceType.Method)]
        public void SetAndGetField(string type, string prefix, object value)
        {
            var target = NewTarget();
            Invoke("set" + type + "Field", target, value);
            GetField(target, prefix + "Field").Should().Be(value);
            Invoke("get" + type + "Field", target).Should().Be(value);
        }

        [DataTestMethod]
        [DynamicData(nameof(StaticFieldTypesAndValues), DynamicDataSourceType.Method)]
        public void SetAndGetStaticField(string type, string prefix, object value)
        {
            Invoke("setStatic" + type + "Field", value);
            GetStaticField(prefix + "Static").Should().Be(value);
            Invoke("getStatic" + type + "Field").Should().Be(value);
        }

        [DataTestMethod]
        [Ignore("SetStaticBooleanField stores the opposite of the value it is given.")]
        [DataRow(true)]
        [DataRow(false)]
        public void SetAndGetStaticBooleanField(bool value)
        {
            Invoke("setStaticBooleanField", value);
            GetStaticField("zStatic").Should().Be(value);
            Invoke("getStaticBooleanField").Should().Be(value);
        }

        [TestMethod]
        [Ignore(NativeCharMarshaling)]
        public void SetAndGetNonAsciiCharField()
        {
            var target = NewTarget();
            Invoke("setCharField", target, '€');
            GetField(target, "cField").Should().Be('€');
            Invoke("getCharField", target).Should().Be('€');

            Invoke("setStaticCharField", '€');
            GetStaticField("cStatic").Should().Be('€');
            Invoke("getStaticCharField").Should().Be('€');
        }

        [TestMethod]
        public void AccessPrivateField() => Invoke("accessPrivateField", NewTarget());

        [TestMethod]
        public void GetStaticConstant() => Invoke("getStaticConstant").Should().Be(99);

        [TestMethod]
        public void GetInterfaceConstant() => Invoke("getInterfaceConstant").Should().Be(77);

        [TestMethod]
        public void AccessInheritedStaticField() => Invoke("accessInheritedStaticField");

        [TestMethod]
        public void AccessArrayAndStringFields() => Invoke("accessArrayAndStringFields", NewTarget());

        [TestMethod]
        public void AccessShadowedFields() => Invoke("accessShadowedFields", NewSubTarget());

        #endregion

        #region Strings

        [TestMethod]
        public void NewString()
        {
            Invoke("newString", UnicodeString.ToCharArray()).Should().Be(UnicodeString);
            Invoke("newString", new char[0]).Should().Be("");
        }

        [TestMethod]
        public void GetStringLength()
        {
            Invoke("getStringLength", UnicodeString).Should().Be(6);
            Invoke("getStringLength", "").Should().Be(0);
        }

        [TestMethod]
        public void GetStringChars() => AssertArray(Invoke("getStringChars", UnicodeString), UnicodeString.ToCharArray());

        [TestMethod]
        public void AllCharsRoundTripThroughNewStringAndGetStringChars()
        {
            var s = AllChars();
            Invoke("newString", s.ToCharArray()).Should().Be(s);
            AssertArray(Invoke("getStringChars", s), s.ToCharArray());
            AssertArray(Invoke("getStringCritical", s), s.ToCharArray());
        }

        [TestMethod]
        public void NewStringUTF()
        {
            Invoke("newStringUTF", UnicodeStringMUTF8).Should().Be(UnicodeString);
            Invoke("newStringUTF", new byte[] { 0x61, 0x62 }).Should().Be("ab");
            Invoke("newStringUTF", new byte[0]).Should().Be("");
        }

        [TestMethod]
        public void NewStringUTFOfNull() => Invoke("newStringUTFOfNull");

        [TestMethod]
        public void NewStringUTFWithInvalidBytes() => Invoke("newStringUTFWithInvalidBytes");

        [TestMethod]
        public void GetStringUTFLength()
        {
            Invoke("getStringUTFLength", UnicodeString).Should().Be(UnicodeStringMUTF8.Length);
            Invoke("getStringUTFLength", "").Should().Be(0);
        }

        [TestMethod]
        public void GetStringUTFChars() => AssertArray(Invoke("getStringUTFChars", UnicodeString), UnicodeStringMUTF8);

        [TestMethod]
        public void AllCharsRoundTripThroughModifiedUTF8()
        {
            // every char, including NUL and unpaired surrogates, which modified UTF-8 encodes one by one
            var s = AllChars();
            var expected = EncodeMUTF8(s);

            AssertArray(Invoke("getStringUTFChars", s), expected);
            Invoke("getStringUTFLength", s).Should().Be(expected.Length);
            Invoke("newStringUTF", expected).Should().Be(s);
        }

        [TestMethod]
        public void LongStringRoundTrip()
        {
            var s = string.Concat(Enumerable.Repeat("abé€\U0001F600", 200000));
            Invoke("newString", s.ToCharArray()).Should().Be(s);
            Invoke("newStringUTF", Invoke("getStringUTFChars", s)).Should().Be(s);
        }

        [TestMethod]
        public void EmptyStrings() => Invoke("emptyStrings");

        [TestMethod]
        public void GetStringRegion()
        {
            AssertArray(Invoke("getStringRegion", UnicodeString, 1, 3), "\0é€".ToCharArray());
            AssertArray(Invoke("getStringRegion", UnicodeString, 6, 0), new char[0]);
        }

        [DataTestMethod]
        [DataRow(-1, 1)]
        [DataRow(4, 3)]
        [DataRow(7, 0)]
        public void GetStringRegionOutOfBounds(int start, int len)
        {
            FluentActions.Invoking(() => Invoke("getStringRegion", UnicodeString, start, len)).Should().Throw<java.lang.StringIndexOutOfBoundsException>();
        }

        [TestMethod]
        [Ignore("GetStringRegion accepts a negative length, instead of throwing StringIndexOutOfBoundsException.")]
        public void GetStringRegionWithNegativeLength()
        {
            FluentActions.Invoking(() => Invoke("getStringRegion", UnicodeString, 0, -1)).Should().Throw<java.lang.StringIndexOutOfBoundsException>();
        }

        [TestMethod]
        public void GetStringUTFRegion()
        {
            AssertArray(Invoke("getStringUTFRegion", UnicodeString, 1, 3), new byte[] { 0xC0, 0x80, 0xC3, 0xA9, 0xE2, 0x82, 0xAC });
            AssertArray(Invoke("getStringUTFRegion", UnicodeString, 4, 2), new byte[] { 0xED, 0xA0, 0xBD, 0xED, 0xB8, 0x80 });
            AssertArray(Invoke("getStringUTFRegion", UnicodeString, 6, 0), new byte[0]);
        }

        [DataTestMethod]
        [DataRow(-1, 1)]
        [DataRow(4, 3)]
        [DataRow(7, 0)]
        public void GetStringUTFRegionOutOfBounds(int start, int len)
        {
            FluentActions.Invoking(() => Invoke("getStringUTFRegion", UnicodeString, start, len)).Should().Throw<java.lang.StringIndexOutOfBoundsException>();
        }

        [TestMethod]
        [Ignore("GetStringUTFRegion accepts a negative length, instead of throwing StringIndexOutOfBoundsException.")]
        public void GetStringUTFRegionWithNegativeLength()
        {
            FluentActions.Invoking(() => Invoke("getStringUTFRegion", UnicodeString, 0, -1)).Should().Throw<java.lang.StringIndexOutOfBoundsException>();
        }

        [TestMethod]
        public void GetStringCritical()
        {
            AssertArray(Invoke("getStringCritical", UnicodeString), UnicodeString.ToCharArray());
            AssertArray(Invoke("getStringCritical", ""), new char[0]);
        }

        #endregion

        #region Arrays

        static readonly string[] PrimitiveArrayTypes = ["Boolean", "Byte", "Char", "Short", "Int", "Long", "Float", "Double"];

        public static IEnumerable<object[]> PrimitiveArrayTypeData() => PrimitiveArrayTypes.Select(t => new object[] { t });

        /// <summary>
        /// Each primitive array type, with each release mode.
        /// </summary>
        public static IEnumerable<object[]> PrimitiveArrayTypesAndModes()
        {
            foreach (var type in PrimitiveArrayTypes)
                foreach (var mode in new[] { 0, JNI_COMMIT, JNI_ABORT })
                    yield return [type, mode];
        }

        /// <summary>
        /// Regions that are out of bounds of an array of five elements.
        /// </summary>
        static readonly (int Start, int Len)[] BadRegions = [(-1, 2), (3, 3), (6, 0), (0, -1)];

        static bool IsBooleanOrByte(string type) => type is "Boolean" or "Byte";

        /// <summary>
        /// Whether the boolean and byte array region functions mishandle the region: they do not check it before
        /// copying, so a negative length or a start past the end is accepted, and a set writes up to the end first.
        /// </summary>
        static bool IsBrokenBooleanOrByteRegion(string type, (int Start, int Len) region, bool set) =>
            IsBooleanOrByte(type) && (region.Len < 0 || region.Start > 5 || (set && region.Start >= 0 && region.Start < 5));

        static IEnumerable<object[]> BadRegionData(bool set, bool broken)
        {
            foreach (var type in PrimitiveArrayTypes)
                foreach (var region in BadRegions)
                    if (IsBrokenBooleanOrByteRegion(type, region, set) == broken)
                        yield return [type, region.Start, region.Len];
        }

        public static IEnumerable<object[]> GetBadRegions() => BadRegionData(false, false);

        public static IEnumerable<object[]> BrokenGetBadRegions() => BadRegionData(false, true);

        public static IEnumerable<object[]> SetBadRegions() => BadRegionData(true, false);

        public static IEnumerable<object[]> BrokenSetBadRegions() => BadRegionData(true, true);

        /// <summary>
        /// Makes an array of the given primitive type. For a boolean array, odd values are true.
        /// </summary>
        static Array MakeArray(string type, params int[] values) => type switch
        {
            "Boolean" => values.Select(v => v % 2 == 1).ToArray(),
            "Byte" => values.Select(v => (byte)v).ToArray(),
            "Char" => values.Select(v => (char)v).ToArray(),
            "Short" => values.Select(v => (short)v).ToArray(),
            "Int" => values.ToArray(),
            "Long" => values.Select(v => (long)v).ToArray(),
            "Float" => values.Select(v => (float)v).ToArray(),
            "Double" => values.Select(v => (double)v).ToArray(),
            _ => throw new ArgumentException(type),
        };

        [TestMethod]
        public void GetArrayLength()
        {
            Invoke("getArrayLength", [new int[5]]).Should().Be(5);
            Invoke("getArrayLength", [new string[3]]).Should().Be(3);
            Invoke("getArrayLength", [new object[0]]).Should().Be(0);
        }

        [TestMethod]
        public void NewObjectArray()
        {
            AssertArray(Invoke("newObjectArray", 3, ClassOf(""), "x"), new string[] { "x", "x", "x" });
            AssertArray(Invoke("newObjectArray", 2, ClassOf(""), null), new string[2]);
        }

        [TestMethod]
        public void NewObjectArrayWithNegativeLength()
        {
            FluentActions.Invoking(() => Invoke("newObjectArray", -1, ClassOf(""), null)).Should().Throw<java.lang.NegativeArraySizeException>();
        }

        [TestMethod]
        public void GetObjectArrayElement()
        {
            var array = new string[] { "a", "b" };
            Invoke("getObjectArrayElement", array, 1).Should().Be("b");
            FluentActions.Invoking(() => Invoke("getObjectArrayElement", array, 2)).Should().Throw<java.lang.ArrayIndexOutOfBoundsException>();
            FluentActions.Invoking(() => Invoke("getObjectArrayElement", array, -1)).Should().Throw<java.lang.ArrayIndexOutOfBoundsException>();
        }

        [TestMethod]
        public void SetObjectArrayElement()
        {
            var array = new string[] { "a", "b" };
            Invoke("setObjectArrayElement", array, 0, "z");
            Invoke("setObjectArrayElement", array, 1, null);
            array.Should().Equal("z", null);

            FluentActions.Invoking(() => Invoke("setObjectArrayElement", array, 2, "c")).Should().Throw<java.lang.ArrayIndexOutOfBoundsException>();
            FluentActions.Invoking(() => Invoke("setObjectArrayElement", array, 0, java.lang.Integer.valueOf(1))).Should().Throw<java.lang.ArrayStoreException>();
            array.Should().Equal("z", null);
        }

        [TestMethod]
        public void ArraysOfArrays() => Invoke("arraysOfArrays");

        [TestMethod]
        public void ObjectArrayCovariance() => Invoke("objectArrayCovariance");

        [TestMethod]
        [Ignore("SetObjectArrayElement does not check the element type of an array of a ghost interface such as CharSequence, which is an object[] in .NET, so it stores anything without throwing ArrayStoreException.")]
        public void GhostInterfaceArrayStoreCheck() => Invoke("ghostInterfaceArrayStoreCheck");

        [DataTestMethod]
        [DynamicData(nameof(PrimitiveArrayTypeData), DynamicDataSourceType.Method)]
        public void NewPrimitiveArray(string type)
        {
            AssertArray(Invoke("new" + type + "Array", 3), MakeArray(type, 0, 0, 0));
            AssertArray(Invoke("new" + type + "Array", 0), MakeArray(type));
        }

        [DataTestMethod]
        [DynamicData(nameof(PrimitiveArrayTypeData), DynamicDataSourceType.Method)]
        public void NewPrimitiveArrayWithNegativeLength(string type)
        {
            FluentActions.Invoking(() => Invoke("new" + type + "Array", -1)).Should().Throw<java.lang.NegativeArraySizeException>();
        }

        [DataTestMethod]
        [DynamicData(nameof(PrimitiveArrayTypesAndModes), DynamicDataSourceType.Method)]
        public void ModifyPrimitiveArrayElements(string type, int mode)
        {
            var array = MakeArray(type, 1, 2, 3, 4);
            Invoke("modify" + type + "ArrayElements", array, mode);

            // JNI_ABORT throws the changes away
            AssertArray(array, mode == JNI_ABORT ? MakeArray(type, 1, 2, 3, 4) : MakeArray(type, 2, 3, 4, 5));
        }

        [TestMethod]
        public void ModifyLargePrimitiveArrayElements()
        {
            var array = Enumerable.Range(0, 1000000).ToArray();
            Invoke("modifyIntArrayElements", array, 0);
            array.Should().Equal(Enumerable.Range(1, 1000000));
        }

        [TestMethod]
        public void EmptyArrayElements() => Invoke("emptyArrayElements");

        [DataTestMethod]
        [DynamicData(nameof(PrimitiveArrayTypeData), DynamicDataSourceType.Method)]
        public void GetPrimitiveArrayRegion(string type)
        {
            var array = MakeArray(type, 10, 11, 12, 13, 14);
            AssertArray(Invoke("get" + type + "ArrayRegion", array, 1, 3), MakeArray(type, 11, 12, 13));
            AssertArray(Invoke("get" + type + "ArrayRegion", array, 0, 5), MakeArray(type, 10, 11, 12, 13, 14));
            AssertArray(Invoke("get" + type + "ArrayRegion", array, 5, 0), MakeArray(type));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetBadRegions), DynamicDataSourceType.Method)]
        public void GetPrimitiveArrayRegionOutOfBounds(string type, int start, int len)
        {
            var array = MakeArray(type, 10, 11, 12, 13, 14);
            FluentActions.Invoking(() => Invoke("get" + type + "ArrayRegion", array, start, len)).Should().Throw<java.lang.ArrayIndexOutOfBoundsException>();
        }

        [DataTestMethod]
        [Ignore(BooleanAndByteArrayRegionBounds)]
        [DynamicData(nameof(BrokenGetBadRegions), DynamicDataSourceType.Method)]
        public void GetBooleanAndByteArrayRegionOutOfBounds(string type, int start, int len)
        {
            GetPrimitiveArrayRegionOutOfBounds(type, start, len);
        }

        [DataTestMethod]
        [DynamicData(nameof(PrimitiveArrayTypeData), DynamicDataSourceType.Method)]
        public void SetPrimitiveArrayRegion(string type)
        {
            var array = MakeArray(type, 10, 11, 12, 13, 14);
            Invoke("set" + type + "ArrayRegion", array, 1, 3, MakeArray(type, 21, 22, 23));
            AssertArray(array, MakeArray(type, 10, 21, 22, 23, 14));

            Invoke("set" + type + "ArrayRegion", array, 5, 0, MakeArray(type));
            AssertArray(array, MakeArray(type, 10, 21, 22, 23, 14));
        }

        [DataTestMethod]
        [DynamicData(nameof(SetBadRegions), DynamicDataSourceType.Method)]
        public void SetPrimitiveArrayRegionOutOfBounds(string type, int start, int len)
        {
            var array = MakeArray(type, 10, 11, 12, 13, 14);
            FluentActions.Invoking(() => Invoke("set" + type + "ArrayRegion", array, start, len, MakeArray(type, 20, 20, 20))).Should().Throw<java.lang.ArrayIndexOutOfBoundsException>();

            // nothing was written, and the values differ from all of the elements, so any partial write shows
            AssertArray(array, MakeArray(type, 10, 11, 12, 13, 14));
        }

        [DataTestMethod]
        [Ignore(BooleanAndByteArrayRegionBounds)]
        [DynamicData(nameof(BrokenSetBadRegions), DynamicDataSourceType.Method)]
        public void SetBooleanAndByteArrayRegionOutOfBounds(string type, int start, int len)
        {
            SetPrimitiveArrayRegionOutOfBounds(type, start, len);
        }

        [TestMethod]
        public void ModifyPrimitiveArrayCritical()
        {
            var array = new int[] { 1, 2, 3 };
            Invoke("modifyPrimitiveArrayCritical", array, 0);
            array.Should().Equal(2, 3, 4);
        }

        [TestMethod]
        public void NestedPrimitiveArrayCritical()
        {
            var a = new int[] { 1, 2 };
            var b = new byte[] { 10, 20 };
            Invoke("nestedPrimitiveArrayCritical", a, b);
            a.Should().Equal(2, 3);
            b.Should().Equal(11, 21);
        }

        [TestMethod]
        public void PrimitiveArrayCriticalOfWideTypes()
        {
            var l = new long[] { 1, 2 };
            var d = new double[] { 1.0 };
            Invoke("primitiveArrayCriticalOfWideTypes", l, d);
            l.Should().Equal(0x100000001L, 0x100000002L);
            d.Should().Equal(1.5);
        }

        #endregion

        #region Native methods

        [TestMethod]
        public void NativeMethodArguments()
        {
            Invoke("echoArgs", true, unchecked((byte)-7), 'q', (short)-12345, int.MinValue, long.MaxValue, 3.25f, -6.5d, "obj").Should().Be("true,-7,113,-12345,-2147483648,9223372036854775807,3.25,-6.5,obj");
            Invoke("echoArgs", false, (byte)0, '\0', (short)0, 0, 0L, 0f, 0d, null).Should().Be("false,0,0,0,0,0,0.0,0.0,null");
        }

        [TestMethod]
        public void StaticNativeMethodArguments()
        {
            InvokeStatic("staticEchoArgs", true, unchecked((byte)-7), 'q', (short)-12345, int.MinValue, long.MaxValue, 3.25f, -6.5d, "obj").Should().Be("true,-7,113,-12345,-2147483648,9223372036854775807,3.25,-6.5,obj");
        }

        [TestMethod]
        [Ignore(NativeCharMarshaling)]
        public void NativeMethodNonAsciiCharArgument()
        {
            Invoke("echoArgs", true, (byte)0, '☺', (short)0, 0, 0L, 0f, 0d, null).Should().Be("true,0,9786,0,0,0,0.0,0.0,null");
        }

        [TestMethod]
        public void NativeMethodWithManyArguments()
        {
            // enough arguments that some are passed on the stack
            Invoke("manyNativeArgs", ManyArgs).Should().Be(Weigh(ManyArgs));
        }

        public static IEnumerable<object[]> ReturnedValues()
        {
            yield return ["Boolean", true];
            yield return ["Boolean", false];
            yield return ["Byte", (byte)0x80];
            yield return ["Byte", (byte)0x7F];
            yield return ["Char", 'A'];
            yield return ["Char", '\0'];
            yield return ["Short", short.MinValue];
            yield return ["Short", short.MaxValue];
            yield return ["Int", int.MinValue];
            yield return ["Int", int.MaxValue];
            yield return ["Long", long.MinValue];
            yield return ["Long", long.MaxValue];
            yield return ["Float", float.MaxValue];
            yield return ["Float", float.Epsilon];
            yield return ["Float", float.NaN];
            yield return ["Double", double.MinValue];
            yield return ["Double", double.Epsilon];
            yield return ["Double", double.NegativeInfinity];
        }

        [DataTestMethod]
        [DynamicData(nameof(ReturnedValues), DynamicDataSourceType.Method)]
        public void NativeMethodReturnsPrimitive(string type, object value)
        {
            Invoke("return" + type, value).Should().Be(value);
        }

        [TestMethod]
        [Ignore(NativeCharMarshaling)]
        public void NativeMethodReturnsNonAsciiChar()
        {
            Invoke("returnChar", '☃').Should().Be('☃');
        }

        [TestMethod]
        public void NativeMethodReturnsObject()
        {
            var o = new object();
            Invoke("returnObject", o).Should().BeSameAs(o);

            var a = new int[] { 1 };
            Invoke("returnArray", [a]).Should().BeSameAs(a);

            Invoke("returnNull").Should().BeNull();
        }

        [TestMethod]
        public void NativeMethodReturnsGlobalRef()
        {
            var o = new object();
            Invoke("returnGlobalRef", o).Should().BeSameAs(o);
            Invoke("returnGlobalRef", "second").Should().Be("second");
        }

        [TestMethod]
        public void NativeMethodsOnManyThreads()
        {
            RunConcurrently(8, i =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    Invoke("returnInt", i * 1000 + j).Should().Be(i * 1000 + j);
                    Invoke("echoArgs", true, (byte)i, 'q', (short)j, i, (long)j, 0.5f, 0.25d, "x").Should().Be($"true,{i},113,{j},{i},{j},0.5,0.25,x");
                }
            });
        }

        [TestMethod]
        public void RegisterNatives() => Invoke("registerNatives");

        [TestMethod]
        public void RegisterMultipleNatives() => Invoke("registerMultipleNatives");

        [TestMethod]
        public void ReRegisterNatives() => Invoke("reRegisterNatives");

        [TestMethod]
        public void UnregisterNatives() => Invoke("unregisterNatives");

        [TestMethod]
        public void RegisterNativesNotFound() => Invoke("registerNativesNotFound");

        [TestMethod]
        public void NativeMethodNameWithUnderscore() => Invoke("under_score").Should().Be(1);

        [TestMethod]
        public void NativeMethodNameWithDollarSign() => Invoke("dollar$sign").Should().Be(2);

        [TestMethod]
        public void NativeMethodNameWithNonAsciiCharacter() => Invoke("café").Should().Be(3);

        [TestMethod]
        public void OverloadedNativeMethodsBindByLongName()
        {
            InvokeMethod(test, testsType.GetMethod("overloaded", [typeof(int)]), [1]).Should().Be(4);
            InvokeMethod(test, testsType.GetMethod("overloaded", [typeof(string)]), ["s"]).Should().Be(5);
        }

        [TestMethod]
        public void NativeMethodNotFound()
        {
            FluentActions.Invoking(() => Invoke("notImplemented")).Should().Throw<java.lang.UnsatisfiedLinkError>();
        }

        [TestMethod]
        public void NativeMethodReturningWrongTypeThrows()
        {
            FluentActions.Invoking(() => Invoke("returnsWrongType", java.lang.Integer.valueOf(1))).Should().Throw<Exception>().Where(e => e is InvalidCastException || e is java.lang.ClassCastException);
        }

        [TestMethod]
        [Ignore("The native method stub writes '*** exception in native code ***' and the exception to standard output when an exception passes through it.")]
        public void NativeMethodReturningWrongTypeDoesNotWriteToStandardOutput()
        {
            var stdout = Console.Out;
            var captured = new StringWriter();
            Console.SetOut(captured);

            try
            {
                FluentActions.Invoking(() => Invoke("returnsWrongType", java.lang.Integer.valueOf(1))).Should().Throw<Exception>();
            }
            finally
            {
                Console.SetOut(stdout);
            }

            captured.ToString().Should().BeEmpty();
        }

        [TestMethod]
        public void SynchronizedNativeMethodHoldsLock() => Invoke("synchronizedHoldsLock").Should().Be(true);

        [TestMethod]
        public void StaticSynchronizedNativeMethodHoldsLock() => InvokeStatic("staticSynchronizedHoldsLock").Should().Be(true);

        #endregion

        #region Monitors

        [TestMethod]
        public void MonitorEnterAndExit() => Invoke("monitorEnterAndExit", new object());

        [TestMethod]
        public void MonitorEnterIsReentrant() => Invoke("monitorEnterIsReentrant", new object());

        [TestMethod]
        public void MonitorEnterThenSynchronizedJava() => Invoke("monitorEnterThenSynchronizedJava", new object());

        [TestMethod]
        public void MonitorExitNotOwned() => Invoke("monitorExitNotOwned", new object());

        #endregion

        #region Direct buffers

        [TestMethod]
        public void NewDirectByteBuffer() => Invoke("newDirectByteBuffer");

        [TestMethod]
        public void DirectBufferFromJava() => Invoke("directBufferFromJava", java.nio.ByteBuffer.allocateDirect(32));

        [TestMethod]
        public void ZeroCapacityDirectBuffer() => Invoke("zeroCapacityDirectBuffer");

        [TestMethod]
        [Ignore("GetDirectBufferAddress and GetDirectBufferCapacity throw ClassCastException for a buffer that isn't direct, instead of returning NULL and -1.")]
        public void GetDirectBufferAddressOfHeapBuffer() => Invoke("getDirectBufferAddressOfHeapBuffer");

        #endregion

        #region Virtual machine

        [TestMethod]
        public void GetJavaVM() => Invoke("getJavaVM");

        [TestMethod]
        public void JavaVMFromOnLoad() => Invoke("javaVMFromOnLoad");

        [TestMethod]
        public void NativeCallOnNewThread() => RunConcurrently(1, i => Invoke("getJavaVM"));

        [TestMethod]
        public void GetEnvWithUnsupportedVersion() => Invoke("getEnvWithUnsupportedVersion");

        [TestMethod]
        public void GetCreatedJavaVMs() => Invoke("getCreatedJavaVMs");

        [TestMethod]
        public void CreateJavaVMWhenOneExists() => Invoke("createJavaVMWhenOneExists");

        [TestMethod]
        public void AttachCurrentThread() => Invoke("attachCurrentThread", 0);

        [TestMethod]
        [Ignore("AttachCurrentThread with JavaVMAttachArgs whose group is NULL looks up global reference -1, and the exception that escapes into native code crashes the process.")]
        public void AttachCurrentThreadWithName() => Invoke("attachCurrentThread", 1);

        [TestMethod]
        public void AttachCurrentThreadAsDaemon() => Invoke("attachCurrentThread", 2);

        [TestMethod]
        public void AttachAndDetachRepeatedly() => Invoke("attachCurrentThread", 3);

        #endregion

    }

}
