using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    public sealed class AttributeReaderCollection : LazyReaderList<AttributeReader, AttributeInfoRecord>
    {

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
        public AttributeReader this[string name] => Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the names of the attribute.
        /// </summary>
        public IEnumerable<string> Names => Enumerable.Range(0, Count).Select(i => this[i]).Select(i => i.Name);

        /// <summary>
        /// Returns <c>true</c> if an attribute with the specified name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => Enumerable.Range(0, Count).Any(i => this[i].Name == name);

        /// <summary>
        /// Attempts to get the attribute with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out AttributeReader value)
        {
            value = Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the attribute with the specified type or returns <c>null</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FirstOfType<T>()
        {
            return Enumerable.Range(0, Count).Select(i => this[i]).OfType<T>().FirstOrDefault();
        }

    }

}
