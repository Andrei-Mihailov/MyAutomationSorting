using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.Api
{
    [ServiceContract]
    public interface IAutomationSortingAPI
    {
        [OperationContract]
        void ResetUPCIndexes();

    }
}
