using AutomationSorting.ConveyorProcessing.Events;
using AutomationSorting.Data;
using AutomationSorting.Data.Abstract;
using AutomationSorting.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class ScanProcessor
    {
        private Queue<ScannedBarcodeStatus> _scanned = new Queue<ScannedBarcodeStatus>();
        private List<ScannedBarcodeStatus> _processed = new List<ScannedBarcodeStatus>();
        private ScanProcessorStateEnum _state = ScanProcessorStateEnum.Stopped;

        private IUpcRepository _upcRepository;
        private ISlpRepository _slpRepository;
        private HardwareController _hardwareController;

        private ProcessingUnit _unit = null;

        public ScanProcessor()
        {
            _upcRepository = RepositoryFactory.GetUpcRepository();
            _slpRepository = RepositoryFactory.GetSlpRepository();

            _hardwareController = HardwareController.GetInstance();
            _hardwareController.NewBarcodeEvent += NewBarcodeEventHandler;
        }

        private void NewBarcodeEventHandler(object sender, NewBarcodeEventArgs e)
        {
            AddBarcode(e.Barcode);
        }

        public void Start(ProcessingUnit unit)
        {
            if (_state == ScanProcessorStateEnum.Running)
                throw new Exception("Critical error! Scan processor is already running and did not finish previous item processing");

            _scanned.Clear();
            _processed.Clear();
            _state = ScanProcessorStateEnum.Running;
            _unit = unit;
        }

        public void AddBarcode(string barcode)
        {
            ScannedBarcodeStatus status = new ScannedBarcodeStatus()
            {
                Barcode = barcode
            };

            _scanned.Enqueue(status);
        }

        private void Process(ScannedBarcodeStatus status)
        {
            status.IsValidGTIN = UPCHelper.IsValidGtin(status.Barcode);

            if(status.IsValidGTIN)
            {
                status.UPC = _upcRepository.GetUPC(status.Barcode);
            }

            status.IsValidSLP = SLPHelper.IsValidSLP(status.Barcode);

            if(status.IsValidSLP)
            {
                status.SLP = _slpRepository.GetSLP(status.Barcode);
            }

            _processed.Add(status);
        }

        public void Complete()
        {
            if (_scanned.Any())
                throw new Exception("Critical error! Not all scanned barcodes were processed");

            if (_state != ScanProcessorStateEnum.Running)
                throw new Exception("Critical error! Processing was not initialized");
            
            _unit.Scanned.AddRange(_processed);
            _processed.Clear();
            _state = ScanProcessorStateEnum.Stopped;
            _unit = null;
            
        }

    }

    public enum ScanProcessorStateEnum
    {
        Stopped,
        Running
    }
}
