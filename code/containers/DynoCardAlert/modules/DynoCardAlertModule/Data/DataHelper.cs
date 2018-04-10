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

        public static async Task<int> PersistDynoCard(DynoCard card)
        {
            int cardID = -1;

            try
            {
                // //Store the data in SQL DB
                using (Sql.SqlConnection conn = new Sql.SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var insertDynoCard = new StringBuilder("INSERT INTO [ACTIVE].[DYNO_CARD] ([PU_ID], [DC_UPDATE_DATE], [DC_UPDATE_BY]) ")
                    .Append("OUTPUT INSERTED.DC_ID ")
                    .Append($"VALUES (1, '{DateTime.Now}', 'edgeModule') ");

                    var insertSurfaceCard = new StringBuilder("INSERT INTO [ACTIVE].[CARD_HEADER] ")
                    .Append("([DC_ID], [CH_EPOC_DATE], [CH_SCALED_MAX_LOAD], [CH_SCALED_MIN_LOAD], ")
                    .Append("[CH_NUMBER_OF_POINTS], [CH_STROKE_LENGTH], ")
                    .Append("[CH_STROKE_PERIOD], [CH_CARD_TYPE], [CH_UPDATE_DATE], [CH_UPDATE_BY]) ")                    
                    .Append("OUTPUT INSERTED.CH_ID ")
                    .Append("VALUES ({0}, ").Append($"CONVERT(int, DATEDIFF(ss, '01-01-1970 00:00:00', '{card.SurfaceCard.Timestamp}')), {card.SurfaceCard.ScaledMaxLoad}, {card.SurfaceCard.ScaledMinLoad}, ")
                    .Append($"{card.SurfaceCard.NumberOfPoints}, {card.SurfaceCard.StrokeLength}, {card.SurfaceCard.StrokePeriod}, 'S', '{DateTime.Now}', 'edgeModule');");

                    var insertPumpCard = new StringBuilder("INSERT INTO [ACTIVE].[CARD_HEADER] ")
                   .Append("([DC_ID], [CH_EPOC_DATE], [CH_SCALED_MAX_LOAD], [CH_SCALED_MIN_LOAD], ")
                   .Append("[CH_NUMBER_OF_POINTS], [CH_GROSS_STROKE], [CH_NET_STROKE], [CH_PUMP_FILLAGE], ")
                   .Append("[CH_FLUID_LOAD], [CH_CARD_TYPE], [CH_UPDATE_DATE], [CH_UPDATE_BY]) ")
                   .Append("OUTPUT INSERTED.CH_ID ")
                   .Append("VALUES ({0}, ").Append($"CONVERT(int, DATEDIFF(ss, '01-01-1970 00:00:00', '{card.PumpCard.Timestamp}')), {card.PumpCard.ScaledMaxLoad}, {card.PumpCard.ScaledMinLoad}, ")
                   .Append($"{card.PumpCard.NumberOfPoints}, {card.PumpCard.GrossStroke}, {card.PumpCard.NetStroke}, {card.PumpCard.PumpFillage}, {card.PumpCard.FluidLoad}, ")
                   .Append($"'P', '{DateTime.Now}', 'edgeModule'); ");

                    var insertDetail = "INSERT INTO [ACTIVE].[CARD_DETAIL] ([CH_ID],[CD_POSITION],[CD_LOAD]) VALUES ({0}, {1}, {2});";

                    using (Sql.SqlCommand dynoCardCommand = new Sql.SqlCommand())
                    {
                        //Insert the DynoCard record
                        dynoCardCommand.Connection = conn;
                        dynoCardCommand.CommandText = insertDynoCard.ToString();
                        var dynoCardID = await dynoCardCommand.ExecuteScalarAsync();
                        cardID = (int)dynoCardID;

                        //Insert the Surface card header record
                        dynoCardCommand.CommandText = string.Format(insertSurfaceCard.ToString(), dynoCardID);
                        var headerID = await dynoCardCommand.ExecuteScalarAsync();
                        
                        //Insert the Surface card detail records
                        foreach (var point in card.SurfaceCard.CardCoordinates)
                        {
                            string detailStatement = string.Format(insertDetail, headerID, point.Position, point.Load);
                            dynoCardCommand.CommandText = detailStatement;
                            await dynoCardCommand.ExecuteNonQueryAsync();
                        }

                        //Insert the Pump card header record
                        dynoCardCommand.CommandText = string.Format(insertPumpCard.ToString(), dynoCardID);
                        headerID = await dynoCardCommand.ExecuteScalarAsync();

                        //Insert the Pump card detail records
                        foreach (var point in card.PumpCard.CardCoordinates)
                        {
                            string detailStatement = string.Format(insertDetail, headerID, point.Position, point.Load);
                            dynoCardCommand.CommandText = detailStatement;
                            await dynoCardCommand.ExecuteNonQueryAsync();
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

            return await Task.FromResult(cardID);
        }

        public static async Task<List<DynoCard>> GetPreviousCards(DynoCardAnomalyResult anomalyCard)
        {
            DateTime start = anomalyCard.Timestamp.Subtract(TimeSpan.FromHours(1));
            DateTime end = anomalyCard.Timestamp;
            int startEpoch = (int)(start.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int endEpoch = (int)(end.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var sql = new StringBuilder("SELECT h.CH_CARD_TYPE, ")
            .Append("dc.DC_ID, ")
            .Append("h.CH_ID, ")
            .Append("h.CH_SCALED_MAX_LOAD, ")
            .Append("h.CH_SCALED_MIN_LOAD, ")
            .Append("h.CH_STROKE_LENGTH, ")
            .Append("h.CH_STROKE_PERIOD, ")
            .Append("h.CH_GROSS_STROKE, ")
            .Append("h.CH_NET_STROKE, ")
            .Append("h.CH_PUMP_FILLAGE, ")
            .Append("h.CH_FLUID_LOAD, ")
            .Append("d.CD_ID, ")
            .Append("d.CD_POSITION, ")
            .Append("d.CD_LOAD, ")
            .Append("h.CH_EPOC_DATE, ")
            .Append("dc.PU_ID ")
            .Append("FROM [ACTIVE].[DYNO_CARD] dc ")
            .Append("JOIN [ACTIVE].[CARD_HEADER] h ON dc.DC_ID = h.DC_ID ")
            .Append("JOIN [ACTIVE].[CARD_DETAIL] d ON h.CH_ID = d.CH_ID ")
            .Append($"WHERE h.CH_EPOC_DATE >= {startEpoch} ")
            .Append($"AND h.CH_EPOC_DATE <= {endEpoch} ")
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
                        PumpCard pumpCard = null;
                        SurfaceCard surfaceCard = null;
                        int previousCardID = 0;
                        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                        while (await results.ReadAsync())
                        {
                            string cardTypeValue = results.GetString(0);
                            int dynoCardID = results.GetInt32(1);
                           
                            CardType cardType = "P".Equals(cardTypeValue) ? CardType.Pump : CardType.Surface;
                            
                            if (previousCardID != dynoCardID)
                            {
                                previousCardID = dynoCardID;
                                pumpCard = null;
                                surfaceCard = null;
                            }

                            if (cardType == CardType.Surface)
                            {
                                if (surfaceCard == null)
                                {
                                    surfaceCard = new SurfaceCard()
                                    {
                                        Id = results.GetInt32(2),
                                        Timestamp = epoch.AddSeconds(results.GetInt32(14)),
                                        ScaledMaxLoad = (int)results.GetFloat(3),
                                        ScaledMinLoad = (int)results.GetFloat(4),
                                        StrokeLength = (int)results.GetFloat(5),
                                        StrokePeriod = (int)results.GetFloat(6),
                                        CardType = cardType,
                                        CardCoordinates = new List<CardCoordinate>()
                                    };

                                    surfaceCard.CardCoordinates.Add(new CardCoordinate()
                                    {
                                        Position = (int)results.GetFloat(12),
                                        Load = (int)results.GetFloat(13)
                                    });

                                    DynoCard dynoCard = null;
                                    if (cardList.ContainsKey(dynoCardID))
                                    {
                                        dynoCard = cardList[dynoCardID];
                                    }
                                    else
                                    {
                                        dynoCard = new DynoCard();
                                        dynoCard.Id = dynoCardID;
                                        dynoCard.Timestamp = surfaceCard.Timestamp;
                                        dynoCard.Pump = results.GetInt32(15);
                                    }

                                    dynoCard.SurfaceCard = surfaceCard;
                                    cardList[dynoCardID] = dynoCard;
                                }
                                else
                                {
                                    cardList[dynoCardID].SurfaceCard.CardCoordinates.Add(new CardCoordinate()
                                    {
                                        Position = (int)results.GetFloat(12),
                                        Load = (int)results.GetFloat(13)
                                    });
                                }
                            }
                            else if (cardType == CardType.Pump)
                            {
                                if (pumpCard == null)
                                {
                                    pumpCard = new PumpCard()
                                    {
                                        Id = results.GetInt32(2),
                                        Timestamp = epoch.AddSeconds(results.GetInt32(14)),
                                        ScaledMaxLoad = (int)results.GetFloat(3),
                                        ScaledMinLoad = (int)results.GetFloat(4),
                                        GrossStroke = (int)results.GetFloat(7),
                                        NetStroke = (int)results.GetFloat(8),
                                        PumpFillage = (int)results.GetFloat(9),
                                        FluidLoad = (int)results.GetFloat(10),
                                        CardType = cardType,
                                        CardCoordinates = new List<CardCoordinate>()
                                    };

                                    pumpCard.CardCoordinates.Add(new CardCoordinate()
                                    {
                                        Position = (int)results.GetFloat(12),
                                        Load = (int)results.GetFloat(13)
                                    });

                                    DynoCard dynoCard = null;
                                    if (cardList.ContainsKey(dynoCardID))
                                    {
                                        dynoCard = cardList[dynoCardID];
                                    }
                                    else
                                    {
                                        dynoCard = new DynoCard();
                                        dynoCard.Id = dynoCardID;
                                        dynoCard.Timestamp = pumpCard.Timestamp;
                                        dynoCard.Pump = results.GetInt32(15);
                                    }

                                    dynoCard.PumpCard = pumpCard;
                                    cardList[dynoCardID] = dynoCard;
                                }
                                else
                                {
                                    cardList[dynoCardID].PumpCard.CardCoordinates.Add(new CardCoordinate()
                                    {
                                        Position = (int)results.GetFloat(12),
                                        Load = (int)results.GetFloat(13)
                                    });
                                }
                            }
                        }
                    }
                }
            }

            var cards = cardList?.Values?.OrderBy(c => c.Timestamp).ToList();
            
            if (cards != null && cards.Count > 0)
            {
                cards.Last().TriggeredEvents = true;
            }
            
            return await Task.FromResult(cards);
        }
    }
}