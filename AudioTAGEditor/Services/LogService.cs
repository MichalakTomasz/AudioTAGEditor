using AudioTAGEditor.Models;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public class LogService : ILogService
    {
        private readonly List<LogItem> logs =
            new List<LogItem>();
        public LogItem Add(
            LogMessageStatusType logMessageStesusType, string message)
        {
            var logItem = new LogItem
            {
                Message = message,
                LogMessageStatusType = logMessageStesusType,
            };

            logs.Add(logItem);
            return logItem;
        }
    }
}
