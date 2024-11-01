using System;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Diagnostics;

using Spectre.Console;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Outputs diagnostics to the console using Spectre.
    /// </summary>
    class ConsoleDiagnosticFormatter : IDiagnosticFormatter
    {

        static readonly Markup TRACE = new Markup("[grey42]TRACE[/]");
        static readonly Markup INFO = new Markup("[silver]INFO[/]");
        static readonly Markup WARNING = new Markup("[yellow]WARNING[/]");
        static readonly Markup ERROR = new Markup("[maroon]ERROR[/]");
        static readonly Markup FATAL = new Markup("[red]FATAL[/]");

        readonly ConsoleDiagnosticFormatterOptions _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public ConsoleDiagnosticFormatter(ConsoleDiagnosticFormatterOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public bool CanWrite(in DiagnosticEvent @event)
        {
            return true;
        }

        /// <inheritdoc />
        public void Write(in DiagnosticEvent @event)
        {
            // skip events we cannot write
            if (CanWrite(@event) == false)
                return;

            try
            {
                WriteDiagnosticEvent(@event);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual Markup GetDiagnosticLevelMarkup(DiagnosticLevel level)
        {
            return level switch
            {
                DiagnosticLevel.Trace => TRACE,
                DiagnosticLevel.Info => INFO,
                DiagnosticLevel.Warning => WARNING,
                DiagnosticLevel.Error => ERROR,
                DiagnosticLevel.Fatal => FATAL,
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void WriteDiagnosticLevel(DiagnosticLevel level)
        {
            AnsiConsole.Write(GetDiagnosticLevelMarkup(level));
        }

        /// <summary>
        /// Formats the output text for the given diagnostic code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual void WriteDiagnosticCode(DiagnosticLevel level, int code)
        {
            AnsiConsole.Write("IKVM");
            AnsiConsole.Write($"{code:D4}");
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected virtual void WriteDiagnosticMessage(DiagnosticLevel level, string message, ReadOnlySpan<object?> args)
        {
            AnsiConsole.MarkupInterpolated(FormattableStringFactory.Create(message, args.ToArray()));
        }

        /// <summary>
        /// Formats the <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="event"></param>
        protected virtual void WriteDiagnosticEvent(in DiagnosticEvent @event)
        {
            WriteDiagnosticLevel(@event.Diagnostic.Level);
            AnsiConsole.Write(" ");
            WriteDiagnosticCode(@event.Diagnostic.Level, @event.Diagnostic.Id);
            AnsiConsole.Write(": ");
            WriteDiagnosticMessage(@event.Diagnostic.Level, @event.Diagnostic.Message, @event.Args);
            AnsiConsole.WriteLine();

            if (@event.Exception != null)
                AnsiConsole.WriteException(@event.Exception, ExceptionFormats.Default);
        }

    }

}
