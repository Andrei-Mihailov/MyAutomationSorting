using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public enum ProcessingErrorCodeEnum
    {
        MultipleUPC,
        MultipleSLP,
        NoUPC,
        DuplicateSLP,
        PrintingError,
        SortedToErroneous,
        SortedToUnsorted
    }
}
