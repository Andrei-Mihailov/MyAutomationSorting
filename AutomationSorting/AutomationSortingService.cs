using AutomationSorting.ConveyorProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting
{
    public partial class AutomationSortingService : ServiceBase
    {
        public Conveyor _conveyor = null;
        public AutomationSortingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, errors) => true;
        }

        protected override void OnStop()
        {
        }
    }
}
