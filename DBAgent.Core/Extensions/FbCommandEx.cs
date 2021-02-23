using System;
using FirebirdSql.Data.FirebirdClient;

namespace DBAgent.Watcher.Extensions
{
    public static class FbCommandEx
    {
        public static void ExecuteNonQuery(this FbCommand command, Action<Exception> onException)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                onException?.Invoke(ex);
            }
        }
    }
}
