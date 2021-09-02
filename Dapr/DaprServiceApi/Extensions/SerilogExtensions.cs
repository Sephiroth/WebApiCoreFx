using Serilog;
using Serilog.Events;
using System;

namespace DaprServiceApi.Extensions
{
    public static class SerilogExtensions
    {
        /// <summary>
        /// 本地日志配置
        /// </summary>
        public static LoggerConfiguration LocalConf
        {
            get
            {
                _localConf ??= new LoggerConfiguration();
                return _localConf;
            }
            private set { }
        }
        private static LoggerConfiguration _localConf;

        public static LoggerConfiguration ConfigLocalLogger(this LoggerConfiguration configuration)
        {
            string LogFilePath(string LogEvent) => $@"{AppContext.BaseDirectory}\log\{LogEvent}\log.log";
            //存储日志文件的格式
            string SerilogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}{NewLine}Message:{Message}{NewLine}{Exception}{NewLine}";
            return configuration.MinimumLevel.Debug()
                .Enrich.FromLogContext()//使用Serilog.Context.LogContext中的属性丰富日志事件。
                .WriteTo.Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(LogFilePath("Info"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(LogFilePath("Warn"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(LogFilePath("Error"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate));
        }

    }
}