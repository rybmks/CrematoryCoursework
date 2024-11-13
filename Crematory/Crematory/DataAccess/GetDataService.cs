using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crematory.DatabaseServices;
using Crematory.Models;
using Npgsql;

namespace Crematory.DataAccess
{
    public static class GetDataService
    {
        public static async Task<List<ServiceModel>> GetServicesDataAsync() 
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllCrematoryServices);

            var services = await db.GetNotesAsync<ServiceModel>(command);

            return (List<ServiceModel>)services;
        }
        public static async Task<List<CrematoryScheduleModel>> GetScheduleDataAsync() {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllCrematorySchedule);

            var services = await db.GetNotesAsync<CrematoryScheduleModel>(command);

            return (List<CrematoryScheduleModel>)services;
        }
        public static async Task<List<CrematoryModel>> GetCrematoryDataAsync() {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllCrematories);

            var services = await db.GetNotesAsync<CrematoryModel>(command);

            return (List<CrematoryModel>)services;
        }
        public static async Task<List<CrematoryScheduleModel>> GetScheduleForCrematoryAsync(int crematoryId)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetScheduleOfCrematory);
            command.Parameters.AddWithValue("@Id", crematoryId);

            var schedule = await db.GetNotesAsync<CrematoryScheduleModel>(command);

            return (List<CrematoryScheduleModel>)schedule;
        }
        public static async Task<bool> IsScheduleIntersects(CrematoryScheduleModel schedule)
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
    }
}
