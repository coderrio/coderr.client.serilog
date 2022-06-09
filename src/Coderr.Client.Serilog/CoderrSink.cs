using System;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;
using Coderr.Client.Serilog.ContextProviders;
using Serilog.Core;
using Serilog.Events;

namespace Coderr.Client.Serilog
{
    /// <summary>
    /// Coderr sink for Serilog.
    /// </summary>
    /// <remarks>
    ///<para>
    ///Allows Coderr to report exceptions from serilog log entries and to be able to attach logs to all errors detected by Coderr.
    /// </para>
    /// </remarks>
    public class CoderrSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// Creates a new instance of <see cref="CoderrSink"/>.
        /// </summary>
        /// <param name="formatProvider">Optional format provider for log messages.</param>
        public CoderrSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        /// <inheritdoc />
        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
            var entry = new LogEntryDto(logEvent.Timestamp.ToUniversalTime().DateTime, ConvertLevel(logEvent.Level),
                message)
            {
                Exception = logEvent.Exception?.ToString(),
            };
            LogsProvider.Instance.Add(entry);

            if (logEvent.Exception == null)
            {
                return;
            }

            IErrorReporterContext context = new ErrorReporterContext(this, logEvent.Exception);
            var dataCollection = new {
                LogLevel = logEvent.Level,
                Message = message,
                logEvent.Properties
            }.ToContextCollection("LogEntry");
            context.ContextCollections.Add(dataCollection);

            var coderrCollection = context.ContextCollections.GetCoderrCollection();
            coderrCollection.Properties[CoderrCollectionProperties.HighlightCollection] = "LogEntry";

            Err.Report(context);
        }

        private int ConvertLevel(LogEventLevel logEventLevel)
        {
            switch (logEventLevel)
            {
                case LogEventLevel.Fatal:
                    return 5;
                case LogEventLevel.Error:
                    return 4;
                case LogEventLevel.Warning:
                    return 3;
                case LogEventLevel.Information:
                    return 2;
                case LogEventLevel.Debug:
                    return 1;
                default:
                    return 0;

            }
        }
    }

}
