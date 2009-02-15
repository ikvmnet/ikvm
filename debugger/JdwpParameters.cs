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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ikvm.debugger
{

    class JdwpParameters
    {
        private String host;

        private int port;

        private bool server;

        private bool suspend = true;

        /// <summary>
        /// Parse the command line parameters. 
        /// Terminate the program if invalid or help parameter.
        /// </summary>
        /// <param name="command">a JDWP parameter</param>
        internal void Parse(String command)
        {
            int length = 0;
            if (command.StartsWith("-Xrunjdwp:"))
            {
                length = "-Xrunjdwp:".Length;
            }
            else if (command.StartsWith("-agentlib:jdwp="))
            {
                length = "-agentlib:jdwp=".Length;
            }

            if (length == 0)
            {
                PrintHelp();
                Exit(1);
            }

            String str = command.Substring(length);
            String[] tokens = str.Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                String token = tokens[i].Trim();
                if ("help".Equals(token))
                {
                    PrintHelp();
                    Exit(0);
                }
                String[] keyValue = token.Split('=');
                if (keyValue.Length != 2)
                {
                    Console.WriteLine("Sytax error with: " + token);
                    PrintHelp();
                    Exit(1);
                }
                String key = keyValue[0].Trim();
                String value = keyValue[1].Trim();
                switch (key)
                {
                    case "transport":
                        if (!value.Equals("dt_socket"))
                        {
                            Console.WriteLine("Only transport=dt_socket is supported.");
                            Exit(1);
                        }
                        break;
                    case "address":
                        String[] hostPort = value.Split(':');
                        if (hostPort.Length != 2)
                        {
                            Console.WriteLine("Wrong address: " + value);
                            Exit(1);
                        }
                        host = hostPort[0];
                        port = Int32.Parse(hostPort[1]);
                        break;
                    case "server":
                        server = "y".Equals(value);
                        break;
                    case "suspend":
                        suspend = "y".Equals(value);
                        break;
                    default:
                        Console.WriteLine("Not supported parameter: " + key);
                        PrintHelp();
                        Exit(1);
                        break;
                }
            }
        }

        internal String Host
        {
            get { return host; }
        }

        internal int Port
        {
            get { return port; }
        }

        /// <summary>
        /// If the debugger should listen as server.
        /// true - listen as server;
        /// false - connect to the debugger;
        /// </summary>
        internal bool Server
        {
            get { return server; }
        }

        /// <summary>
        /// If a instance does not want exit the application then it must override this method.
        /// </summary>
        /// <param name="code">exit code</param>
        private void Exit(int code)
        {
            Environment.Exit(code);
        }

        /// <summary>
        /// Print the usage help to the console
        /// </summary>
        private void PrintHelp()
        {
            Console.WriteLine("               Java Debugger JDWP Agent Library");
            Console.WriteLine("               --------------------------------");
            Console.WriteLine("");
            Console.WriteLine("  (see http://java.sun.com/products/jpda for more information)");
            Console.WriteLine("");
            Console.WriteLine("jdwp usage: ikvm -agentlib:jdwp=[help]|[<option>=<value>, ...]");
            Console.WriteLine("");
            Console.WriteLine("Option Name and Value            Description                       Default");
            Console.WriteLine("---------------------            -----------                       -------");
            Console.WriteLine("suspend=y|n                      wait on startup?                  y");
            Console.WriteLine("transport=<name>                 transport spec                    none");
            Console.WriteLine("address=<listen/attach address>  transport spec                    \"\"");
            Console.WriteLine("server=y|n                       listen for debugger?              n");
            Console.WriteLine("launch=<command line>            run debugger on event             none");
            Console.WriteLine("onthrow=<exception name>         debug on throw                    none");
            Console.WriteLine("onuncaught=y|n                   debug on any uncaught?            n");
            Console.WriteLine("timeout=<timeout value>          for listen/attach in milliseconds n");
            Console.WriteLine("mutf8=y|n                        output modified utf-8             n");
            Console.WriteLine("quiet=y|n");
        }

    }
}
