#region © 2017 JWCommons.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Web;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace JWCommons.Tools.Logging
{
    public class JWEvent : ApplicationException, IJWEvent
    {
        #region Member Variables and Properties

        public int EventID { get; set; }
        public string User { get; set; }
        public int ErrorCode { get; set; }
        public string Action { set; get; }
        public string AppName { get; set; }
        public string Attribute { set; get; }
        public string TargetUser { get; set; }
        public string SourceFile { set; get; }
        public string MethodName { set; get; }
        public string EventSource { set; get; }
        public string TechMessage { set; get; }
        public eCategory Category { set; get; }
        public string EndUserMessage { set; get; }
        public string CourseOfAction { set; get; }
        public string HttpContextData { set; get; }
        public TraceLevel EventTraceLevel { set; get; }
        public EventLogEntryType EventType { set; get; }
        public List<MethodParameter> Parameters { set; get; }

        #endregion

        // ------------------------------------------------

        public JWEvent(int eventID,
                          string eventSource,
                          string message = "No Message Provided...",
                          Exception exp = null)
            : base(message, exp)
        {
            EventID = eventID;
            EventSource = eventSource;
            EventType = EventLogEntryType.Error;
            Parameters = new List<MethodParameter>();
        }

        // ------------------------------------------------
        /// <summary>
        ///     Used to save the ToString() output to the 
        ///     event log.
        /// </summary>

        public bool Log()
        {
            var retVal = true;
            var eLog = new JWEventLog(EventSource, AppName) { TraceLevel = EventTraceLevel };

            try
            {
                eLog.WriteLogEntry(EventID, ToString(), Category, EventType);
            }
            catch(Exception)
            {
                retVal = false;
            }

            DebugOutput();

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     Output to the IDE when running in Debug Mode.
        /// </summary>

        [Conditional("DEBUG")]
        private void DebugOutput()
        {
            Debug.Write(ToString());
        }

        // ------------------------------------------------
        /// <summary>
        ///     Used to obtain pertinant data from an HTTP Context,
        ///     If it is available. This will return data only
        ///     if the underlying call was web based.
        /// </summary>
        /// <returns></returns>

        [ExcludeFromCodeCoverage]
        private NameValueCollection GetHttpContextData()
        {
            NameValueCollection retVal = new NameValueCollection();

            try
            {
                HttpContext ctx = HttpContext.Current;

                if(ctx != null)
                {
                    retVal.Add("Page", ctx.Request.Path == null ? string.Empty : ctx.Request.Path);
                    retVal.Add("User_Id", ctx.Request.Headers["UID"] == null ? string.Empty : ctx.Request.Headers["UID"]);
                    retVal.Add("HTTP_Host", ctx.Request.ServerVariables["HTTP_HOST"] == null ? string.Empty : ctx.Request.ServerVariables["HTTP_HOST"]);
                    retVal.Add("Local_Addr", ctx.Request.ServerVariables["LOCAL_ADDR"] == null ? string.Empty : ctx.Request.ServerVariables["LOCAL_ADDR"]);
                    retVal.Add("Remote_Addr", ctx.Request.ServerVariables["REMOTE_ADDR"] == null ? string.Empty : ctx.Request.ServerVariables["REMOTE_ADDR"]);
                    retVal.Add("Remote_User", ctx.Request.ServerVariables["REMOTE_USER"] == null ? string.Empty : ctx.Request.ServerVariables["REMOTE_USER"]);
                    retVal.Add("Logged_On_User", ctx.Request.ServerVariables["LOGON_USER"] == null ? string.Empty : ctx.Request.ServerVariables["LOGON_USER"]);
                }
            }
            catch
            {
                // Do Nothing, we just don't get the context...
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     ToString() Override.
        /// </summary>
        /// <returns>
        ///     A fromatted string containing information about this exception.
        /// </returns>
        /// <example>
        /// <code>
        /// -----------------------------------------------------------
        ///     Begin JWCommons Event
        /// -----------------------------------------------------------
        ///
        /// Source file name:
        ///     AccountProfile.cs
        ///
        /// Method Name:
        ///     AccountProfile
        ///
        /// Error Code:
        ///     -1
        ///
        /// Technical Message:
        ///     The WebMethods call getAccountInfoList returned -1
        ///     WebMethods URL Used: http://dcaib11:5555/soap/rpc
        ///
        /// Message:
        ///     WebMethods call 'getAccountInfoList' returned -1 for account AA092.
        ///
        ///
        /// End User Message:
        ///     Unable to retrieve Account Information.
        ///
        ///
        /// Recommended Course Of Action:
        ///     Verify that the account 'AA092' is listed in GroupMaster.
        ///
        /// StackTrace:
	    ///
        ///
        /// -----------------------------------------------------------
        ///     End of JWCommons Event
        /// -----------------------------------------------------------
        /// </code>
        /// </example>

        public override string ToString()
        {
            // ------------------------------------------------
            // Each property will be output in a common format.

            string outputFormat = "{0}:\r\n\t{1}\r\n\r\n";

            // ---------------------------------------------------------
            // The beginning and end of an event will be clearly marked.

            string msgStart = string.Format("{0}{3}{0}\tBegin {1} Event - EventID: {2}{0}{3}{0}{0}",
                                            Environment.NewLine,
                                            EventSource,
                                            EventID,
                                            string.Empty.PadLeft(104, '-'));

            string msgEnd = string.Format("{0}{3}{0}\tEnd {1} Event - EventID: {2}{0}{3}{0}",
                                            Environment.NewLine,
                                            EventSource,
                                            EventID,
                                            string.Empty.PadLeft(104, '-'));

            var sb = new StringBuilder();

            sb.Append(msgStart);

            Type obj = GetType();
            var properties = obj.GetProperties();

            // -----------------------------------------------
            // Step through each property in the event object.

            foreach(PropertyInfo prop in properties)
            {
                try
                {
                    object val = prop.GetValue(this, null);

                    if(val is List<MethodParameter>)
                    {
                        var list = (List<MethodParameter>)val;

                        if(list.Count > 0)
                        {
                            sb.AppendFormat("Parameter:{0}", Environment.NewLine);

                            foreach(var parm in list)
                            {
                                sb.AppendFormat("\t{1}{0}{0}", Environment.NewLine, parm.ToString());
                            }
                        }
                    }
                    else if(val != null && val.ToString() != string.Empty)
                    {
                        // -------------------------------------------
                        // Only output the property if it has a value.

                        sb.AppendFormat(outputFormat, prop.Name, val.ToString());
                    }
                }
                catch(Exception ex)
                {
                    sb.AppendFormat(outputFormat, prop.Name, ex.Message);
                }
            }

            sb.Append(msgEnd);

            return sb.ToString();
        }
    }
}
