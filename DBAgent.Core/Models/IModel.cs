using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAgent.Watcher.Models
{
    public interface IModel
    {
        string GetDbProperty(string modelPropertyName);
    }
}
