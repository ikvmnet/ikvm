using System;
using System.Collections.Generic;
using System.Threading;

namespace IKVM.ByteCode
{

    public class Class
    {

        /// <summary>
        /// Gets the value at the given location or initializes it if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        internal static T LazyGet<T>(ref T location, Func<T> create)
            where T : class
        {
            if (location == null)
                Interlocked.CompareExchange(ref location, create(), null);

            return location;
        }

        internal int MAX_STACK_ALLOC = 1024;

        readonly ClassRecord record;

        string name;
        string superName;
        Constant[] constants;
        InterfaceInfoCollection interfaces;
        FieldInfoCollection fields;
        MethodInfoCollection methods;
        AttributeDataCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="record"></param>
        internal Class(ClassRecord record)
        {
            this.record = record;
        }

        /// <summary>
        /// Reference to the underlying record backing this class.
        /// </summary>
        internal ClassRecord Record => record;

        /// <summary>
        /// Gets the minor version of the class.
        /// </summary>
        public ushort MinorVersion => record.MinorVersion;

        /// <summary>
        /// Gets the major version of the class.
        /// </summary>
        public ushort MajorVersion => record.MajorVersion;

        /// <summary>
        /// Gets the access flags of the class.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => LazyGet(ref name, () => ResolveConstant<ClassConstant>(record.ThisClassIndex).Name.Value);

        /// <summary>
        /// Gets the name of the super class.
        /// </summary>
        public string SuperName => LazyGet(ref superName, () => ResolveConstant<ClassConstant>(record.SuperClassIndex).Name.Value);

        /// <summary>
        /// Gets the name of the interfaces implemented by this class.
        /// </summary>
        public InterfaceInfoCollection Interfaces => LazyGet(ref interfaces, () => new InterfaceInfoCollection(this, record.Interfaces));

        /// <summary>
        /// Gets the set of fields declared by the class.
        /// </summary>
        public IReadOnlyList<FieldInfo> Fields => LazyGet(ref fields, () => new FieldInfoCollection(this, record.Fields));

        /// <summary>
        /// Gets the set of methods declared by the class.
        /// </summary>
        public IReadOnlyList<MethodInfo> Methods => LazyGet(ref methods, () => new MethodInfoCollection(this, record.Methods));

        /// <summary>
        /// Gets the dictionary of attributes declared by the class.
        /// </summary>
        public AttributeDataCollection Attributes => LazyGet(ref attributes, () => new AttributeDataCollection(this, record.Attributes));

        /// <summary>
        /// Resolves the constant of the specified value.
        /// </summary>
        /// <typeparam name="TConstant"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        internal TConstant ResolveConstant<TConstant>(ushort index)
            where TConstant : Constant
        {
            return (TConstant)ResolveConstant(index);
        }

        /// <summary>
        /// Resolves the constant of the specified value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal Constant ResolveConstant(ushort index)
        {
            if (index < 1 || index >= record.Constants.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (constants == null)
                Interlocked.CompareExchange(ref constants, new Constant[record.Constants.Length], null);

            // cache already constants constant
            if (constants[index] is Constant constant)
                return constant;

            // generate new constant
            constant = record.Constants[index] switch
            {
                Utf8ConstantRecord c => new Utf8Constant(this, c),
                IntegerConstantRecord c => new IntegerConstant(this, c),
                FloatConstantRecord c => new FloatConstant(this, c),
                LongConstantRecord c => new LongConstant(this, c),
                DoubleConstantRecord c => new DoubleConstant(this, c),
                ClassConstantRecord c => new ClassConstant(this, c),
                StringConstantRecord c => new StringConstant(this, c),
                FieldrefConstantRecord c => new FieldrefConstant(this, c),
                MethodrefConstantRecord c => new MethodrefConstant(this, c),
                InterfaceMethodrefConstantRecord c => new InterfaceMethodrefConstant(this, c),
                NameAndTypeConstantRecord c => new NameAndTypeConstant(this, c),
                MethodHandleConstantRecord c => new MethodHandleConstant(this, c),
                MethodTypeConstantRecord c => new MethodTypeConstant(this, c),
                DynamicConstantRecord c => new DynamicConstant(this, c),
                InvokeDynamicConstantRecord c => new InvokeDynamicConstant(this, c),
                ModuleConstantRecord c => new ModuleConstant(this, c),
                PackageConstantRecord c => new PackageConstant(this, c),
                _ => throw new ClassReaderException("Invalid constant type."),
            };

            // atomic set, only one winner
            Interlocked.CompareExchange(ref constants[index], constant, null);
            return constants[index];
        }

    }

}
