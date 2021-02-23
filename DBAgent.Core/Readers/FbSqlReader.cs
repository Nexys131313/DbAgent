using System;
using System.Collections.Generic;
using DbAgent.Watcher.Extensions;
using DBAgent.Watcher.Models;
using FirebirdSql.Data.FirebirdClient;

namespace DBAgent.Watcher.Readers
{
    public class FbSqlReader
    {

        public FbSqlReader(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }


        public List<ProcessEventsActionModel> ReadAllProcessEventsActionModels()
        {
            var models = new List<ProcessEventsActionModel>();

            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();
                var cmdQuery = "SELECT * FROM PROCESS_EVENTS_ACTIONS";

                using (var command = new FbCommand(cmdQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var id = reader["ID"].Value<int>();
                            var done = reader["DONE"].Value<int>();
                            var categoryId = reader["CATEGORY_ID"].Value<int>();
                            var objectId = reader["OBJECT_ID"].Value<int>();
                            var soundOffTime = reader["SOUND_OFF_TIME"].Value<DateTime>();
                            var electrician = reader["ELECTRICIAN"].Value<int>();
                            var gzId = reader["GZ_ID"].Value<int>();
                            var arriveTime = reader["ARRIVE_TIME"].Value<DateTime>();
                            var doneTimestamp = reader["DONE_TIMESTAMP"].Value<DateTime>();
                            var soundOff = reader["SOUND_OFF"].Value<int>();
                            var electricianOn = reader["ELECTRICIAN_ON"].Value<int>();
                            var gzOn = reader["GZ_ON"].Value<int>();
                            var arriveOn = reader["ARRIVE_ON"].Value<int>();
                            var reasonOn = reader["REASON_ON"].Value<int>();
                            var gzTime = reader["GZ_TIME"].Value<DateTime>();
                            var descOn = reader["DESC_ON"].Value<int>();
                            var userId = reader["USER_ID"].Value<int>();
                            var wUserName = reader["WUSER_NAME"].ValueStr();
                            var hostName = reader["HOST_NAME"].ValueStr();
                            var reasonId = reader["REASON_ID"].Value<int>();
                            var gObjectId = reader["GOBJECT_ID"].Value<int>();
                            var departureGz = reader["DEPARTURE_GZ"].Value<int>();
                            var creationTime = reader["CREATION_TIME"].Value<DateTime>();
                            var isDeleted = reader["IS_DELETE_ACTION"].Value<int>();
                            var isUpdated = reader["IS_UPDATE_ACTION"].Value<int>();
                            var isInsert = reader["IS_INSERT_ACTION"].Value<int>();

                            var model = new ProcessEventsActionModel()
                            {
                                Id = id,
                                DepartureGz = departureGz,
                                ArriveOn = arriveOn,
                                ArriveTime = arriveTime,
                                GObjectId = gObjectId,
                                CategoryId = categoryId,
                                CreationTime = creationTime,
                                DescOn = descOn,
                                Done = done,
                                DoneTimestamp = doneTimestamp,
                                IsDeletedAction = isDeleted,
                                IsInsertAction = isInsert,
                                IsUpdateAction = isUpdated,
                                GzId = gzId,
                                GzOn = gzOn,
                                GzTime = gzTime,
                                UserId = userId,
                                WUserName = wUserName,
                                HostName = hostName,
                                ObjectId = objectId,
                                Electrician = electrician,
                                ElectricianOn = electricianOn,
                                ReasonOn = reasonOn,
                                ReasonId = reasonId,
                                SoundOff = soundOff,
                                SoundOffTime = soundOffTime,
                            };

                            models.Add(model);

                        }
                    }
                }
            }

            return models;
        }
    }
}
