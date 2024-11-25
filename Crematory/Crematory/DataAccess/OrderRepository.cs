using Crematory.Interfaces;
using Npgsql;
using Crematory.DatabaseManager;
using System.Configuration;
using System.Data;
using Crematory.Models.DatabaseModels;
using Crematory.Models.AppModels;
using System.Windows.Controls;

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
        public async Task<List<FullOrderInfoModel>> GetAllPlannedOrdersAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetNotCompletedOrders);

            var orders = await db.FetchRecordsAsync<FullOrderInfoModel>(command);
            
            return (List<FullOrderInfoModel>)orders;
        }
        public async Task<List<FullOrderInfoModel>> GetCompletedOrdersAsync()
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.GetCompletedOrders);

            var orders = await db.FetchRecordsAsync<FullOrderInfoModel>(command);

            return (List<FullOrderInfoModel>)orders;
        }
        public async Task<int> InsertCompletedOrder(CompletedOrderModel order)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertCompletedOrder);

            if (order == null)
                return -1;

            command.Parameters.AddWithValue("@OrderId", order.OrderId);
            command.Parameters.AddWithValue("@CompletionReason", order.CompetionReason ?? "Виконано");

            var result = await db.FetchSingleIntAsync(command);

            return result;
        }
        public async Task<bool> DeleteCompletedAsync(int orderId){
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);

            List<NpgsqlCommand> commands = [];

            var deleteFromCompleted = new NpgsqlCommand(SqlQueries.DeletedCompleted);
            
            deleteFromCompleted.Parameters.AddWithValue("@OrderId", orderId);
            
            var deleteFromOrders = new NpgsqlCommand(SqlQueries.DeleteOrder);
            deleteFromOrders.Parameters.AddWithValue("@Id", orderId);

            commands.Add(deleteFromOrders);
            commands.Add(deleteFromCompleted);


            var res = await db.ExecuteCommandAsync(commands);

            if (res.Count() != commands.Count || res.Any(r => r == 0))
            {
                return false; 
            }

            return true;
        }
    }
}
