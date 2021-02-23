using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DBAgent
{
    [RunInstaller(true)]
    public partial class InstallerService : System.Configuration.Install.Installer
    {
        ServiceInstaller dbagentServiceInstaller;
        ServiceProcessInstaller processInstaller;

        public InstallerService()
        {
            #if DEBUG
            //System.Diagnostics.Debugger.Launch();
            #endif

            InitializeComponent();

            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem; //ServiceAccount.LocalSystem;

            dbagentServiceInstaller = new ServiceInstaller();

            dbagentServiceInstaller.StartType = ServiceStartMode.Automatic; //ServiceStartMode.Manual;
            dbagentServiceInstaller.ServiceName = "DBAgent";


            Installers.Add(processInstaller);
            Installers.Add(dbagentServiceInstaller);

        }
    }
}
