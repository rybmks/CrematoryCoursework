using Crematory.Interfaces;
using Crematory.Models.DatabaseModels;

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
            try
            {
                var res = await _repository.DeleteCrematoryAsync(Item.Id);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateCrematory(CrematoryModel updatedItem)
        {
            var res = await _repository.UpdateCrematoryAsync(updatedItem);
            return res;
        }
    }
}
