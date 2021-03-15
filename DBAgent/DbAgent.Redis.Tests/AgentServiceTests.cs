using System.Threading.Tasks;
using DbAgent.Service.FireBird;
using DbAgent.Service.Tests.Helpers;
using DbAgent.Tests.Core;
using DBAgent.Watcher.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbAgent.Service.Tests
{
    [TestClass]
    public class AgentServiceTests
    {
        [TestMethod]
        public async Task ProcessEventsInsertServiceTests()
        {
            var processEventsId = 7999;
            var cmd = $"INSERT INTO PROCESS_EVENTS (ID) VALUES ({processEventsId});";

            var sqlExecuter = new FbSqlExecuter(TestsContext.MainDbConnectionString);
            sqlExecuter.ExecuteNonQuery($"DELETE FROM PROCESS_EVENTS WHERE ID = {processEventsId}",
                (ex) => { });

            using (var testEngine = new DbAgentServiceTestEngine<ProcessEventsActionModel>())
            {
                await testEngine.CaptureModel(cmd, (model) => model.Id == processEventsId);
            }

            sqlExecuter.ExecuteNonQuery($"DELETE FROM PROCESS_EVENTS WHERE ID = {processEventsId}", 
                (ex) => { });
        }

    }
}
