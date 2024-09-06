using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Runtime.ExceptionServices;

namespace IKVM.Tools.Core.CommandLine
{

    static class CommandLineBuilderExtensions
    {

        public static CommandLineBuilder UseToolParseError(this CommandLineBuilder builder, int? errorExitCode = null)
        {
            return builder.AddMiddleware(OnParseErrorReporting(errorExitCode), MiddlewareOrder.ErrorReporting);
        }

        static InvocationMiddleware OnParseErrorReporting(int? errorExitCode)
        {
            return async (context, next) =>
            {
                if (context.ParseResult.Errors.Count > 0)
                    context.InvocationResult = new ParseErrorResult(errorExitCode);
                else
                    await next(context);
            };
        }

        public static CommandLineBuilder UseToolErrorExceptionHandler(this CommandLineBuilder builder)
        {
            return builder.UseExceptionHandler(OnException, 1);
        }

        static void OnException(Exception exception, InvocationContext context)
        {
            if (exception is ToolErrorException e)
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
