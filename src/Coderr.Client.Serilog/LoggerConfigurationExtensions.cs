using System;
using Coderr.Client.Config;
using Coderr.Client.NLog.ContextProviders;
using Serilog;
using Serilog.Configuration;

namespace Coderr.Client.Serilog
{
    /// <summary>
    /// Extensions used to add Coderr to Serilog.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Log entries to Coderr.
        /// </summary>
        /// <param name="sinkConfiguration">Serilog configuration.</param>
        /// <param name="configuration">Coderr configuration (typically <c>Err.Configuration</c>).</param>
        /// <param name="formatProvider">Optional log entry formatter.</param>
        /// <returns>Logger configuration object.</returns>
        public static LoggerConfiguration Coderr(
            this LoggerSinkConfiguration sinkConfiguration,
            CoderrConfiguration configuration = null,
            IFormatProvider formatProvider = null)
        {
            var config = sinkConfiguration.Sink(new CoderrSink(formatProvider));
            (configuration ?? Err.Configuration).ContextProviders.Add(LogsProvider.Instance);
            return config;
        }
    }
}