using DBAgent.Watcher;

namespace DbAgent.Watcher.Tests
{
    internal static class AppContext
    {
        public static string MainDbConnectionString => Properties.Resources.MainDb_ConnectionString_SQL;

        public static string TempDbConnectionString => Properties.Resources.TempDb_ConnectionString_SQL;

        public static string ExternalDbSource => @"C:\DataBases\ACTIONSDB.FDB";
        public static string ExternalUser => "SYSDBA";
        public static string ExternalPassword => "masterkey";
    }
}
