using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DynoCardWebAPI.Helpers;
using Microsoft.Extensions.Options;

namespace DynoCardWebAPI.Repos
{
    public class DynoCardHealthRepo : IDynoCardHealthRepo
    {
        private Settings Settings { get; set; }

        public DynoCardHealthRepo(IOptions<Settings> settings)
        {
            this.Settings = settings.Value;
        }

        public void CheckHealth()
        {
            using (SqlConnection sqlConn = new SqlConnection(this.Settings.ConnectionStrings.DynoCardDbConnString))
            {
                sqlConn.Open();
                SqlCommand sqlCmd = null;

                try
                {
                    sqlCmd = new SqlCommand("Select USER", sqlConn);
                    sqlCmd.CommandTimeout = 10;
                    sqlCmd.ExecuteScalar();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        sqlConn.Close();
                    }
                }
            }
        }
    }
}
