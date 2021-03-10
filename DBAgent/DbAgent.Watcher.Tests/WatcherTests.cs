using DBAgent.Watcher;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Helpers;
using DBAgent.Watcher.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbAgent.Watcher.Tests
{
    [TestClass]
    public class WatcherTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var watcherOptions = new FbSqlWatcherOptions(GetMainDbConnectionString(), 
                GetTempDbConnectionString(), "Test_trigger.json");
            
            var watcher = new FbSqlWatcher<ProcessEventsActionModel>(watcherOptions);
            watcher.TableChanged += Watcher_TableChanged;

            var insertScheme = new SqlTriggerScheme<ProcessEventsActionModel>(TriggerType.Insert);
            var updateScheme = new SqlTriggerScheme<ProcessEventsActionModel>(TriggerType.Update);
            var deleteScheme = new SqlTriggerScheme<ProcessEventsActionModel>(TriggerType.Delete);

            watcher.AddTrigger(insertScheme);
            watcher.AddTrigger(updateScheme);
            watcher.AddTrigger(deleteScheme);
            watcher.InitializeListeners();

            
            var triggerScheme = SqlTriggerBuilder.BuildSqlTrigger(scheme);

        }

        private void Watcher_TableChanged(object sender, Events.Args.TableChangedEventArgs<ProcessEventsActionModel> args)
        {
            
        }

        private string GetMainDbConnectionString()
        {
            return Properties.Resources.MainDb_ConnectionString_SQL;
        }

        private string GetTempDbConnectionString()
        {
            return Properties.Resources.TempDb_ConnectionString_SQL;
        }
    }
}
