using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBAgent
{
    public partial class DBAgent : ServiceBase
    {
        public DBAgent()
        {
            InitializeComponent();

            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;

        }

        protected override void OnStart(string[] args)
        {
            SetRecoveryOptions("DBAgent");
            Console.WriteLine("Started");

        }

        protected override void OnStop()
        {
            Thread.Sleep(3000);
        }

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;

            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/20000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException();
        }

    }
}
