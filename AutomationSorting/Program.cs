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
        static byte[] reqFlag_ProductExist = { 11, 03, 00, 00, 00, 01 };
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
                    if (hv.Mode == 0)
                    {
                        if (hv.Flag_ProductExist)//появился товар
                        {
                            hv.Mode = 1;
                            hv.Flag_ProductExist = false;
                            hv.thread_stop = false;
                        }
                        else
                        {
                            hv.serialPort_Controller.Write(reqFlag_ProductExist, 0, 6);//опрос контроллера на наличие товара
                        }
                    }
                    else if (hv.Mode == 1)
                    {
                        Thread reqContr = new Thread(TransmitMassageToController);
                        reqContr.Name = "reqContr";
                        reqContr.IsBackground = true;
                        reqContr.Start();
                        reqContr.Join();
                    }
                }
            }
        }
        static void TransmitMassageToController()
        {
            lock (locker)
            {
                while (!hv.thread_stop)
                {
                    Thread.Sleep(100);//100 ms
                    hv.serialPort_Controller.Write(reqFlag_ProductExist, 0, 6);//запрос контроллеру
                    while (hv.queue.Count != 0)
                    {
                        hv.serialPort_Controller.Write(hv.queue.Dequeue(), 0, 6);//передача очереди команд
                    }
                }
            }
        }
    }
}
