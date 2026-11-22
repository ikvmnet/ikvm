using FluentAssertions;

using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Tests.Linking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jvm
{

    using CodeInfo = IKVM.CoreLib.Jvm.CodeInfo<TestJvmTypeSystem, TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>;
    using State = IKVM.CoreLib.Jvm.InstructionState<TestJvmTypeSystem, TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>;
    using StackState = IKVM.CoreLib.Jvm.StackState<TestJvmTypeSystem, TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>;
    using VT = IKVM.CoreLib.Jvm.VerificationType<TestJavaType>;

    [TestClass]
    public class CodeInfoTests
    {

        static readonly TestJvmTypeSystem TypeSystem = new(false);

        /// <summary>
        /// Creates the states of a method of two instructions, of which only the first has been reached. Its stack holds,
        /// from the bottom: 'this', an extended precision float, a plain double, and an int.
        /// </summary>
        static State[] CreateStates()
        {
            var states = new State[2];
            states[0] = new State(TypeSystem, states, 2, 8);
            states[0].PushType(VT.This(TestTypes.String));
            states[0].PushExtendedFloat();
            states[0].PushDouble();
            states[0].PushInt();
            states[0].SetLocalType(0, VT.This(TestTypes.String), 0);
            states[0].SetLocalType(1, VT.ExtendedFloat, 0);
            return states;
        }

        [TestMethod]
        public void HasStateIsFalseForUnreachedInstructions()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.HasState(0).Should().BeTrue();
            info.HasState(1).Should().BeFalse();
        }

        [TestMethod]
        public void StackHeightCountsCategory2ValuesOnce()
        {
            new CodeInfo(TypeSystem, CreateStates()).GetStackHeight(0).Should().Be(4);
        }

        [TestMethod]
        public void StackTypesHaveNoExtendedPrecisionOrThisFlag()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.GetStackType(0, 0).Should().Be(VT.Int);
            info.GetStackType(0, 1).Should().Be(VT.Double);
            info.GetStackType(0, 2).Should().Be(VT.Float);
            info.GetStackType(0, 3).Should().Be(VT.Reference(TestTypes.String));
        }

        [TestMethod]
        public void RawStackTypesKeepTheThisFlag()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.GetRawStackType(0, 2).Should().Be(VT.Float);
            info.GetRawStackType(0, 3).Should().Be(VT.This(TestTypes.String));
        }

        [TestMethod]
        public void ExtendedPrecisionIsReportedSeparately()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.IsStackTypeExtendedFloat(0, 2).Should().BeTrue();
            info.IsStackTypeExtendedDouble(0, 2).Should().BeFalse();
            info.IsStackTypeExtendedDouble(0, 1).Should().BeFalse();
            info.IsStackTypeExtendedFloat(0, 0).Should().BeFalse();
        }

        [TestMethod]
        public void LocalTypesAreReturnedUnchanged()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.GetLocalType(0, 0).Should().Be(VT.This(TestTypes.String));
            info.GetLocalType(0, 1).Should().Be(VT.ExtendedFloat);
        }

        [TestMethod]
        public void FindCommonBaseTypeUsesTheTypeSystem()
        {
            var info = new CodeInfo(TypeSystem, CreateStates());
            info.FindCommonBaseType(VT.Reference(TestTypes.Integer), VT.Reference(TestTypes.Double)).Should().Be(VT.Reference(TestTypes.Number));
            info.FindCommonBaseType(VT.Int, VT.Float).Should().Be(VT.Top);
        }

        [TestMethod]
        public void ClearFaultBlockExceptionReplacesItWithThrowable()
        {
            var states = new State[1];
            states[0] = new State(TypeSystem, states, 0, 1);
            states[0].PushType(VT.FaultBlockException(TestTypes.Throwable, 0));

            new CodeInfo(TypeSystem, states).ClearFaultBlockException(0);
            states[0].GetStackSlotEx(0).Should().Be(VT.Reference(TestTypes.Throwable));
        }

        [TestMethod]
        public void StackStateWalksDownWithoutModifyingTheState()
        {
            var states = CreateStates();
            var stack = new StackState(states, 0);
            stack.PopInt();
            stack.PopDouble();
            stack.PopFloat();
            stack.PeekType().Should().Be(VT.Reference(TestTypes.String), "'this' is a plain reference once popped");
            stack.PopObjectType(TestTypes.CharSequence).Should().Be(VT.Reference(TestTypes.String));
            FluentActions.Invoking(() => stack.PopAnyType()).Should().Throw<VerifyException>().WithMessage("Unable to pop operand off an empty stack");

            states[0].GetStackHeight().Should().Be(4);
        }

        [TestMethod]
        public void StackStateChecksTypes()
        {
            var stack = new StackState(CreateStates(), 0);
            FluentActions.Invoking(() => stack.PopFloat()).Should().Throw<VerifyException>();

            stack = new StackState(CreateStates(), 0);
            stack.PopInt();
            stack.PopDouble();
            FluentActions.Invoking(() => stack.PopArrayType()).Should().Throw<VerifyException>();
        }

    }

}
