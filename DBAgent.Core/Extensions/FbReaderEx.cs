using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAgent.Watcher.Models;

namespace DbAgent.Watcher.Extensions
{
    public static class FbReaderEx
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
