using Crematory.DataAccess;
using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels
{
    class AddEditCrematoryViewModel
    {
        public async Task<bool> AddCrematory(CrematoryModel newItem)
        {
            var res = await InsertDataService.InsertCrematory(newItem);
            return res;
        }
        public async Task<bool> DeleteCrematory(CrematoryModel newItem)
        {
            var res = await DeleteDataService.DeleteCrematory(newItem);
            return res;
        }
        public async Task<bool> UpdateCrematory(CrematoryModel updatedItem)
        {
            var res = await UpdateDataService.UpdateCrematory(updatedItem);
            return res;
        }
    }
}
