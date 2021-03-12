namespace DbAgent.Watcher.Models
{
    public interface IModel
    {
        int UpdateId { get; }

        string GetDbProperty(string modelPropertyName);

        string GetTempTableName();
    }
}
