namespace IKVM.ByteCode.Parsing
{

    public record struct TypeAnnotationRecord(TypeAnnotationTargetType TargetType, TypeAnnotationTargetRecord Target, TypePathRecord TargetPath, ushort TypeIndex, params ElementValuePairRecord[] Elements)
    {

        public static bool TryReadTypeAnnotation(ref ClassFormatReader reader, out TypeAnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryReadU1(out byte targetType) == false)
                return false;
            if (TryReadTarget(ref reader, (TypeAnnotationTargetType)targetType, out var target) == false)
                return false;
            if (TypePathRecord.TryRead(ref reader, out var targetPath) == false)
                return false;
            if (reader.TryReadU2(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
                if (ElementValuePairRecord.TryRead(ref reader, out elements[i]) == false)
                    return false;

            annotation = new TypeAnnotationRecord((TypeAnnotationTargetType)targetType, target, targetPath, typeIndex, elements);
            return true;
        }

        public static bool TryReadTarget(ref ClassFormatReader reader, TypeAnnotationTargetType targetType, out TypeAnnotationTargetRecord targetInfo) => targetType switch
        {
            TypeAnnotationTargetType.ClassTypeParameter => TypeAnnotationParameterTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodTypeParameter => TypeAnnotationParameterTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ClassExtends => TypeAnnotationSuperTypeTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ClassTypeParameterBound => TypeAnnotationParameterBoundTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodTypeParameterBound => TypeAnnotationParameterBoundTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.Field => TypeAnnotationEmptyTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodReturn => TypeAnnotationEmptyTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodReceiver => TypeAnnotationEmptyTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodFormalParameter => TypeAnnotationFormalParameterTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.Throws => TypeAnnotationThrowsTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.LocalVariable => TypeAnnotationLocalVarTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ResourceVariable => TypeAnnotationLocalVarTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ExceptionParameter => TypeAnnotationCatchTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.InstanceOf => TypeAnnotationOffsetTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.New => TypeAnnotationOffsetTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ConstructorReference => TypeAnnotationOffsetTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodReference => TypeAnnotationOffsetTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.Cast => TypeAnnotationTypeArgumentTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ConstructorInvocationTypeArgument => TypeAnnotationTypeArgumentTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodInvocationTypeArgument => TypeAnnotationTypeArgumentTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.ConstructorReferenceTypeArgument => TypeAnnotationTypeArgumentTargetRecord.TryRead(ref reader, out targetInfo),
            TypeAnnotationTargetType.MethodReferenceTypeArgument => TypeAnnotationTypeArgumentTargetRecord.TryRead(ref reader, out targetInfo),
            _ => throw new ByteCodeException($"Invalid type annotation target type: '0x{targetType:X}'."),
        };

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var size = 0;
            size += sizeof(byte);
            size += Target.GetSize();
            size += TargetPath.GetSize();
            size += sizeof(ushort);
            size += sizeof(ushort);

            for (int i = 0; i < Elements.Length; i++)
                size += Elements[i].GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1((byte)TargetType) == false)
                return false;
            if (Target.TryWrite(ref writer) == false)
                return false;
            if (TargetPath.TryWrite(ref writer) == false)
                return false;
            if (writer.TryWriteU2(TypeIndex) == false)
                return false;
            if (writer.TryWriteU2((ushort)Elements.Length) == false)
                return false;

            foreach (var record in Elements)
                if (record.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}
