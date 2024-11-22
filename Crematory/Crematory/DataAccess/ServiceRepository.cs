using System.Configuration;
using Crematory.DatabaseManager;
using Crematory.Interfaces;
using Crematory.Models;
using Npgsql;

namespace Crematory.DataAccess
{
    public class ServiceRepository : IServiceRepository 
    {
        public async Task<List<ServiceModel>> GetAllServicesAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllCrematoryServices);

            var services = await db.FetchRecordsAsync<ServiceModel>(command);

            return (List<ServiceModel>)services;
        }
        public async Task<bool> UpdateServiceAsync(ServiceModel service)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.UpdateService);

            if (service == null || service.Name == null)
                return false;

            command.Parameters.AddWithValue("@Name", service.Name);
            command.Parameters.AddWithValue("@Price", service.Price);
            command.Parameters.AddWithValue("@Id", service.Id);

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> InsertServiceAsync(ServiceModel service)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertService);

            if (service == null || service.Name == null)
                return false;

            command.Parameters.AddWithValue("@Name", service.Name);
            command.Parameters.AddWithValue("@Price", service.Price);

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> DeleteServiceAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteService);

            command.Parameters.AddWithValue("@Id", id);

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
