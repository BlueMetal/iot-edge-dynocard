using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using data_api.Models;
using data_api.Repos;
using data_api.Helpers;

namespace data_api.Controllers
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
        public async Task<List<AnomalyEvent>> Get()
        {
            return await dynoCardAnomalyEventRepo.Get();
        }
    }
}