using AutomationSorting.ConveyorProcessing.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class HardwareController
    {
        public event EventHandler<NewUnitEventArgs> NewUnitEvent;
        public event EventHandler<NewBarcodeEventArgs> NewBarcodeEvent;
        public event EventHandler<SortingCompletedEventArgs> SortingCompletedEvent;

        private static HardwareController _hardwareController = null;
        private HardwareController()
        {

        }

        public static HardwareController GetInstance()
        {
            if (_hardwareController == null)
                _hardwareController = new HardwareController();

            return _hardwareController;
        }

        public void PreparePrinterLabel(string EPL, int index, long startProcessingTime)
        {
        }

        public void SchedulePrintingTask(int index, long startProcessingTime)
        {

        }
        
        public void InitializeSortingIndexes(int index, string [] sortingIndexes)
        {

        }

        public void ScheduleSortingErrorProcessing(ProcessingErrorCodeEnum error, int index, long startProcessingTime)
        {

        }

        public void ScheduleSorting(string sortingIndex, int index, long startProcessingTime)
        {

        }

        public void ScheduleUnsortedProcessing(int index, long startProcessingTime)
        {

        }

    }
}
