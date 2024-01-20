using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Represents a label in the instruction stream. Label is used in conjunction with the <see cref="ILGenerator"/> class.
    /// </summary>
    public readonly struct Label
    {

        public static bool operator ==(Label arg1, Label arg2) => arg1.Handle.Id == arg2.Handle.Id;

        public static bool operator !=(Label arg1, Label arg2) => arg1.Handle.Id != arg2.Handle.Id;

        readonly LabelHandle handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="index"></param>
        internal Label(LabelHandle handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets the underlying metadata handle of the label.
        /// </summary>
        internal LabelHandle Handle => handle;

        /// <summary>
        /// Gets the 0-based index of the label.
        /// </summary>
        internal int Index => handle.Id - 1;

        /// <summary>
        /// Returns <c>true</c> if this label is the same as the other label.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Label other)
        {
            return handle.Id == other.handle.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Label l && Equals(l);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return handle.GetHashCode();
        }

    }

}
