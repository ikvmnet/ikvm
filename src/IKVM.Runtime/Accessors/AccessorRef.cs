namespace IKVM.Runtime.Accessors
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Represents a reference to an accessor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct AccessorRef<T> where T : Accessor
    {

        T accessor;

        /// <summary>
        /// Gets the accessor value.
        /// </summary>
        public T Value => JVM.Internal.BaseAccessors.Get(ref accessor);

    }

#endif

}
