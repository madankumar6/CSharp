using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.Common.Model
{
    public enum ProtocolParserStatus
    {
        Initialized = 0,
        InSufficientData = 1,
        Parsed = 2,
        Saved = 3,
        Finished = 4
    }
}
