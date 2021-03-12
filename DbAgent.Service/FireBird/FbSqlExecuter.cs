using System;
using FirebirdSql.Data.FirebirdClient;

namespace DbAgent.Service.FireBird
{
    public class FbSqlExecuter: IFbSqlExecuter
    {
        public FbSqlExecuter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public void ExecuteNonQuery(string cmdQuery)
        {
            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new FbCommand(cmdQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public bool ExecuteNonQuery(string cmdQuery, Action<Exception> onError)
        {
            try
            {
                ExecuteNonQuery(cmdQuery);
                return true;
            }
            catch (Exception ex)
            {
                onError.Invoke(ex);
                return false;
            }
        }
    }
}
