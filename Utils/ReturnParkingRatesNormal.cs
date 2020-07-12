using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CalcParkingFee
{
    public static class ReturnParkingRatesNormal
    {
        [FunctionName("ReturnParkingRatesNormal")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Asume its coming from dynamic 'Rates service'
            string jsonToReturn = "[{\"MaxHours\":1,\"Rate\":5.0},{\"MaxHours\":2,\"Rate\":10.0},{\"MaxHours\":3,\"Rate\":15.0},{\"MaxHours\":24,\"Rate\":20.0}]";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };

        }
    }
}
