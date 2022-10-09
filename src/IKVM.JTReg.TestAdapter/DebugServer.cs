using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Listens to a random port for signals from child processes.
    /// </summary>
    public class DebugServer : IDisposable
    {

        readonly object syncRoot = new object();
        readonly IRunContext runContext;
        readonly IFrameworkHandle2 frameworkHandle;
        UdpClient client;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DebugServer(IRunContext runContext, IFrameworkHandle2 frameworkHandle)
        {
            this.runContext = runContext ?? throw new ArgumentNullException(nameof(runContext));
            this.frameworkHandle = frameworkHandle ?? throw new ArgumentNullException(nameof(frameworkHandle));
        }

        /// <summary>
        /// Starts listening for signals.
        /// </summary>
        public void Start()
        {
            lock (syncRoot)
            {
                client = new UdpClient(new IPEndPoint(IPAddress.Loopback, 0));
                client.BeginReceive(OnReceive, null);
            }
        }

        /// <summary>
        /// Stops the debug server.
        /// </summary>
        public void Stop()
        {
            lock (syncRoot)
            {
                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }
        }

        /// <summary>
        /// Gets the local endpoint of the server.
        /// </summary>
        public IPEndPoint EndPoint
        {
            get => (IPEndPoint)client?.Client.LocalEndPoint;
        }

        /// <summary>
        /// Invoked when a message is received.
        /// </summary>
        /// <param name="ar"></param>
        void OnReceive(IAsyncResult ar)
        {
            lock (syncRoot)
            {
                try
                {
                    if (client == null)
                        return;

                    // receive existing message
                    var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    var buffer = client.EndReceive(ar, ref remoteEndpoint);
                    if (buffer == null || buffer.Length == 0)
                        return;

                    // receive and process message
                    var message = JsonSerializer.Deserialize<DebugMessage>(buffer);
                    if (message.ProcessId > 0)
                        frameworkHandle.AttachDebuggerToProcess(message.ProcessId);
                }
                finally
                {
                    if (client != null)
                        client.BeginReceive(OnReceive, null);
                }
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
