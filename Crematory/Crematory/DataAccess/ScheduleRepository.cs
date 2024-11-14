using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crematory.DatabaseServices;
using Crematory.Interfaces;
using Crematory.Models;
using Npgsql;

namespace Crematory.DataAccess
{
    public class ScheduleRepository : IScheduleRepository
    {
        public async Task<List<CrematoryScheduleModel>> GetScheduleForCrematoryAsync(int crematoryId)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetScheduleOfCrematory);
            command.Parameters.AddWithValue("@Id", crematoryId);

            var schedule = await db.GetNotesAsync<CrematoryScheduleModel>(command);

            return (List<CrematoryScheduleModel>)schedule;
        }
        public async Task<bool> UpdateScheduleAsync(CrematoryScheduleModel schedule)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
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
        public async Task<bool> InsertScheduleAsync(CrematoryScheduleModel schedule)
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
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteSchedule);

            command.Parameters.AddWithValue("@Id", id);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> IsScheduleIntersectsAsync(CrematoryScheduleModel schedule)
        {
            if (schedule == null || schedule.DayOfWeek == null)
                throw new NullReferenceException();

            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.IntersectionOfSchedules);

            command.Parameters.AddWithValue("@CrematoryId", schedule.CrematoryId);
            command.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
            command.Parameters.AddWithValue("@OpenTime", schedule.OpenTime);
            command.Parameters.AddWithValue("@CloseTime", schedule.CloseTime);

            var res = await db.GetNotesAsync<CrematoryScheduleModel>(command);

            if (res.Any())
            {
                return true;
            }

            return false;
        }

        public async Task<List<CrematoryScheduleModel>> GetCrematoryScheduleForDayAsync(int crematoryId, DayOfWeek dayOfWeek)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetScheduleForDay);
            command.Parameters.AddWithValue("@Id", crematoryId);
            command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

            var schedule = await db.GetNotesAsync<CrematoryScheduleModel>(command);

            return (List<CrematoryScheduleModel>)schedule;
        }
    }
}
