using Crematory.DatabaseServices;
using Crematory.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DataAccess
{
    public static class InsertDataService
    {
        public static async Task<bool> InsertService(ServiceModel service)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertService);

            if (service == null || service.Name == null)
                return false;

            command.Parameters.AddWithValue("@Name", service.Name);
            command.Parameters.AddWithValue("@Price", service.Price);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> InsertCrematory(CrematoryModel crematory)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertCrematory);

            if (crematory == null || crematory.Name == null || 
               crematory.Address == null || crematory.ContactInfo == null)
               
                return false;

            command.Parameters.AddWithValue("@Name", crematory.Name);
            command.Parameters.AddWithValue("@Address", crematory.Address);
            command.Parameters.AddWithValue("@ContactInfo", crematory.ContactInfo);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> InsertSchedule(CrematoryScheduleModel schedule)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertSchedule);

            if (schedule == null || schedule.DayOfWeek == null)
                return false;

            command.Parameters.AddWithValue("@CrematoryId", schedule.CrematoryId);
            command.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
            command.Parameters.AddWithValue("@OpenTime", schedule.OpenTime);
            command.Parameters.AddWithValue("@CloseTime", schedule.CloseTime);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
