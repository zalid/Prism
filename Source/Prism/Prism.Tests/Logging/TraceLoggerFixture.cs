//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces.Logging;
using Prism.Logging;

namespace Prism.Tests.Logging
{
    [TestClass]
    public class TraceLoggerFixture
    {

        [TestMethod]
        public void ShouldWriteToTraceWriter()
        {
            var listener = new MockTraceListener();
            Trace.Listeners.Add(listener);

            var traceLogger = new TraceLogger();
            traceLogger.Log("Blah", Category.Debug, Priority.Low);

            Assert.AreEqual<string>("Blah",listener.LogMessage);

            Trace.Listeners.Remove(listener);
        }


        [TestMethod]
        public void ShouldTraceErrorException()
        {
            var listener = new MockTraceListener();
            Trace.Listeners.Add(listener);

            var traceLogger = new TraceLogger();
            traceLogger.Log("Blah", Category.Exception, Priority.Low);

            Assert.AreEqual<string>("Blah", listener.ErrorMessage);

            Trace.Listeners.Remove(listener);
        }
    }

    class MockTraceListener : TraceListener
    {
        public string LogMessage { get; set; }
        public string ErrorMessage { get; set; }

        public override void Write(string message)
        {
            LogMessage = message;
        }

        public override void WriteLine(string message)
        {
            LogMessage = message;
        }

        public override void WriteLine(string message, string category)
        {
            LogMessage = message;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (eventType == TraceEventType.Error)
            {
                ErrorMessage = message;
            }
            else
            {
                LogMessage = message;
            }
        }
    }
}
