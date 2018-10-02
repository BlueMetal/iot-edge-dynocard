using DynoCardWebAPI.Models;
using System;
using System.Data.SqlClient;
using DynoCardWebAPI.Helpers;
using Microsoft.Extensions.Options;

namespace DynoCardWebAPI.Repos
{
    public class DynoCardAnomalyEventRepo : IDynoCardAnomalyEventRepo
    {
        private Settings settings { get; set; }
        public DynoCardAnomalyEventRepo(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }

        public void Add(DynoCardAnomalyEvent dcae)
        {

            // validate that the main objects are present
            if (dcae == null || dcae.DynoCard == null || dcae.DynoCard.surfaceCard == null || dcae.DynoCard.pumpCard == null)
                return;

            // Create a new SQL Connection
            using (SqlConnection sqlConn = new SqlConnection(this.settings.ConnectionStrings.DynoCardDbConnString))
            {
                // Open a connection to the database
                sqlConn.Open();
                // Begin a SQL transaction
                var sqlTrans = sqlConn.BeginTransaction();
                string cmdText = string.Empty;

                try
                {
                    // Insert into the Event Table
                    int eventId = InsertEvent(sqlConn, sqlTrans, dcae);

                    // Insert the dyno card into the database
                    int dynoCardId = InsertDynoCard(sqlConn, sqlTrans, dcae);

                    // Associate the event with the dyno card
                    int eventDetailId = InsertEventDetail(sqlConn, sqlTrans, eventId, dynoCardId, dcae.DynoCard.TriggeredEvents);

                    /////
                    // Insert surface and pump card for the dyno card
                    /////

                    // Insert Surface Card
                    int surfaceCardId = InsertSurfaceCardHeader(sqlConn, sqlTrans, dynoCardId, dcae.DynoCard);

                    // Loop through all of the coordinates and save them to the database
                    foreach (var cardCoordinate in dcae.DynoCard.surfaceCard.cardCoordinates)
                    {
                        InsertCardCoordinates(sqlConn, sqlTrans, surfaceCardId, cardCoordinate);
                    }

                    // Insert Pump Card
                    int pumpCardId = InsertPumpCardHeader(sqlConn, sqlTrans, dynoCardId, dcae.DynoCard);

                    // Loop through all of the coordinates and save them to the database
                    foreach (var cardCoordinate in dcae.DynoCard.pumpCard.cardCoordinates)
                    {
                        InsertCardCoordinates(sqlConn, sqlTrans, pumpCardId, cardCoordinate);
                    }

                    // Commit the transaction
                    sqlTrans.Commit();
                }
                catch (Exception)
                {
                    sqlTrans.Rollback();
                    throw;
                }
                finally
                {
                    // Close the database connection
                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        sqlConn.Close();
                    }
                }
            }
        }

        private int InsertEvent(SqlConnection sqlConn, SqlTransaction sqlTrans, DynoCardAnomalyEvent dcae)
        {
            string cmdText = string.Empty;
            SqlCommand sqlCmd = null;

            cmdText = "DECLARE @EventId AS INT " +
                "SELECT @EventId = EV_ID FROM ACTIVE.EVENT WITH(UPDLOCK, HOLDLOCK) WHERE EV_ANOMALY_ID = @AnomalyId; " +
                "IF ISNULL(@EventId, 0) = 0 " +
                " BEGIN " +
                    "INSERT INTO ACTIVE.EVENT( " +
                    "PU_ID, " +
                    "EV_EPOC_DATE, " +
                    "EV_ANOMALY_ID, " +
                    "EV_UPDATE_DATE, " +
                    "EV_UPDATE_BY) " +
                    "OUTPUT INSERTED.EV_ID " +
                    "VALUES( " +
                    "@PumpId, " +
                    "@EpochDate, " +
                    "@AnomalyId, " +
                    "GETUTCDATE(), " +
                    "'SYSTEM') " +
                " END " +
                "ELSE " +
                " BEGIN " +
                "   SELECT @EventId " +
                " END";

            sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
            sqlCmd.Parameters.AddWithValue("@PumpId", dcae.PumpId);
            sqlCmd.Parameters.AddWithValue("@EpochDate", TimeHelper.ConvertToEpoch(dcae.Timestamp));
            sqlCmd.Parameters.AddWithValue("@AnomalyId", dcae.AnomalyId.ToString());
            var eventId = sqlCmd.ExecuteScalar();
            return (int)eventId;
        }

        private int InsertDynoCard(SqlConnection sqlConn, SqlTransaction sqlTrans, DynoCardAnomalyEvent dcae)
        {
            string cmdText = string.Empty;
            SqlCommand sqlCmd = null;

            // Insert into the Dyno Card Table
            cmdText = "INSERT INTO ACTIVE.DYNO_CARD " +
                        "(PU_ID, " +
                        "DC_UPDATE_DATE, " +
                        "DC_UPDATE_BY) " +
                        "OUTPUT INSERTED.DC_ID " +
                        "VALUES(" +
                        "@PumpId, " +
                        "GETUTCDATE(), " +
                        "'SYSTEM')";

            sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
            sqlCmd.Parameters.AddWithValue("@PumpId", dcae.PumpId);
            return (int)sqlCmd.ExecuteScalar();
        }

        private int InsertEventDetail(SqlConnection sqlConn, SqlTransaction sqlTrans, int eventId, int dynoCardId, bool triggeredEvents)
        {
            string cmdText = string.Empty;
            SqlCommand sqlCmd = null;

            // Insert into the Dyno Card Table
            cmdText = "INSERT INTO ACTIVE.EVENT_DETAIL " +
                        "(EV_ID, " +
                        "DC_ID, " +
                        "ED_TRIGGERED_EVENTS, " +
                        "ED_UPDATE_DATE, " +
                        "ED_UPDATE_BY) " +
                        "OUTPUT INSERTED.ED_ID " +
                        "VALUES(" +
                        "@EventId, " +
                        "@DynoCardId, " +
                        "@TriggeredEvents, " +
                        "GETUTCDATE(), " +
                        "'SYSTEM')";

            sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
            sqlCmd.Parameters.AddWithValue("@EventId", eventId);
            sqlCmd.Parameters.AddWithValue("@DynoCardId", dynoCardId);
            sqlCmd.Parameters.AddWithValue("@TriggeredEvents", triggeredEvents);
            return (int)sqlCmd.ExecuteScalar();
        }

        private int InsertSurfaceCardHeader(SqlConnection sqlConn, SqlTransaction sqlTrans, int dynoCardId, DynoCard dynoCard)
        {
            string cmdText = string.Empty;
            SqlCommand sqlCmd = null;

            // Insert surface card header for the dyno card
            cmdText = "INSERT INTO ACTIVE.CARD_HEADER " +
                        "(DC_ID, " +
                        "CH_CARD_TYPE, " +
                        "CH_EPOC_DATE, " +
                        "CH_NUMBER_OF_POINTS, " +
                        "CH_UPDATE_DATE, " +
                        "CH_UPDATE_BY, " +
                        "CH_STROKE_LENGTH, " +
                        "CH_STROKE_PERIOD) " +
                        "OUTPUT INSERTED.CH_ID " +
                        "VALUES" +
                        "(@DcId, " +
                        "'S', " +
                        "@EpocDate, " +
                        "@NumPoints, " +
                        "GETUTCDATE(), " +
                        "'SYSTEM', " +
                        "@ScaledMaxLoad, " +
                        "@ScaledMinLoad)";

            sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
            sqlCmd.Parameters.AddWithValue("@DcId", dynoCardId);
            sqlCmd.Parameters.AddWithValue("@EpocDate", TimeHelper.ConvertToEpoch(dynoCard.surfaceCard.Timestamp));
            sqlCmd.Parameters.AddWithValue("@NumPoints", dynoCard.surfaceCard.NumPoints);
            sqlCmd.Parameters.AddWithValue("@ScaledMaxLoad", dynoCard.surfaceCard.ScaledMaxLoad);
            sqlCmd.Parameters.AddWithValue("@ScaledMinLoad", dynoCard.surfaceCard.ScaledMinLoad);
            return (int)sqlCmd.ExecuteScalar();

        }

        private int InsertPumpCardHeader(SqlConnection sqlConn, SqlTransaction sqlTrans, int dynoCardId, DynoCard dynoCard)
        {
            try
            {
                SqlCommand sqlCmd = null;

                string cmdText = string.Empty;

                // Insert surface card header for the dyno card
                cmdText = "INSERT INTO ACTIVE.CARD_HEADER " +
                            "(DC_ID, " +
                            "CH_CARD_TYPE, " +
                            "CH_EPOC_DATE, " +
                            "CH_NUMBER_OF_POINTS, " +
                            "CH_UPDATE_DATE, " +
                            "CH_UPDATE_BY, " +
                            "CH_GROSS_STROKE, " +
                            "CH_NET_STROKE, " +
                            "CH_PUMP_FILLAGE, " +
                            "CH_FLUID_LOAD) " +
                            "OUTPUT INSERTED.CH_ID " +
                            "VALUES" +
                            "(@DcId, " +
                            "'P', " +
                            "@EpocDate, " +
                            "@NumPoints, " +
                            "GETUTCDATE(), " +
                            "'SYSTEM', " +
                            "@GrossStroke, " +
                            "@NetStroke, " +
                            "@PumpFillage, " +
                            "@FluidLoad)";

                sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
                sqlCmd.Parameters.AddWithValue("@DcId", dynoCardId);
                sqlCmd.Parameters.AddWithValue("@EpocDate", TimeHelper.ConvertToEpoch(dynoCard.pumpCard.Timestamp));
                sqlCmd.Parameters.AddWithValue("@NumPoints", dynoCard.pumpCard.NumPoints);
                sqlCmd.Parameters.AddWithValue("@GrossStroke", dynoCard.pumpCard.GrossStroke);
                sqlCmd.Parameters.AddWithValue("@NetStroke", dynoCard.pumpCard.NetStroke);
                sqlCmd.Parameters.AddWithValue("@PumpFillage", dynoCard.pumpCard.PumpFillage);
                sqlCmd.Parameters.AddWithValue("@FluidLoad", dynoCard.pumpCard.FluidLoad);
                return (int)sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertCardCoordinates(SqlConnection sqlConn, SqlTransaction sqlTrans, int cardHeaderId, CardCoordinate cardCoordinate)
        {
            string cmdText = string.Empty;
            SqlCommand sqlCmd = null;

            // Insert surface card details for the dyno card
            cmdText = "INSERT INTO ACTIVE.CARD_DETAIL " +
                        "(CH_ID, " +
                        "CD_POSITION, " +
                        "CD_LOAD, " +
                        "CD_UPDATE_DATE, " +
                        "CD_UPDATE_BY) " +
                        "VALUES" +
                        "(@ChId, " +
                        "@Position, " +
                        "@Load, " +
                        "GETUTCDATE(), " +
                        "'SYSTEM')";

            sqlCmd = new SqlCommand(cmdText, sqlConn, sqlTrans);
            sqlCmd.Parameters.AddWithValue("@ChId", cardHeaderId);
            sqlCmd.Parameters.AddWithValue("@Position", cardCoordinate.Position);
            sqlCmd.Parameters.AddWithValue("@Load", cardCoordinate.Load);
            sqlCmd.ExecuteNonQuery();
        }
    }
}
