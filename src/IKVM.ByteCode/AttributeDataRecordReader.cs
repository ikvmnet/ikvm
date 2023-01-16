using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Provides methods to read <see cref="AttributeDataRecord"/> clases from an underlying <see cref="AttributeInfo"/>.
    /// </summary>
    static class AttributeDataRecordReader
    {

        /// <summary>
        /// Attempts to read an attribute data record from the specified <see cref="AttributeInfo"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool TryReadAttribute(AttributeInfo info, out AttributeDataRecord attribute)
        {
            return TryReadAttribute(info.Name, new ReadOnlySequence<byte>(info.Data), out attribute);
        }

        /// <summary>
        /// Attempts to read an attribute data record from the specified <see cref="AttributeInfo"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool TryReadAttribute(string name, ReadOnlySequence<byte> data, out AttributeDataRecord attribute)
        {
            var reader = new SequenceReader<byte>(data);
            if (TryReadAttribute(name, ref reader, out attribute) == false)
                return false;

            return true;
        }

        /// <summary>
        /// Attempts to read a class record starting at the current position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reader"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="ClassReaderException"></exception>
        public static bool TryReadAttribute(string name, ref SequenceReader<byte> reader, out AttributeDataRecord attribute) => name switch
        {
            "ConstantValue" => TryReadConstantValueAttribute(ref reader, out attribute),
            "Code" => TryReadCodeAttribute(ref reader, out attribute),
            "StackMapTable" => TryReadStackMapTableAttribute(ref reader, out attribute),
            "Exceptions" => TryReadExceptionsAttribute(ref reader, out attribute),
            "InnerClasses" => TryReadInnerClassesAttribute(ref reader, out attribute),
            "EnclosingMethod" => TryReadEnclosingMethodAttribute(ref reader, out attribute),
            "Synthetic" => TryReadSyntheticAttribute(ref reader, out attribute),
            "Signature" => TryReadSignatureAttribute(ref reader, out attribute),
            "SourceFile" => TryReadSourceFileAttribute(ref reader, out attribute),
            "SourceDebugExtension" => TryReadSourceDebugExtensionAttribute(ref reader, out attribute),
            "LineNumberTable" => TryReadLineNumberTableAttribute(ref reader, out attribute),
            "LocalVariableTable" => TryReadLocalVariableTableAttribute(ref reader, out attribute),
            "LocalVariableTypeTable" => TryReadLocalVariableTypeTableAttribute(ref reader, out attribute),
            "Deprecated" => TryReadDeprecatedAttribute(ref reader, out attribute),
            "RuntimeVisibleAnnotations" => TryReadRuntimeVisibleAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleAnnotations" => TryReadRuntimeInvisibleAnnotationsAttribute(ref reader, out attribute),
            "RuntimeVisibleParameterAnnotations" => TryReadRuntimeVisibleParameterAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleParameterAnnotations" => TryReadRuntimeInvisibleParameterAnnotationsAttribute(ref reader, out attribute),
            "RuntimeVisibleTypeAnnotations" => TryReadRuntimeVisibleTypeAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleTypeAnnotations" => TryReadRuntimeInvisibleTypeAnnotationsAttribute(ref reader, out attribute),
            "AnnotationDefault" => TryReadAnnotationDefaultAttribute(ref reader, out attribute),
            "BootstrapMethods" => TryReadBootstrapMethodsAttribute(ref reader, out attribute),
            "MethodParameters" => TryReadMethodParametersAttribute(ref reader, out attribute),
            "Module" => TryReadModuleAttribute(ref reader, out attribute),
            "ModulePackages" => TryReadModulePackagesAttribute(ref reader, out attribute),
            "ModuleMainClass" => TryReadModuleMainClassAttribute(ref reader, out attribute),
            "NestHost" => TryReadNestHostAttribute(ref reader, out attribute),
            "NestMembers" => TryReadNestMembersAttribute(ref reader, out attribute),
            "Record" => TryReadRecordAttribute(ref reader, out attribute),
            "PermittedSubclasses" => TryReadPermittedSubclassesAttribute(ref reader, out attribute),
            _ => TryReadCustomAttribute(ref reader, out attribute),
        };

        static bool TryReadConstantValueAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort valueIndex) == false)
                return false;

            attribute = new ConstantValueAttributeDataRecord(valueIndex);
            return true;
        }

        static unsafe bool TryReadCodeAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort maxStack) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort maxLocals) == false)
                return false;
            if (reader.TryReadBigEndian(out uint codeLength) == false)
                return false;
            if (reader.TryReadExact((int)codeLength, out ReadOnlySequence<byte> code) == false)
                return false;

            var codeBuffer = new byte[code.Length];
            code.CopyTo(codeBuffer);

            if (reader.TryReadBigEndian(out uint exceptionTableLength) == false)
                return false;

            var exceptionTable = new ExceptionHandlerRecord[(int)exceptionTableLength];
            for (int i = 0; i < exceptionTableLength; i++)
            {
                if (reader.TryReadBigEndian(out ushort startOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort endOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort handlerOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort catchTypeIndex) == false)
                    return false;

                exceptionTable[i] = new ExceptionHandlerRecord(startOffset, endOffset, handlerOffset, catchTypeIndex);
            }

            if (ClassRecordReader.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            attribute = new CodeAttributeDataRecord(maxStack, maxLocals, codeBuffer, exceptionTable, attributes);
            return true;
        }

        static unsafe bool TryReadStackMapTableAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out uint entryCount) == false)
                return false;

            var entries = new StackMapFrameRecord[(int)entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                if (reader.TryRead(out byte tag) == false)
                    return false;

                if (TryReadStackFrame(ref reader, tag, out var frame) == false)
                    return false;

                entries[i] = frame;
            }

            if (ClassRecordReader.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            attribute = new StackMapTableAttributeDataRecord(entries);
            return true;
        }

        static bool TryReadStackFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            if (tag >= 0 && tag <= 63)
                return TryReadSameStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 64 && tag <= 127)
                return TryReadSameLocalsOneStackItemStackMapFrame(ref reader, tag, out frame);
            else if (tag == 247)
                return TryReadSameLocalsOneStackItemExtendedStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 248 && tag <= 250)
                return TryReadChopStackMapFrame(ref reader, tag, out frame);
            else if (tag == 251)
                return TryReadSameExtendedStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 252 && tag <= 254)
                return TryReadAppendStackMapFrame(ref reader, tag, out frame);
            else if (tag == 255)
                return TryReadFullStackMapFrame(ref reader, tag, out frame);
            else
                throw new ClassReaderException($"Invalid stack map frame tag value: '{tag}'.");
        }

        static bool TryReadSameStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = new SameStackMapFrameRecord(tag);
            return true;
        }

        static bool TryReadSameLocalsOneStackItemStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (TryReadVerificationTypeInfo(ref reader, out var verificationTypeInfo) == false)
                return false;

            frame = new SameLocalsOneStackMapFrameRecord(tag, verificationTypeInfo);
            return true;
        }

        static bool TryReadSameLocalsOneStackItemExtendedStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;
            if (TryReadVerificationTypeInfo(ref reader, out var verificationTypeInfo) == false)
                return false;

            frame = new SameLocalsOneExtendedStackMapFrameRecord(tag, offsetDelta, verificationTypeInfo);
            return true;
        }

        static bool TryReadChopStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            frame = new ChopStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

        static bool TryReadSameExtendedStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            frame = new SameExtendedStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

        static bool TryReadAppendStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            var locals = new VerificationTypeInfoRecord[tag - 251];
            for (int i = 0; i < tag - 251; i++)
            {
                if (TryReadVerificationTypeInfo(ref reader, out var local) == false)
                    return false;

                locals[i] = local;
            }

            frame = new AppendStackMapFrameRecord(tag, offsetDelta, locals);
            return true;
        }

        static bool TryReadFullStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort localsCount) == false)
                return false;

            var locals = new VerificationTypeInfoRecord[localsCount];
            for (int i = 0; i < localsCount; i++)
            {
                if (TryReadVerificationTypeInfo(ref reader, out var j) == false)
                    return false;

                locals[i] = j;
            }

            if (reader.TryReadBigEndian(out ushort stackCount) == false)
                return false;

            var stack = new VerificationTypeInfoRecord[stackCount];
            for (int i = 0; i < stackCount; i++)
            {
                if (TryReadVerificationTypeInfo(ref reader, out var j) == false)
                    return false;

                stack[i] = j;
            }

            frame = new FullStackMapFrameRecord(tag, offsetDelta, locals, stack);
            return true;
        }

        static bool TryReadVerificationTypeInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort tag) == false)
                return false;

            switch (tag)
            {
                case 0:
                    return TryReadTopVariableInfo(ref reader, out record);
                case 1:
                    return TryReadIntegerVariableInfo(ref reader, out record);
                case 2:
                    return TryReadFloatVariableInfo(ref reader, out record);
                case 3:
                    return TryReadDoubleVariableInfo(ref reader, out record);
                case 4:
                    return TryReadLongVariableInfo(ref reader, out record);
                case 5:
                    return TryReadNullVariableInfo(ref reader, out record);
                case 6:
                    return TryReadUnitializedThisVariableInfo(ref reader, out record);
                case 7:
                    return TryReadObjectVariableInfo(ref reader, out record);
                case 8:
                    return TryReadUnitializedVariableInfo(ref reader, out record);
                default:
                    throw new ClassReaderException($"Invalid verification info tag: '{tag}'.");
            }
        }

        static bool TryReadTopVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new TopVariableInfoRecord();
            return true;
        }

        static bool TryReadIntegerVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new IntegerVariableInfoRecord();
            return true;
        }

        static bool TryReadFloatVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new FloatVariableInfoRecord();
            return true;
        }

        static bool TryReadDoubleVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new DoubleVariableInfoRecord();
            return true;
        }

        static bool TryReadLongVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new LongVariableInfoRecord();
            return true;
        }

        static bool TryReadNullVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new NullVariableInfoRecord();
            return true;
        }

        static bool TryReadUnitializedThisVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new UninitalizedThisVariableInfoRecord();
            return true;
        }

        static bool TryReadObjectVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            record = new ObjectVariableInfoRecord(classIndex);
            return true;
        }

        static bool TryReadUnitializedVariableInfo(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort offset) == false)
                return false;

            record = new UninitializedVariableInfo(offset);
            return true;
        }

        static bool TryReadExceptionsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var entries = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort index) == false)
                    return false;

                entries[i] = index;
            }

            attribute = new ExceptionsAttributeDataRecord(entries);
            return true;
        }

        static bool TryReadInnerClassesAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var entries = new InnerClassRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort inner_class_info_index) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort outer_class_info_index) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort inner_name_index) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort inner_class_access_flags) == false)
                    return false;

                entries[i] = new InnerClassRecord(inner_class_info_index, outer_class_info_index, inner_name_index, (AccessFlag)inner_class_access_flags);
            }

            attribute = new InnerClassesAttributeDataRecord(entries);
            return true;
        }

        static bool TryReadEnclosingMethodAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort methodIndex) == false)
                return false;

            attribute = new EnclosingMethodAttributeDataRecord(classIndex, methodIndex);
            return true;
        }

        static bool TryReadSyntheticAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = new SyntheticAttributeDataRecord();
            return true;
        }

        static bool TryReadSignatureAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort signatureIndex) == false)
                return false;

            attribute = new SignatureAttributeDataRecord(signatureIndex);
            return true;
        }

        static bool TryReadSourceFileAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort sourceFileIndex) == false)
                return false;

            attribute = new SourceFileAttributeDataRecord(sourceFileIndex);
            return true;
        }

        static bool TryReadSourceDebugExtensionAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadExact((int)reader.Length, out ReadOnlySequence<byte> data) == false)
                return false;

            var dataBuffer = new byte[data.Length];
            data.CopyTo(dataBuffer);

            attribute = new SourceDebugExtensionAttributeDataRecord(dataBuffer);
            return true;
        }

        static bool TryReadLineNumberTableAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort itemCount) == false)
                return false;

            var items = new LineNumberTableAttributeDataRecordItem[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort lineNumber) == false)
                    return false;

                items[i] = new LineNumberTableAttributeDataRecordItem(codeOffset, lineNumber);
            }

            attribute = new LineNumberTableAttributeDataRecord(items);
            return true;
        }

        static bool TryReadLocalVariableTableAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var items = new LocalVariableTableAttributeDataRecordItem[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryReadBigEndian(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort codeLength) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort index) == false)
                    return false;

                items[i] = new LocalVariableTableAttributeDataRecordItem(codeOffset, codeLength, nameIndex, descriptorIndex, index);
            }

            attribute = new LocalVariableTableAttributeDataRecord(items);
            return true;
        }

        static bool TryReadLocalVariableTypeTableAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var items = new LocalVariableTypeTableAttributeDataRecordItem[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryReadBigEndian(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort codeLength) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort signatureIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort index) == false)
                    return false;

                items[i] = new LocalVariableTypeTableAttributeDataRecordItem(codeOffset, codeLength, nameIndex, signatureIndex, index);
            }

            attribute = new LocalVariableTypeTableAttributeDataRecord(items);
            return true;
        }

        static bool TryReadDeprecatedAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = new DeprecatedAttributeDataRecord();
            return true;
        }

        static bool TryReadAnnotation(ref SequenceReader<byte> reader, out AnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryReadBigEndian(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (TryReadElementValue(ref reader, out var elementValue) == false)
                    return false;

                elements[i] = new ElementValuePairRecord(nameIndex, elementValue);
            }

            annotation = new AnnotationRecord(typeIndex, elements);
            return true;
        }

        static bool TryReadElementValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryRead(out byte tag) == false)
                return false;

            return (char)tag switch
            {
                'B' or 'C' or 'D' or 'F' or 'I' or 'J' or 'S' or 'Z' or 's' => TryReadElementConstantValue(ref reader, out value),
                'e' => TryReadElementEnumConstantValue(ref reader, out value),
                'c' => TryReadElementClassInfoValue(ref reader, out value),
                '@' => TryReadElementAnnotationValue(ref reader, out value),
                '[' => TryReadElementArrayValue(ref reader, out value),
                _ => throw new ClassReaderException($"Invalid annotation element value tag: '{(char)tag}'."),
            };
        }

        static bool TryReadElementConstantValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort index) == false)
                return false;

            value = new ElementConstantValueRecord(index);
            return true;
        }

        static bool TryReadElementEnumConstantValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort typeNameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort constantNameIndex) == false)
                return false;

            value = new ElementEnumConstantValueRecord(typeNameIndex, constantNameIndex);
            return true;
        }

        static bool TryReadElementClassInfoValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort classInfoIndex) == false)
                return false;

            value = new ElementClassInfoValueRecord(classInfoIndex);
            return true;
        }

        static bool TryReadElementAnnotationValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (TryReadAnnotation(ref reader, out var annotation) == false)
                return false;

            value = new ElementAnnotationValueRecord(annotation);
            return true;
        }

        static bool TryReadElementArrayValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryRead(out ushort length) == false)
                return false;

            var values = new ElementValueRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (TryReadElementValue(ref reader, out var j) == false)
                    return false;

                values[i] = j;
            }

            value = new ElementArrayValueRecord(values);
            return true;
        }

        static bool TryReadTypeAnnotation(ref SequenceReader<byte> reader, out TypeAnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryRead(out byte targetType) == false)
                return false;
            if (TryReadTypeAnnotationTarget(ref reader, targetType, out var target) == false)
                return false;
            if (TryReadTypePath(ref reader, out var targetPath) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (TryReadElementValue(ref reader, out var elementValue) == false)
                    return false;

                elements[i] = new ElementValuePairRecord(nameIndex, elementValue);
            }

            annotation = new TypeAnnotationRecord(target, targetPath, typeIndex, elements);
            return true;
        }

        static bool TryReadTypePath(ref SequenceReader<byte> reader, out TypePathRecord typePath)
        {
            typePath = default;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var path = new TypePathItemRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryRead(out byte kind) == false)
                    return false;
                if (reader.TryRead(out byte argumentIndex) == false)
                    return false;

                path[i] = new TypePathItemRecord((TypePathKind)kind, argumentIndex);
            }

            typePath = new TypePathRecord(path);
            return true;
        }

        static bool TryReadTypeAnnotationTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            return targetType switch
            {
                0x00 or 0x01 => TryReadTypeAnnotationParameterTarget(ref reader, targetType, out targetInfo),
                0x10 => TryReadTypeAnnotationSuperTypeTarget(ref reader, targetType, out targetInfo),
                0x11 or 0x12 => TryReadTypeAnnotationParameterBoundTarget(ref reader, targetType, out targetInfo),
                0x13 or 0x14 or 0x15 => TryReadTypeAnnotationEmptyTarget(ref reader, targetType, out targetInfo),
                0x16 => TryReadTypeAnnotationFormalParameterTarget(ref reader, targetType, out targetInfo),
                0x17 => TryReadTypeAnnotationThrowsTarget(ref reader, targetType, out targetInfo),
                _ => throw new ClassReaderException($"Invalid type annotation target type: '{targetType}'."),
            };
        }

        static bool TryReadTypeAnnotationParameterTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterTargetRecord(targetType, parameterIndex);
            return true;
        }

        static bool TryReadTypeAnnotationSuperTypeTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadBigEndian(out ushort superTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationSuperTypeTargetRecord(targetType, superTypeIndex);
            return true;
        }

        static bool TryReadTypeAnnotationParameterBoundTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;
            if (reader.TryRead(out byte boundIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterBoundTargetRecord(targetType, parameterIndex, boundIndex);
            return true;
        }

        static bool TryReadTypeAnnotationEmptyTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = new TypeAnnotationEmptyTargetRecord(targetType);
            return true;
        }

        static bool TryReadTypeAnnotationFormalParameterTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;

            targetInfo = new TypeAnnotationFormalParameterTargetRecord(targetType, parameterIndex);
            return true;
        }

        static bool TryReadTypeAnnotationThrowsTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadBigEndian(out ushort throwsTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationThrowsTargetRecord(targetType, throwsTypeIndex);
            return true;
        }

        static bool TryReadRuntimeVisibleAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeVisibleAnnotationsAttributeDataRecord(items);
            return true;
        }

        static bool TryReadRuntimeInvisibleAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeInvisibleAnnotationsAttributeDataRecord(items);
            return true;
        }

        static bool TryReadRuntimeVisibleParameterAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeVisibleParameterAnnotationsAttributeDataRecord(items);
            return true;
        }

        static bool TryReadRuntimeInvisibleParameterAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeInvisibleParameterAnnotationsAttributeDataRecord(items);
            return true;
        }

        static bool TryReadRuntimeVisibleTypeAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var annotations = new TypeAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadTypeAnnotation(ref reader, out var annotation) == false)
                    return false;

                annotations[i] = annotation;
            }

            attribute = new RuntimeVisibleTypeAnnotationsAttributeDataRecord(annotations);
            return true;
        }

        static bool TryReadRuntimeInvisibleTypeAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var annotations = new TypeAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadTypeAnnotation(ref reader, out var annotation) == false)
                    return false;

                annotations[i] = annotation;
            }

            attribute = new RuntimeInvisibleTypeAnnotationsAttributeDataRecord(annotations);
            return true;
        }

        static bool TryReadAnnotationDefaultAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (TryReadElementValue(ref reader, out var defaultValue) == false)
                return false;

            attribute = new AnnotationDefaultAttributeDataRecord(defaultValue);
            return true;
        }

        static bool TryReadBootstrapMethodsAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var methods = new BootstrapMethodRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TryReadBootstrapMethod(ref reader, out var method) == false)
                    return false;

                methods[i] = method;
            }

            attribute = new BootstrapMethodsAttributeDataRecord(methods);
            return true;
        }

        static bool TryReadBootstrapMethod(ref SequenceReader<byte> reader, out BootstrapMethodRecord method)
        {
            method = default;

            if (reader.TryReadBigEndian(out ushort methodRefIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort argumentCount) == false)
                return false;

            var arguments = new ushort[argumentCount];
            for (int i = 0; i < argumentCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort argumentIndex) == false)
                    return false;

                arguments[i] = argumentIndex;
            }

            method = new BootstrapMethodRecord(methodRefIndex, arguments);
            return true;
        }

        static bool TryReadMethodParametersAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var arguments = new MethodParametersAttributeDataParameterRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                    return false;

                arguments[i] = new MethodParametersAttributeDataParameterRecord(nameIndex, (AccessFlag)accessFlags);
            }

            attribute = new MethodParametersAttributeDataRecord(arguments);
            return true;
        }

        static bool TryReadModuleAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort moduleNameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort moduleFlags) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort moduleVersionIndex) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort requiresCount) == false)
                return false;

            var requires = new ModuleAttributeDataRequiresRecord[requiresCount];
            for (int i = 0; i < requiresCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort requiresIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort requiresFlags) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort requiresVersionIndex) == false)
                    return false;

                requires[i] = new ModuleAttributeDataRequiresRecord(requiresIndex, (ModuleRequiresFlag)requiresFlags, requiresVersionIndex);
            }

            if (reader.TryReadBigEndian(out ushort exportsCount) == false)
                return false;

            var exports = new ModuleAttributeDataExportsRecord[exportsCount];
            for (int i = 0; i < exportsCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort exportsIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort exportsFlags) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort exportsModuleCount) == false)
                    return false;

                var exportsModules = new ushort[exportsModuleCount];
                for (int j = 0; j < exportsModuleCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort exportsToModuleIndex) == false)
                        return false;

                    exportsModules[j] = exportsToModuleIndex;
                }

                exports[i] = new ModuleAttributeDataExportsRecord(exportsIndex, (ModuleExportsFlag)exportsFlags, exportsModules);
            }

            if (reader.TryReadBigEndian(out ushort opensCount) == false)
                return false;

            var opens = new ModuleAttributeDataOpensRecord[opensCount];
            for (int i = 0; i < opensCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort opensIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort opensFlags) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort opensModulesCount) == false)
                    return false;

                var opensModules = new ushort[opensModulesCount];
                for (int j = 0; j < opensModulesCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort opensModuleIndex) == false)
                        return false;

                    opensModules[j] = opensModuleIndex;
                }

                opens[i] = new ModuleAttributeDataOpensRecord(opensIndex, (ModuleOpensFlag)opensFlags, opensModules);
            }

            if (reader.TryReadBigEndian(out ushort usesCount) == false)
                return false;

            var uses = new ushort[usesCount];
            for (int i = 0; i < usesCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort usesIndex) == false)
                    return false;

                uses[i] = usesIndex;
            }

            if (reader.TryReadBigEndian(out ushort providesCount) == false)
                return false;

            var provides = new ModuleAttributeDataProvidesRecord[providesCount];
            for (int i = 0; i < providesCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort providesIndex) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort providesModulesCount) == false)
                    return false;

                var providesModules = new ushort[providesModulesCount];
                for (int j = 0; j < providesModulesCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort providesModuleIndex) == false)
                        return false;

                    providesModules[j] = providesModuleIndex;
                }

                provides[i] = new ModuleAttributeDataProvidesRecord(providesIndex, providesModules);
            }

            attribute = new ModuleAttributeDataRecord(moduleNameIndex, (ModuleFlag)moduleFlags, moduleVersionIndex, requires, exports, opens, uses, provides);
            return true;
        }

        static bool TryReadModulePackagesAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var packages = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort packageIndex) == false)
                    return false;

                packages[i] = packageIndex;
            }

            attribute = new ModulePackagesAttributeDataRecord(packages);
            return true;
        }

        static bool TryReadModuleMainClassAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort mainClassIndex) == false)
                return false;

            attribute = new ModuleMainClassAttributeDataRecord(mainClassIndex);
            return true;
        }

        static bool TryReadNestHostAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort hostClassIndex) == false)
                return false;

            attribute = new NestHostAttributeDataRecord(hostClassIndex);
            return true;
        }

        static bool TryReadNestMembersAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var classes = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort classIndex) == false)
                    return false;

                classes[i] = classIndex;
            }

            attribute = new NestMembersAttributeDataRecord(classes);
            return true;
        }

        static bool TryReadRecordAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort componentsCount) == false)
                return false;

            var components = new RecordAttributeDataComponentRecord[componentsCount];
            for (int i = 0; i < componentsCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                    return false;
                if (ClassRecordReader.TryReadAttributes(ref reader, out var attributes) == false)
                    return false;

                components[i] = new RecordAttributeDataComponentRecord(nameIndex, descriptorIndex, attributes);
            }

            attribute = new RecordAttributeDataRecord(components);
            return true;
        }

        static bool TryReadPermittedSubclassesAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var classes = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort classIndex) == false)
                    return false;

                classes[i] = classIndex;
            }

            attribute = new PermittedSubclassesAttributeDataRecord(classes);
            return true;
        }

        static bool TryReadCustomAttribute(ref SequenceReader<byte> reader, out AttributeDataRecord attribute)
        {
            var data = new byte[reader.Length];
            reader.TryCopyTo(data);

            attribute = new CustomAttributeDataRecord(data);
            return true;
        }

    }

}
