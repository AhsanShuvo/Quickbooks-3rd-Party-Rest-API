using Serilog;
using Serilog.Events;
using System;
using System.Web.Hosting;

namespace QuickbooksCommon.Logger
{
    public static class Logger
    {
        private static readonly ILogger Debug;
        private static readonly ILogger Error;

        static Logger()
        {
            // 5 mb = 5242880 bytes
            Debug = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(HostingEnvironment.MapPath("~/ErrorLog/Debug/log.txt"),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 5242880,
                    rollOnFileSizeLimit: true)
                    .CreateLogger();

            Error = new LoggerConfiguration()
                    .MinimumLevel.Error()
                   .WriteTo.File(HostingEnvironment.MapPath("~/ErrorLog/Error/log.txt"),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 5242880,
                    rollOnFileSizeLimit: true)
                    .CreateLogger();
        }

        public static void WriteDebug(string message)
        {
            Debug.Write(LogEventLevel.Debug, message);
        }

        public static void WriteError(Exception e, string message)
        {
            Error.Write(LogEventLevel.Error, e.StackTrace, message);
        }
    }
}
