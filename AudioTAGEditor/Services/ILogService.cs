using System;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface ILogService
    {
        LogItem Add(LogMessageStatusType logMessageStesusType, string message);
    }
}