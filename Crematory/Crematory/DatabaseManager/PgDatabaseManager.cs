using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Common;
using Crematory.Interfaces;

namespace Crematory.DatabaseManager
{
    public class PgDatabaseManager(string connectionString) : IDatabaseManager
    {
        private readonly string _connectionString = connectionString;

        public async Task<IEnumerable<int>> ExecuteCommandAsync(IEnumerable<DbCommand> commands)
        {
            using NpgsqlConnection connection = new(_connectionString);
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

                throw new InvalidOperationException($"Помилка: {e.Message}.", e);
            }

            return rowsAffected;
        }
        public async Task<IEnumerable<T>> FetchRecordsAsync<T>(DbCommand command) where T : new()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            command.Connection = connection;
            await connection.OpenAsync();

            List<T> result = [];
            try
            {
                using var reader = await command.ExecuteReaderAsync();
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
            catch (Exception e) 
            {
                throw new InvalidOperationException("Помилка під час виконання транзакції.", e);
            }

            return result;
        }
        public async Task<int> FetchSingleIntAsync(DbCommand command)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            command.Connection = connection;
            await connection.OpenAsync();
            try
            {
               var result = await command.ExecuteScalarAsync();
               return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception e)
            {

                throw new InvalidOperationException("Помилка під час виконання транзакції.", e);
            }
        }
        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.BeginTransactionAsync();
        }
    }
}
