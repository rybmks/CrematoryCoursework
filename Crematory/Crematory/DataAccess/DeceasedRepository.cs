using Crematory.Interfaces;
using Crematory.Models;
using System.Configuration;
using Npgsql;
using Crematory.DatabaseManager;
using System.Data.Common;

namespace Crematory.DataAccess
{
    public class DeceasedRepository : IDeceasedRepository
    {
        public async Task<bool> DeleteDeceasedAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            var command = new NpgsqlCommand(SqlQueries.DeleteDeceased);
            command.Parameters.AddWithValue("Id", id);

            var res = await db.ExecuteCommandAsync([command]);

            if (res.Any() && res.First() > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> InsertDeceasedAsync(DeceasedModel deceased)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            if (deceased == null || deceased.FullName == null)
                throw new NullReferenceException();

            var command = new NpgsqlCommand(SqlQueries.InsertDeceased);

            command.Parameters.AddWithValue("FullName", deceased.FullName);
            command.Parameters.AddWithValue("BirthDate", deceased.BirthDate);
            command.Parameters.AddWithValue("DeathDate", deceased.DeathDate);
            command.Parameters.AddWithValue("Gender", deceased.Gender.ToString());

            var res = await db.ExecuteCommandAsync([command]);

            if (res.Any() && res.First() > 0) 
            {
                return true;
            }

            return false;
        }
        public async Task<int> GetDeceasedIdAsync(DeceasedModel deceased)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            if (deceased == null || deceased.FullName == null)
                throw new NullReferenceException();

            var command = new NpgsqlCommand(SqlQueries.GetDeceasedId);

            command.Parameters.AddWithValue("@FullName", deceased.FullName);
            command.Parameters.AddWithValue("@BirthDate", deceased.BirthDate);
            command.Parameters.AddWithValue("@DeathDate", deceased.DeathDate);

            var res = await db.FetchSingleIntAsync(command);
            if (res <= 0)
                return -1;
            else
                return res;
            
        }
    }
}
