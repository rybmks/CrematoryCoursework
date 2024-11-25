using Crematory.Interfaces;
using Crematory.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels.AdminWindow
{
    public class AddEditServiceViewModel
    {
        private readonly IServiceRepository _repository;
        public AddEditServiceViewModel(IServiceRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddService(ServiceModel newItem)
        {
            var res = await _repository.InsertServiceAsync(newItem);
            return res;
        }
        public async Task<bool> DeleteService(ServiceModel Item)
        {
            var res = await _repository.DeleteServiceAsync(Item.Id);
            return res;
        }
        public async Task<bool> UpdateService(ServiceModel updatedItem)
        {
            var res = await _repository.UpdateServiceAsync(updatedItem);
            return res;
        }
    }
}
