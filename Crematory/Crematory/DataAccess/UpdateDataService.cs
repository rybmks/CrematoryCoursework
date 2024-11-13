using Crematory.DatabaseServices;
using Crematory.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.DataAccess
{
    public static class UpdateDataService
    {
        public static async Task<bool> UpdateService(ServiceModel service)
        {
            var db = new PgDatabaseManager("Host=localhost;Port=5432;Database=Crematory;Username=postgres;Password=9824;");
            var command = new NpgsqlCommand(SqlQueries.UpdateService);

            if (service == null || service.Name == null)
                return false;

            command.Parameters.AddWithValue("@Name", service.Name);
            command.Parameters.AddWithValue("@Price", service.Price);
            command.Parameters.AddWithValue("@Id", service.Id);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> UpdateCrematory(CrematoryModel crematory)
        {
            var db = new PgDatabaseManager("Host=localhost;Port=5432;Database=Crematory;Username=postgres;Password=9824;");
            var command = new NpgsqlCommand(SqlQueries.UpdateCrematory);

            if (crematory == null || crematory.Name == null || 
                crematory.Address == null || crematory.ContactInfo == null)
                return false;

            command.Parameters.AddWithValue("@Name", crematory.Name);
            command.Parameters.AddWithValue("@Address", crematory.Address);
            command.Parameters.AddWithValue("@ContactInfo", crematory.ContactInfo);
            command.Parameters.AddWithValue("@Id", crematory.Id);


            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public static async Task<bool> UpdateSchedule(CrematoryScheduleModel schedule)
        {
            var db = new PgDatabaseManager("Host=localhost;Port=5432;Database=Crematory;Username=postgres;Password=9824;");
            var command = new NpgsqlCommand(SqlQueries.UpdateSchedule);

            if (schedule == null || schedule.DayOfWeek == null)
                return false;

            command.Parameters.AddWithValue("@CrematoryId", schedule.CrematoryId);
            command.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
            command.Parameters.AddWithValue("@OpenTime", schedule.OpenTime);
            command.Parameters.AddWithValue("@CloseTime", schedule.CloseTime);
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
