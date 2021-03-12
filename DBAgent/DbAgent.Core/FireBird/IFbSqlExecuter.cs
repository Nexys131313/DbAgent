using System;

namespace DbAgent.Core.FireBird
{
    public interface IFbSqlExecuter
    {
        void ExecuteNonQuery(string command);

        void ExecuteNonQuery(string cmdQuery, Action<Exception> onError);
    }
}
