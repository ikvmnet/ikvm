using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    public sealed class AttributeReaderCollection : IReadOnlyList<AttributeReader>, IReadOnlyDictionary<string, AttributeReader>, IEnumerable<AttributeReader>
    {

        readonly ClassReader ownerClass;
        readonly AttributeInfoRecord[] attributes;
        AttributeInfoReader[] infoCache;
        AttributeReader[] dataCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ownerClass"></param>
        /// <param name="attributes"></param>
        public AttributeReaderCollection(ClassReader ownerClass, AttributeInfoRecord[] attributes)
        {
            this.ownerClass = ownerClass ?? throw new ArgumentNullException(nameof(ownerClass));
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Resolves the attribute info at the given index.
        /// </summary>
        /// <returns></returns>
        AttributeInfoReader ResolveAttributeInfo(int index)
        {
            if (index < 0 || index >= attributes.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (infoCache == null)
                Interlocked.CompareExchange(ref infoCache, new AttributeInfoReader[attributes.Length], null);

            // consult cache
            if (infoCache[index] is AttributeInfoReader info)
                return info;

            // generate new attribute
            info = new AttributeInfoReader(ownerClass, attributes[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref infoCache[index], info, null);
            return infoCache[index];
        }

        /// <summary>
        /// Resolves the specified field of the class from the records.
        /// </summary>
        /// <returns></returns>
        AttributeReader ResolveAttributeData(int index)
        {
            if (index < 0 || index >= attributes.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (dataCache == null)
                Interlocked.CompareExchange(ref dataCache, new AttributeReader[attributes.Length], null);

            // consult cache
            if (dataCache[index] is AttributeReader attribute)
                return attribute;

            // get attribute info
            var info = ResolveAttributeInfo(index);
            if (info == null)
                throw new ByteCodeException("Unable to read attribute info.");

            // parse attribute data
            if (AttributeRecord.TryReadAttribute(info, out var r) == false)
                throw new ByteCodeException("Unable to read attribute data.");

            // atomic set, only one winner
            Interlocked.CompareExchange(ref dataCache[index], ResolveAttributeData(info, r), null);
            return dataCache[index];
        }

        /// <summary>
        /// Resolves the given attribute data record to an attribute type.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        AttributeReader ResolveAttributeData(AttributeInfoReader info, AttributeRecord data) => data switch
        {
            ConstantValueAttributeRecord d => new ConstantValueAttributeReader(ownerClass, info, d),
            CodeAttributeRecord d => new CodeAttributeReader(ownerClass, info, d),
            StackMapTableAttributeRecord d => new StackMapTableAttributeReader(ownerClass, info, d),
            ExceptionsAttributeRecord d => new ExceptionsAttributeReader(ownerClass, info, d),
            InnerClassesAttributeRecord d => new InnerClassesAttributeReader(ownerClass, info, d),
            EnclosingMethodAttributeRecord d => new EnclosingMethodAttributeReader(ownerClass, info, d),
            SyntheticAttributeRecord d => new SyntheticAttributeReader(ownerClass, info, d),
            SignatureAttributeRecord d => new SignatureAttributeReader(ownerClass, info, d),
            SourceFileAttributeRecord d => new SourceFileAttributeReader(ownerClass, info, d),
            SourceDebugExtensionAttributeRecord d => new SourceDebugExtensionAttributeReader(ownerClass, info, d),
            LineNumberTableAttributeRecord d => new LineNumberTableAttributeReader(ownerClass, info, d),
            LocalVariableTableAttributeRecord d => new LocalVariableTableAttributeReader(ownerClass, info, d),
            LocalVariableTypeTableAttributeRecord d => new LocalVariableTypeTableAttributeReader(ownerClass, info, d),
            DeprecatedAttributeRecord d => new DeprecatedAttributeReader(ownerClass, info, d),
            RuntimeVisibleAnnotationsAttributeRecord d => new RuntimeVisibleAnnotationsAttributeReader(ownerClass, info, d),
            RuntimeInvisibleAnnotationsAttributeRecord d => new RuntimeInvisibleAnnotationsAttributeReader(ownerClass, info, d),
            RuntimeVisibleParameterAnnotationsAttributeRecord d => new RuntimeVisibleParameterAnnotationsAttributeReader(ownerClass, info, d),
            RuntimeInvisibleParameterAnnotationsAttributeRecord d => new RuntimeInvisibleParameterAnnotationsAttributeReader(ownerClass, info, d),
            RuntimeVisibleTypeAnnotationsAttributeRecord d => new RuntimeVisibleTypeAnnotationsAttributeReader(ownerClass, info, d),
            RuntimeInvisibleTypeAnnotationsAttributeRecord d => new RuntimeInvisibleTypeAnnotationsAttributeReader(ownerClass, info, d),
            AnnotationDefaultAttributeRecord d => new AnnotationDefaultAttributeReader(ownerClass, info, d),
            BootstrapMethodsAttributeRecord d => new BootstrapMethodsAttributeReader(ownerClass, info, d),
            MethodParametersAttributeRecord d => new MethodParametersAttributeReader(ownerClass, info, d),
            ModuleAttributeRecord d => new ModuleAttributeReader(ownerClass, info, d),
            ModulePackagesAttributeRecord d => new ModulePackagesAttributeReader(ownerClass, info, d),
            ModuleMainClassAttributeRecord d => new ModuleMainClassAttributeReader(ownerClass, info, d),
            NestHostAttributeRecord d => new NestHostAttributeReader(ownerClass, info, d),
            NestMembersAttributeRecord d => new NestMembersAttributeReader(ownerClass, info, d),
            RecordAttributeRecord d => new RecordAttributeReader(ownerClass, info, d),
            PermittedSubclassesAttributeRecord d => new PermittedSubclassesAttributeReader(ownerClass, info, d),
            UnknownAttributeRecord d => new UnknownAttributeReader(ownerClass, info, d),
            _ => throw new ByteCodeException("Cannot resolve attribute data."),
        };

        /// <summary>
        /// Gets the attribute at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AttributeReader this[int index] => ResolveAttributeData(index);

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AttributeReader this[string key] => Enumerable.Range(0, attributes.Length).Select(i => new { Index = i, Info = ResolveAttributeInfo(i) }).Where(i => i.Info.Name == key).Select(i => ResolveAttributeData(i.Index)).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AttributeReader this[Type type] => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).FirstOrDefault(type.IsInstanceOfType) ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the attribute of the specified type, or <c>null</c> if no such attribute exists.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute Get<TAttribute>() where TAttribute : AttributeReader => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).OfType<TAttribute>().FirstOrDefault();

        /// <summary>
        /// Gets the count of attributes.
        /// </summary>
        public int Count => attributes.Length;

        /// <summary>
        /// Gets the names of the attributes.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, AttributeReader>.Keys => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the attribute values.
        /// </summary>
        IEnumerable<AttributeReader> IReadOnlyDictionary<string, AttributeReader>.Values => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData);

        /// <summary>
        /// Gets an enumerator over each attribute.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerator<AttributeReader> GetEnumerator() => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each attribute.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns <c>true</c> if an attribute with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IReadOnlyDictionary<string, AttributeReader>.ContainsKey(string key) => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the attribute with the specified name.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out AttributeReader value)
        {
            value = Enumerable.Range(0, attributes.Length).Select(i => new { Index = i, Info = ResolveAttributeInfo(i) }).Where(i => i.Info.Name == key).Select(i => ResolveAttributeData(i.Index)).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the items in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, AttributeReader>> IEnumerable<KeyValuePair<string, AttributeReader>>.GetEnumerator() => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).Select(i => new KeyValuePair<string, AttributeReader>(i.Name, i)).GetEnumerator();

    }

}
