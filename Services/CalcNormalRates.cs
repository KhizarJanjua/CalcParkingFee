using CalcParkingFee.DTOs;
using CalcParkingFee.Models;
using CalcParkingFee.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalcParkingFee
{
    public static class CalcNormalRates
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("CalcNormalRates")]
        public static async Task<ParkingRateDTO> CalcNRAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function CalcNormalRates processed a request.");

            string name = string.Empty;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EntryExitDTO entryExitDateTime = JsonConvert.DeserializeObject<EntryExitDTO>(requestBody);

            ParkingRateDTO result = new ParkingRateDTO() { Name = CONSTANTS.RateType.STANDARD };

            string response = await client.GetStringAsync(CONSTANTS.ProjectURLs.ReturnParkingRatesNormal);
            List<NormalRate> normalRates = JsonConvert.DeserializeObject<List<NormalRate>>(response);

            double resultNormalMacro = 0.0;
            double resultNormalMicro = 0.0;
            bool isNormal = false;
            double duration = (entryExitDateTime.EndDT - entryExitDateTime.StartDT).TotalHours;
            NormalRate maxNormalRate = normalRates.OrderBy(nr => nr.MaxHours).LastOrDefault();

            if (duration >= maxNormalRate.MaxHours)
            {
                resultNormalMacro = Math.Floor(duration / maxNormalRate.MaxHours) * maxNormalRate.Rate;
                duration = duration % maxNormalRate.MaxHours;
            }
            if (duration > 0)
            {
                foreach (NormalRate normalRate in normalRates)
                {
                    if (!isNormal && duration <= normalRate.MaxHours)
                    {
                        isNormal = true;
                        resultNormalMicro = normalRate.Rate;
                    }
                }
            }

            result.Price = resultNormalMacro + resultNormalMicro;

            return result;

        }
    }
}
