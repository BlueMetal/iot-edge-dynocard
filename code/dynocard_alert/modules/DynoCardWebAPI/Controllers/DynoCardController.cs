using System.Collections.Generic;
using System.Threading.Tasks;
using DynoCardWebAPI.Helpers;
using DynoCardWebAPI.Messaging;
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
        private IOptions<Settings> settings;

        public DynoCardController(IOptions<Settings> settings, IDynoCardAnomalyEventRepo dynoCardAnomalyEventRepo)
        {
            this.dynoCardAnomalyEventRepo = dynoCardAnomalyEventRepo;
            this.settings = settings;
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

        [HttpGet]
        public List<AnomalyEvent> Get()
        {
            return dynoCardAnomalyEventRepo.Get();
        }

        [HttpGet]
        [Route("anomaly/{state}")]
        public async Task Get(string state)
        {
            await BrokeredMessenger.Send(state, settings.Value.ConnectionStrings.DeviceConnectionString);
        }
    }
}