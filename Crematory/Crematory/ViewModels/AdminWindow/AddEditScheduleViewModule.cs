using Crematory.Interfaces;
using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels.AdminWindow
{
    public class AddEditScheduleViewModule
    {
        private readonly IScheduleRepository _repository;
        public AddEditScheduleViewModule(IScheduleRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddSchedule(CrematoryScheduleModel newItem)
        {
            if (await _repository.IsScheduleExistsToday(newItem))
                return false;

            var res = await _repository.InsertScheduleAsync(newItem);
            return res;
        }
        public async Task<bool> DeleteSchedule(CrematoryScheduleModel item)
        {
            var res = await _repository.DeleteScheduleAsync(item.Id);
            return res;
        }
        public async Task<bool> UpdateSchedule(CrematoryScheduleModel updatedItem)
        {
            if (await _repository.IsScheduleExistsToday(updatedItem))
                return false;

            var res = await _repository.UpdateScheduleAsync(updatedItem);
            return res;
        }
    }
}
