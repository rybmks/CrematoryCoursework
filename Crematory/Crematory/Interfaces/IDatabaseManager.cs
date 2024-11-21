using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IDatabaseManager
    {
        Task<IEnumerable<int>> ExecuteCommandAsync(IEnumerable<DbCommand> commands);
        Task<IEnumerable<T>> FetchRecordsAsync<T>(DbCommand command) where T : new();
        Task<int> FetchSingleIntAsync(DbCommand command);
        Task<IDbTransaction> BeginTransactionAsync();
    }
}
