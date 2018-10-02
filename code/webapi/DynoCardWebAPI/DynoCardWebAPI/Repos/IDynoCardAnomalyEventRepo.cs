using DynoCardWebAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Repos
{
    public interface IDynoCardAnomalyEventRepo
    {
        void Add(DynoCardAnomalyEvent dcae);
    }
}
