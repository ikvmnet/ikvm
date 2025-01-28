using System;
using System.Diagnostics.Metrics;

namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Provides access to record diagnostic metrics.
    /// </summary>
    internal class Metrics
    {

        readonly IMeterFactory _meters;
        readonly Meter _meter;
        readonly Counter<int> _runtimeByteCodeJavaTypeCounter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Metrics() :
            this(new DefaultMeterFactory())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="meters"></param>
        public Metrics(IMeterFactory meters)
        {
            _meters = meters ?? throw new ArgumentNullException(nameof(meters));
            _meter = _meters.Create("ikvm");
            _runtimeByteCodeJavaTypeCounter ??= _meter.CreateCounter<int>("ikvm.RuntimeByteCodeJavaType");
        }

        /// <summary>
        /// Indicates that an instance of RuntimeByteCodeJavaType was created.
        /// </summary>
        public void RuntimeByteCodeJavaType()
        {
            _runtimeByteCodeJavaTypeCounter.Add(1);
        }

    }

}
