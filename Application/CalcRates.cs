using CalcParkingFee.DTOs;
using CalcParkingFee.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcParkingFee
{
    public static class CalcRates
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("CalcRates")]
        public static async Task<ParkingRateDTO> CalcRatesAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function CalcRatesAsync processed a request.");

            string name = string.Empty;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string startdt = data?.startDT, enddt = data?.endDT;
            EntryExitDTO entryExitDT = new EntryExitDTO();

            try
            {
                entryExitDT.StartDT = Convert.ToDateTime(startdt);
                entryExitDT.EndDT = Convert.ToDateTime(enddt);
            }
            catch (Exception)
            {
                log.LogInformation("C# HTTP trigger function CalcRatesAsync processed with bad input dates.");
                return null; //TODO: return StatusCode instead
            }

            //Call NormalRates Service
            ParkingRateDTO normalRatesResults = new ParkingRateDTO();
            Task<HttpResponseMessage> postTask = client.PostAsJsonAsync(CONSTANTS.ProjectURLs.CalcNormalRates, entryExitDT);
            postTask.Wait();

            HttpResponseMessage result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                Task<ParkingRateDTO> readTask = result.Content.ReadAsAsync<ParkingRateDTO>();
                readTask.Wait();
                normalRatesResults = readTask.Result;
            }
            else            
                log.LogInformation("C# HTTP trigger function CalcRatesAsync failed to get rates from CalcNormalRates.");
            

            //Call SpecialRates Service
            ParkingRateDTO selectedRatesResults = new ParkingRateDTO();
            postTask = client.PostAsJsonAsync(CONSTANTS.ProjectURLs.CalcSpecialRates, entryExitDT);
            postTask.Wait();

            result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                Task<ParkingRateDTO> readTask = result.Content.ReadAsAsync<ParkingRateDTO>();
                readTask.Wait();
                selectedRatesResults = readTask.Result;
            }
            else
                log.LogInformation("C# HTTP trigger function CalcRatesAsync failed to get rates from CalcSpecialRates.");

            // select applicable
            if (normalRatesResults.Price > 0 && (selectedRatesResults.Price == 0 || selectedRatesResults.Price > normalRatesResults.Price))
            {
                selectedRatesResults.Name = normalRatesResults.Name;
                selectedRatesResults.Price = normalRatesResults.Price;
            }

            return selectedRatesResults;

        }
    }

}

