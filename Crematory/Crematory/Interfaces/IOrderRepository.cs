using Crematory.Models.AppModels;
using Crematory.Models.DatabaseModels;
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
        Task<int> InsertOrderAsync(OrderModel order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<IDbTransaction> BeginTransactionAsync();
        Task<List<FullOrderInfoModel>> GetAllPlannedOrdersAsync();
        Task<int> InsertCompletedOrder(CompletedOrderModel order);
        Task<List<FullOrderInfoModel>> GetCompletedOrdersAsync();
        Task<bool> DeleteCompletedAsync(int orderId);
    }
}
