using System.Diagnostics;

namespace JWCommons.Tools.Logging
{
    public interface IJWEventLog
    {
        string AppName { get; set; }
        eCategory Category { get; set; }
        TraceLevel TraceLevel { get; set; }

        void WriteLogEntry(int eventId, string message, eCategory category, EventLogEntryType eventLogType);
    }
}