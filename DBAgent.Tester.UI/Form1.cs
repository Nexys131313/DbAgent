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
using DbAgent.Watcher.Models;
using DBAgent.Watcher.Models;
using DbAgent.Watcher.Scheme;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbAgent.Tester.UI
{
    public partial class Form1 : Form
    {
        private FbSqlWatcher<EmployeeActionModel> _watcher;
        private ISchemeFactory _schemeFactory;

        public Form1()
        {
            InitializeComponent();

            _schemeFactory = new SchemeFactory(@"C:\DataBases\ACTIONSDB.FDB",
                "SYSDBA", "masterkey");
        }

        private void OnAddSchemesButtonClick(object sender, EventArgs e)
        {
            try
            {

                //var options = new FbSqlWatcherOptions()
                //{
                //    TriggersFilePath = "Triggers.json",
                //};

                //_watcher = new FbSqlWatcher<EmployeeActionModel>(options);

                var schemes = GetSchemes<EmployeeActionModel>();
                foreach (var scheme in schemes)
                    _watcher.AddTrigger(scheme);

                _watcher.StartListening();
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
            var a = args.TotalChangedModels;
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
        }


        private IEnumerable<SqlTriggerScheme<TModel>> GetSchemes<TModel>()
            where TModel: IModel, new()
        {
            return _schemeFactory.CreateSchemes<TModel>(TriggerType.Insert,
                TriggerType.Delete, TriggerType.Update);
        }
    }
}
