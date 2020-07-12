namespace CalcParkingFee.Utils
{
    public static class CONSTANTS
    {
        public class ProjectURLs
        {
            public static string CalcNormalRates = "http://localhost:7071/api/CalcNormalRates";
            public static string CalcSpecialRates = "http://localhost:7071/api/CalcSpecialRates";
            public static string ReturnParkingRatesNormal = "http://localhost:7071/api/ReturnParkingRatesNormal";
            public static string ReturnParkingRatesSpecial = "http://localhost:7071/api/ReturnParkingRatesSpecial";
        }
        public class RateType
        {
            public const string STANDARD = "Standard Rate";
        }
    }
}
