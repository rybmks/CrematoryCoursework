using Crematory.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IServiceRepository
    {
        Task<bool> DeleteServiceAsync(int id);
        Task<bool> InsertServiceAsync(ServiceModel service);
        Task<bool> UpdateServiceAsync(ServiceModel service);
        Task<List<ServiceModel>> GetAllServicesAsync();
        Task<bool> AddSelectedServices(List<ServiceModel> services, int orderId);
        Task<List<ServiceModel>> GetServicesForOrderAsync(int orderId);
    }
}
