using System;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface ILogService
    {
        Tuple<LogMessageStatusType, string> Add(LogMessageStatusType logMessageStesusType, string message);
    }
}