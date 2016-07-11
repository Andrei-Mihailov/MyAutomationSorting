using AutomationSorting.ConveyorProcessing.Events;
using AutomationSorting.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class Conveyor
    {
        ScanProcessor _scanProcessor = new ScanProcessor();
        PrintProcessor _printProcessor = new PrintProcessor();
        CheckinProcessor _checkinProcessor = new CheckinProcessor();
        SortingProcessor _sortingProcessor = new SortingProcessor();
        LoggingProcessor _loggingProcessor = new LoggingProcessor();

        HardwareController _hardwareController;

        Dictionary<int, ProcessingUnit> _units = new Dictionary<int, ProcessingUnit>();

        public Conveyor()
        {
            _hardwareController = HardwareController.GetInstance();
            _hardwareController.NewUnitEvent += _hardwareController_NewUnitEvent;
        }

        private async void _hardwareController_NewUnitEvent(object sender, NewUnitEventArgs e)
        {
            ProcessingUnit unit = new AutomationSorting.ConveyorProcessing.ProcessingUnit();
            unit.Index = e.Index;
            unit.StartProcessingTime = e.ProcessingStartedTime;
            _units.Add(unit.Index, unit);

            var task = ProcessAsync(unit);
            unit.ProcessingTask = task;

            await task;

            _units.Remove(unit.Index);
        }

        public async Task ProcessAsync(ProcessingUnit unit)
        {
            await Task.Factory.StartNew(() => { Process(unit); });
        }

        public void Process(ProcessingUnit unit)
        {
            _scanProcessor.Start(unit);
            Thread.Sleep(ConveyorHelper.ScanDelay);
            _scanProcessor.Complete();

            _checkinProcessor.Process(unit);
            _printProcessor.Process(unit);
            _sortingProcessor.Process(unit);
            _loggingProcessor.Process(unit);


        }
    }
}
