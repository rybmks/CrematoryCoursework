using Crematory.Interfaces;
using Npgsql;
using System.Configuration;
using Crematory.DatabaseManager;
using Crematory.Models.DatabaseModels;
using Crematory.Models.AppModels;


namespace Crematory.DataAccess
{
    public class ScheduleRepository : IScheduleRepository
    {
        public async Task<List<CrematoryScheduleModel>> GetScheduleForCrematoryAsync(int crematoryId)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetScheduleOfCrematory);
            command.Parameters.AddWithValue("@CrematoryId", crematoryId);

            var schedule = await db.FetchRecordsAsync<CrematoryScheduleModel>(command);

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


            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

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

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

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

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> IsScheduleExistsToday(CrematoryScheduleModel schedule)
        {
            if (schedule == null || schedule.DayOfWeek == null)
                throw new NullReferenceException();

            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.IsScheduleExistsToday);

            command.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);

            var res = await db.FetchRecordsAsync<CrematoryScheduleModel>(command);

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
            command.Parameters.Add(new NpgsqlParameter("DayOfWeek", NpgsqlTypes.NpgsqlDbType.Text) { Value = dayOfWeek.ToString() });

            var schedule = await db.FetchRecordsAsync<CrematoryScheduleModel>(command);

            return (List<CrematoryScheduleModel>)schedule;
        }
        public async Task<List<TimePeriod>> GetFreeTimeAsync(int crematoryId, DateOnly date)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            var commandOrders = new NpgsqlCommand(SqlQueries.GetOrderTime);
            commandOrders.Parameters.AddWithValue("@CrematoryId", crematoryId);
            commandOrders.Parameters.AddWithValue("@DateTime", date); 
            var orders = await db.FetchRecordsAsync<TimePeriod>(commandOrders);

            string dayOfWeek = date.DayOfWeek.ToString(); 
            
            var commandSchedule = new NpgsqlCommand(SqlQueries.GetCrematoryScheduleForDay);
            commandSchedule.Parameters.AddWithValue("@CrematoryId", crematoryId);
            commandSchedule.Parameters.AddWithValue("@DayOfWeek", dayOfWeek); 
            var schedule = await db.FetchRecordsAsync<TimePeriod>(commandSchedule);

            if (schedule == null || !schedule.Any())
                return new List<TimePeriod>();

            var dailySchedule = schedule.First();

            var allPeriods = new List<TimePeriod>
            {
                new() { StartTime = dailySchedule.StartTime, EndTime = dailySchedule.StartTime } 
            };
            
            allPeriods.AddRange(orders);
            allPeriods.Add(new TimePeriod { StartTime = dailySchedule.EndTime, EndTime = dailySchedule.EndTime }); 

            allPeriods = [.. allPeriods.OrderBy(p => p.StartTime)];

            var gaps = new List<TimePeriod>();
            for (int i = 0; i < allPeriods.Count - 1; i++)
            {
                var current = allPeriods[i];
                var next = allPeriods[i + 1];

                if (current.EndTime < next.StartTime)
                {
                    gaps.Add(new TimePeriod
                    {
                        StartTime = current.EndTime,
                        EndTime = next.StartTime
                    });
                }
            }

            return gaps; 
        }
    }
}
