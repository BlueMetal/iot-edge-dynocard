using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynoCardAlertModule.Config;
using DynoCardAlertModule.Data;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class OpcMessage
    {
        public static OpcCardHeaderConfig CardHeaderConfig { get; set; }
        public static OpcSurfaceCardConfig SurfaceCardConfig { get; set; }
        public static OpcPumpCardConfig PumpCardConfig { get; set; }
        public static DynoCard CachedDynoCard { get; set; }
        public DynoCard CurrentCard {get; set; }
        private CardType _currentCardType = CardType.Surface;

        private OpcMessage() {}

        public OpcMessage(Message message)
        {
            System.Console.WriteLine("Creating opc message instance.");

            if (CachedDynoCard == null)
            {
                //Initialize the cached instance with default values
                CachedDynoCard = new DynoCard()
                {
                    Id = -1,
                    Timestamp = DateTime.Now,
                    TriggeredEvents = false,
                    PumpCard = new PumpCard()
                    {
                        Id = -1,
                        CardType = CardType.Pump,
                        FluidLoad = 9500,
                        GrossStroke = 150,
                        NetStroke = 1200,
                        NumberOfPoints = 200,
                        PumpFillage = 77,
                        ScaledMaxLoad = 19500,
                        ScaledMinLoad = 7500,
                        Timestamp = DateTime.Now,
                        CardCoordinates = new List<CardCoordinate>()
                    },
                    SurfaceCard = new SurfaceCard()
                    {
                        Id = -1,
                        CardType = CardType.Surface,
                        StrokePeriod = 150,
                        StrokeLength = 1200,
                        NumberOfPoints = 200,
                        ScaledMaxLoad = 19500,
                        ScaledMinLoad = 7500,
                        Timestamp = DateTime.Now,
                        CardCoordinates = new List<CardCoordinate>()
                    }
                };
            }

            if (CurrentCard == null)
            {
                CurrentCard = new DynoCard();
            }

            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);

            if (!string.IsNullOrEmpty(messageString))
            {
                try
                {
                    var readingValues = JsonConvert.DeserializeObject<List<OpcNodeReading>>(messageString);
                    PopulateReadingValues(readingValues);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"An error occurred trying to populate/parse the values: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Empty opc message received.");
            }
        }

        private async void PopulateReadingValues(List<OpcNodeReading> readingValues)
        {
            // Dyno card header
            string cardID = readingValues.Where(r => r.DisplayName == CardHeaderConfig.CardID).Select(r => r.Value.Value).FirstOrDefault();
            
            //The card ID value is not in the list of values. So, assume only the load and position values have changed
            if (string.IsNullOrEmpty(cardID))
            {
                DynoCardHeader cardHeader = await (new DataHelper()).GetMaxCardID();
                CardType cardType = "S".Equals(cardHeader.CardType) ? CardType.Surface : CardType.Pump;

                PopulateLoadPositionReadingValues(cardType, readingValues);
            }
            else
            {
                CurrentCard.Id = Int32.Parse(cardID);
                CachedDynoCard.Id = Int32.Parse(cardID);

                PopulateAllReadingValues(readingValues);
            }
        }

        private void PopulateLoadPositionReadingValues(CardType cardType, List<OpcNodeReading> readingValues)
        {
            string tempLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Load).Select(r => r.Value.Value).FirstOrDefault();
            string tempPosition = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Position).Select(r => r.Value.Value).FirstOrDefault();
            
            if (!string.IsNullOrEmpty(tempLoad) && !string.IsNullOrEmpty(tempPosition))
            {
                int load = Int32.Parse(tempLoad);
                int position = Int32.Parse(tempPosition);

                if (cardType == CardType.Surface)
                {
                    CurrentCard.SurfaceCard.CardCoordinates.Add(new CardCoordinate()
                    {
                        Load = load,
                        Position = position
                    });

                    CachedDynoCard.SurfaceCard.CardCoordinates = CurrentCard.SurfaceCard.CardCoordinates;
                }
                else
                {
                    CurrentCard.PumpCard.CardCoordinates.Add(new CardCoordinate()
                    {
                        Load = load,
                        Position = position
                    });

                    CachedDynoCard.PumpCard.CardCoordinates = CurrentCard.PumpCard.CardCoordinates;
                }
            }
            else
            {
                if (cardType == CardType.Surface)
                {
                    CurrentCard.SurfaceCard.CardCoordinates = CachedDynoCard.SurfaceCard.CardCoordinates;
                }
                else
                {
                    CurrentCard.PumpCard.CardCoordinates = CachedDynoCard.PumpCard.CardCoordinates;
                }
            }
        }

        private void PopulateAllReadingValues(List<OpcNodeReading> readingValues)
        {
            CardType cardType = CardType.Surface;
            string tempCardType = readingValues.Where(r => r.DisplayName == CardHeaderConfig.CardType).Select(r => r.Value.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(tempCardType))
            {
                cardType = "S".Equals(tempCardType) ? CardType.Surface : CardType.Pump;
            }

            // Surface Card

            if (cardType == CardType.Surface)
            {
                string tempSurfaceNumPoints = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.NumberOfPoints).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceNumPoints))
                {
                    CurrentCard.SurfaceCard.NumberOfPoints = Int32.Parse(tempSurfaceNumPoints);
                    CachedDynoCard.SurfaceCard.NumberOfPoints = Int32.Parse(tempSurfaceNumPoints);
                }
                else
                {
                    CurrentCard.SurfaceCard.NumberOfPoints = CachedDynoCard.SurfaceCard.NumberOfPoints;
                }

                string tempSurfaceMaxLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.MaxLoad).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceMaxLoad))
                {
                    CurrentCard.SurfaceCard.ScaledMaxLoad = Int32.Parse(tempSurfaceMaxLoad);
                    CachedDynoCard.SurfaceCard.ScaledMaxLoad = Int32.Parse(tempSurfaceMaxLoad);
                }
                else
                {
                    CurrentCard.SurfaceCard.ScaledMaxLoad = CachedDynoCard.SurfaceCard.ScaledMaxLoad;
                }

                string tempSurfaceMinLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.MinLoad).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceMinLoad))
                {
                    CurrentCard.SurfaceCard.ScaledMinLoad = Int32.Parse(tempSurfaceMinLoad);
                    CachedDynoCard.SurfaceCard.ScaledMinLoad = Int32.Parse(tempSurfaceMinLoad);
                }
                else
                {
                    CurrentCard.SurfaceCard.ScaledMinLoad = CachedDynoCard.SurfaceCard.ScaledMinLoad;
                }

                string tempSurfaceStrokeLength = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.StrokeLength).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceStrokeLength))
                {
                    CurrentCard.SurfaceCard.StrokeLength = Int32.Parse(tempSurfaceStrokeLength);
                    CachedDynoCard.SurfaceCard.StrokeLength = Int32.Parse(tempSurfaceStrokeLength);
                }
                else
                {
                    CurrentCard.SurfaceCard.StrokeLength = CachedDynoCard.SurfaceCard.StrokeLength;
                }

                string tempSurfaceStrokePeriod = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.StrokePeriod).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceStrokePeriod))
                {
                    CurrentCard.SurfaceCard.StrokePeriod = Int32.Parse(tempSurfaceStrokePeriod);
                    CachedDynoCard.SurfaceCard.StrokePeriod = Int32.Parse(tempSurfaceStrokePeriod);
                }
                else
                {
                    CurrentCard.SurfaceCard.StrokePeriod = CachedDynoCard.SurfaceCard.StrokePeriod;
                }

                string tempSurfaceLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Load).Select(r => r.Value.Value).FirstOrDefault();
                string tempSurfacePosition = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Position).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempSurfaceLoad) && !string.IsNullOrEmpty(tempSurfacePosition))
                {
                    int load = Int32.Parse(tempSurfaceLoad);
                    int position = Int32.Parse(tempSurfacePosition);

                    CurrentCard.SurfaceCard.CardCoordinates.Add(new CardCoordinate()
                    {
                        Load = load,
                        Position = position
                    });

                    CachedDynoCard.SurfaceCard.CardCoordinates = CurrentCard.SurfaceCard.CardCoordinates;
                }
                else
                {
                    CurrentCard.SurfaceCard.CardCoordinates = CachedDynoCard.SurfaceCard.CardCoordinates;
                }
            }

            // Pump Card
            else if (cardType == CardType.Pump)
            {
                string tempPumpNumPoints = readingValues.Where(r => r.DisplayName == PumpCardConfig.NumberOfPoints).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpNumPoints))
                {
                    CurrentCard.PumpCard.NumberOfPoints = Int32.Parse(tempPumpNumPoints);
                    CachedDynoCard.PumpCard.NumberOfPoints = Int32.Parse(tempPumpNumPoints);
                }
                else
                {
                    CurrentCard.PumpCard.NumberOfPoints = CachedDynoCard.PumpCard.NumberOfPoints;
                }

                string tempPumpMaxLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.MaxLoad).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpMaxLoad))
                {
                    CurrentCard.PumpCard.ScaledMaxLoad = Int32.Parse(tempPumpMaxLoad);
                    CachedDynoCard.PumpCard.ScaledMaxLoad = Int32.Parse(tempPumpMaxLoad);
                }
                else
                {
                    CurrentCard.PumpCard.ScaledMaxLoad = CachedDynoCard.PumpCard.ScaledMaxLoad;
                }

                string tempPumpMinLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.MinLoad).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpMinLoad))
                {
                    CurrentCard.PumpCard.ScaledMinLoad = Int32.Parse(tempPumpMinLoad);
                    CachedDynoCard.PumpCard.ScaledMinLoad = Int32.Parse(tempPumpMinLoad);
                }
                else
                {
                    CurrentCard.PumpCard.ScaledMinLoad = CachedDynoCard.PumpCard.ScaledMinLoad;
                }

                string tempPumpGrossStroke = readingValues.Where(r => r.DisplayName == PumpCardConfig.GrossStroke).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpGrossStroke))
                {
                    CurrentCard.PumpCard.GrossStroke = Int32.Parse(tempPumpGrossStroke);
                    CachedDynoCard.PumpCard.GrossStroke = Int32.Parse(tempPumpGrossStroke);
                }
                else
                {
                    CurrentCard.PumpCard.GrossStroke = CachedDynoCard.PumpCard.GrossStroke;
                }

                string tempPumpNetStroke = readingValues.Where(r => r.DisplayName == PumpCardConfig.NetStroke).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpNetStroke))
                {
                    CurrentCard.PumpCard.NetStroke = Int32.Parse(tempPumpNetStroke);
                    CachedDynoCard.PumpCard.NetStroke = Int32.Parse(tempPumpNetStroke);
                }
                else
                {
                    CurrentCard.PumpCard.NetStroke = CachedDynoCard.PumpCard.NetStroke;
                }

                string tempFluidLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.FluidLoad).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempFluidLoad))
                {
                    CurrentCard.PumpCard.FluidLoad = Int32.Parse(tempFluidLoad);
                    CachedDynoCard.PumpCard.FluidLoad = Int32.Parse(tempFluidLoad);
                }
                else
                {
                    CurrentCard.PumpCard.FluidLoad = CachedDynoCard.PumpCard.FluidLoad;
                }

                string tempPumpFillage = readingValues.Where(r => r.DisplayName == PumpCardConfig.PumpFillage).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpFillage))
                {
                    CurrentCard.PumpCard.PumpFillage = Int32.Parse(tempPumpFillage);
                    CachedDynoCard.PumpCard.PumpFillage = Int32.Parse(tempPumpFillage);
                }
                else
                {
                    CurrentCard.PumpCard.PumpFillage = CachedDynoCard.PumpCard.PumpFillage;
                }

                string tempPumpLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.Load).Select(r => r.Value.Value).FirstOrDefault();
                string tempPumpPosition = readingValues.Where(r => r.DisplayName == PumpCardConfig.Position).Select(r => r.Value.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(tempPumpLoad) && !string.IsNullOrEmpty(tempPumpPosition))
                {
                    int load = Int32.Parse(tempPumpLoad);
                    int position = Int32.Parse(tempPumpPosition);

                    CurrentCard.PumpCard.CardCoordinates.Add(new CardCoordinate()
                    {
                        Load = load,
                        Position = position
                    });

                    CachedDynoCard.PumpCard.CardCoordinates = CurrentCard.PumpCard.CardCoordinates;
                }
                else
                {
                    CurrentCard.PumpCard.CardCoordinates = CachedDynoCard.PumpCard.CardCoordinates;
                }
            }
        }
    }
}