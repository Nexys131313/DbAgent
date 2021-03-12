using System;
using System.Collections.Generic;
using System.Linq;
using DbAgent.Redis.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbAgent.Redis.Tests
{
    [TestClass]
    public class BaseRedisClientTests
    {
        [TestMethod]
        public void SetValueMethodTest()
        {
            var client = new BaseRedisClient();
            var key = $"Set_Value_Key";
            var value = $"Set_Value";

            client.SaveData(key, value);

            var remoteValue = client.GetValue(key);

            Assert.AreEqual(value, remoteValue);
        }

        [TestMethod]
        public void DeleteValueMethodTest()
        {
            var client = new BaseRedisClient();
            var key = $"Delete_Value_Key";
            var value = $"Delete_Value";

            client.SaveData(key, value);
            client.Delete(key);
            var remoteValue = client.GetValue(key);

            Assert.AreEqual(string.IsNullOrWhiteSpace(remoteValue), true);

        }

        [TestMethod]
        public void KeysTest()
        {
            var client = new BaseRedisClient();
            var db = client.GetDataBase();

            var key = "Key_List_Test";
            var values = new List<string>()
            {
                "Value1",
                "Value2",
                "Value3",
            };

            client.Delete(key);

            foreach (var value in values)
                db.ListRightPush(key, value);

            var redisValues = db.ListRange(key, 0, 10);
            var remoteValues = redisValues.Select(item => item.ToString()).ToList();

            for (var i = 0; i < remoteValues.Count; i++)
            {
                var localValue = values[i];
                var remoteValue = remoteValues[i];
                Assert.AreEqual(localValue, remoteValue);
            }

            client.Delete(key);

            redisValues = db.ListRange(key, 0, 10);
            remoteValues = redisValues.Select(item => item.ToString()).ToList();

            Assert.AreEqual(remoteValues.Count, 0);
        }
    }
}
