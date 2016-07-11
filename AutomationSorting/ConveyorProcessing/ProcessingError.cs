using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class ProcessingError
    {
        public ProcessingErrorCodeEnum Code { get; set; }
        public string Message { get; set; }
    }
}
