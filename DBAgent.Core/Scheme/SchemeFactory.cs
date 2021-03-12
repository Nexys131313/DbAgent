using System.Collections.Generic;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Scheme
{
    public class SchemeFactory<TModel> : ISchemeFactory<TModel> where TModel : IModel, new()
    {
        public SchemeFactory(string externalDbSource, string externalUser, string externalPassword)
        {
            ExternalUser = externalUser;
            ExternalDbSource = externalDbSource;
            ExternalPassword = externalPassword;
        }

        public string ExternalDbSource { get; }
        public string ExternalUser { get; }
        public string ExternalPassword { get; }

        public SqlTriggerScheme<TModel> CreateScheme(TriggerType triggerType)
        {
            return SqlTriggerScheme<TModel>.CreateScheme(ExternalDbSource,
                ExternalUser, ExternalPassword, triggerType);
        }

        public IEnumerable<SqlTriggerScheme<TModel>> CreateSchemes(params TriggerType[] triggerTypes)
        {
            return SqlTriggerScheme<TModel>.CreateSchemes(ExternalDbSource,
                ExternalUser, ExternalPassword, triggerTypes);
        }
    }
}
