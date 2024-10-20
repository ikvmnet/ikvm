using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Wraps a <see cref="IBufferWriter{T}"/> and queues data writen to it. Requires disposal to ensure flushing.
    /// </summary>
    class AsyncDiagnosticChannelWriter : IDiagnosticChannelWriter, IDisposable
    {

        static readonly UnboundedChannelOptions unboundedChannelOptions = new UnboundedChannelOptions()
        {
            SingleWriter = false,
            SingleReader = true,
            AllowSynchronousContinuations = true
        };

        readonly PipeWriter _writer;
        readonly Channel<(IMemoryOwner<byte> Owner, int Length)> _channel = Channel.CreateUnbounded<(IMemoryOwner<byte> Owner, int Length)>(unboundedChannelOptions);

        Task? _task;
        CancellationTokenSource? _stop;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AsyncDiagnosticChannelWriter(PipeWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Enqueues the given memory to be written. Ownership is transfered.
        /// </summary>
        /// <param name="owner"></param>
        public void Write(IMemoryOwner<byte> owner, int length)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (_stop == null || _task == null)
            {
                lock (this)
                {
                    if (_stop == null || _task == null)
                    {
                        _stop = new CancellationTokenSource();
                        _task = DequeueLoop(_stop.Token);
                    }
                }
            }

            _channel.Writer.TryWrite((owner, length));
        }

        /// <summary>
        /// Loops until cancelled.
        /// </summary>
        async Task DequeueLoop(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    while (await _channel.Reader.WaitToReadAsync(cancellationToken))
                        while (_channel.Reader.TryRead(out var item))
                            await WriteData(item.Owner, item.Length, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // ignore
                }
            }
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="length"></param>
        async ValueTask WriteData(IMemoryOwner<byte> owner, int length, CancellationToken cancellationToken)
        {
            // allocate memory and copy data
            var buffer = _writer.GetMemory(length);
            owner.Memory.Slice(0, length).CopyTo(buffer);
            _writer.Advance(length);
            await _writer.FlushAsync(cancellationToken);

            // we are finished with the owner
            owner.Dispose();
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                // stop dequeue task
                _stop?.Cancel();
                _task?.GetAwaiter().GetResult();
                _stop = null;
                _task = null;

                // empty the channel to pick up any straggling items not caught by the dequeue thread
                while (_channel.Reader.TryRead(out var item))
                    item.Owner.Dispose();

                // complete the channel
                try
                {
                    _channel.Writer.Complete();
                }
                catch (ChannelClosedException)
                {
                    // ignore
                }
            }
        }

    }

}
