using AutomationSorting.ConveyorProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

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
            string str = "ProductList";
            Thread.Sleep(100);//100 ms
            serialPort.Write(str);
        }

    }
    
    static class RequestToController//: HardwareController
    {
       // SerialPort serialPort = null;
       
        static RequestToController()
        {
            SerialPort serialPort = null;
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }
    }
}
