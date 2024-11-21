using Crematory.Interfaces;
using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels.AdminWindow
{
    class AddEditCrematoryViewModel
    {
        private readonly ICrematoryRepository _repository;
        public AddEditCrematoryViewModel(ICrematoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddCrematory(CrematoryModel newItem)
        {
            var res = await _repository.InsertCrematoryAsync(newItem);
            return res;
        }
        public async Task<bool> DeleteCrematory(CrematoryModel Item)
        {
            var res = await _repository.DeleteCrematoryAsync(Item.Id);
            return res;
        }
        public async Task<bool> UpdateCrematory(CrematoryModel updatedItem)
        {
            var res = await _repository.UpdateCrematoryAsync(updatedItem);
            return res;
        }
    }
}
