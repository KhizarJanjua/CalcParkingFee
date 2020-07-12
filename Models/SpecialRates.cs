using CalcParkingFee.Utils;

namespace CalcParkingFee.Models
{
    public class SpecialRate
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double TotalPrice { get; set; }
        public EntryExitSpan Entry { get; set; }
        public EntryExitSpan Exit { get; set; }
        public int MaxDays { get; set; }
    }

}
