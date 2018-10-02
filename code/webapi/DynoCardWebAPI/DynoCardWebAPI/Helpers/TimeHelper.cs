using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Helpers
{
    public class TimeHelper
    {
        private static DateTime startDateTime = new DateTime(1970, 1, 1);

        public static double ConvertToEpoch(string timestamp)
        {
            DateTime timeStampResult;

            // convert the date / time stamp into a Date Time
            DateTime.TryParse(timestamp, out timeStampResult);

            // check if the conversion was successful
            if (timeStampResult == null)
            {
                // not successful, return a -1
                return -1;
            }

            // Return the total number of seconds since the epoch date of 01/01/1970
            return timeStampResult.Subtract(startDateTime).TotalSeconds;
        }

        public static int GetEpoch()
        {
            return (int)(DateTime.UtcNow.Subtract(startDateTime).TotalSeconds);
        }
    }
}
