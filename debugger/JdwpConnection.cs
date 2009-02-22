/*
  Copyright (C) 2009 Volker Berlin (vberlin@inetsoftware.de)

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
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

        Stream stream;

        /// <summary>
        /// Shared buffer for reading and monitor for reading
        /// </summary>
        private readonly byte[] readHeader = new byte[11];

        private readonly Object writeMonitor = new Object();

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
            stream = client.GetStream(); //TODO Bug in BufferedStream, work not asynchron
            //stream = new BufferedStream(client.GetStream());
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

        internal void SendPacket(Packet packet)
        {
            lock (writeMonitor)
            {
                packet.Send(stream);
            }
        }
    }
}
