using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Rendering;
using System.Runtime.ExceptionServices;

namespace IKVM.Tools.Core.CommandLine
{

    static class CommandLineBuilderExtensions
    {

        public static CommandLineBuilder UseCommandExceptionHandler(this CommandLineBuilder builder)
        {
            return builder.UseExceptionHandler(OnException, 1);
        }

        static void OnException(Exception exception, InvocationContext context)
        {
            if (exception is CommandErrorException e)
            {
                var terminal = Terminal.GetTerminal(context.Console);
                terminal.Render(new ContainerSpan(ForegroundColorSpan.Red(), new ContentSpan(e.Message), ForegroundColorSpan.Reset()));
                return;
            }

            // rethrow exception
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

    }

}
