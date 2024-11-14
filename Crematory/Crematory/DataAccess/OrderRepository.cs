using Crematory.DatabaseServices;
using Crematory.Interfaces;
using Crematory.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

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

            var orders = await db.GetNotesAsync<OrderModel>(command);

            return (List<OrderModel>)orders;
        }
        public async Task<bool> InsertOrderAsync(OrderModel order)
        {
            var db = new PgDatabaseManager(ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString);
            var command = new NpgsqlCommand(SqlQueries.InsertOrder);

            if (order == null)
                return false;

            command.Parameters.AddWithValue("@CrematoryId", order.CrematoryId);
            command.Parameters.AddWithValue("@StandardPrice", order.StandardPrice);
            command.Parameters.AddWithValue("@CremationDuration", order.CremationDuration);
            command.Parameters.AddWithValue("@DeceasedId", order.DeceasedId);
            command.Parameters.AddWithValue("@ContactPersonId", order.ContactPersonId);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@CremationDateTime", order.CremationDateTime);

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
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

            var res = await db.ExecuteQueriesAsync(new List<NpgsqlCommand> { command });

            if (!res.Any() || res.First() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
