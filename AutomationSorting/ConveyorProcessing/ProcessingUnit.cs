using AutomationSorting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class ProcessingUnit
    {
        public long StartProcessingTime { get; set; }
        public int Index { get; set; }

        public List<ScannedBarcodeStatus> Scanned { get; set; }
        public List<ProcessingError> Errors { get; set; }

        public Task ProcessingTask { get; set; }

        public ProcessingUnit()
        {
            Scanned = new List<ConveyorProcessing.ScannedBarcodeStatus>();
            Errors = new List<ConveyorProcessing.ProcessingError>();
        }

        public bool HasError
        {
            get
            {
                return Errors.Any();
            }
        }

        public ProductDataModel Product { get; set; }
        public string SortingIndex { get; set; }
    }
}
