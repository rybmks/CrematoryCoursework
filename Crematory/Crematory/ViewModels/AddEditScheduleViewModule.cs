using Crematory.DataAccess;
using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels
{
    public class AddEditScheduleViewModule
    {
        public async Task<bool> AddSchedule(CrematoryScheduleModel newItem)
        {
            if (await GetDataService.IsScheduleIntersects(newItem))
                return false;

            var res = await InsertDataService.InsertSchedule(newItem);
            return res;
        }
        public async Task<bool> DeleteSchedule(CrematoryScheduleModel item)
        {
            var res = await DeleteDataService.DeleteSchedule(item);
            return res;
        }
        public async Task<bool> UpdateSchedule(CrematoryScheduleModel updatedItem)
        {
            if (await GetDataService.IsScheduleIntersects(updatedItem))
                return false;

            var res = await UpdateDataService.UpdateSchedule(updatedItem);
            return res;
        }
    }
}
