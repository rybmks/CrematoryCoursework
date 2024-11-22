using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> InsertOrderAsync(OrderModel order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<IDbTransaction> BeginTransactionAsync();
    }
}
