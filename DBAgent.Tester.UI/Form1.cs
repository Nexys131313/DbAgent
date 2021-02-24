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
        private FbSqlWatcher<EmployeeActionModel> _watcher;

        public Form1()
        {
            InitializeComponent();
        }

        private void OnAddSchemesButtonClick(object sender, EventArgs e)
        {
            try
            {

                var options = new FbSqlWatcherOptions()
                {
                    TriggersFilePath = "Triggers.json",
                };

                _watcher = new FbSqlWatcher<EmployeeActionModel>(options);

                var schemes = GetSchemes<EmployeeActionModel>();
                foreach (var scheme in schemes)
                    _watcher.AddTrigger(scheme);

                _watcher.InitializeListeners();
                _watcher.TableChanged += _watcher_TableChanged;

                ((Button)(sender)).BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                ((Button)(sender)).BackColor = Color.Red;
                ShowError(ex);
            }
        }

        private void _watcher_TableChanged(object sender,
            Watcher.Events.Args.TableChangedEventArgs<EmployeeActionModel> args)
        {
            var a = args.ChangedModels;
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
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
            var scheme = new SqlTriggerScheme<ProcessEventsActionModel>(TriggerType.Insert)
            {
                ExternalDataSource = @"C:\DataBases\ACTIONSDB.FDB",
                ExternalPassword = "masterkey",
                ExternalUser = "SYSDBA",
                ExternalTableName = "PROCESS_EVENTS_ACTIONS",
            };

            var sql = SqlTriggerBuilder.BuildSqlTrigger(scheme);
            textBox1.Text = sql;
        }


        private IEnumerable<SqlTriggerScheme<TModel>> GetSchemes<TModel>()
            where TModel: IModel, new()
        {
            var schemes = SqlTriggerScheme<TModel>.
                InitializeSchemes(@"C:\DataBases\ACTIONSDB.FDB",
                    "SYSDBA", "masterkey", new[]
                    {
                        TriggerType.Insert,
                        TriggerType.Delete,
                        TriggerType.Update
                    });


            return schemes;
        }
    }
}
