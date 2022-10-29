using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Listens to a random port for signals from child processes.
    /// </summary>
    public class IkvmTraceServer : IDisposable
    {

        readonly object syncRoot = new object();
        readonly IJTRegExecutionContext context;
        TcpListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmTraceServer(IJTRegExecutionContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Starts listening for signals.
        /// </summary>
        public void Start()
        {
            lock (syncRoot)
            {
                listener = new TcpListener(new IPEndPoint(IPAddress.Loopback, 0));
                listener.Start();
                listener.BeginAcceptSocket(OnAcceptSocket, null);
            }
        }

        /// <summary>
        /// Stops the debug server.
        /// </summary>
        public void Stop()
        {
            lock (syncRoot)
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
            }
        }

        /// <summary>
        /// Gets the local endpoint of the server.
        /// </summary>
        public Uri Uri
        {
            get => listener != null ? new UriBuilder("tcp", ((IPEndPoint)listener.LocalEndpoint).Address.ToString(), ((IPEndPoint)listener.LocalEndpoint).Port).Uri : null;
        }

        /// <summary>
        /// Invoked when a new client connects.
        /// </summary>
        /// <param name="ar"></param>
        void OnAcceptSocket(IAsyncResult ar)
        {
            lock (syncRoot)
            {
                try
                {
                    if (listener == null)
                        return;

                    var socket = listener.EndAcceptSocket(ar);
                    var pipe = new Pipe();
                    Task writing = FillPipeAsync(socket, pipe.Writer);
                    Task reading = ReadPipeAsync(pipe.Reader);
                    Task.Run(() => Task.WhenAll(reading, writing));
                }
                catch (Exception e)
                {
                    context.SendMessage(JTRegTestMessageLevel.Error, $"Exception accepting TCP socket for trace.\n{e}");
                }

                listener.BeginAcceptSocket(OnAcceptSocket, null);
            }
        }

        /// <summary>
        /// Task to handle receiving data from client.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        async Task FillPipeAsync(Socket client, PipeWriter writer)
        {
            while (true)
            {
                var memory = writer.GetMemory(512);

                try
                {
#if NETFRAMEWORK
                    // framwork cannot receive directly into Memory, so receive into temporary array
                    var netBuffer = ArrayPool<byte>.Shared.Rent(memory.Length);
                    var bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(netBuffer, 0, memory.Length), SocketFlags.None);
                    netBuffer.CopyTo(memory);
                    ArrayPool<byte>.Shared.Return(netBuffer);
#else
                    var bytesRead = await client.ReceiveAsync(memory, SocketFlags.None);
#endif
                    if (bytesRead == 0)
                        break;

                    writer.Advance(bytesRead);
                }
                catch (Exception e)
                {
                    context.SendMessage(JTRegTestMessageLevel.Error, $"Exception reading socket for Trace.\n{e}");
                    break;
                }

                var result = await writer.FlushAsync();
                if (result.IsCompleted)
                    break;
            }
        }

        /// <summary>
        /// Task to handle processing data from client.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                while (TryReadMessage(ref buffer, out ReadOnlySequence<byte> message))
                    ProcessMessage(message);

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                    break;
            }
        }

        /// <summary>
        /// Attempts to rad a complete message from the buffer, searching for a NULL.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool TryReadMessage(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> message)
        {
            var position = buffer.PositionOf((byte)'\0');
            if (position == null)
            {
                message = default;
                return false;
            }

            // select the results up to the NULL and skip
            message = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        /// <summary>
        /// Handles a completed message buffer.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        void ProcessMessage(ReadOnlySequence<byte> buffer)
        {
            try
            {
                // receive and process message
                var reader = new Utf8JsonReader(buffer);
                var @event = JsonSerializer.Deserialize<IkvmStartEvent>(ref reader);
                if (@event != null)
                    HandleStartEvent(@event);
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"Exception reading message for trace.\n{e}");
            }
        }

        /// <summary>
        /// Handles a start event from IKVM.
        /// </summary>
        /// <param name="event"></param>
        void HandleStartEvent(IkvmStartEvent @event)
        {
            try
            {
                if (@event.ProcessId > 0)
                    context.AttachDebuggerToProcess(@event.ProcessId);
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"Exception handling IKVM start event for trace.\n{e}");
            }
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

    }

}
