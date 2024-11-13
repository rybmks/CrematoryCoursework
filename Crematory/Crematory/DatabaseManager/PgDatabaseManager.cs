using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using Crematory.Interfaces;
using System.Data.Common;
using System.Linq;

namespace Crematory.DataAccess
{
    public class PgDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;

        public PgDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<int>> ExecuteQueriesAsync(IEnumerable<DbCommand> commands)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            var rowsAffected = new List<int>(commands.Count());

            try
            {
                foreach (var command in commands)
                {
                    command.Transaction = transaction;
                    command.Connection = connection;
                    rowsAffected.Add(await command.ExecuteNonQueryAsync());
                }
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new InvalidOperationException("Помилка під час виконання транзакції.", e);
            }

            return rowsAffected;
        }
        public async Task<IEnumerable<T>> GetNotesAsync<T>(DbCommand command) where T : new()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            command.Connection = connection;
            await connection.OpenAsync();

            List<T> result = new List<T>();
            try
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    while (await reader.ReadAsync())
                    {
                        var item = new T();

                        foreach (var property in properties)
                        {
                            var columnName = property.Name;

                            if (!reader.HasColumn(columnName)) continue;

                            var value = reader[columnName];

                            if (value == DBNull.Value) continue;

                            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            property.SetValue(item, Convert.ChangeType(value, targetType));
                        }

                        result.Add(item);
                    }
                }
            }
            catch (Exception e) 
            {

                throw new InvalidOperationException("Помилка під час виконання транзакції.", e);
            }

            return result;
        }
    }
}
