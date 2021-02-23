using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBAgent.Watcher;
using DBAgent.Watcher.Enums;

namespace DbAgent.Tester.UI
{
    public partial class Form1 : Form
    {
        private FbSqlWatcher _watcher;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _watcher.EnsureAllTriggersExists();
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
                Tables = new List<TableType>() { TableType.ProcessEvents },
            };

            _watcher = new FbSqlWatcher(options);
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
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
    }
}
