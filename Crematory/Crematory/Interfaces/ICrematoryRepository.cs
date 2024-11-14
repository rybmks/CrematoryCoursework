using Crematory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface ICrematoryRepository
    {
        Task<bool> InsertCrematoryAsync(CrematoryModel crematory);
        Task<bool> DeleteCrematoryAsync(int id);
        Task<bool> UpdateCrematoryAsync(CrematoryModel crematory);
        Task<List<CrematoryModel>> GetAllCrematoriesAsync();
    }
}
