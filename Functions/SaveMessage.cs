using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IoTCloud_AF.models;
using System.Data.SqlClient;
using System;

namespace IoTCloud_AF
{
    public static class SaveMessage
    {

        [FunctionName("SaveDeviceMessages")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IoTHubConn")]
            EventData message,
            ILogger log)
        {

            var msg = JsonConvert.DeserializeObject<DeviceMessage>(Encoding.UTF8.GetString(message.Body.Array));

            msg.adress = message.SystemProperties["iothub-connection-device-id"].ToString(); 

            using(var conn = new SqlConnection(Environment.GetEnvironmentVariable("DBConn")))
            {
                conn.Open();

                using var cmd = new SqlCommand("", conn);

                var TableIds = new Id
                {
                    DeviceId = GetDeviceId(cmd, msg),
                    PriceId = GetPriceId(cmd, msg)
                };

                AddMeasurement(cmd, msg, TableIds);

            }


            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }

        private static int GetDeviceId(SqlCommand cmd, DeviceMessage msg)
        {
            const string deviceQuery = "IF NOT EXISTS " +
                "(SELECT id FROM device WHERE adress = @adress) " +
                "INSERT INTO device OUTPUT inserted.id VALUES(@adress, @alias) " +
                "ELSE SELECT id FROM device " +
                "WHERE adress = @adress";

            cmd.CommandText = deviceQuery;

            cmd.Parameters.AddWithValue("@adress", msg.adress);
            cmd.Parameters.AddWithValue("@alias", msg.alias ?? "");

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int GetPriceId(SqlCommand cmd, DeviceMessage msg)
        {
            const string priceQuery = "SELECT id FROM spot_price WHERE time_stamp_day = @day AND time_stamp_hour = @hour";

            cmd.Parameters.Clear();
            cmd.CommandText = priceQuery;

            cmd.Parameters.AddWithValue("@day", msg.timestamp.Date);
            cmd.Parameters.AddWithValue("@hour", msg.timestamp.Hour);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static void AddMeasurement(SqlCommand cmd, DeviceMessage msg, Id TableIds)
        {
            const string measurementQuery = "INSERT INTO measurements VALUES(@time_stamp, @price_id, @device_id, @measurement)";

            cmd.Parameters.Clear();
            cmd.CommandText = measurementQuery;

            var timestamp = new DateTimeOffset(msg.timestamp).ToUnixTimeSeconds();

            cmd.Parameters.AddWithValue("@time_stamp", timestamp);
            cmd.Parameters.AddWithValue("@price_id", TableIds.PriceId);
            cmd.Parameters.AddWithValue("@device_id", TableIds.DeviceId);
            cmd.Parameters.AddWithValue("@measurement", msg.measurement);

            

            var rows = cmd.ExecuteNonQuery();

        }

        private struct Id {
            public int PriceId { get; set; }
            public int DeviceId { get; set; }
        }
    }
}