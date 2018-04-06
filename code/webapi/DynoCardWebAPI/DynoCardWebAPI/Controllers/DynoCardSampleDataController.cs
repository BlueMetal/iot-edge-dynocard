using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynoCardWebAPI.Helpers;
using DynoCardWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DynoCardWebAPI.Controllers
{
    /// <summary>
    /// This is a controller to generate a JSON document that can be used by an API testing tool like Postman
    /// </summary>
    [Route("api/[controller]")]
    public class DynoCardSampleDataController : Controller
    {
        [HttpGet]
        public string Get()
        {
            DynoCardAnomalyEvent dcae = new DynoCardAnomalyEvent();

            // Add dyno card header Info
            dcae.PumpId = 1;
            dcae.Epoch = TimeHelper.GetEpoch();

            DynoCard dynoCard = null;

            //////
            // Add Dyno Card #1
            //////
            dynoCard = new DynoCard();
            dynoCard.TriggeredEvents = true;

            // Add Surface Card #1
            dynoCard.surfaceCard = new SurfaceCard();
            dynoCard.surfaceCard.Epoch = TimeHelper.GetEpoch();
            dynoCard.surfaceCard.NumPoints = 100;
            dynoCard.surfaceCard.ScaledMaxLoad = 101;
            dynoCard.surfaceCard.ScaledMinLoad = 102;
            dynoCard.surfaceCard.StrokeLength = 103;
            dynoCard.surfaceCard.StrokePeriod = 104;

            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(1.1F, 1.2F));
            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(1.3F, 1.4F));
            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(1.5F, 1.6F));

            // Add Pump Card #1
            dynoCard.pumpCard = new PumpCard();
            dynoCard.pumpCard.Epoch = TimeHelper.GetEpoch();
            dynoCard.pumpCard.FluidLoad = 200;
            dynoCard.pumpCard.GrossStroke = 201;
            dynoCard.pumpCard.NetStroke = 202;
            dynoCard.pumpCard.NumPoints = 203;
            dynoCard.pumpCard.PumpFillage = 204;
            dynoCard.pumpCard.ScaledMaxLoad = 205;
            dynoCard.pumpCard.ScaledMinLoad = 206;

            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(2.1F, 1.2F));
            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(2.3F, 2.4F));
            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(2.5F, 2.6F));

            dcae.dynoCards.Add(dynoCard);

            //////
            // Add Dyno Card #2
            //////
            dynoCard = new DynoCard();
            dynoCard.TriggeredEvents = false;

            // Add Surface Card #3
            dynoCard.surfaceCard = new SurfaceCard();
            dynoCard.surfaceCard.Epoch = TimeHelper.GetEpoch();
            dynoCard.surfaceCard.NumPoints = 300;
            dynoCard.surfaceCard.ScaledMaxLoad = 301;
            dynoCard.surfaceCard.ScaledMinLoad = 302;
            dynoCard.surfaceCard.StrokeLength = 303;
            dynoCard.surfaceCard.StrokePeriod = 304;

            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(3.1F, 3.2F));
            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(3.3F, 3.4F));
            dynoCard.surfaceCard.cardCoordinates.Add(new CardCoordinate(3.5F, 3.6F));

            // Add Pump Card #4
            dynoCard.pumpCard = new PumpCard();
            dynoCard.pumpCard.Epoch = TimeHelper.GetEpoch();
            dynoCard.pumpCard.FluidLoad = 400;
            dynoCard.pumpCard.GrossStroke = 401;
            dynoCard.pumpCard.NetStroke = 402;
            dynoCard.pumpCard.NumPoints = 403;
            dynoCard.pumpCard.PumpFillage = 404;
            dynoCard.pumpCard.ScaledMaxLoad = 405;
            dynoCard.pumpCard.ScaledMinLoad = 406;

            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(4.1F, 4.2F));
            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(4.3F, 4.4F));
            dynoCard.pumpCard.cardCoordinates.Add(new CardCoordinate(4.5F, 4.6F));

            dcae.dynoCards.Add(dynoCard);

            return JsonConvert.SerializeObject(dcae);
        }

    }
}