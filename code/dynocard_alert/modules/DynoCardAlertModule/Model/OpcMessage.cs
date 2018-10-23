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
        public static OpcSurfaceCardConfig SurfaceCardConfig { get; set; }
        public static OpcPumpCardConfig PumpCardConfig { get; set; }
        public DynoCard CurrentCard { get; set; }
        private OpcMessage() {}

        public OpcMessage(Message message)
        {
            System.Console.WriteLine("Creating opc message instance.");

            CurrentCard = new DynoCard()
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
                    System.Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("Empty opc message received.");
            }
        }

        private void PopulateReadingValues(List<OpcNodeReading> readingValues)
        {
            string tempSurfaceNumPoints = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.NumberOfPoints).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceNumPoints))
            {
                CurrentCard.SurfaceCard.NumberOfPoints = Int32.Parse(tempSurfaceNumPoints);
            }
            
            string tempSurfaceMaxLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.MaxLoad).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceMaxLoad))
            {
                CurrentCard.SurfaceCard.ScaledMaxLoad = Int32.Parse(tempSurfaceMaxLoad);
            }
            
            string tempSurfaceMinLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.MinLoad).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceMinLoad))
            {
                CurrentCard.SurfaceCard.ScaledMinLoad = Int32.Parse(tempSurfaceMinLoad);
            }

            string tempSurfaceStrokeLength = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.StrokeLength).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceStrokeLength))
            {
                CurrentCard.SurfaceCard.StrokeLength = Int32.Parse(tempSurfaceStrokeLength);
            }

            string tempSurfaceStrokePeriod = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.StrokePeriod).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceStrokePeriod))
            {
                CurrentCard.SurfaceCard.StrokePeriod = Int32.Parse(tempSurfaceStrokePeriod);
            }

            string tempSurfaceLoad = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Load).Select(r => r.Value.Value).FirstOrDefault();
            string tempSurfacePosition = readingValues.Where(r => r.DisplayName == SurfaceCardConfig.Position).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempSurfaceLoad) && !string.IsNullOrEmpty(tempSurfacePosition))
            {
                string[] loadValues = tempSurfaceLoad.Split(',');
                string[] positionValues = tempSurfacePosition.Split(',');

                for (int i = 0; i < loadValues.Length; i++)
                {
                    CurrentCard.SurfaceCard.CardCoordinates.Add(new CardCoordinate()
                    {
                        Load = Int32.Parse(loadValues[i]),
                        Position = (int)(Decimal.Parse(positionValues[i]) * 100)
                    });
                }
            }
            
            string tempPumpNumPoints = readingValues.Where(r => r.DisplayName == PumpCardConfig.NumberOfPoints).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpNumPoints))
            {
                CurrentCard.PumpCard.NumberOfPoints = Int32.Parse(tempPumpNumPoints);
            }
           
            string tempPumpMaxLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.MaxLoad).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpMaxLoad))
            {
                CurrentCard.PumpCard.ScaledMaxLoad = Int32.Parse(tempPumpMaxLoad);
            }

            string tempPumpMinLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.MinLoad).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpMinLoad))
            {
                CurrentCard.PumpCard.ScaledMinLoad = Int32.Parse(tempPumpMinLoad);
            }

            string tempPumpGrossStroke = readingValues.Where(r => r.DisplayName == PumpCardConfig.GrossStroke).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpGrossStroke))
            {
                CurrentCard.PumpCard.GrossStroke = Int32.Parse(tempPumpGrossStroke);
            }

            string tempPumpNetStroke = readingValues.Where(r => r.DisplayName == PumpCardConfig.NetStroke).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpNetStroke))
            {
                CurrentCard.PumpCard.NetStroke = Int32.Parse(tempPumpNetStroke);
            }

            string tempFluidLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.FluidLoad).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempFluidLoad))
            {
                CurrentCard.PumpCard.FluidLoad = Int32.Parse(tempFluidLoad);
            }

            string tempPumpFillage = readingValues.Where(r => r.DisplayName == PumpCardConfig.PumpFillage).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpFillage))
            {
                CurrentCard.PumpCard.PumpFillage = Int32.Parse(tempPumpFillage);
            }

            string tempPumpLoad = readingValues.Where(r => r.DisplayName == PumpCardConfig.Load).Select(r => r.Value.Value).FirstOrDefault();
            string tempPumpPosition = readingValues.Where(r => r.DisplayName == PumpCardConfig.Position).Select(r => r.Value.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempPumpLoad) && !string.IsNullOrEmpty(tempPumpPosition))
            {
                string[] loadValues = tempPumpLoad.Split(',');
                string[] positionValues = tempPumpPosition.Split(',');

                int coordinateLength = Math.Min(loadValues.Length, positionValues.Length);

                for (int i = 0; i < coordinateLength; i++)
                {
                    try
                    {
                        CurrentCard.PumpCard.CardCoordinates.Add(new CardCoordinate()
                        {
                            Load = Int32.Parse(loadValues[i]),
                            Position = (int)(Decimal.Parse(positionValues[i]) * 100)
                        });
                    }
                    catch
                    {
                        System.Console.WriteLine($"Error parsing load/position: {loadValues[i]}, {positionValues[i]}");
                    }
                }
            }
        }
    }
}