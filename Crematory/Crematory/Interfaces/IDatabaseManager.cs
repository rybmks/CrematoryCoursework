using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IDatabaseManager
    {
        Task<IEnumerable<int>> ExecuteQueriesAsync(IEnumerable<DbCommand> commands);
        Task<IEnumerable<T>> GetNotesAsync<T>(DbCommand command) where T : new();
    }
}
