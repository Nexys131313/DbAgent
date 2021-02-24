using System;
using System.Reflection;
using DbAgent.Watcher.Attributes;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Helpers
{
    public class SqlTriggerBuilder
    {
        public static string BuildSqlTrigger<TModel>(SqlTriggerScheme<TModel> scheme)
            where TModel : IModel, new()
        {
            var sql = GetOpenPart(scheme);
            sql += GetInsertQuery(scheme);
            sql += GetExecuteStatement(scheme);
            sql += GetClosePart(scheme);
            return sql;
        }

        private static string GetInsertQuery<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var query = $"S = 'INSERT INTO {scheme.ExternalTableName} ";
            query += GetTableFieldsSequence(scheme);
            query += GetValuesSequence(scheme);
            return query;
        }

        private static string GetExecuteStatement<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var prefix = GetExternalInsertPrefix(scheme.TriggerType);
            var sql = "EXECUTE STATEMENT (:S) (";
            var counter = 1;

            foreach (var propertyInfo in scheme.InsertDataModel.GetType().GetProperties())
            {
                var attribute = (DbPropertyAttribute)propertyInfo.GetCustomAttribute(typeof(DbPropertyAttribute));

                if (attribute.OnlyTempDbField && attribute.TriggerProperty == scheme.TriggerType)
                {
                    sql += $"V{counter} := 1, ";
                    counter++;
                }
                else
                {
                    if (attribute.OnlyTempDbField) continue;

                    sql += $"V{counter} := {prefix}.{attribute.PropertyName}, ";
                    counter++;

                }

            }

            sql = sql.Substring(0, sql.Length - 2);
            sql += ")";

            return sql;
        }

        private static string GetExternalInsertPrefix(TriggerType triggerType)
        {
            switch (triggerType)
            {
                case TriggerType.Delete:
                    return "OLD";
                case TriggerType.Insert:
                case TriggerType.Update:
                    return "NEW";

                default:
                    throw new NotSupportedException($"Trigger type: {triggerType} not supported");
            }
        }

        private static string GetValuesSequence<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var sequence = "values (";
            var counter = 1;

            foreach (var propertyInfo in scheme.InsertDataModel.GetType().GetProperties())
            {
                var attribute = (DbPropertyAttribute)propertyInfo.GetCustomAttribute(typeof(DbPropertyAttribute));
                if (attribute.OnlyTempDbField) continue;

                sequence += $":V{counter}, ";
                counter++;
            }

            sequence += $":V{counter}, "; // for insert, update or delete

            sequence = sequence.Substring(0, sequence.Length - 2);
            sequence += ")'; ";
            return sequence;
        }

        private static string GetTableFieldsSequence<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var sequence = "(";

            foreach (var propertyInfo in scheme.InsertDataModel.GetType().GetProperties())
            {
                var attribute = (DbPropertyAttribute)propertyInfo.GetCustomAttribute(typeof(DbPropertyAttribute));

                if (attribute.OnlyTempDbField && attribute.TriggerProperty == scheme.TriggerType)
                {
                    sequence += $"{attribute.PropertyName},";
                }
                else
                {
                    if (attribute.OnlyTempDbField) continue;
                    sequence += $"{attribute.PropertyName},";
                }
            }

            sequence = sequence.Substring(0, sequence.Length - 1);
            sequence += ") ";
            return sequence;
        }

        private static string GetOpenPart<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var sql = $"CREATE OR ALTER {scheme.TriggerName} FOR {scheme.TableName}{Environment.NewLine}";
            sql += $"ACTIVE AFTER INSERT POSITION 0{Environment.NewLine}";
            sql += $"AS{Environment.NewLine}";
            sql += $"DECLARE S VARCHAR(3000);{Environment.NewLine}";
            sql += $"BEGIN{Environment.NewLine}";
            return sql;
        }

        private static string GetClosePart<TModel>(SqlTriggerScheme<TModel> scheme)
        {
            var sql = $"ON EXTERNAL DATA SOURCE '{scheme.ExternalDataSource}' ";
            sql += $"AS USER '{scheme.ExternalUser}' PASSWORD '{scheme.ExternalPassword}';";
            sql += Environment.NewLine;
            sql += $"POST_EVENT  '{scheme.EventName}';";
            sql += $"{Environment.NewLine}END";

            return sql;
        }
    }
}
