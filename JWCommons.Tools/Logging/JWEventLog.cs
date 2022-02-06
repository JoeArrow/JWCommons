#region Copyright © 2017 JWCommons
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JWCommons.Tools.Logging
{
    [Serializable]
    [System.ComponentModel.DesignerCategory("Code")]
    public class JWEventLog : EventLog
    {
        public string AppName { set; get; }
        public eCategory Category { set; get; }
        public TraceLevel TraceLevel { set; get; }

        // ------------------------------------------------

        public JWEventLog(string appName, string logName = "Application")
        {
            Initialize(appName, logName);
        }

        // ------------------------------------------------

        private void Initialize(string appName, string logName = "Application")
        {
            Category = eCategory.None;

            try
            {
                if(string.IsNullOrEmpty(appName))
                {
                    AppName = "Unknown Event source";
                }
                else
                {
                    AppName = appName;
                }

                if(!SourceExists(AppName))
                {
                    try
                    {
                        // ---------------------------------------------
                        // This call requires elevated rights on Servers

                        CreateEventSource(appName, logName);
                    }
                    catch(Exception)
                    {
                        // Do Nothing...
                    }
                }
            }
            catch(Exception)
            {
                // On error resume next
            }
        }

        // ------------------------------------------------
        /// <summary>
        ///     This public method uses the TraceLevel to
        ///     determine if the message needs to be sent to 
        ///     a private method, which would actually write
        ///     the message to the log.
        /// </summary>
        /// <param name="message">Body of the Event Message</param>
        /// <param name="eventLogType">Log Type (Error, Information, etc)</param>
        /// <param name="eventId">An Integer ID for this event</param>
        /// <param name="category"></param>

        public void WriteLogEntry(int eventId,
                                  string message,
                                  eCategory category,
                                  EventLogEntryType eventLogType)
        {
            switch(TraceLevel)
            {
                case TraceLevel.Error:

                    // ---------------------------
                    // Only Errors will be Logged.

                    if(eventLogType == EventLogEntryType.Error)
                    {
                        WriteToEventLog(eventId, message, category, eventLogType);
                    }
                    break;

                case TraceLevel.Warning:

                    // --------------------------------------
                    // Only Errors and Warnings will be Logged.

                    if(eventLogType == EventLogEntryType.Error ||
                       eventLogType == EventLogEntryType.Warning)
                    {
                        WriteToEventLog(eventId, message, category, eventLogType);
                    }
                    break;

                default:

                    WriteToEventLog(eventId, message, category, eventLogType);
                    break;
            }
        }

        // ------------------------------------------------
        /// <summary>
        ///     Write an event to the EventLog
        /// </summary>
        /// <param name="Message">Body of the Event Message</param>
        /// <param name="EventLogType">Log Type (Error, Information, etc)</param>
        /// <param name="eventId">An Integer ID for this event</param>
        /// <param name="category">Category (Network, Critical, Benign, etc)</param>

        private void WriteToEventLog(int eventId,
                                     string messageBody,
                                     eCategory category,
                                     EventLogEntryType EventLogType)
        {
            Source = AppName;

            try
            {
                WriteEntry(messageBody, EventLogType, eventId, (short)category);
            }
            catch(Exception)
            {
                // --------------------
                // On error resume next
            }
        }
    }
}
