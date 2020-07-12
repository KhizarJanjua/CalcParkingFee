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
    public static class CalcSpecialRates
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("CalcSpecialRates")]
        public static async Task<ParkingRateDTO> CalcSRAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function CalcSpecialRates processed a request.");

            string name = string.Empty;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EntryExitDTO entryExitDateTime = JsonConvert.DeserializeObject<EntryExitDTO>(requestBody);

            ParkingRateDTO result = new ParkingRateDTO();

            string response = await client.GetStringAsync(CONSTANTS.ProjectURLs.ReturnParkingRatesSpecial);
            List<SpecialRate> specialRates = JsonConvert.DeserializeObject<List<SpecialRate>>(response);


            foreach (SpecialRate specialRate in specialRates)
            {
                // Start of parking time
                bool isSpecial = (entryExitDateTime.StartDT.TimeOfDay >= specialRate.Entry.Start && entryExitDateTime.StartDT.TimeOfDay <= specialRate.Entry.End) ||
                                 (specialRate.MaxDays > 0 &&
                                  (specialRate.Entry.Start <= entryExitDateTime.StartDT.TimeOfDay &&
                                   entryExitDateTime.StartDT.TimeOfDay <= specialRate.Entry.End.Add(TimeSpan.FromDays(1))) ||
                                  (specialRate.Entry.Start.Subtract(TimeSpan.FromDays(1)) >= entryExitDateTime.StartDT.TimeOfDay &&
                                   entryExitDateTime.StartDT.TimeOfDay <= specialRate.Entry.End));

                if (!specialRate.Entry.Days.Any(d => string.Equals(d, entryExitDateTime.StartDT.DayOfWeek.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    isSpecial = false;
                }

                // End of parking time
                DateTime maxExitDay = entryExitDateTime.StartDT.AddDays(specialRate.MaxDays);
                DateTime maxExit = new DateTime(maxExitDay.Year, maxExitDay.Month, maxExitDay.Day, specialRate.Exit.End.Hours,
                    specialRate.Exit.End.Minutes, 0);
                if ((entryExitDateTime.EndDT.TimeOfDay < specialRate.Exit.Start || entryExitDateTime.EndDT.TimeOfDay > specialRate.Exit.End))
                {
                    isSpecial = false;
                }
                if (entryExitDateTime.EndDT > maxExit)
                {
                    isSpecial = false;
                }

                if (!specialRate.Exit.Days.Any(d => string.Equals(d, entryExitDateTime.EndDT.DayOfWeek.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    isSpecial = false;
                }

                if (isSpecial)
                {
                    if (result.Price == 0 || result.Price > specialRate.TotalPrice)
                    {
                        result.Name = specialRate.Name;
                        result.Price = specialRate.TotalPrice;
                    }
                }
            }

            return result;

        }
    }
}
