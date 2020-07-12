using System;
using System.Collections.Generic;

namespace CalcParkingFee.Utils
{
    public class EntryExitSpan
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public IEnumerable<string> Days { get; set; }
    }
}
