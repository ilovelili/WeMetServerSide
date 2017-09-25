using System;
using System.Diagnostics;
using System.IO;
using Nancy;

namespace NamecardScanner.Logger
{
    public static class SimpleLogger
    {
        private static readonly EventLog EventLog = new EventLog("Application", ".", "ASP.NET 4.0.30319.0");

        /// <summary>
        /// Referrence: http://msdn.microsoft.com/ja-jp/library/ms998320.aspx#paght000015_eventlogaccess
        /// </summary>        
        public static void WriteException(Exception exception, Request request)
        {
            var logMessage = $"Exception:\r\nException Type: {exception.GetType().Name}\r\nException Message: {exception.Message}\r\nInner Exception Message: {exception.InnerException}\r\n{exception.StackTrace}\r\n\r\nRequest Info:\r\nRequest URL:{request.Url}";
            EventLog.WriteEntry(logMessage, EventLogEntryType.Error);
        }

        public static void WriteDevelopLog(string message, EventLogEntryType type)
        {
            var logMessage = $"DEVELOP INFO:{message}";
            EventLog.WriteEntry(logMessage, type);
        }

        public static void WriteFileLog(string message)
        {
            File.AppendAllText(Path.Combine(Config.Config.LogOutput, DateTime.Now.ToString("yyyyMMddHHmm")), $"{DateTime.Now}:{message}\n");
        }
    }
}
