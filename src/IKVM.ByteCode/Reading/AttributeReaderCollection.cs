using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    public sealed class AttributeReaderCollection : LazyReaderList<AttributeReader, AttributeInfoRecord>
    {

        ConstantValueAttributeReader constantValueAttribute;
        CodeAttributeReader codeAttribute;
        StackMapTableAttributeReader stackMapTableAttribute;
        ExceptionsAttributeReader exceptionsAttribute;
        InnerClassesAttributeReader innerClassesAttribute;
        EnclosingMethodAttributeReader enclosingMethodAttribute;
        SyntheticAttributeReader syntheticAttribute;
        SignatureAttributeReader signatureAttribute;
        SourceFileAttributeReader sourceFileAttribute;
        SourceDebugExtensionAttributeReader sourceDebugExtensionAttribute;
        LineNumberTableAttributeReader lineNumberTableAttribute;
        LocalVariableTableAttributeReader localVariableTableAttribute;
        LocalVariableTypeTableAttributeReader localVariableTypeTableAttribute;
        DeprecatedAttributeReader deprecatedAttribute;
        RuntimeVisibleAnnotationsAttributeReader runtimeVisibleAnnotationsAttribute;
        RuntimeInvisibleAnnotationsAttributeReader runtimeInvisibleAnnotationsAttribute;
        RuntimeVisibleParameterAnnotationsAttributeReader runtimeVisibleParameterAnnotationsAttribute;
        RuntimeInvisibleParameterAnnotationsAttributeReader runtimeInvisibleParameterAnnotationsAttribute;
        RuntimeVisibleTypeAnnotationsAttributeReader runtimeVisibleTypeAnnotationsAttribute;
        RuntimeInvisibleTypeAnnotationsAttributeReader runtimeInvisibleTypeAnnotationsAttribute;
        AnnotationDefaultAttributeReader annotationDefaultAttribute;
        BootstrapMethodsAttributeReader bootstrapMethodsAttribute;
        MethodParametersAttributeReader methodParametersAttribute;
        ModuleAttributeReader moduleAttribute;
        ModulePackagesAttributeReader modulePackagesAttribute;
        ModuleMainClassAttributeReader moduleMainClassAttribute;
        NestHostAttributeReader nestHostAttribute;
        NestMembersAttributeReader nestMembersAttribute;
        RecordAttributeReader recordAttribute;
        PermittedSubclassesAttributeReader permittedSubclassesAttribute;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        public AttributeReaderCollection(ClassReader declaringClass, AttributeInfoRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        protected override AttributeReader CreateReader(int index, AttributeInfoRecord record)
        {
            var info = new AttributeInfoReader(DeclaringClass, record);
            if (AttributeRecord.TryRead(info, out var attribute) == false)
                throw new ByteCodeException("Unable to read attribute data.");

            return attribute switch
            {
                ConstantValueAttributeRecord d => new ConstantValueAttributeReader(DeclaringClass, info, d),
                CodeAttributeRecord d => new CodeAttributeReader(DeclaringClass, info, d),
                StackMapTableAttributeRecord d => new StackMapTableAttributeReader(DeclaringClass, info, d),
                ExceptionsAttributeRecord d => new ExceptionsAttributeReader(DeclaringClass, info, d),
                InnerClassesAttributeRecord d => new InnerClassesAttributeReader(DeclaringClass, info, d),
                EnclosingMethodAttributeRecord d => new EnclosingMethodAttributeReader(DeclaringClass, info, d),
                SyntheticAttributeRecord d => new SyntheticAttributeReader(DeclaringClass, info, d),
                SignatureAttributeRecord d => new SignatureAttributeReader(DeclaringClass, info, d),
                SourceFileAttributeRecord d => new SourceFileAttributeReader(DeclaringClass, info, d),
                SourceDebugExtensionAttributeRecord d => new SourceDebugExtensionAttributeReader(DeclaringClass, info, d),
                LineNumberTableAttributeRecord d => new LineNumberTableAttributeReader(DeclaringClass, info, d),
                LocalVariableTableAttributeRecord d => new LocalVariableTableAttributeReader(DeclaringClass, info, d),
                LocalVariableTypeTableAttributeRecord d => new LocalVariableTypeTableAttributeReader(DeclaringClass, info, d),
                DeprecatedAttributeRecord d => new DeprecatedAttributeReader(DeclaringClass, info, d),
                RuntimeVisibleAnnotationsAttributeRecord d => new RuntimeVisibleAnnotationsAttributeReader(DeclaringClass, info, d),
                RuntimeInvisibleAnnotationsAttributeRecord d => new RuntimeInvisibleAnnotationsAttributeReader(DeclaringClass, info, d),
                RuntimeVisibleParameterAnnotationsAttributeRecord d => new RuntimeVisibleParameterAnnotationsAttributeReader(DeclaringClass, info, d),
                RuntimeInvisibleParameterAnnotationsAttributeRecord d => new RuntimeInvisibleParameterAnnotationsAttributeReader(DeclaringClass, info, d),
                RuntimeVisibleTypeAnnotationsAttributeRecord d => new RuntimeVisibleTypeAnnotationsAttributeReader(DeclaringClass, info, d),
                RuntimeInvisibleTypeAnnotationsAttributeRecord d => new RuntimeInvisibleTypeAnnotationsAttributeReader(DeclaringClass, info, d),
                AnnotationDefaultAttributeRecord d => new AnnotationDefaultAttributeReader(DeclaringClass, info, d),
                BootstrapMethodsAttributeRecord d => new BootstrapMethodsAttributeReader(DeclaringClass, info, d),
                MethodParametersAttributeRecord d => new MethodParametersAttributeReader(DeclaringClass, info, d),
                ModuleAttributeRecord d => new ModuleAttributeReader(DeclaringClass, info, d),
                ModulePackagesAttributeRecord d => new ModulePackagesAttributeReader(DeclaringClass, info, d),
                ModuleMainClassAttributeRecord d => new ModuleMainClassAttributeReader(DeclaringClass, info, d),
                NestHostAttributeRecord d => new NestHostAttributeReader(DeclaringClass, info, d),
                NestMembersAttributeRecord d => new NestMembersAttributeReader(DeclaringClass, info, d),
                RecordAttributeRecord d => new RecordAttributeReader(DeclaringClass, info, d),
                PermittedSubclassesAttributeRecord d => new PermittedSubclassesAttributeReader(DeclaringClass, info, d),
                UnknownAttributeRecord d => new UnknownAttributeReader(DeclaringClass, info, d),
                _ => throw new ByteCodeException("Cannot resolve attribute data."),
            };
        }

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AttributeReader this[string name] => this.FirstOrDefault(i => i.Info.Name.Value == name) ?? throw new KeyNotFoundException();

        /// <summary>
        /// Returns <c>true</c> if an attribute with the specified name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => this.Any(i => i.Info.Name.Value == name);

        /// <summary>
        /// Attempts to get the attribute with the specified name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<TAttribute>(out TAttribute value)
        {
            value = this.OfType<TAttribute>().FirstOrDefault();
            return value != null;
        }

        public ConstantValueAttributeReader ConstantValue => LazyGet(ref constantValueAttribute, () => TryGet<ConstantValueAttributeReader>(out var attribute) ? attribute : null);

        public CodeAttributeReader Code => LazyGet(ref codeAttribute, () => TryGet<CodeAttributeReader>(out var attribute) ? attribute : null);

        public StackMapTableAttributeReader StackMapTable => LazyGet(ref stackMapTableAttribute, () => TryGet<StackMapTableAttributeReader>(out var attribute) ? attribute : null);

        public ExceptionsAttributeReader Exceptions => LazyGet(ref exceptionsAttribute, () => TryGet<ExceptionsAttributeReader>(out var attribute) ? attribute : null);

        public InnerClassesAttributeReader InnerClasses => LazyGet(ref innerClassesAttribute, () => TryGet<InnerClassesAttributeReader>(out var attribute) ? attribute : null);

        public EnclosingMethodAttributeReader EnclosingMethod => LazyGet(ref enclosingMethodAttribute, () => TryGet<EnclosingMethodAttributeReader>(out var attribute) ? attribute : null);

        public SyntheticAttributeReader Synthetic => LazyGet(ref syntheticAttribute, () => TryGet<SyntheticAttributeReader>(out var attribute) ? attribute : null);

        public SignatureAttributeReader Signature => LazyGet(ref signatureAttribute, () => TryGet<SignatureAttributeReader>(out var attribute) ? attribute : null);

        public SourceFileAttributeReader SourceFile => LazyGet(ref sourceFileAttribute, () => TryGet<SourceFileAttributeReader>(out var attribute) ? attribute : null);

        public SourceDebugExtensionAttributeReader SourceDebugExtension => LazyGet(ref sourceDebugExtensionAttribute, () => TryGet<SourceDebugExtensionAttributeReader>(out var attribute) ? attribute : null);

        public LineNumberTableAttributeReader LineNumberTable => LazyGet(ref lineNumberTableAttribute, () => TryGet<LineNumberTableAttributeReader>(out var attribute) ? attribute : null);

        public LocalVariableTableAttributeReader LocalVariableTable => LazyGet(ref localVariableTableAttribute, () => TryGet<LocalVariableTableAttributeReader>(out var attribute) ? attribute : null);

        public LocalVariableTypeTableAttributeReader LocalVariableTypeTable => LazyGet(ref localVariableTypeTableAttribute, () => TryGet<LocalVariableTypeTableAttributeReader>(out var attribute) ? attribute : null);

        public DeprecatedAttributeReader Deprecated => LazyGet(ref deprecatedAttribute, () => TryGet<DeprecatedAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeVisibleAnnotationsAttributeReader RuntimeVisibleAnnotations => LazyGet(ref runtimeVisibleAnnotationsAttribute, () => TryGet<RuntimeVisibleAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeInvisibleAnnotationsAttributeReader RuntimeInvisibleAnnotations => LazyGet(ref runtimeInvisibleAnnotationsAttribute, () => TryGet<RuntimeInvisibleAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeVisibleParameterAnnotationsAttributeReader RuntimeVisibleParameterAnnotations => LazyGet(ref runtimeVisibleParameterAnnotationsAttribute, () => TryGet<RuntimeVisibleParameterAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeInvisibleParameterAnnotationsAttributeReader RuntimeInvisibleParameterAnnotations => LazyGet(ref runtimeInvisibleParameterAnnotationsAttribute, () => TryGet<RuntimeInvisibleParameterAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeVisibleTypeAnnotationsAttributeReader RuntimeVisibleTypeAnnotations => LazyGet(ref runtimeVisibleTypeAnnotationsAttribute, () => TryGet<RuntimeVisibleTypeAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public RuntimeInvisibleTypeAnnotationsAttributeReader RuntimeInvisibleTypeAnnotations => LazyGet(ref runtimeInvisibleTypeAnnotationsAttribute, () => TryGet<RuntimeInvisibleTypeAnnotationsAttributeReader>(out var attribute) ? attribute : null);

        public AnnotationDefaultAttributeReader AnnotationDefault => LazyGet(ref annotationDefaultAttribute, () => TryGet<AnnotationDefaultAttributeReader>(out var attribute) ? attribute : null);

        public BootstrapMethodsAttributeReader BootstrapMethods => LazyGet(ref bootstrapMethodsAttribute, () => TryGet<BootstrapMethodsAttributeReader>(out var attribute) ? attribute : null);

        public MethodParametersAttributeReader MethodParameters => LazyGet(ref methodParametersAttribute, () => TryGet<MethodParametersAttributeReader>(out var attribute) ? attribute : null);

        public ModuleAttributeReader Module => LazyGet(ref moduleAttribute, () => TryGet<ModuleAttributeReader>(out var attribute) ? attribute : null);

        public ModulePackagesAttributeReader ModulePackages => LazyGet(ref modulePackagesAttribute, () => TryGet<ModulePackagesAttributeReader>(out var attribute) ? attribute : null);

        public ModuleMainClassAttributeReader ModuleMainClass => LazyGet(ref moduleMainClassAttribute, () => TryGet<ModuleMainClassAttributeReader>(out var attribute) ? attribute : null);

        public NestHostAttributeReader NestHost => LazyGet(ref nestHostAttribute, () => TryGet<NestHostAttributeReader>(out var attribute) ? attribute : null);

        public NestMembersAttributeReader NestMembers => LazyGet(ref nestMembersAttribute, () => TryGet<NestMembersAttributeReader>(out var attribute) ? attribute : null);

        public RecordAttributeReader Record => LazyGet(ref recordAttribute, () => TryGet<RecordAttributeReader>(out var attribute) ? attribute : null);

        public PermittedSubclassesAttributeReader PermittedSubclasses => LazyGet(ref permittedSubclassesAttribute, () => TryGet<PermittedSubclassesAttributeReader>(out var attribute) ? attribute : null);

    }

}
