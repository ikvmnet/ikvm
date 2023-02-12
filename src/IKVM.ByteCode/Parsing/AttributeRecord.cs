using IKVM.ByteCode.Reading;
using System.Buffers;

namespace IKVM.ByteCode.Parsing
{

    internal abstract record AttributeRecord
    {

        /// <summary>
        /// Attempts to read an attribute data record from the specified <see cref="AttributeInfoReader"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool TryRead(AttributeInfoReader info, out AttributeRecord attribute)
        {
            return TryRead(info.Name.Value, new ReadOnlySequence<byte>(info.Data), out attribute);
        }

        /// <summary>
        /// Attempts to read an attribute data record from the specified <see cref="AttributeInfoReader"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool TryRead(string name, ReadOnlySequence<byte> data, out AttributeRecord attribute)
        {
            var reader = new ClassFormatReader(data);
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
        /// <exception cref="ByteCodeException"></exception>
        public static bool TryReadAttribute(string name, ref ClassFormatReader reader, out AttributeRecord attribute) => name switch
        {
            ConstantValueAttributeRecord.Name => ConstantValueAttributeRecord.TryReadConstantValueAttribute(ref reader, out attribute),
            CodeAttributeRecord.Name => CodeAttributeRecord.TryReadCodeAttribute(ref reader, out attribute),
            "StackMapTable" => StackMapTableAttributeRecord.TryRead(ref reader, out attribute),
            ExceptionsAttributeRecord.Name => ExceptionsAttributeRecord.TryReadExceptionsAttribute(ref reader, out attribute),
            InnerClassesAttributeRecord.Name => InnerClassesAttributeRecord.TryReadInnerClassesAttribute(ref reader, out attribute),
            "EnclosingMethod" => EnclosingMethodAttributeRecord.TryReadEnclosingMethodAttribute(ref reader, out attribute),
            "Synthetic" => SyntheticAttributeRecord.TryReadSyntheticAttribute(ref reader, out attribute),
            SignatureAttributeRecord.Name => SignatureAttributeRecord.TryReadSignatureAttribute(ref reader, out attribute),
            "SourceFile" => SourceFileAttributeRecord.TryReadSourceFileAttribute(ref reader, out attribute),
            "SourceDebugExtension" => SourceDebugExtensionAttributeRecord.TryReadSourceDebugExtensionAttribute(ref reader, out attribute),
            "LineNumberTable" => LineNumberTableAttributeRecord.TryReadLineNumberTableAttribute(ref reader, out attribute),
            "LocalVariableTable" => LocalVariableTableAttributeRecord.TryReadLocalVariableTableAttribute(ref reader, out attribute),
            "LocalVariableTypeTable" => LocalVariableTypeTableAttributeRecord.TryReadLocalVariableTypeTableAttribute(ref reader, out attribute),
            DeprecatedAttributeRecord.Name => DeprecatedAttributeRecord.TryReadDeprecatedAttribute(ref reader, out attribute),
            RuntimeVisibleAnnotationsAttributeRecord.Name => RuntimeVisibleAnnotationsAttributeRecord.TryReadRuntimeVisibleAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleAnnotations" => RuntimeInvisibleAnnotationsAttributeRecord.TryReadRuntimeInvisibleAnnotationsAttribute(ref reader, out attribute),
            "RuntimeVisibleParameterAnnotations" => RuntimeVisibleParameterAnnotationsAttributeRecord.TryReadRuntimeVisibleParameterAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleParameterAnnotations" => RuntimeInvisibleParameterAnnotationsAttributeRecord.TryReadRuntimeInvisibleParameterAnnotationsAttribute(ref reader, out attribute),
            "RuntimeVisibleTypeAnnotations" => RuntimeVisibleTypeAnnotationsAttributeRecord.TryReadRuntimeVisibleTypeAnnotationsAttribute(ref reader, out attribute),
            "RuntimeInvisibleTypeAnnotations" => RuntimeInvisibleTypeAnnotationsAttributeRecord.TryReadRuntimeInvisibleTypeAnnotationsAttribute(ref reader, out attribute),
            AnnotationDefaultAttributeRecord.Name => AnnotationDefaultAttributeRecord.TryReadAnnotationDefaultAttribute(ref reader, out attribute),
            BootstrapMethodsAttributeRecord.Name => BootstrapMethodsAttributeRecord.TryReadBootstrapMethodsAttribute(ref reader, out attribute),
            MethodParametersAttributeRecord.Name => MethodParametersAttributeRecord.TryReadMethodParametersAttribute(ref reader, out attribute),
            "Module" => ModuleAttributeRecord.TryReadModuleAttribute(ref reader, out attribute),
            "ModulePackages" => ModulePackagesAttributeRecord.TryReadModulePackagesAttribute(ref reader, out attribute),
            "ModuleMainClass" => ModuleMainClassAttributeRecord.TryReadModuleMainClassAttribute(ref reader, out attribute),
            "NestHost" => NestHostAttributeRecord.TryReadNestHostAttribute(ref reader, out attribute),
            "NestMembers" => NestMembersAttributeRecord.TryReadNestMembersAttribute(ref reader, out attribute),
            "Record" => RecordAttributeRecord.TryReadRecordAttribute(ref reader, out attribute),
            "PermittedSubclasses" => PermittedSubclassesAttributeRecord.TryReadPermittedSubclassesAttribute(ref reader, out attribute),
            _ => UnknownAttributeRecord.TryReadCustomAttribute(ref reader, out attribute),
        };

        public abstract bool TryWrite(ref ClassFormatWriter writer);

        public abstract int GetSize();
    }

}
