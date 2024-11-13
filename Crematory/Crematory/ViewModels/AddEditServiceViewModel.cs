using Crematory.DataAccess;
using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels
{
    public class AddEditServiceViewModel
    {
        public async Task<bool> AddService(ServiceModel newItem)
        {
            var res = await InsertDataService.InsertService(newItem);
            return res;
        }
        public async Task<bool> DeleteService(ServiceModel newItem)
        {
            var res = await DeleteDataService.DeleteService(newItem);
            return res;
        }
        public async Task<bool> UpdateService(ServiceModel updatedItem)
        {
            var res = await UpdateDataService.UpdateService(updatedItem);
            return res;
        }
    }
}
