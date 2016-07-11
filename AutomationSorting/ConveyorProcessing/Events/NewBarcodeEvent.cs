using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing.Events
{
    public class NewBarcodeEventArgs : EventArgs
    {
        public string Barcode { get; set; }
    }
}
