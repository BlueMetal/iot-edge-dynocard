using data_api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data_api.Repos
{
    public interface IDynoCardAnomalyEventRepo
    {        
        Task<List<AnomalyEvent>> Get();
    }
}
