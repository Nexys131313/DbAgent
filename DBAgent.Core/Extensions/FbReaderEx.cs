using System;
using System.Data;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Extensions
{
    internal static class FbReaderEx
    {
        public static T? Value<T>(this object dbValue) where T: struct
        {
            if (dbValue is DBNull)
            {
                return null;
            }
            else
            {
                return (T) dbValue;
            }
        }

        public static TModel ReadAsModel<TModel>(this IDataRecord reader)
            where TModel : IModel, new()
        {
            var model = new TModel();
            foreach (var property in model.GetType().GetProperties())
            {
                var value = reader[model.GetDbProperty(property.Name)];

                if (value is DBNull)
                    value = null;

                property.SetValue(model, value);
            }

            return model;
        }
    }
}
