using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Lazy init collection of attribute data.
    /// </summary>
    public sealed class AttributeDataCollection : IReadOnlyList<AttributeData>, IReadOnlyDictionary<string, AttributeData>, IEnumerable<AttributeData>
    {

        readonly Class clazz;
        readonly AttributeInfoRecord[] attributes;
        AttributeInfo[] infoCache;
        AttributeData[] dataCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="attributes"></param>
        public AttributeDataCollection(Class clazz, AttributeInfoRecord[] attributes)
        {
            this.clazz = clazz ?? throw new ArgumentNullException(nameof(clazz));
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        /// <summary>
        /// Resolves the attribute info at the given index.
        /// </summary>
        /// <returns></returns>
        AttributeInfo ResolveAttributeInfo(int index)
        {
            if (index < 0 || index >= attributes.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (infoCache == null)
                Interlocked.CompareExchange(ref infoCache, new AttributeInfo[attributes.Length], null);

            // consult cache
            if (infoCache[index] is AttributeInfo info)
                return info;

            // generate new attribute
            info = new AttributeInfo(clazz, attributes[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref infoCache[index], info, null);
            return infoCache[index];
        }

        /// <summary>
        /// Resolves the specified field of the class from the records.
        /// </summary>
        /// <returns></returns>
        AttributeData ResolveAttributeData(int index)
        {
            if (index < 0 || index >= attributes.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (dataCache == null)
                Interlocked.CompareExchange(ref dataCache, new AttributeData[attributes.Length], null);

            // consult cache
            if (dataCache[index] is AttributeData attribute)
                return attribute;

            // get attribute info
            var info = ResolveAttributeInfo(index);
            if (info == null)
                throw new ClassReaderException("Unable to read attribute info.");

            // parse attribute data
            if (AttributeDataRecordReader.TryReadAttribute(info, out var r) == false)
                throw new ClassReaderException("Unable to read attribute data.");

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
        AttributeData ResolveAttributeData(AttributeInfo info, AttributeDataRecord data) => data switch
        {
            ConstantValueAttributeDataRecord d => new ConstantValueAttributeData(clazz, info, d),
            CodeAttributeDataRecord d => new CodeAttributeData(clazz, info, d),
            StackMapTableAttributeDataRecord d => new StackMapTableAttributeData(clazz, info, d),
            ExceptionsAttributeDataRecord d => new ExceptionsAttributeData(clazz, info, d),
            InnerClassesAttributeDataRecord d => new InnerClassesAttributeData(clazz, info, d),
            EnclosingMethodAttributeDataRecord d => new EnclosingMethodAttributeData(clazz, info, d),
            SyntheticAttributeDataRecord d => new SyntheticAttributeData(clazz, info, d),
            SignatureAttributeDataRecord d => new SignatureAttributeData(clazz, info, d),
            SourceFileAttributeDataRecord d => new SourceFileAttributeData(clazz, info, d),
            SourceDebugExtensionAttributeDataRecord d => new SourceDebugExtensionAttributeData(clazz, info, d),
            LineNumberTableAttributeDataRecord d => new LineNumberTableAttributeData(clazz, info, d),
            LocalVariableTableAttributeDataRecord d => new LocalVariableTableAttributeData(clazz, info, d),
            LocalVariableTypeTableAttributeDataRecord d => new LocalVariableTypeTableAttributeData(clazz, info, d),
            DeprecatedAttributeDataRecord d => new DeprecatedAttributeData(clazz, info, d),
            RuntimeVisibleAnnotationsAttributeDataRecord d => new RuntimeVisibleAnnotationsAttributeData(clazz, info, d),
            RuntimeInvisibleAnnotationsAttributeDataRecord d => new RuntimeInvisibleAnnotationsAttributeData(clazz, info, d),
            RuntimeVisibleParameterAnnotationsAttributeDataRecord d => new RuntimeVisibleParameterAnnotationsAttributeData(clazz, info, d),
            RuntimeInvisibleParameterAnnotationsAttributeDataRecord d => new RuntimeInvisibleParameterAnnotationsAttributeData(clazz, info, d),
            RuntimeVisibleTypeAnnotationsAttributeDataRecord d => new RuntimeVisibleTypeAnnotationsAttributeData(clazz, info, d),
            RuntimeInvisibleTypeAnnotationsAttributeDataRecord d => new RuntimeInvisibleTypeAnnotationsAttributeData(clazz, info, d),
            AnnotationDefaultAttributeDataRecord d => new AnnotationDefaultAttributeData(clazz, info, d),
            BootstrapMethodsAttributeDataRecord d => new BootstrapMethodsAttributeData(clazz, info, d),
            MethodParametersAttributeDataRecord d => new MethodParametersAttributeData(clazz, info, d),
            ModuleAttributeDataRecord d => new ModuleAttributeData(clazz, info, d),
            ModulePackagesAttributeDataRecord d => new ModulePackagesAttributeData(clazz, info, d),
            ModuleMainClassAttributeDataRecord d => new ModuleMainClassAttributeData(clazz, info, d),
            NestHostAttributeDataRecord d => new NestHostAttributeData(clazz, info, d),
            NestMembersAttributeDataRecord d => new NestMembersAttributeData(clazz, info, d),
            RecordAttributeDataRecord d => new RecordAttributeData(clazz, info, d),
            PermittedSubclassesAttributeDataRecord d => new PermittedSubclassesAttributeData(clazz, info, d),
            CustomAttributeDataRecord d => new CustomAttributeData(clazz, info, d),
            _ => throw new ClassReaderException("Cannot resolve attribute data."),
        };

        /// <summary>
        /// Gets the attribute at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AttributeData this[int index] => ResolveAttributeData(index);

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AttributeData this[string key] => Enumerable.Range(0, attributes.Length).Select(i => new { Index = i, Info = ResolveAttributeInfo(i) }).Where(i => i.Info.Name == key).Select(i => ResolveAttributeData(i.Index)).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the attribute with the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AttributeData this[Type type] => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).FirstOrDefault(type.IsInstanceOfType) ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the attribute of the specified type, or <c>null</c> if no such attribute exists.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute Get<TAttribute>() where TAttribute : AttributeData => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).OfType<TAttribute>().FirstOrDefault();

        /// <summary>
        /// Gets the count of attributes.
        /// </summary>
        public int Count => attributes.Length;

        /// <summary>
        /// Gets the names of the attributes.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, AttributeData>.Keys => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeInfo).Select(i => i.Name);

        /// <summary>
        /// Gets all of the attribute values.
        /// </summary>
        IEnumerable<AttributeData> IReadOnlyDictionary<string, AttributeData>.Values => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData);

        /// <summary>
        /// Gets an enumerator over each attribute.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerator<AttributeData> GetEnumerator() => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).GetEnumerator();

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
        bool IReadOnlyDictionary<string, AttributeData>.ContainsKey(string key) => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeInfo).Any(i => i.Name == key);

        /// <summary>
        /// Attempts to get the attribute with the specified name.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out AttributeData value)
        {
            value = Enumerable.Range(0, attributes.Length).Select(i => new { Index = i, Info = ResolveAttributeInfo(i) }).Where(i => i.Info.Name == key).Select(i => ResolveAttributeData(i.Index)).FirstOrDefault();
            return value != null;
        }

        /// <summary>
        /// Gets the list of the items in the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<string, AttributeData>> IEnumerable<KeyValuePair<string, AttributeData>>.GetEnumerator() => Enumerable.Range(0, attributes.Length).Select(ResolveAttributeData).Select(i => new KeyValuePair<string, AttributeData>(i.Name, i)).GetEnumerator();

    }

}
