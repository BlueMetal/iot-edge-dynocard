using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data_api.Helpers
{
    public class Settings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Settings()
        {
        }

    }

    public class ConnectionStrings
    {
        public string DynoCardDbConnString { get; set; }
        public string DeviceConnectionString { get; set; }

        public ConnectionStrings()
        { }
    }
}
