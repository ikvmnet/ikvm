using System;

using FluentAssertions;

using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Tests.Linking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Jvm
{

    using State = IKVM.CoreLib.Jvm.InstructionState<TestJvmTypeSystem, TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>;
    using VT = IKVM.CoreLib.Jvm.VerificationType<TestJavaType>;

    [TestClass]
    public class InstructionStateTests
    {

        static readonly TestJvmTypeSystem Runtime = new(false);
        static readonly TestJvmTypeSystem Importer = new(true);

        static VT Ref(TestJavaType type) => VT.Reference(type);

        static State NewState(int maxLocals = 4, int maxStack = 4, TestJvmTypeSystem typeSystem = default)
        {
            return new State(typeSystem, new State[1], maxLocals, maxStack);
        }

        /// <summary>
        /// Merges the two types in both orders, and asserts that both give the same result.
        /// </summary>
        static VT Merge(VT type1, VT type2)
        {
            var states = new State[1];
            var result = State.FindCommonBaseType(Runtime, states, type1, type2);
            State.FindCommonBaseType(Runtime, states, type2, type1).Should().Be(result, "merging is commutative");
            return result;
        }

        [TestMethod]
        public void MergingEqualTypesGivesThatType()
        {
            Merge(Ref(TestTypes.String), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.String));
            Merge(VT.Int, VT.Int).Should().Be(VT.Int);
            Merge(VT.Null, VT.Null).Should().Be(VT.Null);
            Merge(Ref(TestTypes.Missing), Ref(TestTypes.Missing)).Should().Be(Ref(TestTypes.Missing));
        }

        [TestMethod]
        public void MergingWithObjectGivesObject()
        {
            Merge(Ref(TestTypes.Object), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.Object), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.Object), Ref(TestTypes.Comparable)).Should().Be(Ref(TestTypes.Object));
        }

        [TestMethod]
        public void MergingWithNullGivesTheOtherType()
        {
            Merge(VT.Null, Ref(TestTypes.String)).Should().Be(Ref(TestTypes.String));
            Merge(VT.Null, Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.String.MakeArrayType()));
        }

        [TestMethod]
        public void MergingWithTopGivesTop()
        {
            Merge(VT.Top, Ref(TestTypes.String)).Should().Be(VT.Top);
            Merge(VT.Top, VT.Int).Should().Be(VT.Top);
        }

        [TestMethod]
        public void MergingDifferentPrimitivesGivesTop()
        {
            Merge(VT.Int, VT.Float).Should().Be(VT.Top);
            Merge(VT.Long, VT.Double).Should().Be(VT.Top);
            Merge(VT.Int, Ref(TestTypes.Integer)).Should().Be(VT.Top);
            Merge(VT.Int, VT.Null).Should().Be(VT.Int, "null is checked before primitives, and the stack merge rejects primitives itself");
        }

        [TestMethod]
        public void MergingUninitializedGivesTop()
        {
            Merge(VT.Uninitialized(TestTypes.String, 3), Ref(TestTypes.String)).Should().Be(VT.Top);
            Merge(VT.Uninitialized(TestTypes.String, 3), VT.Uninitialized(TestTypes.String, 5)).Should().Be(VT.Top);
            Merge(VT.UninitializedThis, Ref(TestTypes.String)).Should().Be(VT.Top);
        }

        [TestMethod]
        public void MergingUninitializedWithItselfGivesItself()
        {
            Merge(VT.Uninitialized(TestTypes.String, 3), VT.Uninitialized(TestTypes.String, 3)).Should().Be(VT.Uninitialized(TestTypes.String, 3));
        }

        [TestMethod]
        public void MergingUnloadableGivesTheVerifierUnloadableType()
        {
            Merge(Ref(TestTypes.Missing), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.VerifierUnloadable));
        }

        [TestMethod]
        public void MergingClassesGivesTheirCommonSuperclass()
        {
            Merge(Ref(TestTypes.Integer), Ref(TestTypes.Double)).Should().Be(Ref(TestTypes.Number));
            Merge(Ref(TestTypes.Integer), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.RuntimeException), Ref(TestTypes.Error)).Should().Be(Ref(TestTypes.Throwable));
            Merge(Ref(TestTypes.Exception), Ref(TestTypes.RuntimeException)).Should().Be(Ref(TestTypes.Exception));
        }

        [TestMethod]
        public void MergingWithAnImplementedInterfaceGivesTheInterface()
        {
            Merge(Ref(TestTypes.Comparable), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.Comparable));
            Merge(Ref(TestTypes.Comparable), Ref(TestTypes.Integer)).Should().Be(Ref(TestTypes.Comparable));
            Merge(Ref(TestTypes.Collection), Ref(TestTypes.ArrayList)).Should().Be(Ref(TestTypes.Collection), "interfaces are inherited through other interfaces");
            Merge(Ref(TestTypes.Cloneable), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Cloneable), "arrays implement Cloneable");
        }

        [TestMethod]
        public void MergingWithAnInterfaceThatIsNotImplementedGivesObject()
        {
            Merge(Ref(TestTypes.Comparable), Ref(TestTypes.Number)).Should().Be(Ref(TestTypes.Object));
        }

        [TestMethod]
        public void MergingTwoInterfacesGivesObject()
        {
            // even when one extends the other, as the JVM leaves the check to invokeinterface
            Merge(Ref(TestTypes.List), Ref(TestTypes.Collection)).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.Comparable), Ref(TestTypes.CharSequence)).Should().Be(Ref(TestTypes.Object));
        }

        [TestMethod]
        public void MergingReferenceArraysMergesTheirElementTypes()
        {
            Merge(Ref(TestTypes.Integer.MakeArrayType()), Ref(TestTypes.Double.MakeArrayType())).Should().Be(Ref(TestTypes.Number.MakeArrayType()));
            Merge(Ref(TestTypes.String.MakeArrayType()), Ref(TestTypes.Integer.MakeArrayType())).Should().Be(Ref(TestTypes.Object.MakeArrayType()));
            Merge(Ref(TestTypes.Comparable.MakeArrayType()), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Comparable.MakeArrayType()));
            Merge(Ref(TestTypes.Integer.MakeArrayType(2)), Ref(TestTypes.Double.MakeArrayType(2))).Should().Be(Ref(TestTypes.Number.MakeArrayType(2)));
        }

        [TestMethod]
        public void MergingArraysOfDifferentRankMergesAtTheShorterRank()
        {
            Merge(Ref(TestTypes.String.MakeArrayType(2)), Ref(TestTypes.Integer.MakeArrayType())).Should().Be(Ref(TestTypes.Object.MakeArrayType()));
            Merge(Ref(TestTypes.PrimitiveInt.MakeArrayType(2)), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Object.MakeArrayType()), "int[] is an Object");
        }

        [TestMethod]
        public void MergingArraysOfDifferentPrimitivesGivesObjectAtOneLowerRank()
        {
            Merge(Ref(TestTypes.PrimitiveInt.MakeArrayType()), Ref(TestTypes.PrimitiveLong.MakeArrayType())).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.PrimitiveInt.MakeArrayType()), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Object));
            Merge(Ref(TestTypes.PrimitiveInt.MakeArrayType(2)), Ref(TestTypes.PrimitiveLong.MakeArrayType(2))).Should().Be(Ref(TestTypes.Object.MakeArrayType()));
        }

        [TestMethod]
        public void MergingArraysOfValueTypesGivesObjectAtOneLowerRank()
        {
            Merge(Ref(TestTypes.ValueType.MakeArrayType()), Ref(TestTypes.String.MakeArrayType())).Should().Be(Ref(TestTypes.Object));
        }

        [TestMethod]
        public void MergingAnArrayWithAClassGivesTheirCommonSuperclass()
        {
            Merge(Ref(TestTypes.String.MakeArrayType()), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.Object));
        }

        [TestMethod]
        public void MergingThisWithItsPlainTypeGivesThePlainType()
        {
            Merge(VT.This(TestTypes.String), Ref(TestTypes.String)).Should().Be(Ref(TestTypes.String));
        }

        [TestMethod]
        public void MergingAFaultBlockExceptionClearsItAtItsHandler()
        {
            var states = new State[3];
            states[2] = new State(Runtime, states, 0, 1);
            states[2].PushType(VT.FaultBlockException(TestTypes.Throwable, 2));
            states[2]._changed = false;

            var result = State.FindCommonBaseType(Runtime, states, VT.FaultBlockException(TestTypes.Throwable, 2), Ref(TestTypes.Exception));

            result.Should().Be(Ref(TestTypes.Throwable));
            states[2].GetStackSlotEx(0).Should().Be(Ref(TestTypes.Throwable));
            states[2]._changed.Should().BeTrue("the handler must be analyzed again");
        }

        [TestMethod]
        public void MergingStatesWithDifferentStackHeightsFails()
        {
            var s1 = NewState();
            var s2 = NewState();
            s2.PushInt();
            FluentActions.Invoking(() => s1 + s2).Should().Throw<VerifyException>().WithMessage("Inconsistent stack height*");
        }

        [TestMethod]
        public void MergingIntoAnUninitializedStateCopiesTheOther()
        {
            var s2 = NewState();
            s2.PushInt();
            var s = default(State) + s2;
            s.GetStackHeight().Should().Be(1);
            s.GetStackSlotEx(0).Should().Be(VT.Int);
        }

        [TestMethod]
        public void MergingIdenticalStatesLeavesTheChangedFlag()
        {
            var s1 = NewState();
            s1.PushType(Ref(TestTypes.String));
            s1._changed = false;
            var s2 = s1.Copy();

            var s = s1 + s2;
            s._changed.Should().BeFalse();
            s.GetStackSlotEx(0).Should().Be(Ref(TestTypes.String));
        }

        [TestMethod]
        public void MergingStackReferencesGivesTheCommonBaseType()
        {
            var s1 = NewState();
            s1.PushType(Ref(TestTypes.Integer));
            s1._changed = false;
            var s2 = NewState();
            s2.PushType(Ref(TestTypes.Double));

            var s = s1 + s2;
            s._changed.Should().BeTrue();
            s.GetStackSlotEx(0).Should().Be(Ref(TestTypes.Number));
            s1.GetStackSlotEx(0).Should().Be(Ref(TestTypes.Integer), "merging does not modify its operands");
        }

        [TestMethod]
        public void MergingPlainAndExtendedPrecisionGivesExtendedPrecision()
        {
            foreach (var (plain, extended) in new[] { (VT.Float, VT.ExtendedFloat), (VT.Double, VT.ExtendedDouble) })
            {
                var p = NewState();
                p.PushType(plain);
                var e = NewState();
                e.PushType(extended);

                (p + e).GetStackSlotEx(0).Should().Be(extended);
                (e + p).GetStackSlotEx(0).Should().Be(extended);
            }
        }

        [TestMethod]
        public void MergingDifferentStackPrimitivesFails()
        {
            var s1 = NewState();
            s1.PushInt();
            var s2 = NewState();
            s2.PushFloat();
            FluentActions.Invoking(() => s1 + s2).Should().Throw<VerifyException>().WithMessage("cannot merge*");
        }

        [TestMethod]
        public void MergingAStackReferenceWithAPrimitiveFails()
        {
            var s1 = NewState();
            s1.PushType(Ref(TestTypes.String));
            var s2 = NewState();
            s2.PushInt();
            FluentActions.Invoking(() => s1 + s2).Should().Throw<VerifyException>().WithMessage("cannot merge*");
        }

        [TestMethod]
        public void MergingDifferentLocalsGivesTop()
        {
            var s1 = NewState();
            s1.SetLocalType(0, VT.Int, 0);
            s1.SetLocalType(1, Ref(TestTypes.Integer), 0);
            var s2 = NewState();
            s2.SetLocalType(0, VT.Float, 0);
            s2.SetLocalType(1, Ref(TestTypes.Double), 0);

            var s = s1 + s2;
            s.GetLocalType(0).Should().Be(VT.Top);
            s.GetLocalType(1).Should().Be(Ref(TestTypes.Number));
        }

        [TestMethod]
        public void MergingPropagatesUninitializedThis()
        {
            var s1 = NewState();
            s1._changed = false;
            var s2 = NewState();
            s2.SetUnitializedThis(true);

            var s = s1 + s2;
            s._changed.Should().BeTrue();
            FluentActions.Invoking(() => s.CheckUninitializedThis()).Should().Throw<VerifyException>();
            FluentActions.Invoking(() => s1.CheckUninitializedThis()).Should().NotThrow();
        }

        [TestMethod]
        public void PushingPastTheMaximumStackFails()
        {
            var s = NewState(maxStack: 2);
            s.PushInt();
            s.PushInt();
            FluentActions.Invoking(() => s.PushInt()).Should().Throw<VerifyException>().WithMessage("Stack overflow");
        }

        [TestMethod]
        public void Category2ValuesTakeTwoStackSlots()
        {
            var s = NewState(maxStack: 2);
            s.PushLong();
            s.GetStackHeight().Should().Be(1);
            FluentActions.Invoking(() => s.PushInt()).Should().Throw<VerifyException>().WithMessage("Stack overflow");

            s.PopLong();
            s.PushInt();
            s.PushInt();
            s.GetStackHeight().Should().Be(2);
        }

        [TestMethod]
        public void PoppingAnEmptyStackFails()
        {
            var s = NewState();
            FluentActions.Invoking(() => s.PopAnyType()).Should().Throw<VerifyException>().WithMessage("Unable to pop operand off an empty stack");
            FluentActions.Invoking(() => s.PeekType()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingHalfOfACategory2ValueFails()
        {
            var s = NewState();
            s.PushDouble();
            FluentActions.Invoking(() => s.PopType()).Should().Throw<VerifyException>().WithMessage("Attempt to split long or double on the stack");
        }

        [TestMethod]
        public void PoppingTheWrongPrimitiveFails()
        {
            var s = NewState();
            s.PushFloat();
            FluentActions.Invoking(() => s.PopInt()).Should().Throw<VerifyException>().WithMessage("Int expected on stack");
            s.PushInt();
            FluentActions.Invoking(() => s.PopFloat()).Should().Throw<VerifyException>().WithMessage("Float expected on stack");
            s.PushInt();
            FluentActions.Invoking(() => s.PopLong()).Should().Throw<VerifyException>().WithMessage("Long expected on stack");
            s.PushLong();
            FluentActions.Invoking(() => s.PopDouble()).Should().Throw<VerifyException>().WithMessage("Double expected on stack");
        }

        [TestMethod]
        public void PoppingAFloatReportsExtendedPrecision()
        {
            var s = NewState();
            s.PushFloat();
            s.PushExtendedFloat();
            s.PopFloat().Should().BeTrue();
            s.PopFloat().Should().BeFalse();

            s.PushDouble();
            s.PushExtendedDouble();
            s.PopDouble().Should().BeTrue();
            s.PopDouble().Should().BeFalse();
        }

        static VT PopAs(VT value, TestJavaType baseType, TestJvmTypeSystem typeSystem = default)
        {
            var s = NewState(typeSystem: typeSystem);
            s.PushType(value);
            return s.PopType(baseType);
        }

        [TestMethod]
        public void PoppingAPrimitiveAsItsTypeSucceeds()
        {
            PopAs(VT.Int, TestTypes.PrimitiveInt).Should().Be(VT.Int);
            PopAs(VT.Int, TestTypes.PrimitiveBoolean).Should().Be(VT.Int, "boolean is an int on the stack");
            PopAs(VT.Int, TestTypes.PrimitiveChar).Should().Be(VT.Int);
            PopAs(VT.Long, TestTypes.PrimitiveLong).Should().Be(VT.Long);
            PopAs(VT.Double, TestTypes.PrimitiveDouble).Should().Be(VT.Double);
        }

        [TestMethod]
        public void PoppingExtendedPrecisionAsThePlainTypeKeepsThePrecision()
        {
            PopAs(VT.ExtendedFloat, TestTypes.PrimitiveFloat).Should().Be(VT.ExtendedFloat);
            PopAs(VT.ExtendedDouble, TestTypes.PrimitiveDouble).Should().Be(VT.ExtendedDouble);
        }

        [TestMethod]
        public void PoppingAPrimitiveAsAnotherTypeFails()
        {
            FluentActions.Invoking(() => PopAs(VT.Float, TestTypes.PrimitiveDouble)).Should().Throw<VerifyException>().WithMessage("Unexpected type * where double was expected");
            FluentActions.Invoking(() => PopAs(VT.Int, TestTypes.Integer)).Should().Throw<VerifyException>();
            FluentActions.Invoking(() => PopAs(Ref(TestTypes.Integer), TestTypes.PrimitiveInt)).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingAReferenceAsASuperclassSucceeds()
        {
            PopAs(Ref(TestTypes.Integer), TestTypes.Number).Should().Be(Ref(TestTypes.Integer));
            PopAs(Ref(TestTypes.Integer), TestTypes.Object).Should().Be(Ref(TestTypes.Integer));
            PopAs(Ref(TestTypes.Integer.MakeArrayType()), TestTypes.Number.MakeArrayType()).Should().Be(Ref(TestTypes.Integer.MakeArrayType()));
            PopAs(VT.Null, TestTypes.String).Should().Be(VT.Null);
        }

        [TestMethod]
        public void PoppingAReferenceAsAnUnrelatedClassFails()
        {
            FluentActions.Invoking(() => PopAs(Ref(TestTypes.String), TestTypes.Number)).Should().Throw<VerifyException>().WithMessage("Unexpected type * where java.lang.Number was expected");
            FluentActions.Invoking(() => PopAs(Ref(TestTypes.Number), TestTypes.Integer)).Should().Throw<VerifyException>();
            FluentActions.Invoking(() => PopAs(Ref(TestTypes.PrimitiveInt.MakeArrayType()), TestTypes.PrimitiveLong.MakeArrayType())).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingAnyReferenceAsAnInterfaceSucceeds()
        {
            // the JVM leaves the check to invokeinterface
            PopAs(Ref(TestTypes.Number), TestTypes.Comparable).Should().Be(Ref(TestTypes.Number));
            PopAs(Ref(TestTypes.Number.MakeArrayType()), TestTypes.Comparable.MakeArrayType()).Should().Be(Ref(TestTypes.Number.MakeArrayType()));
        }

        [TestMethod]
        public void PoppingUnloadableTypesSucceeds()
        {
            PopAs(Ref(TestTypes.Missing), TestTypes.String).Should().Be(Ref(TestTypes.Missing));
            PopAs(Ref(TestTypes.String), TestTypes.Missing).Should().Be(Ref(TestTypes.String));
        }

        [TestMethod]
        public void PoppingATypeWithAnUnloadableBaseTypeSucceedsOnlyInTheImporter()
        {
            PopAs(Ref(TestTypes.ExtendsMissing), TestTypes.String, Importer).Should().Be(Ref(TestTypes.ExtendsMissing));
            FluentActions.Invoking(() => PopAs(Ref(TestTypes.ExtendsMissing), TestTypes.String, Runtime)).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingAnUninitializedObjectAsAnyTypeFails()
        {
            FluentActions.Invoking(() => PopAs(VT.Uninitialized(TestTypes.String, 0), TestTypes.String)).Should().Throw<VerifyException>().WithMessage("Expecting to find object/array on stack");
            FluentActions.Invoking(() => PopAs(VT.UninitializedThis, TestTypes.Object)).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingAnObjectAcceptsOnlyNullAndInitializedReferences()
        {
            var s = NewState();
            s.PushType(VT.Null);
            s.PopObjectType().Should().Be(VT.Null);
            s.PushType(Ref(TestTypes.String));
            s.PopObjectType().Should().Be(Ref(TestTypes.String));
            s.PushInt();
            FluentActions.Invoking(() => s.PopObjectType()).Should().Throw<VerifyException>().WithMessage("Expected object reference on stack");
            s.PushType(VT.Uninitialized(TestTypes.String, 0));
            FluentActions.Invoking(() => s.PopObjectType()).Should().Throw<VerifyException>();
        }

        [TestMethod]
        public void PoppingAnObjectOfABaseTypeChecksAssignability()
        {
            var s = NewState();
            s.PushType(Ref(TestTypes.Integer));
            s.PopObjectType(TestTypes.Number).Should().Be(Ref(TestTypes.Integer));
            s.PushType(Ref(TestTypes.Integer));
            FluentActions.Invoking(() => s.PopObjectType(TestTypes.String)).Should().Throw<VerifyException>().WithMessage("Unexpected type * where java.lang.String was expected");
            s.PushType(Ref(TestTypes.Number));
            s.PopObjectType(TestTypes.CharSequence).Should().Be(Ref(TestTypes.Number), "any reference is accepted for an interface");
            s.PushType(Ref(TestTypes.Missing));
            s.PopObjectType(TestTypes.String).Should().Be(Ref(TestTypes.Missing));
        }

        [TestMethod]
        public void PoppingAnArrayAcceptsArraysNullAndUnloadableTypes()
        {
            var s = NewState();
            s.PushType(Ref(TestTypes.String.MakeArrayType()));
            s.PopArrayType().Should().Be(Ref(TestTypes.String.MakeArrayType()));
            s.PushType(Ref(TestTypes.PrimitiveInt.MakeArrayType()));
            s.PopArrayType().Should().Be(Ref(TestTypes.PrimitiveInt.MakeArrayType()));
            s.PushType(VT.Null);
            s.PopArrayType().Should().Be(VT.Null);
            s.PushType(Ref(TestTypes.Missing));
            s.PopArrayType().Should().Be(Ref(TestTypes.Missing));
            s.PushType(Ref(TestTypes.String));
            FluentActions.Invoking(() => s.PopArrayType()).Should().Throw<VerifyException>().WithMessage("Array reference expected on stack");
        }

        [TestMethod]
        public void PoppingThisGivesAPlainReference()
        {
            var s = NewState();
            s.PushType(VT.This(TestTypes.String));
            s.PeekType().Should().Be(VT.This(TestTypes.String));
            s.PopAnyType().Should().Be(Ref(TestTypes.String));
        }

        [TestMethod]
        public void PoppingAFaultBlockExceptionClearsItAtItsHandler()
        {
            var states = new State[2];
            states[1] = new State(Runtime, states, 0, 1);
            states[1].PushType(VT.FaultBlockException(TestTypes.Throwable, 1));

            var s = new State(Runtime, states, 0, 1);
            s.PushType(VT.FaultBlockException(TestTypes.Throwable, 1));
            s.PopAnyType().Should().Be(Ref(TestTypes.Throwable));
            states[1].GetStackSlotEx(0).Should().Be(Ref(TestTypes.Throwable));
        }

        [TestMethod]
        public void PoppingAFaultBlockExceptionDirectlyLeavesItsHandler()
        {
            var states = new State[2];
            states[1] = new State(Runtime, states, 0, 1);
            states[1].PushType(VT.FaultBlockException(TestTypes.Throwable, 1));

            var s = states[1].Copy();
            s.PopFaultBlockException().Should().Be(VT.FaultBlockException(TestTypes.Throwable, 1));
            states[1].GetStackSlotEx(0).IsFaultBlockException.Should().BeTrue();
        }

        [TestMethod]
        public void PushingARuntimeTypeConvertsItToAVerificationType()
        {
            var s = NewState(maxStack: 8);
            s.PushType(TestTypes.PrimitiveByte);
            s.PushType(TestTypes.PrimitiveFloat);
            s.PushType(TestTypes.VerifierNull);
            s.PushType(TestTypes.String);
            s.PopAnyType().Should().Be(Ref(TestTypes.String));
            s.PopAnyType().Should().Be(VT.Null);
            s.PopAnyType().Should().Be(VT.Float);
            s.PopAnyType().Should().Be(VT.Int);
        }

        [TestMethod]
        public void GetVerificationTypeMapsEveryKindOfType()
        {
            State.GetVerificationType(Runtime, TestTypes.PrimitiveBoolean).Should().Be(VT.Int);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveByte).Should().Be(VT.Int);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveChar).Should().Be(VT.Int);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveShort).Should().Be(VT.Int);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveInt).Should().Be(VT.Int);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveLong).Should().Be(VT.Long);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveFloat).Should().Be(VT.Float);
            State.GetVerificationType(Runtime, TestTypes.PrimitiveDouble).Should().Be(VT.Double);
            State.GetVerificationType(Runtime, TestTypes.VerifierNull).Should().Be(VT.Null);
            State.GetVerificationType(Runtime, TestTypes.String).Should().Be(Ref(TestTypes.String));
            State.GetVerificationType(Runtime, TestTypes.Missing).Should().Be(Ref(TestTypes.Missing));
        }

        [TestMethod]
        public void GetVerificationTypeRejectsVoid()
        {
            FluentActions.Invoking(() => State.GetVerificationType(Runtime, TestTypes.PrimitiveVoid)).Should().Throw<ArgumentException>();
            FluentActions.Invoking(() => State.GetVerificationType(Runtime, null!)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Category2LocalsTakeTwoSlots()
        {
            var s = NewState();
            s.SetLocalType(0, VT.Int, 0);
            s.SetLocalType(1, VT.Int, 0);
            s.SetLocalLong(0, 0);
            s.GetLocalType(0).Should().Be(VT.Long);
            s.GetLocalType(1).Should().Be(VT.Top);
            s.GetLocalLong(0);
        }

        [TestMethod]
        public void OverwritingTheSecondHalfOfACategory2LocalInvalidatesIt()
        {
            var s = NewState();
            s.SetLocalDouble(0, 0);
            s.SetLocalInt(1, 0);
            s.GetLocalType(0).Should().Be(VT.Top);
            s.GetLocalType(1).Should().Be(VT.Int);

            s.SetLocalLong(1, 0);
            s.SetLocalLong(2, 0);
            s.GetLocalType(1).Should().Be(VT.Top);
            s.GetLocalType(2).Should().Be(VT.Long);
        }

        [TestMethod]
        public void AccessingALocalOutOfRangeFails()
        {
            var s = NewState(maxLocals: 2);
            FluentActions.Invoking(() => s.GetLocalType(2)).Should().Throw<VerifyException>().WithMessage("Illegal local variable number");
            FluentActions.Invoking(() => s.SetLocalInt(2, 0)).Should().Throw<VerifyException>().WithMessage("Illegal local variable number");
            FluentActions.Invoking(() => s.SetLocalLong(1, 0)).Should().Throw<VerifyException>().WithMessage("Illegal local variable number");
        }

        [TestMethod]
        public void ReadingALocalOfTheWrongTypeFails()
        {
            var s = NewState();
            s.SetLocalFloat(0, 0);
            FluentActions.Invoking(() => s.GetLocalInt(0)).Should().Throw<VerifyException>();
            FluentActions.Invoking(() => s.GetLocalDouble(0)).Should().Throw<VerifyException>();
            FluentActions.Invoking(() => s.GetLocalLong(0)).Should().Throw<VerifyException>();
            s.GetLocalFloat(0);
        }

        [TestMethod]
        public void CopiesDoNotShareModifications()
        {
            var s = NewState();
            s.PushInt();
            s.SetLocalInt(0, 0);

            var copy = s.Copy();
            copy.PopInt();
            copy.PushFloat();
            copy.SetLocalFloat(0, 0);

            s.GetStackSlotEx(0).Should().Be(VT.Int);
            s.GetLocalType(0).Should().Be(VT.Int);
            copy.GetStackSlotEx(0).Should().Be(VT.Float);
            copy.GetLocalType(0).Should().Be(VT.Float);
        }

        [TestMethod]
        public void CopyingLocalsGivesAnEmptyStack()
        {
            var s = NewState();
            s.PushInt();
            s.SetLocalInt(0, 0);

            var copy = s.CopyLocals();
            copy.GetStackHeight().Should().Be(0);
            copy.GetLocalType(0).Should().Be(VT.Int);
            copy.SetLocalFloat(0, 0);
            s.GetLocalType(0).Should().Be(VT.Int);
        }

        [TestMethod]
        public void MarkingInitializedReplacesEveryCopyOfTheUninitializedObject()
        {
            var uninitialized = VT.Uninitialized(TestTypes.String, 7);
            var other = VT.Uninitialized(TestTypes.String, 9);
            var s = NewState();
            s.PushType(uninitialized);
            s.PushType(uninitialized);
            s.PushType(other);
            s.SetLocalType(0, uninitialized, 0);
            s.SetLocalType(1, other, 0);

            var original = s;
            s = s.Copy();
            s.MarkInitialized(uninitialized, Ref(TestTypes.String), 0);

            s.GetStackByIndex(0).Should().Be(Ref(TestTypes.String));
            s.GetStackByIndex(1).Should().Be(Ref(TestTypes.String));
            s.GetStackByIndex(2).Should().Be(other);
            s.GetLocalType(0).Should().Be(Ref(TestTypes.String));
            s.GetLocalType(1).Should().Be(other);
            original.GetStackByIndex(0).Should().Be(uninitialized, "a copy does not modify the state it was copied from");
            original.GetLocalType(0).Should().Be(uninitialized);
        }

        [TestMethod]
        public void StackSlotsStripExtendedPrecision()
        {
            var s = NewState();
            s.PushExtendedDouble();
            s.GetStackSlot(0).Should().Be(VT.Double);
            s.GetStackSlotEx(0).Should().Be(VT.ExtendedDouble);
        }

    }

}
