using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessFunctionsAppNETCore20.Tests
{
    public class MockTraceWriter : TraceWriter
    {
        List<TraceEvent> events = new List<TraceEvent>();
        public MockTraceWriter() : base(TraceLevel.Info) { }
        public override void Trace(TraceEvent traceEvent)
        {
            events.Add(traceEvent);
        }
    }
}
