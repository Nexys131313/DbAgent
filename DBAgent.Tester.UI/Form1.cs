using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBAgent.Watcher;
using DBAgent.Watcher.Enums;
using DbAgent.Watcher.Helpers;
using DbAgent.Watcher.Models;
using DBAgent.Watcher.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbAgent.Tester.UI
{
    public partial class Form1 : Form
    {
        private FbSqlWatcher<ProcessEventsActionModel> _watcher;

        public Form1()
        {
            InitializeComponent();
        }

        private void OnAddSchemesButtonClick(object sender, EventArgs e)
        {
            try
            {
                var schemes = GetProcessEventsSchemes();
                foreach (var scheme in schemes)
                    _watcher.AddTrigger(scheme);

                _watcher.InitializeListeners();

                ((Button)(sender)).BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                ((Button)(sender)).BackColor = Color.Red;
                ShowError(ex);
            }
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var options = new FbSqlWatcherOptions()
            {
                TriggersFilePath = "Triggers.json",
            };

            _watcher = new FbSqlWatcher<ProcessEventsActionModel>(options);
        }

        private void OnRemoveTriggersButtonClick(object sender, EventArgs e)
        {
            try
            {
                _watcher.EnsureAllTriggersRemoved();
                ((Button)(sender)).BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                ((Button)(sender)).BackColor = Color.Red;
                ShowError(ex);
            }
        }

        private void OnRandomInsertButtonClick(object sender, EventArgs e)
        {
            try
            {
                _watcher.InsertRandomToProcessEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void OnCreateSchemeButtonClick(object sender, EventArgs e)
        {
            var scheme = new SqlTriggerScheme<ProcessEventsActionModel>
            {
                TriggerType = TriggerType.Insert,
                EventName = "PROCESS_EVENT_INSERT",
                TableName = "PROCESS_EVENTS",
                ExternalDataSource = @"C:\DataBases\ACTIONSDB.FDB",
                ExternalPassword = "masterkey",
                ExternalUser = "SYSDBA",
                TriggerName = "PE_RIGGER_INSERT",
                ExternalTableName = "PROCESS_EVENTS_ACTIONS",
            };

            var sql = SqlTriggerBuilder.BuildSqlTrigger(scheme);
            textBox1.Text = sql;
        }


        private IEnumerable<SqlTriggerScheme<ProcessEventsActionModel>> GetProcessEventsSchemes()
        {
            var schemes = new List<SqlTriggerScheme<ProcessEventsActionModel>>();

            var insert = new SqlTriggerScheme<ProcessEventsActionModel>
            {
                TriggerType = TriggerType.Insert,
                EventName = "PROCESS_EVENT_INSERT",
                TableName = "PROCESS_EVENTS",
                ExternalDataSource = @"C:\DataBases\ACTIONSDB.FDB",
                ExternalPassword = "masterkey",
                ExternalUser = "SYSDBA",
                TriggerName = "PE_RIGGER_INSERT",
                ExternalTableName = "PROCESS_EVENTS_ACTIONS",
            };

            var delete = new SqlTriggerScheme<ProcessEventsActionModel>
            {
                TriggerType = TriggerType.Delete,
                EventName = "PROCESS_EVENT_DELETE",
                TableName = "PROCESS_EVENTS",
                ExternalDataSource = @"C:\DataBases\ACTIONSDB.FDB",
                ExternalPassword = "masterkey",
                ExternalUser = "SYSDBA",
                TriggerName = "PE_RIGGER_DELETE",
                ExternalTableName = "PROCESS_EVENTS_ACTIONS",
            };

            var update = new SqlTriggerScheme<ProcessEventsActionModel>
            {
                TriggerType = TriggerType.Update,
                EventName = "PROCESS_EVENT_UPDATE",
                TableName = "PROCESS_EVENTS",
                ExternalDataSource = @"C:\DataBases\ACTIONSDB.FDB",
                ExternalPassword = "masterkey",
                ExternalUser = "SYSDBA",
                TriggerName = "PE_RIGGER_UPDATE",
                ExternalTableName = "PROCESS_EVENTS_ACTIONS",
            };

            schemes.Add(insert);
            schemes.Add(delete);
            schemes.Add(update);
            return schemes;
        }
    }
}
