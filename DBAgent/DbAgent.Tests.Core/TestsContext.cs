namespace DbAgent.Tests.Core
{
    public static class TestsContext
    {
        public static string MainDbConnectionString => Properties.Resources.maindb_connectionstring_sql;
        public static string TempDbConnectionString => Properties.Resources.tempdb_connectionstring_sql;

        public static string ExternalDbSource => @"C:\DataBases\ACTIONSDB.FDB";
        public static string ExternalUser => "SYSDBA";
        public static string ExternalPassword => "masterkey";

        public static string RedisConnectionString => "10.44.127.201:6379, password=PnRD0BiAFj8F%7P$09G9";
        public static string MainDbId = "MY_DB";

    }
}
