using AutomationSorting.ConveyorProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationSorting
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        const int STD_OUTPUT_HANDLE = -11;
        private static Conveyor _conveyor = null;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            bool runAsWindowsService = false;
            IntPtr iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (iStdOut == IntPtr.Zero)
                runAsWindowsService = true;

            if (runAsWindowsService)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new AutomationSortingService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                _conveyor = new ConveyorProcessing.Conveyor();

                while(true)
                Thread.Sleep(1000);
            }
        }
    }
}
