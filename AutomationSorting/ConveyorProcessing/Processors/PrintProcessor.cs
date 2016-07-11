using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class PrintProcessor
    {
        private HardwareController _hardwareController = HardwareController.GetInstance();
        public void Process(ProcessingUnit unit)
        {
            if (unit.HasError)
            {
                unit.Errors.Add(new ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.PrintingError,
                    Message = "Label could not be printed because unit has processing errors"
                });
                return;
            }

            var ZPL = GetProductZPL(unit);
            _hardwareController.PreparePrinterLabel(ZPL, unit.Index, unit.StartProcessingTime);
            _hardwareController.SchedulePrintingTask(unit.Index, unit.StartProcessingTime);
        }

        private string GetProductZPL(ProcessingUnit unit)
        {
            int numberOfLabels = 1;
            string productEPL = "! 0 200 200 230 " + numberOfLabels + "\r\n" +
                    "CENTER\r\n";
            //"TEXT 4 0 0 0 FULL FIRST LINE (LARGE)\r\n" + 

            if (!string.IsNullOrWhiteSpace(unit.SortingIndex))
            {
                productEPL += "TEXT 7 0 0 0 " + unit.SortingIndex + "\r\n";
            }
            else
            {
                productEPL += "TEXT 7 0 0 0 " + "" + "\r\n";
            }

            productEPL +=
                    "BARCODE 128 1 1 85 0 35 TRG-" + unit.Product.CompanyProductID + "\r\n" +
                    "TEXT 4 0 0 120 " + unit.Product.CompanyProductID + "\r\n";


            productEPL += "FORM\r\n" +
                "PRINT\r\n";

            return productEPL;
        }
    }
}
