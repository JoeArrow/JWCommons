using System.Collections.Generic;
using System.Diagnostics;

namespace JWCommons.Tools.Logging
{
    public interface IJWEvent
    {
        string Action { get; set; }
        string AppName { get; set; }
        string Attribute { get; set; }
        eCategory Category { get; set; }
        string CourseOfAction { get; set; }
        string EndUserMessage { get; set; }
        int ErrorCode { get; set; }
        int EventID { get; set; }
        string EventSource { get; set; }
        TraceLevel EventTraceLevel { get; set; }
        EventLogEntryType EventType { get; set; }
        string HttpContextData { get; set; }
        string MethodName { get; set; }
        List<MethodParameter> Parameters { get; set; }
        string SourceFile { get; set; }
        string TargetUser { get; set; }
        string TechMessage { get; set; }
        string User { get; set; }

        bool Log();
        string ToString();
    }
}