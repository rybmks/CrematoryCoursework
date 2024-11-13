using Crematory.DatabaseServices;
using Crematory.Models;
using Npgsql;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DataAccess
{
    public static class DeleteDataService
    {
        public static async Task<bool> DeleteService(ServiceModel service)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteService);

            if (service == null)
                return false;

            command.Parameters.AddWithValue("@Id", service.Id);
            
            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> DeleteCrematory(CrematoryModel crematory)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteCrematory);

            if (crematory == null)
                return false;

            command.Parameters.AddWithValue("@Id", crematory.Id);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> DeleteSchedule(CrematoryScheduleModel schedule)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteSchedule);

            if (schedule == null)
                return false;

            command.Parameters.AddWithValue("@Id", schedule.Id);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
