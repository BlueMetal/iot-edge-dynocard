using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sql = System.Data.SqlClient;
using System.Text;
using DynoCardAlertModule.Model;

namespace DynoCardAlertModule.Data
{
    public static class DataHelper
    {
        public static string ConnectionString { get; set; }

        public static async Task<bool> PersistDynoCard(DynoCard card)
        {
            try
            {
                // //Store the data in SQL db
                using (Sql.SqlConnection conn = new Sql.SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var insertHeader = new StringBuilder("INSERT INTO db4cards.[ACTIVE].[CARD_HEADER] ([EV_ID],[CH_COLLECTED],[CH_SCALED_MAX_LOAD],")
                    .Append("[CH_SCALED_MIN_LOAD],[CH_STROKE_LENGTH],[CH_STROKE_PERIOD],[CH_CARD_TYPE],[CH_UPDATE_DATE],[CH_UPDATE_BY]) ")
                    .Append("OUTPUT INSERTED.CH_ID ")
                    .Append($"VALUES (null, '{card.Timestamp}', {card.MaxLoad}, {card.MinLoad}, {card.StrokeLength}, {card.StrokePeriod}, {(int)card.CardType}, '{DateTime.Now}', 'edgeModule');");
                    System.Console.WriteLine($"Header insert: {insertHeader}");

                    var insertDetail = "INSERT INTO db4cards.[ACTIVE].[CARD_DETAIL] ([CH_ID],[CD_POSITION],[CD_LOAD]) VALUES ({0}, {1}, {2});";

                    using (Sql.SqlCommand headerInsert = new Sql.SqlCommand(insertHeader.ToString(), conn))
                    {
                        var headerID = await headerInsert.ExecuteScalarAsync();

                        using (Sql.SqlCommand cmd = new Sql.SqlCommand())
                        {
                            cmd.Connection = conn;

                            foreach (var point in card.CardPoints)
                            {
                                string detailStatement = string.Format(insertDetail, headerID, point.Position, point.Load);
                                cmd.CommandText = detailStatement;
                                //System.Console.WriteLine($"Detail insert: {detailStatement}");
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error trying to insert dyno card data: {ex.Message}");
                System.Console.WriteLine(ex.StackTrace);
                await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        public static async Task<List<DynoCard>> GetPreviousCards(DynoCardAnomalyResult anomalyCard)
        {
            DateTime start = anomalyCard.DynoCardTimestamp.Subtract(TimeSpan.FromHours(1));
            DateTime end = anomalyCard.DynoCardTimestamp;
            string startDateString = start.ToString("s");
            string endDateString = end.ToString("s");

            var sql = new StringBuilder("SELECT h.CH_ID, ")
            .Append("CH_COLLECTED, ")
            .Append("CH_SCALED_MAX_LOAD, ")
            .Append("CH_SCALED_MIN_LOAD, ")
            .Append("CH_STROKE_LENGTH, ")
            .Append("CH_STROKE_PERIOD, ")
            .Append("CH_CARD_TYPE, ")
            .Append("CD_ID, ")
            .Append("CD_POSITION, ")
            .Append("CD_LOAD ")
            .Append("FROM[ACTIVE].[CARD_HEADER] h ")
            .Append("JOIN[ACTIVE].[CARD_DETAIL] d ON h.CH_ID = d.CH_ID ")
            .Append($"WHERE h.CH_COLLECTED >= '{startDateString}' ")
            .Append($"AND h.CH_COLLECTED <= '{endDateString}' ")
            .Append("ORDER BY h.CH_ID DESC");

            Dictionary<int, DynoCard> cardList = new Dictionary<int, DynoCard>();

            // //Store the data in SQL db
            using (Sql.SqlConnection conn = new Sql.SqlConnection(ConnectionString))
            {
                conn.Open();

                using (Sql.SqlCommand cardHistorySelect = new Sql.SqlCommand(sql.ToString(), conn))
                {
                    var results = await cardHistorySelect.ExecuteReaderAsync();

                    if (results.HasRows)
                    {
                        DynoCard card = null;
                        int headerID = 0;

                        while (await results.ReadAsync())
                        {
                            int currentID = results.GetInt32(0);

                            if (headerID != currentID)
                            {
                                headerID = currentID;

                                card = new DynoCard()
                                {
                                    Id = currentID,
                                    Timestamp = results.GetDateTime(1),
                                    MaxLoad = (int)results.GetFloat(2),
                                    MinLoad = (int)results.GetFloat(3),
                                    StrokeLength = (int)results.GetFloat(4),
                                    StrokePeriod = (int)results.GetFloat(5),
                                    CardType = results.GetString(6) == "0" ? DynoCardType.Pump : DynoCardType.Surface,
                                    CardPoints = new List<DynoCardPoint>()
                                };

                                card.CardPoints.Add(new DynoCardPoint()
                                {
                                    Position = (int)results.GetFloat(8),
                                    Load = (int)results.GetFloat(9)
                                });

                                cardList.Add(currentID, card);
                            }
                            else
                            {
                                cardList[currentID].CardPoints.Add(new DynoCardPoint()
                                {
                                    Position = (int)results.GetFloat(8),
                                    Load = (int)results.GetFloat(9)
                                });
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(cardList?.Values?.ToList());
        }
    }
}