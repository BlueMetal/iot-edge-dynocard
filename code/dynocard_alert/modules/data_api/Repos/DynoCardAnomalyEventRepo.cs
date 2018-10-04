using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using data_api.Models;
using data_api.Helpers;

namespace data_api.Repos
{
    public class DynoCardAnomalyEventRepo : IDynoCardAnomalyEventRepo
    {
        private Settings settings { get; set; }
        public static string ConnectionString { get; set; }

        public DynoCardAnomalyEventRepo(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task<List<AnomalyEvent>> Get()
        {
            List<AnomalyEvent> eventList = new List<AnomalyEvent>();

            var sql = new StringBuilder()
            .Append("SELECT Pump_ID, ")
            .Append("Event_ID, ")
            .Append("CardHeader_ID, ")
            .Append("Card_Type, ")
            .Append("EPOC_DATE, ")
            .Append("Card_ID, ")
            .Append("Postion, ")
            .Append("Load ")
            .Append("FROM[ACTIVE].[DYNO_CARD_ANOMALY_VIEW] ");

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Open a connection to the database
                conn.Open();

                using (SqlCommand eventSelect = new SqlCommand(sql.ToString(), conn))
                {
                    var results = await eventSelect.ExecuteReaderAsync();

                    if (results.HasRows)
                    {
                        while (await results.ReadAsync())
                        {
                            eventList.Add(new AnomalyEvent()
                            {
                                CardHeader_ID = (string)results["CardHeader_ID"],
                                Card_ID = (string)results["Card_ID"],
                                Card_Type = (string)results["Card_Type"],
                                EPOC_DATE = (string)results["EPOC_DATE"],
                                Event_ID = (string)results["Event_ID"],
                                Pump_ID = (string)results["Pump_ID"],
                                Load = (string)results["Load"],
                                Position = (string)results["Postion"]
                            });
                        }
                    }
                }
            }

            return eventList;
        }
    }
}
