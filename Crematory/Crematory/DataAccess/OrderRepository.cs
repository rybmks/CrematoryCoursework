using Crematory.Interfaces;
using Npgsql;
using Crematory.DatabaseManager;
using System.Configuration;
using System.Data;
using Crematory.Models.DatabaseModels;

namespace Crematory.DataAccess
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.DeleteOrder);

            command.Parameters.AddWithValue("@Id", id);

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetAllOrders);

            var orders = await db.FetchRecordsAsync<OrderModel>(command);

            return (List<OrderModel>)orders;
        }
        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            return await db.BeginTransactionAsync();
        }
        public async Task<int> InsertOrderAsync(OrderModel order)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertOrderReturningId);

            if (order == null)
                return -1;

            command.Parameters.AddWithValue("@CrematoryId", order.CrematoryId);
            command.Parameters.AddWithValue("@StandardPrice", order.StandardPrice);
            command.Parameters.AddWithValue("@CremationDuration", order.CremationDuration);
            command.Parameters.AddWithValue("@DeceasedId", order.DeceasedId);
            command.Parameters.AddWithValue("@ContactPersonId", order.ContactPersonId);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@CremationDateTime", order.CremationDateTime);

            var result = await db.FetchSingleIntAsync(command);
            
            return result;
        }
        public async Task<bool> UpdateOrderAsync(OrderModel order)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.UpdateOrders);

            if (order == null)
                return false;

            command.Parameters.AddWithValue("@CrematoryId", order.CrematoryId);
            command.Parameters.AddWithValue("@StandardPrice", order.StandardPrice);
            command.Parameters.AddWithValue("@CremationDuration", order.CremationDuration);
            command.Parameters.AddWithValue("@DeceasedId", order.DeceasedId);
            command.Parameters.AddWithValue("@ContactPersonId", order.ContactPersonId);
            command.Parameters.AddWithValue("@Id", order.Id);
            command.Parameters.AddWithValue("@CremationDateTime", order.CremationDateTime);

            var res = await db.ExecuteCommandAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
