﻿using System;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Rendering;

namespace IKVM.Tools.Core.CommandLine
{

    class ParseErrorResult : IInvocationResult
    {

        readonly int? _errorExitCode;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="errorExitCode"></param>
        public ParseErrorResult(int? errorExitCode)
        {
            _errorExitCode = errorExitCode;
        }

        /// <inheritdoc />
        public void Apply(InvocationContext context)
        {
            var terminal = context.Console.GetTerminal();
            if (terminal != null)
            {
                terminal.ResetColor();
                terminal.ForegroundColor = ConsoleColor.Red;

                foreach (var error in context.ParseResult.Errors)
                    terminal.Error.WriteLine(error.Message);

                terminal.ResetColor();
            }
            else
            {
                foreach (var error in context.ParseResult.Errors)
                    context.Console.Error.WriteLine(error.Message);
            }

            context.ExitCode = _errorExitCode ?? 1;
        }

    }

}
