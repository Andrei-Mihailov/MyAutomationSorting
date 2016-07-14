using AutomationSorting.ConveyorProcessing;
using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;

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
        static HardwareController hv = new HardwareController();
        static object locker = new object();
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
                _conveyor = new Conveyor();
                
                while (true)
                {
                    Thread reqContr = new Thread(TransmitMassageToController);
                    reqContr.Name = "reqContr";
                    reqContr.IsBackground = true;
                    reqContr.Start();
                }
            }
        }
        static void TransmitMassageToController()
        {
            lock (locker)
            {
                Thread.Sleep(100);//100 ms
                hv.serialPort.Write("ProductList");//запрос контроллеру
                while (hv.queue.Count != 0)
                {
                    hv.serialPort.Write(hv.queue.Dequeue().ToString());//передача очереди команд
                }
            }
        }
    }
}
