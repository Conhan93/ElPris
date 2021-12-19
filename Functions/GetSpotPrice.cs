using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using IoTCloud_AF.models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IoTCloud_AF
{
    public static class GetSpotPrice
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("GetSpotPrice")]
        public static void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        { 
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // fetch spot prices from API
            var result = GetApiResponse().GetAwaiter().GetResult();

            var prices = JsonConvert.DeserializeObject<List<SpotPrice>>(result);
            
            // add spot prices to database table spot_prices
            AddPricesToDB(prices);

            log.LogInformation("Spot prices added to DB");
        }

        private static async Task<string> GetApiResponse()
        {
            var current_date = DateTime.Now.ToString("yyyy-MM-dd");

            string base_url = Environment.GetEnvironmentVariable("API_URL");
            var url = String.Format($"{base_url}/{current_date}/{current_date}/SN1");


            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static void AddPricesToDB(List<SpotPrice> prices)
        {
    
            // exit if list empty
            if (prices.Count == 0) return;

            using (var conn = new SqlConnection(Environment.GetEnvironmentVariable("DBConn")))
            {
                conn.Open();

                const string query = "insert into spot_price values(@time_stamp, @time_stamp_day, @time_stamp_hour, @price, @price_area, @unit)";

                foreach (var price in prices)
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@time_stamp", price.TimeStamp);
                        cmd.Parameters.AddWithValue("@time_stamp_day", price.TimeStamp.Date.ToShortDateString());
                        cmd.Parameters.AddWithValue("@time_stamp_hour", price.TimeStamp.Hour);
                        cmd.Parameters.AddWithValue("@price", price.Value);
                        cmd.Parameters.AddWithValue("@price_area", "SN1");
                        cmd.Parameters.AddWithValue("@unit", price.Unit);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }
    }
}
