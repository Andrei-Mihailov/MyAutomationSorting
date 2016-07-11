using AutomationSorting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class ScannedBarcodeStatus
    {
        public string Barcode { get; set; }

        public bool IsValidGTIN { get; set; }
        public UPCDataModel UPC { get; set; }


        public bool IsValidSLP { get; set; }
        public SLPDataModel SLP { get; set; }


    }
}
