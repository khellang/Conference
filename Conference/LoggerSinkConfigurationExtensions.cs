using System;
using Serilog;
using Serilog.Configuration;

namespace Conference
{
    public static class LoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration AzureWebAppDiagnostics(this LoggerSinkConfiguration config) =>
            config.File(
                @"D:\home\LogFiles\Application\log.txt",
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1));
    }
}
