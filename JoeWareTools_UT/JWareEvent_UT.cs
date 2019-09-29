#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.IO;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using System.Web.SessionState;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JoeWare.Tools.Logging;

namespace JoeWareTools_UT
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for JoeWareTools_UT
    /// </summary>

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class JWareEvent_UT
    {
        public JWareEvent_UT() { }

        private TestContext testContextInstance;

        // ------------------------------------------------
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        ///</summary>

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // ------------------------------------------------
        //  You can use the following additional attributes 
        //  as you write your tests:

        // ------------------------------------------------
        //  Use ClassInitialize to run code before running 
        //  the first test in the class

        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }

        // ------------------------------------------------
        //  Use ClassCleanup to run code after all tests in 
        //  a class have run

        // [ClassCleanup()]
        // public static void MyClassCleanup() { }

        // ------------------------------------------------
        //  Use TestInitialize to run code before running 
        //  each test 

        // [TestInitialize()]
        // public void MyTestInitialize() { }

        // ------------------------------------------------
        //  Use TestCleanup to run code after each test has 
        //  run

        // [TestCleanup()]
        // public void MyTestCleanup() { }

        #endregion

        // ------------------------------------------------

        [DeploymentItem("Test Data\\JWareEvent_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\JWareEvent_TD.xml",
                    "Logging",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void Logging_JWareEvent_Writes_To_The_Event_Log()
        {
            // -------
            // Arrange

            var eventID = Convert.ToInt32(TestContext.DataRow["EventID"]);
            var message = Convert.ToString(TestContext.DataRow["Message"]);
            var errorCode = Convert.ToInt32(TestContext.DataRow["ErrorCode"]);
            var eventSource = Convert.ToString(TestContext.DataRow["EventSource"]);
            var category = (eCategory)Enum.Parse(typeof(eCategory), Convert.ToString(TestContext.DataRow["Category"]));
            var traceLevel = (TraceLevel)Enum.Parse(typeof(TraceLevel), Convert.ToString(TestContext.DataRow["TraceLevel"]));
            var eventType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), Convert.ToString(TestContext.DataRow["EventType"]));

            var evt = new JWareEvent(eventID, eventSource, message)
            {
                Category = category,
                ErrorCode = errorCode,
                EventType = eventType,
                EventTraceLevel = traceLevel,
                MethodName = MethodBase.GetCurrentMethod().Name,
            };

            evt.Parameters.Add(new MethodParameter() { Name = "Test1", Value = "No Real Value 1" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test2", Value = "No Real Value 2" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test3", Value = "No Real Value 3" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test4", Value = "No Real Value 4" });

            // ---
            // Log

            Console.Write(evt.ToString());

            // ---
            // Act

            var res = evt.Log();

            // ------
            // Assert

            Assert.IsTrue(res);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\JWareEvent_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\JWareEvent_TD.xml",
                    "LoggingAllProperties",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void Logging_All_Properties_JWareEvent_Writes_To_The_Event_Log()
        {
            // -------
            // Arrange

            var eventID = Convert.ToInt32(TestContext.DataRow["EventID"]);
            var message = Convert.ToString(TestContext.DataRow["Message"]);
            var errorCode = Convert.ToInt32(TestContext.DataRow["ErrorCode"]);
            var eventSource = Convert.ToString(TestContext.DataRow["EventSource"]);
            var category = (eCategory)Enum.Parse(typeof(eCategory), Convert.ToString(TestContext.DataRow["Category"]));
            var traceLevel = (TraceLevel)Enum.Parse(typeof(TraceLevel), Convert.ToString(TestContext.DataRow["TraceLevel"]));
            var eventType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), Convert.ToString(TestContext.DataRow["EventType"]));

            var evt = new JWareEvent(eventID, eventSource, message)
            {
                Category = category,
                ErrorCode = errorCode,
                EventType = eventType,
                EventTraceLevel = traceLevel,
                MethodName = MethodBase.GetCurrentMethod().Name,
            };

            evt.Parameters.Add(new MethodParameter() { Name = "Test1", Value = "No Real Value 1" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test2", Value = "No Real Value 2" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test3", Value = "No Real Value 3" });
            evt.Parameters.Add(new MethodParameter() { Name = "Test4", Value = "No Real Value 4" });

            // ---
            // Log

            Console.Write(evt.ToString());

            // ---
            // Act

            var res = evt.Log();

            // ------
            // Assert

            Assert.IsTrue(res);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\JWareEvent_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\JWareEvent_TD.xml",
                    "CreateEventSource",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void Create_New_EventSource_JWareEvent()
        {
            // -------
            // Arrange

            var eventSource = Convert.ToString(TestContext.DataRow["EventSource"]);

            var evt = new JWareEvent(10, eventSource);

            // ---
            // Act

            evt.Log();

            // ------
            // Assert

            Assert.IsTrue(EventLog.SourceExists(eventSource));

            // --------
            // Clean Up

            EventLog.DeleteEventSource(eventSource);

            Assert.IsFalse(EventLog.SourceExists(eventSource));
        }

        // ------------------------------------------------
        /// <summary>
        ///     I got this from the internet.
        ///     I had no idea how to fake an HTTP context.
        ///     
        ///     This is where I found some information:
        ///     
        ///         http://edspencer.me.uk/2013/01/25/mocking-httpcontext-and-setting-its-session-values/
        ///     
        ///     They created an entire class, I just created a method.
        /// </summary>
        /// <returns></returns>

        private HttpContext FakeHttpContext()
        {
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpRequest = new HttpRequest("Bogus.html", "http://about_blank", "bogus=Bogus");
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id",
                                                                 new SessionStateItemCollection(),
                                                                 new HttpStaticObjectsCollection(),
                                                                 10,
                                                                 true,
                                                                 HttpCookieMode.AutoDetect,
                                                                 SessionStateMode.InProc,
                                                                 false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                                      null,
                                                                                      CallingConventions.Standard,
                                                                                      new[] { typeof(HttpSessionStateContainer) },
                                                                                      null).Invoke(new object[] { sessionContainer });
            httpContext.ApplicationInstance = new HttpApplication();

            return httpContext;
        }
    }
}
