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
    public class CrematoryRepository : ICrematoryRepository
    {
        public async Task<bool> InsertCrematoryAsync(CrematoryModel crematory)
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
        public async Task<bool> DeleteCrematoryAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteCrematory);

            command.Parameters.AddWithValue("@Id", id);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> UpdateCrematoryAsync(CrematoryModel crematory)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
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
        public async Task<List<CrematoryModel>> GetAllCrematoriesAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllCrematories);

            var services = await db.GetNotesAsync<CrematoryModel>(command);

            return (List<CrematoryModel>)services;
        }
    }
}
