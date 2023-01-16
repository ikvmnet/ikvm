using System;

namespace IKVM.ByteCode
{

    public abstract class Constant
    {

        readonly Class owner;
        readonly ConstantRecord record;

        /// <summary>
        /// Initializes a new insance.
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Constant(Class owner, ConstantRecord record)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the class of which this constant is a member.
        /// </summary>
        protected Class DeclaringClass => owner;

        /// <summary>
        /// Gets the underlying record of the constant.
        /// </summary>
        protected ConstantRecord Record => record;

    }

    public abstract class Constant<TRecord> : Constant
        where TRecord : ConstantRecord
    {

        readonly TRecord record;

        /// <summary>
        /// Initializes a new insance.
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Constant(Class owner, TRecord record) :
            base(owner, record)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the underlying record of the constant.
        /// </summary>
        protected new TRecord Record => record;

    }

}