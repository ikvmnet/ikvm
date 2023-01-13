using System;
using System.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Allows a consumer to accept the structure of a class file.
    /// </summary>
    public interface IClassParserHandler : IClassParserAttributeHandler
    {

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the major and minor versions of the class file.
        /// </summary>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        void AcceptVersion(ushort majorVersion, ushort minorVersion);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the constant pool count.
        /// </summary>
        /// <param name="count"></param>
        void AcceptConstantCount(ushort count);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a UTF8 string in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void AcceptUtf8Constant(int index, in ReadOnlySequence<byte> value);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters an Integer in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void AcceptIntegerConstant(int index, int value);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Float in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void AcceptFloatConstant(int index, float value);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Long in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void AcceptLongConstant(int index, long value);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Double in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void AcceptDoubleConstant(int index, double value);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Class in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="utf8Index"></param>
        void AcceptClassConstant(int index, ushort utf8Index);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a String in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="utf8Index"></param>
        void AcceptStringConstant(int index, ushort utf8Index);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Fieldref in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="classIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        void AcceptFieldrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Methodref in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="classIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        void AcceptMethodrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a InterfaceMethodref in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="classIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        void AcceptInterfaceMethodrefConstant(int index, ushort classIndex, ushort nameAndTypeIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a NameAndType in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nameIndex"></param>
        /// <param name="descriptorIndex"></param>
        void AcceptNameAndTypeConstant(int index, ushort nameIndex, ushort descriptorIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a MethodHandle in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="kind"></param>
        /// <param name="referenceIndex"></param>
        void AcceptMethodHandleConstant(int index, ReferenceKind kind, ushort referenceIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a MethodType in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="descriptorIndex"></param>
        void AcceptMethodTypeConstant(int index, ushort descriptorIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Dynamic in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bootstrapMethodAttrIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        void AcceptDynamicConstant(int index, ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a InvokeDynamic in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bootstrapMethodAttrIndex"></param>
        /// <param name="nameAndTypeIndex"></param>
        void AcceptInvokeDynamicConstant(int index, ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Module in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nameIndex"></param>
        void AcceptModuleConstant(int index, ushort nameIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a Package in the constant pool.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nameIndex"></param>
        void AcceptPackageConstant(int index, ushort nameIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the AccessFlags field.
        /// </summary>
        /// <param name="accessFlags"></param>
        void AcceptAccessFlags(AccessFlag accessFlags);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the ThisClass field.
        /// </summary>
        /// <param name="thisClass"></param>
        void AcceptThisClass(ushort thisClass);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the SuperClass field.
        /// </summary>
        /// <param name="superClass"></param>
        void AcceptSuperClass(ushort superClass);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the interface count.
        /// </summary>
        /// <param name="count"></param>
        void AcceptInterfaceCount(ushort count);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters an interface.
        /// </summary>
        /// <param name="classIndex"></param>
        void AcceptInterface(ushort classIndex);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the field count.
        /// </summary>
        /// <param name="count"></param>
        void AcceptFieldCount(ushort count);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a field. Set the <see cref="IClassParserAttributeHandler"/> to accept the attributes for this field.
        /// </summary>
        /// <param name="accessFlags"></param>
        /// <param name="nameIndex"></param>
        /// <param name="descriptorIndex"></param>
        /// <param name="attributes"></param>
        void AcceptField(AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex, out IClassParserAttributeHandler attributes);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters the method count.
        /// </summary>
        /// <param name="count"></param>
        void AcceptMethodCount(ushort count);

        /// <summary>
        /// Invoked when the <see cref="ClassParser"/> encounters a method. Set the <see cref="IClassParserAttributeHandler"/> to accept the attributes for this method.
        /// </summary>
        /// <param name="accessFlags"></param>
        /// <param name="nameIndex"></param>
        /// <param name="descriptorIndex"></param>
        /// <param name="attributes"></param>
        void AcceptMethod(AccessFlag accessFlags, ushort nameIndex, ushort descriptorIndex, out IClassParserAttributeHandler attributes);

    }

}
