namespace DbAgent.Watcher.Models
{
    public interface IModel
    {
        string GetDbProperty(string modelPropertyName);
    }
}
