using Crematory.Interfaces;
using Crematory.DatabaseManager;
using Npgsql;
using System.Configuration;
using System.Data.Common;
using Crematory.Models.DatabaseModels;

namespace Crematory.DataAccess
{
    public class ContactPersonRepository : IContactPersonRepository
    {
        public async Task<bool> DeleteContactPersonAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertDeceased);
          
            command.Parameters.AddWithValue("Id", id);

            var res = await db.ExecuteCommandAsync([command]);

            if (res.Any() && res.First() > 0)
            {
                return true;
            }
            
            return false;
        }
        public async Task<int> GetContactPersonIdAsync(ContactPersonModel contactPerson)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            if (contactPerson == null || contactPerson.FullName == null ||
                contactPerson.PhoneNumber == null || contactPerson.Address == null)
                throw new NullReferenceException();

            var command = new NpgsqlCommand(SqlQueries.GetContactPersonId);

            command.Parameters.AddWithValue("@FullName", contactPerson.FullName);
            command.Parameters.AddWithValue("@PhoneNumber", contactPerson.PhoneNumber);
            command.Parameters.AddWithValue("@Address", contactPerson.Address);

            var res = await db.FetchSingleIntAsync(command);
            if (res <= 0)
                return -1;
            else
                return res;
        }
        public async Task<bool> InsertContactPersonAsync(ContactPersonModel contactPerson)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            if (contactPerson == null || contactPerson.FullName == null
                || contactPerson.PhoneNumber == null || contactPerson.Address == null)
                throw new NullReferenceException();

            var command = new NpgsqlCommand(SqlQueries.InsertContactPerson);

            command.Parameters.AddWithValue("FullName", contactPerson.FullName);
            command.Parameters.AddWithValue("PhoneNumber", contactPerson.PhoneNumber);
            command.Parameters.AddWithValue("Address", contactPerson.Address);

            var res = await db.ExecuteCommandAsync([command]);

            if (res.Any() && res.First() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
