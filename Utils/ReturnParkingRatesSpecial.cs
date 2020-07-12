using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CalcParkingFee
{
    public static class ReturnParkingRatesSpecial
    {
        [FunctionName("ReturnParkingRatesSpecial")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function ReturnParkingRatesSpecial processed a request.");

            //Asume its coming from dynamic 'Rates service'
            string jsonToReturn = "[{  \"name\": \"Early Bird\",  \"type\": \"Flat Rate\",  \"totalPrice\": 13.00,  \"entry\": {\"start\": \"6:00\",\"end\": \"9:00\",\"days\": [\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\"]  },  \"exit\": {\"start\": \"15:30\",\"end\": \"23:30\",\"days\": [\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\"]},\"maxDays\":  0  },  {\"name\": \"Night Rate\",\"type\": \"Flat Rate\",\"totalPrice\": 6.50,\"entry\": {\"start\": \"18:00\",\"end\": \"23:59\",\"days\": [ \"Monday\",  \"Tuesday\",  \"Wednesday\",  \"Thursday\",  \"Friday\"]},\"exit\": {\"start\": \"15:30\",\"end\": \"23:30\",\"days\": [  \"Sunday\",  \"Monday\",  \"Tuesday\",  \"Wednesday\",  \"Thursday\",  \"Friday\",  \"Saturday\"]},\"maxDays\": 1  },  {\"name\": \"Weekend Rate\",\"type\": \"Flat Rate\",\"totalPrice\": 10.00,\"entry\": {\"start\": \"00:00\",\"end\": \"23:59\",\"days\": [  \"Sunday\",  \"Saturday\"]},\"exit\": {\"start\": \"00:00\",\"end\": \"23:59\",\"days\": [  \"Saturday\",  \"Sunday\",  \"Monday\"]},\"maxDays\": 2   }]";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }

}
