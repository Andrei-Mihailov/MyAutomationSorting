using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class SortingProcessor
    {
        private HardwareController _hardwareController = HardwareController.GetInstance();
        public void Process(ProcessingUnit unit)
        {
            if (unit.HasError)
            {
                unit.Errors.Add(new ConveyorProcessing.ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.SortedToErroneous,
                    Message = "Unit has errors and cannot be sorted"
                });

                _hardwareController.ScheduleSortingErrorProcessing(unit.Errors[0].Code, unit.Index, unit.StartProcessingTime);

                return;
            }

            if (string.IsNullOrWhiteSpace(unit.SortingIndex))
            {
                unit.Errors.Add(new ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.SortedToUnsorted,
                    Message = "Product was processed as unsorted item"
                });

                _hardwareController.ScheduleUnsortedProcessing(unit.Index, unit.StartProcessingTime);

                return;
            }

            _hardwareController.ScheduleSorting(unit.SortingIndex, unit.Index, unit.StartProcessingTime);
        }
    }
}
