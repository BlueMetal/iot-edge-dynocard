using System;
using System.Collections.Generic;
using DynoCardWebAPI.Helpers;
using DynoCardWebAPI.Models;
using DynoCardWebAPI.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DynoCardWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DynoCardHealthCheckController : Controller
    {
        public IDynoCardHealthRepo DynoCardHealthRepo;

        public DynoCardHealthCheckController(IOptions<Settings> settings, IDynoCardHealthRepo dynoCardHealthRepo)
        {
            this.DynoCardHealthRepo = dynoCardHealthRepo;
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                DynoCardHealthRepo.CheckHealth();
                return string.Format("Healthy on {0}", DateTime.Now.ToString("s"));
            }
            catch(Exception ex)
            {
                return string.Format("Unhealthy on {0}", DateTime.Now.ToString("s"));
            }
        }

    }
}