namespace IKVM.ByteCode.Parser
{

    public interface IClassListener
    {

        /// <summary>
        /// Invoked when the 'magic' field is parsed.
        /// </summary>
        /// <param name="flags"></param>
        void SetMagic(uint value);

        /// <summary>
        /// Invoked when the 'minor_version' field is parsed.
        /// </summary>
        /// <param name="flags"></param>
        void SetMinorVersion(ushort value);

        /// <summary>
        /// Invoked when the 'major_version' field is parsed.
        /// </summary>
        /// <param name="flags"></param>
        void SetMajorVersion(ushort value);

        /// <summary>
        /// Invoked when the 'constant_pool_count' field is parsed.
        /// </summary>
        /// <param name="flags"></param>
        void SetConstantCount(ushort count);

        /// <summary>
        /// Invoked when a constant pool is parsed. Returns a <see cref="IConstantPoolListener"/> for accepting parse events for the pool.
        /// </summary>
        /// <returns></returns>
        IConstantPoolListener AddConstant(ConstantPoolIndex index);

        /// <summary>
        /// Invoked when the 'access_flags' field is parsed.
        /// </summary>
        /// <param name="flags"></param>
        void SetAccessFlags(AccessFlag flags);

        /// <summary>
        /// Invoked when the 'this_class' field is parsed.
        /// </summary>
        /// <param name="index"></param>
        void SetThisClass(ConstantPoolIndex index);

        /// <summary>
        /// Invoked when the 'super_class' field is parsed.
        /// </summary>
        /// <param name="index"></param>
        void SetSuperClass(ConstantPoolIndex index);

        /// <summary>
        /// Invoked when the 'interfaces_count' field is parsed.
        /// </summary>
        /// <param name="count"></param>
        void SetInterfacesCount(int count);

        /// <summary>
        /// Invoked when an interface is parsed.
        /// </summary>
        /// <param name="index"></param>
        void AddInterface(int index);

        /// <summary>
        /// Invoked when the 'fields_count' field is parsed.
        /// </summary>
        /// <param name="count"></param>
        void SetFieldsCount(int count);

        /// <summary>
        /// Invoked when a field is parsed. Returns a <see cref="IFieldInfoListener"/> for accepting parse events for the field.
        /// </summary>
        /// <returns></returns>
        IFieldInfoListener AddField();

        /// <summary>
        /// Invoked when the 'methods_count' field is parsed.
        /// </summary>
        /// <param name="count"></param>
        void SetMethodsCount(int count);

        /// <summary>
        /// Invoked when a method is parsed. Returns a <see cref="IMethodListener"/> for accepting parse events for the method.
        /// </summary>
        /// <returns></returns>
        IMethodListener AddMethod();

        /// <summary>
        /// Invoked when the 'attributes_count' field is parsed.
        /// </summary>
        /// <param name="count"></param>
        void SetAttributesCount(int count);

        /// <summary>
        /// Invoked when an attribute is parsed. Returns a <see cref="IAttributeListener"/> for accepting parse events for the attribute.
        /// </summary>
        /// <returns></returns>
        IAttributeListener AddAttribute();

    }

}