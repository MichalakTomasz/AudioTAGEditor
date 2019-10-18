using AudioTAGEditor.Models;
using System;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public class LogService : ILogService
    {
        private readonly Dictionary<LogMessageStatusType, string> logs =
            new Dictionary<LogMessageStatusType, string>();
        public Tuple<LogMessageStatusType, string> Add(
            LogMessageStatusType logMessageStesusType, string message)
        {
            logs.Add(logMessageStesusType, message);
            return Tuple.Create(logMessageStesusType, message);
        }
    }
}
