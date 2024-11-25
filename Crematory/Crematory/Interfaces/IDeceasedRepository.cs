using Crematory.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IDeceasedRepository
    {
        Task<bool> InsertDeceasedAsync(DeceasedModel deceased);
        Task<bool> DeleteDeceasedAsync(int id);
        Task<int> GetDeceasedIdAsync(DeceasedModel deceased);
        Task<DeceasedModel> GetDeceasedById(int id);
    }
}
