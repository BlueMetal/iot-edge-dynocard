using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynoCardWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DynoCardWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class DynoCardController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "dyno1", "dyno2" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            DynoCardAnomalyEvent dynoCardAnomalyEvent = JsonConvert.DeserializeObject<DynoCardAnomalyEvent>(value);
        }

    }
}