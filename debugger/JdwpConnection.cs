using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ikvm.debugger
{
    class JdwpConnection
    {
        private readonly JdwpParameters parameters;

        private TcpClient client;

        BufferedStream stream;

        private readonly byte[] readHeader = new byte[11];

        internal JdwpConnection(JdwpParameters parameters)
        {
            this.parameters = parameters;
        }

        internal void Connect()
        {
            if (parameters.Server)
            {
                IPHostEntry ipEntry = Dns.GetHostEntry(parameters.Host);
                IPAddress ipAddress = ipEntry.AddressList[0];
                TcpListener listener = new TcpListener(ipAddress, parameters.Port);
                listener.Start();
                client = listener.AcceptTcpClient();
            }
            else
            {
                client = new TcpClient(parameters.Host, parameters.Port);
            }
            stream = new BufferedStream(client.GetStream());
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] hello = encoding.GetBytes("JDWP-Handshake");
            stream.Write(hello, 0, hello.Length);
            byte[] b = new byte[hello.Length];
            int received = 0;
            while (received < b.Length)
            {
                int n = stream.Read(b, received, b.Length - received);
                if (n < 0)
                {
                    client.Close();
                    Console.WriteLine("handshake failed - connection prematurally closed");
                    Environment.Exit(2);
                }
                received += n;
            }
            for (int j = 0; j < hello.Length; j++)
            {
                if (b[j] != hello[j])
                {
                    client.Close();
                    Console.WriteLine("handshake failed - unrecognized message from target VM");
                    Environment.Exit(2);
                }
            }
        }

        internal Packet ReadPacket()
        {
            lock (readHeader)
            {
                DebuggerUtils.ReadFully(stream, readHeader);
                return Packet.Read(readHeader, stream);
            }
        }
    }
}
