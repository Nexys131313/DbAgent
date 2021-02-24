using System.Collections.Generic;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DbAgent.Watcher.Extensions;
using DbAgent.Watcher.Models;
using FirebirdSql.Data.FirebirdClient;

namespace DBAgent.Watcher.Readers
{
    public class FbSqlReader
    {
        public FbSqlReader(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public List<TModel> ReadModels<TModel>()
            where TModel: IModel, new()
        {
            var tableName = typeof(TModel).GetCustomAttribute<DbTableNameAttribute>().TableName;
            var models = new List<TModel>();

            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();
                var cmdQuery = $"SELECT * FROM {tableName}";

                using (var command = new FbCommand(cmdQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var model = reader.ReadAsModel<TModel>();
                            models.Add(model);
                        }
                    }
                }
            }

            return models;
        }
    }
}
