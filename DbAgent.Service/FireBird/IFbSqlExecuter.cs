using System;

namespace DbAgent.Service.FireBird
{
    public interface IFbSqlExecuter
    {
        void ExecuteNonQuery(string command);

        bool ExecuteNonQuery(string cmdQuery, Action<Exception> onError);
    }
}
