using System;
using System.Threading.Tasks;
using DBAgent.Watcher.Models;
using DbAgent.Watcher.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbAgent.Watcher.Tests
{
    [TestClass]
    public class WatcherTests
    {
        [TestMethod]
        public async Task ProcessEventsInsertCaptureTest()
        {
            var rnd = new Random();
            var processEventsId = rnd.Next(1, 1000000);
            var cmd = $"INSERT INTO PROCESS_EVENTS (ID) VALUES ({processEventsId});";

            using (var testEngine = new ModelTestEngine<ProcessEventsActionModel>())
            {
               await testEngine.CaptureModel(cmd, (model) => model.Id == processEventsId);
            }
        }
    }
}
