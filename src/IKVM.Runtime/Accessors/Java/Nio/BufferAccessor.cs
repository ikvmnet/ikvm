namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.nio.Buffer' type.
    /// </summary>
    internal sealed class BufferAccessor : Accessor<object>
    {

        FieldAccessor<object, long> address;
        FieldAccessor<object, int> capacity;
        FieldAccessor<object, int> position;
        FieldAccessor<object, int> limit;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public BufferAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.nio.Buffer"))
        {

        }

        /// <summary>
        /// Gets the value of the 'address' field.
        /// </summary>
        public long GetAddress(object self) => GetField(ref address, nameof(address)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'address' field.
        /// </summary>
        public int GetCapacity(object self) => GetField(ref capacity, nameof(capacity)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'address' field.
        /// </summary>
        public int GetPosition(object self) => GetField(ref position, nameof(position)).GetValue(self);

        /// <summary>
        /// Gets the value of the 'address' field.
        /// </summary>
        public int GetLimit(object self) => GetField(ref limit, nameof(limit)).GetValue(self);

    }

#endif

}
