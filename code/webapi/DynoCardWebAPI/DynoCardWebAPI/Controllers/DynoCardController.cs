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
    public class DynoCardController : Controller
    {
        private IDynoCardAnomalyEventRepo dynoCardAnomalyEventRepo;

        public DynoCardController(IOptions<Settings> settings, IDynoCardAnomalyEventRepo dynoCardAnomalyEventRepo)
        {
            this.dynoCardAnomalyEventRepo = dynoCardAnomalyEventRepo;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "dyno card 1", "dyno card 2" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]DynoCardAnomalyEvent dcae)
        {
            if (dcae != null)
            {
                dynoCardAnomalyEventRepo.Add(dcae);
            }
        }

    }
}